using api.Dtos;

namespace api.Services
{
    public interface IGenerateCessationCostProfile
    {
        Task<CessationCostWrapperDto> Generate(Guid caseId);
    }
}
