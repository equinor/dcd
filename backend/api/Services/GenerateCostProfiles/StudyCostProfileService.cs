using System.Globalization;

using api.Models;

namespace api.Services;

public class StudyCostProfileService : IStudyCostProfileService
{
    private readonly ICaseService _caseService;
    private readonly ILogger<StudyCostProfileService> _logger;
    private readonly IWellProjectService _wellProjectService;
    private readonly ITopsideService _topsideService;
    private readonly ISubstructureService _substructureService;
    private readonly ISurfService _surfService;
    private readonly ITransportService _transportService;

    public StudyCostProfileService(
        ILoggerFactory loggerFactory,
        ICaseService caseService,
        IWellProjectService wellProjectService,
        ITopsideService topsideService,
        ISubstructureService substructureService,
        ISurfService surfService,
        ITransportService transportService)
    {
        _logger = loggerFactory.CreateLogger<StudyCostProfileService>();
        _caseService = caseService;
        _wellProjectService = wellProjectService;
        _topsideService = topsideService;
        _substructureService = substructureService;
        _surfService = surfService;
        _transportService = transportService;
    }

    public async Task Generate(Guid caseId)
    {
        var caseItem = await _caseService.GetCaseWithIncludes(
            caseId,
            c => c.TotalFeasibilityAndConceptStudies!,
            c => c.TotalFeasibilityAndConceptStudiesOverride!,
            c => c.TotalFEEDStudies!,
            c => c.TotalFEEDStudiesOverride!
        );

        var sumFacilityCost = await SumAllCostFacility(caseItem);
        var sumWellCost = await SumWellCost(caseItem);

        CalculateTotalFeasibilityAndConceptStudies(caseItem, sumFacilityCost, sumWellCost);
        CalculateTotalFEEDStudies(caseItem, sumFacilityCost, sumWellCost);
    }

    public void CalculateTotalFeasibilityAndConceptStudies(Case caseItem, double sumFacilityCost, double sumWellCost)
    {
        if (caseItem.TotalFeasibilityAndConceptStudiesOverride?.Override == true)
        {
            return;
        }

        var totalFeasibilityAndConceptStudies = (sumFacilityCost + sumWellCost) * caseItem.CapexFactorFeasibilityStudies;

        var dg0 = caseItem.DG0Date;
        var dg2 = caseItem.DG2Date;

        if (dg0.Year == 1 || dg2.Year == 1)
        {
            CalculationHelper.ResetTimeSeries(caseItem.TotalFeasibilityAndConceptStudies);
            return;
        }

        if (dg2.DayOfYear == 1) { dg2 = dg2.AddDays(-1); } // Treat the 1st of January as the 31st of December

        var totalDays = (dg2 - dg0).Days + 1;

        var firstYearDays = (new DateTimeOffset(dg0.Year, 12, 31, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero) - dg0).Days + 1;
        var firstYearPercentage = firstYearDays / (double)totalDays;

        var lastYearDays = dg2.DayOfYear;
        var lastYearPercentage = lastYearDays / (double)totalDays;

        var percentageOfYearList = new List<double>
        {
            firstYearPercentage
        };
        for (int i = dg0.Year + 1; i < dg2.Year; i++)
        {
            var days = DateTime.IsLeapYear(i) ? 366 : 365;
            var percentage = days / (double)totalDays;
            percentageOfYearList.Add(percentage);
        }
        percentageOfYearList.Add(lastYearPercentage);

        var valuesList = percentageOfYearList.ConvertAll(x => x * totalFeasibilityAndConceptStudies);

        var feasibilityAndConceptStudiesCost = new TotalFeasibilityAndConceptStudies
        {
            StartYear = dg0.Year - caseItem.DG4Date.Year,
            Values = valuesList.ToArray()
        };

        if (caseItem.TotalFeasibilityAndConceptStudies != null)
        {
            caseItem.TotalFeasibilityAndConceptStudies.Values = feasibilityAndConceptStudiesCost.Values;
            caseItem.TotalFeasibilityAndConceptStudies.StartYear = feasibilityAndConceptStudiesCost.StartYear;
        }
        else
        {
            caseItem.TotalFeasibilityAndConceptStudies = feasibilityAndConceptStudiesCost;
        }
    }

    public void CalculateTotalFEEDStudies(Case caseItem, double sumFacilityCost, double sumWellCost)
    {
        if (caseItem.TotalFEEDStudiesOverride?.Override == true)
        {
            return;
        }

        var totalFeedStudies = (sumFacilityCost + sumWellCost) * caseItem.CapexFactorFEEDStudies;

        var dg2 = caseItem.DG2Date;
        var dg3 = caseItem.DG3Date;

        if (dg2.Year == 1 || dg3.Year == 1)
        {
            CalculationHelper.ResetTimeSeries(caseItem.TotalFEEDStudies);
            return;
        }

        if (!DateIsEqual(dg2, dg3) && dg3.DayOfYear == 1) { dg3 = dg3.AddDays(-1); } // Treat the 1st of January as the 31st of December, only if dates are not equal

        var totalDays = (dg3 - dg2).Days + 1;

        var firstYearDays = (new DateTimeOffset(dg2.Year, 12, 31, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero) - dg2).Days + 1;
        var firstYearPercentage = firstYearDays / (double)totalDays;

        var lastYearDays = dg3.DayOfYear;
        var lastYearPercentage = lastYearDays / (double)totalDays;

        var percentageOfYearList = new List<double>
        {
            firstYearPercentage
        };
        for (int i = dg2.Year + 1; i < dg3.Year; i++)
        {
            var days = DateTime.IsLeapYear(i) ? 366 : 365;
            var percentage = days / (double)totalDays;
            percentageOfYearList.Add(percentage);
        }
        percentageOfYearList.Add(lastYearPercentage);

        var valuesList = percentageOfYearList.ConvertAll(x => x * totalFeedStudies);

        var totalFeedStudiesCost = new TotalFEEDStudies
        {
            StartYear = dg2.Year - caseItem.DG4Date.Year,
            Values = valuesList.ToArray()
        };

        if (caseItem.TotalFEEDStudies != null)
        {
            caseItem.TotalFEEDStudies.Values = totalFeedStudiesCost.Values;
            caseItem.TotalFEEDStudies.StartYear = totalFeedStudiesCost.StartYear;
        }
        else
        {
            caseItem.TotalFEEDStudies = totalFeedStudiesCost;
        }
    }

    public async Task<double> SumAllCostFacility(Case caseItem)
    {
        var sumFacilityCost = 0.0;

        var substructure = await _substructureService.GetSubstructureWithIncludes(
            caseItem.SubstructureLink,
            s => s.CostProfileOverride!,
            s => s.CostProfile!
        );

        var surf = await _surfService.GetSurfWithIncludes(
            caseItem.SurfLink,
            s => s.CostProfileOverride!,
            s => s.CostProfile!
        );

        var topside = await _topsideService.GetTopsideWithIncludes(
            caseItem.TopsideLink,
            t => t.CostProfileOverride!,
            t => t.CostProfile!
        );

        var transport = await _transportService.GetTransportWithIncludes(
            caseItem.TransportLink,
            t => t.CostProfileOverride!,
            t => t.CostProfile!
        );

        sumFacilityCost += SumOverrideOrProfile(substructure.CostProfile, substructure.CostProfileOverride);
        sumFacilityCost += SumOverrideOrProfile(surf.CostProfile, surf.CostProfileOverride);
        sumFacilityCost += SumOverrideOrProfile(topside.CostProfile, topside.CostProfileOverride);
        sumFacilityCost += SumOverrideOrProfile(transport.CostProfile, transport.CostProfileOverride);

        return sumFacilityCost;
    }

    public async Task<double> SumWellCost(Case caseItem)
    {
        var sumWellCost = 0.0;

        var wellProject = await _wellProjectService.GetWellProjectWithIncludes(
            caseItem.WellProjectLink,
            w => w.OilProducerCostProfileOverride!,
            w => w.OilProducerCostProfile!,
            w => w.GasProducerCostProfileOverride!,
            w => w.GasProducerCostProfile!,
            w => w.WaterInjectorCostProfileOverride!,
            w => w.WaterInjectorCostProfile!,
            w => w.GasInjectorCostProfileOverride!,
            w => w.GasInjectorCostProfile!
        );

        sumWellCost += SumOverrideOrProfile(wellProject.OilProducerCostProfile, wellProject.OilProducerCostProfileOverride);
        sumWellCost += SumOverrideOrProfile(wellProject.GasProducerCostProfile, wellProject.GasProducerCostProfileOverride);
        sumWellCost += SumOverrideOrProfile(wellProject.WaterInjectorCostProfile, wellProject.WaterInjectorCostProfileOverride);
        sumWellCost += SumOverrideOrProfile(wellProject.GasInjectorCostProfile, wellProject.GasInjectorCostProfileOverride);

        return sumWellCost;
    }

    private static bool DateIsEqual(DateTimeOffset date1, DateTimeOffset date2)
    {
        return date1.Year == date2.Year && date1.DayOfYear == date2.DayOfYear;
    }

    private static double SumOverrideOrProfile<T>(TimeSeries<double>? profile, T? profileOverride)
        where T : TimeSeries<double>, ITimeSeriesOverride
    {
        if (profileOverride?.Override == true)
        {
            return profileOverride.Values.Sum();
        }
        else if (profile != null)
        {
            return profile.Values.Sum();
        }
        return 0;
    }
}
