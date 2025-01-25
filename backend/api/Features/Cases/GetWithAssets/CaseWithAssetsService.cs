using api.Context;
using api.Context.Extensions;
using api.Features.Cases.GetWithAssets.Dtos;
using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.ProjectData.Dtos.AssetDtos;
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
            CessationWellsCost = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCost)),
            CessationWellsCostOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCostOverride)),
            CessationOffshoreFacilitiesCost = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCost)),
            CessationOffshoreFacilitiesCostOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCostOverride)),
            CessationOnshoreFacilitiesCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.CessationOnshoreFacilitiesCostProfile)),
            TotalFeasibilityAndConceptStudies = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudies)),
            TotalFeasibilityAndConceptStudiesOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudiesOverride)),
            TotalFEEDStudies = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudies)),
            TotalFEEDStudiesOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudiesOverride)),
            TotalOtherStudiesCostProfile = MapToDto<TotalOtherStudiesCostProfile, TimeSeriesCostDto>(caseItem.TotalOtherStudiesCostProfile, caseItem.TotalOtherStudiesCostProfile?.Id),
            HistoricCostCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.HistoricCostCostProfile)),
            WellInterventionCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfile)),
            WellInterventionCostProfileOverride =  MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfileOverride)),
            OffshoreFacilitiesOperationsCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfile)),
            OffshoreFacilitiesOperationsCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride)),
            OnshoreRelatedOPEXCostProfile = MapToDto<OnshoreRelatedOPEXCostProfile, TimeSeriesCostDto>(caseItem.OnshoreRelatedOPEXCostProfile, caseItem.OnshoreRelatedOPEXCostProfile?.Id),
            AdditionalOPEXCostProfile = MapToDto<AdditionalOPEXCostProfile, TimeSeriesCostDto>(caseItem.AdditionalOPEXCostProfile, caseItem.AdditionalOPEXCostProfile?.Id),
            CalculatedTotalIncomeCostProfile = MapToDto<CalculatedTotalIncomeCostProfile, TimeSeriesCostDto>(caseItem.CalculatedTotalIncomeCostProfile, caseItem.CalculatedTotalIncomeCostProfile?.Id),
            CalculatedTotalCostCostProfile = MapToDto<CalculatedTotalCostCostProfile, TimeSeriesCostDto>(caseItem.CalculatedTotalCostCostProfile, caseItem.CalculatedTotalCostCostProfile?.Id),
            DrainageStrategy = conversionMapperService.MapToDto<DrainageStrategy, DrainageStrategyDto>(drainageStrategy, drainageStrategy.Id, project.PhysicalUnit),
            ProductionProfileOil = ConversionMapToDto<ProductionProfileOil, TimeSeriesVolumeDto>(drainageStrategy.ProductionProfileOil, drainageStrategy.ProductionProfileOil?.Id, project.PhysicalUnit),
            AdditionalProductionProfileOil = ConversionMapToDto<AdditionalProductionProfileOil, TimeSeriesVolumeDto>(drainageStrategy.AdditionalProductionProfileOil, drainageStrategy.AdditionalProductionProfileOil?.Id, project.PhysicalUnit),
            ProductionProfileGas = ConversionMapToDto<ProductionProfileGas, TimeSeriesVolumeDto>(drainageStrategy.ProductionProfileGas, drainageStrategy.ProductionProfileGas?.Id, project.PhysicalUnit),
            AdditionalProductionProfileGas = ConversionMapToDto<AdditionalProductionProfileGas, TimeSeriesVolumeDto>(drainageStrategy.AdditionalProductionProfileGas, drainageStrategy.AdditionalProductionProfileGas?.Id, project.PhysicalUnit),
            ProductionProfileWater = ConversionMapToDto<ProductionProfileWater, TimeSeriesVolumeDto>(drainageStrategy.ProductionProfileWater, drainageStrategy.ProductionProfileWater?.Id, project.PhysicalUnit),
            ProductionProfileWaterInjection = ConversionMapToDto<ProductionProfileWaterInjection, TimeSeriesVolumeDto>(drainageStrategy.ProductionProfileWaterInjection, drainageStrategy.ProductionProfileWaterInjection?.Id, project.PhysicalUnit),
            FuelFlaringAndLosses = ConversionMapToDto<FuelFlaringAndLosses, TimeSeriesVolumeDto>(drainageStrategy.FuelFlaringAndLosses, drainageStrategy.FuelFlaringAndLosses?.Id, project.PhysicalUnit),
            FuelFlaringAndLossesOverride = ConversionMapToDto<FuelFlaringAndLossesOverride, TimeSeriesVolumeOverrideDto>(drainageStrategy.FuelFlaringAndLossesOverride, drainageStrategy.FuelFlaringAndLossesOverride?.Id, project.PhysicalUnit),
            NetSalesGas = ConversionMapToDto<NetSalesGas, TimeSeriesVolumeDto>(drainageStrategy.NetSalesGas, drainageStrategy.NetSalesGas?.Id, project.PhysicalUnit),
            NetSalesGasOverride = ConversionMapToDto<NetSalesGasOverride, TimeSeriesVolumeOverrideDto>(drainageStrategy.NetSalesGasOverride, drainageStrategy.NetSalesGasOverride?.Id, project.PhysicalUnit),
            Co2Emissions = ConversionMapToDto<Co2Emissions, TimeSeriesMassDto>(drainageStrategy.Co2Emissions, drainageStrategy.Co2Emissions?.Id, project.PhysicalUnit),
            Co2EmissionsOverride = ConversionMapToDto<Co2EmissionsOverride, TimeSeriesMassOverrideDto>(drainageStrategy.Co2EmissionsOverride, drainageStrategy.Co2EmissionsOverride?.Id, project.PhysicalUnit),
            Co2Intensity = ConversionMapToDto<Co2Intensity, TimeSeriesMassDto>(drainageStrategy.Co2Intensity, drainageStrategy.Co2Intensity?.Id, project.PhysicalUnit),
            ProductionProfileNgl = ConversionMapToDto<ProductionProfileNgl, TimeSeriesVolumeDto>(drainageStrategy.ProductionProfileNgl, drainageStrategy.ProductionProfileNgl?.Id, project.PhysicalUnit),
            ImportedElectricity = ConversionMapToDto<ImportedElectricity, TimeSeriesEnergyDto>(drainageStrategy.ImportedElectricity, drainageStrategy.ImportedElectricity?.Id, project.PhysicalUnit),
            ImportedElectricityOverride = ConversionMapToDto<ImportedElectricityOverride, TimeSeriesEnergyOverrideDto>(drainageStrategy.ImportedElectricityOverride, drainageStrategy.ImportedElectricityOverride?.Id, project.PhysicalUnit),
            DeferredOilProduction = ConversionMapToDto<DeferredOilProduction, TimeSeriesVolumeDto>(drainageStrategy.DeferredOilProduction, drainageStrategy.DeferredOilProduction?.Id, project.PhysicalUnit),
            DeferredGasProduction = ConversionMapToDto<DeferredGasProduction, TimeSeriesVolumeDto>(drainageStrategy.DeferredGasProduction, drainageStrategy.DeferredGasProduction?.Id, project.PhysicalUnit),
            Topside = mapperService.MapToDto<Topside, TopsideDto>(topside, topside.Id),
            TopsideCostProfile = MapToDto<TopsideCostProfile, TimeSeriesCostDto>(topside.CostProfile, topside.CostProfile?.Id),
            TopsideCostProfileOverride = MapToDto<TopsideCostProfileOverride, TimeSeriesCostOverrideDto>(topside.CostProfileOverride, topside.CostProfileOverride?.Id),
            TopsideCessationCostProfile = MapToDto<TopsideCessationCostProfile, TimeSeriesCostDto>(topside.CessationCostProfile, topside.CessationCostProfile?.Id),
            Substructure = mapperService.MapToDto<Substructure, SubstructureDto>(substructure, substructure.Id),
            SubstructureCostProfile = MapToDto<SubstructureCostProfile, TimeSeriesCostDto>(substructure.CostProfile, substructure.CostProfile?.Id),
            SubstructureCostProfileOverride = MapToDto<SubstructureCostProfileOverride, TimeSeriesCostOverrideDto>(substructure.CostProfileOverride, substructure.CostProfileOverride?.Id),
            SubstructureCessationCostProfile = MapToDto<SubstructureCessationCostProfile, TimeSeriesCostDto>(substructure.CessationCostProfile, substructure.CessationCostProfile?.Id),
            Surf = mapperService.MapToDto<Surf, SurfDto>(surf, surf.Id),
            SurfCostProfile = MapToDto<SurfCostProfile, TimeSeriesCostDto>(surf.CostProfile, surf.CostProfile?.Id),
            SurfCostProfileOverride = MapToDto<SurfCostProfileOverride, TimeSeriesCostOverrideDto>(surf.CostProfileOverride, surf.CostProfileOverride?.Id),
            SurfCessationCostProfile = MapToDto<SurfCessationCostProfile, TimeSeriesCostDto>(surf.CessationCostProfile, surf.CessationCostProfile?.Id),
            Transport = mapperService.MapToDto<Transport, TransportDto>(transport, transport.Id),
            TransportCostProfile = MapToDto<TransportCostProfile, TimeSeriesCostDto>(transport.CostProfile, transport.CostProfile?.Id),
            TransportCostProfileOverride = MapToDto<TransportCostProfileOverride, TimeSeriesCostOverrideDto>(transport.CostProfileOverride, transport.CostProfileOverride?.Id),
            TransportCessationCostProfile = MapToDto<TransportCessationCostProfile, TimeSeriesCostDto>(transport.CessationCostProfile, transport.CessationCostProfile?.Id),
            OnshorePowerSupply = mapperService.MapToDto<OnshorePowerSupply, OnshorePowerSupplyDto>(onshorePowerSupply, onshorePowerSupply.Id),
            OnshorePowerSupplyCostProfile = MapToDto<OnshorePowerSupplyCostProfile, TimeSeriesCostDto>(onshorePowerSupply.CostProfile, onshorePowerSupply.CostProfile?.Id),
            OnshorePowerSupplyCostProfileOverride = MapToDto<OnshorePowerSupplyCostProfileOverride, TimeSeriesCostOverrideDto>(onshorePowerSupply.CostProfileOverride, onshorePowerSupply.CostProfileOverride?.Id),
            Exploration = mapperService.MapToDto<Exploration, ExplorationDto>(exploration, exploration.Id),
            ExplorationWells = exploration.ExplorationWells.Select(w => mapperService.MapToDto<ExplorationWell, ExplorationWellDto>(w, w.ExplorationId)).ToList(),
            ExplorationWellCostProfile = MapToDto<ExplorationWellCostProfile, TimeSeriesCostDto>(exploration.ExplorationWellCostProfile, exploration.ExplorationWellCostProfile?.Id),
            AppraisalWellCostProfile = MapToDto<AppraisalWellCostProfile, TimeSeriesCostDto>(exploration.AppraisalWellCostProfile, exploration.AppraisalWellCostProfile?.Id),
            SidetrackCostProfile = MapToDto<SidetrackCostProfile, TimeSeriesCostDto>(exploration.SidetrackCostProfile, exploration.SidetrackCostProfile?.Id),
            GAndGAdminCost = MapToDto<GAndGAdminCost, TimeSeriesCostDto>(exploration.GAndGAdminCost, exploration.GAndGAdminCost?.Id),
            GAndGAdminCostOverride = MapToDto<GAndGAdminCostOverride, TimeSeriesCostOverrideDto>(exploration.GAndGAdminCostOverride, exploration.GAndGAdminCostOverride?.Id),
            SeismicAcquisitionAndProcessing = MapToDto<SeismicAcquisitionAndProcessing, TimeSeriesCostDto>(exploration.SeismicAcquisitionAndProcessing, exploration.SeismicAcquisitionAndProcessing?.Id),
            CountryOfficeCost = MapToDto<CountryOfficeCost, TimeSeriesCostDto>(exploration.CountryOfficeCost, exploration.CountryOfficeCost?.Id),
            WellProject = mapperService.MapToDto<WellProject, WellProjectDto>(wellProject, wellProject.Id),
            WellProjectWells = wellProject.WellProjectWells.Select(w => mapperService.MapToDto<WellProjectWell, WellProjectWellDto>(w, w.WellProjectId)).ToList(),
            OilProducerCostProfile = MapToDto<OilProducerCostProfile, TimeSeriesCostDto>(wellProject.OilProducerCostProfile, wellProject.OilProducerCostProfile?.Id),
            OilProducerCostProfileOverride = MapToDto<OilProducerCostProfileOverride, TimeSeriesCostOverrideDto>(wellProject.OilProducerCostProfileOverride, wellProject.OilProducerCostProfileOverride?.Id),
            GasProducerCostProfile = MapToDto<GasProducerCostProfile, TimeSeriesCostDto>(wellProject.GasProducerCostProfile, wellProject.GasProducerCostProfile?.Id),
            GasProducerCostProfileOverride = MapToDto<GasProducerCostProfileOverride, TimeSeriesCostOverrideDto>(wellProject.GasProducerCostProfileOverride, wellProject.GasProducerCostProfileOverride?.Id),
            WaterInjectorCostProfile = MapToDto<WaterInjectorCostProfile, TimeSeriesCostDto>(wellProject.WaterInjectorCostProfile, wellProject.WaterInjectorCostProfile?.Id),
            WaterInjectorCostProfileOverride = MapToDto<WaterInjectorCostProfileOverride, TimeSeriesCostOverrideDto>(wellProject.WaterInjectorCostProfileOverride, wellProject.WaterInjectorCostProfileOverride?.Id),
            GasInjectorCostProfile = MapToDto<GasInjectorCostProfile, TimeSeriesCostDto>(wellProject.GasInjectorCostProfile, wellProject.GasInjectorCostProfile?.Id),
            GasInjectorCostProfileOverride = MapToDto<GasInjectorCostProfileOverride, TimeSeriesCostOverrideDto>(wellProject.GasInjectorCostProfileOverride, wellProject.GasInjectorCostProfileOverride?.Id)
        };
    }

    private static TimeSeriesCostDto? MapToDto(TimeSeriesProfile? timeSeriesProfile)
    {
        if (timeSeriesProfile == null)
        {
            return null;
        }

        return new TimeSeriesCostDto
        {
            Id = timeSeriesProfile.Id,
            StartYear = timeSeriesProfile.StartYear,
            Values = timeSeriesProfile.Values
        };
    }

    private static TimeSeriesCostOverrideDto? MapToOverrideDto(TimeSeriesProfile? timeSeriesProfile)
    {
        if (timeSeriesProfile == null)
        {
            return null;
        }

        return new TimeSeriesCostOverrideDto
        {
            Id = timeSeriesProfile.Id,
            StartYear = timeSeriesProfile.StartYear,
            Values = timeSeriesProfile.Values,
            Override = timeSeriesProfile.Override
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
            CreateTime = caseItem.CreatedUtc,
            ModifyTime = caseItem.UpdatedUtc,
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
