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
    public class TransportServiceTests
    {
        private readonly TransportService _transportService;
        private readonly IProjectService _projectService = Substitute.For<IProjectService>();
        private readonly ILoggerFactory _loggerFactory = Substitute.For<ILoggerFactory>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly ITransportRepository _repository = Substitute.For<ITransportRepository>();
        private readonly ICaseRepository _caseRepository = Substitute.For<ICaseRepository>();
        private readonly IMapperService _mapperService = Substitute.For<IMapperService>();

        public TransportServiceTests()
        {
            var options = new DbContextOptionsBuilder<DcdDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = Substitute.For<DcdDbContext>(options);
            _transportService = new TransportService(
                context,
                _projectService,
                _loggerFactory,
                _mapper,
                _caseRepository,
                _repository,
                _mapperService
            );
        }

        [Fact]
        public async Task UpdateTransport_ShouldUpdateTransport_WhenGivenValidInput()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var transportId = Guid.NewGuid();
            var updatedTransportDto = new APIUpdateTransportDto();

            var existingTransport = new Transport { Id = transportId };
            _repository.GetTransport(transportId).Returns(existingTransport);

            var updatedTransport = new Transport { Id = transportId };
            _repository.UpdateTransport(existingTransport).Returns(updatedTransport);

            var updatedTransportDtoResult = new TransportDto();
            _mapperService.MapToDto<Transport, TransportDto>(updatedTransport, transportId).Returns(updatedTransportDtoResult);

            // Act
            var result = await _transportService.UpdateTransport<BaseUpdateTransportDto>(caseId, transportId, updatedTransportDto);

            // Assert
            Assert.Equal(updatedTransportDtoResult, result);
            await _repository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task UpdateTransport_ShouldThrowException_WhenDbUpdateExceptionOccurs()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            var transportId = Guid.NewGuid();
            var updatedTransportDto = new APIUpdateTransportDto();

            var existingTransport = new Transport { Id = transportId };
            _repository.GetTransport(transportId).Returns(existingTransport);

            _repository.When(r => r.UpdateTransport(existingTransport)).Do(x => throw new DbUpdateException());

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _transportService.UpdateTransport<BaseUpdateTransportDto>(caseId, transportId, updatedTransportDto));
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
            _mapperService.MapToDto<TransportCostProfileOverride, TransportCostProfileOverrideDto>(updatedTransportCostProfileOverride, costProfileId).Returns(updatedTransportCostProfileOverrideDtoResult);

            // Act
            var result = await _transportService.UpdateTransportCostProfileOverride(caseId, transportId, costProfileId, updatedTransportCostProfileOverrideDto);

            // Assert
            Assert.Equal(updatedTransportCostProfileOverrideDtoResult, result);
            await _repository.Received(1).SaveChangesAsync();
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
            _repository.GetTransportWithCostProfile(transportId).Returns(existingTransport);

            _repository.GetTransportCostProfile(profileId).Returns(existingCostProfile);
            _repository.UpdateTransportCostProfile(existingCostProfile).Returns(existingCostProfile);

            var updatedTransportCostProfileDtoResult = new TransportCostProfileDto { Id = profileId };
            _mapperService.MapToDto<TransportCostProfile, TransportCostProfileDto>(existingCostProfile, existingCostProfile.Id).Returns(updatedTransportCostProfileDtoResult);

            // Act
            var result = await _transportService.AddOrUpdateTransportCostProfile(caseId, transportId, updatedTransportCostProfileDto);

            // Assert
            Assert.Equal(updatedTransportCostProfileDtoResult, result);
            await _repository.Received(1).SaveChangesAsync();
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
            _repository.GetTransportWithCostProfile(transportId).Returns(existingTransport);

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
            await _repository.Received(1).SaveChangesAsync();
        }
    }
}
