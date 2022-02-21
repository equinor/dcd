using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class WellProjectDtoAdapter
    {
        public static WellProjectDto Convert(WellProject wellProject)
        {
            var wellProjectDto = new WellProjectDto();
            wellProjectDto.Id = wellProject.Id;
            wellProjectDto.ProjectId = wellProject.ProjectId;
            wellProjectDto.Name = wellProject.Name;
            wellProjectDto.ProducerCount = wellProject.ProducerCount;
            wellProjectDto.GasInjectorCount = wellProject.GasInjectorCount;
            wellProjectDto.WaterInjectorCount = wellProject.WaterInjectorCount;
            wellProjectDto.ArtificialLift = wellProject.ArtificialLift;
            wellProjectDto.RigMobDemob = wellProject.RigMobDemob;
            wellProjectDto.AnnualWellInterventionCost = wellProject.AnnualWellInterventionCost;
            wellProjectDto.PluggingAndAbandonment = wellProject.PluggingAndAbandonment;
            wellProjectDto.CostProfile = Convert(wellProject.CostProfile);
            wellProjectDto.DrillingSchedule = Convert(wellProject.DrillingSchedule);
            return wellProjectDto;
        }

        private static WellProjectCostProfileDto Convert(WellProjectCostProfile wellProjectCostProfile)
        {
            if (wellProjectCostProfile == null)
            {
                return null!;
            }
            return new WellProjectCostProfileDto
            {
                EPAVersion = wellProjectCostProfile.EPAVersion,
                Currency = wellProjectCostProfile.Currency,
                StartYear = wellProjectCostProfile.StartYear,
                Values = wellProjectCostProfile.Values
            };
        }

        private static DrillingScheduleDto Convert(DrillingSchedule wellProjectDrillingSchedule)
        {
            if (wellProjectDrillingSchedule == null)
            {
                return null!;
            }
            return new DrillingScheduleDto
            {
                StartYear = wellProjectDrillingSchedule.StartYear,
                Values = wellProjectDrillingSchedule.Values
            };
        }
    }
}
