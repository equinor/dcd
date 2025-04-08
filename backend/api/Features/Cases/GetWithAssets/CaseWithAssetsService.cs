using api.Context;
using api.Context.Extensions;
using api.Features.Cases.GetWithAssets.AssetMappers;
using api.Features.Cases.GetWithAssets.Dtos;
using api.Features.Profiles;
using api.Models;
using api.Models.Enums;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Cases.GetWithAssets;

public class CaseWithAssetsService(DcdDbContext context, CaseWithAssetsRepository caseWithAssetsRepository)
{
    public async Task<CaseWithAssetsDto> GetCaseWithAssets(Guid projectId, Guid caseId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectIdOrRevisionId(projectId);
        var projectData = await context.Projects
            .Where(x => x.Id == projectPk)
            .Select(x => new{ x.PhysicalUnit, x.Currency, x.ExchangeRateUsdToNok})
            .SingleAsync();

        var physicalUnit = projectData.PhysicalUnit;
        var currency = projectData.Currency;
        var usdToNok = projectData.ExchangeRateUsdToNok;

        var caseItem = await caseWithAssetsRepository.GetCaseWithAssets(projectPk, caseId);

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
            TotalFeedStudies = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.TotalFeedStudies)),
            TotalFeedStudiesOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.TotalFeedStudiesOverride)),
            TotalOtherStudiesCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.TotalOtherStudiesCostProfile)),
            HistoricCostCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.HistoricCostCostProfile)),
            WellInterventionCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfile)),
            WellInterventionCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.WellInterventionCostProfileOverride)),
            OffshoreFacilitiesOperationsCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfile)),
            OffshoreFacilitiesOperationsCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride)),
            OnshoreRelatedOpexCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.OnshoreRelatedOpexCostProfile)),
            AdditionalOpexCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.AdditionalOpexCostProfile)),
            CalculatedTotalIncomeCostProfile = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalIncomeCostProfile),ProfileTypes.CalculatedTotalIncomeCostProfile, physicalUnit, currency, usdToNok),
            CalculatedTotalCostCostProfile = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalCostCostProfile),ProfileTypes.CalculatedTotalCostCostProfile, physicalUnit, currency, usdToNok),
            CalculatedTotalCashflow = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalCashflow), ProfileTypes.CalculatedTotalCashflow, physicalUnit, currency, usdToNok),
            CalculatedDiscountedCashflow = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.CalculatedDiscountedCashflow), ProfileTypes.CalculatedDiscountedCashflow, physicalUnit, currency, usdToNok),
            CalculatedTotalOilIncomeCostProfile = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalOilIncomeCostProfile), ProfileTypes.CalculatedTotalOilIncomeCostProfile, physicalUnit, currency, usdToNok),
            CalculatedTotalGasIncomeCostProfile = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalGasIncomeCostProfile), ProfileTypes.CalculatedTotalGasIncomeCostProfile, physicalUnit, currency, usdToNok),

            DrainageStrategy = DrainageStrategyMapper.MapToDto(caseItem.DrainageStrategy),
            ProductionProfileOil = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil), ProfileTypes.ProductionProfileOil, physicalUnit, currency, usdToNok),
            AdditionalProductionProfileOil = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil), ProfileTypes.AdditionalProductionProfileOil, physicalUnit, currency, usdToNok),
            ProductionProfileGas = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas), ProfileTypes.ProductionProfileGas, physicalUnit, currency, usdToNok),
            AdditionalProductionProfileGas = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas), ProfileTypes.AdditionalProductionProfileGas, physicalUnit, currency, usdToNok),
            ProductionProfileWater = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileWater), ProfileTypes.ProductionProfileWater, physicalUnit, currency, usdToNok),
            ProductionProfileWaterInjection = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileWaterInjection), ProfileTypes.ProductionProfileWaterInjection, physicalUnit, currency, usdToNok),
            FuelFlaringAndLosses = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.FuelFlaringAndLosses), ProfileTypes.FuelFlaringAndLosses, physicalUnit, currency, usdToNok),
            FuelFlaringAndLossesOverride = ConversionMapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.FuelFlaringAndLossesOverride), ProfileTypes.FuelFlaringAndLossesOverride, physicalUnit, currency, usdToNok),
            NetSalesGas = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.NetSalesGas), ProfileTypes.NetSalesGas, physicalUnit, currency, usdToNok),
            NetSalesGasOverride = ConversionMapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.NetSalesGasOverride), ProfileTypes.NetSalesGasOverride, physicalUnit, currency, usdToNok),
            TotalExportedVolumes = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.TotalExportedVolumes), ProfileTypes.TotalExportedVolumes, physicalUnit, currency, usdToNok),
            TotalExportedVolumesOverride = ConversionMapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.TotalExportedVolumesOverride), ProfileTypes.TotalExportedVolumesOverride, physicalUnit, currency, usdToNok),
            Co2Emissions = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.Co2Emissions), ProfileTypes.Co2Emissions, physicalUnit, currency, usdToNok),
            Co2EmissionsOverride = ConversionMapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.Co2EmissionsOverride), ProfileTypes.Co2EmissionsOverride, physicalUnit, currency, usdToNok),
            Co2Intensity = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.Co2Intensity), ProfileTypes.Co2Intensity, physicalUnit, currency, usdToNok),
            Co2IntensityOverride = ConversionMapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.Co2IntensityOverride), ProfileTypes.Co2IntensityOverride, physicalUnit, currency, usdToNok),
            ProductionProfileNgl = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileNgl), ProfileTypes.ProductionProfileNgl, physicalUnit, currency, usdToNok),
            ProductionProfileNglOverride = ConversionMapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileNglOverride), ProfileTypes.ProductionProfileNglOverride, physicalUnit, currency, usdToNok),
            CondensateProduction = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.CondensateProduction), ProfileTypes.CondensateProduction, physicalUnit, currency, usdToNok),
            CondensateProductionOverride = ConversionMapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.CondensateProductionOverride), ProfileTypes.CondensateProductionOverride, physicalUnit, currency, usdToNok),
            ImportedElectricity = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.ImportedElectricity), ProfileTypes.ImportedElectricity, physicalUnit, currency, usdToNok),
            ImportedElectricityOverride = ConversionMapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.ImportedElectricityOverride), ProfileTypes.ImportedElectricityOverride, physicalUnit, currency, usdToNok),
            DeferredOilProduction = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.DeferredOilProduction), ProfileTypes.DeferredOilProduction, physicalUnit, currency, usdToNok),
            DeferredGasProduction = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.DeferredGasProduction), ProfileTypes.DeferredGasProduction, physicalUnit, currency, usdToNok),

            Topside = TopsideMapper.MapToDto(caseItem.Topside),
            TopsideCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfile)),
            TopsideCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfileOverride)),
            TopsideCessationCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.TopsideCessationCostProfile)),

            Substructure = SubstructureMapper.MapToDto(caseItem.Substructure),
            SubstructureCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfile)),
            SubstructureCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfileOverride)),
            SubstructureCessationCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.SubstructureCessationCostProfile)),

            Surf = SurfMapper.MapToDto(caseItem.Surf),
            SurfCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfile)),
            SurfCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfileOverride)),
            SurfCessationCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.SurfCessationCostProfile)),

            Transport = TransportMapper.MapToDto(caseItem.Transport),
            TransportCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfile)),
            TransportCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfileOverride)),
            TransportCessationCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.TransportCessationCostProfile)),

            OnshorePowerSupply = OnshorePowerSupplyMapper.MapToDto(caseItem.OnshorePowerSupply),
            OnshorePowerSupplyCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfile)),
            OnshorePowerSupplyCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfileOverride)),

            ExplorationWellCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.ExplorationWellCostProfile)),
            ExplorationWellCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.ExplorationWellCostProfileOverride)),
            AppraisalWellCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.AppraisalWellCostProfile)),
            AppraisalWellCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.AppraisalWellCostProfileOverride)),
            SidetrackCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.SidetrackCostProfile)),
            SidetrackCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.SidetrackCostProfileOverride)),
            GAndGAdminCost = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCost)),
            GAndGAdminCostOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCostOverride)),
            SeismicAcquisitionAndProcessing = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.SeismicAcquisitionAndProcessing)),
            CountryOfficeCost = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.CountryOfficeCost)),
            ProjectSpecificDrillingCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.ProjectSpecificDrillingCostProfile)),
            ExplorationRigUpgradingCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.ExplorationRigUpgradingCostProfile)),
            ExplorationRigUpgradingCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.ExplorationRigUpgradingCostProfileOverride)),
            ExplorationRigMobDemob = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.ExplorationRigMobDemob)),
            ExplorationRigMobDemobOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.ExplorationRigMobDemobOverride)),

            OilProducerCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.OilProducerCostProfile)),
            OilProducerCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.OilProducerCostProfileOverride)),
            GasProducerCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.GasProducerCostProfile)),
            GasProducerCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.GasProducerCostProfileOverride)),
            WaterInjectorCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.WaterInjectorCostProfile)),
            WaterInjectorCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.WaterInjectorCostProfileOverride)),
            GasInjectorCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.GasInjectorCostProfile)),
            GasInjectorCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.GasInjectorCostProfileOverride)),
            DevelopmentRigUpgradingCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.DevelopmentRigUpgradingCostProfile)),
            DevelopmentRigUpgradingCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.DevelopmentRigUpgradingCostProfileOverride)),
            DevelopmentRigMobDemob = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.DevelopmentRigMobDemob)),
            DevelopmentRigMobDemobOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.DevelopmentRigMobDemobOverride)),

            DevelopmentCampaigns = caseItem.Campaigns.Where(x => x.CampaignType == CampaignType.DevelopmentCampaign).OrderBy(x => x.CreatedUtc).Select(CampaignMapper.MapToDto).ToList(),
            ExplorationCampaigns = caseItem.Campaigns.Where(x => x.CampaignType == CampaignType.ExplorationCampaign).OrderBy(x => x.CreatedUtc).Select(CampaignMapper.MapToDto).ToList()
        };
    }

    private static TimeSeriesDto? MapToDto(TimeSeriesProfile? timeSeriesProfile)
    {
        if (timeSeriesProfile == null)
        {
            return null;
        }

        return new TimeSeriesDto
        {
            StartYear = timeSeriesProfile.StartYear,
            Values = timeSeriesProfile.Values,
            UpdatedUtc = timeSeriesProfile.UpdatedUtc
        };
    }

    private static TimeSeriesOverrideDto? MapToOverrideDto(TimeSeriesProfile? timeSeriesProfile)
    {
        if (timeSeriesProfile == null)
        {
            return null;
        }

        return new TimeSeriesOverrideDto
        {
            StartYear = timeSeriesProfile.StartYear,
            Values = timeSeriesProfile.Values,
            Override = timeSeriesProfile.Override,
            UpdatedUtc = timeSeriesProfile.UpdatedUtc
        };
    }

    private static TimeSeriesDto? ConversionMapToDto(TimeSeriesProfile? entity, string profileType, PhysUnit physUnit, Currency currency, double usdToNok)
    {
        if (entity == null)
        {
            return null;
        }

        return new TimeSeriesDto
        {
            StartYear = entity.StartYear,
            Values = UnitConversionHelpers.ConvertValuesToDto(entity.Values, physUnit, currency, usdToNok, profileType),
            UpdatedUtc = entity.UpdatedUtc
        };
    }

    private static TimeSeriesOverrideDto? ConversionMapToOverrideDto(TimeSeriesProfile? entity, string profileType, PhysUnit physUnit, Currency currency, double usdToNok)
    {
        if (entity == null)
        {
            return null;
        }

        return new TimeSeriesOverrideDto
        {
            StartYear = entity.StartYear,
            Override = entity.Override,
            Values = UnitConversionHelpers.ConvertValuesToDto(entity.Values, physUnit, currency, usdToNok, profileType),
            UpdatedUtc = entity.UpdatedUtc
        };
    }
}
