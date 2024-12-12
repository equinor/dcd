using api.Context.Recalculation;
using api.Features.Assets.CaseAssets.Surfs.Dtos;
using api.Features.Assets.CaseAssets.Surfs.Dtos.Update;
using api.Features.Assets.CaseAssets.Surfs.Repositories;
using api.Features.Assets.CaseAssets.Surfs.Services;
using api.Features.CaseProfiles.Repositories;
using api.Features.ProjectAccess;
using api.ModelMapping;
using api.Models;

using NSubstitute;

using Xunit;

namespace tests.Services;

public class SurfServiceTimeSeriesTests
{
    private readonly SurfTimeSeriesService _surfService;
    private readonly ILogger<SurfService> _logger = Substitute.For<ILogger<SurfService>>();
    private readonly ISurfTimeSeriesRepository _repository = Substitute.For<ISurfTimeSeriesRepository>();
    private readonly ISurfRepository _surfRepository = Substitute.For<ISurfRepository>();
    private readonly ICaseRepository _caseRepository = Substitute.For<ICaseRepository>();
    private readonly IMapperService _mapperService = Substitute.For<IMapperService>();
    private readonly IProjectAccessService _projectAccessService = Substitute.For<IProjectAccessService>();
    private readonly IRecalculationService _recalculationService = Substitute.For<IRecalculationService>();

    public SurfServiceTimeSeriesTests()
    {
        _surfService = new SurfTimeSeriesService(
            _logger,
            _repository,
            _surfRepository,
            _caseRepository,
            _mapperService,
            _projectAccessService,
            _recalculationService
        );
    }

    [Fact]
    public async Task UpdateSurfCostProfileOverride_ShouldUpdateSurfCostProfileOverride_WhenGivenValidInput()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var caseId = Guid.NewGuid();
        var surfId = Guid.NewGuid();
        var costProfileId = Guid.NewGuid();
        var updatedSurfCostProfileOverrideDto = new UpdateSurfCostProfileOverrideDto();

        var existingSurfCostProfileOverride = new SurfCostProfileOverride
        {
            Id = costProfileId,
            Surf = new Surf { Id = surfId }
        };
        _repository.GetSurfCostProfileOverride(costProfileId).Returns(existingSurfCostProfileOverride);

        var updatedSurfCostProfileOverride = new SurfCostProfileOverride
        {
            Id = costProfileId,
            Surf = new Surf { Id = surfId }
        };
        _repository.UpdateSurfCostProfileOverride(existingSurfCostProfileOverride).Returns(updatedSurfCostProfileOverride);

        var updatedSurfCostProfileOverrideDtoResult = new SurfCostProfileOverrideDto();
        _mapperService.MapToDto<SurfCostProfileOverride, SurfCostProfileOverrideDto>(existingSurfCostProfileOverride, costProfileId).Returns(updatedSurfCostProfileOverrideDtoResult);

        // Act
        var result = await _surfService.UpdateSurfCostProfileOverride(projectId, caseId, surfId, costProfileId, updatedSurfCostProfileOverrideDto);

        // Assert
        Assert.Equal(updatedSurfCostProfileOverrideDtoResult, result);
        await _recalculationService.Received(1).SaveChangesAndRecalculateAsync(caseId);
    }

    [Fact]
    public async Task AddOrUpdateSurfCostProfile_ShouldUpdateSurfCostProfile_WhenGivenValidInputForExistingProfile()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var caseId = Guid.NewGuid();
        var surfId = Guid.NewGuid();
        var profileId = Guid.NewGuid();
        var updatedSurfCostProfileDto = new UpdateSurfCostProfileDto();

        var existingCostProfile = new SurfCostProfile
        {
            Id = profileId,
            Surf = new Surf { Id = surfId }
        };
        var existingSurf = new Surf { Id = surfId, CostProfile = existingCostProfile };
        _surfRepository.GetSurfWithCostProfile(surfId).Returns(existingSurf);

        _repository.GetSurfCostProfile(profileId).Returns(existingCostProfile);
        _repository.UpdateSurfCostProfile(existingCostProfile).Returns(existingCostProfile);

        var updatedSurfCostProfileDtoResult = new SurfCostProfileDto { Id = profileId };
        _mapperService.MapToDto<SurfCostProfile, SurfCostProfileDto>(existingCostProfile, existingCostProfile.Id).Returns(updatedSurfCostProfileDtoResult);

        // Act
        var result = await _surfService.AddOrUpdateSurfCostProfile(projectId, caseId, surfId, updatedSurfCostProfileDto);

        // Assert
        Assert.Equal(updatedSurfCostProfileDtoResult, result);
        await _recalculationService.Received(1).SaveChangesAndRecalculateAsync(caseId);
    }

    [Fact]
    public async Task AddOrUpdateSurfCostProfile_ShouldAddSurfCostProfile_WhenGivenValidInputForNewProfile()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var caseId = Guid.NewGuid();
        var surfId = Guid.NewGuid();
        var profileId = Guid.NewGuid();
        var updatedSurfCostProfileDto = new UpdateSurfCostProfileDto();

        var existingSurf = new Surf { Id = surfId };
        _surfRepository.GetSurfWithCostProfile(surfId).Returns(existingSurf);

        var newCostProfile = new SurfCostProfile { Surf = existingSurf };
        _mapperService.MapToEntity(Arg.Any<UpdateSurfCostProfileDto>(), Arg.Any<SurfCostProfile>(), Arg.Any<Guid>())
            .Returns(newCostProfile);

        _repository.CreateSurfCostProfile(newCostProfile).Returns(newCostProfile);

        var updatedSurfCostProfileDtoResult = new SurfCostProfileDto { Id = profileId };
        _mapperService.MapToDto<SurfCostProfile, SurfCostProfileDto>(newCostProfile, newCostProfile.Id).Returns(updatedSurfCostProfileDtoResult);

        // Act
        var result = await _surfService.AddOrUpdateSurfCostProfile(projectId, caseId, surfId, updatedSurfCostProfileDto);

        // Assert
        Assert.Equal(updatedSurfCostProfileDtoResult, result);
        await _recalculationService.Received(1).SaveChangesAndRecalculateAsync(caseId);
    }
}
