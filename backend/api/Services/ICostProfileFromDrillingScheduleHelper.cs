using api.Dtos;
using api.Models;

namespace api.Services;

public interface ICostProfileFromDrillingScheduleHelper
{
    Task UpdateCostProfilesForWells(List<Guid> wellIds);
    Task<Exploration> UpdateExplorationCostProfilesForCase(Guid caseId);
    Task<Exploration> UpdateExplorationCostProfiles(Guid explorationId);
    Task<WellProject> UpdateWellProjectCostProfilesForCase(Guid caseId);
    Task<WellProject> UpdateWellProjectCostProfiles(Guid wellProjectId);
}
