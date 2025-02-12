using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Models;

namespace api.Features.Cases.GetWithAssets.AssetMappers;

public static class TransportMapper
{
    public static TransportDto MapToDto(Transport entity)
    {
        return new TransportDto
        {
            Id = entity.Id,
            Maturity = entity.Maturity,
            GasExportPipelineLength = entity.GasExportPipelineLength,
            OilExportPipelineLength = entity.OilExportPipelineLength,
            LastChangedDate = entity.LastChangedDate,
            CostYear = entity.CostYear,
            Source = entity.Source,
            ProspVersion = entity.ProspVersion,
            DG3Date = entity.DG3Date,
            DG4Date = entity.DG4Date
        };
    }
}
