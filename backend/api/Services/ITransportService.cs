using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface ITransportService
    {
        Task<ProjectDto> CreateTransport(TransportDto transportDto, Guid sourceCaseId);
        Task<Transport> NewCreateTransport(TransportDto transportDto, Guid sourceCaseId);
        Task<TransportDto> CopyTransport(Guid transportId, Guid sourceCaseId);
        Task<ProjectDto> DeleteTransport(Guid transportId);
        Task<Transport> GetTransport(Guid transportId);
        Task<ProjectDto> UpdateTransport(TransportDto updatedTransportDto);
        Task<TransportDto> NewUpdateTransport(TransportDto updatedTransportDto);
    }
}
