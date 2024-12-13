namespace api.Features.Cases.Recalculation.Types.Co2EmissionsProfile;

public interface ICo2EmissionsProfileService
{
    Task Generate(Guid caseId);
}
