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
    public class SubstructureServiceTimeSeriesTests
    {
        private readonly SubstructureTimeSeriesService _substructureService;
        private readonly IProjectService _projectService = Substitute.For<IProjectService>();
        private readonly ILoggerFactory _loggerFactory = Substitute.For<ILoggerFactory>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly ISubstructureRepository _substructureRepository = Substitute.For<ISubstructureRepository>();

        private readonly ISubstructureTimeSeriesRepository _repository = Substitute.For<ISubstructureTimeSeriesRepository>();
        private readonly ICaseRepository _caseRepository = Substitute.For<ICaseRepository>();
        private readonly IMapperService _mapperService = Substitute.For<IMapperService>();

        public SubstructureServiceTimeSeriesTests()
        {
            var options = new DbContextOptionsBuilder<DcdDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = Substitute.For<DcdDbContext>(options);
            _substructureService = new SubstructureTimeSeriesService(
                context,
                _projectService,
                _loggerFactory,
                _mapper,
                _substructureRepository,
                _repository,
                _caseRepository,
                _mapperService
            );
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
            _substructureRepository.GetSubstructureWithCostProfile(substructureId).Returns(existingSubstructure);

            _repository.GetSubstructureCostProfile(profileId).Returns(existingCostProfile);
            _repository.UpdateSubstructureCostProfile(existingCostProfile).Returns(existingCostProfile);

            var updatedSubstructureCostProfileDtoResult = new SubstructureCostProfileDto { Id = profileId };
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
            _substructureRepository.GetSubstructureWithCostProfile(substructureId).Returns(existingSubstructure);

            var newCostProfile = new SubstructureCostProfile { Substructure = existingSubstructure };
            _mapperService.MapToEntity(Arg.Any<UpdateSubstructureCostProfileDto>(), Arg.Any<SubstructureCostProfile>(), Arg.Any<Guid>())
                          .Returns(newCostProfile);

            _repository.CreateSubstructureCostProfile(newCostProfile).Returns(newCostProfile);

            var updatedSubstructureCostProfileDtoResult = new SubstructureCostProfileDto { Id = profileId };
            _mapperService.MapToDto<SubstructureCostProfile, SubstructureCostProfileDto>(newCostProfile, newCostProfile.Id).Returns(updatedSubstructureCostProfileDtoResult);

            // Act
            var result = await _substructureService.AddOrUpdateSubstructureCostProfile(caseId, substructureId, updatedSubstructureCostProfileDto);

            // Assert
            Assert.Equal(updatedSubstructureCostProfileDtoResult, result);
            await _repository.Received(1).SaveChangesAsync();
        }
    }
}
