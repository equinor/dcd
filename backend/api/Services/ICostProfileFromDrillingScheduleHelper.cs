using api.Dtos;
using api.Models;

namespace api.Services;

public interface ICostProfileFromDrillingScheduleHelper
{
    Task UpdateCostProfilesForWells(List<Guid> wellIds);
    Task<ExplorationDto> UpdateExplorationCostProfilesForCase(Guid caseId);
    ExplorationDto UpdateExplorationCostProfilesForCase(Exploration exploration, IEnumerable<ExplorationWell> explorationWells);
    Task<WellProjectDto> UpdateWellProjectCostProfilesForCase(Guid caseId);
    WellProjectDto UpdateWellProjectCostProfilesForCase(WellProject wellProject, IEnumerable<WellProjectWell> wellProjectWells);
    IEnumerable<Well> GetAllWells();
    IEnumerable<ExplorationWell> GetAllExplorationWells();
    IEnumerable<WellProjectWell> GetAllWellProjectWells();
}
