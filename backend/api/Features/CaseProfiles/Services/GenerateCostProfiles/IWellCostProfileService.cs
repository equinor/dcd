using api.Models;

namespace api.Features.CaseProfiles.Services.GenerateCostProfiles;

public interface IWellCostProfileService
{
    Task UpdateCostProfilesForWellsFromDrillingSchedules(List<Guid> drillingScheduleIds);
    Task UpdateCostProfilesForWells(List<Well> wells);
}
