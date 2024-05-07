
using api.Adapters;
using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class SubstructureService : ISubstructureService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<SubstructureService> _logger;
    private readonly IMapper _mapper;
    private readonly SubstructureRepository _repository;
    private readonly ICaseRepository _caseRepository;

    public SubstructureService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IMapper mapper,
        SubstructureRepository substructureRepository,
        ICaseRepository caseRepository
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<SubstructureService>();
        _mapper = mapper;
        _repository = substructureRepository;
        _caseRepository = caseRepository;
    }

    public async Task<Substructure> CreateSubstructure(Guid projectId, Guid sourceCaseId, CreateSubstructureDto substructureDto)
    {
        var substructure = _mapper.Map<Substructure>(substructureDto);
        if (substructure == null)
        {
            throw new ArgumentNullException(nameof(substructure));
        }
        var project = await _projectService.GetProject(projectId);
        substructure.Project = project;
        substructure.LastChangedDate = DateTimeOffset.UtcNow;
        var createdSubstructure = _context.Substructures!.Add(substructure);
        await _context.SaveChangesAsync();
        await SetCaseLink(substructure, sourceCaseId, project);
        return createdSubstructure.Entity;
    }

    public async Task<SubstructureDto> CopySubstructure(Guid substructureId, Guid sourceCaseId)
    {
        var source = await GetSubstructure(substructureId);
        var newSubstructureDto = _mapper.Map<SubstructureDto>(source);
        if (newSubstructureDto == null)
        {
            throw new ArgumentNullException(nameof(newSubstructureDto));
        }
        newSubstructureDto.Id = Guid.Empty;
        if (newSubstructureDto.CostProfile != null)
        {
            newSubstructureDto.CostProfile.Id = Guid.Empty;
        }
        if (newSubstructureDto.CostProfileOverride != null)
        {
            newSubstructureDto.CostProfileOverride.Id = Guid.Empty;
        }
        if (newSubstructureDto.CessationCostProfile != null)
        {
            newSubstructureDto.CessationCostProfile.Id = Guid.Empty;
        }

        // var topside = await NewCreateSubstructure(newSubstructureDto, sourceCaseId);
        // var dto = SubstructureDtoAdapter.Convert(topside);

        // return dto;
        return newSubstructureDto;
    }

    private async Task SetCaseLink(Substructure substructure, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }
        case_.SubstructureLink = substructure.Id;
        await _context.SaveChangesAsync();
    }

    public async Task<SubstructureDto> UpdateSubstructureAndCostProfiles<TDto>(TDto updatedSubstructureDto, Guid substructureId)
        where TDto : BaseUpdateSubstructureDto
    {
        var existing = await GetSubstructure(substructureId);

        _mapper.Map(updatedSubstructureDto, existing);

        existing.LastChangedDate = DateTimeOffset.UtcNow;
        _context.Substructures!.Update(existing);
        await _context.SaveChangesAsync();
        var dto = _mapper.Map<SubstructureDto>(existing);
        return dto ?? throw new ArgumentNullException(nameof(dto));
    }

    public async Task<Substructure> GetSubstructure(Guid substructureId)
    {
        var substructure = await _context.Substructures!
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .FirstOrDefaultAsync(o => o.Id == substructureId);
        if (substructure == null)
        {
            throw new ArgumentException(string.Format("Substructure {0} not found.", substructureId));
        }
        return substructure;
    }

    public async Task<SubstructureDto> UpdateSubstructure<TDto>(Guid caseId, Guid substructureId, TDto updatedSubstructureDto)
        where TDto : BaseUpdateSubstructureDto
    {
        var existingSubstructure = await _repository.GetSubstructure(substructureId)
            ?? throw new NotFoundInDBException($"Substructure with id {substructureId} not found.");

        _mapper.Map(updatedSubstructureDto, existingSubstructure);
        existingSubstructure.LastChangedDate = DateTimeOffset.UtcNow;

        Substructure updatedSubstructure;
        try
        {
            updatedSubstructure = await _repository.UpdateSubstructure(existingSubstructure);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update substructure with id {SubstructureId} for case id {CaseId}.", substructureId, caseId);
            throw;
        }

        await _caseRepository.UpdateModifyTime(caseId);

        var dto = MapToDto(updatedSubstructure, substructureId);

        return dto;
    }

    private SubstructureDto MapToDto(Substructure updatedSubstructure, Guid substructureId)
    {
        var dto = _mapper.Map<SubstructureDto>(updatedSubstructure);
        if (dto == null)
        {
            _logger.LogError("Mapping of substructure with id {SubstructureId} resulted in a null DTO.", substructureId);
            throw new MappingException("Mapping resulted in a null DTO.", substructureId);
        }
        return dto;
    }
}
