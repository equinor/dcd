using api.Context.Recalculation;
using api.Features.Assets.CaseAssets.Substructures.Dtos;
using api.Features.Assets.CaseAssets.Substructures.Dtos.Update;
using api.Features.Assets.CaseAssets.Substructures.Repositories;
using api.Features.Assets.CaseAssets.Substructures.Services;
using api.Features.CaseProfiles.Repositories;
using api.Features.ProjectAccess;
using api.ModelMapping;
using api.Models;

using NSubstitute;

using Xunit;

namespace tests.Services;

public class SubstructureServiceTimeSeriesTests
{
    private readonly SubstructureTimeSeriesService _substructureService;
    private readonly ILogger<SubstructureService> _logger = Substitute.For<ILogger<SubstructureService>>();
    private readonly ISubstructureRepository _substructureRepository = Substitute.For<ISubstructureRepository>();

    private readonly ISubstructureTimeSeriesRepository _repository = Substitute.For<ISubstructureTimeSeriesRepository>();
    private readonly ICaseRepository _caseRepository = Substitute.For<ICaseRepository>();
    private readonly IMapperService _mapperService = Substitute.For<IMapperService>();
    private readonly IProjectAccessService _projectAccessService = Substitute.For<IProjectAccessService>();
    private readonly IRecalculationService _recalculationService = Substitute.For<IRecalculationService>();

    public SubstructureServiceTimeSeriesTests()
    {
        _substructureService = new SubstructureTimeSeriesService(
            _logger,
            _substructureRepository,
            _repository,
            _caseRepository,
            _mapperService,
            _projectAccessService,
            _recalculationService
        );
    }

    [Fact]
    public async Task UpdateSubstructureCostProfileOverride_ShouldUpdateSubstructureCostProfileOverride_WhenGivenValidInput()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var caseId = Guid.NewGuid();
        var substructureId = Guid.NewGuid();
        var costProfileId = Guid.NewGuid();
        var updatedSubstructureCostProfileOverrideDto = new UpdateSubstructureCostProfileOverrideDto();

        var existingSubstructureCostProfileOverride = new SubstructureCostProfileOverride
        {
            Id = costProfileId,
            Substructure = new Substructure { Id = substructureId }
        };
        _repository.GetSubstructureCostProfileOverride(costProfileId).Returns(existingSubstructureCostProfileOverride);

        var updatedSubstructureCostProfileOverride = new SubstructureCostProfileOverride
        {
            Id = costProfileId,
            Substructure = new Substructure { Id = substructureId }
        };
        _repository.UpdateSubstructureCostProfileOverride(existingSubstructureCostProfileOverride).Returns(updatedSubstructureCostProfileOverride);

        var updatedSubstructureCostProfileOverrideDtoResult = new SubstructureCostProfileOverrideDto();
        _mapperService.MapToDto<SubstructureCostProfileOverride, SubstructureCostProfileOverrideDto>(existingSubstructureCostProfileOverride, costProfileId)
            .Returns(updatedSubstructureCostProfileOverrideDtoResult);

        // Act
        var result = await _substructureService.UpdateSubstructureCostProfileOverride(projectId, caseId, substructureId, costProfileId, updatedSubstructureCostProfileOverrideDto);

        // Assert
        Assert.Equal(updatedSubstructureCostProfileOverrideDtoResult, result);
        await _recalculationService.Received(1).SaveChangesAndRecalculateAsync(caseId);
    }

    [Fact]
    public async Task AddOrUpdateSubstructureCostProfile_ShouldUpdateSubstructureCostProfile_WhenGivenValidInputForExistingProfile()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var caseId = Guid.NewGuid();
        var substructureId = Guid.NewGuid();
        var profileId = Guid.NewGuid();
        var updatedSubstructureCostProfileDto = new UpdateSubstructureCostProfileDto();

        var existingCostProfile = new SubstructureCostProfile
        {
            Id = profileId,
            Substructure = new Substructure { Id = substructureId }
        };
        var existingSubstructure = new Substructure { Id = substructureId, CostProfile = existingCostProfile };
        _substructureRepository.GetSubstructureWithCostProfile(substructureId).Returns(existingSubstructure);

        _repository.GetSubstructureCostProfile(profileId).Returns(existingCostProfile);
        _repository.UpdateSubstructureCostProfile(existingCostProfile).Returns(existingCostProfile);

        var updatedSubstructureCostProfileDtoResult = new SubstructureCostProfileDto { Id = profileId };
        _mapperService.MapToDto<SubstructureCostProfile, SubstructureCostProfileDto>(existingCostProfile, existingCostProfile.Id).Returns(updatedSubstructureCostProfileDtoResult);

        // Act
        var result = await _substructureService.AddOrUpdateSubstructureCostProfile(projectId, caseId, substructureId, updatedSubstructureCostProfileDto);

        // Assert
        Assert.Equal(updatedSubstructureCostProfileDtoResult, result);
        await _recalculationService.Received(1).SaveChangesAndRecalculateAsync(caseId);
    }

    [Fact]
    public async Task AddOrUpdateSubstructureCostProfile_ShouldAddSubstructureCostProfile_WhenGivenValidInputForNewProfile()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var caseId = Guid.NewGuid();
        var substructureId = Guid.NewGuid();
        var profileId = Guid.NewGuid();
        var updatedSubstructureCostProfileDto = new UpdateSubstructureCostProfileDto();

        var existingSubstructure = new Substructure { Id = substructureId };
        _substructureRepository.GetSubstructureWithCostProfile(substructureId).Returns(existingSubstructure);

        var newCostProfile = new SubstructureCostProfile { Substructure = existingSubstructure };
        _mapperService.MapToEntity(Arg.Any<UpdateSubstructureCostProfileDto>(), Arg.Any<SubstructureCostProfile>(), Arg.Any<Guid>())
            .Returns(newCostProfile);

        _repository.CreateSubstructureCostProfile(newCostProfile).Returns(newCostProfile);

        var updatedSubstructureCostProfileDtoResult = new SubstructureCostProfileDto { Id = profileId };
        _mapperService.MapToDto<SubstructureCostProfile, SubstructureCostProfileDto>(newCostProfile, newCostProfile.Id).Returns(updatedSubstructureCostProfileDtoResult);

        // Act
        var result = await _substructureService.AddOrUpdateSubstructureCostProfile(projectId, caseId, substructureId, updatedSubstructureCostProfileDto);

        // Assert
        Assert.Equal(updatedSubstructureCostProfileDtoResult, result);
        await _recalculationService.Received(1).SaveChangesAndRecalculateAsync(caseId);
    }
}
