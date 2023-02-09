using api.Dtos;

namespace api.Services
{
    public interface IGenerateCessationCostProfile
    {
        Task<CessationCostWrapperDto> GenerateAsync(Guid caseId);
    }
}
