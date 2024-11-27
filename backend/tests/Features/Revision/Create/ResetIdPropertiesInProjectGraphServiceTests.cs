using api.Features.Revision.Create;
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
                    ExplorationLink = Guid.NewGuid(),
                    SubstructureLink = Guid.NewGuid(),
                    WellProjectLink = Guid.NewGuid(),
                    SurfLink = Guid.NewGuid(),
                    TransportLink = Guid.NewGuid(),
                    TopsideLink = Guid.NewGuid(),
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

        ResetIdPropertiesInProjectGraphService.ResetPrimaryKeysAndForeignKeysInGraph(project);

        Assert.Equal(Guid.Empty, project.ReferenceCaseId);
        Assert.Equal(Guid.Empty, project.FusionProjectId);
        Assert.Equal(Guid.Empty, project.CommonLibraryId);
        Assert.Equal(originalProjectId, project.OriginalProjectId);

        Assert.Equal(Guid.Empty, project.Cases.First().ExplorationLink);
        Assert.Equal(Guid.Empty, project.Cases.First().SubstructureLink);
        Assert.Equal(Guid.Empty, project.Cases.First().WellProjectLink);
        Assert.Equal(Guid.Empty, project.Cases.First().SurfLink);
        Assert.Equal(Guid.Empty, project.Cases.First().TransportLink);
        Assert.Equal(Guid.Empty, project.Cases.First().TopsideLink);

        var newProjectId = project.Id;

        Assert.NotEqual(newProjectId, projectId);

        Assert.Equal(newProjectId, project.Cases.First().ProjectId);
        Assert.Equal(newProjectId, project.Cases.First().Substructure!.ProjectId);
        Assert.Equal(newProjectId, project.Cases.First().Topside!.ProjectId);
    }
}
