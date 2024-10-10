using api.Dtos;

namespace api.Services
{
    public interface ICalculateNPVService
    {
        Task CalculateNPV(Guid caseId);
    }
}
