using System;
using System.Linq;

using api.Models;
using api.SampleData.Builders;
using api.SampleData.Generators;
using api.Services;

using Xunit;


namespace tests
{
    [Collection("Database collection")]
    public class SubstructureServiceShould : IDisposable
    {
        private readonly DatabaseFixture fixture;

        public SubstructureServiceShould()
        {
            fixture = new DatabaseFixture();
        }

        public void Dispose()
        {
            fixture.Dispose();
        }

        [Fact]
        public void GetSubstructures()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var substructureService = new SubstructureService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var expectedSubstructures = fixture.context.Substructures.ToList().Where(o => o.Project.Id == project.Id);

            // Act
            var actualSubstructures = substructureService.GetSubstructures(project.Id);

            // Assert
            Assert.Equal(expectedSubstructures.Count(), actualSubstructures.Count());
            var substructuresExpectedAndActual = expectedSubstructures.OrderBy(d => d.Name)
                .Zip(actualSubstructures.OrderBy(d => d.Name));
            foreach (var substructurePair in substructuresExpectedAndActual)
            {
                TestHelper.CompareSubstructures(substructurePair.First, substructurePair.Second);
            }
        }

        [Fact]
        public void CreateNewSubstructure()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var substructureService = new SubstructureService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var expectedSubstructure = CreateTestSubstructure(project);

            // Act
            substructureService.CreateSubstructure(expectedSubstructure);

            // Assert
            var actualSubstructure = fixture.context.Substructures.FirstOrDefault(o => o.Name == expectedSubstructure.Name);
            Assert.NotNull(actualSubstructure);
            TestHelper.CompareSubstructures(expectedSubstructure, actualSubstructure);
        }

        private static Substructure CreateTestSubstructure(Project project)
        {
            return new SubstructureBuilder
            {
                Name = "DrainStrat Test",
                Maturity = Maturity.A,
                DryWeight = 7.2,
                Project = project,
                ProjectId = project.Id,
            }
                .WithCostProfile(new SubstructureCostProfileBuilder()
                    .WithYearValue(2030, 2.3)
                    .WithYearValue(2031, 3.3)
                    .WithYearValue(2032, 4.4)
                );
        }
    }
}
