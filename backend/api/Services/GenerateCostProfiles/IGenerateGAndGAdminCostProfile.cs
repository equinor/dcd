using api.Dtos;

namespace api.Services
{
    public interface IGenerateGAndGAdminCostProfile
    {
        Task<GAndGAdminCostDto> GenerateAsync(Guid caseId);
    }
}
