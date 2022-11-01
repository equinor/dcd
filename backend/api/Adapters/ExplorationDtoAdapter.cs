using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class ExplorationDtoAdapter
{
    public static ExplorationDto Convert(Exploration exploration)
    {
        var explorationDto = new ExplorationDto
        {
            Id = exploration.Id,
            ProjectId = exploration.ProjectId,
            Name = exploration.Name,
            RigMobDemob = exploration.RigMobDemob,
            Currency = exploration.Currency,
            CostProfile = Convert(exploration.CostProfile),
            ExplorationWellCostProfile = Convert(exploration.ExplorationWellCostProfile),
            AppraisalWellCostProfile = Convert(exploration.AppraisalWellCostProfile),
            SidetrackCostProfile = Convert(exploration.SidetrackCostProfile),
            GAndGAdminCost = Convert(exploration.GAndGAdminCost),
            SeismicAcquisitionAndProcessing = Convert(exploration.SeismicAcquisitionAndProcessing),
            CountryOfficeCost = Convert(exploration.CountryOfficeCost),
            ExplorationWells = exploration.ExplorationWells?.Select(ew => ExplorationWellDtoAdapter.Convert(ew)).ToList()
        };
        return explorationDto;
    }

    private static ExplorationCostProfileDto Convert(ExplorationCostProfile? costProfile)
    {
        if (costProfile == null)
        {
            return null!;
        }
        return new ExplorationCostProfileDto
        {
            Id = costProfile.Id,
            Currency = costProfile.Currency,
            EPAVersion = costProfile.EPAVersion,
            StartYear = costProfile.StartYear,
            Values = costProfile.Values,
            Override = costProfile.Override,
        };
    }
    private static ExplorationWellCostProfileDto Convert(ExplorationWellCostProfile? costProfile)
    {
        if (costProfile == null)
        {
            return null!;
        }
        return new ExplorationWellCostProfileDto
        {
            Id = costProfile.Id,
            Currency = costProfile.Currency,
            EPAVersion = costProfile.EPAVersion,
            StartYear = costProfile.StartYear,
            Values = costProfile.Values,
            Override = costProfile.Override,
        };
    }
    private static AppraisalWellCostProfileDto Convert(AppraisalWellCostProfile? costProfile)
    {
        if (costProfile == null)
        {
            return null!;
        }
        return new AppraisalWellCostProfileDto
        {
            Id = costProfile.Id,
            Currency = costProfile.Currency,
            EPAVersion = costProfile.EPAVersion,
            StartYear = costProfile.StartYear,
            Values = costProfile.Values,
            Override = costProfile.Override,
        };
    }
    private static SidetrackCostProfileDto Convert(SidetrackCostProfile? costProfile)
    {
        if (costProfile == null)
        {
            return null!;
        }
        return new SidetrackCostProfileDto
        {
            Id = costProfile.Id,
            Currency = costProfile.Currency,
            EPAVersion = costProfile.EPAVersion,
            StartYear = costProfile.StartYear,
            Values = costProfile.Values,
            Override = costProfile.Override,
        };
    }

    public static GAndGAdminCostDto Convert(GAndGAdminCost? gAndGAdminCost)
    {
        if (gAndGAdminCost == null)
        {
            return null!;
        }
        return new GAndGAdminCostDto
        {
            Id = gAndGAdminCost.Id,
            Currency = gAndGAdminCost.Currency,
            EPAVersion = gAndGAdminCost.EPAVersion,
            StartYear = gAndGAdminCost.StartYear,
            Values = gAndGAdminCost.Values,
        };
    }
    private static SeismicAcquisitionAndProcessingDto Convert(SeismicAcquisitionAndProcessing? seismicAcquisitionAndProcessing)
    {
        if (seismicAcquisitionAndProcessing == null)
        {
            return null!;
        }
        return new SeismicAcquisitionAndProcessingDto
        {
            Id = seismicAcquisitionAndProcessing.Id,
            Currency = seismicAcquisitionAndProcessing.Currency,
            EPAVersion = seismicAcquisitionAndProcessing.EPAVersion,
            StartYear = seismicAcquisitionAndProcessing.StartYear,
            Values = seismicAcquisitionAndProcessing.Values,
        };
    }

    private static CountryOfficeCostDto Convert(CountryOfficeCost? countryOfficeCost)
    {
        if (countryOfficeCost == null)
        {
            return null!;
        }
        return new CountryOfficeCostDto
        {
            Id = countryOfficeCost.Id,
            Currency = countryOfficeCost.Currency,
            EPAVersion = countryOfficeCost.EPAVersion,
            StartYear = countryOfficeCost.StartYear,
            Values = countryOfficeCost.Values,
        };
    }
}
