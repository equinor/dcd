namespace api.Services;

public interface IStudyCostProfileService
{
    Task Generate(Guid caseId);
}
