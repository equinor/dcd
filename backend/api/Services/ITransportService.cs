using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface ITransportService
    {
        TransportDto CopyTransport(Guid transportId, Guid sourceCaseId);
        ProjectDto CreateTransport(TransportDto transportDto, Guid sourceCaseId);
        ProjectDto DeleteTransport(Guid transportId);
        Transport GetTransport(Guid transportId);
        Transport NewCreateTransport(TransportDto transportDto, Guid sourceCaseId);
        TransportDto NewUpdateTransport(TransportDto updatedTransportDto);
        ProjectDto UpdateTransport(TransportDto updatedTransportDto);
    }
}
