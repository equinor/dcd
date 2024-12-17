using api.Context;
using api.Context.Extensions;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Revisions.Create;

public class CreateRevisionService(CreateRevisionRepository createRevisionRepository, DcdDbContext context)
{
    public async Task<Guid> CreateRevision(Guid projectId, CreateRevisionDto createRevisionDto)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var revision = await createRevisionRepository.GetProjectAndAssetsNoTracking(projectPk);

        revision.IsRevision = true;
        revision.OriginalProjectId = projectPk;
        revision.InternalProjectPhase = createRevisionDto.InternalProjectPhase ?? revision.InternalProjectPhase;
        revision.Classification = createRevisionDto.Classification ?? revision.Classification;

        ResetIdPropertiesInProjectGraphService.ResetPrimaryKeysAndForeignKeysInGraph(revision);

        revision.RevisionDetails = new RevisionDetails
        {
            RevisionName = createRevisionDto.Name,
            Mdqc = createRevisionDto.Mdqc,
            Arena = createRevisionDto.Arena,
            RevisionDate = DateTimeOffset.UtcNow,
            Revision = revision,
            Classification = createRevisionDto.Classification ?? revision.Classification
        };

        context.Projects.Add(revision);

        var existingProject = await context.Projects.SingleAsync(p => p.Id == projectPk);
        existingProject.InternalProjectPhase = createRevisionDto.InternalProjectPhase ?? existingProject.InternalProjectPhase;
        existingProject.Classification = createRevisionDto.Classification ?? existingProject.Classification;

        await context.SaveChangesAsync();

        return revision.Id;
    }
}
