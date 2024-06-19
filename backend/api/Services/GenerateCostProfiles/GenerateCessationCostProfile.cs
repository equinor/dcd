using System.Reflection.Metadata.Ecma335;

using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using AutoMapper;

namespace api.Services;

public class GenerateCessationCostProfile : IGenerateCessationCostProfile
{
    private readonly ICaseService _caseService;
    private readonly ILogger<GenerateCessationCostProfile> _logger;
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly IWellProjectService _wellProjectService;
    private readonly IWellProjectWellService _wellProjectWellService;
    private readonly ISurfService _surfService;
    private readonly IProjectService _projectService;
    private readonly DcdDbContext _context;
    private readonly IMapper _mapper;

    public GenerateCessationCostProfile(
        DcdDbContext context,
        ILoggerFactory loggerFactory,
        ICaseService caseService,
        IDrainageStrategyService drainageStrategyService,
        IWellProjectService wellProjectService,
        IWellProjectWellService wellProjectWellService,
        ISurfService surfService,
        IProjectService projectService,
        IMapper mapper)
    {
        _context = context;
        _logger = loggerFactory.CreateLogger<GenerateCessationCostProfile>();
        _caseService = caseService;
        _drainageStrategyService = drainageStrategyService;
        _wellProjectService = wellProjectService;
        _wellProjectWellService = wellProjectWellService;
        _surfService = surfService;
        _projectService = projectService;
        _mapper = mapper;
    }

    public async Task<CessationCostWrapperDto> Generate(Guid caseId)
    {
        var result = new CessationCostWrapperDto();
        var caseItem = await _caseService.GetCase(caseId);
        var project = await _projectService.GetProjectWithoutAssets(caseItem.ProjectId);

        var cessationWellsCost = await GetCessationWellsCost(caseItem, project);
        var cessationOffshoreFacilitiesCost = await GetCessationOffshoreFacilitiesCost(caseItem);
        var cessationOnshoreFacilitiesCostProfile = caseItem.CessationOnshoreFacilitiesCostProfile ?? new CessationOnshoreFacilitiesCostProfile();

        var cessationWellsDto = _mapper.Map<CessationWellsCostDto>(cessationWellsCost);
        result.CessationWellsCostDto = cessationWellsDto;

        var cessationOffshoreFacilitiesCostDto = _mapper.Map<CessationOffshoreFacilitiesCostDto>(cessationOffshoreFacilitiesCost);
        result.CessationOffshoreFacilitiesCostDto = cessationOffshoreFacilitiesCostDto;

        var cessationOnshoreFacilitiesCostProfileDto = _mapper.Map<CessationOnshoreFacilitiesCostProfileDto>(caseItem.CessationOnshoreFacilitiesCostProfile ?? new CessationOnshoreFacilitiesCostProfile());
        result.CessationOnshoreFacilitiesCostProfileDto = cessationOnshoreFacilitiesCostProfileDto;

        await UpdateCaseAndSave(caseItem, cessationWellsCost, cessationOffshoreFacilitiesCost, cessationOnshoreFacilitiesCostProfile);

        var cessationTimeSeries = TimeSeriesCost.MergeCostProfilesList(new List<TimeSeries<double>> { cessationWellsCost, cessationOffshoreFacilitiesCost, cessationOnshoreFacilitiesCostProfile });
        var cessation = new CessationCost
        {
            StartYear = cessationTimeSeries.StartYear,
            Values = cessationTimeSeries.Values
        };
        var cessationDto = _mapper.Map<CessationCostDto>(cessation);
        result.CessationCostDto = cessationDto;
        return result;
    }

    private async Task<CessationWellsCost> GetCessationWellsCost(Case caseItem, Project project)
    {
        if (caseItem.CessationWellsCostOverride != null)
        {
            var overrideCost = caseItem.CessationWellsCostOverride;
            return new CessationWellsCost
            {
                StartYear = overrideCost.StartYear,
                Values = overrideCost.Values,
                Currency = overrideCost.Currency
            };
        }
        else
        {
            var lastYear = await GetRelativeLastYearOfProduction(caseItem);
            if (lastYear.HasValue)
            {
                var wellProject = await _wellProjectService.GetWellProject(caseItem.WellProjectLink);
                return await GenerateCessationWellsCost(wellProject, project, lastYear.Value, new CessationWellsCost());
            }
            else
            {
                return new CessationWellsCost();
            }
        }
    }

    private async Task<CessationOffshoreFacilitiesCost> GetCessationOffshoreFacilitiesCost(Case caseItem)
    {
        if (caseItem.CessationOffshoreFacilitiesCostOverride != null)
        {
            var overrideCost = caseItem.CessationOffshoreFacilitiesCostOverride;
            return new CessationOffshoreFacilitiesCost
            {
                StartYear = overrideCost.StartYear,
                Values = overrideCost.Values,
                Currency = overrideCost.Currency
            };
        }
        else
        {
            var lastYear = await GetRelativeLastYearOfProduction(caseItem);
            if (lastYear.HasValue)
            {
                var surf = await _surfService.GetSurf(caseItem.SurfLink);
                return GenerateCessationOffshoreFacilitiesCost(surf, lastYear.Value, new CessationOffshoreFacilitiesCost());
            }
            else
            {
                return new CessationOffshoreFacilitiesCost();
            }
        }
    }

    private async Task<int> UpdateCaseAndSave(Case caseItem, CessationWellsCost cessationWellsCost, CessationOffshoreFacilitiesCost cessationOffshoreFacilitiesCost, CessationOnshoreFacilitiesCostProfile CessationOnshoreFacilitiesCostProfile)
    {
        caseItem.CessationWellsCost = cessationWellsCost;
        caseItem.CessationOffshoreFacilitiesCost = cessationOffshoreFacilitiesCost;
        caseItem.CessationOnshoreFacilitiesCostProfile = CessationOnshoreFacilitiesCostProfile;
        return await _context.SaveChangesAsync();
    }

    private async Task<CessationWellsCost> GenerateCessationWellsCost(WellProject wellProject, Project project, int lastYear, CessationWellsCost cessationWells)
    {
        var linkedWells = await _wellProjectWellService.GetWellProjectWellsForWellProject(wellProject.Id);
        if (linkedWells != null)
        {
            var pluggingAndAbandonment = project.DevelopmentOperationalWellCosts?.PluggingAndAbandonment ?? 0;

            var sumDrilledWells = 0;
            foreach (var well in linkedWells)
            {
                sumDrilledWells += well.DrillingSchedule?.Values.Sum() ?? 0;
            }
            var totalCost = sumDrilledWells * (double)pluggingAndAbandonment;
            cessationWells.StartYear = lastYear;
            var cessationWellsValues = new double[] { totalCost / 2, totalCost / 2 };
            cessationWells.Values = cessationWellsValues;
        }
        return cessationWells;
    }

    private static CessationOffshoreFacilitiesCost GenerateCessationOffshoreFacilitiesCost(Surf surf, int lastYear, CessationOffshoreFacilitiesCost cessationOffshoreFacilities)
    {
        var surfCessationCost = surf.CessationCost;

        cessationOffshoreFacilities.StartYear = lastYear + 1;
        var cessationOffshoreFacilitiesValues = new double[] { (double)surfCessationCost / 2, (double)surfCessationCost / 2 };
        cessationOffshoreFacilities.Values = cessationOffshoreFacilitiesValues;
        return cessationOffshoreFacilities;
    }

    private async Task<int?> GetRelativeLastYearOfProduction(Case caseItem)
    {
        var drainageStrategy = new DrainageStrategy();
        try
        {
            drainageStrategy = await _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("DrainageStrategy {0} not found.", caseItem.DrainageStrategyLink);
            return null;
        }
        if (drainageStrategy.ProductionProfileOil == null || drainageStrategy.ProductionProfileOil.Values.Length == 0) { return null; }
        var lastYear = drainageStrategy.ProductionProfileOil.StartYear + drainageStrategy.ProductionProfileOil.Values.Length - 1;
        return lastYear;
    }
}
