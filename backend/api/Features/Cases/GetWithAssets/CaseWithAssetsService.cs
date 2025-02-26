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

        var physicalUnit = await context.Projects.Where(x => x.Id == projectPk).Select(x => x.PhysicalUnit).SingleAsync();

        var caseItem = await caseWithAssetsRepository.GetCaseWithAssets(caseId);

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
            CalculatedTotalIncomeCostProfileUsd = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalIncomeCostProfileUsd)),
            CalculatedTotalCostCostProfileUsd = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.CalculatedTotalCostCostProfileUsd)),

            DrainageStrategy = DrainageStrategyMapper.MapToDto(caseItem.DrainageStrategy),
            ProductionProfileOil = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil), ProfileTypes.ProductionProfileOil, physicalUnit),
            AdditionalProductionProfileOil = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil), ProfileTypes.AdditionalProductionProfileOil, physicalUnit),
            ProductionProfileGas = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas), ProfileTypes.ProductionProfileGas, physicalUnit),
            AdditionalProductionProfileGas = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas), ProfileTypes.AdditionalProductionProfileGas, physicalUnit),
            ProductionProfileWater = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileWater), ProfileTypes.ProductionProfileWater, physicalUnit),
            ProductionProfileWaterInjection = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileWaterInjection), ProfileTypes.ProductionProfileWaterInjection, physicalUnit),
            FuelFlaringAndLosses = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.FuelFlaringAndLosses), ProfileTypes.FuelFlaringAndLosses, physicalUnit),
            FuelFlaringAndLossesOverride = ConversionMapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.FuelFlaringAndLossesOverride), ProfileTypes.FuelFlaringAndLossesOverride, physicalUnit),
            NetSalesGas = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.NetSalesGas), ProfileTypes.NetSalesGas, physicalUnit),
            NetSalesGasOverride = ConversionMapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.NetSalesGasOverride), ProfileTypes.NetSalesGasOverride, physicalUnit),
            Co2Emissions = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.Co2Emissions), ProfileTypes.Co2Emissions, physicalUnit),
            Co2EmissionsOverride = ConversionMapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.Co2EmissionsOverride), ProfileTypes.Co2EmissionsOverride, physicalUnit),
            Co2Intensity = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.Co2Intensity), ProfileTypes.Co2Intensity, physicalUnit),
            ProductionProfileNgl = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileNgl), ProfileTypes.ProductionProfileNgl, physicalUnit),
            ImportedElectricity = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.ImportedElectricity), ProfileTypes.ImportedElectricity, physicalUnit),
            ImportedElectricityOverride = ConversionMapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.ImportedElectricityOverride), ProfileTypes.ImportedElectricityOverride, physicalUnit),
            DeferredOilProduction = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.DeferredOilProduction), ProfileTypes.DeferredOilProduction, physicalUnit),
            DeferredGasProduction = ConversionMapToDto(caseItem.GetProfileOrNull(ProfileTypes.DeferredGasProduction), ProfileTypes.DeferredGasProduction, physicalUnit),

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
            AppraisalWellCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.AppraisalWellCostProfile)),
            SidetrackCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.SidetrackCostProfile)),
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

            DevelopmentCampaigns = caseItem.Campaigns.Where(x => x.CampaignType == CampaignType.DevelopmentCampaign).Select(CampaignMapper.MapToDto).ToList(),
            ExplorationCampaigns = caseItem.Campaigns.Where(x => x.CampaignType == CampaignType.ExplorationCampaign).Select(CampaignMapper.MapToDto).ToList()
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

    private static TimeSeriesDto? ConversionMapToDto(TimeSeriesProfile? entity, string profileType, PhysUnit physUnit)
    {
        if (entity == null)
        {
            return null;
        }

        return new TimeSeriesDto
        {
            StartYear = entity.StartYear,
            Values = UnitConversionHelpers.ConvertValuesToDto(entity.Values, physUnit, profileType),
            UpdatedUtc = entity.UpdatedUtc
        };
    }

    private static TimeSeriesOverrideDto? ConversionMapToOverrideDto(TimeSeriesProfile? entity, string profileType, PhysUnit physUnit)
    {
        if (entity == null)
        {
            return null;
        }

        return new TimeSeriesOverrideDto
        {
            StartYear = entity.StartYear,
            Override = entity.Override,
            Values = UnitConversionHelpers.ConvertValuesToDto(entity.Values, physUnit, profileType),
            UpdatedUtc = entity.UpdatedUtc
        };
    }
}
