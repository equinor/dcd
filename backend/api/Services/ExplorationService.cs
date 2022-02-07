using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{

    public class ExplorationService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;

        public ExplorationService(DcdDbContext context, ProjectService
                projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        public IEnumerable<Exploration> GetExplorations(Guid projectId)
        {
            if (_context.Explorations != null)
            {
                return _context.Explorations
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.GAndGAdminCost)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.DrillingSchedule)
                            .ThenInclude(c => c.YearValues)
                    .Where(d => d.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<Exploration>();
            }
        }

        public Project CreateExploration(Exploration exploration, Guid sourceCaseId)
        {
            var project = _projectService.GetProject(exploration.ProjectId);
            exploration.Project = project;
            _context.Explorations!.Add(exploration);
            _context.SaveChanges();
            SetCaseLink(exploration, sourceCaseId, project);
            return _projectService.GetProject(exploration.ProjectId);
        }

        private void SetCaseLink(Exploration exploration, Guid sourceCaseId, Project project)
        {
            var case_ = project.Cases.FirstOrDefault(o => o.Id == sourceCaseId);
            if (case_ == null)
            {
                throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
            }
            case_.ExplorationLink = exploration.Id;
            _context.SaveChanges();
        }

        public Project DeleteExploration(Guid explorationId)
        {
            var exploration = GetExploration(explorationId);
            _context.Explorations!.Remove(exploration);
            _context.SaveChanges();
            DeleteCaseLinks(explorationId);
            return _projectService.GetProject(exploration.ProjectId);
        }

        private void DeleteCaseLinks(Guid explorationId)
        {
            foreach (Case c in _context.Cases!)
            {
                if (c.ExplorationLink == explorationId)
                {
                    c.ExplorationLink = Guid.Empty;
                }
            }
        }
        public Project UpdateExploration(Guid explorationId, Exploration updatedExploration)
        {
            var exploration = GetExploration(explorationId);
            CopyData(exploration, updatedExploration);
            _context.Explorations!.Update(exploration);
            _context.SaveChanges();
            return _projectService.GetProject(exploration.ProjectId);
        }

        public Exploration GetExploration(Guid explorationId)
        {

            var exploration = _context.Explorations!
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.GAndGAdminCost)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.DrillingSchedule)
                            .ThenInclude(c => c.YearValues)
                             .FirstOrDefault(o => o.Id == explorationId);
            if (exploration == null)
            {
                throw new ArgumentException(string.Format("Exploration {0} not found.", explorationId));
            }
            return exploration;
        }

        private static void CopyData(Exploration exploration, Exploration updatedExploration)
        {
            exploration.Name = updatedExploration.Name;
            exploration.RigMobDemob = updatedExploration.RigMobDemob;
            exploration.WellType = updatedExploration.WellType;
            exploration.GAndGAdminCost = updatedExploration.GAndGAdminCost;
            exploration.DrillingSchedule = updatedExploration.DrillingSchedule;
            exploration.CostProfile = updatedExploration.CostProfile;
        }
    }
}

