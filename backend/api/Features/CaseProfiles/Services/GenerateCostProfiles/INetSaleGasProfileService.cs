namespace api.Features.CaseProfiles.Services.GenerateCostProfiles;

public interface INetSaleGasProfileService
{
    Task Generate(Guid caseId);
}
