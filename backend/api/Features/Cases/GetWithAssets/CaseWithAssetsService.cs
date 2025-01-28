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
            DrainageStrategy = conversionMapperService.MapToDto<DrainageStrategy, DrainageStrategyDto>(drainageStrategy, drainageStrategy.Id, project.PhysicalUnit),
            ProductionProfileOil = ConversionMapToDto<TimeSeriesProfile, TimeSeriesCostDto>(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil), project.PhysicalUnit),
            AdditionalProductionProfileOil = ConversionMapToDto<TimeSeriesProfile, TimeSeriesCostDto>(caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil), project.PhysicalUnit),
            ProductionProfileGas = ConversionMapToDto<TimeSeriesProfile, TimeSeriesCostDto>(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileGas), project.PhysicalUnit),
            AdditionalProductionProfileGas = ConversionMapToDto<TimeSeriesProfile, TimeSeriesCostDto>(caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas), project.PhysicalUnit),
            ProductionProfileWater = ConversionMapToDto<TimeSeriesProfile, TimeSeriesCostDto>(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileWater), project.PhysicalUnit),
            ProductionProfileWaterInjection = ConversionMapToDto<TimeSeriesProfile, TimeSeriesCostDto>(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileWaterInjection), project.PhysicalUnit),
            FuelFlaringAndLosses = ConversionMapToDto<TimeSeriesProfile, TimeSeriesCostDto>(caseItem.GetProfileOrNull(ProfileTypes.FuelFlaringAndLosses), project.PhysicalUnit),
            FuelFlaringAndLossesOverride = ConversionMapToDto<TimeSeriesProfile, TimeSeriesCostOverrideDto>(caseItem.GetProfileOrNull(ProfileTypes.FuelFlaringAndLossesOverride), project.PhysicalUnit),
            NetSalesGas = ConversionMapToDto<TimeSeriesProfile, TimeSeriesCostDto>(caseItem.GetProfileOrNull(ProfileTypes.NetSalesGas), project.PhysicalUnit),
            NetSalesGasOverride = ConversionMapToDto<TimeSeriesProfile, TimeSeriesCostOverrideDto>(caseItem.GetProfileOrNull(ProfileTypes.NetSalesGasOverride), project.PhysicalUnit),
            Co2Emissions = ConversionMapToDto<TimeSeriesProfile, TimeSeriesCostDto>(caseItem.GetProfileOrNull(ProfileTypes.Co2Emissions), project.PhysicalUnit),
            Co2EmissionsOverride = ConversionMapToDto<TimeSeriesProfile, TimeSeriesCostOverrideDto>(caseItem.GetProfileOrNull(ProfileTypes.Co2EmissionsOverride), project.PhysicalUnit),
            Co2Intensity = ConversionMapToDto<TimeSeriesProfile, TimeSeriesCostDto>(caseItem.GetProfileOrNull(ProfileTypes.Co2Intensity), project.PhysicalUnit),
            ProductionProfileNgl = ConversionMapToDto<TimeSeriesProfile, TimeSeriesCostDto>(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileNgl), project.PhysicalUnit),
            ImportedElectricity = ConversionMapToDto<TimeSeriesProfile, TimeSeriesCostDto>(caseItem.GetProfileOrNull(ProfileTypes.ImportedElectricity), project.PhysicalUnit),
            ImportedElectricityOverride = ConversionMapToDto<TimeSeriesProfile, TimeSeriesCostOverrideDto>(caseItem.GetProfileOrNull(ProfileTypes.ImportedElectricityOverride), project.PhysicalUnit),
            DeferredOilProduction = ConversionMapToDto<TimeSeriesProfile, TimeSeriesCostDto>(caseItem.GetProfileOrNull(ProfileTypes.DeferredOilProduction), project.PhysicalUnit),
            DeferredGasProduction = ConversionMapToDto<DeferredGasProduction, TimeSeriesCostDto>(drainageStrategy.DeferredGasProduction, drainageStrategy.DeferredGasProduction?.Id, project.PhysicalUnit),
            Topside = mapperService.MapToDto<Topside, TopsideDto>(topside, topside.Id),
            TopsideCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfile)),
            TopsideCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfileOverride)),
            TopsideCessationCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.TopsideCessationCostProfile)),
            Substructure = mapperService.MapToDto<Substructure, SubstructureDto>(substructure, substructure.Id),
            SubstructureCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfile)),
            SubstructureCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfileOverride)),
            SubstructureCessationCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.SubstructureCessationCostProfile)),
            Surf = mapperService.MapToDto<Surf, SurfDto>(surf, surf.Id),
            SurfCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfile)),
            SurfCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfileOverride)),
            SurfCessationCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.SurfCessationCostProfile)),
            Transport = mapperService.MapToDto<Transport, TransportDto>(transport, transport.Id),
            TransportCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfile)),
            TransportCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfileOverride)),
            TransportCessationCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.TransportCessationCostProfile)),
            OnshorePowerSupply = mapperService.MapToDto<OnshorePowerSupply, OnshorePowerSupplyDto>(onshorePowerSupply, onshorePowerSupply.Id),
            OnshorePowerSupplyCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfile)),
            OnshorePowerSupplyCostProfileOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfileOverride)),
            Exploration = mapperService.MapToDto<Exploration, ExplorationDto>(exploration, exploration.Id),
            ExplorationWells = exploration.ExplorationWells.Select(w => mapperService.MapToDto<ExplorationWell, ExplorationWellDto>(w, w.ExplorationId)).ToList(),
            ExplorationWellCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.ExplorationWellCostProfile)),
            AppraisalWellCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.AppraisalWellCostProfile)),
            SidetrackCostProfile = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.SidetrackCostProfile)),
            GAndGAdminCost = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCost)),
            GAndGAdminCostOverride = MapToOverrideDto(caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCostOverride)),
            SeismicAcquisitionAndProcessing = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.SeismicAcquisitionAndProcessing)),
            CountryOfficeCost = MapToDto(caseItem.GetProfileOrNull(ProfileTypes.CountryOfficeCost)),
            WellProject = mapperService.MapToDto<WellProject, WellProjectDto>(wellProject, wellProject.Id),
            WellProjectWells = wellProject.WellProjectWells.Select(w => mapperService.MapToDto<WellProjectWell, WellProjectWellDto>(w, w.WellProjectId)).ToList(),
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

    private TDto? ConversionMapToDto<T, TDto>(T? source, Guid? id, PhysUnit physUnit) where T : class where TDto : class
    {
        if (source == null || id == null)
        {
            return null;
        }

        return conversionMapperService.MapToDto<T, TDto>(source, (Guid)id, physUnit);
    }

    private TDto? ConversionMapToDto<T, TDto>(T? source, PhysUnit physUnit) where T : class where TDto : class
    {
        if (source == null)
        {
            return null;
        }

        return conversionMapperService.MapToDto<T, TDto>(source, physUnit);
    }
}
