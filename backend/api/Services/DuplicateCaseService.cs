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
        private readonly IDrainageStrategyService drainageStrategyService;
        private readonly ITopsideService topsideService;
        private readonly ISurfService surfService;
        private readonly ISubstructureService substructureService;
        private readonly ITransportService transportService;
        private readonly IExplorationService explorationService;
        private readonly IWellProjectService wellProjectService;
        private readonly IWellProjectWellService wellProjectWellService;
        private readonly IExplorationWellService explorationWellService;
        private readonly ILogger<CaseService> _logger;

        public DuplicateCaseService(DcdDbContext context, IProjectService projectService, ILoggerFactory loggerFactory, IDrainageStrategyService drainageStrategyService,
            ITopsideService topsideService, ISurfService surfService, ISubstructureService substructureService, ITransportService transportService,
            IExplorationService explorationService, IWellProjectService wellProjectService, IWellProjectWellService wellProjectWellService, IExplorationWellService explorationWellService)
        {
            _context = context;
            _projectService = projectService;
            this.drainageStrategyService = drainageStrategyService;
            this.topsideService = topsideService;
            this.surfService = surfService;
            this.substructureService = substructureService;
            this.transportService = transportService;
            this.explorationService = explorationService;
            this.wellProjectService = wellProjectService;
            this.wellProjectWellService = wellProjectWellService;
            this.explorationWellService = explorationWellService;
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

            drainageStrategyService.CopyDrainageStrategy(caseItem.DrainageStrategyLink, caseItem.Id);
            topsideService.CopyTopside(caseItem.TopsideLink, caseItem.Id);
            surfService.CopySurf(caseItem.SurfLink, caseItem.Id);
            substructureService.CopySubstructure(caseItem.SubstructureLink, caseItem.Id);
            transportService.CopyTransport(caseItem.TransportLink, caseItem.Id);

            var newWellProject = wellProjectService.CopyWellProject(caseItem.WellProjectLink, caseItem.Id);
            var newExploration = explorationService.CopyExploration(caseItem.ExplorationLink, caseItem.Id);

            wellProjectWellService.CopyWellProjectWell(sourceWellProjectId, newWellProject.Id);
            explorationWellService.CopyExplorationWell(sourceExplorationId, newExploration.Id);

            _context.SaveChanges();
            return _projectService.GetProjectDto(project.Id);
        }
    }
}
