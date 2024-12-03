namespace api.Features.CaseProfiles.Services.GenerateCostProfiles;

public interface IOpexCostProfileService
{
    Task Generate(Guid caseId);
}
