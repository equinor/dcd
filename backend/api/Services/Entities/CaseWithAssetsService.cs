using System.Globalization;
using System.Runtime.CompilerServices;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Mappings;
using api.Models;
using api.Repositories;
using api.Services.GenerateCostProfiles;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class CaseWithAssetsService : ICaseWithAssetsService
{

    private readonly ICaseWithAssetsRepository _repository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapperService _mapperService;
    private readonly IConversionMapperService _conversionMapperService;
    public CaseWithAssetsService(
        ICaseWithAssetsRepository repository,
        IProjectRepository projectRepository,
        IMapperService mapperService,
        IConversionMapperService conversionMapperService
    )
    {
        _repository = repository;
        _projectRepository = projectRepository;
        _mapperService = mapperService;
        _conversionMapperService = conversionMapperService;
    }

    public async Task<CaseWithAssetsDto> GetCaseWithAssets(Guid projectId, Guid caseId)
    {
        var project = await _projectRepository.GetProject(projectId) ?? throw new NotFoundInDBException($"Project with id {projectId} not found");

        var (caseItem, drainageStrategy, topside, exploration, substructure, surf, transport, wellProject) = await _repository.GetCaseWithAssets(caseId);

        var caseWithAssetsDto = new CaseWithAssetsDto
        {
            Case = _mapperService.MapToDto<Case, CaseDto>(caseItem, caseItem.Id),
            CessationWellsCost = MapToDto<CessationWellsCost, CessationWellsCostDto>(caseItem.CessationWellsCost, caseItem.CessationWellsCost?.Id),
            CessationWellsCostOverride = MapToDto<CessationWellsCostOverride, CessationWellsCostOverrideDto>(caseItem.CessationWellsCostOverride, caseItem.CessationWellsCostOverride?.Id),
            CessationOffshoreFacilitiesCost = MapToDto<CessationOffshoreFacilitiesCost, CessationOffshoreFacilitiesCostDto>(caseItem.CessationOffshoreFacilitiesCost, caseItem.CessationOffshoreFacilitiesCost?.Id),
            CessationOffshoreFacilitiesCostOverride = MapToDto<CessationOffshoreFacilitiesCostOverride, CessationOffshoreFacilitiesCostOverrideDto>(caseItem.CessationOffshoreFacilitiesCostOverride, caseItem.CessationOffshoreFacilitiesCostOverride?.Id),
            CessationOnshoreFacilitiesCostProfile = MapToDto<CessationOnshoreFacilitiesCostProfile, CessationOnshoreFacilitiesCostProfileDto>(caseItem.CessationOnshoreFacilitiesCostProfile, caseItem.CessationOnshoreFacilitiesCostProfile?.Id),
            TotalFeasibilityAndConceptStudies = MapToDto<TotalFeasibilityAndConceptStudies, TotalFeasibilityAndConceptStudiesDto>(caseItem.TotalFeasibilityAndConceptStudies, caseItem.TotalFeasibilityAndConceptStudies?.Id),
            TotalFeasibilityAndConceptStudiesOverride = MapToDto<TotalFeasibilityAndConceptStudiesOverride, TotalFeasibilityAndConceptStudiesOverrideDto>(caseItem.TotalFeasibilityAndConceptStudiesOverride, caseItem.TotalFeasibilityAndConceptStudiesOverride?.Id),
            TotalFEEDStudies = MapToDto<TotalFEEDStudies, TotalFEEDStudiesDto>(caseItem.TotalFEEDStudies, caseItem.TotalFEEDStudies?.Id),
            TotalFEEDStudiesOverride = MapToDto<TotalFEEDStudiesOverride, TotalFEEDStudiesOverrideDto>(caseItem.TotalFEEDStudiesOverride, caseItem.TotalFEEDStudiesOverride?.Id),
            TotalOtherStudies = MapToDto<TotalOtherStudies, TotalOtherStudiesDto>(caseItem.TotalOtherStudies, caseItem.TotalOtherStudies?.Id),
            HistoricCostCostProfile = MapToDto<HistoricCostCostProfile, HistoricCostCostProfileDto>(caseItem.HistoricCostCostProfile, caseItem.HistoricCostCostProfile?.Id),
            WellInterventionCostProfile = MapToDto<WellInterventionCostProfile, WellInterventionCostProfileDto>(caseItem.WellInterventionCostProfile, caseItem.WellInterventionCostProfile?.Id),
            WellInterventionCostProfileOverride = MapToDto<WellInterventionCostProfileOverride, WellInterventionCostProfileOverrideDto>(caseItem.WellInterventionCostProfileOverride, caseItem.WellInterventionCostProfileOverride?.Id),
            OffshoreFacilitiesOperationsCostProfile = MapToDto<OffshoreFacilitiesOperationsCostProfile, OffshoreFacilitiesOperationsCostProfileDto>(caseItem.OffshoreFacilitiesOperationsCostProfile, caseItem.OffshoreFacilitiesOperationsCostProfile?.Id),
            OffshoreFacilitiesOperationsCostProfileOverride = MapToDto<OffshoreFacilitiesOperationsCostProfileOverride, OffshoreFacilitiesOperationsCostProfileOverrideDto>(caseItem.OffshoreFacilitiesOperationsCostProfileOverride, caseItem.OffshoreFacilitiesOperationsCostProfileOverride?.Id),
            OnshoreRelatedOPEXCostProfile = MapToDto<OnshoreRelatedOPEXCostProfile, OnshoreRelatedOPEXCostProfileDto>(caseItem.OnshoreRelatedOPEXCostProfile, caseItem.OnshoreRelatedOPEXCostProfile?.Id),
            AdditionalOPEXCostProfile = MapToDto<AdditionalOPEXCostProfile, AdditionalOPEXCostProfileDto>(caseItem.AdditionalOPEXCostProfile, caseItem.AdditionalOPEXCostProfile?.Id),

            DrainageStrategy = _conversionMapperService.MapToDto<DrainageStrategy, DrainageStrategyDto>(drainageStrategy, drainageStrategy.Id, project.PhysicalUnit),
            ProductionProfileOil = ConversionMapToDto<ProductionProfileOil, ProductionProfileOilDto>(drainageStrategy?.ProductionProfileOil, drainageStrategy?.ProductionProfileOil?.Id, project.PhysicalUnit),
            ProductionProfileGas = ConversionMapToDto<ProductionProfileGas, ProductionProfileGasDto>(drainageStrategy?.ProductionProfileGas, drainageStrategy?.ProductionProfileGas?.Id, project.PhysicalUnit),
            ProductionProfileWater = ConversionMapToDto<ProductionProfileWater, ProductionProfileWaterDto>(drainageStrategy?.ProductionProfileWater, drainageStrategy?.ProductionProfileWater?.Id, project.PhysicalUnit),
            ProductionProfileWaterInjection = ConversionMapToDto<ProductionProfileWaterInjection, ProductionProfileWaterInjectionDto>(drainageStrategy?.ProductionProfileWaterInjection, drainageStrategy?.ProductionProfileWaterInjection?.Id, project.PhysicalUnit),
            FuelFlaringAndLosses = ConversionMapToDto<FuelFlaringAndLosses, FuelFlaringAndLossesDto>(drainageStrategy?.FuelFlaringAndLosses, drainageStrategy?.FuelFlaringAndLosses?.Id, project.PhysicalUnit),
            FuelFlaringAndLossesOverride = ConversionMapToDto<FuelFlaringAndLossesOverride, FuelFlaringAndLossesOverrideDto>(drainageStrategy?.FuelFlaringAndLossesOverride, drainageStrategy?.FuelFlaringAndLossesOverride?.Id, project.PhysicalUnit),
            NetSalesGas = ConversionMapToDto<NetSalesGas, NetSalesGasDto>(drainageStrategy?.NetSalesGas, drainageStrategy?.NetSalesGas?.Id, project.PhysicalUnit),
            NetSalesGasOverride = ConversionMapToDto<NetSalesGasOverride, NetSalesGasOverrideDto>(drainageStrategy?.NetSalesGasOverride, drainageStrategy?.NetSalesGasOverride?.Id, project.PhysicalUnit),
            Co2Emissions = ConversionMapToDto<Co2Emissions, Co2EmissionsDto>(drainageStrategy?.Co2Emissions, drainageStrategy?.Co2Emissions?.Id, project.PhysicalUnit),
            Co2EmissionsOverride = ConversionMapToDto<Co2EmissionsOverride, Co2EmissionsOverrideDto>(drainageStrategy?.Co2EmissionsOverride, drainageStrategy?.Co2EmissionsOverride?.Id, project.PhysicalUnit),
            ProductionProfileNGL = ConversionMapToDto<ProductionProfileNGL, ProductionProfileNGLDto>(drainageStrategy?.ProductionProfileNGL, drainageStrategy?.ProductionProfileNGL?.Id, project.PhysicalUnit),
            ImportedElectricity = ConversionMapToDto<ImportedElectricity, ImportedElectricityDto>(drainageStrategy?.ImportedElectricity, drainageStrategy?.ImportedElectricity?.Id, project.PhysicalUnit),
            ImportedElectricityOverride = ConversionMapToDto<ImportedElectricityOverride, ImportedElectricityOverrideDto>(drainageStrategy?.ImportedElectricityOverride, drainageStrategy?.ImportedElectricityOverride?.Id, project.PhysicalUnit),
            // Co2Intensity = ConversionMapToDto<Co2Intensity, Co2IntensityDto>(drainageStrategy?.Co2Intensity, drainageStrategy?.Co2Intensity?.Id, project.PhysicalUnit),
            DeferredOilProduction = ConversionMapToDto<DeferredOilProduction, DeferredOilProductionDto>(drainageStrategy?.DeferredOilProduction, drainageStrategy?.DeferredOilProduction?.Id, project.PhysicalUnit),
            DeferredGasProduction = ConversionMapToDto<DeferredGasProduction, DeferredGasProductionDto>(drainageStrategy?.DeferredGasProduction, drainageStrategy?.DeferredGasProduction?.Id, project.PhysicalUnit),

            Topside = _mapperService.MapToDto<Topside, TopsideDto>(topside, topside.Id),
            TopsideCostProfile = MapToDto<TopsideCostProfile, TopsideCostProfileDto>(topside.CostProfile, topside.CostProfile?.Id),
            TopsideCostProfileOverride = MapToDto<TopsideCostProfileOverride, TopsideCostProfileOverrideDto>(topside.CostProfileOverride, topside.CostProfileOverride?.Id),
            TopsideCessationCostProfile = MapToDto<TopsideCessationCostProfile, TopsideCessationCostProfileDto>(topside.CessationCostProfile, topside.CessationCostProfile?.Id),

            Substructure = _mapperService.MapToDto<Substructure, SubstructureDto>(substructure, substructure.Id),
            SubstructureCostProfile = MapToDto<SubstructureCostProfile, SubstructureCostProfileDto>(substructure.CostProfile, substructure.CostProfile?.Id),
            SubstructureCostProfileOverride = MapToDto<SubstructureCostProfileOverride, SubstructureCostProfileOverrideDto>(substructure.CostProfileOverride, substructure.CostProfileOverride?.Id),
            SubstructureCessationCostProfile = MapToDto<SubstructureCessationCostProfile, SubstructureCessationCostProfileDto>(substructure.CessationCostProfile, substructure.CessationCostProfile?.Id),

            Surf = _mapperService.MapToDto<Surf, SurfDto>(surf, surf.Id),
            SurfCostProfile = MapToDto<SurfCostProfile, SurfCostProfileDto>(surf.CostProfile, surf.CostProfile?.Id),
            SurfCostProfileOverride = MapToDto<SurfCostProfileOverride, SurfCostProfileOverrideDto>(surf.CostProfileOverride, surf.CostProfileOverride?.Id),
            SurfCessationCostProfile = MapToDto<SurfCessationCostProfile, SurfCessationCostProfileDto>(surf.CessationCostProfile, surf.CessationCostProfile?.Id),

            Transport = _mapperService.MapToDto<Transport, TransportDto>(transport, transport.Id),
            TransportCostProfile = MapToDto<TransportCostProfile, TransportCostProfileDto>(transport.CostProfile, transport.CostProfile?.Id),
            TransportCostProfileOverride = MapToDto<TransportCostProfileOverride, TransportCostProfileOverrideDto>(transport.CostProfileOverride, transport.CostProfileOverride?.Id),
            TransportCessationCostProfile = MapToDto<TransportCessationCostProfile, TransportCessationCostProfileDto>(transport.CessationCostProfile, transport.CessationCostProfile?.Id),

            Exploration = _mapperService.MapToDto<Exploration, ExplorationDto>(exploration, exploration.Id),
            ExplorationWells = exploration.ExplorationWells?.Select(w => _mapperService.MapToDto<ExplorationWell, ExplorationWellDto>(w, w.ExplorationId)).ToList() ?? new List<ExplorationWellDto>(),
            ExplorationWellCostProfile = MapToDto<ExplorationWellCostProfile, ExplorationWellCostProfileDto>(exploration.ExplorationWellCostProfile, exploration.ExplorationWellCostProfile?.Id),
            AppraisalWellCostProfile = MapToDto<AppraisalWellCostProfile, AppraisalWellCostProfileDto>(exploration.AppraisalWellCostProfile, exploration.AppraisalWellCostProfile?.Id),
            SidetrackCostProfile = MapToDto<SidetrackCostProfile, SidetrackCostProfileDto>(exploration.SidetrackCostProfile, exploration.SidetrackCostProfile?.Id),
            GAndGAdminCost = MapToDto<GAndGAdminCost, GAndGAdminCostDto>(exploration.GAndGAdminCost, exploration.GAndGAdminCost?.Id),
            SeismicAcquisitionAndProcessing = MapToDto<SeismicAcquisitionAndProcessing, SeismicAcquisitionAndProcessingDto>(exploration.SeismicAcquisitionAndProcessing, exploration.SeismicAcquisitionAndProcessing?.Id),
            CountryOfficeCost = MapToDto<CountryOfficeCost, CountryOfficeCostDto>(exploration.CountryOfficeCost, exploration.CountryOfficeCost?.Id),

            WellProject = _mapperService.MapToDto<WellProject, WellProjectDto>(wellProject, wellProject.Id),
            WellProjectWells = wellProject.WellProjectWells?.Select(w => _mapperService.MapToDto<WellProjectWell, WellProjectWellDto>(w, w.WellProjectId)).ToList() ?? new List<WellProjectWellDto>(),
            OilProducerCostProfile = MapToDto<OilProducerCostProfile, OilProducerCostProfileDto>(wellProject.OilProducerCostProfile, wellProject.OilProducerCostProfile?.Id),
            OilProducerCostProfileOverride = MapToDto<OilProducerCostProfileOverride, OilProducerCostProfileOverrideDto>(wellProject.OilProducerCostProfileOverride, wellProject.OilProducerCostProfileOverride?.Id),
            GasProducerCostProfile = MapToDto<GasProducerCostProfile, GasProducerCostProfileDto>(wellProject.GasProducerCostProfile, wellProject.GasProducerCostProfile?.Id),
            GasProducerCostProfileOverride = MapToDto<GasProducerCostProfileOverride, GasProducerCostProfileOverrideDto>(wellProject.GasProducerCostProfileOverride, wellProject.GasProducerCostProfileOverride?.Id),
            WaterInjectorCostProfile = MapToDto<WaterInjectorCostProfile, WaterInjectorCostProfileDto>(wellProject.WaterInjectorCostProfile, wellProject.WaterInjectorCostProfile?.Id),
            WaterInjectorCostProfileOverride = MapToDto<WaterInjectorCostProfileOverride, WaterInjectorCostProfileOverrideDto>(wellProject.WaterInjectorCostProfileOverride, wellProject.WaterInjectorCostProfileOverride?.Id),
            GasInjectorCostProfile = MapToDto<GasInjectorCostProfile, GasInjectorCostProfileDto>(wellProject.GasInjectorCostProfile, wellProject.GasInjectorCostProfile?.Id),
            GasInjectorCostProfileOverride = MapToDto<GasInjectorCostProfileOverride, GasInjectorCostProfileOverrideDto>(wellProject.GasInjectorCostProfileOverride, wellProject.GasInjectorCostProfileOverride?.Id),
        };

        return caseWithAssetsDto;
    }

    private TDto? MapToDto<T, TDto>(T? source, Guid? id) where T : class where TDto : class
    {
        if (source == null || id == null)
        {
            return null;
        }
        return _mapperService.MapToDto<T, TDto>(source, (Guid)id);
    }

    private TDto? ConversionMapToDto<T, TDto>(T? source, Guid? id, PhysUnit physUnit) where T : class where TDto : class
    {
        if (source == null || id == null)
        {
            return null;
        }
        return _conversionMapperService.MapToDto<T, TDto>(source, (Guid)id, physUnit);
    }
}
