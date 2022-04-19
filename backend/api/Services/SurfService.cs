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
        public SurfService(DcdDbContext context, ProjectService projectService)
        {
            _context = context;
            _projectService = projectService;

        }

        public IEnumerable<Surf> GetSurfs(Guid projectId)
        {
            if (_context.Surfs != null)
            {
                return _context.Surfs
                    .Include(c => c.CostProfile)
                    //          .ThenInclude(c => c.YearValues)
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

            _context.Surfs!.Update(existing);
            _context.SaveChanges();
            return _projectService.GetProjectDto(existing.ProjectId);
        }
        public Surf GetSurf(Guid surfId)
        {
            var surf = _context.Surfs!
                .Include(c => c.CostProfile)
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
