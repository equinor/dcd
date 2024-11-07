namespace api.Services;

public interface IOpexCostProfileService
{
    Task Generate(Guid caseId);
}
