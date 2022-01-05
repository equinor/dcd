using System;
using System.Linq;

using api.Models;
using api.Services;

using Xunit;

namespace tests
{
    [Collection("Database collection")]
    public class ProjectServiceTest
    {
        DatabaseFixture fixture;

        public ProjectServiceTest(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }
        [Fact]
        public void GetQueryable()
        {
            ProjectService projectService = new ProjectService(fixture.context);
            IQueryable<Project> projectQueryable = projectService.GetAll();

            Assert.True(projectQueryable.Count() > 0);
        }

        [Fact]
        public void GetProject()
        {
            ProjectService projectService = new ProjectService(fixture.context);

            Project projectExsists = projectService.GetAll().First();

            Project projectGotten = projectService.GetProject(projectExsists.Id);

            Assert.Equal(projectExsists, projectGotten);
        }

        [Fact]
        public void GetDoesNotExist()
        {
            ProjectService projectService = new ProjectService(fixture.context);

            Assert.Throws<NotFoundInDBException>(() => projectService.GetProject("some_project_id_that_does_not_exist"));
        }
    }
}
