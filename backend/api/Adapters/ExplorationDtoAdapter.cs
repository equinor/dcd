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
            ExplorationWellCostProfile = Convert<ExplorationWellCostProfileDto, ExplorationWellCostProfile>(exploration.ExplorationWellCostProfile),
            AppraisalWellCostProfile = Convert<AppraisalWellCostProfileDto, AppraisalWellCostProfile>(exploration.AppraisalWellCostProfile),
            SidetrackCostProfile = Convert<SidetrackCostProfileDto, SidetrackCostProfile>(exploration.SidetrackCostProfile),
            GAndGAdminCost = Convert<GAndGAdminCostDto, GAndGAdminCost>(exploration.GAndGAdminCost),
            SeismicAcquisitionAndProcessing = Convert<SeismicAcquisitionAndProcessingDto, SeismicAcquisitionAndProcessing>(exploration.SeismicAcquisitionAndProcessing),
            CountryOfficeCost = Convert<CountryOfficeCostDto, CountryOfficeCost>(exploration.CountryOfficeCost),
            ExplorationWells = exploration.ExplorationWells?.Select(ew => ExplorationWellDtoAdapter.Convert(ew)).ToList()
        };
        return explorationDto;
    }

    public static TDto? Convert<TDto, TModel>(TModel? model)
    where TDto : TimeSeriesCostDto, new()
    where TModel : TimeSeriesCost
    {
        if (model == null) { return null; }

        return new TDto
        {
            Id = model.Id,
            Currency = model.Currency,
            EPAVersion = model.EPAVersion,
            Values = model.Values,
            StartYear = model.StartYear,
        };
    }

    public static TDto? ConvertOverride<TDto, TModel>(TModel? model)
        where TDto : TimeSeriesCostDto, ITimeSeriesOverrideDto, new()
        where TModel : TimeSeriesCost, ITimeSeriesOverride
    {
        if (model == null) { return null; }

        return new TDto
        {
            Id = model.Id,
            Override = model.Override,
            Currency = model.Currency,
            EPAVersion = model.EPAVersion,
            Values = model.Values,
            StartYear = model.StartYear,
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
