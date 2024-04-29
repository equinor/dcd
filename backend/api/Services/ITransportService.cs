using api.Dtos;
using api.Models;

namespace api.Services;

public interface ITransportService
{
    Task<Transport> CreateTransport(Guid projectId, Guid sourceCaseId, CreateTransportDto transportDto);
    Task<TransportDto> CopyTransport(Guid transportId, Guid sourceCaseId);
    Task<Transport> GetTransport(Guid transportId);
    Task<ProjectDto> UpdateTransport<TDto>(TDto updatedTransportDto, Guid transportId) where TDto : BaseUpdateTransportDto;
}
