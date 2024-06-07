using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class WellProjectService : IWellProjectService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ILogger<WellProjectService> _logger;
    private readonly IMapper _mapper;
    private readonly IWellProjectRepository _repository;
    private readonly ICaseRepository _caseRepository;
    private readonly IMapperService _mapperService;

    public WellProjectService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IMapper mapper,
        IWellProjectRepository repository,
        ICaseRepository caseRepository,
        IMapperService mapperService
        )
    {
        _context = context;
        _projectService = projectService;
        _logger = loggerFactory.CreateLogger<WellProjectService>();
        _mapper = mapper;
        _repository = repository;
        _caseRepository = caseRepository;
        _mapperService = mapperService;
    }

    public async Task<WellProjectWithProfilesDto> CopyWellProject(Guid wellProjectId, Guid sourceCaseId)
    {
        var source = await GetWellProject(wellProjectId);
        var newWellProjectDto = _mapper.Map<WellProjectWithProfilesDto>(source);
        if (newWellProjectDto == null)
        {
            _logger.LogError("Failed to map well project to dto");
            throw new Exception("Failed to map well project to dto");
        }
        newWellProjectDto.Id = Guid.Empty;

        if (newWellProjectDto.OilProducerCostProfile != null)
        {
            newWellProjectDto.OilProducerCostProfile.Id = Guid.Empty;
        }
        if (newWellProjectDto.OilProducerCostProfileOverride != null)
        {
            newWellProjectDto.OilProducerCostProfileOverride.Id = Guid.Empty;
        }

        if (newWellProjectDto.GasProducerCostProfile != null)
        {
            newWellProjectDto.GasProducerCostProfile.Id = Guid.Empty;
        }
        if (newWellProjectDto.GasProducerCostProfileOverride != null)
        {
            newWellProjectDto.GasProducerCostProfileOverride.Id = Guid.Empty;
        }

        if (newWellProjectDto.WaterInjectorCostProfile != null)
        {
            newWellProjectDto.WaterInjectorCostProfile.Id = Guid.Empty;
        }
        if (newWellProjectDto.WaterInjectorCostProfileOverride != null)
        {
            newWellProjectDto.WaterInjectorCostProfileOverride.Id = Guid.Empty;
        }

        if (newWellProjectDto.GasInjectorCostProfile != null)
        {
            newWellProjectDto.GasInjectorCostProfile.Id = Guid.Empty;
        }
        if (newWellProjectDto.GasInjectorCostProfileOverride != null)
        {
            newWellProjectDto.GasInjectorCostProfileOverride.Id = Guid.Empty;
        }

        // var wellProject = await NewCreateWellProject(newWellProjectDto, sourceCaseId);
        // var dto = WellProjectDtoAdapter.Convert(wellProject);
        // return dto;
        return newWellProjectDto;
    }

    public async Task<WellProject> CreateWellProject(Guid projectId, Guid sourceCaseId, CreateWellProjectDto wellProjectDto)
    {
        var wellProject = _mapper.Map<WellProject>(wellProjectDto);
        if (wellProject == null)
        {
            throw new ArgumentNullException(nameof(wellProject));
        }
        var project = await _projectService.GetProject(projectId);
        wellProject.Project = project;
        var createdWellProject = _context.WellProjects!.Add(wellProject);
        await _context.SaveChangesAsync();
        await SetCaseLink(wellProject, sourceCaseId, project);
        return createdWellProject.Entity;
    }

    private async Task SetCaseLink(WellProject wellProject, Guid sourceCaseId, Project project)
    {
        var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
        if (case_ == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
        }
        case_.WellProjectLink = wellProject.Id;
        await _context.SaveChangesAsync();
    }

    public async Task<WellProjectWithProfilesDto> UpdateWellProjectAndCostProfiles(WellProjectWithProfilesDto updatedWellProjectDto)
    {
        var existing = await GetWellProject(updatedWellProjectDto.Id);
        _mapper.Map(updatedWellProjectDto, existing);

        var updatedWellProject = _context.WellProjects!.Update(existing);
        await _context.SaveChangesAsync();
        var dto = _mapper.Map<WellProjectWithProfilesDto>(updatedWellProject);
        if (dto == null)
        {
            _logger.LogError("Failed to map well project to dto");
            throw new Exception("Failed to map well project to dto");
        }
        return dto;
    }

    public async Task<WellProject> GetWellProject(Guid wellProjectId)
    {
        var wellProject = await _context.WellProjects!
            .Include(c => c.OilProducerCostProfile)
            .Include(c => c.OilProducerCostProfileOverride)
            .Include(c => c.GasProducerCostProfile)
            .Include(c => c.GasProducerCostProfileOverride)
            .Include(c => c.WaterInjectorCostProfile)
            .Include(c => c.WaterInjectorCostProfileOverride)
            .Include(c => c.GasInjectorCostProfile)
            .Include(c => c.GasInjectorCostProfileOverride)
            .FirstOrDefaultAsync(o => o.Id == wellProjectId);
        if (wellProject == null)
        {
            throw new ArgumentException(string.Format("Well project {0} not found.", wellProjectId));
        }
        return wellProject;
    }

    public async Task<WellProjectDto> UpdateWellProject(
        Guid caseId,
        Guid wellProjectId,
        UpdateWellProjectDto updatedWellProjectDto
    )
    {
        var existingWellProject = await _repository.GetWellProject(wellProjectId)
            ?? throw new NotFoundInDBException($"Well project with id {wellProjectId} not found.");

        _mapperService.MapToEntity(updatedWellProjectDto, existingWellProject, wellProjectId);

        WellProject updatedWellProject;
        try
        {
            updatedWellProject = _repository.UpdateWellProject(existingWellProject);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update well project with id {wellProjectId} for case id {caseId}.", wellProjectId, caseId);
            throw;
        }

        var dto = _mapperService.MapToDto<WellProject, WellProjectDto>(updatedWellProject, wellProjectId);
        return dto;
    }

    public async Task<WellProjectWellDto> UpdateWellProjectWell(
        Guid caseId,
        Guid wellProjectId,
        Guid wellId,
        UpdateWellProjectWellDto updatedWellProjectWellDto
    )
    {
        var existingWellProject = await _repository.GetWellProjectWell(wellProjectId, wellId)
            ?? throw new NotFoundInDBException($"Well project well with id {wellProjectId} and ${wellId} not found.");

        _mapperService.MapToEntity(updatedWellProjectWellDto, existingWellProject, wellProjectId);

        WellProjectWell updatedWellProject;
        try
        {
            updatedWellProject = _repository.UpdateWellProjectWell(existingWellProject);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update well project well with id {wellProjectId} and well id {wellId}.", wellProjectId, wellId);
            throw;
        }

        var dto = _mapperService.MapToDto<WellProjectWell, WellProjectWellDto>(updatedWellProject, wellProjectId);
        return dto;
    }

    public async Task<OilProducerCostProfileOverrideDto> UpdateOilProducerCostProfileOverride(
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateOilProducerCostProfileOverrideDto updateDto
    )
    {
        return await UpdateWellProjectCostProfile<OilProducerCostProfileOverride, OilProducerCostProfileOverrideDto, UpdateOilProducerCostProfileOverrideDto>(
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            _repository.GetOilProducerCostProfileOverride,
            _repository.UpdateOilProducerCostProfileOverride
        );
    }

    public async Task<GasProducerCostProfileOverrideDto> UpdateGasProducerCostProfileOverride(
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateGasProducerCostProfileOverrideDto updateDto
    )
    {
        return await UpdateWellProjectCostProfile<GasProducerCostProfileOverride, GasProducerCostProfileOverrideDto, UpdateGasProducerCostProfileOverrideDto>(
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            _repository.GetGasProducerCostProfileOverride,
            _repository.UpdateGasProducerCostProfileOverride
        );
    }

    public async Task<WaterInjectorCostProfileOverrideDto> UpdateWaterInjectorCostProfileOverride(
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateWaterInjectorCostProfileOverrideDto updateDto
    )
    {
        return await UpdateWellProjectCostProfile<WaterInjectorCostProfileOverride, WaterInjectorCostProfileOverrideDto, UpdateWaterInjectorCostProfileOverrideDto>(
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            _repository.GetWaterInjectorCostProfileOverride,
            _repository.UpdateWaterInjectorCostProfileOverride
        );
    }

    public async Task<GasInjectorCostProfileOverrideDto> UpdateGasInjectorCostProfileOverride(
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        UpdateGasInjectorCostProfileOverrideDto updateDto
    )
    {
        return await UpdateWellProjectCostProfile<GasInjectorCostProfileOverride, GasInjectorCostProfileOverrideDto, UpdateGasInjectorCostProfileOverrideDto>(
            caseId,
            wellProjectId,
            profileId,
            updateDto,
            _repository.GetGasInjectorCostProfileOverride,
            _repository.UpdateGasInjectorCostProfileOverride
        );
    }

    private async Task<TDto> UpdateWellProjectCostProfile<TProfile, TDto, TUpdateDto>(
        Guid caseId,
        Guid wellProjectId,
        Guid profileId,
        TUpdateDto updatedProfileDto,
        Func<Guid, Task<TProfile?>> getProfile,
        Func<TProfile, TProfile> updateProfile
    )
        where TProfile : class, IWellProjectTimeSeries
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(profileId)
            ?? throw new NotFoundInDBException($"Cost profile with id {profileId} not found.");

        _mapperService.MapToEntity(updatedProfileDto, existingProfile, wellProjectId);

        TProfile updatedProfile;
        try
        {
            updatedProfile = updateProfile(existingProfile);
            await _caseRepository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            var profileName = typeof(TProfile).Name;
            _logger.LogError(ex, "Failed to update profile {profileName} with id {profileId} for case id {caseId}.", profileName, profileId, caseId);
            throw;
        }

        var updatedDto = _mapperService.MapToDto<TProfile, TDto>(updatedProfile, profileId);
        return updatedDto;
    }
}
