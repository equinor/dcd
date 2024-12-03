using api.Features.Assets.CaseAssets.Transports.Dtos;
using api.Features.Assets.CaseAssets.Transports.Dtos.Create;
using api.Features.Assets.CaseAssets.Transports.Dtos.Update;

namespace api.Features.Assets.CaseAssets.Transports.Services;

public interface ITransportTimeSeriesService
{
    Task<TransportCostProfileDto> AddOrUpdateTransportCostProfile(
        Guid projectId,
        Guid caseId,
        Guid transportId,
        UpdateTransportCostProfileDto dto
    );
    Task<TransportCostProfileOverrideDto> CreateTransportCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid transportId,
        CreateTransportCostProfileOverrideDto dto
    );
    Task<TransportCostProfileOverrideDto> UpdateTransportCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid transportId,
        Guid costProfileId,
        UpdateTransportCostProfileOverrideDto dto);
}
