using api.Dtos;
using api.Models;

namespace api.Services;

public interface ITransportService
{
    Task<Transport> CreateTransport(Guid projectId, Guid sourceCaseId, CreateTransportDto transportDto);
    Task<Transport> GetTransport(Guid transportId);
    Task<TransportDto> UpdateTransport<TDto>(Guid caseId, Guid transportId, TDto updatedTransportDto)
            where TDto : BaseUpdateTransportDto;
}
