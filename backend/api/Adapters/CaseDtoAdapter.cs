using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class CaseDtoAdapter
{
    public static CaseDto Convert(Case case_)
    {
        var caseDto = new CaseDto
        {
            Id = case_.Id,
            ProjectId = case_.ProjectId,
            Name = case_.Name,
            Description = case_.Description,
            ReferenceCase = case_.ReferenceCase,
            DGADate = case_.DGADate,
            DGBDate = case_.DGBDate,
            DGCDate = case_.DGCDate,
            APXDate = case_.APXDate,
            APZDate = case_.APZDate,
            DG0Date = case_.DG0Date,
            DG1Date = case_.DG1Date,
            DG2Date = case_.DG2Date,
            DG3Date = case_.DG3Date,
            DG4Date = case_.DG4Date,
            CreateTime = case_.CreateTime,
            ModifyTime = case_.ModifyTime,
            DrainageStrategyLink = case_.DrainageStrategyLink,
            WellProjectLink = case_.WellProjectLink,
            SurfLink = case_.SurfLink,
            SubstructureLink = case_.SubstructureLink,
            TopsideLink = case_.TopsideLink,
            TransportLink = case_.TransportLink,
            ExplorationLink = case_.ExplorationLink,
            ArtificialLift = case_.ArtificialLift,
            ProductionStrategyOverview = case_.ProductionStrategyOverview,
            ProducerCount = case_.ProducerCount,
            GasInjectorCount = case_.GasInjectorCount,
            WaterInjectorCount = case_.WaterInjectorCount,
            FacilitiesAvailability = case_.FacilitiesAvailability,
            TemplateCount = case_.TemplateCount,
            SharepointFileId = case_.SharepointFileId,
            SharepointFileName = case_.SharepointFileName,
        };

        return caseDto;
    }

    public static CessationCostDto Convert(CessationCost? opexCost)
    {
        if (opexCost == null)
        {
            return null!;
        }
        return new CessationCostDto
        {
            Id = opexCost.Id,
            Currency = opexCost.Currency,
            EPAVersion = opexCost.EPAVersion,
            StartYear = opexCost.StartYear,
            Values = opexCost.Values,
        };
    }

    public static OpexCostProfileDto Convert(OpexCostProfile? opexCost)
    {
        if (opexCost == null)
        {
            return null!;
        }
        return new OpexCostProfileDto
        {
            Id = opexCost.Id,
            Currency = opexCost.Currency,
            EPAVersion = opexCost.EPAVersion,
            StartYear = opexCost.StartYear,
            Values = opexCost.Values,
        };
    }

    public static StudyCostProfileDto Convert(StudyCostProfile? studyCost)
    {
        if (studyCost == null)
        {
            return null!;
        }
        return new StudyCostProfileDto
        {
            Id = studyCost.Id,
            Currency = studyCost.Currency,
            EPAVersion = studyCost.EPAVersion,
            StartYear = studyCost.StartYear,
            Values = studyCost.Values,
        };
    }
}
