using api.Dtos;

namespace api.Services;

public interface IWellService
{
    Task<WellDto> UpdateWell(Guid wellId, UpdateWellDto updatedWellDto);
}
