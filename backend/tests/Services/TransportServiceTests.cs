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
    public class TransportServiceTests
    {
        private readonly TransportService _transportService;
        private readonly IProjectService _projectService = Substitute.For<IProjectService>();
        private readonly ILoggerFactory _loggerFactory = Substitute.For<ILoggerFactory>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly ITransportRepository _repository = Substitute.For<ITransportRepository>();
        private readonly ICaseRepository _caseRepository = Substitute.For<ICaseRepository>();
        private readonly IMapperService _mapperService = Substitute.For<IMapperService>();
        private readonly IProjectAccessService _projectAccessService = Substitute.For<IProjectAccessService>();


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
                _mapperService,
                _projectAccessService
            );
        }

        [Fact]
        public async Task UpdateTransport_ShouldUpdateTransport_WhenGivenValidInput()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var caseId = Guid.NewGuid();
            var transportId = Guid.NewGuid();
            var updatedTransportDto = new APIUpdateTransportDto();

            var existingTransport = new Transport { Id = transportId };
            _repository.GetTransport(transportId).Returns(existingTransport);

            var updatedTransport = new Transport { Id = transportId };
            _repository.UpdateTransport(existingTransport).Returns(updatedTransport);

            var updatedTransportDtoResult = new TransportDto();
            _mapperService.MapToDto<Transport, TransportDto>(existingTransport, transportId).Returns(updatedTransportDtoResult);

            // Act
            var result = await _transportService.UpdateTransport<BaseUpdateTransportDto>(projectId, caseId, transportId, updatedTransportDto);

            // Assert
            Assert.Equal(updatedTransportDtoResult, result);
            await _repository.Received(1).SaveChangesAndRecalculateAsync(caseId);
        }

        [Fact]
        public async Task UpdateTransport_ShouldThrowException_WhenDbUpdateExceptionOccurs()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var caseId = Guid.NewGuid();
            var transportId = Guid.NewGuid();
            var updatedTransportDto = new APIUpdateTransportDto();

            var existingTransport = new Transport { Id = transportId };
            _repository.GetTransport(transportId).Returns(existingTransport);

            _repository.When(r => r.SaveChangesAndRecalculateAsync(caseId)).Do(x => throw new DbUpdateException());

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _transportService.UpdateTransport<BaseUpdateTransportDto>(projectId, caseId, transportId, updatedTransportDto));
        }
    }
}
