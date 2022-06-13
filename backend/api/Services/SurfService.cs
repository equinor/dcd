using api.Adapters;
using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class SurfService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;
        private readonly ILogger<SurfService> _logger;
        public SurfService(DcdDbContext context, ProjectService projectService, ILoggerFactory loggerFactory)
        {
            _context = context;
            _projectService = projectService;
            _logger = loggerFactory.CreateLogger<SurfService>();

        }

        public IEnumerable<Surf> GetSurfs(Guid projectId)
        {
            if (_context.Surfs != null)
            {
                return _context.Surfs
                    .Include(c => c.CostProfile)
                    .Include(c => c.CessationCostProfile)
                    .Where(c => c.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<Surf>();
            }
        }

        public ProjectDto UpdateSurf(SurfDto updatedSurfDto)
        {
            var existing = GetSurf(updatedSurfDto.Id);
            SurfAdapter.ConvertExisting(existing, updatedSurfDto);

            if (updatedSurfDto.CostProfile == null && existing.CostProfile != null)
            {
                _context.SurfCostProfile!.Remove(existing.CostProfile);
            }

            if (updatedSurfDto.CessationCostProfile == null && existing.CessationCostProfile != null)
            {
                _context.SurfCessationCostProfiles!.Remove(existing.CessationCostProfile);
            }
            existing.LastChangedDate = DateTimeOffset.Now;
            _context.Surfs!.Update(existing);
            _context.SaveChanges();
            return _projectService.GetProjectDto(existing.ProjectId);
        }
        public Surf GetSurf(Guid surfId)
        {
            var surf = _context.Surfs!
                .Include(c => c.CostProfile)
                .Include(c => c.CessationCostProfile)
                .FirstOrDefault(o => o.Id == surfId);
            if (surf == null)
            {
                throw new ArgumentException(string.Format("Surf {0} not found.", surfId));
            }
            return surf;
        }

        public ProjectDto CreateSurf(SurfDto surfDto, Guid sourceCaseId)
        {
            var surf = SurfAdapter.Convert(surfDto);
            var project = _projectService.GetProject(surf.ProjectId);
            surf.Project = project;
            surf.ProspVersion = surfDto.ProspVersion;
            surf.LastChangedDate = DateTimeOffset.Now;
            _context.Surfs!.Add(surf);
            _context.SaveChanges();
            SetCaseLink(surf, sourceCaseId, project);
            return _projectService.GetProjectDto(surf.ProjectId);
        }

        private void SetCaseLink(Surf surf, Guid sourceCaseId, Project project)
        {
            var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
            if (case_ == null)
            {
                throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
            }
            case_.SurfLink = surf.Id;
            _context.SaveChanges();
        }

        public ProjectDto DeleteSurf(Guid surfId)
        {
            var surf = GetSurf(surfId);
            _context.Surfs!.Remove(surf);
            DeleteCaseLinks(surfId);
            return _projectService.GetProjectDto(surf.ProjectId);
        }

        private void DeleteCaseLinks(Guid surfId)
        {
            foreach (Case c in _context.Cases!)
            {
                if (c.SurfLink == surfId)
                {
                    c.SurfLink = Guid.Empty;
                }
            }
            _context.SaveChanges();
        }
    }
}
