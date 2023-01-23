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
            Currency = wellProjectDto.Currency,
        };

        if (wellProjectDto.OilProducerCostProfile != null)
        {
            wellProject.OilProducerCostProfile = Convert<OilProducerCostProfileDto, OilProducerCostProfile>(wellProjectDto.OilProducerCostProfile, wellProject);
        }
        if (wellProjectDto.OilProducerCostProfileOverride != null)
        {
            wellProject.OilProducerCostProfileOverride = ConvertOverride<OilProducerCostProfileOverrideDto, OilProducerCostProfileOverride>(wellProjectDto.OilProducerCostProfileOverride, wellProject);
        }
        if (wellProjectDto.GasProducerCostProfile != null)
        {
            wellProject.GasProducerCostProfile = Convert<GasProducerCostProfileDto, GasProducerCostProfile>(wellProjectDto.GasProducerCostProfile, wellProject);
        }
        if (wellProjectDto.GasProducerCostProfileOverride != null)
        {
            wellProject.GasProducerCostProfileOverride = ConvertOverride<GasProducerCostProfileOverrideDto, GasProducerCostProfileOverride>(wellProjectDto.GasProducerCostProfileOverride, wellProject);
        }
        if (wellProjectDto.WaterInjectorCostProfile != null)
        {
            wellProject.WaterInjectorCostProfile = Convert<WaterInjectorCostProfileDto, WaterInjectorCostProfile>(wellProjectDto.WaterInjectorCostProfile, wellProject);
        }
        if (wellProjectDto.WaterInjectorCostProfileOverride != null)
        {
            wellProject.WaterInjectorCostProfileOverride = ConvertOverride<WaterInjectorCostProfileOverrideDto, WaterInjectorCostProfileOverride>(wellProjectDto.WaterInjectorCostProfileOverride, wellProject);
        }
        if (wellProjectDto.GasInjectorCostProfile != null)
        {
            wellProject.GasInjectorCostProfile = Convert<GasInjectorCostProfileDto, GasInjectorCostProfile>(wellProjectDto.GasInjectorCostProfile, wellProject);
        }
        if (wellProjectDto.GasInjectorCostProfileOverride != null)
        {
            wellProject.GasInjectorCostProfileOverride = ConvertOverride<GasInjectorCostProfileOverrideDto, GasInjectorCostProfileOverride>(wellProjectDto.GasInjectorCostProfileOverride, wellProject);
        }
        return wellProject;
    }
    public static void ConvertExisting(WellProject existing, WellProjectDto wellProjectDto)
    {
        existing.Id = wellProjectDto.Id;
        existing.ProjectId = wellProjectDto.ProjectId;
        existing.Name = wellProjectDto.Name;
        existing.ArtificialLift = wellProjectDto.ArtificialLift;
        existing.Currency = wellProjectDto.Currency;

        existing.OilProducerCostProfile = Convert<OilProducerCostProfileDto, OilProducerCostProfile>(wellProjectDto.OilProducerCostProfile, existing);
        existing.OilProducerCostProfileOverride = ConvertOverride<OilProducerCostProfileOverrideDto, OilProducerCostProfileOverride>(wellProjectDto.OilProducerCostProfileOverride, existing);

        existing.GasProducerCostProfile = Convert<GasProducerCostProfileDto, GasProducerCostProfile>(wellProjectDto.GasProducerCostProfile, existing);
        existing.GasProducerCostProfileOverride = ConvertOverride<GasProducerCostProfileOverrideDto, GasProducerCostProfileOverride>(wellProjectDto.GasProducerCostProfileOverride, existing);

        existing.WaterInjectorCostProfile = Convert<WaterInjectorCostProfileDto, WaterInjectorCostProfile>(wellProjectDto.WaterInjectorCostProfile, existing);
        existing.WaterInjectorCostProfileOverride = ConvertOverride<WaterInjectorCostProfileOverrideDto, WaterInjectorCostProfileOverride>(wellProjectDto.WaterInjectorCostProfileOverride, existing);

        existing.GasInjectorCostProfile = Convert<GasInjectorCostProfileDto, GasInjectorCostProfile>(wellProjectDto.GasInjectorCostProfile, existing);
        existing.GasInjectorCostProfileOverride = ConvertOverride<GasInjectorCostProfileOverrideDto, GasInjectorCostProfileOverride>(wellProjectDto.GasInjectorCostProfileOverride, existing);
    }

    private static TModel? ConvertOverride<TDto, TModel>(TDto? dto, WellProject wellProject)
    where TDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
    where TModel : TimeSeriesCost, ITimeSeriesOverride, IWellProjectTimeSeries, new()
    {
        if (dto == null) { return null; }

        return new TModel
        {
            Id = dto.Id,
            Override = dto.Override,
            StartYear = dto.StartYear,
            Currency = dto.Currency,
            EPAVersion = dto.EPAVersion,
            Values = dto.Values,
            WellProject = wellProject,
        };
    }

    private static TModel? Convert<TDto, TModel>(TDto? dto, WellProject wellProject)
        where TDto : TimeSeriesCostDto
        where TModel : TimeSeriesCost, IWellProjectTimeSeries, new()
    {
        if (dto == null) { return null; }

        return new TModel
        {
            Id = dto.Id,
            StartYear = dto.StartYear,
            Currency = dto.Currency,
            EPAVersion = dto.EPAVersion,
            Values = dto.Values,
            WellProject = wellProject,
        };
    }
}
