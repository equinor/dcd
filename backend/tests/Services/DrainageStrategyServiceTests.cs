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
    public class DrainageStrategyServiceTests
    {
        private readonly IDrainageStrategyService _drainageStrategyService;
        private readonly IProjectService _projectService = Substitute.For<IProjectService>();
        private readonly ILoggerFactory _loggerFactory = Substitute.For<ILoggerFactory>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly IDrainageStrategyRepository _repository = Substitute.For<IDrainageStrategyRepository>();
        private readonly ICaseRepository _caseRepository = Substitute.For<ICaseRepository>();
        private readonly IConversionMapperService _conversionMapperService = Substitute.For<IConversionMapperService>();
        private readonly IProjectRepository _projectRepository = Substitute.For<IProjectRepository>();

        public DrainageStrategyServiceTests()
        {
            var options = new DbContextOptionsBuilder<DcdDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = Substitute.For<DcdDbContext>(options);
            _drainageStrategyService = new DrainageStrategyService(
                context,
                _projectService,
                _loggerFactory,
                _mapper,
                _caseRepository,
                _repository,
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
            _repository.GetDrainageStrategy(drainageStrategyId).Returns(drainageStrategy);

            _repository.DrainageStrategyHasProfile(drainageStrategyId, DrainageStrategyProfileNames.DeferredGasProduction).Returns(false);

            var project = new Project();
            _projectRepository.GetProject(projectId).Returns(project);

            var createdProfile = new DeferredGasProduction();
            _repository.CreateDeferredGasProduction(Arg.Any<DeferredGasProduction>()).Returns(createdProfile);

            var expectedDto = new DeferredGasProductionDto();
            _conversionMapperService.MapToDto<DeferredGasProduction, DeferredGasProductionDto>(createdProfile, createdProfile.Id, project.PhysicalUnit).Returns(expectedDto);

            // Act
            var result = await _drainageStrategyService.CreateDeferredGasProduction(projectId, caseId, drainageStrategyId, createProfileDto);

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
            _repository.GetDrainageStrategy(drainageStrategyId).Returns(drainageStrategy);

            _repository.DrainageStrategyHasProfile(drainageStrategyId, DrainageStrategyProfileNames.DeferredGasProduction).Returns(true);

            var project = new Project();
            _projectRepository.GetProject(projectId).Returns(project);

            var createdProfile = new DeferredGasProduction();
            _repository.CreateDeferredGasProduction(Arg.Any<DeferredGasProduction>()).Returns(createdProfile);

            var expectedDto = new DeferredGasProductionDto();
            _conversionMapperService.MapToDto<DeferredGasProduction, DeferredGasProductionDto>(createdProfile, createdProfile.Id, project.PhysicalUnit).Returns(expectedDto);

            // Act & Assert
            await Assert.ThrowsAsync<ResourceAlreadyExistsException>(() => _drainageStrategyService.CreateDeferredGasProduction(projectId, caseId, drainageStrategyId, createProfileDto));
        }
    }
}
