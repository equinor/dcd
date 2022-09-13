using System.Globalization;

using api.Adapters;
using api.Dtos;
using api.Models;

namespace api.Services;

public class GenerateStudyCostProfile
{
    private readonly CaseService _caseService;
    private readonly ILogger<CaseService> _logger;
    private readonly WellProjectService _wellProjectService;
    private readonly TopsideService _topsideService;
    private readonly SubstructureService _substructureService;
    private readonly SurfService _surfService;
    private readonly TransportService _transportService;

    public GenerateStudyCostProfile(ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
    {
        _logger = loggerFactory.CreateLogger<CaseService>();
        _caseService = serviceProvider.GetRequiredService<CaseService>();
        _wellProjectService = serviceProvider.GetRequiredService<WellProjectService>();
        _topsideService = serviceProvider.GetRequiredService<TopsideService>();
        _substructureService = serviceProvider.GetRequiredService<SubstructureService>();
        _surfService = serviceProvider.GetRequiredService<SurfService>();
        _transportService = serviceProvider.GetRequiredService<TransportService>();
    }

    public StudyCostProfileDto Generate(Guid caseId)
    {
        var feasibility = CalculateTotalFeasibilityAndConceptStudies(caseId);
        var feed = CalculateTotalFEEDStudies(caseId);

        if (feasibility.Values.Length == 0 && feed.Values.Length == 0)
        {
            return new StudyCostProfileDto();
        }
        var cost = TimeSeriesCost.MergeCostProfiles(feasibility, feed);
        if (cost == null) { return new StudyCostProfileDto(); }
        var studyCost = new StudyCostProfile
        {
            StartYear = cost.StartYear,
            Values = cost.Values
        };
        var dto = CaseDtoAdapter.Convert(studyCost);
        return dto;
    }

    public TimeSeries<double> CalculateTotalFeasibilityAndConceptStudies(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);

        var sumFacilityCost = SumAllCostFacility(caseId);
        var sumWellCost = SumWellCost(caseId);

        var totalFeasibilityAndConceptStudies = (sumFacilityCost + sumWellCost) * caseItem.CapexFactorFeasibilityStudies;

        var dg0 = caseItem.DG0Date;
        var dg2 = caseItem.DG2Date;

        if (dg0.Year == 1 || dg2.Year == 1) { return new TimeSeries<double>(); }
        if (dg2.DayOfYear == 1) { dg2 = dg2.AddDays(-1); } // Treat the 1st of January as the 31st of December

        var totalDays = (dg2 - dg0).Days + 1;

        var firstYearDays = (new DateTimeOffset(dg0.Year, 12, 31, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero) - dg0).Days + 1;
        var firstYearPercentage = firstYearDays / (double)totalDays;

        var lastYearDays = dg2.DayOfYear;
        var lastYearPercentage = lastYearDays / (double)totalDays;

        var percentageOfYearList = new List<double>();
        percentageOfYearList.Add(firstYearPercentage);
        for (int i = dg0.Year + 1; i < dg2.Year; i++)
        {
            var days = DateTime.IsLeapYear(i) ? 366 : 365;
            var percentage = days / (double)totalDays;
            percentageOfYearList.Add(percentage);
        }
        percentageOfYearList.Add(lastYearPercentage);

        var valuesList = percentageOfYearList.ConvertAll(x => x * totalFeasibilityAndConceptStudies);

        var feasibilityAndConceptStudiesCost = new TimeSeries<double>
        {
            StartYear = dg0.Year - caseItem.DG4Date.Year,
            Values = valuesList.ToArray()
        };

        return feasibilityAndConceptStudiesCost;
    }

    public TimeSeries<double> CalculateTotalFEEDStudies(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);

        var sumFacilityCost = SumAllCostFacility(caseId);
        var sumWellCost = SumWellCost(caseId);

        var totalFeasibilityAndConceptStudies = (sumFacilityCost + sumWellCost) * caseItem.CapexFactorFEEDStudies;

        var dg2 = caseItem.DG2Date;
        var dg3 = caseItem.DG3Date;

        if (dg2.Year == 1 || dg3.Year == 1) { return new TimeSeries<double>(); }
        if (dg3.DayOfYear == 1) { dg3 = dg3.AddDays(-1); } // Treat the 1st of January as the 31st of December

        var totalDays = (dg3 - dg2).Days + 1;

        var firstYearDays = (new DateTimeOffset(dg2.Year, 12, 31, 0, 0, 0, 0, new GregorianCalendar(), TimeSpan.Zero) - dg2).Days + 1;
        var firstYearPercentage = firstYearDays / (double)totalDays;

        var lastYearDays = dg3.DayOfYear;
        var lastYearPercentage = lastYearDays / (double)totalDays;

        var percentageOfYearList = new List<double>();
        percentageOfYearList.Add(firstYearPercentage);
        for (int i = dg2.Year + 1; i < dg3.Year; i++)
        {
            var days = DateTime.IsLeapYear(i) ? 366 : 365;
            var percentage = days / (double)totalDays;
            percentageOfYearList.Add(percentage);
        }
        percentageOfYearList.Add(lastYearPercentage);

        var valuesList = percentageOfYearList.ConvertAll(x => x * totalFeasibilityAndConceptStudies);

        var feasibilityAndConceptStudiesCost = new TimeSeries<double>
        {
            StartYear = dg2.Year - caseItem.DG4Date.Year,
            Values = valuesList.ToArray()
        };

        return feasibilityAndConceptStudiesCost;
    }

    public double SumAllCostFacility(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);

        var sumFacilityCost = 0.0;

        Substructure substructure;
        try
        {
            substructure = _substructureService.GetSubstructure(caseItem.SubstructureLink);
            if (substructure.CostProfile != null)
            {
                sumFacilityCost += substructure.CostProfile.Values.Sum();
            }
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("Substructure {0} not found.", caseItem.SubstructureLink);
        }

        Surf surf;
        try
        {
            surf = _surfService.GetSurf(caseItem.SurfLink);
            if (surf.CostProfile != null)
            {
                sumFacilityCost += surf.CostProfile.Values.Sum();
            }
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("Surf {0} not found.", caseItem.SurfLink);
        }

        Topside topside;
        try
        {
            topside = _topsideService.GetTopside(caseItem.TopsideLink);
            if (topside.CostProfile != null)
            {
                sumFacilityCost += topside.CostProfile.Values.Sum();
            }
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("Topside {0} not found.", caseItem.TopsideLink);
        }

        Transport transport;
        try
        {
            transport = _transportService.GetTransport(caseItem.TransportLink);
            if (transport.CostProfile != null)
            {
                sumFacilityCost += transport.CostProfile.Values.Sum();
            }
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("Transport {0} not found.", caseItem.TransportLink);
        }

        return sumFacilityCost;
    }

    public double SumWellCost(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);

        var sumWellCost = 0.0;

        var wellProject = new WellProject();
        try
        {
            wellProject = _wellProjectService.GetWellProject(caseItem.WellProjectLink);
            if (wellProject?.CostProfile != null)
            {
                sumWellCost = wellProject.CostProfile.Values.Sum();
            }
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("WellProject {0} not found.", caseItem.WellProjectLink);
        }

        return sumWellCost;
    }
}
