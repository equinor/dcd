namespace api.Features.Cases.Recalculation.Types.FuelFlaringLossesProfile;

public interface IFuelFlaringLossesProfileService
{
    Task Generate(Guid caseId);
}
