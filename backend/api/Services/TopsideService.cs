using api.Adapters;
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
        private readonly ILogger<TopsideService> _logger;

        public TopsideService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory)
        {
            _context = context;
            _projectService = projectService;
            _logger = loggerFactory.CreateLogger<TopsideService>();
        }

        public IEnumerable<Topside> GetTopsides(Guid projectId)
        {
            if (_context.Topsides != null)
            {
                return _context.Topsides
                        .Include(c => c.CostProfile)
                        .Include(c => c.CessationCostProfile)
                    .Where(c => c.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<Topside>();
            }
        }

        public ProjectDto CreateTopside(TopsideDto topsideDto, Guid sourceCaseId)
        {
            var topside = TopsideAdapter.Convert(topsideDto);
            var project = _projectService.GetProject(topsideDto.ProjectId);
            topside.Project = project;
            topside.LastChangedDate = DateTimeOffset.Now;
            topside.ProspVersion = null;
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

        public ProjectDto UpdateTopside(TopsideDto updatedTopsideDto)
        {
            var existing = GetTopside(updatedTopsideDto.Id);
            TopsideAdapter.ConvertExisting(existing, updatedTopsideDto);

            if (updatedTopsideDto.CostProfile == null && existing.CostProfile != null)
            {
                _context.TopsideCostProfiles!.Remove(existing.CostProfile);
            }

            if (updatedTopsideDto.CessationCostProfile == null && existing.CessationCostProfile != null)
            {
                _context.TopsideCessationCostProfiles!.Remove(existing.CessationCostProfile);
            }
            existing.LastChangedDate = DateTimeOffset.Now;
            _context.Topsides!.Update(existing);
            _context.SaveChanges();
            return _projectService.GetProjectDto(updatedTopsideDto.ProjectId);
        }

        public Topside GetTopside(Guid topsideId)
        {
            var topside = _context.Topsides!
                .Include(c => c.Project)
                .Include(c => c.CostProfile)
                .Include(c => c.CessationCostProfile)
                .FirstOrDefault(o => o.Id == topsideId);
            if (topside == null)
            {
                throw new ArgumentException(string.Format("Topside {0} not found.", topsideId));
            }
            return topside;
        }
    }
}
