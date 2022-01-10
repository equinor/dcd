using System;
using System.Collections.Generic;
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
        public void RetrieveAllProjects()
        {
            ProjectService projectService = new ProjectService(fixture.context);
            IEnumerable<Project> projects = projectService.GetAll();

            Assert.True(projects.Count() > 0);
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
            Assert.Throws<NotFoundInDBException>(() => projectService.GetProject(new Guid()));
        }
    }
}
