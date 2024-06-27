using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface INetSaleGasProfileService
    {
        Task<NetSalesGasDto> Generate(Guid caseId);
    }
}
