using api.Models;

namespace api.Services
{
    public interface ITransportService
    {
        Transport GetTransport(Guid projectId);
        IEnumerable<Transport> GetTransports(Guid projectId);
        Project UpdateTransport(Guid OldTransportId, Transport transport);

        Project CreateTransport(Transport transport);

        Project DeleteTransport(Transport transport);
    }
}
