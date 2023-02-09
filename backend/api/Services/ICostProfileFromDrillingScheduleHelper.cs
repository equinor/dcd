using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface ICostProfileFromDrillingScheduleHelper
    {
        void UpdateCostProfilesForWells(List<Guid> wellIds);
        ExplorationDto UpdateExplorationCostProfilesForCase(Exploration exploration, IEnumerable<ExplorationWell> explorationWells);
        ExplorationDto UpdateExplorationCostProfilesForCase(Guid caseId);
        WellProjectDto UpdateWellProjectCostProfilesForCase(Guid caseId);
        WellProjectDto UpdateWellProjectCostProfilesForCase(WellProject wellProject, IEnumerable<WellProjectWell> wellProjectWells);
    }
}
