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
}
