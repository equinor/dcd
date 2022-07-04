using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class WellProjectDtoAdapter
    {
        public static WellProjectDto Convert(WellProject wellProject)
        {
            var wellProjectDto = new WellProjectDto
            {
                Id = wellProject.Id,
                ProjectId = wellProject.ProjectId,
                Name = wellProject.Name,
                ArtificialLift = wellProject.ArtificialLift,
                RigMobDemob = wellProject.RigMobDemob,
                AnnualWellInterventionCost = wellProject.AnnualWellInterventionCost,
                PluggingAndAbandonment = wellProject.PluggingAndAbandonment,
                Currency = wellProject.Currency,
                WellProjectWells = wellProject.WellProjectWells?.Select(wc => WellProjectWellDtoAdapter.Convert(wc)).ToList()
            };
            if (wellProject.CostProfile != null)
            {
                wellProjectDto.CostProfile = Convert(wellProject.CostProfile);
            }
            if (wellProject.DrillingSchedule != null)
            {
                wellProjectDto.DrillingSchedule = Convert(wellProject.DrillingSchedule);
            }
            return wellProjectDto;
        }

        private static WellProjectCostProfileDto? Convert(WellProjectCostProfile? costProfile)
        {
            if (costProfile == null)
            {
                return null!;
            }
            var wellProjectCostProfileDto = new WellProjectCostProfileDto
            {
                Id = costProfile.Id,
                EPAVersion = costProfile.EPAVersion,
                Currency = costProfile.Currency,
                StartYear = costProfile.StartYear,
                Values = costProfile.Values
            };
            return wellProjectCostProfileDto;
        }

        private static DrillingScheduleDto? Convert(DrillingSchedule? drillingSchedule)
        {
            if (drillingSchedule == null)
            {
                return null!;
            }
            var drillingScheduleDto = new DrillingScheduleDto
            {
                Id = drillingSchedule.Id,
                StartYear = drillingSchedule.StartYear,
                Values = drillingSchedule.Values
            };
            return drillingScheduleDto;
        }
    }
}
