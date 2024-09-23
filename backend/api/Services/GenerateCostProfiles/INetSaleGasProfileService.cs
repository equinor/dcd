using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface INetSaleGasProfileService
    {
        Task Generate(Guid caseId);
    }
}
