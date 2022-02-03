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

        [Fact]
        public void DeleteSubstructure()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var substructureService = new SubstructureService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var substructureToDelete = CreateTestSubstructure(project);
            fixture.context.Substructures.Add(substructureToDelete);
            fixture.context.SaveChanges();

            // Act
            substructureService.DeleteSubstructure(substructureToDelete.Id);

            // Assert
            var actualSubstructure = fixture.context.Substructures.FirstOrDefault(o => o.Name == substructureToDelete.Name);
            Assert.Null(actualSubstructure);
        }

        [Fact]
        public void ThrowArgumentExceptionIfTryingToDeleteNonExistentSubstructure()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var substructureService = new SubstructureService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var substructureToDelete = CreateTestSubstructure(project);
            fixture.context.Substructures.Add(substructureToDelete);
            fixture.context.SaveChanges();

            // Act, assert
            Assert.Throws<ArgumentException>(() => substructureService.DeleteSubstructure(new Guid()));
        }

        [Fact]
        public void UpdateSubstructure()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var substructureService = new SubstructureService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var expectedSubstructure = CreateTestSubstructure(project);
            fixture.context.Substructures.Add(expectedSubstructure);
            fixture.context.SaveChanges();

            expectedSubstructure.DryWeight = 11.11;
            expectedSubstructure.Maturity = Maturity.D;
            expectedSubstructure.Name = "Updated name";
            expectedSubstructure.CostProfile.Currency = Currency.NOK;
            expectedSubstructure.CostProfile.EPAVersion = "another EPA version";

            // Act
            substructureService.UpdateSubstructure(expectedSubstructure.Id, expectedSubstructure);

            // Assert
            var actualSubstructure = fixture.context.Substructures.FirstOrDefault(o => o.Name == expectedSubstructure.Name);
            Assert.NotNull(actualSubstructure);
            TestHelper.CompareSubstructures(expectedSubstructure, actualSubstructure);
        }

        [Fact]
        public void ThrowArgumentExceptionIfTryingToUpdateNonExistentSubstructure()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var substructureService = new SubstructureService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var expectedSubstructure = CreateTestSubstructure(project);
            fixture.context.Substructures.Add(expectedSubstructure);
            fixture.context.SaveChanges();

            // Act, assert
            Assert.Throws<ArgumentException>(() => substructureService.UpdateSubstructure(new Guid(), expectedSubstructure));
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
                .WithCostProfile(new SubstructureCostProfileBuilder
                {
                    Currency = Currency.USD,
                    EPAVersion = "test EPA"
                }
                    .WithYearValue(2030, 2.3)
                    .WithYearValue(2031, 3.3)
                    .WithYearValue(2032, 4.4)
                );
        }
    }
}
