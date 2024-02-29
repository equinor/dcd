using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface IGenerateNetSaleGasProfile
    {
        Task<NetSalesGasDto> Generate(Guid caseId);
    }
}
