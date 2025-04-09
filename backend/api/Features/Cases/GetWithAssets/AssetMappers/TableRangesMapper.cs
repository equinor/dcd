using api.Features.ProjectData.Dtos.AssetDtos;
using api.Models;

namespace api.Features.Cases.GetWithAssets.AssetMappers;

public static class TableRangesMapper
{
    public static TableRangesDto MapToDto(Case caseItem)
    {
        return new TableRangesDto
        {
            Co2EmissionsYears = caseItem.Co2EmissionsYears,
            DrillingScheduleYears = caseItem.DrillingScheduleYears,
            CaseCostYears = caseItem.CaseCostYears,
            ProductionProfilesYears = caseItem.ProductionProfilesYears
        };
    }
}
