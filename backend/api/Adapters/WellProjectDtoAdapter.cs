using api.Dtos;
using api.Models;

namespace api.Adapters;

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
        if (wellProject.OilProducerCostProfile != null)
        {
            wellProjectDto.OilProducerCostProfile = Convert(wellProject.OilProducerCostProfile);
        }
        if (wellProject.GasProducerCostProfile != null)
        {
            wellProjectDto.GasProducerCostProfile = Convert(wellProject.GasProducerCostProfile);
        }
        if (wellProject.WaterInjectorCostProfile != null)
        {
            wellProjectDto.WaterInjectorCostProfile = Convert(wellProject.WaterInjectorCostProfile);
        }
        if (wellProject.GasInjectorCostProfile != null)
        {
            wellProjectDto.GasInjectorCostProfile = Convert(wellProject.GasInjectorCostProfile);
        }
        return wellProjectDto;
    }

    private static OilProducerCostProfileDto? Convert(OilProducerCostProfile? costProfile)
    {
        if (costProfile == null)
        {
            return null!;
        }
        var wellProjectCostProfileDto = new OilProducerCostProfileDto
        {
            Id = costProfile.Id,
            EPAVersion = costProfile.EPAVersion,
            Currency = costProfile.Currency,
            StartYear = costProfile.StartYear,
            Values = costProfile.Values,
            // Override = costProfile.Override,
        };
        return wellProjectCostProfileDto;
    }
    private static GasProducerCostProfileDto? Convert(GasProducerCostProfile? costProfile)
    {
        if (costProfile == null)
        {
            return null!;
        }
        var wellProjectCostProfileDto = new GasProducerCostProfileDto
        {
            Id = costProfile.Id,
            EPAVersion = costProfile.EPAVersion,
            Currency = costProfile.Currency,
            StartYear = costProfile.StartYear,
            Values = costProfile.Values,
            // Override = costProfile.Override,
        };
        return wellProjectCostProfileDto;
    }
    private static WaterInjectorCostProfileDto? Convert(WaterInjectorCostProfile? costProfile)
    {
        if (costProfile == null)
        {
            return null!;
        }
        var wellProjectCostProfileDto = new WaterInjectorCostProfileDto
        {
            Id = costProfile.Id,
            EPAVersion = costProfile.EPAVersion,
            Currency = costProfile.Currency,
            StartYear = costProfile.StartYear,
            Values = costProfile.Values,
            // Override = costProfile.Override,
        };
        return wellProjectCostProfileDto;
    }
    private static GasInjectorCostProfileDto? Convert(GasInjectorCostProfile? costProfile)
    {
        if (costProfile == null)
        {
            return null!;
        }
        var wellProjectCostProfileDto = new GasInjectorCostProfileDto
        {
            Id = costProfile.Id,
            EPAVersion = costProfile.EPAVersion,
            Currency = costProfile.Currency,
            StartYear = costProfile.StartYear,
            Values = costProfile.Values,
            // Override = costProfile.Override,
        };
        return wellProjectCostProfileDto;
    }

}
