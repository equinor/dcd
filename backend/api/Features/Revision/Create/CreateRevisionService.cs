using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Revision.Create;

public class CreateRevisionService(CreateRevisionRepository createRevisionRepository, DcdDbContext context)
{
    public async Task<Guid> CreateRevision(Guid projectId, CreateRevisionDto createRevisionDto)
    {
        var project = await createRevisionRepository.GetProjectAndAssetsNoTracking(projectId);

        project.IsRevision = true;
        project.OriginalProjectId = projectId;
        project.InternalProjectPhase = createRevisionDto.InternalProjectPhase;
        project.Classification = createRevisionDto.Classification;

        ResetIdPropertiesInProjectGraphService.ResetPrimaryKeysAndForeignKeysInGraph(project);

        project.RevisionDetails = new RevisionDetails
        {
            OriginalProjectId = projectId,
            RevisionName = createRevisionDto.Name,
            Mdqc = createRevisionDto.Mdqc,
            Arena = createRevisionDto.Arena,
            RevisionDate = DateTimeOffset.UtcNow,
            Revision = project,
            Classification = createRevisionDto.Classification
        };

        context.Projects.Add(project);

        var existingProject = await context.Projects.FirstAsync(p => (p.Id == projectId || p.FusionProjectId == projectId) && !p.IsRevision);
        existingProject.InternalProjectPhase = createRevisionDto.InternalProjectPhase;
        existingProject.Classification = createRevisionDto.Classification;

        await context.SaveChangesAsync();

        return project.Id;
    }
}
