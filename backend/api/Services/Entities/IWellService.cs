using api.Dtos;

namespace api.Services;

public interface IWellService
{
    Task<WellDto> UpdateWell(Guid projectId, Guid wellId, UpdateWellDto updatedWellDto);
    Task<WellDto> CreateWell(Guid projectId, CreateWellDto createWellDto);
    Task DeleteWell(Guid projectId, Guid wellId);
    Task<IEnumerable<CaseDto>> GetAffectedCases(Guid wellId);
}
