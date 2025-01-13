using System.Linq.Expressions;

using api.Features.Assets.CaseAssets.Transports.Dtos;
using api.Features.Assets.CaseAssets.Transports.Dtos.Update;
using api.Models;

namespace api.Features.Assets.CaseAssets.Transports.Services;

public interface ITransportService
{
    Task<TransportDto> UpdateTransport<TDto>(Guid projectId, Guid caseId, Guid transportId, TDto updatedTransportDto)
            where TDto : BaseUpdateTransportDto;
}
