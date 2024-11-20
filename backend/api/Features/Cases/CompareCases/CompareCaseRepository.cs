using api.Context;
using api.Exceptions;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class CompareCaseRepository(DcdDbContext context)
{
    public async Task<Project> LoadProject(Guid projectId)
    {
        var project = await context.Projects
            .Include(p => p.Cases)
            .Include(p => p.Wells)
            .Include(p => p.ExplorationOperationalWellCosts)
            .Include(p => p.DevelopmentOperationalWellCosts)
            .FirstOrDefaultAsync(p => p.Id.Equals(projectId) || p.FusionProjectId.Equals(projectId));

        if (project == null)
        {
            throw new NotFoundInDBException($"Project {projectId} not found");
        }

        var caseIds = project.Cases.Where(x => !x.Archived).Select(c => c.Id).ToList();
        await context.Cases
            .Where(x => caseIds.Contains(x.Id))
            .Include(c => c.CessationWellsCostOverride)
            .Include(c => c.CessationWellsCost)
            .Include(c => c.CessationOffshoreFacilitiesCostOverride)
            .Include(c => c.CessationOffshoreFacilitiesCost)
            .Include(c => c.CessationOnshoreFacilitiesCostProfile)
            .Include(c => c.TotalFeasibilityAndConceptStudies)
            .Include(c => c.TotalFeasibilityAndConceptStudiesOverride)
            .Include(c => c.TotalFEEDStudies)
            .Include(c => c.TotalFEEDStudiesOverride)
            .Include(c => c.TotalOtherStudiesCostProfile)
            .Include(c => c.WellInterventionCostProfile)
            .Include(c => c.WellInterventionCostProfileOverride)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfile)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfileOverride)
            .Include(c => c.HistoricCostCostProfile)
            .Include(c => c.OnshoreRelatedOPEXCostProfile)
            .Include(c => c.AdditionalOPEXCostProfile)
            .LoadAsync();

        var drainageStrategyLinkIds = project.Cases.Select(x => x.DrainageStrategyLink).ToList();
        await context.DrainageStrategies
            .Include(d => d.ProductionProfileOil)
            .Include(d => d.AdditionalProductionProfileOil)
            .Include(d => d.ProductionProfileGas)
            .Include(d => d.AdditionalProductionProfileGas)
            .Include(d => d.Co2EmissionsOverride)
            .Include(d => d.Co2Emissions)
            .Where(x => drainageStrategyLinkIds.Contains(x.Id))
            .LoadAsync();

        var explorationLinkIds = project.Cases.Select(x => x.ExplorationLink).ToList();
        await context.Explorations
            .Include(e => e.GAndGAdminCost)
            .Include(e => e.CountryOfficeCost)
            .Include(e => e.SeismicAcquisitionAndProcessing)
            .Include(e => e.ExplorationWellCostProfile)
            .Include(e => e.AppraisalWellCostProfile)
            .Include(e => e.SidetrackCostProfile)
            .Where(x => explorationLinkIds.Contains(x.Id))
            .LoadAsync();

        var wellProjectLinks = project.Cases.Select(x => x.WellProjectLink).ToList();
        await context.WellProjects
            .Include(w => w.OilProducerCostProfileOverride)
            .Include(w => w.OilProducerCostProfile)
            .Include(w => w.GasProducerCostProfileOverride)
            .Include(w => w.GasProducerCostProfile)
            .Include(w => w.WaterInjectorCostProfileOverride)
            .Include(w => w.WaterInjectorCostProfile)
            .Include(w => w.GasInjectorCostProfileOverride)
            .Include(w => w.GasInjectorCostProfile)
            .Where(x => wellProjectLinks.Contains(x.Id))
            .LoadAsync();

        var substructureLinkIds = project.Cases.Select(x => x.SubstructureLink).ToList();
        await context.Substructures
            .Include(x => x.CostProfileOverride)
            .Include(x => x.CostProfile)
            .Where(x => substructureLinkIds.Contains(x.Id))
            .LoadAsync();

        var surfLinkIds = project.Cases.Select(x => x.SurfLink).ToList();
        await context.Surfs
            .Include(x => x.CostProfileOverride)
            .Include(x => x.CostProfile)
            .Where(x => surfLinkIds.Contains(x.Id))
            .LoadAsync();

        var topsideLinkIds = project.Cases.Select(x => x.TopsideLink).ToList();
        await context.Topsides
            .Include(x => x.CostProfileOverride)
            .Include(x => x.CostProfile)
            .Where(x => topsideLinkIds.Contains(x.Id))
            .LoadAsync();

        var transportLinkIds = project.Cases.Select(x => x.TransportLink).ToList();
        await context.Transports
            .Include(x => x.CostProfileOverride)
            .Include(x => x.CostProfile)
            .Where(x => transportLinkIds.Contains(x.Id))
            .LoadAsync();

        return project;
    }
}
