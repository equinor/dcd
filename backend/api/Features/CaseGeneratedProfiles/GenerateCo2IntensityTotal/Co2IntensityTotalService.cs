using api.Context;
using api.Features.Assets.CaseAssets.DrainageStrategies.Profiles.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.CaseGeneratedProfiles.GenerateCo2IntensityTotal;

public class Co2IntensityTotalService(DcdDbContext context)
{
    public async Task<double> Calculate(Guid caseId)
    {
        var caseItem = await context.Cases
            .Include(c => c.TotalFeasibilityAndConceptStudies)
            .Include(c => c.TotalFeasibilityAndConceptStudiesOverride)
            .Include(c => c.TotalFEEDStudies)
            .Include(c => c.TotalFEEDStudiesOverride)
            .Include(c => c.TotalOtherStudiesCostProfile)
            .Include(c => c.HistoricCostCostProfile)
            .Include(c => c.WellInterventionCostProfile)
            .Include(c => c.WellInterventionCostProfileOverride)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfile)
            .Include(c => c.OffshoreFacilitiesOperationsCostProfileOverride)
            .Include(c => c.OnshoreRelatedOPEXCostProfile)
            .Include(c => c.AdditionalOPEXCostProfile)
            .Include(c => c.CessationWellsCost)
            .Include(c => c.CessationWellsCostOverride)
            .Include(c => c.CessationOffshoreFacilitiesCost)
            .Include(c => c.CessationOffshoreFacilitiesCostOverride)
            .Include(c => c.CessationOnshoreFacilitiesCostProfile)
            .Include(c => c.CalculatedTotalIncomeCostProfile)
            .Include(c => c.CalculatedTotalCostCostProfile)
            .SingleAsync(c => c.Id == caseId);

        var projectPhysicalUnit = await context.Projects
            .Where(x => x.Id == caseItem.ProjectId)
            .Select(x => x.PhysicalUnit)
            .SingleAsync();

        var drainageStrategy = await context.DrainageStrategies
            .Include(d => d.Co2Emissions)
            .Include(d => d.Co2EmissionsOverride)
            .Include(d => d.ProductionProfileOil)
            .Include(d => d.AdditionalProductionProfileOil)
            .Include(d => d.ProductionProfileGas)
            .Include(d => d.AdditionalProductionProfileGas)
            .SingleAsync(x => x.Id == caseItem.DrainageStrategyLink);

        var generateCo2EmissionsProfile = new Co2EmissionsDto();
        if (drainageStrategy.Co2EmissionsOverride?.Override == true)
        {
            generateCo2EmissionsProfile.Values = drainageStrategy.Co2EmissionsOverride.Values;
            generateCo2EmissionsProfile.StartYear = drainageStrategy.Co2EmissionsOverride.StartYear;
        }
        else
        {
            generateCo2EmissionsProfile.Values = drainageStrategy.Co2Emissions?.Values ?? [];
            generateCo2EmissionsProfile.StartYear = drainageStrategy.Co2Emissions?.StartYear ?? 0;
        }

        return CalculateCo2Intensity(projectPhysicalUnit, drainageStrategy, generateCo2EmissionsProfile);
    }

    private static double CalculateTotalOilProduction(PhysUnit physicalUnit, DrainageStrategy drainageStrategy, Boolean excludeOilFieldConversion)
    {
        var sumOilProduction = 0.0;
        var million = 1E6;
        var bblConversionFactor = 6.29;

        if (drainageStrategy.ProductionProfileOil != null)
        {
            sumOilProduction += drainageStrategy.ProductionProfileOil.Values.Sum();
        }
        if (drainageStrategy.AdditionalProductionProfileOil != null)
        {
            sumOilProduction += drainageStrategy.AdditionalProductionProfileOil.Values.Sum();
        }

        if (physicalUnit != 0 && !excludeOilFieldConversion)
        {
            return sumOilProduction * bblConversionFactor / million;
        }

        return sumOilProduction / million;
    }

    private static double CalculateTotalGasProduction(PhysUnit physicalUnit, DrainageStrategy drainageStrategy, Boolean excludeOilFieldConversion)
    {
        var sumGasProduction = 0.0;
        var billion = 1E9;
        var scfConversionFactor = 35.315;

        if (drainageStrategy.ProductionProfileGas != null)
        {
            sumGasProduction += drainageStrategy.ProductionProfileGas.Values.Sum();
        }
        if (drainageStrategy.AdditionalProductionProfileGas != null)
        {
            sumGasProduction += drainageStrategy.AdditionalProductionProfileGas.Values.Sum();
        }

        if (physicalUnit != 0 && !excludeOilFieldConversion)
        {
            return sumGasProduction * scfConversionFactor / billion;
        }

        return sumGasProduction / billion;
    }

    private static double CalculateTotalExportedVolumes(PhysUnit physicalUnit, DrainageStrategy drainageStrategy, Boolean excludeOilFieldConversion)
    {
        var oilEquivalentFactor = 5.61;
        if (physicalUnit != 0 && !excludeOilFieldConversion)
        {
            return CalculateTotalOilProduction(physicalUnit, drainageStrategy, false) + CalculateTotalGasProduction(physicalUnit, drainageStrategy, false) / oilEquivalentFactor;
        }
        return CalculateTotalOilProduction(physicalUnit, drainageStrategy, true) + CalculateTotalGasProduction(physicalUnit, drainageStrategy, true);
    }

    private static double CalculateTotalCo2Emissions(Co2EmissionsDto generateCo2EmissionsProfile)
    {
        return generateCo2EmissionsProfile.Sum;
    }

    private static double CalculateCo2Intensity(PhysUnit physicalUnit, DrainageStrategy drainageStrategy, Co2EmissionsDto generateCo2EmissionsProfile)
    {
        var tonnesToKgFactor = 1000;
        var boeConversionFactor = 6.29;
        var totalExportedVolumes = CalculateTotalExportedVolumes(physicalUnit, drainageStrategy, true);
        var totalCo2Emissions = CalculateTotalCo2Emissions(generateCo2EmissionsProfile);
        if (totalExportedVolumes > 0 && totalCo2Emissions > 0)
        {
            return (totalCo2Emissions / totalExportedVolumes) / boeConversionFactor * tonnesToKgFactor;
        }
        return 0;
    }
}
