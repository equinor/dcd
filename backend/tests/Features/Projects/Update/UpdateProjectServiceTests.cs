using api.Features.Projects.Update;
using api.Models;
using api.Models.Enums;

using Xunit;

namespace tests.Features.Projects.Update;

public class UpdateProjectServiceTests
{
    [Theory]
    [InlineData(ProjectClassification.Internal, ProjectClassification.Internal, false)]
    [InlineData(ProjectClassification.Internal, ProjectClassification.Confidential, true)]
    [InlineData(ProjectClassification.Confidential, ProjectClassification.Confidential, false)]
    public void ClassificationChangesToMoreStrict(ProjectClassification existing, ProjectClassification updated, bool expected)
    {
        var existingProject = new Project
        {
            Classification = existing
        };

        var actual = ProjectClassificationHelper.ClassificationChangedToMoreStrict(existingProject, updated);

        Assert.Equal(expected, actual);
    }
}
