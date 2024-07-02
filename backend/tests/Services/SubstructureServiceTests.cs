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
    public class SubstructureServiceTests
    {
        private readonly SubstructureService _substructureService;
        private readonly IProjectService _projectService = Substitute.For<IProjectService>();
        private readonly ILoggerFactory _loggerFactory = Substitute.For<ILoggerFactory>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly ISubstructureRepository _repository = Substitute.For<ISubstructureRepository>();
        private readonly ICaseRepository _caseRepository = Substitute.For<ICaseRepository>();
        private readonly IMapperService _mapperService = Substitute.For<IMapperService>();

        public SubstructureServiceTests()
        {
            var options = new DbContextOptionsBuilder<DcdDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = Substitute.For<DcdDbContext>(options);
            _substructureService = new SubstructureService(
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
        public async Task UpdateSubstructure_ShouldUpdateSubstructure_WhenGivenValidInput()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var substructureId = Guid.NewGuid();
            var updatedSubstructureDto = new APIUpdateSubstructureDto();

            var existingSubstructure = new Substructure { Id = substructureId };
            _repository.GetSubstructure(substructureId).Returns(existingSubstructure);

            var updatedSubstructure = new Substructure { Id = substructureId };
            _repository.UpdateSubstructure(existingSubstructure).Returns(updatedSubstructure);

            var updatedSubstructureDtoResult = new SubstructureDto();
            _mapperService.MapToDto<Substructure, SubstructureDto>(updatedSubstructure, substructureId).Returns(updatedSubstructureDtoResult);

            // Act
            var result = await _substructureService.UpdateSubstructure<BaseUpdateSubstructureDto>(caseId, substructureId, updatedSubstructureDto);

            // Assert
            Assert.Equal(updatedSubstructureDtoResult, result);
            await _repository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task UpdateSubstructure_ShouldThrowException_WhenDbUpdateExceptionOccurs()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var substructureId = Guid.NewGuid();
            var updatedSubstructureDto = new APIUpdateSubstructureDto();

            var existingSubstructure = new Substructure { Id = substructureId };
            _repository.GetSubstructure(substructureId).Returns(existingSubstructure);

            _repository.When(r => r.UpdateSubstructure(existingSubstructure)).Do(x => throw new DbUpdateException());

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _substructureService.UpdateSubstructure<BaseUpdateSubstructureDto>(caseId, substructureId, updatedSubstructureDto));
        }
    }
}
