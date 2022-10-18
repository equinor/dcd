using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class WellProjectAdapter
{
    public static WellProject Convert(WellProjectDto wellProjectDto)
    {
        var wellProject = new WellProject
        {
            Id = wellProjectDto.Id,
            ProjectId = wellProjectDto.ProjectId,
            Name = wellProjectDto.Name,
            ArtificialLift = wellProjectDto.ArtificialLift,
            RigMobDemob = wellProjectDto.RigMobDemob,
            AnnualWellInterventionCost = wellProjectDto.AnnualWellInterventionCost,
            PluggingAndAbandonment = wellProjectDto.PluggingAndAbandonment,
            Currency = wellProjectDto.Currency,
        };

        if (wellProjectDto.CostProfile != null)
        {
            wellProject.CostProfile = Convert(wellProjectDto.CostProfile, wellProject);
        }
        return wellProject;
    }
    public static void ConvertExisting(WellProject existing, WellProjectDto wellProjectDto)
    {
        existing.Id = wellProjectDto.Id;
        existing.ProjectId = wellProjectDto.ProjectId;
        existing.Name = wellProjectDto.Name;
        existing.ArtificialLift = wellProjectDto.ArtificialLift;
        existing.RigMobDemob = wellProjectDto.RigMobDemob;
        existing.AnnualWellInterventionCost = wellProjectDto.AnnualWellInterventionCost;
        existing.PluggingAndAbandonment = wellProjectDto.PluggingAndAbandonment;
        existing.Currency = wellProjectDto.Currency;

        if (wellProjectDto.CostProfile != null)
        {
            if (existing.CostProfile != null)
            {
                existing.CostProfile = ConvertExisting(wellProjectDto.CostProfile, existing);
            }
            else
            {
                existing.CostProfile = Convert(wellProjectDto.CostProfile, existing);
            }
        }
    }

    private static WellProjectCostProfile? ConvertExisting(WellProjectCostProfileDto? costProfile, WellProject wellProject)
    {
        if (costProfile == null) return null;

        var existing = wellProject.CostProfile;

        existing!.EPAVersion = costProfile.EPAVersion;
        existing.Currency = costProfile.Currency;
        existing.StartYear = costProfile.StartYear;
        existing.Values = costProfile.Values;
        existing.Override = costProfile.Override;

        return existing;
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
            Values = costProfile.Values,
            Override = costProfile.Override
        };
        return wellProjectCostProfile;
    }
}
