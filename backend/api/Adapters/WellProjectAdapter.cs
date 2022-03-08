using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public class WellProjectAdapter
    {
        public WellProject Convert(WellProjectDto wellProjectDto)
        {
            var wellProject = new WellProject();
            wellProject.ProjectId = wellProjectDto.ProjectId;
            wellProject.Name = wellProjectDto.Name;
            wellProject.ProducerCount = wellProjectDto.ProducerCount;
            wellProject.GasInjectorCount = wellProjectDto.GasInjectorCount;
            wellProject.WaterInjectorCount = wellProjectDto.WaterInjectorCount;
            wellProject.ArtificialLift = wellProjectDto.ArtificialLift;
            wellProject.RigMobDemob = wellProjectDto.RigMobDemob;
            wellProject.AnnualWellInterventionCost = wellProjectDto.AnnualWellInterventionCost;
            wellProject.PluggingAndAbandonment = wellProjectDto.PluggingAndAbandonment;
            if (wellProjectDto.CostProfile != null)
            {
                wellProject.CostProfile = Convert(wellProjectDto.CostProfile, wellProject);
            }
            if (wellProjectDto.DrillingSchedule != null)
            {
                wellProject.DrillingSchedule = Convert(wellProjectDto.DrillingSchedule, wellProject);
            }
            return wellProject;
        }

        private WellProjectCostProfile Convert(WellProjectCostProfileDto wellProjectCostProfileDto, WellProject wellProject)
        {
            return new WellProjectCostProfile
            {
                WellProject = wellProject,
                EPAVersion = wellProjectCostProfileDto.EPAVersion,
                Currency = wellProjectCostProfileDto.Currency,
                StartYear = wellProjectCostProfileDto.StartYear,
                Values = wellProjectCostProfileDto.Values
            };
        }

        private DrillingSchedule Convert(DrillingScheduleDto wellProjectDrillingScheduleDto, WellProject wellProject)
        {
            return new DrillingSchedule
            {
                WellProject = wellProject,
                StartYear = wellProjectDrillingScheduleDto.StartYear,
                Values = wellProjectDrillingScheduleDto.Values
            };
        }
    }
}
