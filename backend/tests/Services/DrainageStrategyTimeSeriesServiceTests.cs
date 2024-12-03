using api.Exceptions;
using api.Features.Assets.CaseAssets.DrainageStrategies.Dtos;
using api.Features.Assets.CaseAssets.DrainageStrategies.Dtos.Create;
using api.Features.Assets.CaseAssets.DrainageStrategies.Repositories;
using api.Features.Assets.CaseAssets.DrainageStrategies.Services;
using api.Features.CaseProfiles.Enums;
using api.Features.CaseProfiles.Repositories;
using api.Features.ProjectAccess;
using api.Models;
using api.Services;

using NSubstitute;

using Xunit;

namespace tests.Services;

public class DrainageStrategyTimeSeriesServiceTests
{
    private readonly IDrainageStrategyTimeSeriesService _drainageStrategyTimeSeriesService;
    private readonly ILogger<DrainageStrategyService> _logger = Substitute.For<ILogger<DrainageStrategyService>>();
    private readonly IDrainageStrategyTimeSeriesRepository _repository = Substitute.For<IDrainageStrategyTimeSeriesRepository>();
    private readonly IDrainageStrategyRepository _drainageStrategyRepository = Substitute.For<IDrainageStrategyRepository>();
    private readonly ICaseRepository _caseRepository = Substitute.For<ICaseRepository>();
    private readonly IConversionMapperService _conversionMapperService = Substitute.For<IConversionMapperService>();
    private readonly IProjectAccessService _projectAccessService = Substitute.For<IProjectAccessService>();

    public DrainageStrategyTimeSeriesServiceTests()
    {
        _drainageStrategyTimeSeriesService = new DrainageStrategyTimeSeriesService(
            _logger,
            _caseRepository,
            _repository,
            _drainageStrategyRepository,
            _conversionMapperService,
            _projectAccessService
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
        _caseRepository.GetProject(projectId).Returns(project);

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
        _caseRepository.GetProject(projectId).Returns(project);

        var createdProfile = new DeferredGasProduction();
        _repository.CreateDeferredGasProduction(Arg.Any<DeferredGasProduction>()).Returns(createdProfile);

        var expectedDto = new DeferredGasProductionDto();
        _conversionMapperService.MapToDto<DeferredGasProduction, DeferredGasProductionDto>(createdProfile, createdProfile.Id, project.PhysicalUnit).Returns(expectedDto);

        // Act & Assert
        await Assert.ThrowsAsync<ResourceAlreadyExistsException>(() => _drainageStrategyTimeSeriesService.CreateDeferredGasProduction(projectId, caseId, drainageStrategyId, createProfileDto));
    }
}
