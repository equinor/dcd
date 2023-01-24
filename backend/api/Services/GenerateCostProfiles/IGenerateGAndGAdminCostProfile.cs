using api.Dtos;

namespace api.Services
{
    public interface IGenerateGAndGAdminCostProfile
    {
        GAndGAdminCostDto Generate(Guid caseId);
    }
}
