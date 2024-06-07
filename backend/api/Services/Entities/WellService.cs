using api.Dtos;
using api.Exceptions;
using api.Models;
using api.Repositories;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class WellService : IWellService
{
    private readonly IWellRepository _repository;
    private readonly ILogger<WellService> _logger;
    private readonly IMapperService _mapperService;

    public WellService(
        ILoggerFactory loggerFactory,
        IWellRepository repository,
        IMapperService mapperService
        )
    {
        _logger = loggerFactory.CreateLogger<WellService>();
        _repository = repository;
        _mapperService = mapperService;
    }

    public async Task<WellDto> UpdateWell(Guid wellId, UpdateWellDto updatedWellDto)
    {
        var existingWell = await _repository.GetWell(wellId)
            ?? throw new NotFoundInDBException($"Well with id {wellId} not found");

        _mapperService.MapToEntity(updatedWellDto, existingWell, wellId);

        // TODO: Update project last changed date
        Well updatedWell;
        try
        {
            updatedWell = _repository.UpdateWell(existingWell);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update well with id {wellId}", wellId);
            throw;
        }

        var dto = _mapperService.MapToDto<Well, WellDto>(updatedWell, wellId);
        return dto;
    }
}
