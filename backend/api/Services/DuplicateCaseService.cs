using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
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
        private readonly ILogger<CaseService> _logger;

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
            _logger = loggerFactory.CreateLogger<CaseService>();
        }

        public Case GetCaseNoTracking(Guid caseId)
        {
            var caseItem = _context.Cases!.AsNoTracking()
                .Include(c => c.TotalFeasibilityAndConceptStudies)
                .Include(c => c.TotalFeasibilityAndConceptStudiesOverride)
                .Include(c => c.TotalFEEDStudies)
                .Include(c => c.TotalFEEDStudiesOverride)
                .FirstOrDefault(c => c.Id == caseId);
            if (caseItem == null)
            {
                throw new NotFoundInDBException(string.Format("Case {0} not found.", caseId));
            }
            return caseItem;
        }

        public ProjectDto DuplicateCase(Guid caseId)
        {
            var caseItem = GetCaseNoTracking(caseId);

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

            var project = _projectService.GetProject(caseItem.ProjectId);
            caseItem.Project = project;

            caseItem.Name += " - copy";
            _context.Cases!.Add(caseItem);

            _drainageStrategyService.CopyDrainageStrategy(caseItem.DrainageStrategyLink, caseItem.Id);
            _topsideService.CopyTopside(caseItem.TopsideLink, caseItem.Id);
            _surfService.CopySurf(caseItem.SurfLink, caseItem.Id);
            _substructureService.CopySubstructure(caseItem.SubstructureLink, caseItem.Id);
            _transportService.CopyTransport(caseItem.TransportLink, caseItem.Id);

            var newWellProject = _wellProjectService.CopyWellProject(caseItem.WellProjectLink, caseItem.Id);
            var newExploration = _explorationService.CopyExploration(caseItem.ExplorationLink, caseItem.Id);

            _wellProjectWellService.CopyWellProjectWell(sourceWellProjectId, newWellProject.Id);
            _explorationWellService.CopyExplorationWell(sourceExplorationId, newExploration.Id);

            _context.SaveChanges();
            return _projectService.GetProjectDto(project.Id);
        }
    }
}
