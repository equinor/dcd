using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class SurfService : ISurfService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<SurfService> _logger;
    private readonly IMapper _mapper;
    private readonly ISurfRepository _repository;
    private readonly ICaseRepository _caseRepository;
    private readonly IMapperService _mapperService;
    public SurfService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IMapper mapper,
        ISurfRepository repository,
        ICaseRepository caseRepository,
        IMapperService mapperService
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<SurfService>();
        _mapper = mapper;
        _repository = repository;
        _caseRepository = caseRepository;
        _mapperService = mapperService;
    }

    public async Task<SurfWithProfilesDto> CopySurf(Guid surfId, Guid sourceCaseId)
    {
        var source = await GetSurf(surfId);
        var newSurfDto = _mapper.Map<SurfWithProfilesDto>(source);
        if (newSurfDto == null)
        {
            _logger.LogError("Failed to map surf to dto");
            throw new Exception("Failed to map surf to dto");
        }
        newSurfDto.Id = Guid.Empty;
        if (newSurfDto.CostProfile != null)
        {
            newSurfDto.CostProfile.Id = Guid.Empty;
        }
        if (newSurfDto.CostProfileOverride != null)
        {
            newSurfDto.CostProfileOverride.Id = Guid.Empty;
        }
        if (newSurfDto.CessationCostProfile != null)
        {
            newSurfDto.CessationCostProfile.Id = Guid.Empty;
        }

        // var surf = await NewCreateSurf(newSurfDto, sourceCaseId);
        // var dto = SurfDtoAdapter.Convert(surf);

        // return dto;
        return newSurfDto;
    }

    public async Task<ProjectDto> UpdateSurfAndCostProfiles<TDto>(TDto updatedSurfDto, Guid surfId)
        where TDto : BaseUpdateSurfDto
    {
        var existing = await GetSurf(surfId);
        _mapper.Map(updatedSurfDto, existing);

        existing.LastChangedDate = DateTimeOffset.UtcNow;
        _context.Surfs!.Update(existing);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDto(existing.ProjectId);
    }

    public async Task<Surf> GetSurf(Guid surfId)
    {
        var surf = await _context.Surfs!
            .Include(c => c.CostProfile)
            .Include(c => c.CostProfileOverride)
            .Include(c => c.CessationCostProfile)
            .FirstOrDefaultAsync(o => o.Id == surfId);
        if (surf == null)
        {
            throw new ArgumentException(string.Format("Surf {0} not found.", surfId));
        }
        return surf;
    }

    public async Task<Surf> CreateSurf(Guid projectId, Guid sourceCaseId, CreateSurfDto surfDto)
    {
        var surf = _mapper.Map<Surf>(surfDto);
        if (surf == null)
        {
            throw new ArgumentNullException(nameof(surf));
        }
        var project = await _projectService.GetProject(projectId);
        surf.Project = project;
        surf.LastChangedDate = DateTimeOffset.UtcNow;
        var createdSurf = _context.Surfs!.Add(surf);
        await _context.SaveChangesAsync();
        await SetCaseLink(surf, sourceCaseId, project);
        return createdSurf.Entity;
    }

    private async Task SetCaseLink(Surf surf, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }
        case_.SurfLink = surf.Id;
        await _context.SaveChangesAsync();
    }

    public async Task<SurfDto> UpdateSurf<TDto>(
        Guid caseId,
        Guid surfId,
        TDto updatedSurfDto
    )
        where TDto : BaseUpdateSurfDto
    {
        var existingSurf = await _repository.GetSurf(surfId)
            ?? throw new ArgumentException(string.Format($"Surf with id {surfId} not found."));

        _mapperService.MapToEntity(updatedSurfDto, existingSurf, surfId);
        existingSurf.LastChangedDate = DateTimeOffset.UtcNow;

        Surf updatedSurf;
        try
        {
            updatedSurf = await _repository.UpdateSurf(existingSurf);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update surf with id {surfId} for case id {caseId}.", surfId, caseId);
            throw;
        }

        await _caseRepository.UpdateModifyTime(caseId);

        var dto = _mapperService.MapToDto<Surf, SurfDto>(updatedSurf, surfId);
        return dto;
    }

    public async Task<SurfCostProfileOverrideDto> UpdateSurfCostProfileOverride(
        Guid caseId,
        Guid surfId,
        Guid costProfileId,
        UpdateSurfCostProfileOverrideDto updatedSurfCostProfileOverrideDto
    )
    {
        var existingSurfCostProfileOverride = await _repository.GetSurfCostProfileOverride(costProfileId)
            ?? throw new NotFoundInDBException($"Cost profile override with id {costProfileId} not found.");

        _mapperService.MapToEntity(updatedSurfCostProfileOverrideDto, existingSurfCostProfileOverride, costProfileId);

        SurfCostProfileOverride updatedSurfCostProfileOverride;
        try
        {
            updatedSurfCostProfileOverride = await _repository.UpdateSurfCostProfileOverride(existingSurfCostProfileOverride);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update surf cost profile override with id {costProfileId} for surf id {surfId} for case id {caseId}.", costProfileId, surfId, caseId);
            throw;
        }

        await _caseRepository.UpdateModifyTime(caseId);

        var dto = _mapperService.MapToDto<SurfCostProfileOverride, SurfCostProfileOverrideDto>(updatedSurfCostProfileOverride, costProfileId);
        return dto;
    }
}
