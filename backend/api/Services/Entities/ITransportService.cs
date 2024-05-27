using api.Dtos;
using api.Models;

namespace api.Services;

public interface ITransportService
{
    Task<Transport> CreateTransport(Guid projectId, Guid sourceCaseId, CreateTransportDto transportDto);
    Task<TransportWithProfilesDto> CopyTransport(Guid transportId, Guid sourceCaseId);
    Task<Transport> GetTransport(Guid transportId);
    Task<ProjectDto> UpdateTransportAndCostProfiles<TDto>(TDto updatedTransportDto, Guid transportId) where TDto : BaseUpdateTransportDto;

    Task<TransportDto> UpdateTransport<TDto>(Guid caseId, Guid transportId, TDto updatedTransportDto)
            where TDto : BaseUpdateTransportDto;

    Task<TransportCostProfileOverrideDto> UpdateTransportCostProfileOverride(
        Guid caseId,
        Guid transportId,
        Guid costProfileId,
        UpdateTransportCostProfileOverrideDto dto);
}
