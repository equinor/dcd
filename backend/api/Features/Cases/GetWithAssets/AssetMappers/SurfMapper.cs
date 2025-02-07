using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Models;

namespace api.Features.Cases.GetWithAssets.AssetMappers;

public static class SurfMapper
{
    public static SurfDto MapToDto(Surf entity)
    {
        return new SurfDto
        {
            Id = entity.Id,
            Name = entity.Name,
            ProjectId = entity.Case.ProjectId,
            CessationCost = entity.CessationCost,
            Maturity = entity.Maturity,
            InfieldPipelineSystemLength = entity.InfieldPipelineSystemLength,
            UmbilicalSystemLength = entity.UmbilicalSystemLength,
            ArtificialLift = entity.ArtificialLift,
            RiserCount = entity.RiserCount,
            TemplateCount = entity.TemplateCount,
            ProducerCount = entity.ProducerCount,
            GasInjectorCount = entity.GasInjectorCount,
            WaterInjectorCount = entity.WaterInjectorCount,
            ProductionFlowline = entity.ProductionFlowline,
            Currency = entity.Currency,
            LastChangedDate = entity.LastChangedDate ?? default,
            CostYear = entity.CostYear,
            Source = entity.Source,
            ProspVersion = entity.ProspVersion,
            ApprovedBy = entity.ApprovedBy,
            DG3Date = entity.DG3Date,
            DG4Date = entity.DG4Date
        };
    }
}
