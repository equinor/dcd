using System.Globalization;
using System.Runtime.CompilerServices;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Mappings;
using api.Models;
using api.Services.GenerateCostProfiles;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

// TODO: Delete once autosave is implemented
public class CaseAndAssetsService : ICaseAndAssetsService
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
    private readonly ICostProfileFromDrillingScheduleHelper _costProfileFromDrillingScheduleHelper;
    private readonly ILogger<CaseAndAssetsService> _logger;
    private readonly IStudyCostProfileService _generateStudyCostProfile;
    private readonly IOpexCostProfileService _generateOpexCostProfile;
    private readonly ICessationCostProfileService _generateCessationCostProfile;
    private readonly ICo2EmissionsProfileService _generateCo2EmissionsProfile;
    private readonly IGenerateGAndGAdminCostProfile _generateGAndGAdminCostProfile;
    private readonly IImportedElectricityProfileService _generateImportedElectricityProfile;
    private readonly IFuelFlaringLossesProfileService _generateFuelFlaringLossesProfile;
    private readonly INetSaleGasProfileService _generateNetSaleGasProfile;
    private readonly IMapper _mapper;

    public CaseAndAssetsService(
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
        ICostProfileFromDrillingScheduleHelper costProfileFromDrillingScheduleHelper,
        ILoggerFactory loggerFactory,
        IStudyCostProfileService generateStudyCostProfile,
        IOpexCostProfileService generateOpexCostProfile,
        ICessationCostProfileService generateCessationCostProfile,
        ICo2EmissionsProfileService generateCo2EmissionsProfile,
        IGenerateGAndGAdminCostProfile generateGAndGAdminCostProfile,
        IImportedElectricityProfileService generateImportedElectricityProfile,
        IFuelFlaringLossesProfileService generateFuelFlaringLossesProfile,
        INetSaleGasProfileService generateNetSaleGasProfile,
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

        _costProfileFromDrillingScheduleHelper = costProfileFromDrillingScheduleHelper;

        _logger = loggerFactory.CreateLogger<CaseAndAssetsService>();
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

    public async Task<ProjectWithGeneratedProfilesDto> UpdateCaseWithAssets(Guid projectId, Guid caseId, CaseWithAssetsWrapperDto wrapper)
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
            await CreateAndUpdateExplorationWells(wrapper.ExplorationWellDto, updatedCaseDto.Id, profilesToGenerate);
        }
        if (wrapper.WellProjectWellDtos?.Length > 0)
        {
            await CreateAndUpdateWellProjectWells(wrapper.WellProjectWellDtos, updatedCaseDto.Id, profilesToGenerate);
        }

        await _context.SaveChangesAsync();

        var generatedProfiles = await GenerateProfiles(profilesToGenerate, caseId);

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

    private async Task<GeneratedProfilesDto> GenerateProfiles(ProfilesToGenerate profilesToGenerate, Guid caseId)
    {
        var generatedProfiles = new GeneratedProfilesDto();

        if (profilesToGenerate.StudyCost)
        {
            await AddStudyCost(caseId, generatedProfiles);
        }
        if (profilesToGenerate.OpexCost)
        {
            await AddOpexCost(caseId, generatedProfiles);
        }
        if (profilesToGenerate.CessationCost)
        {
            await AddCessationCost(caseId, generatedProfiles);
        }
        if (profilesToGenerate.Co2Emissions)
        {
            await AddCo2EmissionsCost(caseId, generatedProfiles);
        }
        if (profilesToGenerate.GAndGAdminCost)
        {
            await AddGAndGAdminCost(caseId, generatedProfiles);
        }
        if (profilesToGenerate.ImportedElectricity)
        {
            await AddImportedElectricity(caseId, generatedProfiles);
        }
        if (profilesToGenerate.FuelFlaringAndLosses)
        {
            await AddFuelFlaringAndLosses(caseId, generatedProfiles);
            profilesToGenerate.NetSalesGas = true;
        }
        if (profilesToGenerate.NetSalesGas)
        {
            await AddNetSalesGas(caseId, generatedProfiles);
        }

        return generatedProfiles;
    }

    private async Task AddStudyCost(Guid caseId, GeneratedProfilesDto generatedProfiles)
    {
        var dtoWrapper = await _generateStudyCostProfile.Generate(caseId);
        generatedProfiles.StudyCostProfileWrapperDto = dtoWrapper;
    }

    private async Task AddOpexCost(Guid caseId, GeneratedProfilesDto generatedProfiles)
    {
        var dtoWrapper = await _generateOpexCostProfile.Generate(caseId);
        generatedProfiles.OpexCostProfileWrapperDto = dtoWrapper;
    }

    private async Task AddCessationCost(Guid caseId, GeneratedProfilesDto generatedProfiles)
    {
        var dtoWrapper = await _generateCessationCostProfile.Generate(caseId);
        generatedProfiles.CessationCostWrapperDto = dtoWrapper;
    }

    private async Task AddCo2EmissionsCost(Guid caseId, GeneratedProfilesDto generatedProfiles)
    {
        var dtoWrapper = await _generateCo2EmissionsProfile.Generate(caseId);
        generatedProfiles.Co2EmissionsDto = dtoWrapper;
    }

    private async Task AddGAndGAdminCost(Guid caseId, GeneratedProfilesDto generatedProfiles)
    {
        var dtoWrapper = await _generateGAndGAdminCostProfile.Generate(caseId);
        generatedProfiles.GAndGAdminCostDto = dtoWrapper;
    }

    private async Task AddImportedElectricity(Guid caseId, GeneratedProfilesDto generatedProfiles)
    {
        var dtoWrapper = await _generateImportedElectricityProfile.Generate(caseId);
        generatedProfiles.ImportedElectricityDto = dtoWrapper;
    }

    private async Task AddFuelFlaringAndLosses(Guid caseId, GeneratedProfilesDto generatedProfiles)
    {
        var dtoWrapper = await _generateFuelFlaringLossesProfile.Generate(caseId);
        generatedProfiles.FuelFlaringAndLossesDto = dtoWrapper;
    }

    private async Task AddNetSalesGas(Guid caseId, GeneratedProfilesDto generatedProfiles)
    {
        var dtoWrapper = await _generateNetSaleGasProfile.Generate(caseId);
        generatedProfiles.NetSalesGasDto = dtoWrapper;
    }

    private async Task CreateAndUpdateExplorationWells(UpdateExplorationWellDto[] explorationWellDtos, Guid caseId, ProfilesToGenerate profilesToGenerate)
    {
        foreach (var explorationWellDto in explorationWellDtos)
        {
            var existingExplorationWell = await _context.ExplorationWell!
                .Include(wpw => wpw.DrillingSchedule)
                .FirstOrDefaultAsync(w => w.WellId == explorationWellDto.WellId && w.ExplorationId == explorationWellDto.ExplorationId);
            if (existingExplorationWell != null)
            {
                _mapper.Map(explorationWellDto, existingExplorationWell);
                if (explorationWellDto.DrillingSchedule == null && existingExplorationWell.DrillingSchedule != null)
                {
                    _context.DrillingSchedule!.Remove(existingExplorationWell.DrillingSchedule);
                }
            }
            else
            {
                var explorationWell = _mapper.Map<ExplorationWell>(explorationWellDto);
                if (explorationWell == null)
                {
                    throw new Exception("Failed to map exploration well");
                }
                _context.ExplorationWell!.Add(explorationWell);
            }
        }

        await _context.SaveChangesAsync();
        var wellProject = await _costProfileFromDrillingScheduleHelper.UpdateExplorationCostProfilesForCase(caseId);
    }

    private async Task CreateAndUpdateWellProjectWells(UpdateWellProjectWellDto[] wellProjectWellDtos, Guid caseId, ProfilesToGenerate profilesToGenerate)
    {
        foreach (var wellProjectWellDto in wellProjectWellDtos)
        {
            var existingWellProjectWell = await _context.WellProjectWell!
                .Include(wpw => wpw.DrillingSchedule)
                .FirstOrDefaultAsync(w => w.WellId == wellProjectWellDto.WellId && w.WellProjectId == wellProjectWellDto.WellProjectId);

            if (existingWellProjectWell != null)
            {
                _mapper.Map(wellProjectWellDto, existingWellProjectWell);
                if (wellProjectWellDto.DrillingSchedule == null && existingWellProjectWell.DrillingSchedule != null)
                {
                    _context.DrillingSchedule!.Remove(existingWellProjectWell.DrillingSchedule);
                }
            }
            else
            {
                var wellProjectWell = _mapper.Map<WellProjectWell>(wellProjectWellDto);
                if (wellProjectWell == null)
                {
                    throw new Exception("Failed to map WellProjectWell");
                }
                _context.WellProjectWell!.Add(wellProjectWell);
            }
        }

        await _context.SaveChangesAsync();
        var wellProject = await _costProfileFromDrillingScheduleHelper.UpdateWellProjectCostProfilesForCase(caseId);
    }

    private async Task<CaseWithProfilesDto> UpdateCase(Guid caseId, APIUpdateCaseWithProfilesDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        profilesToGenerate.StudyCost = true;
        profilesToGenerate.Co2Emissions = true;
        profilesToGenerate.GAndGAdminCost = true;
        profilesToGenerate.FuelFlaringAndLosses = true;
        profilesToGenerate.ImportedElectricity = true;

        var item = await _caseService.GetCase(caseId);
        _mapper.Map(updatedDto, item);
        var updatedItem = _context.Cases!.Update(item);
        var caseDto = _mapper.Map<CaseWithProfilesDto>(updatedItem.Entity);
        if (caseDto == null)
        {
            throw new Exception("Failed to update case");
        }
        return caseDto;
    }

    private async Task<DrainageStrategyWithProfilesDto?> UpdateDrainageStrategy(
        Guid drainageStrategyId,
        UpdateDrainageStrategyWithProfilesDto updatedDto,
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

        _mapper.Map(updatedDto, item, opts => opts.Items["ConversionUnit"] = unit.ToString());

        var updatedItem = _context.DrainageStrategies!.Update(item);

        var destination = _mapper.Map<DrainageStrategy, DrainageStrategyWithProfilesDto>(updatedItem.Entity, opts => opts.Items["ConversionUnit"] = unit.ToString());

        return destination;
    }

    private async Task<WellProjectWithProfilesDto?> UpdateWellProject(Guid wellProjectLink, UpdateWellProjectWithProfilesDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        profilesToGenerate.StudyCost = true;

        var item = await _wellProjectService.GetWellProject(wellProjectLink);

        _mapper.Map(updatedDto, item);

        var updatedItem = _context.WellProjects!.Update(item);
        return _mapper.Map<WellProjectWithProfilesDto>(updatedItem.Entity);
    }

    private async Task<ExplorationWithProfilesDto?> UpdateExploration(Guid explorationLink, UpdateExplorationWithProfilesDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        profilesToGenerate.GAndGAdminCost = true;

        var item = await _explorationService.GetExploration(explorationLink);

        _mapper.Map(updatedDto, item);

        var updatedItem = _context.Explorations!.Update(item);
        return _mapper.Map<ExplorationWithProfilesDto>(updatedItem.Entity);
    }

    private async Task<SurfWithProfilesDto?> UpdateSurf(Guid surfLink, APIUpdateSurfWithProfilesDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        profilesToGenerate.StudyCost = true;
        profilesToGenerate.CessationCost = true;

        var item = await _surfService.GetSurf(surfLink);

        _mapper.Map(updatedDto, item);

        var updatedItem = _context.Surfs!.Update(item);
        return _mapper.Map<SurfWithProfilesDto>(updatedItem.Entity);
    }

    private async Task<SubstructureWithProfilesDto?> UpdateSubstructure(Guid substructureLink, APIUpdateSubstructureWithProfilesDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        profilesToGenerate.StudyCost = true;

        var item = await _substructureService.GetSubstructure(substructureLink);

        _mapper.Map(updatedDto, item);

        var updatedItem = _context.Substructures!.Update(item);
        return _mapper.Map<SubstructureWithProfilesDto>(updatedItem.Entity);
    }

    private async Task<TransportWithProfilesDto?> UpdateTransport(Guid transportLink, APIUpdateTransportWithProfilesDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        profilesToGenerate.StudyCost = true;

        var item = await _transportService.GetTransport(transportLink);

        _mapper.Map(updatedDto, item);

        var updatedItem = _context.Transports!.Update(item);
        return _mapper.Map<TransportWithProfilesDto>(updatedItem.Entity);
    }
    private async Task<TopsideWithProfilesDto?> UpdateTopside(Guid topsideLink, APIUpdateTopsideWithProfilesDto updatedDto, ProfilesToGenerate profilesToGenerate)
    {
        profilesToGenerate.StudyCost = true;
        profilesToGenerate.OpexCost = true;
        profilesToGenerate.Co2Emissions = true;
        profilesToGenerate.FuelFlaringAndLosses = true;
        profilesToGenerate.ImportedElectricity = true;

        var item = await _topsideService.GetTopside(topsideLink);

        _mapper.Map(updatedDto, item);

        var updatedItem = _context.Topsides!.Update(item);
        return _mapper.Map<TopsideWithProfilesDto>(updatedItem.Entity);
    }
}
