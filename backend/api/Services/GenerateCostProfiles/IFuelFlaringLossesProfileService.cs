namespace api.Services.GenerateCostProfiles;

public interface IFuelFlaringLossesProfileService
{
    Task Generate(Guid caseId);
}
