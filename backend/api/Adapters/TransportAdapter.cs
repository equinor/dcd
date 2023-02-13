using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class TransportAdapter
{

    public static Transport Convert(TransportDto transportDto)
    {
        var transport = new Transport
        {
            Id = transportDto.Id,
            ProjectId = transportDto.ProjectId,
            Name = transportDto.Name,
            Maturity = transportDto.Maturity,
            GasExportPipelineLength = transportDto.GasExportPipelineLength,
            OilExportPipelineLength = transportDto.OilExportPipelineLength,
            Currency = transportDto.Currency,
            LastChangedDate = transportDto.LastChangedDate,
            CostYear = transportDto.CostYear,
            Source = transportDto.Source,
            ProspVersion = transportDto.ProspVersion,
            DG3Date = transportDto.DG3Date,
            DG4Date = transportDto.DG4Date
        };

        if (transportDto.CostProfile != null)
        {
            transport.CostProfile = Convert<TransportCostProfileDto, TransportCostProfile>(transportDto.CostProfile, transport);
        }

        if (transportDto.CostProfileOverride != null)
        {
            transport.CostProfileOverride = ConvertOverride<TransportCostProfileOverrideDto, TransportCostProfileOverride>(transportDto.CostProfileOverride, transport);
        }

        if (transportDto.CessationCostProfile != null)
        {
            transport.CessationCostProfile = Convert<TransportCessationCostProfileDto, TransportCessationCostProfile>(transportDto.CessationCostProfile, transport);
        }
        return transport;
    }

    public static void ConvertExisting(Transport existing, TransportDto transportDto)
    {
        existing.Id = transportDto.Id;
        existing.ProjectId = transportDto.ProjectId;
        existing.Name = transportDto.Name;
        existing.Maturity = transportDto.Maturity;
        existing.GasExportPipelineLength = transportDto.GasExportPipelineLength;
        existing.OilExportPipelineLength = transportDto.OilExportPipelineLength;
        existing.Currency = transportDto.Currency;
        existing.CostProfile = Convert<TransportCostProfileDto, TransportCostProfile>(transportDto.CostProfile, existing);
        existing.CostProfileOverride = ConvertOverride<TransportCostProfileOverrideDto, TransportCostProfileOverride>(transportDto.CostProfileOverride, existing);
        existing.CessationCostProfile = Convert<TransportCessationCostProfileDto, TransportCessationCostProfile>(transportDto.CessationCostProfile, existing);
        existing.LastChangedDate = transportDto.LastChangedDate;
        existing.CostYear = transportDto.CostYear;
        existing.Source = transportDto.Source;
        existing.ProspVersion = transportDto.ProspVersion;
        existing.DG3Date = transportDto.DG3Date;
        existing.DG4Date = transportDto.DG4Date;
    }

    private static TModel? ConvertOverride<TDto, TModel>(TDto? dto, Transport transport)
        where TDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
        where TModel : TimeSeriesCost, ITimeSeriesOverride, ITransportTimeSeries, new()
    {
        if (dto == null) { return new TModel(); }

        return new TModel
        {
            Id = dto.Id,
            Override = dto.Override,
            StartYear = dto.StartYear,
            Currency = dto.Currency,
            EPAVersion = dto.EPAVersion,
            Values = dto.Values,
            Transport = transport,
        };
    }

    private static TModel? Convert<TDto, TModel>(TDto? dto, Transport transport)
        where TDto : TimeSeriesCostDto
        where TModel : TimeSeriesCost, ITransportTimeSeries, new()
    {
        if (dto == null) { return new TModel(); }

        return new TModel
        {
            Id = dto.Id,
            StartYear = dto.StartYear,
            Currency = dto.Currency,
            EPAVersion = dto.EPAVersion,
            Values = dto.Values,
            Transport = transport,
        };
    }
}
