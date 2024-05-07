using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class TopsideService : ITopsideService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<TopsideService> _logger;
    private readonly IMapper _mapper;
    private readonly TopsideRepository _repository;
    private readonly ICaseRepository _caseRepository;
    private readonly IMapperService _mapperService;

    public TopsideService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IMapper mapper,
        TopsideRepository repository,
        ICaseRepository caseRepository,
        IMapperService mapperService
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<TopsideService>();
        _mapper = mapper;
        _repository = repository;
        _caseRepository = caseRepository;
        _mapperService = mapperService;
    }

    public async Task<TopsideDto> CopyTopside(Guid topsideId, Guid sourceCaseId)
    {
        var source = await GetTopside(topsideId);
        var newTopsideDto = _mapper.Map<TopsideDto>(source);
        if (newTopsideDto == null)
        {
            _logger.LogError("Failed to map topside to dto");
            throw new Exception("Failed to map topside to dto");
        }
        newTopsideDto.Id = Guid.Empty;
        if (newTopsideDto.CostProfile != null)
        {
            newTopsideDto.CostProfile.Id = Guid.Empty;
        }
        if (newTopsideDto.CostProfileOverride != null)
        {
            newTopsideDto.CostProfileOverride.Id = Guid.Empty;
        }
        if (newTopsideDto.CessationCostProfile != null)
        {
            newTopsideDto.CessationCostProfile.Id = Guid.Empty;
        }

        // var topside = await NewCreateTopside(newTopsideDto, sourceCaseId);
        // var dto = TopsideDtoAdapter.Convert(topside);

        // return dto;
        return newTopsideDto;
    }

    public async Task<Topside> CreateTopside(Guid projectId, Guid sourceCaseId, CreateTopsideDto topsideDto)
    {
        var topside = _mapper.Map<Topside>(topsideDto);
        if (topside == null)
        {
            throw new ArgumentNullException(nameof(topside));
        }
        var project = await _projectService.GetProject(projectId);
        topside.Project = project;
        topside.LastChangedDate = DateTimeOffset.UtcNow;
        var createdTopside = _context.Topsides!.Add(topside);
        await _context.SaveChangesAsync();
        await SetCaseLink(topside, sourceCaseId, project);
        return createdTopside.Entity;
    }

    private async Task SetCaseLink(Topside topside, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }
        case_.TopsideLink = topside.Id;
        await _context.SaveChangesAsync();
    }

    public async Task<ProjectDto> UpdateTopsideAndCostProfiles<TDto>(TDto updatedTopsideDto, Guid topsideId)
        where TDto : BaseUpdateTopsideDto
    {
        var existing = await GetTopside(topsideId);
        _mapper.Map(updatedTopsideDto, existing);

        existing.LastChangedDate = DateTimeOffset.UtcNow;
        _context.Topsides!.Update(existing);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDto(existing.ProjectId);
    }



    public async Task<Topside> GetTopside(Guid topsideId)
    {
        var topside = await _context.Topsides!
            .Include(c => c.Project)
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .FirstOrDefaultAsync(o => o.Id == topsideId);
        if (topside == null)
        {
            throw new ArgumentException(string.Format("Topside {0} not found.", topsideId));
        }
        return topside;
    }

    public async Task<TopsideDto> UpdateTopside<TDto>(
        Guid caseId,
        Guid topsideId,
        TDto updatedTopsideDto
    )
        where TDto : BaseUpdateTopsideDto
    {
        var existingTopside = await _repository.GetTopside(topsideId)
            ?? throw new NotFoundInDBException($"Topside with id {topsideId} not found.");

        _mapper.Map(updatedTopsideDto, existingTopside);
        existingTopside.LastChangedDate = DateTimeOffset.UtcNow;

        Topside updatedTopside;
        try
        {
            updatedTopside = await _repository.UpdateTopside(existingTopside);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update topside with id {topsideId} for case id {CaseId}.", topsideId, caseId);
            throw;
        }

        var dto = _mapperService.MapToDto<Topside, TopsideDto>(updatedTopside, topsideId);
        return dto;
    }

    public async Task<TopsideCostProfileOverrideDto> UpdateTopsideCostProfileOverride(
        Guid caseId, 
        Guid topsideId,
        Guid costProfileId,
        UpdateTopsideCostProfileOverrideDto dto
    )
    {
        var existingCostProfile = await _repository.GetTopsideCostProfileOverride(costProfileId)
            ?? throw new NotFoundInDBException($"Cost profile override with id {costProfileId} not found.");

        _mapper.Map(dto, existingCostProfile);

        TopsideCostProfileOverride updatedCostProfile;
        try
        {
            updatedCostProfile = await _repository.UpdateTopsideCostProfileOverride(existingCostProfile);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update cost profile override with id {CostProfileId} for topside id {TopsideId} for case id {CaseId}.", costProfileId, topsideId, caseId);
            throw;
        }

        await _caseRepository.UpdateModifyTime(caseId);

        var updatedDto = _mapperService.MapToDto<TopsideCostProfileOverride, TopsideCostProfileOverrideDto>(updatedCostProfile, costProfileId);

        return updatedDto;
    }
}
