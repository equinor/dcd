using api.Adapters;
using api.Context;
using api.Dtos;
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
                        .Include(c => c.GAndGAdminCost)
                        .Include(c => c.DrillingSchedule)
                    .Where(d => d.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<Exploration>();
            }
        }

        public ProjectDto CreateExploration(ExplorationDto eplorationDto, Guid sourceCaseId)
        {
            var exploration = ExplorationAdapter.Convert(eplorationDto);
            var project = _projectService.GetProject(exploration.ProjectId);
            exploration.Project = project;
            _context.Explorations!.Add(exploration);
            _context.SaveChanges();
            SetCaseLink(exploration, sourceCaseId, project);
            return _projectService.GetProjectDto(exploration.ProjectId);
        }

        private void SetCaseLink(Exploration exploration, Guid sourceCaseId, Project project)
        {
            var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
            if (case_ == null)
            {
                throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
            }
            case_.ExplorationLink = exploration.Id;
            _context.SaveChanges();
        }

        public ProjectDto DeleteExploration(Guid explorationId)
        {
            var exploration = GetExploration(explorationId);
            _context.Explorations!.Remove(exploration);
            DeleteCaseLinks(explorationId);
            _context.SaveChanges();
            return _projectService.GetProjectDto(exploration.ProjectId);
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


        public ProjectDto UpdateExploration(ExplorationDto updatedExplorationDto)
        {
            var updatedExploration = ExplorationAdapter.Convert(updatedExplorationDto);
            _context.Explorations!.Update(updatedExploration);
            _context.SaveChanges();
            return _projectService.GetProjectDto(updatedExploration.ProjectId);
        }

        public Exploration GetExploration(Guid explorationId)
        {

            var exploration = _context.Explorations!
                        .Include(c => c.CostProfile)
                        .Include(c => c.GAndGAdminCost)
                        .Include(c => c.DrillingSchedule)
                             .FirstOrDefault(o => o.Id == explorationId);
            if (exploration == null)
            {
                throw new ArgumentException(string.Format("Exploration {0} not found.", explorationId));
            }
            return exploration;
        }
    }
}

