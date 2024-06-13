using api.Context;
using api.Dtos;
using api.Enums;
using api.Exceptions;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class CaseService : ICaseService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly ITopsideService _topsideService;
    private readonly ISurfService _surfService;
    private readonly ISubstructureService _substructureService;
    private readonly ITransportService _transportService;
    private readonly IExplorationService _explorationService;
    private readonly IWellProjectService _wellProjectService;
    private readonly ILogger<CaseService> _logger;
    private readonly IMapper _mapper;
    private readonly IMapperService _mapperService;
    private readonly ICaseRepository _repository;

    public CaseService(
        DcdDbContext context,
        IProjectService projectService,
        ILoggerFactory loggerFactory,
        IDrainageStrategyService drainageStrategyService,
        ITopsideService topsideService,
        ISurfService surfService,
        ISubstructureService substructureService,
        ITransportService transportService,
        IExplorationService explorationService,
        IWellProjectService wellProjectService,
        ICaseRepository repository,
        IMapperService mapperService,
        IMapper mapper)
    {
        _context = context;
        _projectService = projectService;
        _drainageStrategyService = drainageStrategyService;
        _topsideService = topsideService;
        _surfService = surfService;
        _substructureService = substructureService;
        _transportService = transportService;
        _explorationService = explorationService;
        _wellProjectService = wellProjectService;
        _logger = loggerFactory.CreateLogger<CaseService>();
        _mapper = mapper;
        _mapperService = mapperService;
        _repository = repository;
    }

    public async Task<ProjectDto> CreateCase(Guid projectId, CreateCaseDto createCaseDto)
    {
        var caseItem = _mapper.Map<Case>(createCaseDto);
        if (caseItem == null)
        {
            throw new ArgumentNullException(nameof(caseItem));
        }
        var project = await _projectService.GetProject(projectId);
        caseItem.Project = project;
        caseItem.CapexFactorFeasibilityStudies = 0.015;
        caseItem.CapexFactorFEEDStudies = 0.015;

        if (caseItem.DG4Date == DateTimeOffset.MinValue)
        {
            caseItem.DG4Date = new DateTimeOffset(2030, 1, 1, 0, 0, 0, TimeSpan.Zero);
        }

        caseItem.CreateTime = DateTimeOffset.UtcNow;

        var createdCase = _context.Cases!.Add(caseItem);
        await _context.SaveChangesAsync();

        var drainageStrategyDto = new CreateDrainageStrategyDto
        {
            Name = "Drainage strategy",
            Description = ""
        };
        var drainageStrategy = await _drainageStrategyService.CreateDrainageStrategy(projectId, createdCase.Entity.Id, drainageStrategyDto);
        caseItem.DrainageStrategyLink = drainageStrategy.Id;

        var topsideDto = new CreateTopsideDto
        {
            Name = "Topside",
            Source = Source.ConceptApp,
        };
        var topside = await _topsideService.CreateTopside(projectId, createdCase.Entity.Id, topsideDto);
        caseItem.TopsideLink = topside.Id;

        var surfDto = new CreateSurfDto
        {
            Name = "Surf",
            Source = Source.ConceptApp,
        };
        var surf = await _surfService.CreateSurf(projectId, createdCase.Entity.Id, surfDto);
        caseItem.SurfLink = surf.Id;

        var substructureDto = new CreateSubstructureDto
        {
            Name = "Substructure",
            Source = Source.ConceptApp,
        };
        var substructure = await _substructureService.CreateSubstructure(projectId, createdCase.Entity.Id, substructureDto);
        caseItem.SubstructureLink = substructure.Id;

        var transportDto = new CreateTransportDto
        {
            Name = "Transport",
            Source = Source.ConceptApp,
        };
        var transport = await _transportService.CreateTransport(projectId, createdCase.Entity.Id, transportDto);
        caseItem.TransportLink = transport.Id;

        var explorationDto = new CreateExplorationDto
        {
            Name = "Exploration",
        };
        var exploration = await _explorationService.CreateExploration(projectId, createdCase.Entity.Id, explorationDto);
        caseItem.ExplorationLink = exploration.Id;

        var wellProjectDto = new CreateWellProjectDto
        {
            Name = "WellProject",
        };
        var wellProject = await _wellProjectService.CreateWellProject(projectId, createdCase.Entity.Id, wellProjectDto);
        caseItem.WellProjectLink = wellProject.Id;

        return await _projectService.GetProjectDto(project.Id);
    }

    public async Task<ProjectDto> UpdateCaseAndProfiles<TDto>(Guid caseId, TDto updatedCaseDto)
        where TDto : BaseUpdateCaseDto
    {
        var caseItem = await GetCase(caseId);

        _mapper.Map(updatedCaseDto, caseItem);

        _context.Cases!.Update(caseItem);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDto(caseItem.ProjectId);
    }

    public async Task<ProjectDto> DeleteCase(Guid caseId)
    {
        var caseItem = await GetCase(caseId);
        _context.Cases!.Remove(caseItem);
        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDto(caseItem.ProjectId);
    }

    public async Task<Case> GetCase(Guid caseId)
    {
        var caseItem = await _context.Cases!
            .Include(c => c.TotalFeasibilityAndConceptStudies)
            .Include(c => c.TotalFeasibilityAndConceptStudiesOverride)
            .Include(c => c.TotalFEEDStudies)
            .Include(c => c.TotalFEEDStudiesOverride)
            .Include(c => c.TotalOtherStudies)
            .Include(c => c.HistoricCostCostProfile)
            .Include(c => c.WellInterventionCostProfile)
            .Include(c => c.WellInterventionCostProfileOverride)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfile)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfileOverride)
            .Include(c => c.OnshoreRelatedOPEXCostProfile)
            .Include(c => c.AdditionalOPEXCostProfile)
            .Include(c => c.CessationWellsCost)
            .Include(c => c.CessationWellsCostOverride)
            .Include(c => c.CessationOffshoreFacilitiesCost)
            .Include(c => c.CessationOffshoreFacilitiesCostOverride)
            .Include(c => c.CessationOnshoreFacilitiesCostProfile)
            .FirstOrDefaultAsync(c => c.Id == caseId);
        if (caseItem == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found.", caseId));
        }
        return caseItem;
    }

    public async Task<IEnumerable<Case>> GetAll()
    {
        if (_context.Cases != null)
        {
            return await _context.Cases
                    .Include(c => c.TotalFeasibilityAndConceptStudies)
                    .Include(c => c.TotalFeasibilityAndConceptStudiesOverride)
                    .Include(c => c.TotalFEEDStudies)
                    .Include(c => c.TotalFEEDStudiesOverride)
                    .Include(c => c.TotalOtherStudies)
                    .Include(c => c.HistoricCostCostProfile)
                    .Include(c => c.WellInterventionCostProfile)
                    .Include(c => c.WellInterventionCostProfileOverride)
                    .Include(c => c.OffshoreFacilitiesOperationsCostProfile)
                    .Include(c => c.OffshoreFacilitiesOperationsCostProfileOverride)
                    .Include(c => c.OnshoreRelatedOPEXCostProfile)
                    .Include(c => c.AdditionalOPEXCostProfile)
                    .Include(c => c.CessationWellsCost)
                    .Include(c => c.CessationWellsCostOverride)
                    .Include(c => c.CessationOffshoreFacilitiesCost)
                    .Include(c => c.CessationOffshoreFacilitiesCostOverride)
                    .Include(c => c.CessationOnshoreFacilitiesCostProfile)
                    .ToListAsync();
        }
        else
        {
            _logger.LogInformation("No cases exists");
            return new List<Case>();
        }
    }

    public async Task<CaseDto> UpdateCase<TDto>(
            Guid caseId,
            TDto updatedCaseDto
    )
        where TDto : BaseUpdateCaseDto
    {
        var existingCase = await _repository.GetCase(caseId)
            ?? throw new NotFoundInDBException($"Case with id {caseId} not found.");

        _mapperService.MapToEntity(updatedCaseDto, existingCase, caseId);

        existingCase.ModifyTime = DateTimeOffset.UtcNow;

        Case updatedCase;
        try
        {
            updatedCase = _repository.UpdateCase(existingCase);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update case with id {caseId}.", caseId);
            throw;
        }

        var dto = _mapperService.MapToDto<Case, CaseDto>(updatedCase, caseId);
        return dto;
    }

    public async Task<CessationWellsCostOverrideDto> UpdateCessationWellsCostOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateCessationWellsCostOverrideDto updatedCostProfileDto
    )
    {
        return await UpdateCaseCostProfile<CessationWellsCostOverride, CessationWellsCostOverrideDto, UpdateCessationWellsCostOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            _repository.GetCessationWellsCostOverride,
            _repository.UpdateCessationWellsCostOverride
        );
    }

    public async Task<CessationOffshoreFacilitiesCostOverrideDto> UpdateCessationOffshoreFacilitiesCostOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateCessationOffshoreFacilitiesCostOverrideDto updatedCostProfileDto
    )
    {
        return await UpdateCaseCostProfile<CessationOffshoreFacilitiesCostOverride, CessationOffshoreFacilitiesCostOverrideDto, UpdateCessationOffshoreFacilitiesCostOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            _repository.GetCessationOffshoreFacilitiesCostOverride,
            _repository.UpdateCessationOffshoreFacilitiesCostOverride
        );
    }

    public async Task<TotalFeasibilityAndConceptStudiesOverrideDto> UpdateTotalFeasibilityAndConceptStudiesOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateTotalFeasibilityAndConceptStudiesOverrideDto updatedCostProfileDto
    )
    {
        return await UpdateCaseCostProfile<TotalFeasibilityAndConceptStudiesOverride, TotalFeasibilityAndConceptStudiesOverrideDto, UpdateTotalFeasibilityAndConceptStudiesOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            _repository.GetTotalFeasibilityAndConceptStudiesOverride,
            _repository.UpdateTotalFeasibilityAndConceptStudiesOverride
        );
    }

    public async Task<TotalFEEDStudiesOverrideDto> UpdateTotalFEEDStudiesOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateTotalFEEDStudiesOverrideDto updatedCostProfileDto
    )
    {
        return await UpdateCaseCostProfile<TotalFEEDStudiesOverride, TotalFEEDStudiesOverrideDto, UpdateTotalFEEDStudiesOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            _repository.GetTotalFEEDStudiesOverride,
            _repository.UpdateTotalFEEDStudiesOverride
        );
    }

    public async Task<HistoricCostCostProfileDto> UpdateHistoricCostCostProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateHistoricCostCostProfileDto updatedCostProfileDto
    )
    {
        return await UpdateCaseCostProfile<HistoricCostCostProfile, HistoricCostCostProfileDto, UpdateHistoricCostCostProfileDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            _repository.GetHistoricCostCostProfile,
            _repository.UpdateHistoricCostCostProfile
        );
    }

    public async Task<WellInterventionCostProfileOverrideDto> UpdateWellInterventionCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateWellInterventionCostProfileOverrideDto updatedCostProfileDto
    )
    {
        return await UpdateCaseCostProfile<WellInterventionCostProfileOverride, WellInterventionCostProfileOverrideDto, UpdateWellInterventionCostProfileOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            _repository.GetWellInterventionCostProfileOverride,
            _repository.UpdateWellInterventionCostProfileOverride
        );
    }

    public async Task<OffshoreFacilitiesOperationsCostProfileOverrideDto> UpdateOffshoreFacilitiesOperationsCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto updatedCostProfileDto
    )
    {
        return await UpdateCaseCostProfile<OffshoreFacilitiesOperationsCostProfileOverride, OffshoreFacilitiesOperationsCostProfileOverrideDto, UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            _repository.GetOffshoreFacilitiesOperationsCostProfileOverride,
            _repository.UpdateOffshoreFacilitiesOperationsCostProfileOverride
        );
    }

    public async Task<OnshoreRelatedOPEXCostProfileDto> UpdateOnshoreRelatedOPEXCostProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateOnshoreRelatedOPEXCostProfileDto updatedCostProfileDto
    )
    {
        return await UpdateCaseCostProfile<OnshoreRelatedOPEXCostProfile, OnshoreRelatedOPEXCostProfileDto, UpdateOnshoreRelatedOPEXCostProfileDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            _repository.GetOnshoreRelatedOPEXCostProfile,
            _repository.UpdateOnshoreRelatedOPEXCostProfile
        );
    }

    public async Task<OffshoreFacilitiesOperationsCostProfileOverrideDto> CreateOffshoreFacilitiesOperationsCostProfileOverride(
        Guid projectId,
        Guid caseId,
        CreateOffshoreFacilitiesOperationsCostProfileOverrideDto createProfileDto
    )
    {
        return await CreateCaseProfile<OffshoreFacilitiesOperationsCostProfileOverride, OffshoreFacilitiesOperationsCostProfileOverrideDto, CreateOffshoreFacilitiesOperationsCostProfileOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            _repository.CreateOffshoreFacilitiesOperationsCostProfileOverride,
            CaseProfileNames.OffshoreFacilitiesOperationsCostProfileOverride
        );
    }
    public async Task<CessationWellsCostOverrideDto> CreateCessationWellsCostOverride(
        Guid projectId,
        Guid caseId,
        CreateCessationWellsCostOverrideDto createProfileDto
    )
    {
        return await CreateCaseProfile<CessationWellsCostOverride, CessationWellsCostOverrideDto, CreateCessationWellsCostOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            _repository.CreateCessationWellsCostOverride,
            CaseProfileNames.CessationWellsCostOverride
        );
    }

    public async Task<CessationOffshoreFacilitiesCostOverrideDto> CreateCessationOffshoreFacilitiesCostOverride(
        Guid projectId,
        Guid caseId,
        CreateCessationOffshoreFacilitiesCostOverrideDto createProfileDto
    )
    {
        return await CreateCaseProfile<CessationOffshoreFacilitiesCostOverride, CessationOffshoreFacilitiesCostOverrideDto, CreateCessationOffshoreFacilitiesCostOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            _repository.CreateCessationOffshoreFacilitiesCostOverride,
            CaseProfileNames.CessationOffshoreFacilitiesCostOverride
        );
    }

    public async Task<TotalFeasibilityAndConceptStudiesOverrideDto> CreateTotalFeasibilityAndConceptStudiesOverride(
        Guid projectId,
        Guid caseId,
        CreateTotalFeasibilityAndConceptStudiesOverrideDto createProfileDto
    )
    {
        return await CreateCaseProfile<TotalFeasibilityAndConceptStudiesOverride, TotalFeasibilityAndConceptStudiesOverrideDto, CreateTotalFeasibilityAndConceptStudiesOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            _repository.CreateTotalFeasibilityAndConceptStudiesOverride,
            CaseProfileNames.TotalFeasibilityAndConceptStudiesOverride
        );
    }

    public async Task<TotalFEEDStudiesOverrideDto> CreateTotalFEEDStudiesOverride(
        Guid projectId,
        Guid caseId,
        CreateTotalFEEDStudiesOverrideDto createProfileDto
    )
    {
        return await CreateCaseProfile<TotalFEEDStudiesOverride, TotalFEEDStudiesOverrideDto, CreateTotalFEEDStudiesOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            _repository.CreateTotalFEEDStudiesOverride,
            CaseProfileNames.TotalFEEDStudiesOverride
        );
    }

    public async Task<HistoricCostCostProfileDto> CreateHistoricCostCostProfile(
        Guid projectId,
        Guid caseId,
        CreateHistoricCostCostProfileDto createProfileDto
    )
    {
        return await CreateCaseProfile<HistoricCostCostProfile, HistoricCostCostProfileDto, CreateHistoricCostCostProfileDto>(
            projectId,
            caseId,
            createProfileDto,
            _repository.CreateHistoricCostCostProfile,
            CaseProfileNames.HistoricCostCostProfile
        );
    }

    public async Task<WellInterventionCostProfileOverrideDto> CreateWellInterventionCostProfileOverride(
        Guid projectId,
        Guid caseId,
        CreateWellInterventionCostProfileOverrideDto createProfileDto
    )
    {
        return await CreateCaseProfile<WellInterventionCostProfileOverride, WellInterventionCostProfileOverrideDto, CreateWellInterventionCostProfileOverrideDto>(
            projectId,
            caseId,
            createProfileDto,
            _repository.CreateWellInterventionCostProfileOverride,
            CaseProfileNames.WellInterventionCostProfileOverride
        );
    }

    public async Task<OnshoreRelatedOPEXCostProfileDto> CreateOnshoreRelatedOPEXCostProfile(
        Guid projectId,
        Guid caseId,
        CreateOnshoreRelatedOPEXCostProfileDto createProfileDto
    )
    {
        return await CreateCaseProfile<OnshoreRelatedOPEXCostProfile, OnshoreRelatedOPEXCostProfileDto, CreateOnshoreRelatedOPEXCostProfileDto>(
            projectId,
            caseId,
            createProfileDto,
            _repository.CreateOnshoreRelatedOPEXCostProfile,
            CaseProfileNames.OnshoreRelatedOPEXCostProfile
        );
    }

    public async Task<AdditionalOPEXCostProfileDto> CreateAdditionalOPEXCostProfile(
        Guid projectId,
        Guid caseId,
        CreateAdditionalOPEXCostProfileDto createProfileDto
    )
    {
        return await CreateCaseProfile<AdditionalOPEXCostProfile, AdditionalOPEXCostProfileDto, CreateAdditionalOPEXCostProfileDto>(
            projectId,
            caseId,
            createProfileDto,
            _repository.CreateAdditionalOPEXCostProfile,
            CaseProfileNames.AdditionalOPEXCostProfile
        );
    }

    public async Task<AdditionalOPEXCostProfileDto> UpdateAdditionalOPEXCostProfile(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        UpdateAdditionalOPEXCostProfileDto updatedCostProfileDto
    )
    {
        return await UpdateCaseCostProfile<AdditionalOPEXCostProfile, AdditionalOPEXCostProfileDto, UpdateAdditionalOPEXCostProfileDto>(
            projectId,
            caseId,
            costProfileId,
            updatedCostProfileDto,
            _repository.GetAdditionalOPEXCostProfile,
            _repository.UpdateAdditionalOPEXCostProfile
        );
    }

    private async Task<TDto> UpdateCaseCostProfile<TProfile, TDto, TUpdateDto>(
        Guid projectId,
        Guid caseId,
        Guid costProfileId,
        TUpdateDto updatedCostProfileDto,
        Func<Guid, Task<TProfile?>> getProfile,
        Func<TProfile, TProfile> updateProfile
    )
        where TProfile : class
        where TDto : class
        where TUpdateDto : class
    {
        var existingProfile = await getProfile(costProfileId)
            ?? throw new NotFoundInDBException($"Production profile with id {costProfileId} not found.");

        _mapperService.MapToEntity(updatedCostProfileDto, existingProfile, caseId);

        TProfile updatedProfile;
        try
        {
            updatedProfile = updateProfile(existingProfile);
            await _repository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            var profileName = typeof(TProfile).Name;
            _logger.LogError(ex, "Failed to update profile {profileName} with id {costProfileId} for case id {caseId}.", profileName, costProfileId, caseId);
            throw;
        }


        var updatedDto = _mapperService.MapToDto<TProfile, TDto>(updatedProfile, costProfileId);
        return updatedDto;
    }

    private async Task<TDto> CreateCaseProfile<TProfile, TDto, TCreateDto>(
        Guid projectId,
        Guid caseId,
        TCreateDto createProfileDto,
        Func<TProfile, TProfile> createProfile,
        CaseProfileNames profileName
    )
        where TProfile : class, ICaseTimeSeries, new()
        where TDto : class
        where TCreateDto : class
    {
        var caseEntity = await _repository.GetCase(caseId)
            ?? throw new NotFoundInDBException($"Case with id {caseId} not found.");

        var resourceHasProfile = await _repository.CaseHasProfile(caseId, profileName);

        if (resourceHasProfile)
        {
            throw new ResourceAlreadyExistsException($"Case with id {caseId} already has a profile of type {typeof(TProfile).Name}.");
        }

        TProfile profile = new()
        {
            Case = caseEntity,
        };

        var newProfile = _mapperService.MapToEntity(createProfileDto, profile, caseId);

        TProfile createdProfile;
        try
        {
            createdProfile = createProfile(newProfile);
            await _repository.UpdateModifyTime(caseId);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create profile {profileName} for case id {caseId}.", profileName, caseId);
            throw;
        }

        var updatedDto = _mapperService.MapToDto<TProfile, TDto>(createdProfile, createdProfile.Id);
        return updatedDto;
    }
}
