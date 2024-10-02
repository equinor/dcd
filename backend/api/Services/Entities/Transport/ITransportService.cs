using System.Linq.Expressions;

using api.Dtos;
using api.Models;

namespace api.Services;

public interface ITransportService
{
    Task<Transport> GetTransportWithIncludes(Guid transportId, params Expression<Func<Transport, object>>[] includes);
    Task<TransportDto> UpdateTransport<TDto>(Guid projectId, Guid caseId, Guid transportId, TDto updatedTransportDto)
            where TDto : BaseUpdateTransportDto;
}
