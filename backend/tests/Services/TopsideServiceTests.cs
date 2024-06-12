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
    public class TopsideServiceTests
    {
        private readonly TopsideService _topsideService;
        private readonly IProjectService _projectService = Substitute.For<IProjectService>();
        private readonly ILoggerFactory _loggerFactory = Substitute.For<ILoggerFactory>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly ITopsideRepository _repository = Substitute.For<ITopsideRepository>();
        private readonly ICaseRepository _caseRepository = Substitute.For<ICaseRepository>();
        private readonly IMapperService _mapperService = Substitute.For<IMapperService>();

        public TopsideServiceTests()
        {
            var options = new DbContextOptionsBuilder<DcdDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = Substitute.For<DcdDbContext>(options);
            _topsideService = new TopsideService(
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
        public async Task UpdateTopside_ShouldUpdateTopside_WhenGivenValidInput()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var topsideId = Guid.NewGuid();
            var updatedTopsideDto = new APIUpdateTopsideDto();

            var existingTopside = new Topside { Id = topsideId };
            _repository.GetTopside(topsideId).Returns(existingTopside);

            var updatedTopside = new Topside { Id = topsideId };
            _repository.UpdateTopside(existingTopside).Returns(updatedTopside);

            var updatedTopsideDtoResult = new TopsideDto();
            _mapperService.MapToDto<Topside, TopsideDto>(updatedTopside, topsideId).Returns(updatedTopsideDtoResult);

            // Act
            var result = await _topsideService.UpdateTopside<BaseUpdateTopsideDto>(caseId, topsideId, updatedTopsideDto);

            // Assert
            Assert.Equal(updatedTopsideDtoResult, result);
            await _repository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task UpdateTopside_ShouldThrowException_WhenDbUpdateExceptionOccurs()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var topsideId = Guid.NewGuid();
            var updatedTopsideDto = new APIUpdateTopsideDto();

            var existingTopside = new Topside { Id = topsideId };
            _repository.GetTopside(topsideId).Returns(existingTopside);

            _repository.When(r => r.UpdateTopside(existingTopside)).Do(x => throw new DbUpdateException());

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _topsideService.UpdateTopside<BaseUpdateTopsideDto>(caseId, topsideId, updatedTopsideDto));
        }

        [Fact]
        public async Task UpdateTopsideCostProfileOverride_ShouldUpdateTopsideCostProfileOverride_WhenGivenValidInput()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var topsideId = Guid.NewGuid();
            var costProfileId = Guid.NewGuid();
            var updatedTopsideCostProfileOverrideDto = new UpdateTopsideCostProfileOverrideDto();

            var existingTopsideCostProfileOverride = new TopsideCostProfileOverride { Id = costProfileId };
            _repository.GetTopsideCostProfileOverride(costProfileId).Returns(existingTopsideCostProfileOverride);

            var updatedTopsideCostProfileOverride = new TopsideCostProfileOverride { Id = costProfileId };
            _repository.UpdateTopsideCostProfileOverride(existingTopsideCostProfileOverride).Returns(updatedTopsideCostProfileOverride);

            var updatedTopsideCostProfileOverrideDtoResult = new TopsideCostProfileOverrideDto();
            _mapperService.MapToDto<TopsideCostProfileOverride, TopsideCostProfileOverrideDto>(updatedTopsideCostProfileOverride, costProfileId).Returns(updatedTopsideCostProfileOverrideDtoResult);

            // Act
            var result = await _topsideService.UpdateTopsideCostProfileOverride(caseId, topsideId, costProfileId, updatedTopsideCostProfileOverrideDto);

            // Assert
            Assert.Equal(updatedTopsideCostProfileOverrideDtoResult, result);
            await _repository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task AddOrUpdateTopsideCostProfile_ShouldUpdateTopsideCostProfile_WhenGivenValidInputForExistingProfile()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var topsideId = Guid.NewGuid();
            var profileId = Guid.NewGuid();
            var updatedTopsideCostProfileDto = new UpdateTopsideCostProfileDto();

            var existingCostProfile = new TopsideCostProfile { Id = profileId };
            var existingTopside = new Topside { Id = topsideId, CostProfile = existingCostProfile };
            _repository.GetTopsideWithCostProfile(topsideId).Returns(existingTopside);

            _repository.GetTopsideCostProfile(profileId).Returns(existingCostProfile);
            _repository.UpdateTopsideCostProfile(existingCostProfile).Returns(existingCostProfile);

            var updatedTopsideCostProfileDtoResult = new TopsideCostProfileDto { Id = profileId };
            _mapperService.MapToDto<TopsideCostProfile, TopsideCostProfileDto>(existingCostProfile, existingCostProfile.Id).Returns(updatedTopsideCostProfileDtoResult);

            // Act
            var result = await _topsideService.AddOrUpdateTopsideCostProfile(caseId, topsideId, updatedTopsideCostProfileDto);

            // Assert
            Assert.Equal(updatedTopsideCostProfileDtoResult, result);
            await _repository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task AddOrUpdateTopsideCostProfile_ShouldAddTopsideCostProfile_WhenGivenValidInputForNewProfile()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var topsideId = Guid.NewGuid();
            var profileId = Guid.NewGuid();
            var updatedTopsideCostProfileDto = new UpdateTopsideCostProfileDto();

            var existingTopside = new Topside { Id = topsideId };
            _repository.GetTopsideWithCostProfile(topsideId).Returns(existingTopside);

            var newCostProfile = new TopsideCostProfile { Topside = existingTopside };
            _mapperService.MapToEntity(Arg.Any<UpdateTopsideCostProfileDto>(), Arg.Any<TopsideCostProfile>(), Arg.Any<Guid>())
                          .Returns(newCostProfile);

            _repository.CreateTopsideCostProfile(newCostProfile).Returns(newCostProfile);

            var updatedTopsideCostProfileDtoResult = new TopsideCostProfileDto { Id = profileId };
            _mapperService.MapToDto<TopsideCostProfile, TopsideCostProfileDto>(newCostProfile, newCostProfile.Id).Returns(updatedTopsideCostProfileDtoResult);

            // Act
            var result = await _topsideService.AddOrUpdateTopsideCostProfile(caseId, topsideId, updatedTopsideCostProfileDto);

            // Assert
            Assert.Equal(updatedTopsideCostProfileDtoResult, result);
            await _repository.Received(1).SaveChangesAsync();
        }
    }
}
