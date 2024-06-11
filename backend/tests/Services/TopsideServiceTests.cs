using api.Context;
using api.Dtos;
using api.Models;
using api.Repositories;
using api.Services;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using NSubstitute;

using System;
using System.Threading.Tasks;

using Xunit;

namespace api.Tests.Services
{
    public class TopsideServiceTests
    {
        private readonly TopsideService _topsideService;
        private readonly DcdDbContext _context; // = Substitute.For<DcdDbContext>();
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

            _context = Substitute.For<DcdDbContext>(options);
            _topsideService = new TopsideService(
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
            await _repository.Received().SaveChangesAsync();
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
            await _repository.Received().SaveChangesAsync();
        }
    }
}
