using api.Dtos;

namespace api.Services.GenerateCostProfiles
{
    public interface IGenerateNetSaleGasProfile
    {
        NetSalesGasDto Generate(Guid caseId);
    }
}
