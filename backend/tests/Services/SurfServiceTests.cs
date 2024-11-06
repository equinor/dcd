using api.Dtos;
using api.Models;
using api.Repositories;
using api.Services;

using Microsoft.EntityFrameworkCore;

using NSubstitute;

using Xunit;

namespace tests.Services
{
    public class SurfServiceTests
    {
        private readonly SurfService _surfService;
        private readonly ILogger<SurfService> _logger = Substitute.For<ILogger<SurfService>>();
        private readonly ISurfRepository _repository = Substitute.For<ISurfRepository>();
        private readonly ICaseRepository _caseRepository = Substitute.For<ICaseRepository>();
        private readonly IMapperService _mapperService = Substitute.For<IMapperService>();
        private readonly IProjectAccessService _projectAccessService = Substitute.For<IProjectAccessService>();

        public SurfServiceTests()
        {
            _surfService = new SurfService(
                _logger,
                _repository,
                _caseRepository,
                _mapperService,
                _projectAccessService
            );
        }

        [Fact]
        public async Task UpdateSurf_ShouldUpdateSurf_WhenGivenValidInput()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var caseId = Guid.NewGuid();
            var surfId = Guid.NewGuid();
            var updatedSurfDto = new APIUpdateSurfDto();

            var existingSurf = new Surf { Id = surfId };
            _repository.GetSurf(surfId).Returns(existingSurf);

            var updatedSurf = new Surf { Id = surfId };
            _repository.UpdateSurf(existingSurf).Returns(updatedSurf);

            var updatedSurfDtoResult = new SurfDto();
            _mapperService.MapToDto<Surf, SurfDto>(existingSurf, surfId).Returns(updatedSurfDtoResult);

            // Act
            var result = await _surfService.UpdateSurf<BaseUpdateSurfDto>(projectId, caseId, surfId, updatedSurfDto);

            // Assert
            Assert.Equal(updatedSurfDtoResult, result);
            await _repository.Received(1).SaveChangesAndRecalculateAsync(caseId);
        }

        [Fact]
        public async Task UpdateSurf_ShouldThrowException_WhenDbUpdateExceptionOccurs()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var caseId = Guid.NewGuid();
            var surfId = Guid.NewGuid();
            var updatedSurfDto = new APIUpdateSurfDto();

            var existingSurf = new Surf { Id = surfId };
            _repository.GetSurf(surfId).Returns(existingSurf);

            _repository.When(r => r.SaveChangesAndRecalculateAsync(caseId)).Do(x => throw new DbUpdateException());

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _surfService.UpdateSurf<BaseUpdateSurfDto>(projectId, caseId, surfId, updatedSurfDto));
        }
    }
}
