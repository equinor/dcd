using api.Models;

namespace api.Features.Cases.Recalculation.Types.WellCostProfile;

public interface IWellCostProfileService
{
    Task UpdateCostProfilesForWellsFromDrillingSchedules(List<Guid> drillingScheduleIds);
    Task UpdateCostProfilesForWells(List<Well> wells);
}
