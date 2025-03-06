using api.Context;
using api.Context.Extensions;
using api.Features.Images.Delete;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Revisions.Delete;

public class DeleteRevisionService(DcdDbContext context, DeleteCaseImageService deleteCaseImageService, DeleteProjectImageService deleteProjectImageService)
{
    public async Task DeleteRevision(Guid projectId, Guid revisionId)
    {
        await context.EnsureRevisionIsConnectedToProject(projectId, revisionId);

        var caseImages = await context.CaseImages.Where(x => x.Case.ProjectId == revisionId).ToListAsync();
        var projectImages = await context.ProjectImages.Where(x => x.ProjectId == revisionId).ToListAsync();

        foreach (var caseImage in caseImages)
        {
            await deleteCaseImageService.DeleteRevisionImage(caseImage);
        }

        foreach (var projectImage in projectImages)
        {
            await deleteProjectImageService.DeleteRevisionImage(projectImage);
        }

        context.CaseImages.RemoveRange(caseImages);
        context.ProjectImages.RemoveRange(projectImages);

        context.CampaignWells.RemoveRange(await context.CampaignWells.Where(x => x.Campaign.Case.ProjectId == revisionId).ToListAsync());
        context.Campaigns.RemoveRange(await context.Campaigns.Where(x => x.Case.ProjectId == revisionId).ToListAsync());

        context.TimeSeriesProfiles.RemoveRange(await context.TimeSeriesProfiles.Where(x => x.Case.ProjectId == revisionId).ToListAsync());
        context.Transports.RemoveRange(await context.Transports.Where(x => x.Case.ProjectId == revisionId).ToListAsync());
        context.Topsides.RemoveRange(await context.Topsides.Where(x => x.Case.ProjectId == revisionId).ToListAsync());
        context.Substructures.RemoveRange(await context.Substructures.Where(x => x.Case.ProjectId == revisionId).ToListAsync());
        context.Surfs.RemoveRange(await context.Surfs.Where(x => x.Case.ProjectId == revisionId).ToListAsync());
        context.OnshorePowerSupplies.RemoveRange(await context.OnshorePowerSupplies.Where(x => x.Case.ProjectId == revisionId).ToListAsync());
        context.DrainageStrategies.RemoveRange(await context.DrainageStrategies.Where(x => x.Case.ProjectId == revisionId).ToListAsync());
        context.Cases.RemoveRange(await context.Cases.Where(x => x.ProjectId == revisionId).ToListAsync());

        context.ProjectMembers.RemoveRange(await context.ProjectMembers.Where(x => x.ProjectId == revisionId).ToListAsync());
        context.Wells.RemoveRange(await context.Wells.Where(x => x.ProjectId == revisionId).ToListAsync());
        context.DevelopmentOperationalWellCosts.Remove(await context.DevelopmentOperationalWellCosts.Where(x => x.ProjectId == revisionId).SingleAsync());
        context.ExplorationOperationalWellCosts.Remove(await context.ExplorationOperationalWellCosts.Where(x => x.ProjectId == revisionId).SingleAsync());
        context.RevisionDetails.Remove(await context.RevisionDetails.Where(x => x.RevisionId == revisionId).SingleAsync());

        context.Projects.Remove(await context.Projects.Where(x => x.Id == revisionId).SingleAsync());

        await context.SaveChangesAsync();
    }
}
