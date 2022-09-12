using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class SubstructureDtoAdapter
{
    public static SubstructureDto Convert(Substructure substructure)
    {
        var substructureDto = new SubstructureDto
        {
            Id = substructure.Id,
            ProjectId = substructure.ProjectId,
            Name = substructure.Name,
            DryWeight = substructure.DryWeight,
            Maturity = substructure.Maturity,
            Currency = substructure.Currency,
            ApprovedBy = substructure.ApprovedBy,
            CostYear = substructure.CostYear,
            CostProfile = Convert(substructure.CostProfile),
            CessationCostProfile = Convert(substructure.CessationCostProfile),
            ProspVersion = substructure.ProspVersion,
            Source = substructure.Source,
            LastChangedDate = substructure.LastChangedDate,
            Concept = substructure.Concept,
            DG3Date = substructure.DG3Date,
            DG4Date = substructure.DG4Date
        };
        return substructureDto;
    }

    private static SubstructureCostProfileDto? Convert(SubstructureCostProfile? costProfile)
    {
        if (costProfile == null)
        {
            return null;
        }
        var substructureCostProfile = new SubstructureCostProfileDto
        {
            Id = costProfile.Id,
            EPAVersion = costProfile.EPAVersion,
            Currency = costProfile.Currency,
            StartYear = costProfile.StartYear,
            Values = costProfile.Values
        };
        return substructureCostProfile;
    }

    private static SubstructureCessationCostProfileDto? Convert(SubstructureCessationCostProfile? substructureCessationCostProfile)
    {
        if (substructureCessationCostProfile == null)
        {
            return null;
        }
        SubstructureCessationCostProfileDto substructureCessationCostProfileDto = new SubstructureCessationCostProfileDto
        {
            Id = substructureCessationCostProfile.Id,
            EPAVersion = substructureCessationCostProfile.EPAVersion,
            Currency = substructureCessationCostProfile.Currency,
            StartYear = substructureCessationCostProfile.StartYear,
            Values = substructureCessationCostProfile.Values
        };
        return substructureCessationCostProfileDto;
    }
}
