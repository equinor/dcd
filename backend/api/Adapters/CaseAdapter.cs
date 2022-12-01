using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class CaseAdapter
{
    public static Case Convert(CaseDto caseDto)
    {
        return new Case
        {
            Id = caseDto.Id,
            ProjectId = caseDto.ProjectId,
            Name = caseDto.Name,
            Description = caseDto.Description,
            ReferenceCase = caseDto.ReferenceCase,
            DGADate = caseDto.DGADate,
            DGBDate = caseDto.DGBDate,
            DGCDate = caseDto.DGCDate,
            APXDate = caseDto.APXDate,
            APZDate = caseDto.APZDate,
            DG0Date = caseDto.DG0Date,
            DG1Date = caseDto.DG1Date,
            DG2Date = caseDto.DG2Date,
            DG3Date = caseDto.DG3Date,
            DG4Date = caseDto.DG4Date,
            CreateTime = DateTimeOffset.UtcNow,
            ModifyTime = DateTimeOffset.UtcNow,
            DrainageStrategyLink = caseDto.DrainageStrategyLink,
            ExplorationLink = caseDto.ExplorationLink,
            WellProjectLink = caseDto.WellProjectLink,
            SurfLink = caseDto.SurfLink,
            TopsideLink = caseDto.TopsideLink,
            SubstructureLink = caseDto.SubstructureLink,
            TransportLink = caseDto.TransportLink,
            ArtificialLift = caseDto.ArtificialLift,
            ProductionStrategyOverview = caseDto.ProductionStrategyOverview,
            ProducerCount = caseDto.ProducerCount,
            GasInjectorCount = caseDto.GasInjectorCount,
            WaterInjectorCount = caseDto.WaterInjectorCount,
            FacilitiesAvailability = caseDto.FacilitiesAvailability,
            CapexFactorFeasibilityStudies = caseDto.CapexFactorFeasibilityStudies,
            CapexFactorFEEDStudies = caseDto.CapexFactorFEEDStudies,
            NPV = caseDto.NPV,
            BreakEven = caseDto.BreakEven,
            Host = caseDto.Host,
            SharepointFileId = caseDto.SharepointFileId,
            SharepointFileName = caseDto.SharepointFileName,
            SharepointFileUrl = caseDto.SharepointFileUrl,
        };
    }

    public static void ConvertExisting(Case existing, CaseDto caseDto)
    {
        existing.Id = caseDto.Id;
        existing.ProjectId = caseDto.ProjectId;
        existing.Name = caseDto.Name;
        existing.Description = caseDto.Description;
        existing.ReferenceCase = caseDto.ReferenceCase;
        existing.DGADate = caseDto.DGADate;
        existing.DGBDate = caseDto.DGBDate;
        existing.DGCDate = caseDto.DGCDate;
        existing.APXDate = caseDto.APXDate;
        existing.APZDate = caseDto.APZDate;
        existing.DG0Date = caseDto.DG0Date;
        existing.DG1Date = caseDto.DG1Date;
        existing.DG2Date = caseDto.DG2Date;
        existing.DG3Date = caseDto.DG3Date;
        existing.DG4Date = caseDto.DG4Date;
        existing.ModifyTime = DateTimeOffset.UtcNow;
        existing.DrainageStrategyLink = caseDto.DrainageStrategyLink;
        existing.ExplorationLink = caseDto.ExplorationLink;
        existing.WellProjectLink = caseDto.WellProjectLink;
        existing.SurfLink = caseDto.SurfLink;
        existing.TopsideLink = caseDto.TopsideLink;
        existing.SubstructureLink = caseDto.SubstructureLink;
        existing.TransportLink = caseDto.TransportLink;
        existing.ArtificialLift = caseDto.ArtificialLift;
        existing.ProductionStrategyOverview = caseDto.ProductionStrategyOverview;
        existing.ProducerCount = caseDto.ProducerCount;
        existing.GasInjectorCount = caseDto.GasInjectorCount;
        existing.WaterInjectorCount = caseDto.WaterInjectorCount;
        existing.FacilitiesAvailability = caseDto.FacilitiesAvailability;
        existing.CapexFactorFeasibilityStudies = caseDto.CapexFactorFeasibilityStudies;
        existing.CapexFactorFEEDStudies = caseDto.CapexFactorFEEDStudies;
        existing.NPV = caseDto.NPV;
        existing.BreakEven = caseDto.BreakEven;
        existing.Host = caseDto.Host;
        existing.SharepointFileId = caseDto.SharepointFileId;
        existing.SharepointFileName = caseDto.SharepointFileName;
    }
}
