using api.Dtos;
using api.Models;

namespace api.Services;

public interface ITransportService
{
    Task<ProjectDto> CreateTransport(TransportDto transportDto, Guid sourceCaseId);
    Task<Transport> NewCreateTransport(Guid projectId, Guid sourceCaseId, CreateTransportDto transportDto);
    Task<TransportDto> CopyTransport(Guid transportId, Guid sourceCaseId);
    Task<Transport> GetTransport(Guid transportId);
    Task<ProjectDto> UpdateTransport(TransportDto updatedTransportDto);
}
