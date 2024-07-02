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
    public class SurfServiceTimeSeriesTests
    {
        private readonly SurfTimeSeriesService _surfService;
        private readonly IProjectService _projectService = Substitute.For<IProjectService>();
        private readonly ILoggerFactory _loggerFactory = Substitute.For<ILoggerFactory>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly ISurfTimeSeriesRepository _repository = Substitute.For<ISurfTimeSeriesRepository>();
        private readonly ISurfRepository _surfRepository = Substitute.For<ISurfRepository>();
        private readonly ICaseRepository _caseRepository = Substitute.For<ICaseRepository>();
        private readonly IMapperService _mapperService = Substitute.For<IMapperService>();

        public SurfServiceTimeSeriesTests()
        {
            var options = new DbContextOptionsBuilder<DcdDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = Substitute.For<DcdDbContext>(options);
            _surfService = new SurfTimeSeriesService(
                context,
                _projectService,
                _loggerFactory,
                _mapper,
                _repository,
                _surfRepository,
                _caseRepository,
                _mapperService
            );
        }

        [Fact]
        public async Task UpdateSurfCostProfileOverride_ShouldUpdateSurfCostProfileOverride_WhenGivenValidInput()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var surfId = Guid.NewGuid();
            var costProfileId = Guid.NewGuid();
            var updatedSurfCostProfileOverrideDto = new UpdateSurfCostProfileOverrideDto();

            var existingSurfCostProfileOverride = new SurfCostProfileOverride { Id = costProfileId };
            _repository.GetSurfCostProfileOverride(costProfileId).Returns(existingSurfCostProfileOverride);

            var updatedSurfCostProfileOverride = new SurfCostProfileOverride { Id = costProfileId };
            _repository.UpdateSurfCostProfileOverride(existingSurfCostProfileOverride).Returns(updatedSurfCostProfileOverride);

            var updatedSurfCostProfileOverrideDtoResult = new SurfCostProfileOverrideDto();
            _mapperService.MapToDto<SurfCostProfileOverride, SurfCostProfileOverrideDto>(updatedSurfCostProfileOverride, costProfileId).Returns(updatedSurfCostProfileOverrideDtoResult);

            // Act
            var result = await _surfService.UpdateSurfCostProfileOverride(caseId, surfId, costProfileId, updatedSurfCostProfileOverrideDto);

            // Assert
            Assert.Equal(updatedSurfCostProfileOverrideDtoResult, result);
            await _repository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task AddOrUpdateSurfCostProfile_ShouldUpdateSurfCostProfile_WhenGivenValidInputForExistingProfile()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var surfId = Guid.NewGuid();
            var profileId = Guid.NewGuid();
            var updatedSurfCostProfileDto = new UpdateSurfCostProfileDto();

            var existingCostProfile = new SurfCostProfile { Id = profileId };
            var existingSurf = new Surf { Id = surfId, CostProfile = existingCostProfile };
            _surfRepository.GetSurfWithCostProfile(surfId).Returns(existingSurf);

            _repository.GetSurfCostProfile(profileId).Returns(existingCostProfile);
            _repository.UpdateSurfCostProfile(existingCostProfile).Returns(existingCostProfile);

            var updatedSurfCostProfileDtoResult = new SurfCostProfileDto { Id = profileId };
            _mapperService.MapToDto<SurfCostProfile, SurfCostProfileDto>(existingCostProfile, existingCostProfile.Id).Returns(updatedSurfCostProfileDtoResult);

            // Act
            var result = await _surfService.AddOrUpdateSurfCostProfile(caseId, surfId, updatedSurfCostProfileDto);

            // Assert
            Assert.Equal(updatedSurfCostProfileDtoResult, result);
            await _repository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task AddOrUpdateSurfCostProfile_ShouldAddSurfCostProfile_WhenGivenValidInputForNewProfile()
        {
            // Arrange
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
            var result = await _surfService.AddOrUpdateSurfCostProfile(caseId, surfId, updatedSurfCostProfileDto);

            // Assert
            Assert.Equal(updatedSurfCostProfileDtoResult, result);
            await _repository.Received(1).SaveChangesAsync();
        }
    }
}
