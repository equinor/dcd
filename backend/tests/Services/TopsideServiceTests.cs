using api.Features.Assets.CaseAssets.Topsides.Dtos;
using api.Features.Assets.CaseAssets.Topsides.Dtos.Update;
using api.Features.Assets.CaseAssets.Topsides.Repositories;
using api.Features.Assets.CaseAssets.Topsides.Services;
using api.Features.CaseProfiles.Repositories;
using api.Features.Cases.Recalculation;
using api.Features.ProjectAccess;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

using NSubstitute;

using Xunit;

namespace tests.Services;

public class TopsideServiceTests
{
    private readonly TopsideService _topsideService;
    private readonly ILogger<TopsideService> _logger = Substitute.For<ILogger<TopsideService>>();
    private readonly ITopsideRepository _repository = Substitute.For<ITopsideRepository>();
    private readonly ICaseRepository _caseRepository = Substitute.For<ICaseRepository>();
    private readonly IMapperService _mapperService = Substitute.For<IMapperService>();
    private readonly IProjectAccessService _projectAccessService = Substitute.For<IProjectAccessService>();
    private readonly IRecalculationService _recalculationService = Substitute.For<IRecalculationService>();

    public TopsideServiceTests()
    {
        _topsideService = new TopsideService(
            _logger,
            _repository,
            _caseRepository,
            _mapperService,
            _projectAccessService,
            _recalculationService
        );
    }

    [Fact]
    public async Task UpdateTopside_ShouldUpdateTopside_WhenGivenValidInput()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var caseId = Guid.NewGuid();
        var topsideId = Guid.NewGuid();
        var updatedTopsideDto = new APIUpdateTopsideDto();

        var existingTopside = new Topside { Id = topsideId };
        _repository.GetTopside(topsideId).Returns(existingTopside);

        var updatedTopside = new Topside { Id = topsideId };
        _repository.UpdateTopside(existingTopside).Returns(updatedTopside);

        var updatedTopsideDtoResult = new TopsideDto();
        _mapperService.MapToDto<Topside, TopsideDto>(existingTopside, topsideId).Returns(updatedTopsideDtoResult);

        // Act
        var result = await _topsideService.UpdateTopside<BaseUpdateTopsideDto>(projectId, caseId, topsideId, updatedTopsideDto);

        // Assert
        Assert.Equal(updatedTopsideDtoResult, result);
        await _recalculationService.Received(1).SaveChangesAndRecalculateAsync(caseId);
    }

    [Fact]
    public async Task UpdateTopside_ShouldThrowException_WhenDbUpdateExceptionOccurs()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var caseId = Guid.NewGuid();
        var topsideId = Guid.NewGuid();
        var updatedTopsideDto = new APIUpdateTopsideDto();

        var existingTopside = new Topside { Id = topsideId };
        _repository.GetTopside(topsideId).Returns(existingTopside);

        _recalculationService.When(r => r.SaveChangesAndRecalculateAsync(caseId)).Do(_ => throw new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(() => _topsideService.UpdateTopside<BaseUpdateTopsideDto>(projectId, caseId, topsideId, updatedTopsideDto));
    }
}
