using api.Features.Revisions.Create;
using api.Models;

using Xunit;

namespace tests.Features.Revision.Create;

public class ResetIdPropertiesInProjectGraphServiceTests
{
    [Fact]
    public void ResetPrimaryKeysAndForeignKeysInGraph__should_reset_all_ids_except_project_id()
    {
        var projectId = Guid.NewGuid();
        var originalProjectId = Guid.NewGuid();

        var project = new Project
        {
            Id = projectId,
            ReferenceCaseId = Guid.NewGuid(),
            FusionProjectId = Guid.NewGuid(),
            CommonLibraryId = Guid.NewGuid(),
            OriginalProjectId = originalProjectId,
            Cases = new List<Case>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProjectId = projectId,
                    ExplorationId = Guid.NewGuid(),
                    SubstructureId = Guid.NewGuid(),
                    WellProjectId = Guid.NewGuid(),
                    SurfId = Guid.NewGuid(),
                    TransportId = Guid.NewGuid(),
                    TopsideId = Guid.NewGuid(),
                    Substructure = new Substructure
                    {
                        Id = Guid.NewGuid(),
                        ProjectId = projectId
                    },
                    Topside = new Topside
                    {
                        Id = Guid.NewGuid(),
                        ProjectId = projectId
                    }
                }
            }
        };

        var caseIdMapping = project.Cases.ToDictionary(x => x.Id, _ => Guid.NewGuid());

        ResetIdPropertiesInProjectGraphService.ResetPrimaryKeysAndForeignKeysInGraph(project, caseIdMapping);

        Assert.Equal(Guid.Empty, project.ReferenceCaseId);
        Assert.Equal(originalProjectId, project.OriginalProjectId);

        Assert.Equal(Guid.Empty, project.Cases.First().ExplorationId);
        Assert.Equal(Guid.Empty, project.Cases.First().SubstructureId);
        Assert.Equal(Guid.Empty, project.Cases.First().WellProjectId);
        Assert.Equal(Guid.Empty, project.Cases.First().SurfId);
        Assert.Equal(Guid.Empty, project.Cases.First().TransportId);
        Assert.Equal(Guid.Empty, project.Cases.First().TopsideId);

        var newProjectId = project.Id;

        Assert.NotEqual(newProjectId, projectId);
        Assert.NotEqual(Guid.Empty, project.FusionProjectId);
        Assert.NotEqual(Guid.Empty, project.CommonLibraryId);

        Assert.Equal(newProjectId, project.Cases.First().ProjectId);
        Assert.Equal(newProjectId, project.Cases.First().Substructure!.ProjectId);
        Assert.Equal(newProjectId, project.Cases.First().Topside!.ProjectId);
    }
}
