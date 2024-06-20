using api.Dtos;
using api.Models;

namespace api.Services;

public interface ITransportService
{
    Task<Transport> CreateTransport(Guid projectId, Guid sourceCaseId, CreateTransportDto transportDto);
    Task<TransportWithProfilesDto> CopyTransport(Guid transportId, Guid sourceCaseId);
    Task<Transport> GetTransport(Guid transportId);
    Task<TransportDto> UpdateTransport<TDto>(Guid caseId, Guid transportId, TDto updatedTransportDto)
            where TDto : BaseUpdateTransportDto;

    Task<TransportCostProfileDto> AddOrUpdateTransportCostProfile(
        Guid caseId,
        Guid transportId,
        UpdateTransportCostProfileDto dto
    );
    Task<TransportCostProfileOverrideDto> CreateTransportCostProfileOverride(
        Guid caseId,
        Guid transportId,
        CreateTransportCostProfileOverrideDto dto
    );
    Task<TransportCostProfileOverrideDto> UpdateTransportCostProfileOverride(
        Guid caseId,
        Guid transportId,
        Guid costProfileId,
        UpdateTransportCostProfileOverrideDto dto);
}
