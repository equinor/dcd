using System;
using System.Collections.Generic;

using api.Adapters;
using api.Dtos;
using api.Models;

using Xunit;

namespace tests
{
    [Collection("Database collection")]
    public class ProjectAdapterShould
    {
        [Fact]
        public void ConvertProjectDtoToDataModel()
        {

            var expected = createProjectDto();

            // Act
            var actual = ProjectAdapter.Convert(expected);

            Assert.Equal(actual.Name, expected.Name);
            Assert.Equal(actual.Description, expected.Description);
            Assert.Equal(actual.Country, expected.Country);
            Assert.Equal(actual.ProjectPhase, expected.ProjectPhase);
            Assert.Equal(actual.ProjectCategory, expected.ProjectCategory);
        }

        private ProjectDto createProjectDto()
        {
            return new ProjectDto
            {
                Name = "First Project",
                Description = "Description",
                ProjectCategory = ProjectCategory.OffshoreWind,
                ProjectPhase = ProjectPhase.DG2,
                Country = "Barbados"
            };
        }
    }
}
