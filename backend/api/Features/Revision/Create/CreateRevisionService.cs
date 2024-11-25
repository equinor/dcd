using api.Context;
using api.Models;
using api.Repositories;

namespace api.Features.Revision.Create;

public class CreateRevisionService(CreateRevisionRepository createRevisionRepository,
    IProjectRepository projectRepository,
    DcdDbContext context)
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

        var existingProject = (await projectRepository.GetProject(projectId))!;
        existingProject.InternalProjectPhase = createRevisionDto.InternalProjectPhase;
        existingProject.Classification = createRevisionDto.Classification;

        await context.SaveChangesAsync();

        return project.Id;
    }
}
