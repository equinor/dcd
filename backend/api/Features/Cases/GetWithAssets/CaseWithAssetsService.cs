using api.Context;
using api.Context.Extensions;
using api.Features.Cases.GetWithAssets.AssetMappers;
using api.Features.Cases.GetWithAssets.Dtos;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.GetWithAssets;

public class CaseWithAssetsService(DcdDbContext context, CaseWithAssetsRepository caseWithAssetsRepository)
{
    public async Task<CaseWithAssetsDto> GetCaseWithAssetsNoTracking(Guid projectId, Guid caseId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectIdOrRevisionId(projectId);

        var project = await context.Projects.SingleAsync(p => p.Id == projectPk);

        var (caseItem, drainageStrategy, topside, exploration, substructure, surf, transport, onshorePowerSupply, wellProject) = await caseWithAssetsRepository.GetCaseWithAssetsNoTracking(caseId);

        return new CaseWithAssetsDto
        {
            Case = CaseMapper.MapToDto(caseItem),
            CessationWellsCost = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCost)),
            CessationWellsCostOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCostOverride)),
            CessationOffshoreFacilitiesCost = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCost)),
            CessationOffshoreFacilitiesCostOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCostOverride)),
            CessationOnshoreFacilitiesCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.CessationOnshoreFacilitiesCostProfile)),
            TotalFeasibilityAndConceptStudies = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudies)),
            TotalFeasibilityAndConceptStudiesOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudiesOverride)),
            TotalFEEDStudies = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudies)),
            TotalFEEDStudiesOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudiesOverride)),
            TotalOtherStudiesCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.TotalOtherStudiesCostProfile)),
            HistoricCostCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.HistoricCostCostProfile)),
            WellInterventionCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfile)),
            WellInterventionCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfileOverride)),
            OffshoreFacilitiesOperationsCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfile)),
            OffshoreFacilitiesOperationsCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride)),
            OnshoreRelatedOPEXCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.OnshoreRelatedOPEXCostProfile)),
            AdditionalOPEXCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.AdditionalOPEXCostProfile)),
            CalculatedTotalIncomeCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalIncomeCostProfile)),
            CalculatedTotalCostCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalCostCostProfile)),
            DrainageStrategy = DrainageStrategyMapper.MapToDto(drainageStrategy),
            ProductionProfileOil = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil), ProfileTypes.ProductionProfileOil, project.PhysicalUnit),
            AdditionalProductionProfileOil = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil), ProfileTypes.AdditionalProductionProfileOil, project.PhysicalUnit),
            ProductionProfileGas = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas), ProfileTypes.ProductionProfileGas, project.PhysicalUnit),
            AdditionalProductionProfileGas = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas), ProfileTypes.AdditionalProductionProfileGas, project.PhysicalUnit),
            ProductionProfileWater = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileWater), ProfileTypes.ProductionProfileWater, project.PhysicalUnit),
            ProductionProfileWaterInjection = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileWaterInjection), ProfileTypes.ProductionProfileWaterInjection, project.PhysicalUnit),
            FuelFlaringAndLosses = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.FuelFlaringAndLosses), ProfileTypes.FuelFlaringAndLosses, project.PhysicalUnit),
            FuelFlaringAndLossesOverride = ConversionMapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.FuelFlaringAndLossesOverride), ProfileTypes.FuelFlaringAndLossesOverride, project.PhysicalUnit),
            NetSalesGas = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.NetSalesGas), ProfileTypes.NetSalesGas, project.PhysicalUnit),
            NetSalesGasOverride = ConversionMapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.NetSalesGasOverride), ProfileTypes.NetSalesGasOverride, project.PhysicalUnit),
            Co2Emissions = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.Co2Emissions), ProfileTypes.Co2Emissions, project.PhysicalUnit),
            Co2EmissionsOverride = ConversionMapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.Co2EmissionsOverride), ProfileTypes.Co2EmissionsOverride, project.PhysicalUnit),
            Co2Intensity = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.Co2Intensity), ProfileTypes.Co2Intensity, project.PhysicalUnit),
            ProductionProfileNgl = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileNgl), ProfileTypes.ProductionProfileNgl, project.PhysicalUnit),
            ImportedElectricity = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.ImportedElectricity), ProfileTypes.ImportedElectricity, project.PhysicalUnit),
            ImportedElectricityOverride = ConversionMapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.ImportedElectricityOverride), ProfileTypes.ImportedElectricityOverride, project.PhysicalUnit),
            DeferredOilProduction = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.DeferredOilProduction), ProfileTypes.DeferredOilProduction, project.PhysicalUnit),
            DeferredGasProduction = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.DeferredGasProduction), ProfileTypes.DeferredGasProduction, project.PhysicalUnit),
            Topside = TopsideMapper.MapToDto(topside),
            TopsideCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfile)),
            TopsideCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfileOverride)),
            TopsideCessationCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.TopsideCessationCostProfile)),
            Substructure = SubstructureMapper.MapToDto(substructure),
            SubstructureCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfile)),
            SubstructureCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfileOverride)),
            SubstructureCessationCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.SubstructureCessationCostProfile)),
            Surf = SurfMapper.MapToDto(surf),
            SurfCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfile)),
            SurfCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfileOverride)),
            SurfCessationCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.SurfCessationCostProfile)),
            Transport = TransportMapper.MapToDto(transport),
            TransportCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfile)),
            TransportCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfileOverride)),
            TransportCessationCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.TransportCessationCostProfile)),
            OnshorePowerSupply = OnshorePowerSupplyMapper.MapToDto(onshorePowerSupply),
            OnshorePowerSupplyCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfile)),
            OnshorePowerSupplyCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfileOverride)),
            Exploration = ExplorationMapper.MapToDto(exploration),
            ExplorationWells = ExplorationWellsMapper.MapToDtos(exploration.ExplorationWells),
            ExplorationWellCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.ExplorationWellCostProfile)),
            AppraisalWellCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.AppraisalWellCostProfile)),
            SidetrackCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.SidetrackCostProfile)),
            GAndGAdminCost = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCost)),
            GAndGAdminCostOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCostOverride)),
            SeismicAcquisitionAndProcessing = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.SeismicAcquisitionAndProcessing)),
            CountryOfficeCost = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.CountryOfficeCost)),
            WellProject = WellProjectMapper.MapToDto(wellProject),
            WellProjectWells = WellProjectWellsMapper.MapToDtos(wellProject.WellProjectWells),
            OilProducerCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.OilProducerCostProfile)),
            OilProducerCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.OilProducerCostProfileOverride)),
            GasProducerCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.GasProducerCostProfile)),
            GasProducerCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.GasProducerCostProfileOverride)),
            WaterInjectorCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.WaterInjectorCostProfile)),
            WaterInjectorCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.WaterInjectorCostProfileOverride)),
            GasInjectorCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.GasInjectorCostProfile)),
            GasInjectorCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.GasInjectorCostProfileOverride))
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

    private static TimeSeriesCostDto? ConversionMapToDto(TimeSeriesProfile? entity, string profileType, PhysUnit physUnit)
    {
        if (entity == null)
        {
            return null;
        }

        return new TimeSeriesCostDto
        {
            Id = entity.Id,
            StartYear = entity.StartYear,
            Values = UnitConversionHelpers.ConvertValuesToDto(entity.Values, physUnit, profileType)
        };
    }

    private static TimeSeriesCostOverrideDto? ConversionMapToOverrideDto(TimeSeriesProfile? entity, string profileType, PhysUnit physUnit)
    {
        if (entity == null)
        {
            return null;
        }

        return new TimeSeriesCostOverrideDto
        {
            Id = entity.Id,
            StartYear = entity.StartYear,
            Override = entity.Override,
            Values = UnitConversionHelpers.ConvertValuesToDto(entity.Values, physUnit, profileType)
        };
    }
}
