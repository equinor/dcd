using api.Context;
using api.Dtos;
using api.Models;
using api.Repositories;
using api.Services;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using NSubstitute;

using Xunit;

namespace api.Tests.Services
{
    public class SubstructureServiceTests
    {
        private readonly SubstructureService _substructureService;
        private readonly DcdDbContext _context;
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

            _context = Substitute.For<DcdDbContext>(options);
            _substructureService = new SubstructureService(
                _context,
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

        [Fact]
        public async Task UpdateSubstructureCostProfileOverride_ShouldUpdateSubstructureCostProfileOverride_WhenGivenValidInput()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var substructureId = Guid.NewGuid();
            var costProfileId = Guid.NewGuid();
            var updatedSubstructureCostProfileOverrideDto = new UpdateSubstructureCostProfileOverrideDto();

            var existingSubstructureCostProfileOverride = new SubstructureCostProfileOverride { Id = costProfileId };
            _repository.GetSubstructureCostProfileOverride(costProfileId).Returns(existingSubstructureCostProfileOverride);

            var updatedSubstructureCostProfileOverride = new SubstructureCostProfileOverride { Id = costProfileId };
            _repository.UpdateSubstructureCostProfileOverride(existingSubstructureCostProfileOverride).Returns(updatedSubstructureCostProfileOverride);

            var updatedSubstructureCostProfileOverrideDtoResult = new SubstructureCostProfileOverrideDto();
            _mapperService.MapToDto<SubstructureCostProfileOverride, SubstructureCostProfileOverrideDto>(updatedSubstructureCostProfileOverride, costProfileId).Returns(updatedSubstructureCostProfileOverrideDtoResult);

            // Act
            var result = await _substructureService.UpdateSubstructureCostProfileOverride(caseId, substructureId, costProfileId, updatedSubstructureCostProfileOverrideDto);

            // Assert
            Assert.Equal(updatedSubstructureCostProfileOverrideDtoResult, result);
            await _repository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task AddOrUpdateSubstructureCostProfile_ShouldUpdateSubstructureCostProfile_WhenGivenValidInputForExistingProfile()
        {

            // Arrange
            var caseId = Guid.NewGuid();
            var substructureId = Guid.NewGuid();
            var profileId = Guid.NewGuid();
            var updatedSubstructureCostProfileDto = new UpdateSubstructureCostProfileDto();

            var existingCostProfile = new SubstructureCostProfile { Id = profileId };
            var existingSubstructure = new Substructure { Id = substructureId, CostProfile = existingCostProfile };
            _repository.GetSubstructureWithCostProfile(substructureId).Returns(existingSubstructure);

            _repository.GetSubstructureCostProfile(profileId).Returns(existingCostProfile);
            _repository.UpdateSubstructureCostProfile(existingCostProfile).Returns(existingCostProfile);

            var updatedSubstructureCostProfileDtoResult = new SubstructureCostProfileDto() { Id = profileId };
            _mapperService.MapToDto<SubstructureCostProfile, SubstructureCostProfileDto>(existingCostProfile, existingCostProfile.Id).Returns(updatedSubstructureCostProfileDtoResult);

            // Act
            var result = await _substructureService.AddOrUpdateSubstructureCostProfile(caseId, substructureId, updatedSubstructureCostProfileDto);

            // Assert
            Assert.Equal(updatedSubstructureCostProfileDtoResult, result);
            await _repository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task AddOrUpdateSubstructureCostProfile_ShouldAddSubstructureCostProfile_WhenGivenValidInputForNewProfile()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var substructureId = Guid.NewGuid();
            var profileId = Guid.NewGuid();
            var updatedSubstructureCostProfileDto = new UpdateSubstructureCostProfileDto();

            var existingSubstructure = new Substructure { Id = substructureId };
            _repository.GetSubstructureWithCostProfile(substructureId).Returns(existingSubstructure);

            var newCostProfile = new SubstructureCostProfile { Substructure = existingSubstructure };
            _mapperService.MapToEntity(Arg.Any<UpdateSubstructureCostProfileDto>(), Arg.Any<SubstructureCostProfile>(), Arg.Any<Guid>())
                          .Returns(newCostProfile);

            _repository.CreateSubstructureCostProfile(newCostProfile).Returns(newCostProfile);

            var updatedSubstructureCostProfileDtoResult = new SubstructureCostProfileDto() { Id = profileId };
            _mapperService.MapToDto<SubstructureCostProfile, SubstructureCostProfileDto>(newCostProfile, newCostProfile.Id).Returns(updatedSubstructureCostProfileDtoResult);

            // Act
            var result = await _substructureService.AddOrUpdateSubstructureCostProfile(caseId, substructureId, updatedSubstructureCostProfileDto);

            // Assert
            Assert.Equal(updatedSubstructureCostProfileDtoResult, result);
            await _repository.Received(1).SaveChangesAsync();
        }
    }
}
