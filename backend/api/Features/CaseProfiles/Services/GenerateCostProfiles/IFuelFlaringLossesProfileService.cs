namespace api.Features.CaseProfiles.Services.GenerateCostProfiles;

public interface IFuelFlaringLossesProfileService
{
    Task Generate(Guid caseId);
}
