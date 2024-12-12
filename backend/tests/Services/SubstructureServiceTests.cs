using api.Context.Recalculation;
using api.Features.Assets.CaseAssets.Substructures.Dtos;
using api.Features.Assets.CaseAssets.Substructures.Dtos.Update;
using api.Features.Assets.CaseAssets.Substructures.Repositories;
using api.Features.Assets.CaseAssets.Substructures.Services;
using api.Features.CaseProfiles.Repositories;
using api.Features.ProjectAccess;
using api.ModelMapping;
using api.Models;

using Microsoft.EntityFrameworkCore;

using NSubstitute;

using Xunit;

namespace tests.Services;

public class SubstructureServiceTests
{
    private readonly SubstructureService _substructureService;
    private readonly ILogger<SubstructureService> _logger = Substitute.For<ILogger<SubstructureService>>();
    private readonly ISubstructureRepository _repository = Substitute.For<ISubstructureRepository>();
    private readonly ICaseRepository _caseRepository = Substitute.For<ICaseRepository>();
    private readonly IMapperService _mapperService = Substitute.For<IMapperService>();
    private readonly IProjectAccessService _projectAccessService = Substitute.For<IProjectAccessService>();
    private readonly IRecalculationService _recalculationService = Substitute.For<IRecalculationService>();

    public SubstructureServiceTests()
    {
        _substructureService = new SubstructureService(_logger,
            _repository,
            _caseRepository,
            _mapperService,
            _projectAccessService,
            _recalculationService
        );
    }

    [Fact]
    public async Task UpdateSubstructure_ShouldUpdateSubstructure_WhenGivenValidInput()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var caseId = Guid.NewGuid();
        var substructureId = Guid.NewGuid();
        var updatedSubstructureDto = new APIUpdateSubstructureDto();

        var existingSubstructure = new Substructure { Id = substructureId };
        _repository.GetSubstructure(substructureId).Returns(existingSubstructure);

        var updatedSubstructure = new Substructure { Id = substructureId };
        _repository.UpdateSubstructure(existingSubstructure).Returns(updatedSubstructure);

        var updatedSubstructureDtoResult = new SubstructureDto();
        _mapperService.MapToDto<Substructure, SubstructureDto>(existingSubstructure, substructureId).Returns(updatedSubstructureDtoResult);

        // Act
        var result = await _substructureService.UpdateSubstructure<BaseUpdateSubstructureDto>(projectId, caseId, substructureId, updatedSubstructureDto);

        // Assert
        Assert.Equal(updatedSubstructureDtoResult, result);
        await _recalculationService.Received(1).SaveChangesAndRecalculateAsync(caseId);
    }

    [Fact]
    public async Task UpdateSubstructure_ShouldThrowException_WhenDbUpdateExceptionOccurs()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var caseId = Guid.NewGuid();
        var substructureId = Guid.NewGuid();
        var updatedSubstructureDto = new APIUpdateSubstructureDto();

        var existingSubstructure = new Substructure { Id = substructureId };
        _repository.GetSubstructure(substructureId).Returns(existingSubstructure);

        _recalculationService.When(r => r.SaveChangesAndRecalculateAsync(caseId)).Do(_ => throw new DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(() => _substructureService.UpdateSubstructure<BaseUpdateSubstructureDto>(projectId, caseId, substructureId, updatedSubstructureDto));
    }
}
