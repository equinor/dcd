using api.Dtos;

namespace api.Services
{
    public interface IGenerateGAndGAdminCostProfile
    {
        Task<GAndGAdminCostDto> Generate(Guid caseId);
    }
}
