using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class TransportDtoAdapter
{

    public static TransportDto Convert(Transport transport)
    {
        var transportDto = new TransportDto
        {
            Id = transport.Id,
            ProjectId = transport.ProjectId,
            Name = transport.Name,
            Maturity = transport.Maturity,
            GasExportPipelineLength = transport.GasExportPipelineLength,
            OilExportPipelineLength = transport.OilExportPipelineLength,
            Currency = transport.Currency,
            LastChangedDate = transport.LastChangedDate,
            CostYear = transport.CostYear,
            Source = transport.Source,
            ProspVersion = transport.ProspVersion,
            CostProfile = Convert<TransportCostProfileDto, TransportCostProfile>(transport.CostProfile) ?? new TransportCostProfileDto(),
            CostProfileOverride = ConvertOverride<TransportCostProfileOverrideDto, TransportCostProfileOverride>(transport.CostProfileOverride) ?? new TransportCostProfileOverrideDto(),
            CessationCostProfile = Convert<TransportCessationCostProfileDto, TransportCessationCostProfile>(transport.CessationCostProfile) ?? new TransportCessationCostProfileDto(),
            DG3Date = transport.DG3Date,
            DG4Date = transport.DG4Date
        };
        return transportDto;
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
