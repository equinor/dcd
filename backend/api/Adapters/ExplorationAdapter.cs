using api.Dtos;
using api.Models;
using api.Services;

namespace api.Adapters
{
    public class ExplorationAdapter
    {
        private IProjectService _projectService;

        public ExplorationAdapter(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public Exploration Convert(ExplorationDto explorationDto)
        {
            var exploration = new Exploration();
            exploration.Project = _projectService.GetProject(explorationDto.ProjectId);
            exploration.Name = explorationDto.Name;
            exploration.RigMobDemob = explorationDto.RigMobDemob;
            exploration.WellType = explorationDto.WellType;
            exploration.CostProfile = Convert(explorationDto.CostProfile, exploration);
            exploration.DrillingSchedule = Convert(explorationDto.DrillingSchedule, exploration);
            exploration.GAndGAdminCost = Convert(explorationDto.GAndGAdminCost, exploration);
            return exploration;
        }

        private ExplorationCostProfile Convert(ExplorationCostProfileDto? costProfileDto, Exploration exploration)
        {
            if (costProfileDto == null)
            {
                return null!;
            }
            return new ExplorationCostProfile
            {
                Currency = costProfileDto.Currency,
                EPAVersion = costProfileDto.EPAVersion,
                Exploration = exploration,
                YearValues = costProfileDto.YearValues,
            };
        }
        private ExplorationDrillingSchedule Convert(ExplorationDrillingScheduleDto? drillingScheduleDto, Exploration exploration)
        {
            if (drillingScheduleDto == null)
            {
                return null!;
            }
            return new ExplorationDrillingSchedule
            {
                Exploration = exploration,
                YearValues = drillingScheduleDto.YearValues,
            };
        }
        private GAndGAdminCost Convert(GAndGAdminCostDto? gAndGAdminCostDto, Exploration exploration)
        {
            if (gAndGAdminCostDto == null)
            {
                return null!;
            }
            return new GAndGAdminCost
            {
                Currency = gAndGAdminCostDto.Currency,
                EPAVersion = gAndGAdminCostDto.EPAVersion,
                Exploration = exploration,
                YearValues = gAndGAdminCostDto.YearValues,
            };
        }
    }
}
