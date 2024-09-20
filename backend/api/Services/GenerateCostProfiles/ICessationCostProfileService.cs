using api.Dtos;

namespace api.Services
{
    public interface ICessationCostProfileService
    {
        Task Generate(Guid caseId);
    }
}
