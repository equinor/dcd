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
        if (wellProjectDto.OilProducerCostProfile != null)
        {
            wellProject.OilProducerCostProfile = Convert(wellProjectDto.OilProducerCostProfile, wellProject);
        }
        if (wellProjectDto.GasProducerCostProfile != null)
        {
            wellProject.GasProducerCostProfile = Convert(wellProjectDto.GasProducerCostProfile, wellProject);
        }
        if (wellProjectDto.WaterInjectorCostProfile != null)
        {
            wellProject.WaterInjectorCostProfile = Convert(wellProjectDto.WaterInjectorCostProfile, wellProject);
        }
        if (wellProjectDto.GasInjectorCostProfile != null)
        {
            wellProject.GasInjectorCostProfile = Convert(wellProjectDto.GasInjectorCostProfile, wellProject);
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
        if (wellProjectDto.OilProducerCostProfile != null)
        {
            if (existing.OilProducerCostProfile != null)
            {
                existing.OilProducerCostProfile = ConvertExisting(wellProjectDto.OilProducerCostProfile, existing);
            }
            else
            {
                existing.OilProducerCostProfile = Convert(wellProjectDto.OilProducerCostProfile, existing);
            }
        }
        if (wellProjectDto.GasProducerCostProfile != null)
        {
            if (existing.GasProducerCostProfile != null)
            {
                existing.GasProducerCostProfile = ConvertExisting(wellProjectDto.GasProducerCostProfile, existing);
            }
            else
            {
                existing.GasProducerCostProfile = Convert(wellProjectDto.GasProducerCostProfile, existing);
            }
        }
        if (wellProjectDto.WaterInjectorCostProfile != null)
        {
            if (existing.WaterInjectorCostProfile != null)
            {
                existing.WaterInjectorCostProfile = ConvertExisting(wellProjectDto.WaterInjectorCostProfile, existing);
            }
            else
            {
                existing.WaterInjectorCostProfile = Convert(wellProjectDto.WaterInjectorCostProfile, existing);
            }
        }
        if (wellProjectDto.GasInjectorCostProfile != null)
        {
            if (existing.GasInjectorCostProfile != null)
            {
                existing.GasInjectorCostProfile = ConvertExisting(wellProjectDto.GasInjectorCostProfile, existing);
            }
            else
            {
                existing.GasInjectorCostProfile = Convert(wellProjectDto.GasInjectorCostProfile, existing);
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

    private static OilProducerCostProfile? ConvertExisting(OilProducerCostProfileDto? costProfile, WellProject wellProject)
    {
        if (costProfile == null) return null;

        var existing = wellProject.OilProducerCostProfile;

        existing!.EPAVersion = costProfile.EPAVersion;
        existing.Currency = costProfile.Currency;
        existing.StartYear = costProfile.StartYear;
        existing.Values = costProfile.Values;
        existing.Override = costProfile.Override;

        return existing;
    }

    private static OilProducerCostProfile? Convert(OilProducerCostProfileDto? costProfile, WellProject wellProject)
    {
        if (costProfile == null) return null;
        var wellProjectCostProfile = new OilProducerCostProfile
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

    private static GasProducerCostProfile? ConvertExisting(GasProducerCostProfileDto? costProfile, WellProject wellProject)
    {
        if (costProfile == null) return null;

        var existing = wellProject.GasProducerCostProfile;

        existing!.EPAVersion = costProfile.EPAVersion;
        existing.Currency = costProfile.Currency;
        existing.StartYear = costProfile.StartYear;
        existing.Values = costProfile.Values;
        existing.Override = costProfile.Override;

        return existing;
    }

    private static GasProducerCostProfile? Convert(GasProducerCostProfileDto? costProfile, WellProject wellProject)
    {
        if (costProfile == null) return null;
        var wellProjectCostProfile = new GasProducerCostProfile
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

    private static WaterInjectorCostProfile? ConvertExisting(WaterInjectorCostProfileDto? costProfile, WellProject wellProject)
    {
        if (costProfile == null) return null;

        var existing = wellProject.WaterInjectorCostProfile;

        existing!.EPAVersion = costProfile.EPAVersion;
        existing.Currency = costProfile.Currency;
        existing.StartYear = costProfile.StartYear;
        existing.Values = costProfile.Values;
        existing.Override = costProfile.Override;

        return existing;
    }

    private static WaterInjectorCostProfile? Convert(WaterInjectorCostProfileDto? costProfile, WellProject wellProject)
    {
        if (costProfile == null) return null;
        var wellProjectCostProfile = new WaterInjectorCostProfile
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

    private static GasInjectorCostProfile? ConvertExisting(GasInjectorCostProfileDto? costProfile, WellProject wellProject)
    {
        if (costProfile == null) return null;

        var existing = wellProject.GasInjectorCostProfile;

        existing!.EPAVersion = costProfile.EPAVersion;
        existing.Currency = costProfile.Currency;
        existing.StartYear = costProfile.StartYear;
        existing.Values = costProfile.Values;
        existing.Override = costProfile.Override;

        return existing;
    }

    private static GasInjectorCostProfile? Convert(GasInjectorCostProfileDto? costProfile, WellProject wellProject)
    {
        if (costProfile == null) return null;
        var wellProjectCostProfile = new GasInjectorCostProfile
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
