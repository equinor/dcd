using api.Features.Cases.Duplicate;
using api.Models;

using Xunit;

namespace tests.Features.Cases.Duplicate;

public class ResetIdPropertiesInCaseGraphServiceTests
{
    [Fact]
    public void ResetPrimaryKeysAndForeignKeysInGraph__should_reset_all_ids_except_project_id()
    {
        var caseId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var wellId = Guid.NewGuid();

        var caseItem = new Case
        {
            Id = caseId,
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
            },
            WellProject = new WellProject
            {
                Id = Guid.NewGuid(),
                DevelopmentWells =
                [
                    new DevelopmentWell
                    {
                        Id = Guid.NewGuid(),
                        WellId = wellId
                    }
                ]
            }
        };

        var duplicateCaseId = Guid.NewGuid();

        ResetIdPropertiesInCaseGraphService.ResetPrimaryKeysAndForeignKeysInGraph(caseItem, duplicateCaseId);

        Assert.Equal(Guid.Empty, caseItem.ExplorationId);
        Assert.Equal(Guid.Empty, caseItem.SubstructureId);
        Assert.Equal(Guid.Empty, caseItem.WellProjectId);
        Assert.Equal(Guid.Empty, caseItem.SurfId);
        Assert.Equal(Guid.Empty, caseItem.TransportId);
        Assert.Equal(Guid.Empty, caseItem.TopsideId);
        Assert.Equal(Guid.Empty, caseItem.WellProjectId);

        Assert.NotNull(caseItem.WellProject);
        Assert.Equal(Guid.Empty, caseItem.WellProject.DevelopmentWells![0].Id);

        Assert.Equal(projectId, caseItem.ProjectId);
        Assert.Equal(projectId, caseItem.Substructure.ProjectId);
        Assert.Equal(projectId, caseItem.Topside.ProjectId);
        Assert.Equal(wellId, caseItem.WellProject.DevelopmentWells![0].WellId);

        Assert.NotEqual(caseId, caseItem.Id);
    }
}
