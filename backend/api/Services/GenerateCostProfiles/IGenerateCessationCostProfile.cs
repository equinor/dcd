using api.Dtos;

namespace api.Services
{
    public interface IGenerateCessationCostProfile
    {
        CessationCostWrapperDto Generate(Guid caseId);
    }
}
