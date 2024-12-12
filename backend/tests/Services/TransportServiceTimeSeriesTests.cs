using api.Context.Recalculation;
using api.Features.Assets.CaseAssets.Transports.Dtos;
using api.Features.Assets.CaseAssets.Transports.Dtos.Update;
using api.Features.Assets.CaseAssets.Transports.Repositories;
using api.Features.Assets.CaseAssets.Transports.Services;
using api.Features.CaseProfiles.Repositories;
using api.Features.ProjectAccess;
using api.ModelMapping;
using api.Models;

using NSubstitute;

using Xunit;

namespace tests.Services;

public class TransportServiceTimeSeriesTests
{
    private readonly TransportTimeSeriesService _transportService;
    private readonly ILogger<TransportService> _logger = Substitute.For<ILogger<TransportService>>();
    private readonly ITransportTimeSeriesRepository _repository = Substitute.For<ITransportTimeSeriesRepository>();
    private readonly ITransportRepository _transportRepository = Substitute.For<ITransportRepository>();
    private readonly ICaseRepository _caseRepository = Substitute.For<ICaseRepository>();
    private readonly IMapperService _mapperService = Substitute.For<IMapperService>();
    private readonly IProjectAccessService _projectAccessService = Substitute.For<IProjectAccessService>();
    private readonly IRecalculationService _recalculationService = Substitute.For<IRecalculationService>();

    public TransportServiceTimeSeriesTests()
    {
        _transportService = new TransportTimeSeriesService(
            _logger,
            _caseRepository,
            _transportRepository,
            _repository,
            _mapperService,
            _projectAccessService,
            _recalculationService
        );
    }

    [Fact]
    public async Task UpdateTransportCostProfileOverride_ShouldUpdateTransportCostProfileOverride_WhenGivenValidInput()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var caseId = Guid.NewGuid();
        var transportId = Guid.NewGuid();
        var costProfileId = Guid.NewGuid();
        var updatedTransportCostProfileOverrideDto = new UpdateTransportCostProfileOverrideDto();

        var existingTransportCostProfileOverride = new TransportCostProfileOverride
        {
            Id = costProfileId,
            Transport = new Transport { Id = transportId }
        };
        _repository.GetTransportCostProfileOverride(costProfileId).Returns(existingTransportCostProfileOverride);

        var updatedTransportCostProfileOverride = new TransportCostProfileOverride
        {
            Id = costProfileId,
            Transport = new Transport { Id = transportId }
        };
        _repository.UpdateTransportCostProfileOverride(existingTransportCostProfileOverride).Returns(updatedTransportCostProfileOverride);

        var updatedTransportCostProfileOverrideDtoResult = new TransportCostProfileOverrideDto();
        _mapperService.MapToDto<TransportCostProfileOverride, TransportCostProfileOverrideDto>(existingTransportCostProfileOverride, costProfileId)
            .Returns(updatedTransportCostProfileOverrideDtoResult);

        // Act
        var result = await _transportService.UpdateTransportCostProfileOverride(projectId, caseId, transportId, costProfileId, updatedTransportCostProfileOverrideDto);

        // Assert
        Assert.Equal(updatedTransportCostProfileOverrideDtoResult, result);
        await _recalculationService.Received(1).SaveChangesAndRecalculateAsync(caseId);
    }

    [Fact]
    public async Task AddOrUpdateTransportCostProfile_ShouldUpdateTransportCostProfile_WhenGivenValidInputForExistingProfile()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var caseId = Guid.NewGuid();
        var transportId = Guid.NewGuid();
        var profileId = Guid.NewGuid();
        var updatedTransportCostProfileDto = new UpdateTransportCostProfileDto();

        var existingCostProfile = new TransportCostProfile
        {
            Id = profileId,
            Transport = new Transport { Id = transportId }
        };
        var existingTransport = new Transport { Id = transportId, CostProfile = existingCostProfile };
        _transportRepository.GetTransportWithCostProfile(transportId).Returns(existingTransport);

        _repository.GetTransportCostProfile(profileId).Returns(existingCostProfile);
        _repository.UpdateTransportCostProfile(existingCostProfile).Returns(existingCostProfile);

        var updatedTransportCostProfileDtoResult = new TransportCostProfileDto { Id = profileId };
        _mapperService.MapToDto<TransportCostProfile, TransportCostProfileDto>(existingCostProfile, existingCostProfile.Id).Returns(updatedTransportCostProfileDtoResult);

        // Act
        var result = await _transportService.AddOrUpdateTransportCostProfile(projectId, caseId, transportId, updatedTransportCostProfileDto);

        // Assert
        Assert.Equal(updatedTransportCostProfileDtoResult, result);
        await _recalculationService.Received(1).SaveChangesAndRecalculateAsync(caseId);
    }

    [Fact]
    public async Task AddOrUpdateTransportCostProfile_ShouldAddTransportCostProfile_WhenGivenValidInputForNewProfile()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var caseId = Guid.NewGuid();
        var transportId = Guid.NewGuid();
        var profileId = Guid.NewGuid();
        var updatedTransportCostProfileDto = new UpdateTransportCostProfileDto();

        var existingTransport = new Transport { Id = transportId };
        _transportRepository.GetTransportWithCostProfile(transportId).Returns(existingTransport);

        var newCostProfile = new TransportCostProfile { Transport = existingTransport };
        _mapperService.MapToEntity(Arg.Any<UpdateTransportCostProfileDto>(), Arg.Any<TransportCostProfile>(), Arg.Any<Guid>())
            .Returns(newCostProfile);

        _repository.CreateTransportCostProfile(newCostProfile).Returns(newCostProfile);

        var updatedTransportCostProfileDtoResult = new TransportCostProfileDto { Id = profileId };
        _mapperService.MapToDto<TransportCostProfile, TransportCostProfileDto>(newCostProfile, newCostProfile.Id).Returns(updatedTransportCostProfileDtoResult);

        // Act
        var result = await _transportService.AddOrUpdateTransportCostProfile(projectId, caseId, transportId, updatedTransportCostProfileDto);

        // Assert
        Assert.Equal(updatedTransportCostProfileDtoResult, result);
        await _recalculationService.Received(1).SaveChangesAndRecalculateAsync(caseId);
    }
}
