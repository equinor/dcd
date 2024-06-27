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
    public class TransportServiceTimeSeriesTests
    {
        private readonly TransportTimeSeriesService _transportService;
        private readonly IProjectService _projectService = Substitute.For<IProjectService>();
        private readonly ILoggerFactory _loggerFactory = Substitute.For<ILoggerFactory>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly ITransportTimeSeriesRepository _repository = Substitute.For<ITransportTimeSeriesRepository>();
        private readonly ITransportRepository _transportRepository = Substitute.For<ITransportRepository>();
        private readonly ICaseRepository _caseRepository = Substitute.For<ICaseRepository>();
        private readonly IMapperService _mapperService = Substitute.For<IMapperService>();

        public TransportServiceTimeSeriesTests()
        {
            var options = new DbContextOptionsBuilder<DcdDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = Substitute.For<DcdDbContext>(options);
            _transportService = new TransportTimeSeriesService(
                context,
                _projectService,
                _loggerFactory,
                _mapper,
                _caseRepository,
                _transportRepository,
                _repository,
                _mapperService
            );
        }

        [Fact]
        public async Task UpdateTransportCostProfileOverride_ShouldUpdateTransportCostProfileOverride_WhenGivenValidInput()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var transportId = Guid.NewGuid();
            var costProfileId = Guid.NewGuid();
            var updatedTransportCostProfileOverrideDto = new UpdateTransportCostProfileOverrideDto();

            var existingTransportCostProfileOverride = new TransportCostProfileOverride { Id = costProfileId };
            _repository.GetTransportCostProfileOverride(costProfileId).Returns(existingTransportCostProfileOverride);

            var updatedTransportCostProfileOverride = new TransportCostProfileOverride { Id = costProfileId };
            _repository.UpdateTransportCostProfileOverride(existingTransportCostProfileOverride).Returns(updatedTransportCostProfileOverride);

            var updatedTransportCostProfileOverrideDtoResult = new TransportCostProfileOverrideDto();
            _mapperService.MapToDto<TransportCostProfileOverride, TransportCostProfileOverrideDto>(existingTransportCostProfileOverride, costProfileId).Returns(updatedTransportCostProfileOverrideDtoResult);

            // Act
            var result = await _transportService.UpdateTransportCostProfileOverride(caseId, transportId, costProfileId, updatedTransportCostProfileOverrideDto);

            // Assert
            Assert.Equal(updatedTransportCostProfileOverrideDtoResult, result);
            await _repository.Received(1).SaveChangesAndRecalculateAsync(caseId);
        }

        [Fact]
        public async Task AddOrUpdateTransportCostProfile_ShouldUpdateTransportCostProfile_WhenGivenValidInputForExistingProfile()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var transportId = Guid.NewGuid();
            var profileId = Guid.NewGuid();
            var updatedTransportCostProfileDto = new UpdateTransportCostProfileDto();

            var existingCostProfile = new TransportCostProfile { Id = profileId };
            var existingTransport = new Transport { Id = transportId, CostProfile = existingCostProfile };
            _transportRepository.GetTransportWithCostProfile(transportId).Returns(existingTransport);

            _repository.GetTransportCostProfile(profileId).Returns(existingCostProfile);
            _repository.UpdateTransportCostProfile(existingCostProfile).Returns(existingCostProfile);

            var updatedTransportCostProfileDtoResult = new TransportCostProfileDto { Id = profileId };
            _mapperService.MapToDto<TransportCostProfile, TransportCostProfileDto>(existingCostProfile, existingCostProfile.Id).Returns(updatedTransportCostProfileDtoResult);

            // Act
            var result = await _transportService.AddOrUpdateTransportCostProfile(caseId, transportId, updatedTransportCostProfileDto);

            // Assert
            Assert.Equal(updatedTransportCostProfileDtoResult, result);
            await _repository.Received(1).SaveChangesAndRecalculateAsync(caseId);
        }

        [Fact]
        public async Task AddOrUpdateTransportCostProfile_ShouldAddTransportCostProfile_WhenGivenValidInputForNewProfile()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var transportId = Guid.NewGuid();
            var profileId = Guid.NewGuid();
            var updatedTransportCostProfileDto = new UpdateTransportCostProfileDto();

            var existingTransport = new Transport { Id = transportId };
            _transportRepository.GetTransportWithCostProfile(transportId).Returns(existingTransport);

            var newCostProfile = new TransportCostProfile { Transport = existingTransport };
            _mapperService.MapToEntity(Arg.Any<UpdateTransportCostProfileDto>(), Arg.Any<TransportCostProfile>(), Arg.Any<Guid>())
                          .Returns(newCostProfile);

            _repository.CreateTransportCostProfile(newCostProfile).Returns(newCostProfile);

            var updatedTransportCostProfileDtoResult = new TransportCostProfileDto { Id = profileId };
            _mapperService.MapToDto<TransportCostProfile, TransportCostProfileDto>(newCostProfile, newCostProfile.Id).Returns(updatedTransportCostProfileDtoResult);

            // Act
            var result = await _transportService.AddOrUpdateTransportCostProfile(caseId, transportId, updatedTransportCostProfileDto);

            // Assert
            Assert.Equal(updatedTransportCostProfileDtoResult, result);
            await _repository.Received(1).SaveChangesAndRecalculateAsync(caseId);
        }
    }
}
