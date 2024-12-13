namespace api.Features.Cases.Recalculation.Types.NetSaleGasProfile;

public interface INetSaleGasProfileService
{
    Task Generate(Guid caseId);
}
