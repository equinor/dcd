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
            AverageCo2Intensity = caseItem.AverageCo2Intensity,
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
