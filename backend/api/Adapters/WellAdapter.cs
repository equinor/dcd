using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class WellAdapter
{
    public static Well Convert(WellDto wellDto)
    {
        var well = new Well
        {
            Id = wellDto.Id,
            Name = wellDto.Name,
            WellInterventionCost = wellDto.WellInterventionCost,
            PlugingAndAbandonmentCost = wellDto.PlugingAndAbandonmentCost,
            ProjectId = wellDto.ProjectId,
            WellCategory = wellDto.WellCategory,
            WellCost = wellDto.WellCost,
            DrillingDays = wellDto.DrillingDays
        };
        return well;
    }

    public static void ConvertExisting(Well existing, WellDto wellDto)
    {
        existing.Id = wellDto.Id;
        existing.Name = wellDto.Name;
        existing.WellInterventionCost = wellDto.WellInterventionCost;
        existing.PlugingAndAbandonmentCost = wellDto.PlugingAndAbandonmentCost;
        existing.ProjectId = wellDto.ProjectId;
        existing.WellCategory = wellDto.WellCategory;
        existing.WellCost = wellDto.WellCost;
        existing.DrillingDays = wellDto.DrillingDays;
    }
}