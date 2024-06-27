using api.Context;
using api.Dtos;
using api.Models;
using api.Repositories;
using api.Services;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using NSubstitute;

using Xunit;

namespace tests.Services
{
    public class SurfServiceTests
    {
        private readonly SurfService _surfService;
        private readonly IProjectService _projectService = Substitute.For<IProjectService>();
        private readonly ILoggerFactory _loggerFactory = Substitute.For<ILoggerFactory>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly ISurfRepository _repository = Substitute.For<ISurfRepository>();
        private readonly ICaseRepository _caseRepository = Substitute.For<ICaseRepository>();
        private readonly IMapperService _mapperService = Substitute.For<IMapperService>();

        public SurfServiceTests()
        {
            var options = new DbContextOptionsBuilder<DcdDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = Substitute.For<DcdDbContext>(options);
            _surfService = new SurfService(
                context,
                _projectService,
                _loggerFactory,
                _mapper,
                _repository,
                _caseRepository,
                _mapperService
            );
        }

        [Fact]
        public async Task UpdateSurf_ShouldUpdateSurf_WhenGivenValidInput()
        {
            // Arrange
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
            var result = await _surfService.UpdateSurf<BaseUpdateSurfDto>(caseId, surfId, updatedSurfDto);

            // Assert
            Assert.Equal(updatedSurfDtoResult, result);
            await _repository.Received(1).SaveChangesAndRecalculateAsync(caseId);
        }

        [Fact]
        public async Task UpdateSurf_ShouldThrowException_WhenDbUpdateExceptionOccurs()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var surfId = Guid.NewGuid();
            var updatedSurfDto = new APIUpdateSurfDto();

            var existingSurf = new Surf { Id = surfId };
            _repository.GetSurf(surfId).Returns(existingSurf);

            _repository.When(r => r.SaveChangesAndRecalculateAsync(caseId)).Do(x => throw new DbUpdateException());

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _surfService.UpdateSurf<BaseUpdateSurfDto>(caseId, surfId, updatedSurfDto));
        }
    }
}
