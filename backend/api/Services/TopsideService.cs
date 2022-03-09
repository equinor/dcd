using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class TopsideService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;

        public TopsideService(DcdDbContext context, ProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        public IEnumerable<Topside> GetTopsides(Guid projectId)
        {
            if (_context.Topsides != null)
            {
                return _context.Topsides
                        .Include(c => c.CostProfile)
                    .Where(c => c.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<Topside>();
            }
        }

        public ProjectDto CreateTopside(Topside topside, Guid sourceCaseId)
        {
            var project = _projectService.GetProject(topside.ProjectId);
            topside.Project = project;
            _context.Topsides!.Add(topside);
            _context.SaveChanges();
            SetCaseLink(topside, sourceCaseId, project);
            return _projectService.GetProjectDto(project.Id);
        }

        private void SetCaseLink(Topside topside, Guid sourceCaseId, Project project)
        {
            var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
            if (case_ == null)
            {
                throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
            }
            case_.TopsideLink = topside.Id;
            _context.SaveChanges();
        }

        public ProjectDto DeleteTopside(Guid topsideId)
        {
            var topside = GetTopside(topsideId);
            _context.Topsides!.Remove(topside);
            DeleteCaseLinks(topsideId);
            _context.SaveChanges();
            return _projectService.GetProjectDto(topside.ProjectId);
        }

        private void DeleteCaseLinks(Guid topsideId)
        {
            foreach (Case c in _context.Cases!)
            {
                if (c.TopsideLink == topsideId)
                {
                    c.TopsideLink = Guid.Empty;
                }
            }
        }

        public ProjectDto UpdateTopside(Topside updatedTopside)
        {
            _context.Topsides!.Update(updatedTopside);
            _context.SaveChanges();
            return _projectService.GetProjectDto(updatedTopside.ProjectId);
        }

        public Topside GetTopside(Guid topsideId)
        {
            var topside = _context.Topsides!
                .Include(c => c.Project)
                .Include(c => c.CostProfile)
                .FirstOrDefault(o => o.Id == topsideId);
            if (topside == null)
            {
                throw new ArgumentException(string.Format("Topside {0} not found.", topsideId));
            }
            return topside;
        }
    }
}
