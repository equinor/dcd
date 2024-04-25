using api.Dtos;
using api.Models;

namespace api.Services;

public interface ICostProfileFromDrillingScheduleHelper
{
    Task UpdateCostProfilesForWells(List<Guid> wellIds);
    Task<Exploration> UpdateExplorationCostProfilesForCase(Guid caseId);
    Task<WellProject> UpdateWellProjectCostProfilesForCase(Guid caseId);
}
