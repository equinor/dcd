using api.Context;
using api.Context.Extensions;
using api.Features.Assets.CaseAssets.DrainageStrategies.Dtos;
using api.Features.Assets.CaseAssets.Explorations.Dtos;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos;
using api.Features.Assets.CaseAssets.Substructures.Dtos;
using api.Features.Assets.CaseAssets.Surfs.Dtos;
using api.Features.Assets.CaseAssets.Topsides.Dtos;
using api.Features.Assets.CaseAssets.Transports.Dtos;
using api.Features.Assets.CaseAssets.WellProjects.Dtos;
using api.Features.Profiles.Cases.AdditionalOpexCostProfiles.Dtos;
using api.Features.Profiles.Cases.CessationOffshoreFacilitiesCostOverrides.Dtos;
using api.Features.Profiles.Cases.CessationOnshoreFacilitiesCostProfiles.Dtos;
using api.Features.Profiles.Cases.CessationWellsCostOverrides.Dtos;
using api.Features.Profiles.Cases.HistoricCostCostProfiles.Dtos;
using api.Features.Profiles.Cases.OffshoreFacilitiesOperationsCostProfileOverrides.Dtos;
using api.Features.Profiles.Cases.OnshoreRelatedOpexCostProfiles.Dtos;
using api.Features.Profiles.Cases.TotalFeasibilityAndConceptStudiesOverrides.Dtos;
using api.Features.Profiles.Cases.TotalFeedStudiesOverrides.Dtos;
using api.Features.Profiles.Cases.TotalOtherStudiesCostProfiles.Dtos;
using api.Features.Profiles.Cases.WellInterventionCostProfileOverrides.Dtos;
using api.Features.Profiles.DrainageStrategies.AdditionalProductionProfileGases.Dtos;
using api.Features.Profiles.DrainageStrategies.AdditionalProductionProfileOils.Dtos;
using api.Features.Profiles.DrainageStrategies.Co2EmissionsOverrides.Dtos;
using api.Features.Profiles.DrainageStrategies.DeferredGasProductions.Dtos;
using api.Features.Profiles.DrainageStrategies.DeferredOilProductions.Dtos;
using api.Features.Profiles.DrainageStrategies.FuelFlaringAndLossesOverrides.Dtos;
using api.Features.Profiles.DrainageStrategies.ImportedElectricityOverrides.Dtos;
using api.Features.Profiles.DrainageStrategies.NetSalesGasOverrides.Dtos;
using api.Features.Profiles.DrainageStrategies.ProductionProfileGases.Dtos;
using api.Features.Profiles.DrainageStrategies.ProductionProfileOils.Dtos;
using api.Features.Profiles.DrainageStrategies.ProductionProfileWaterInjections.Dtos;
using api.Features.Profiles.DrainageStrategies.ProductionProfileWaters.Dtos;
using api.Features.ProjectData.Dtos.AssetDtos;
using api.Features.Stea.Dtos;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.GetWithAssets;

public class CaseWithAssetsService(
    DcdDbContext context,
    CaseWithAssetsRepository caseWithAssetsRepository,
    IMapperService mapperService,
    IConversionMapperService conversionMapperService)
{
    public async Task<CaseWithAssetsDto> GetCaseWithAssetsNoTracking(Guid projectId, Guid caseId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectIdOrRevisionId(projectId);

        var project = await context.Projects.SingleAsync(p => p.Id == projectPk);

        var (caseItem, drainageStrategy, topside, exploration, substructure, surf, transport, onshorePowerSupply, wellProject) = await caseWithAssetsRepository.GetCaseWithAssetsNoTracking(caseId);

        return new CaseWithAssetsDto
        {
            Case = MapToCaseOverviewDto(caseItem),
            CessationWellsCost = MapToDto<CessationWellsCost, CessationWellsCostDto>(caseItem.CessationWellsCost, caseItem.CessationWellsCost?.Id),
            CessationWellsCostOverride = MapToDto<CessationWellsCostOverride, CessationWellsCostOverrideDto>(caseItem.CessationWellsCostOverride, caseItem.CessationWellsCostOverride?.Id),
            CessationOffshoreFacilitiesCost = MapToDto<CessationOffshoreFacilitiesCost, CessationOffshoreFacilitiesCostDto>(caseItem.CessationOffshoreFacilitiesCost, caseItem.CessationOffshoreFacilitiesCost?.Id),
            CessationOffshoreFacilitiesCostOverride = MapToDto<CessationOffshoreFacilitiesCostOverride, CessationOffshoreFacilitiesCostOverrideDto>(caseItem.CessationOffshoreFacilitiesCostOverride, caseItem.CessationOffshoreFacilitiesCostOverride?.Id),
            CessationOnshoreFacilitiesCostProfile = MapToDto<CessationOnshoreFacilitiesCostProfile, CessationOnshoreFacilitiesCostProfileDto>(caseItem.CessationOnshoreFacilitiesCostProfile, caseItem.CessationOnshoreFacilitiesCostProfile?.Id),
            TotalFeasibilityAndConceptStudies = MapToDto<TotalFeasibilityAndConceptStudies, TotalFeasibilityAndConceptStudiesDto>(caseItem.TotalFeasibilityAndConceptStudies, caseItem.TotalFeasibilityAndConceptStudies?.Id),
            TotalFeasibilityAndConceptStudiesOverride = MapToDto<TotalFeasibilityAndConceptStudiesOverride, TotalFeasibilityAndConceptStudiesOverrideDto>(caseItem.TotalFeasibilityAndConceptStudiesOverride, caseItem.TotalFeasibilityAndConceptStudiesOverride?.Id),
            TotalFEEDStudies = MapToDto<TotalFEEDStudies, TotalFEEDStudiesDto>(caseItem.TotalFEEDStudies, caseItem.TotalFEEDStudies?.Id),
            TotalFEEDStudiesOverride = MapToDto<TotalFEEDStudiesOverride, TotalFeedStudiesOverrideDto>(caseItem.TotalFEEDStudiesOverride, caseItem.TotalFEEDStudiesOverride?.Id),
            TotalOtherStudiesCostProfile = MapToDto<TotalOtherStudiesCostProfile, TotalOtherStudiesCostProfileDto>(caseItem.TotalOtherStudiesCostProfile, caseItem.TotalOtherStudiesCostProfile?.Id),
            HistoricCostCostProfile = MapToDto<HistoricCostCostProfile, HistoricCostCostProfileDto>(caseItem.HistoricCostCostProfile, caseItem.HistoricCostCostProfile?.Id),
            WellInterventionCostProfile = MapToDto<WellInterventionCostProfile, WellInterventionCostProfileDto>(caseItem.WellInterventionCostProfile, caseItem.WellInterventionCostProfile?.Id),
            WellInterventionCostProfileOverride = MapToDto<WellInterventionCostProfileOverride, WellInterventionCostProfileOverrideDto>(caseItem.WellInterventionCostProfileOverride, caseItem.WellInterventionCostProfileOverride?.Id),
            OffshoreFacilitiesOperationsCostProfile = MapToDto<OffshoreFacilitiesOperationsCostProfile, OffshoreFacilitiesOperationsCostProfileDto>(caseItem.OffshoreFacilitiesOperationsCostProfile, caseItem.OffshoreFacilitiesOperationsCostProfile?.Id),
            OffshoreFacilitiesOperationsCostProfileOverride = MapToDto<OffshoreFacilitiesOperationsCostProfileOverride, OffshoreFacilitiesOperationsCostProfileOverrideDto>(caseItem.OffshoreFacilitiesOperationsCostProfileOverride, caseItem.OffshoreFacilitiesOperationsCostProfileOverride?.Id),
            OnshoreRelatedOPEXCostProfile = MapToDto<OnshoreRelatedOPEXCostProfile, OnshoreRelatedOpexCostProfileDto>(caseItem.OnshoreRelatedOPEXCostProfile, caseItem.OnshoreRelatedOPEXCostProfile?.Id),
            AdditionalOPEXCostProfile = MapToDto<AdditionalOPEXCostProfile, AdditionalOpexCostProfileDto>(caseItem.AdditionalOPEXCostProfile, caseItem.AdditionalOPEXCostProfile?.Id),
            CalculatedTotalIncomeCostProfile = MapToDto<CalculatedTotalIncomeCostProfile, CalculatedTotalIncomeCostProfileDto>(caseItem.CalculatedTotalIncomeCostProfile, caseItem.CalculatedTotalIncomeCostProfile?.Id),
            CalculatedTotalCostCostProfile = MapToDto<CalculatedTotalCostCostProfile, CalculatedTotalCostCostProfileDto>(caseItem.CalculatedTotalCostCostProfile, caseItem.CalculatedTotalCostCostProfile?.Id),
            DrainageStrategy = conversionMapperService.MapToDto<DrainageStrategy, DrainageStrategyDto>(drainageStrategy, drainageStrategy.Id, project.PhysicalUnit),
            ProductionProfileOil = ConversionMapToDto<ProductionProfileOil, ProductionProfileOilDto>(drainageStrategy.ProductionProfileOil, drainageStrategy.ProductionProfileOil?.Id, project.PhysicalUnit),
            AdditionalProductionProfileOil = ConversionMapToDto<AdditionalProductionProfileOil, AdditionalProductionProfileOilDto>(drainageStrategy.AdditionalProductionProfileOil, drainageStrategy.AdditionalProductionProfileOil?.Id, project.PhysicalUnit),
            ProductionProfileGas = ConversionMapToDto<ProductionProfileGas, ProductionProfileGasDto>(drainageStrategy.ProductionProfileGas, drainageStrategy.ProductionProfileGas?.Id, project.PhysicalUnit),
            AdditionalProductionProfileGas = ConversionMapToDto<AdditionalProductionProfileGas, AdditionalProductionProfileGasDto>(drainageStrategy.AdditionalProductionProfileGas, drainageStrategy.AdditionalProductionProfileGas?.Id, project.PhysicalUnit),
            ProductionProfileWater = ConversionMapToDto<ProductionProfileWater, ProductionProfileWaterDto>(drainageStrategy.ProductionProfileWater, drainageStrategy.ProductionProfileWater?.Id, project.PhysicalUnit),
            ProductionProfileWaterInjection = ConversionMapToDto<ProductionProfileWaterInjection, ProductionProfileWaterInjectionDto>(drainageStrategy.ProductionProfileWaterInjection, drainageStrategy.ProductionProfileWaterInjection?.Id, project.PhysicalUnit),
            FuelFlaringAndLosses = ConversionMapToDto<FuelFlaringAndLosses, FuelFlaringAndLossesDto>(drainageStrategy.FuelFlaringAndLosses, drainageStrategy.FuelFlaringAndLosses?.Id, project.PhysicalUnit),
            FuelFlaringAndLossesOverride = ConversionMapToDto<FuelFlaringAndLossesOverride, FuelFlaringAndLossesOverrideDto>(drainageStrategy.FuelFlaringAndLossesOverride, drainageStrategy.FuelFlaringAndLossesOverride?.Id, project.PhysicalUnit),
            NetSalesGas = ConversionMapToDto<NetSalesGas, NetSalesGasDto>(drainageStrategy.NetSalesGas, drainageStrategy.NetSalesGas?.Id, project.PhysicalUnit),
            NetSalesGasOverride = ConversionMapToDto<NetSalesGasOverride, NetSalesGasOverrideDto>(drainageStrategy.NetSalesGasOverride, drainageStrategy.NetSalesGasOverride?.Id, project.PhysicalUnit),
            Co2Emissions = ConversionMapToDto<Co2Emissions, Co2EmissionsDto>(drainageStrategy.Co2Emissions, drainageStrategy.Co2Emissions?.Id, project.PhysicalUnit),
            Co2EmissionsOverride = ConversionMapToDto<Co2EmissionsOverride, Co2EmissionsOverrideDto>(drainageStrategy.Co2EmissionsOverride, drainageStrategy.Co2EmissionsOverride?.Id, project.PhysicalUnit),
            ProductionProfileNgl = ConversionMapToDto<ProductionProfileNgl, ProductionProfileNglDto>(drainageStrategy.ProductionProfileNgl, drainageStrategy.ProductionProfileNgl?.Id, project.PhysicalUnit),
            ImportedElectricity = ConversionMapToDto<ImportedElectricity, ImportedElectricityDto>(drainageStrategy.ImportedElectricity, drainageStrategy.ImportedElectricity?.Id, project.PhysicalUnit),
            ImportedElectricityOverride = ConversionMapToDto<ImportedElectricityOverride, ImportedElectricityOverrideDto>(drainageStrategy.ImportedElectricityOverride, drainageStrategy.ImportedElectricityOverride?.Id, project.PhysicalUnit),
            DeferredOilProduction = ConversionMapToDto<DeferredOilProduction, DeferredOilProductionDto>(drainageStrategy.DeferredOilProduction, drainageStrategy.DeferredOilProduction?.Id, project.PhysicalUnit),
            DeferredGasProduction = ConversionMapToDto<DeferredGasProduction, DeferredGasProductionDto>(drainageStrategy.DeferredGasProduction, drainageStrategy.DeferredGasProduction?.Id, project.PhysicalUnit),
            Topside = mapperService.MapToDto<Topside, TopsideDto>(topside, topside.Id),
            TopsideCostProfile = MapToDto<TopsideCostProfile, TopsideCostProfileDto>(topside.CostProfile, topside.CostProfile?.Id),
            TopsideCostProfileOverride = MapToDto<TopsideCostProfileOverride, TopsideCostProfileOverrideDto>(topside.CostProfileOverride, topside.CostProfileOverride?.Id),
            TopsideCessationCostProfile = MapToDto<TopsideCessationCostProfile, TopsideCessationCostProfileDto>(topside.CessationCostProfile, topside.CessationCostProfile?.Id),
            Substructure = mapperService.MapToDto<Substructure, SubstructureDto>(substructure, substructure.Id),
            SubstructureCostProfile = MapToDto<SubstructureCostProfile, SubstructureCostProfileDto>(substructure.CostProfile, substructure.CostProfile?.Id),
            SubstructureCostProfileOverride = MapToDto<SubstructureCostProfileOverride, SubstructureCostProfileOverrideDto>(substructure.CostProfileOverride, substructure.CostProfileOverride?.Id),
            SubstructureCessationCostProfile = MapToDto<SubstructureCessationCostProfile, SubstructureCessationCostProfileDto>(substructure.CessationCostProfile, substructure.CessationCostProfile?.Id),
            Surf = mapperService.MapToDto<Surf, SurfDto>(surf, surf.Id),
            SurfCostProfile = MapToDto<SurfCostProfile, SurfCostProfileDto>(surf.CostProfile, surf.CostProfile?.Id),
            SurfCostProfileOverride = MapToDto<SurfCostProfileOverride, SurfCostProfileOverrideDto>(surf.CostProfileOverride, surf.CostProfileOverride?.Id),
            SurfCessationCostProfile = MapToDto<SurfCessationCostProfile, SurfCessationCostProfileDto>(surf.CessationCostProfile, surf.CessationCostProfile?.Id),
            Transport = mapperService.MapToDto<Transport, TransportDto>(transport, transport.Id),
            TransportCostProfile = MapToDto<TransportCostProfile, TransportCostProfileDto>(transport.CostProfile, transport.CostProfile?.Id),
            TransportCostProfileOverride = MapToDto<TransportCostProfileOverride, TransportCostProfileOverrideDto>(transport.CostProfileOverride, transport.CostProfileOverride?.Id),
            TransportCessationCostProfile = MapToDto<TransportCessationCostProfile, TransportCessationCostProfileDto>(transport.CessationCostProfile, transport.CessationCostProfile?.Id),
            OnshorePowerSupply = mapperService.MapToDto<OnshorePowerSupply, OnshorePowerSupplyDto>(onshorePowerSupply, onshorePowerSupply.Id),
            OnshorePowerSupplyCostProfile = MapToDto<OnshorePowerSupplyCostProfile, OnshorePowerSupplyCostProfileDto>(onshorePowerSupply.CostProfile, onshorePowerSupply.CostProfile?.Id),
            OnshorePowerSupplyCostProfileOverride = MapToDto<OnshorePowerSupplyCostProfileOverride, OnshorePowerSupplyCostProfileOverrideDto>(onshorePowerSupply.CostProfileOverride, onshorePowerSupply.CostProfileOverride?.Id),
            Exploration = mapperService.MapToDto<Exploration, ExplorationDto>(exploration, exploration.Id),
            ExplorationWells = exploration.ExplorationWells.Select(w => mapperService.MapToDto<ExplorationWell, ExplorationWellDto>(w, w.ExplorationId)).ToList(),
            ExplorationWellCostProfile = MapToDto<ExplorationWellCostProfile, ExplorationWellCostProfileDto>(exploration.ExplorationWellCostProfile, exploration.ExplorationWellCostProfile?.Id),
            AppraisalWellCostProfile = MapToDto<AppraisalWellCostProfile, AppraisalWellCostProfileDto>(exploration.AppraisalWellCostProfile, exploration.AppraisalWellCostProfile?.Id),
            SidetrackCostProfile = MapToDto<SidetrackCostProfile, SidetrackCostProfileDto>(exploration.SidetrackCostProfile, exploration.SidetrackCostProfile?.Id),
            GAndGAdminCost = MapToDto<GAndGAdminCost, GAndGAdminCostDto>(exploration.GAndGAdminCost, exploration.GAndGAdminCost?.Id),
            GAndGAdminCostOverride = MapToDto<GAndGAdminCostOverride, GAndGAdminCostOverrideDto>(exploration.GAndGAdminCostOverride, exploration.GAndGAdminCostOverride?.Id),
            SeismicAcquisitionAndProcessing = MapToDto<SeismicAcquisitionAndProcessing, SeismicAcquisitionAndProcessingDto>(exploration.SeismicAcquisitionAndProcessing, exploration.SeismicAcquisitionAndProcessing?.Id),
            CountryOfficeCost = MapToDto<CountryOfficeCost, CountryOfficeCostDto>(exploration.CountryOfficeCost, exploration.CountryOfficeCost?.Id),
            WellProject = mapperService.MapToDto<WellProject, WellProjectDto>(wellProject, wellProject.Id),
            WellProjectWells = wellProject.WellProjectWells.Select(w => mapperService.MapToDto<WellProjectWell, WellProjectWellDto>(w, w.WellProjectId)).ToList(),
            OilProducerCostProfile = MapToDto<OilProducerCostProfile, OilProducerCostProfileDto>(wellProject.OilProducerCostProfile, wellProject.OilProducerCostProfile?.Id),
            OilProducerCostProfileOverride = MapToDto<OilProducerCostProfileOverride, OilProducerCostProfileOverrideDto>(wellProject.OilProducerCostProfileOverride, wellProject.OilProducerCostProfileOverride?.Id),
            GasProducerCostProfile = MapToDto<GasProducerCostProfile, GasProducerCostProfileDto>(wellProject.GasProducerCostProfile, wellProject.GasProducerCostProfile?.Id),
            GasProducerCostProfileOverride = MapToDto<GasProducerCostProfileOverride, GasProducerCostProfileOverrideDto>(wellProject.GasProducerCostProfileOverride, wellProject.GasProducerCostProfileOverride?.Id),
            WaterInjectorCostProfile = MapToDto<WaterInjectorCostProfile, WaterInjectorCostProfileDto>(wellProject.WaterInjectorCostProfile, wellProject.WaterInjectorCostProfile?.Id),
            WaterInjectorCostProfileOverride = MapToDto<WaterInjectorCostProfileOverride, WaterInjectorCostProfileOverrideDto>(wellProject.WaterInjectorCostProfileOverride, wellProject.WaterInjectorCostProfileOverride?.Id),
            GasInjectorCostProfile = MapToDto<GasInjectorCostProfile, GasInjectorCostProfileDto>(wellProject.GasInjectorCostProfile, wellProject.GasInjectorCostProfile?.Id),
            GasInjectorCostProfileOverride = MapToDto<GasInjectorCostProfileOverride, GasInjectorCostProfileOverrideDto>(wellProject.GasInjectorCostProfileOverride, wellProject.GasInjectorCostProfileOverride?.Id)
        };
    }

    private static CaseOverviewDto MapToCaseOverviewDto(Case caseItem)
    {
        return new CaseOverviewDto
        {
            CaseId = caseItem.Id,
            ProjectId = caseItem.ProjectId,
            Name = caseItem.Name,
            Description = caseItem.Description,
            Archived = caseItem.Archived,
            ReferenceCase = caseItem.ReferenceCase,
            ProductionStrategyOverview = caseItem.ProductionStrategyOverview,
            ArtificialLift = caseItem.ArtificialLift,
            ProducerCount = caseItem.ProducerCount,
            GasInjectorCount = caseItem.GasInjectorCount,
            WaterInjectorCount = caseItem.WaterInjectorCount,
            NPV = caseItem.NPV,
            NPVOverride = caseItem.NPVOverride,
            BreakEven = caseItem.BreakEven,
            BreakEvenOverride = caseItem.BreakEvenOverride,
            FacilitiesAvailability = caseItem.FacilitiesAvailability,
            CapexFactorFeasibilityStudies = caseItem.CapexFactorFeasibilityStudies,
            CapexFactorFEEDStudies = caseItem.CapexFactorFEEDStudies,
            Host = caseItem.Host,
            DGADate = caseItem.DGADate,
            DGBDate = caseItem.DGBDate,
            DGCDate = caseItem.DGCDate,
            APBODate = caseItem.APBODate,
            BORDate = caseItem.BORDate,
            VPBODate = caseItem.VPBODate,
            DG0Date = caseItem.DG0Date,
            DG1Date = caseItem.DG1Date,
            DG2Date = caseItem.DG2Date,
            DG3Date = caseItem.DG3Date,
            DG4Date = caseItem.DG4Date,
            CreateTime = caseItem.CreateTime,
            ModifyTime = caseItem.ModifyTime,
            SurfLink = caseItem.SurfLink,
            SubstructureLink = caseItem.SubstructureLink,
            TopsideLink = caseItem.TopsideLink,
            TransportLink = caseItem.TransportLink,
            OnshorePowerSupplyLink = caseItem.OnshorePowerSupplyLink,
            SharepointFileId = caseItem.SharepointFileId,
            SharepointFileName = caseItem.SharepointFileName,
            SharepointFileUrl = caseItem.SharepointFileUrl
        };
    }

    private TDto? MapToDto<T, TDto>(T? source, Guid? id) where T : class where TDto : class
    {
        if (source == null || id == null)
        {
            return null;
        }

        return mapperService.MapToDto<T, TDto>(source, (Guid)id);
    }

    private TDto? ConversionMapToDto<T, TDto>(T? source, Guid? id, PhysUnit physUnit) where T : class where TDto : class
    {
        if (source == null || id == null)
        {
            return null;
        }

        return conversionMapperService.MapToDto<T, TDto>(source, (Guid)id, physUnit);
    }
}
