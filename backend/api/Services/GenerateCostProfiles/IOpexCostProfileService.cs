using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IOpexCostProfileService
    {
        Task Generate(Guid caseId);
    }
}
