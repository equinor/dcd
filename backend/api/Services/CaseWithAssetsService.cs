using System.Globalization;
using System.Runtime.CompilerServices;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;
using api.Services.GenerateCostProfiles;

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

    public CaseWithAssetsService(DcdDbContext context, IProjectService projectService, ICaseService caseService,
    IDrainageStrategyService drainageStrategyService, IWellProjectService wellProjectService, IExplorationService explorationService,
    ISurfService surfService, ISubstructureService substructureService, ITransportService transportService, ITopsideService topsideService,
    IExplorationWellService explorationWellService, IWellProjectWellService wellProjectWellService,
    ICostProfileFromDrillingScheduleHelper costProfileFromDrillingScheduleHelper,
    ILoggerFactory loggerFactory,
    IGenerateStudyCostProfile generateStudyCostProfile, IGenerateOpexCostProfile generateOpexCostProfile,
    IGenerateCessationCostProfile generateCessationCostProfile, IGenerateCo2EmissionsProfile generateCo2EmissionsProfile,
    IGenerateGAndGAdminCostProfile generateGAndGAdminCostProfile, IGenerateImportedElectricityProfile generateImportedElectricityProfile,
    IGenerateFuelFlaringLossesProfile generateFuelFlaringLossesProfile, IGenerateNetSaleGasProfile generateNetSaleGasProfile)
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

    public async Task<ProjectWithGeneratedProfilesDto> UpdateCaseWithAssetsAsync(CaseWithAssetsWrapperDto wrapper)
    {
        var project = _projectService.GetProjectWithoutAssets(wrapper.CaseDto.ProjectId);

        var profilesToGenerate = new ProfilesToGenerate();

        var updatedCaseDto = UpdateCase(wrapper.CaseDto, profilesToGenerate);

        UpdateDrainageStrategy(wrapper.DrainageStrategyDto, project.PhysicalUnit, profilesToGenerate);
        UpdateWellProject(wrapper.WellProjectDto, profilesToGenerate);
        UpdateExploration(wrapper.ExplorationDto, profilesToGenerate);
        UpdateSurf(wrapper.SurfDto, profilesToGenerate);
        UpdateSubstructure(wrapper.SubstructureDto, profilesToGenerate);
        UpdateTransport(wrapper.TransportDto, profilesToGenerate);
        UpdateTopside(wrapper.TopsideDto, profilesToGenerate);

        if (wrapper.ExplorationWellDto?.Length > 0)
        {
            await CreateAndUpdateExplorationWellsAsync(wrapper.ExplorationWellDto, updatedCaseDto.Id, profilesToGenerate);
        }

        if (wrapper.WellProjectWellDtos?.Length > 0)
        {
            await CreateAndUpdateWellProjectWellsAsync(wrapper.WellProjectWellDtos, updatedCaseDto.Id, profilesToGenerate);
        }

        var saveResult = await _context.SaveChangesAsync();

        var generatedProfiles = await GenerateProfilesAsync(profilesToGenerate, wrapper.CaseDto.Id);

        var projectDto = _projectService.GetProjectDto(updatedCaseDto.ProjectId);

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

    public async Task CreateAndUpdateExplorationWellsAsync(ExplorationWellDto[] explorationWellDtos, Guid caseId, ProfilesToGenerate profilesToGenerate)
    {
        var changes = false;
        var runSaveChanges = false;
        foreach (var explorationWellDto in explorationWellDtos)
        {
            if (explorationWellDto.DrillingSchedule?.Values?.Length > 0)
            {
                if (explorationWellDto.DrillingSchedule.Id == Guid.Empty)
                {
                    var explorationWell = ExplorationWellAdapter.Convert(explorationWellDto);
                    _context.ExplorationWell!.Add(explorationWell);
                    changes = true;
                    runSaveChanges = true;
                }
                else
                {
                    if (explorationWellDto.HasChanges)
                    {
                        var existingExplorationWell = _explorationWellService.GetExplorationWell(explorationWellDto.WellId, explorationWellDto.ExplorationId);
                        ExplorationWellAdapter.ConvertExisting(existingExplorationWell, explorationWellDto);
                        if (explorationWellDto.DrillingSchedule == null && existingExplorationWell.DrillingSchedule != null)
                        {
                            _context.DrillingSchedule!.Remove(existingExplorationWell.DrillingSchedule);
                        }

                        _context.ExplorationWell!.Update(existingExplorationWell);
                        changes = true;
                    }
                }
            }
        }

        if (changes)
        {
            if (runSaveChanges)
            {
                await _context.SaveChangesAsync();
            }
            var explorationDto = _costProfileFromDrillingScheduleHelper.UpdateExplorationCostProfilesForCase(caseId);
            explorationDto.HasChanges = true;
            UpdateExploration(explorationDto, profilesToGenerate);
        }
    }

    public async Task CreateAndUpdateWellProjectWellsAsync(WellProjectWellDto[] wellProjectWellDtos, Guid caseId, ProfilesToGenerate profilesToGenerate)
    {
        var changes = false;
        var runSaveChanges = false;
        foreach (var wellProjectWellDto in wellProjectWellDtos)
        {
            if (wellProjectWellDto.DrillingSchedule?.Values?.Length > 0)
            {
                if (wellProjectWellDto.DrillingSchedule.Id == Guid.Empty)
                {
                    var wellProjectWell = WellProjectWellAdapter.Convert(wellProjectWellDto);
                    _context.WellProjectWell!.Add(wellProjectWell);
                    changes = true;
                    runSaveChanges = true;
                }
                else
                {
                    if (wellProjectWellDto.HasChanges)
                    {
                        var existingWellProjectWell = _wellProjectWellService.GetWellProjectWell(wellProjectWellDto.WellId, wellProjectWellDto.WellProjectId);
                        WellProjectWellAdapter.ConvertExisting(existingWellProjectWell, wellProjectWellDto);
                        if (wellProjectWellDto.DrillingSchedule == null && existingWellProjectWell.DrillingSchedule != null)
                        {
                            _context.DrillingSchedule!.Remove(existingWellProjectWell.DrillingSchedule);
                        }

                        _context.WellProjectWell!.Update(existingWellProjectWell);
                        changes = true;
                    }
                }
            }
        }

        if (changes)
        {
            if (runSaveChanges)
            {
                await _context.SaveChangesAsync();
            }
            var wellProjectDto = _costProfileFromDrillingScheduleHelper.UpdateWellProjectCostProfilesForCase(caseId);
            wellProjectDto.HasChanges = true;
            UpdateWellProject(wellProjectDto, profilesToGenerate);
        }
    }

    public CaseDto UpdateCase(CaseDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }

        profilesToGenerate.StudyCost = true;
        profilesToGenerate.Co2Emissions = true;
        profilesToGenerate.GAndGAdminCost = true;
        profilesToGenerate.FuelFlaringAndLosses = true;
        profilesToGenerate.ImportedElectricity = true;

        var item = _caseService.GetCase(updatedDto.Id);
        CaseAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Cases!.Update(item);
        return CaseDtoAdapter.Convert(updatedItem.Entity);
    }

    public DrainageStrategyDto UpdateDrainageStrategy(DrainageStrategyDto updatedDto, PhysUnit unit, ProfilesToGenerate profilesToGenerate)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }

        profilesToGenerate.OpexCost = true;
        profilesToGenerate.CessationCost = true;
        profilesToGenerate.Co2Emissions = true;
        profilesToGenerate.FuelFlaringAndLosses = true;
        profilesToGenerate.ImportedElectricity = true;

        var item = _drainageStrategyService.GetDrainageStrategy(updatedDto.Id);
        DrainageStrategyAdapter.ConvertExisting(item, updatedDto, unit, false);
        var updatedItem = _context.DrainageStrategies!.Update(item);
        return DrainageStrategyDtoAdapter.Convert(updatedItem.Entity, unit);
    }

    public WellProjectDto UpdateWellProject(WellProjectDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }

        profilesToGenerate.StudyCost = true;

        var item = _wellProjectService.GetWellProject(updatedDto.Id);
        WellProjectAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.WellProjects!.Update(item);
        return WellProjectDtoAdapter.Convert(updatedItem.Entity);
    }

    public ExplorationDto UpdateExploration(ExplorationDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }

        profilesToGenerate.GAndGAdminCost = true;

        var item = _explorationService.GetExploration(updatedDto.Id);
        ExplorationAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Explorations!.Update(item);
        return ExplorationDtoAdapter.Convert(updatedItem.Entity);
    }

    public SurfDto UpdateSurf(SurfDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }

        profilesToGenerate.StudyCost = true;
        profilesToGenerate.CessationCost = true;

        var item = _surfService.GetSurf(updatedDto.Id);
        SurfAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Surfs!.Update(item);
        return SurfDtoAdapter.Convert(updatedItem.Entity);
    }

    public SubstructureDto UpdateSubstructure(SubstructureDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }

        profilesToGenerate.StudyCost = true;

        var item = _substructureService.GetSubstructure(updatedDto.Id);
        SubstructureAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Substructures!.Update(item);
        return SubstructureDtoAdapter.Convert(updatedItem.Entity);
    }

    public TransportDto UpdateTransport(TransportDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }

        profilesToGenerate.StudyCost = true;

        var item = _transportService.GetTransport(updatedDto.Id);
        TransportAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Transports!.Update(item);
        return TransportDtoAdapter.Convert(updatedItem.Entity);
    }
    public TopsideDto UpdateTopside(TopsideDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        if (!updatedDto.HasChanges)
        {
            return updatedDto;
        }

        profilesToGenerate.StudyCost = true;
        profilesToGenerate.OpexCost = true;
        profilesToGenerate.Co2Emissions = true;
        profilesToGenerate.FuelFlaringAndLosses = true;
        profilesToGenerate.ImportedElectricity = true;

        var item = _topsideService.GetTopside(updatedDto.Id);
        TopsideAdapter.ConvertExisting(item, updatedDto);
        var updatedItem = _context.Topsides!.Update(item);
        return TopsideDtoAdapter.Convert(updatedItem.Entity);
    }
}
