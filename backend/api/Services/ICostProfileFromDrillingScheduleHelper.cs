using api.Dtos;
using api.Models;

namespace api.Services;

public interface ICostProfileFromDrillingScheduleHelper
{
    Task UpdateCostProfilesForWells(List<Guid> wellIds);
    Task<Exploration> UpdateExplorationCostProfilesForCase(Guid caseId);
    Exploration UpdateExplorationCostProfilesForCase(Exploration exploration, IEnumerable<ExplorationWell> explorationWells);
    Task<WellProject> UpdateWellProjectCostProfilesForCase(Guid caseId);
    WellProject UpdateWellProjectCostProfilesForCase(WellProject wellProject, IEnumerable<WellProjectWell> wellProjectWells);
    IEnumerable<Well> GetAllWells();
    IEnumerable<ExplorationWell> GetAllExplorationWells();
    IEnumerable<WellProjectWell> GetAllWellProjectWells();
}
