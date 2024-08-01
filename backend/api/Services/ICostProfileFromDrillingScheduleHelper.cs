using api.Dtos;
using api.Models;

namespace api.Services;

public interface ICostProfileFromDrillingScheduleHelper
{
    Task UpdateCostProfilesForWellsFromDrillingSchedules(List<Guid> drillingScheduleIds);
    Task UpdateCostProfilesForWells(List<Guid> wellIds);
    Task<Exploration> UpdateExplorationCostProfiles(Guid explorationId);
    Task<WellProject> UpdateWellProjectCostProfiles(Guid wellProjectId);
}
