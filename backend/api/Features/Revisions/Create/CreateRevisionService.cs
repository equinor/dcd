using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Revisions.Create;

public class CreateRevisionService(CreateRevisionRepository createRevisionRepository, DcdDbContext context)
{
    public async Task<Guid> CreateRevision(Guid projectId, CreateRevisionDto createRevisionDto)
    {
        var revision = await createRevisionRepository.GetProjectAndAssetsNoTracking(projectId);

        revision.IsRevision = true;
        revision.OriginalProjectId = projectId;
        revision.InternalProjectPhase = createRevisionDto.InternalProjectPhase;
        revision.Classification = createRevisionDto.Classification;

        ResetIdPropertiesInProjectGraphService.ResetPrimaryKeysAndForeignKeysInGraph(revision);

        revision.RevisionDetails = new RevisionDetails
        {
            OriginalProjectId = projectId,
            RevisionName = createRevisionDto.Name,
            Mdqc = createRevisionDto.Mdqc,
            Arena = createRevisionDto.Arena,
            RevisionDate = DateTimeOffset.UtcNow,
            Revision = revision,
            Classification = createRevisionDto.Classification
        };

        context.Projects.Add(revision);

        var existingProject = await context.Projects.FirstAsync(p => (p.Id == projectId || p.FusionProjectId == projectId) && !p.IsRevision);
        existingProject.InternalProjectPhase = createRevisionDto.InternalProjectPhase;
        existingProject.Classification = createRevisionDto.Classification;

        await context.SaveChangesAsync();

        return revision.Id;
    }
}
