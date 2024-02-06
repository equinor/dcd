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
            Currency = wellProject.Currency,
            WellProjectWells = wellProject.WellProjectWells?.Select(wc => WellProjectWellDtoAdapter.Convert(wc)).ToList(),
            OilProducerCostProfile = Convert<OilProducerCostProfileDto, OilProducerCostProfile>(wellProject.OilProducerCostProfile),
            OilProducerCostProfileOverride = ConvertOverride<OilProducerCostProfileOverrideDto, OilProducerCostProfileOverride>(wellProject.OilProducerCostProfileOverride),

            GasProducerCostProfile = Convert<GasProducerCostProfileDto, GasProducerCostProfile>(wellProject.GasProducerCostProfile),
            GasProducerCostProfileOverride = ConvertOverride<GasProducerCostProfileOverrideDto, GasProducerCostProfileOverride>(wellProject.GasProducerCostProfileOverride),

            WaterInjectorCostProfile = Convert<WaterInjectorCostProfileDto, WaterInjectorCostProfile>(wellProject.WaterInjectorCostProfile),
            WaterInjectorCostProfileOverride = ConvertOverride<WaterInjectorCostProfileOverrideDto, WaterInjectorCostProfileOverride>(wellProject.WaterInjectorCostProfileOverride),

            GasInjectorCostProfile = Convert<GasInjectorCostProfileDto, GasInjectorCostProfile>(wellProject.GasInjectorCostProfile),
            GasInjectorCostProfileOverride = ConvertOverride<GasInjectorCostProfileOverrideDto, GasInjectorCostProfileOverride>(wellProject.GasInjectorCostProfileOverride)
        };

        return wellProjectDto;
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
}
