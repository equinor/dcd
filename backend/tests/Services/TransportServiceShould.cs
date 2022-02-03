using System;
using System.Collections.Generic;
using System.Linq;

using api.Models;
using api.SampleData.Builders;
using api.SampleData.Generators;
using api.Services;

using Xunit;


namespace tests
{
    [Collection("Database collection")]
    public class TransportServiceShould
    {
        private readonly DatabaseFixture fixture;

        public TransportServiceShould()
        {
            fixture = new DatabaseFixture();
        }

        [Fact]
        public void GetTransports()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var transportService = new TransportService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var expectedTransports = fixture.context.Transports.ToList().Where(o => o.Project.Id == project.Id);

            // Act
            var actualTransports = transportService.GetTransports(project.Id);

            // Assert
            Assert.Equal(expectedTransports.Count(), actualTransports.Count());
            var transportsExpectedAndActual = expectedTransports.OrderBy(d => d.Name)
                .Zip(actualTransports.OrderBy(d => d.Name));
            foreach (var transportsPair in transportsExpectedAndActual)
            {
                TestHelper.CompareTransports(transportsPair.First, transportsPair.Second);
            }
        }

        [Fact]
        public void CreateNewTransport()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var transportService = new TransportService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var expectedTransport = CreateTestTransport(project);

            // Act
            var projectResult = transportService.CreateTransport(expectedTransport);

            // Assert
            var actualTransport = projectResult.Transports.FirstOrDefault(o => o.Name == expectedTransport.Name);
            Assert.NotNull(actualTransport);
            TestHelper.CompareTransports(expectedTransport, actualTransport);
        }

        [Fact]
        public void DeleteTransport()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var transportService = new TransportService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var transportToDelete = CreateTestTransport(project);

            fixture.context.Transports.Add(transportToDelete);
            fixture.context.SaveChanges();

            // Act
            var projectResult = transportService.DeleteTransport(transportToDelete);

            // Assert
            var actualDrainageStrategy = projectResult.DrainageStrategies.FirstOrDefault(o => o.Name == transportToDelete.Name);
            Assert.Null(actualDrainageStrategy);
        }

        [Fact]
        public void UpdateTransport()
        {
            // Arrange
            var projectService = new ProjectService(fixture.context);
            var transportService = new TransportService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
            var oldTransport = CreateTestTransport(project);
            fixture.context.Transports.Add(oldTransport);
            fixture.context.SaveChanges();
            var updatedTransport = CreateUpdatedTransport(project);

            // Act
            var projectResult = transportService.UpdateTransport(oldTransport.Id, updatedTransport);

            // Assert
            var actualTransport = projectResult.Transports.FirstOrDefault(o => o.Name == updatedTransport.Name);
            Assert.NotNull(actualTransport);
            TestHelper.CompareTransports(updatedTransport, actualTransport);
        }

        private static Transport CreateUpdatedTransport(Project project)
        {
            return new TransportBuilder
            {
                Name = "Updated Transport",
                Project = project,
                ProjectId = project.Id,
                GasExportPipelineLength = 100,
                OilExportPipelineLength = 100,
            }.WithCostProfile(new TransportCostProfileBuilder()
                .WithYearValue(2023, 55)
                .WithYearValue(2024, 55.7)
                .WithYearValue(2025, 52.3)
            );

        }

        private static Transport CreateTestTransport(Project project)
        {
            return new TransportBuilder
            {
                Name = "Transport Transport",
                Project = project,
                ProjectId = project.Id,
                GasExportPipelineLength = 999,
                OilExportPipelineLength = 999,

            }.WithCostProfile(new TransportCostProfileBuilder().WithYearValue(2023, 10))
            .WithCostProfile(new TransportCostProfileBuilder().WithYearValue(2024, 24));
        }
    }
}
