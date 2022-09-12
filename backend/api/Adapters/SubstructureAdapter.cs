using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class SubstructureAdapter
{
    public static Substructure Convert(SubstructureDto substructureDto)
    {
        var substructure = new Substructure
        {
            Id = substructureDto.Id,
            ProjectId = substructureDto.ProjectId,
            Name = substructureDto.Name,
            DryWeight = substructureDto.DryWeight,
            Maturity = substructureDto.Maturity,
            Currency = substructureDto.Currency,
            ApprovedBy = substructureDto.ApprovedBy,
            CostYear = substructureDto.CostYear,
            ProspVersion = substructureDto.ProspVersion,
            Source = substructureDto.Source,
            LastChangedDate = substructureDto.LastChangedDate,
            Concept = substructureDto.Concept,
            DG3Date = substructureDto.DG3Date,
            DG4Date = substructureDto.DG4Date
        };

        substructure.CostProfile = Convert(substructureDto.CostProfile, substructure);
        substructure.CessationCostProfile = Convert(substructureDto.CessationCostProfile, substructure);

        return substructure;
    }

    public static void ConvertExisting(Substructure existing, SubstructureDto substructureDto)
    {
        existing.Id = substructureDto.Id;
        existing.ProjectId = substructureDto.ProjectId;
        existing.Name = substructureDto.Name;
        existing.DryWeight = substructureDto.DryWeight;
        existing.Maturity = substructureDto.Maturity;
        existing.Currency = substructureDto.Currency;
        existing.ApprovedBy = substructureDto.ApprovedBy;
        existing.CostYear = substructureDto.CostYear;
        existing.ProspVersion = substructureDto.ProspVersion;
        existing.Source = substructureDto.Source;
        existing.LastChangedDate = substructureDto.LastChangedDate;
        existing.Concept = substructureDto.Concept;
        existing.DG3Date = substructureDto.DG3Date;
        existing.DG4Date = substructureDto.DG4Date;

        existing.CostProfile = Convert(substructureDto.CostProfile, existing);
        existing.CessationCostProfile = Convert(substructureDto.CessationCostProfile, existing);
    }

    private static SubstructureCostProfile? Convert(SubstructureCostProfileDto? costprofile, Substructure substructure)
    {
        if (costprofile == null)
        {
            return null;
        }
        var substructureCostProfile = new SubstructureCostProfile
        {
            Substructure = substructure,
            Id = costprofile.Id,
            EPAVersion = costprofile.EPAVersion,
            Currency = costprofile.Currency,
            StartYear = costprofile.StartYear,
            Values = costprofile.Values
        };
        return substructureCostProfile;
    }

    private static SubstructureCessationCostProfile? Convert(SubstructureCessationCostProfileDto? substructureCessationCostProfileDto, Substructure substructure)
    {
        if (substructureCessationCostProfileDto == null)
        {
            return null;
        }
        SubstructureCessationCostProfile substructureCessationCostProfile = new SubstructureCessationCostProfile
        {
            Substructure = substructure,
            Id = substructureCessationCostProfileDto.Id,
            EPAVersion = substructureCessationCostProfileDto.EPAVersion,
            Currency = substructureCessationCostProfileDto.Currency,
            StartYear = substructureCessationCostProfileDto.StartYear,
            Values = substructureCessationCostProfileDto.Values
        };
        return substructureCessationCostProfile;
    }
}