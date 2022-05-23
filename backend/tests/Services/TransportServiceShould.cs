using System;
using System.Collections.Generic;
using System.Linq;

using api.Adapters;
using api.Dtos;
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
<<<<<<< Updated upstream
            var projectService = new ProjectService(fixture.context);
            var transportService = new TransportService(fixture.context, projectService);
            var project = fixture.context.Projects.FirstOrDefault();
=======
            var loggerFactory = new LoggerFactory();
            var projectService = new ProjectService(fixture.context, loggerFactory);
            var transportService = new TransportService(fixture.context, projectService, loggerFactory);
            var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
>>>>>>> Stashed changes
            var caseId = project.Cases.FirstOrDefault().Id;
            var expectedTransport = CreateTestTransport(project);

            // Act
            var projectResult = transportService.CreateTransport(expectedTransport, caseId);

            // Assert
            var actualTransport = projectResult.Transports.FirstOrDefault(o => o.Name == expectedTransport.Name);
            Assert.NotNull(actualTransport);
            TestHelper.CompareTransports(expectedTransport, actualTransport);
            var case_ = fixture.context.Cases.FirstOrDefault(o => o.Id == caseId);
            Assert.Equal(actualTransport.Id, case_.TransportLink);
        }

        [Fact]
        public void ThrowNotInDatabaseExceptionWhenCreatingTransportWithBadProjectId()
        {
            // Arrange
            var loggerFactory = new LoggerFactory();
            var projectService = new ProjectService(fixture.context, loggerFactory);
            var transportService = new TransportService(fixture.context, projectService, loggerFactory);
            var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
            var caseId = project.Cases.FirstOrDefault().Id;
            var expectedTransport = CreateTestTransport(new Project { Id = new Guid() });

            // Act, assert
            Assert.Throws<NotFoundInDBException>(() => transportService.CreateTransport(expectedTransport, caseId));
        }

        [Fact]
        public void ThrowNotFoundInDatabaseExceptionWhenCreatingTransportWithBadCaseId()
        {
            // Arrange
            var loggerFactory = new LoggerFactory();
            var projectService = new ProjectService(fixture.context, loggerFactory);
            var transportService = new TransportService(fixture.context, projectService, loggerFactory);
            var project = fixture.context.Projects.FirstOrDefault(o => o.Cases.Any());
            var expectedTransport = CreateTestTransport(project);

            // Act, assert
            Assert.Throws<NotFoundInDBException>(() => transportService.CreateTransport(expectedTransport, new Guid()));
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
            fixture.context.Cases.Add(new Case
            {
                Project = project,
                TransportLink = transportToDelete.Id
            });
            fixture.context.SaveChanges();

            // Act
            var projectResult = transportService.DeleteTransport(transportToDelete.Id);

            // Assert
            var actualDrainageStrategy = projectResult.DrainageStrategies.FirstOrDefault(o => o.Name == transportToDelete.Name);
            Assert.Null(actualDrainageStrategy);
            var casesWithTransportLink = projectResult.Cases.Where(o => o.TransportLink == transportToDelete.Id);
            Assert.Empty(casesWithTransportLink);
        }

        [Fact]
        public void ThrowArgumentExceptionIfTryingToDeleteNonExistentTransport()
        {
            // Arrange
            var loggerFactory = new LoggerFactory();
            var projectService = new ProjectService(fixture.context, loggerFactory);
            var transportService = new TransportService(fixture.context, projectService, loggerFactory);
            var project = fixture.context.Projects.FirstOrDefault();
            var transportToDelete = CreateTestTransport(project);
            fixture.context.Transports.Add(transportToDelete);
            fixture.context.SaveChanges();

            // Act, assert
            Assert.Throws<ArgumentException>(() => transportService.DeleteTransport(new Guid()));
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
            var updatedTransport = CreateUpdatedTransport(project, oldTransport);
            updatedTransport.Id = oldTransport.Id;

            // Act
            var projectResult = transportService.UpdateTransport(updatedTransport);

            // Assert
            var actualTransport = projectResult.Transports.FirstOrDefault(o => o.Name == updatedTransport.Name);
            Assert.NotNull(actualTransport);
            TestHelper.CompareTransports(updatedTransport, actualTransport);
        }

        private static TransportDto CreateUpdatedTransport(Project project, Transport oldTransport)
        {
            return TransportDtoAdapter.Convert(new TransportBuilder
            {
                Id = oldTransport.Id,
                Name = "Updated Transport",
                Project = project,
                ProjectId = project.Id,
                GasExportPipelineLength = 100,
                OilExportPipelineLength = 100,
            }.WithCostProfile(new TransportCostProfile()
            {
                Currency = Currency.USD,
                StartYear = 2030,
                Values = new double[] { 13.4, 18.9, 34.3 }
            }
            ));
        }

        [Fact]
        public void ThrowArgumentExceptionIfTryingToUpdateNonExistentTransport()
        {
            // Arrange
            var loggerFactory = new LoggerFactory();
            var projectService = new ProjectService(fixture.context, loggerFactory);
            var transportService = new TransportService(fixture.context, projectService, loggerFactory);
            var project = fixture.context.Projects.FirstOrDefault();
            var oldTransport = CreateTestTransport(project);
            fixture.context.Transports.Add(oldTransport);
            fixture.context.SaveChanges();
            var updatedTransport = CreateUpdatedTransport(project, oldTransport);

            // Act, assert
            Assert.Throws<ArgumentException>(() => transportService.UpdateTransport(updatedTransport));
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

            }.WithCostProfile(new TransportCostProfile()
            {
                Currency = Currency.USD,
                StartYear = 2030,
                Values = new double[] { 13.4, 18.9, 34.3 }
            })
            .WithCostProfile(new TransportCostProfile()
            {
                Currency = Currency.USD,
                StartYear = 2030,
                Values = new double[] { 13.4, 18.9, 34.3 }
            });
        }
    }
}
