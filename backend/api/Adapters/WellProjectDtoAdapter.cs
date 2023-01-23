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
            WellProjectWells = wellProject.WellProjectWells?.Select(wc => WellProjectWellDtoAdapter.Convert(wc)).ToList()
        };
        wellProjectDto.OilProducerCostProfile = Convert<OilProducerCostProfileDto, OilProducerCostProfile>(wellProject.OilProducerCostProfile);
        wellProjectDto.OilProducerCostProfileOverride = ConvertOverride<OilProducerCostProfileOverrideDto, OilProducerCostProfileOverride>(wellProject.OilProducerCostProfileOverride);

        wellProjectDto.GasProducerCostProfile = Convert<GasProducerCostProfileDto, GasProducerCostProfile>(wellProject.GasProducerCostProfile);
        wellProjectDto.GasProducerCostProfileOverride = ConvertOverride<GasProducerCostProfileOverrideDto, GasProducerCostProfileOverride>(wellProject.GasProducerCostProfileOverride);

        wellProjectDto.WaterInjectorCostProfile = Convert<WaterInjectorCostProfileDto, WaterInjectorCostProfile>(wellProject.WaterInjectorCostProfile);
        wellProjectDto.WaterInjectorCostProfileOverride = ConvertOverride<WaterInjectorCostProfileOverrideDto, WaterInjectorCostProfileOverride>(wellProject.WaterInjectorCostProfileOverride);

        wellProjectDto.GasInjectorCostProfile = Convert<GasInjectorCostProfileDto, GasInjectorCostProfile>(wellProject.GasInjectorCostProfile);
        wellProjectDto.GasInjectorCostProfileOverride = ConvertOverride<GasInjectorCostProfileOverrideDto, GasInjectorCostProfileOverride>(wellProject.GasInjectorCostProfileOverride);

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
