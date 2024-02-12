using System.Globalization;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Services;

public class GenerateStudyCostProfile : IGenerateStudyCostProfile
{
    private readonly ICaseService _caseService;
    private readonly ILogger<GenerateStudyCostProfile> _logger;
    private readonly IWellProjectService _wellProjectService;
    private readonly ITopsideService _topsideService;
    private readonly ISubstructureService _substructureService;
    private readonly ISurfService _surfService;
    private readonly ITransportService _transportService;
    private readonly DcdDbContext _context;
    private readonly IMapper _mapper;

    public GenerateStudyCostProfile(
        DcdDbContext context, 
        ILoggerFactory loggerFactory, 
        ICaseService caseService, 
        IWellProjectService wellProjectService, 
        ITopsideService topsideService,
        ISubstructureService substructureService, 
        ISurfService surfService, 
        ITransportService transportService,
        IMapper mapper)
    {
        _context = context;
        _logger = loggerFactory.CreateLogger<GenerateStudyCostProfile>();
        _caseService = caseService;
        _wellProjectService = wellProjectService;
        _topsideService = topsideService;
        _substructureService = substructureService;
        _surfService = surfService;
        _transportService = transportService;
        _mapper = mapper;
    }

    public async Task<StudyCostProfileWrapperDto> GenerateAsync(Guid caseId)
    {
        var caseItem = await _caseService.GetCase(caseId);

        var sumFacilityCost = await SumAllCostFacility(caseItem);
        var sumWellCost = await SumWellCost(caseItem);

        var newFeasibility = CalculateTotalFeasibilityAndConceptStudies(caseItem, sumFacilityCost, sumWellCost);
        var newFeed = CalculateTotalFEEDStudies(caseItem, sumFacilityCost, sumWellCost);

        var feasibility = caseItem.TotalFeasibilityAndConceptStudies ?? newFeasibility;
        feasibility.StartYear = newFeasibility.StartYear;
        feasibility.Values = newFeasibility.Values;

        var feed = caseItem.TotalFEEDStudies ?? newFeed;
        feed.StartYear = newFeed.StartYear;
        feed.Values = newFeed.Values;

        await UpdateCaseAndSaveAsync(caseItem, feasibility, feed);

        var result = new StudyCostProfileWrapperDto();
        var feasibilityDto = _mapper.Map<TotalFeasibilityAndConceptStudiesDto>(feasibility);
        var feedDto = _mapper.Map<TotalFEEDStudiesDto>(feed);

        result.TotalFeasibilityAndConceptStudiesDto = feasibilityDto;
        result.TotalFEEDStudiesDto = feedDto;

        if (feasibility.Values.Length == 0 && feed.Values.Length == 0)
        {
            return new StudyCostProfileWrapperDto();
        }
        var cost = TimeSeriesCost.MergeCostProfiles(feasibility, feed);
        var studyCost = new StudyCostProfile
        {
            StartYear = cost.StartYear,
            Values = cost.Values
        };
        var study = _mapper.Map<StudyCostProfileDto>(studyCost);
        result.StudyCostProfileDto = study;
        return result;
    }

    private async Task<int> UpdateCaseAndSaveAsync(Case caseItem, TotalFeasibilityAndConceptStudies totalFeasibilityAndConceptStudies, TotalFEEDStudies totalFEEDStudies)
    {
        caseItem.TotalFeasibilityAndConceptStudies = totalFeasibilityAndConceptStudies;
        caseItem.TotalFEEDStudies = totalFEEDStudies;
        return await _context.SaveChangesAsync();
    }

    public TotalFeasibilityAndConceptStudies CalculateTotalFeasibilityAndConceptStudies(Case caseItem, double sumFacilityCost, double sumWellCost)
    {
        var totalFeasibilityAndConceptStudies = (sumFacilityCost + sumWellCost) * caseItem.CapexFactorFeasibilityStudies;

        var dg0 = caseItem.DG0Date;
        var dg2 = caseItem.DG2Date;

        if (dg0.Year == 1 || dg2.Year == 1) { return new TotalFeasibilityAndConceptStudies(); }
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

        return feasibilityAndConceptStudiesCost;
    }

    public TotalFEEDStudies CalculateTotalFEEDStudies(Case caseItem, double sumFacilityCost, double sumWellCost)
    {
        var totalFeasibilityAndConceptStudies = (sumFacilityCost + sumWellCost) * caseItem.CapexFactorFEEDStudies;

        var dg2 = caseItem.DG2Date;
        var dg3 = caseItem.DG3Date;

        if (dg2.Year == 1 || dg3.Year == 1) { return new TotalFEEDStudies(); }
        if (dg3.DayOfYear == 1) { dg3 = dg3.AddDays(-1); } // Treat the 1st of January as the 31st of December

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

        var valuesList = percentageOfYearList.ConvertAll(x => x * totalFeasibilityAndConceptStudies);

        var feasibilityAndConceptStudiesCost = new TotalFEEDStudies
        {
            StartYear = dg2.Year - caseItem.DG4Date.Year,
            Values = valuesList.ToArray()
        };

        return feasibilityAndConceptStudiesCost;
    }

    public async Task<double> SumAllCostFacility(Case caseItem)
    {
        var sumFacilityCost = 0.0;

        Substructure substructure;
        try
        {
            substructure = await _substructureService.GetSubstructure(caseItem.SubstructureLink);
            if (substructure.CostProfileOverride?.Override == true)
            {
                sumFacilityCost += substructure.CostProfileOverride.Values.Sum();
            }
            else if (substructure.CostProfile != null)
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
            surf = await _surfService.GetSurf(caseItem.SurfLink);
            if (surf.CostProfileOverride?.Override == true)
            {
                sumFacilityCost += surf.CostProfileOverride.Values.Sum();
            }
            else if (surf.CostProfile != null)
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
            topside = await _topsideService.GetTopside(caseItem.TopsideLink);
            if (topside.CostProfileOverride?.Override == true)
            {
                sumFacilityCost += topside.CostProfileOverride.Values.Sum();
            }
            else if (topside.CostProfile != null)
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
            transport = await _transportService.GetTransport(caseItem.TransportLink);
            if (transport.CostProfileOverride?.Override == true)
            {
                sumFacilityCost += transport.CostProfileOverride.Values.Sum();
            }
            else if (transport.CostProfile != null)
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

    public async Task<double> SumWellCost(Case caseItem)
    {
        var sumWellCost = 0.0;

        WellProject wellProject;
        try
        {
            wellProject = await _wellProjectService.GetWellProject(caseItem.WellProjectLink);

            if (wellProject.OilProducerCostProfileOverride?.Override == true)
            {
                sumWellCost += wellProject.OilProducerCostProfileOverride.Values.Sum();
            }
            else if (wellProject.OilProducerCostProfile != null)
            {
                sumWellCost += wellProject.OilProducerCostProfile.Values.Sum();
            }

            if (wellProject.GasProducerCostProfileOverride?.Override == true)
            {
                sumWellCost += wellProject.GasProducerCostProfileOverride.Values.Sum();
            }
            else if (wellProject.GasProducerCostProfile != null)
            {
                sumWellCost += wellProject.GasProducerCostProfile.Values.Sum();
            }

            if (wellProject.WaterInjectorCostProfileOverride?.Override == true)
            {
                sumWellCost += wellProject.WaterInjectorCostProfileOverride.Values.Sum();
            }
            else if (wellProject.WaterInjectorCostProfile != null)
            {
                sumWellCost += wellProject.WaterInjectorCostProfile.Values.Sum();
            }

            if (wellProject.GasInjectorCostProfileOverride?.Override == true)
            {
                sumWellCost += wellProject.GasInjectorCostProfileOverride.Values.Sum();
            }
            else if (wellProject.GasInjectorCostProfile != null)
            {
                sumWellCost += wellProject.GasInjectorCostProfile.Values.Sum();
            }
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("WellProject {0} not found.", caseItem.WellProjectLink);
        }

        return sumWellCost;
    }
}
