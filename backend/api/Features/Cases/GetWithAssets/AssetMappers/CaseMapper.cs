using api.Features.ProjectData.Dtos.AssetDtos;
using api.Models;

namespace api.Features.Cases.GetWithAssets.AssetMappers;

public static class CaseMapper
{
    public static CaseOverviewDto MapToDto(Case caseItem)
    {
        return new CaseOverviewDto
        {
            CaseId = caseItem.Id,
            ProjectId = caseItem.ProjectId,
            Name = caseItem.Name,
            Description = caseItem.Description,
            Archived = caseItem.Archived,
            ProductionStrategyOverview = caseItem.ProductionStrategyOverview,
            ArtificialLift = caseItem.ArtificialLift,
            ProducerCount = caseItem.ProducerCount,
            GasInjectorCount = caseItem.GasInjectorCount,
            WaterInjectorCount = caseItem.WaterInjectorCount,
            Npv = caseItem.Npv,
            NpvOverride = caseItem.NpvOverride,
            BreakEven = caseItem.BreakEven,
            BreakEvenOverride = caseItem.BreakEvenOverride,
            DiscountedCashflow = caseItem.DiscountedCashflow,
            FacilitiesAvailability = caseItem.FacilitiesAvailability,
            CapexFactorFeasibilityStudies = caseItem.CapexFactorFeasibilityStudies,
            CapexFactorFeedStudies = caseItem.CapexFactorFeedStudies,
            InitialYearsWithoutWellInterventionCost = caseItem.InitialYearsWithoutWellInterventionCost,
            FinalYearsWithoutWellInterventionCost = caseItem.FinalYearsWithoutWellInterventionCost,
            Host = caseItem.Host,
            AverageCo2Intensity = caseItem.AverageCo2Intensity,
            DgaDate = caseItem.DgaDate,
            DgbDate = caseItem.DgbDate,
            DgcDate = caseItem.DgcDate,
            ApboDate = caseItem.ApboDate,
            BorDate = caseItem.BorDate,
            VpboDate = caseItem.VpboDate,
            Dg0Date = caseItem.Dg0Date,
            Dg1Date = caseItem.Dg1Date,
            Dg2Date = caseItem.Dg2Date,
            Dg3Date = caseItem.Dg3Date,
            Dg4Date = caseItem.Dg4Date,
            CreatedUtc = caseItem.CreatedUtc,
            UpdatedUtc = caseItem.UpdatedUtc,
            SurfId = caseItem.SurfId,
            SubstructureId = caseItem.SubstructureId,
            TopsideId = caseItem.TopsideId,
            TransportId = caseItem.TransportId,
            OnshorePowerSupplyId = caseItem.OnshorePowerSupplyId,
            SharepointFileId = caseItem.SharepointFileId,
            SharepointFileName = caseItem.SharepointFileName,
            SharepointFileUrl = caseItem.SharepointFileUrl
        };
    }
}
