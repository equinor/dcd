using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Wells.GetAffectedCases;

public class GetAffectedCasesService(DcdDbContext context)
{
    public async Task<List<CaseDto>> GetAffectedCases(Guid projectId, Guid wellId)
    {
        _ = await context.Wells
            .Where(x => x.ProjectId == projectId)
            .Where(x => x.Id == wellId)
            .SingleAsync();

        var cases = await GetCasesAffectedByDeleteWell(wellId);

        return cases.Select(MapToDto).ToList();
    }

    private async Task<List<Case>> GetCasesAffectedByDeleteWell(Guid wellId)
    {
        var well = await context.Wells
            .Include(w => w.WellProjectWells).ThenInclude(wp => wp.DrillingSchedule)
            .Include(w => w.WellProjectWells).ThenInclude(wp => wp.WellProject)
            .Include(w => w.ExplorationWells).ThenInclude(ew => ew.DrillingSchedule)
            .Include(w => w.ExplorationWells).ThenInclude(ew => ew.Exploration)
            .FirstOrDefaultAsync(w => w.Id == wellId);

        if (well == null)
        {
            return [];
        }

        var wellProjectIds = well.WellProjectWells
            .Where(x => x.DrillingSchedule?.Values.Length != 0)
            .Select(x => x.WellProject.Id)
            .Distinct();

        var explorationIds = well.ExplorationWells
            .Where(x => x.DrillingSchedule?.Values.Length != 0)
            .Select(x => x.Exploration.Id)
            .Distinct();

        return await context.Cases
            .Where(x => wellProjectIds.Contains(x.WellProjectLink) || explorationIds.Contains(x.ExplorationLink))
            .ToListAsync();
    }

    private static CaseDto MapToDto(Case caseItem)
    {
        return new CaseDto
        {
            Id = caseItem.Id,
            ProjectId = caseItem.ProjectId,
            Name = caseItem.Name,
            Description = caseItem.Description,
            ReferenceCase = caseItem.ReferenceCase,
            Archived = caseItem.Archived,
            ArtificialLift = caseItem.ArtificialLift,
            ProductionStrategyOverview = caseItem.ProductionStrategyOverview,
            ProducerCount = caseItem.ProducerCount,
            GasInjectorCount = caseItem.GasInjectorCount,
            WaterInjectorCount = caseItem.WaterInjectorCount,
            FacilitiesAvailability = caseItem.FacilitiesAvailability,
            CapexFactorFeasibilityStudies = caseItem.CapexFactorFeasibilityStudies,
            CapexFactorFEEDStudies = caseItem.CapexFactorFEEDStudies,
            NPV = caseItem.NPV,
            NPVOverride = caseItem.NPVOverride,
            BreakEven = caseItem.BreakEven,
            BreakEvenOverride = caseItem.BreakEvenOverride,
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
            DrainageStrategyLink = caseItem.DrainageStrategyLink,
            WellProjectLink = caseItem.WellProjectLink,
            SurfLink = caseItem.SurfLink,
            SubstructureLink = caseItem.SubstructureLink,
            TopsideLink = caseItem.TopsideLink,
            TransportLink = caseItem.TransportLink,
            ExplorationLink = caseItem.ExplorationLink,
            SharepointFileId = caseItem.SharepointFileId,
            SharepointFileName = caseItem.SharepointFileName,
            SharepointFileUrl = caseItem.SharepointFileUrl
        };
    }
}
