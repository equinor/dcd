using api.Dtos;
using api.Models;

namespace api.Services;

public interface ITransportTimeSeriesService
{
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
