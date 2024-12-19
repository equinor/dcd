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

        var caseItem = new Case
        {
            Id = caseId,
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
        };

        var duplicateCaseId = Guid.NewGuid();

        ResetIdPropertiesInCaseGraphService.ResetPrimaryKeysAndForeignKeysInGraph(caseItem, duplicateCaseId);

        Assert.Equal(Guid.Empty, caseItem.ExplorationLink);
        Assert.Equal(Guid.Empty, caseItem.SubstructureLink);
        Assert.Equal(Guid.Empty, caseItem.WellProjectLink);
        Assert.Equal(Guid.Empty, caseItem.SurfLink);
        Assert.Equal(Guid.Empty, caseItem.TransportLink);
        Assert.Equal(Guid.Empty, caseItem.TopsideLink);

        Assert.Equal(projectId, caseItem.ProjectId);
        Assert.Equal(projectId, caseItem.Substructure.ProjectId);
        Assert.Equal(projectId, caseItem.Topside.ProjectId);

        Assert.NotEqual(caseId, caseItem.Id);
    }
}
