using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Models;

namespace api.Features.Cases.GetWithAssets.AssetMappers;

public static class SubstructureMapper
{
    public static SubstructureDto MapToDto(Substructure entity)
    {
        return new SubstructureDto
        {
            Id = entity.Id,
            DryWeight = entity.DryWeight,
            Maturity = entity.Maturity,
            ApprovedBy = entity.ApprovedBy,
            CostYear = entity.CostYear,
            ProspVersion = entity.ProspVersion,
            Source = entity.Source,
            LastChangedDate = entity.LastChangedDate,
            Concept = entity.Concept,
            DG3Date = entity.DG3Date,
            DG4Date = entity.DG4Date
        };
    }
}
