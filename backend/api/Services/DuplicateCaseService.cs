using api.Context;
using api.Dtos;
using api.Exceptions;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class DuplicateCaseService : IDuplicateCaseService
{
    private readonly DcdDbContext _context;
    private readonly IProjectService _projectService;
    private readonly IDrainageStrategyService _drainageStrategyService;
    private readonly ITopsideService _topsideService;
    private readonly ISurfService _surfService;
    private readonly ISubstructureService _substructureService;
    private readonly ITransportService _transportService;
    private readonly IExplorationService _explorationService;
    private readonly IWellProjectService _wellProjectService;
    private readonly IWellProjectWellService _wellProjectWellService;
    private readonly IExplorationWellService _explorationWellService;
    private readonly ILogger<DuplicateCaseService> _logger;

    public DuplicateCaseService(DcdDbContext context, IProjectService projectService, ILoggerFactory loggerFactory, IDrainageStrategyService drainageStrategyService,
        ITopsideService topsideService, ISurfService surfService, ISubstructureService substructureService, ITransportService transportService,
        IExplorationService explorationService, IWellProjectService wellProjectService, IWellProjectWellService wellProjectWellService, IExplorationWellService explorationWellService)
    {
        _context = context;
        _projectService = projectService;
        _drainageStrategyService = drainageStrategyService;
        _topsideService = topsideService;
        _surfService = surfService;
        _substructureService = substructureService;
        _transportService = transportService;
        _explorationService = explorationService;
        _wellProjectService = wellProjectService;
        _wellProjectWellService = wellProjectWellService;
        _explorationWellService = explorationWellService;
        _logger = loggerFactory.CreateLogger<DuplicateCaseService>();
    }

    public async Task<Case> GetCaseNoTracking(Guid caseId)
    {
        var caseItem = await _context.Cases!.AsNoTracking()
            .Include(c => c.TotalFeasibilityAndConceptStudies)
            .Include(c => c.TotalFeasibilityAndConceptStudiesOverride)
            .Include(c => c.TotalFEEDStudies)
            .Include(c => c.TotalFEEDStudiesOverride)
            .Include(c => c.TotalOtherStudies)
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
            .FirstOrDefaultAsync(c => c.Id == caseId);
        if (caseItem == null)
        {
            throw new NotFoundInDBException(string.Format("Case {0} not found.", caseId));
        }
        return caseItem;
    }

    public async Task<ProjectDto> DuplicateCase(Guid caseId)
    {
        var caseItem = await GetCaseNoTracking(caseId);

        var sourceWellProjectId = caseItem.WellProjectLink;
        var sourceExplorationId = caseItem.ExplorationLink;

        caseItem.CreateTime = DateTimeOffset.UtcNow;
        caseItem.ModifyTime = DateTimeOffset.UtcNow;
        caseItem.Id = new Guid();

        if (caseItem.TotalFeasibilityAndConceptStudies != null)
        {
            caseItem.TotalFeasibilityAndConceptStudies.Id = Guid.Empty;
        }
        if (caseItem.TotalFeasibilityAndConceptStudiesOverride != null)
        {
            caseItem.TotalFeasibilityAndConceptStudiesOverride.Id = Guid.Empty;
        }
        if (caseItem.TotalFEEDStudies != null)
        {
            caseItem.TotalFEEDStudies.Id = Guid.Empty;
        }
        if (caseItem.TotalFEEDStudiesOverride != null)
        {
            caseItem.TotalFEEDStudiesOverride.Id = Guid.Empty;
        }
        if (caseItem.TotalOtherStudies != null)
        {
            caseItem.TotalOtherStudies.Id = Guid.Empty;
        }
        if (caseItem.CessationWellsCost != null)
        {
            caseItem.CessationWellsCost.Id = Guid.Empty;
        }
        if (caseItem.CessationWellsCostOverride != null)
        {
            caseItem.CessationWellsCostOverride.Id = Guid.Empty;
        }
        if (caseItem.CessationOffshoreFacilitiesCost != null)
        {
            caseItem.CessationOffshoreFacilitiesCost.Id = Guid.Empty;
        }
        if (caseItem.CessationOffshoreFacilitiesCostOverride != null)
        {
            caseItem.CessationOffshoreFacilitiesCostOverride.Id = Guid.Empty;
        }
        if (caseItem.CessationOnshoreFacilitiesCostProfile != null)
        {
            caseItem.CessationOnshoreFacilitiesCostProfile.Id = Guid.Empty;
        }
        if (caseItem.WellInterventionCostProfile != null)
        {
            caseItem.WellInterventionCostProfile.Id = Guid.Empty;
        }
        if (caseItem.WellInterventionCostProfileOverride != null)
        {
            caseItem.WellInterventionCostProfileOverride.Id = Guid.Empty;
        }
        if (caseItem.OffshoreFacilitiesOperationsCostProfile != null)
        {
            caseItem.OffshoreFacilitiesOperationsCostProfile.Id = Guid.Empty;
        }
        if (caseItem.OffshoreFacilitiesOperationsCostProfileOverride != null)
        {
            caseItem.OffshoreFacilitiesOperationsCostProfileOverride.Id = Guid.Empty;
        }
        if (caseItem.HistoricCostCostProfile != null)
        {
            caseItem.HistoricCostCostProfile.Id = Guid.Empty;
        }
        if (caseItem.OnshoreRelatedOPEXCostProfile != null)
        {
            caseItem.OnshoreRelatedOPEXCostProfile.Id = Guid.Empty;
        }
        if (caseItem.AdditionalOPEXCostProfile != null)
        {
            caseItem.AdditionalOPEXCostProfile.Id = Guid.Empty;
        }

        var project = await _projectService.GetProject(caseItem.ProjectId);
        caseItem.Project = project;
        if (project.Cases != null)
        {
            caseItem.Name = GetUniqueCopyName(project.Cases, caseItem.Name);
        }
        _context.Cases!.Add(caseItem);

        await _drainageStrategyService.CopyDrainageStrategy(caseItem.DrainageStrategyLink, caseItem.Id);
        await _topsideService.CopyTopside(caseItem.TopsideLink, caseItem.Id);
        await _surfService.CopySurf(caseItem.SurfLink, caseItem.Id);
        await _substructureService.CopySubstructure(caseItem.SubstructureLink, caseItem.Id);
        await _transportService.CopyTransport(caseItem.TransportLink, caseItem.Id);

        var newWellProject = await _wellProjectService.CopyWellProject(caseItem.WellProjectLink, caseItem.Id);
        var newExploration = await _explorationService.CopyExploration(caseItem.ExplorationLink, caseItem.Id);

        await _wellProjectWellService.CopyWellProjectWell(sourceWellProjectId, newWellProject.Id);
        await _explorationWellService.CopyExplorationWell(sourceExplorationId, newExploration.Id);

        await _context.SaveChangesAsync();
        return await _projectService.GetProjectDto(project.Id);
    }

    private string GetUniqueCopyName(IEnumerable<Case> cases, string originalName)
    {
        var copyName = " - copy";
        var newName = originalName + copyName;
        var i = 1;

        string potentialName = newName;
        while (cases.Any(c => c.Name == potentialName))
        {
            i++;
            potentialName = newName + $" ({i})";
        }

        return potentialName;
    }
}
