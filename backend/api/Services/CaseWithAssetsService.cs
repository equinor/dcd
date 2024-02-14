using System.Globalization;
using System.Runtime.CompilerServices;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;
using api.Services.GenerateCostProfiles;

using AutoMapper;

namespace api.Services;

public class CaseWithAssetsService : ICaseWithAssetsService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly ICaseService _caseService;
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly IWellProjectService _wellProjectService;
    private readonly IExplorationService _explorationService;
    private readonly ISurfService _surfService;
    private readonly ISubstructureService _substructureService;
    private readonly ITransportService _transportService;
    private readonly ITopsideService _topsideService;
    private readonly IExplorationWellService _explorationWellService;
    private readonly IWellProjectWellService _wellProjectWellService;
    private readonly ICostProfileFromDrillingScheduleHelper _costProfileFromDrillingScheduleHelper;
    private readonly ILogger<CaseWithAssetsService> _logger;
    private readonly IGenerateStudyCostProfile _generateStudyCostProfile;
    private readonly IGenerateOpexCostProfile _generateOpexCostProfile;
    private readonly IGenerateCessationCostProfile _generateCessationCostProfile;
    private readonly IGenerateCo2EmissionsProfile _generateCo2EmissionsProfile;
    private readonly IGenerateGAndGAdminCostProfile _generateGAndGAdminCostProfile;
    private readonly IGenerateImportedElectricityProfile _generateImportedElectricityProfile;
    private readonly IGenerateFuelFlaringLossesProfile _generateFuelFlaringLossesProfile;
    private readonly IGenerateNetSaleGasProfile _generateNetSaleGasProfile;
    private readonly IMapper _mapper;

    public CaseWithAssetsService(
        DcdDbContext context,
        IProjectService projectService,
        ICaseService caseService,
        IDrainageStrategyService drainageStrategyService,
        IWellProjectService wellProjectService,
        IExplorationService explorationService,
        ISurfService surfService,
        ISubstructureService substructureService,
        ITransportService transportService,
        ITopsideService topsideService,
        IExplorationWellService explorationWellService,
        IWellProjectWellService wellProjectWellService,
        ICostProfileFromDrillingScheduleHelper costProfileFromDrillingScheduleHelper,
        ILoggerFactory loggerFactory,
        IGenerateStudyCostProfile generateStudyCostProfile,
        IGenerateOpexCostProfile generateOpexCostProfile,
        IGenerateCessationCostProfile generateCessationCostProfile,
        IGenerateCo2EmissionsProfile generateCo2EmissionsProfile,
        IGenerateGAndGAdminCostProfile generateGAndGAdminCostProfile,
        IGenerateImportedElectricityProfile generateImportedElectricityProfile,
        IGenerateFuelFlaringLossesProfile generateFuelFlaringLossesProfile,
        IGenerateNetSaleGasProfile generateNetSaleGasProfile,
        IMapper mapper
    )
    {
        _context = context;

        _projectService = projectService;
        _caseService = caseService;

        _drainageStrategyService = drainageStrategyService;
        _wellProjectService = wellProjectService;
        _explorationService = explorationService;
        _surfService = surfService;
        _substructureService = substructureService;
        _transportService = transportService;
        _topsideService = topsideService;

        _explorationWellService = explorationWellService;
        _wellProjectWellService = wellProjectWellService;

        _costProfileFromDrillingScheduleHelper = costProfileFromDrillingScheduleHelper;

        _logger = loggerFactory.CreateLogger<CaseWithAssetsService>();
        _mapper = mapper;

        _generateStudyCostProfile = generateStudyCostProfile;
        _generateOpexCostProfile = generateOpexCostProfile;
        _generateCessationCostProfile = generateCessationCostProfile;
        _generateCo2EmissionsProfile = generateCo2EmissionsProfile;
        _generateGAndGAdminCostProfile = generateGAndGAdminCostProfile;
        _generateImportedElectricityProfile = generateImportedElectricityProfile;
        _generateFuelFlaringLossesProfile = generateFuelFlaringLossesProfile;
        _generateNetSaleGasProfile = generateNetSaleGasProfile;
    }

    public class ProfilesToGenerate
    {
        public bool StudyCost { get; set; }
        public bool CessationCost { get; set; }
        public bool OpexCost { get; set; }
        public bool Co2Emissions { get; set; }
        public bool GAndGAdminCost { get; set; }
        public bool ImportedElectricity { get; set; }
        public bool FuelFlaringAndLosses { get; set; }
        public bool NetSalesGas { get; set; }
    }

    public async Task<ProjectWithGeneratedProfilesDto> UpdateCaseWithAssetsAsync(Guid projectId, Guid caseId, CaseWithAssetsWrapperDto wrapper)
    {
        var project = await _projectService.GetProjectWithoutAssets(projectId);

        var profilesToGenerate = new ProfilesToGenerate();

        var updatedCaseDto = await UpdateCase(caseId, wrapper.CaseDto, profilesToGenerate);

        await UpdateDrainageStrategy(updatedCaseDto.DrainageStrategyLink, wrapper.DrainageStrategyDto, project.PhysicalUnit, profilesToGenerate);
        await UpdateWellProject(updatedCaseDto.WellProjectLink, wrapper.WellProjectDto, profilesToGenerate);
        await UpdateExploration(updatedCaseDto.ExplorationLink, wrapper.ExplorationDto, profilesToGenerate);
        await UpdateSurf(updatedCaseDto.SurfLink, wrapper.SurfDto, profilesToGenerate);
        await UpdateSubstructure(updatedCaseDto.SubstructureLink, wrapper.SubstructureDto, profilesToGenerate);
        await UpdateTransport(updatedCaseDto.TransportLink, wrapper.TransportDto, profilesToGenerate);
        await UpdateTopside(updatedCaseDto.TopsideLink, wrapper.TopsideDto, profilesToGenerate);

        if (wrapper.ExplorationWellDto?.Length > 0)
        {
            await CreateAndUpdateExplorationWellsAsync(wrapper.ExplorationWellDto, updatedCaseDto.Id, profilesToGenerate);
        }

        if (wrapper.WellProjectWellDtos?.Length > 0)
        {
            await CreateAndUpdateWellProjectWellsAsync(wrapper.WellProjectWellDtos, updatedCaseDto.Id, profilesToGenerate);
        }

        await _context.SaveChangesAsync();

        var generatedProfiles = await GenerateProfilesAsync(profilesToGenerate, caseId);

        var projectDto = await _projectService.GetProjectDto(updatedCaseDto.ProjectId);

        var result = new ProjectWithGeneratedProfilesDto
        {
            GeneratedProfilesDto = generatedProfiles,
            ProjectDto = projectDto
        };

        return result;
    }

    public class GeneratedProfilesDto
    {
        public StudyCostProfileWrapperDto? StudyCostProfileWrapperDto { get; set; }
        public OpexCostProfileWrapperDto? OpexCostProfileWrapperDto { get; set; }
        public CessationCostWrapperDto? CessationCostWrapperDto { get; set; }
        public Co2EmissionsDto? Co2EmissionsDto { get; set; }
        public GAndGAdminCostDto? GAndGAdminCostDto { get; set; }
        public ImportedElectricityDto? ImportedElectricityDto { get; set; }
        public FuelFlaringAndLossesDto? FuelFlaringAndLossesDto { get; set; }
        public NetSalesGasDto? NetSalesGasDto { get; set; }
    }

    private async Task<GeneratedProfilesDto> GenerateProfilesAsync(ProfilesToGenerate profilesToGenerate, Guid caseId)
    {
        var generatedProfiles = new GeneratedProfilesDto();

        if (profilesToGenerate.StudyCost)
        {
            await AddStudyCostAsync(caseId, generatedProfiles);
        }
        if (profilesToGenerate.OpexCost)
        {
            await AddOpexCostAsync(caseId, generatedProfiles);
        }
        if (profilesToGenerate.CessationCost)
        {
            await AddCessationCostAsync(caseId, generatedProfiles);
        }
        if (profilesToGenerate.Co2Emissions)
        {
            await AddCo2EmissionsCostAsync(caseId, generatedProfiles);
        }
        if (profilesToGenerate.GAndGAdminCost)
        {
            await AddGAndGAdminCostAsync(caseId, generatedProfiles);
        }
        if (profilesToGenerate.ImportedElectricity)
        {
            await AddImportedElectricityAsync(caseId, generatedProfiles);
        }
        if (profilesToGenerate.FuelFlaringAndLosses)
        {
            await AddFuelFlaringAndLossesAsync(caseId, generatedProfiles);
            profilesToGenerate.NetSalesGas = true;
        }
        if (profilesToGenerate.NetSalesGas)
        {
            await AddNetSalesGasAsync(caseId, generatedProfiles);
        }

        return generatedProfiles;
    }

    private async Task AddStudyCostAsync(Guid caseId, GeneratedProfilesDto generatedProfiles)
    {
        var dtoWrapper = await _generateStudyCostProfile.GenerateAsync(caseId);
        generatedProfiles.StudyCostProfileWrapperDto = dtoWrapper;
    }

    private async Task AddOpexCostAsync(Guid caseId, GeneratedProfilesDto generatedProfiles)
    {
        var dtoWrapper = await _generateOpexCostProfile.GenerateAsync(caseId);
        generatedProfiles.OpexCostProfileWrapperDto = dtoWrapper;
    }

    private async Task AddCessationCostAsync(Guid caseId, GeneratedProfilesDto generatedProfiles)
    {
        var dtoWrapper = await _generateCessationCostProfile.GenerateAsync(caseId);
        generatedProfiles.CessationCostWrapperDto = dtoWrapper;
    }

    private async Task AddCo2EmissionsCostAsync(Guid caseId, GeneratedProfilesDto generatedProfiles)
    {
        var dtoWrapper = await _generateCo2EmissionsProfile.GenerateAsync(caseId);
        generatedProfiles.Co2EmissionsDto = dtoWrapper;
    }

    private async Task AddGAndGAdminCostAsync(Guid caseId, GeneratedProfilesDto generatedProfiles)
    {
        var dtoWrapper = await _generateGAndGAdminCostProfile.GenerateAsync(caseId);
        generatedProfiles.GAndGAdminCostDto = dtoWrapper;
    }

    private async Task AddImportedElectricityAsync(Guid caseId, GeneratedProfilesDto generatedProfiles)
    {
        var dtoWrapper = await _generateImportedElectricityProfile.GenerateAsync(caseId);
        generatedProfiles.ImportedElectricityDto = dtoWrapper;
    }

    private async Task AddFuelFlaringAndLossesAsync(Guid caseId, GeneratedProfilesDto generatedProfiles)
    {
        var dtoWrapper = await _generateFuelFlaringLossesProfile.GenerateAsync(caseId);
        generatedProfiles.FuelFlaringAndLossesDto = dtoWrapper;
    }

    private async Task AddNetSalesGasAsync(Guid caseId, GeneratedProfilesDto generatedProfiles)
    {
        var dtoWrapper = await _generateNetSaleGasProfile.GenerateAsync(caseId);
        generatedProfiles.NetSalesGasDto = dtoWrapper;
    }

    public async Task CreateAndUpdateExplorationWellsAsync(UpdateExplorationWellDto[] explorationWellDtos, Guid caseId, ProfilesToGenerate profilesToGenerate)
    {
        var changes = false;
        var runSaveChanges = false;
        foreach (var explorationWellDto in explorationWellDtos)
        {
            if (explorationWellDto.DrillingSchedule?.Values?.Length > 0)
            {
                if (explorationWellDto.DrillingSchedule.Id == Guid.Empty)
                {
                    var explorationWell = _mapper.Map<ExplorationWell>(explorationWellDto);
                    if (explorationWell == null)
                    {
                        throw new Exception("Failed to map exploration well");
                    }
                    _context.ExplorationWell!.Add(explorationWell);
                    changes = true;
                    runSaveChanges = true;
                }
                else
                {
                    var existingExplorationWell = await _explorationWellService.GetExplorationWell(explorationWellDto.WellId, explorationWellDto.ExplorationId);
                    _mapper.Map(explorationWellDto, existingExplorationWell);
                    if (explorationWellDto.DrillingSchedule == null && existingExplorationWell.DrillingSchedule != null)
                    {
                        _context.DrillingSchedule!.Remove(existingExplorationWell.DrillingSchedule);
                    }

                    _context.ExplorationWell!.Update(existingExplorationWell);
                    changes = true;
                }
            }
        }

        if (changes)
        {
            if (runSaveChanges)
            {
                await _context.SaveChangesAsync();
            }
            var explorationDto = await _costProfileFromDrillingScheduleHelper.UpdateExplorationCostProfilesForCase(caseId);
            var updateDto = _mapper.Map<UpdateExplorationDto>(explorationDto);
            if (updateDto == null)
            {
                throw new Exception("Failed to update exploration cost profiles");
            }
            await UpdateExploration(explorationDto.Id, updateDto, profilesToGenerate);
        }
    }

    public async Task CreateAndUpdateWellProjectWellsAsync(UpdateWellProjectWellDto[] wellProjectWellDtos, Guid caseId, ProfilesToGenerate profilesToGenerate)
    {
        var changes = false;
        var runSaveChanges = false;
        foreach (var wellProjectWellDto in wellProjectWellDtos)
        {
            if (wellProjectWellDto.DrillingSchedule?.Values?.Length > 0)
            {
                if (wellProjectWellDto.DrillingSchedule.Id == Guid.Empty)
                {
                    var wellProjectWell = _mapper.Map<WellProjectWell>(wellProjectWellDto);
                    if (wellProjectWell == null)
                    {
                        throw new Exception("Failed to map well project well");
                    }
                    _context.WellProjectWell!.Add(wellProjectWell);
                    changes = true;
                    runSaveChanges = true;
                }
                else
                {
                    var existingWellProjectWell = await _wellProjectWellService.GetWellProjectWell(wellProjectWellDto.WellId, wellProjectWellDto.WellProjectId);
                    _mapper.Map(wellProjectWellDto, existingWellProjectWell);
                    if (wellProjectWellDto.DrillingSchedule == null && existingWellProjectWell.DrillingSchedule != null)
                    {
                        _context.DrillingSchedule!.Remove(existingWellProjectWell.DrillingSchedule);
                    }

                    _context.WellProjectWell!.Update(existingWellProjectWell);
                    changes = true;
                }
            }

            if (changes)
            {
                if (runSaveChanges)
                {
                    await _context.SaveChangesAsync();
                }
                var wellProjectDto = await _costProfileFromDrillingScheduleHelper.UpdateWellProjectCostProfilesForCase(caseId);
                var updateDto = _mapper.Map<UpdateWellProjectDto>(wellProjectDto);
                if (updateDto == null)
                {
                    throw new Exception("Failed to update well project cost profiles");
                }
                await UpdateWellProject(wellProjectDto.Id, updateDto, profilesToGenerate);
            }
        }
    }

    public async Task<CaseDto> UpdateCase(Guid caseId, UpdateCaseDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        profilesToGenerate.StudyCost = true;
        profilesToGenerate.Co2Emissions = true;
        profilesToGenerate.GAndGAdminCost = true;
        profilesToGenerate.FuelFlaringAndLosses = true;
        profilesToGenerate.ImportedElectricity = true;

        var item = await _caseService.GetCase(caseId);
        _mapper.Map(updatedDto, item);
        var updatedItem = _context.Cases!.Update(item);
        var caseDto = _mapper.Map<CaseDto>(updatedItem.Entity);
        if (caseDto == null)
        {
            throw new Exception("Failed to update case");
        }
        return caseDto;
    }

    public async Task<DrainageStrategyDto?> UpdateDrainageStrategy(
        Guid drainageStrategyId,
        UpdateDrainageStrategyDto updatedDto,
        PhysUnit unit,
        ProfilesToGenerate profilesToGenerate
        )
    {
        profilesToGenerate.OpexCost = true;
        profilesToGenerate.CessationCost = true;
        profilesToGenerate.Co2Emissions = true;
        profilesToGenerate.FuelFlaringAndLosses = true;
        profilesToGenerate.ImportedElectricity = true;

        var item = await _drainageStrategyService.GetDrainageStrategy(drainageStrategyId);

        _mapper.Map(updatedDto, item);

        var updatedItem = _context.DrainageStrategies!.Update(item);
        return _mapper.Map<DrainageStrategyDto>(updatedItem.Entity);
    }

    public async Task<WellProjectDto?> UpdateWellProject(Guid wellProjectLink, UpdateWellProjectDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        profilesToGenerate.StudyCost = true;

        var item = await _wellProjectService.GetWellProject(wellProjectLink);

        _mapper.Map(updatedDto, item);

        var updatedItem = _context.WellProjects!.Update(item);
        return _mapper.Map<WellProjectDto>(updatedItem.Entity);
    }

    public async Task<ExplorationDto?> UpdateExploration(Guid explorationLink, UpdateExplorationDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        profilesToGenerate.GAndGAdminCost = true;

        var item = await _explorationService.GetExploration(explorationLink);

        _mapper.Map(updatedDto, item);

        var updatedItem = _context.Explorations!.Update(item);
        return _mapper.Map<ExplorationDto>(updatedItem.Entity);
    }

    public async Task<SurfDto?> UpdateSurf(Guid surfLink, UpdateSurfDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        profilesToGenerate.StudyCost = true;
        profilesToGenerate.CessationCost = true;

        var item = await _surfService.GetSurf(surfLink);

        _mapper.Map(updatedDto, item);

        var updatedItem = _context.Surfs!.Update(item);
        return _mapper.Map<SurfDto>(updatedItem.Entity);
    }

    public async Task<SubstructureDto?> UpdateSubstructure(Guid substructureLink, UpdateSubstructureDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        profilesToGenerate.StudyCost = true;

        var item = await _substructureService.GetSubstructure(substructureLink);

        _mapper.Map(updatedDto, item);

        var updatedItem = _context.Substructures!.Update(item);
        return _mapper.Map<SubstructureDto>(updatedItem.Entity);
    }

    public async Task<TransportDto?> UpdateTransport(Guid transportLink, UpdateTransportDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        profilesToGenerate.StudyCost = true;

        var item = await _transportService.GetTransport(transportLink);

        _mapper.Map(updatedDto, item);

        var updatedItem = _context.Transports!.Update(item);
        return _mapper.Map<TransportDto>(updatedItem.Entity);
    }
    public async Task<TopsideDto?> UpdateTopside(Guid topsideLink, UpdateTopsideDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        profilesToGenerate.StudyCost = true;
        profilesToGenerate.OpexCost = true;
        profilesToGenerate.Co2Emissions = true;
        profilesToGenerate.FuelFlaringAndLosses = true;
        profilesToGenerate.ImportedElectricity = true;

        var item = await _topsideService.GetTopside(topsideLink);

        _mapper.Map(updatedDto, item);

        var updatedItem = _context.Topsides!.Update(item);
        return _mapper.Map<TopsideDto>(updatedItem.Entity);
    }
}
