using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public class WellProjectAdapter
    {
        public WellProject Convert(WellProjectDto wellProjectDto)
        {
            var wellProject = new WellProject
            {
                Id = wellProjectDto.Id,
                ProjectId = wellProjectDto.ProjectId,
                Name = wellProjectDto.Name,
                ProducerCount = wellProjectDto.ProducerCount,
                GasInjectorCount = wellProjectDto.GasInjectorCount,
                WaterInjectorCount = wellProjectDto.WaterInjectorCount,
                ArtificialLift = wellProjectDto.ArtificialLift,
                RigMobDemob = wellProjectDto.RigMobDemob,
                AnnualWellInterventionCost = wellProjectDto.AnnualWellInterventionCost,
                PluggingAndAbandonment = wellProjectDto.PluggingAndAbandonment
            };
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
        public static void ConvertExisting(WellProject existing, WellProjectDto wellProjectDto)
        {

            existing.Id = wellProjectDto.Id;
            existing.ProjectId = wellProjectDto.ProjectId;
            existing.Name = wellProjectDto.Name;
            existing.ProducerCount = wellProjectDto.ProducerCount;
            existing.GasInjectorCount = wellProjectDto.GasInjectorCount;
            existing.WaterInjectorCount = wellProjectDto.WaterInjectorCount;
            existing.ArtificialLift = wellProjectDto.ArtificialLift;
            existing.RigMobDemob = wellProjectDto.RigMobDemob;
            existing.AnnualWellInterventionCost = wellProjectDto.AnnualWellInterventionCost;
            existing.PluggingAndAbandonment = wellProjectDto.PluggingAndAbandonment;

            if (wellProjectDto.CostProfile != null)
            {
                existing.CostProfile = Convert(wellProjectDto.CostProfile, existing);
            }
            if (wellProjectDto.DrillingSchedule != null)
            {
                existing.DrillingSchedule = Convert(wellProjectDto.DrillingSchedule, existing);
            }
        }

        private static WellProjectCostProfile? Convert(WellProjectCostProfileDto? costProfile, WellProject wellProject)
        {
            if (costProfile == null) return null;
            var wellProjectCostProfile = new WellProjectCostProfile
            {
                Id = costProfile.Id,
                WellProject = wellProject,
                EPAVersion = costProfile.EPAVersion,
                Currency = costProfile.Currency,
                StartYear = costProfile.StartYear,
                Values = costProfile.Values
            };
            return wellProjectCostProfile;
        }

        private static DrillingSchedule? Convert(DrillingScheduleDto? drillingScheduleDto, WellProject wellProject)
        {
            if (drillingScheduleDto == null) return null;
            var drillingSchedule = new DrillingSchedule
            {
                Id = drillingScheduleDto.Id,
                WellProject = wellProject,
                StartYear = drillingScheduleDto.StartYear,
                Values = drillingScheduleDto.Values
            };
            return drillingSchedule;
        }
    }
}
