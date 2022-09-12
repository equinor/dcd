using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class WellDtoAdapter
{
    public static WellDto Convert(Well well)
    {
        var wellDto = new WellDto
        {
            Id = well.Id,
            Name = well.Name,
            WellInterventionCost = well.WellInterventionCost,
            PlugingAndAbandonmentCost = well.PlugingAndAbandonmentCost,
            WellCategory = well.WellCategory,
            WellCost = well.WellCost,
            DrillingDays = well.DrillingDays,
            ProjectId = well.ProjectId,
        };
        return wellDto;
    }
}