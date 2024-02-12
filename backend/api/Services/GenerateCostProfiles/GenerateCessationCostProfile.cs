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
        ISurfService surfService, 
        IProjectService projectService,
        IMapper mapper)
    {
        _context = context;
        _logger = loggerFactory.CreateLogger<GenerateCessationCostProfile>();
        _caseService = caseService;
        _drainageStrategyService = drainageStrategyService;
        _wellProjectService = wellProjectService;
        _surfService = surfService;
        _projectService = projectService;
        _mapper = mapper;
    }

    public async Task<CessationCostWrapperDto> GenerateAsync(Guid caseId)
    {
        var result = new CessationCostWrapperDto();
        var caseItem = await _caseService.GetCase(caseId);
        var project = await _projectService.GetProjectWithoutAssets(caseItem.ProjectId);

        var cessationWellsCost = caseItem.CessationWellsCost ?? new CessationWellsCost();
        var cessationOffshoreFacilitiesCost = caseItem.CessationOffshoreFacilitiesCost ?? new CessationOffshoreFacilitiesCost();

        var lastYear = await GetRelativeLastYearOfProduction(caseItem);
        if (lastYear == null)
        {
            await UpdateCaseAndSaveAsync(caseItem, cessationWellsCost, cessationOffshoreFacilitiesCost);
            return new CessationCostWrapperDto();
        }

        WellProject wellProject;
        try
        {
            wellProject = await _wellProjectService.GetWellProject(caseItem.WellProjectLink);
            cessationWellsCost = GenerateCessationWellsCost(wellProject, project, (int)lastYear, cessationWellsCost);

            var cessationWellsDto = _mapper.Map<CessationWellsCostDto>(cessationWellsCost);

            result.CessationWellsCostDto = cessationWellsDto;
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("WellProject {0} not found.", caseItem.WellProjectLink);
        }

        Surf surf;
        try
        {
            surf = await _surfService.GetSurf(caseItem.SurfLink);
            cessationOffshoreFacilitiesCost = GenerateCessationOffshoreFacilitiesCost(surf, (int)lastYear, cessationOffshoreFacilitiesCost);

            var cessationOffshoreFacilitiesCostDto = _mapper.Map<CessationOffshoreFacilitiesCostDto>(cessationOffshoreFacilitiesCost);

            result.CessationOffshoreFacilitiesCostDto = cessationOffshoreFacilitiesCostDto;
        }
        catch (ArgumentException)
        {
            _logger.LogInformation("Surf {0} not found.", caseItem.SurfLink);
        }

        await UpdateCaseAndSaveAsync(caseItem, cessationWellsCost, cessationOffshoreFacilitiesCost);

        var cessationTimeSeries = TimeSeriesCost.MergeCostProfiles(cessationWellsCost, cessationOffshoreFacilitiesCost);
        var cessation = new CessationCost
        {
            StartYear = cessationTimeSeries.StartYear,
            Values = cessationTimeSeries.Values
        };

        var cessationDto = _mapper.Map<CessationCostDto>(cessation);

        result.CessationCostDto = cessationDto;
        return result;
    }

    private async Task<int> UpdateCaseAndSaveAsync(Case caseItem, CessationWellsCost cessationWellsCost, CessationOffshoreFacilitiesCost cessationOffshoreFacilitiesCost)
    {
        caseItem.CessationWellsCost = cessationWellsCost;
        caseItem.CessationOffshoreFacilitiesCost = cessationOffshoreFacilitiesCost;
        return await _context.SaveChangesAsync();
    }

    private static CessationWellsCost GenerateCessationWellsCost(WellProject wellProject, Project project, int lastYear, CessationWellsCost cessationWells)
    {
        var linkedWells = wellProject.WellProjectWells?.Where(ew => Well.IsWellProjectWell(ew.Well.WellCategory)).ToList();
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
