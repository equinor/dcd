using api.Dtos;

namespace api.Services
{
    public interface ICessationCostProfileService
    {
        Task<CessationCostWrapperDto> Generate(Guid caseId);
    }
}
