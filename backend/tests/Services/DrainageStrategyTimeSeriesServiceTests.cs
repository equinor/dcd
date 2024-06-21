using api.Context;
using api.Dtos;
using api.Enums;
using api.Exceptions;
using api.Models;
using api.Repositories;
using api.Services;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using NSubstitute;

using Xunit;

namespace api.Tests.Services
{
    public class DrainageStrategyTimeSeriesServiceTests
    {
        private readonly IDrainageStrategyTimeSeriesService _drainageStrategyTimeSeriesService;
        private readonly IProjectService _projectService = Substitute.For<IProjectService>();
        private readonly ILoggerFactory _loggerFactory = Substitute.For<ILoggerFactory>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly IDrainageStrategyTimeSeriesRepository _repository = Substitute.For<IDrainageStrategyTimeSeriesRepository>();
        private readonly IDrainageStrategyRepository _drainageStrategyRepository = Substitute.For<IDrainageStrategyRepository>();
        private readonly ICaseRepository _caseRepository = Substitute.For<ICaseRepository>();
        private readonly IConversionMapperService _conversionMapperService = Substitute.For<IConversionMapperService>();
        private readonly IProjectRepository _projectRepository = Substitute.For<IProjectRepository>();

        public DrainageStrategyTimeSeriesServiceTests()
        {
            var options = new DbContextOptionsBuilder<DcdDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = Substitute.For<DcdDbContext>(options);
            _drainageStrategyTimeSeriesService = new DrainageStrategyTimeSeriesService(
                _loggerFactory,
                _caseRepository,
                _repository,
                _drainageStrategyRepository,
                _conversionMapperService,
                _projectRepository
            );
        }

        [Fact]
        public async Task CreateDeferredGasProduction_ShouldReturnDto_WhenSuccessful()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var caseId = Guid.NewGuid();
            var drainageStrategyId = Guid.NewGuid();
            var createProfileDto = new CreateDeferredGasProductionDto();

            var drainageStrategy = new DrainageStrategy();
            _drainageStrategyRepository.GetDrainageStrategy(drainageStrategyId).Returns(drainageStrategy);

            _drainageStrategyRepository.DrainageStrategyHasProfile(drainageStrategyId, DrainageStrategyProfileNames.DeferredGasProduction).Returns(false);

            var project = new Project();
            _projectRepository.GetProject(projectId).Returns(project);

            var createdProfile = new DeferredGasProduction();
            _repository.CreateDeferredGasProduction(Arg.Any<DeferredGasProduction>()).Returns(createdProfile);

            var expectedDto = new DeferredGasProductionDto();
            _conversionMapperService.MapToDto<DeferredGasProduction, DeferredGasProductionDto>(createdProfile, createdProfile.Id, project.PhysicalUnit).Returns(expectedDto);

            // Act
            var result = await _drainageStrategyTimeSeriesService.CreateDeferredGasProduction(projectId, caseId, drainageStrategyId, createProfileDto);

            // Assert
            Assert.Equal(expectedDto, result);
        }

        [Fact]
        public async Task CreateDeferredGasProduction_ShouldThrow_WhenResourceExists()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var caseId = Guid.NewGuid();
            var drainageStrategyId = Guid.NewGuid();
            var createProfileDto = new CreateDeferredGasProductionDto();

            var drainageStrategy = new DrainageStrategy();
            _drainageStrategyRepository.GetDrainageStrategy(drainageStrategyId).Returns(drainageStrategy);

            _drainageStrategyRepository.DrainageStrategyHasProfile(drainageStrategyId, DrainageStrategyProfileNames.DeferredGasProduction).Returns(true);

            var project = new Project();
            _projectRepository.GetProject(projectId).Returns(project);

            var createdProfile = new DeferredGasProduction();
            _repository.CreateDeferredGasProduction(Arg.Any<DeferredGasProduction>()).Returns(createdProfile);

            var expectedDto = new DeferredGasProductionDto();
            _conversionMapperService.MapToDto<DeferredGasProduction, DeferredGasProductionDto>(createdProfile, createdProfile.Id, project.PhysicalUnit).Returns(expectedDto);

            // Act & Assert
            await Assert.ThrowsAsync<ResourceAlreadyExistsException>(() => _drainageStrategyTimeSeriesService.CreateDeferredGasProduction(projectId, caseId, drainageStrategyId, createProfileDto));
        }
    }
}
