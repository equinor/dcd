using api.Context;
using api.Dtos;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class SubstructureService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;

        public SubstructureService(DcdDbContext context, ProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        public IEnumerable<Substructure> GetSubstructures(Guid projectId)
        {
            if (_context.Substructures != null)
            {
                return _context.Substructures
                        .Include(c => c.CostProfile)
                            .ThenInclude(c => c.YearValues)
                    .Where(c => c.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<Substructure>();
            }
        }

        public ProjectDto CreateSubstructure(Substructure substructure, Guid sourceCaseId)
        {
            var project = _projectService.GetProject(substructure.ProjectId);
            substructure.Project = project;
            _context.Substructures!.Add(substructure);
            _context.SaveChanges();
            SetCaseLink(substructure, sourceCaseId, project);
            return _projectService.GetProjectDto(project.Id);
        }

        private void SetCaseLink(Substructure substructure, Guid sourceCaseId, Project project)
        {
            var case_ = project.Cases.FirstOrDefault(o => o.Id == sourceCaseId);
            if (case_ == null)
            {
                throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
            }
            case_.SubstructureLink = substructure.Id;
            _context.SaveChanges();
        }

        public ProjectDto DeleteSubstructure(Guid substructureId)
        {
            var substructure = GetSubstructure(substructureId);
            _context.Substructures!.Remove(substructure);
            DeleteCaseLinks(substructureId);
            _context.SaveChanges();
            return _projectService.GetProjectDto(substructure.ProjectId);
        }

        private void DeleteCaseLinks(Guid substructureId)
        {
            foreach (Case c in _context.Cases!)
            {
                if (c.SubstructureLink == substructureId)
                {
                    c.SubstructureLink = Guid.Empty;
                }
            }
        }

        public ProjectDto UpdateSubstructure(Guid substructureId, Substructure updatedSubstructure)
        {
            var substructure = GetSubstructure(substructureId);
            CopyData(substructure, updatedSubstructure);
            _context.Substructures!.Update(substructure);
            _context.SaveChanges();
            return _projectService.GetProjectDto(substructure.ProjectId);
        }

        public Substructure GetSubstructure(Guid substructureId)
        {
            var substructure = _context.Substructures!
                .Include(c => c.Project)
                .Include(c => c.CostProfile)
                    .ThenInclude(c => c.YearValues)
                .FirstOrDefault(o => o.Id == substructureId);
            if (substructure == null)
            {
                throw new ArgumentException(string.Format("Substructure {0} not found.", substructureId));
            }
            return substructure;
        }

        private static void CopyData(Substructure substructure, Substructure updatedSubstructure)
        {
            substructure.Name = updatedSubstructure.Name;
            substructure.DryWeight = updatedSubstructure.DryWeight;
            substructure.Maturity = updatedSubstructure.Maturity;
            substructure.CostProfile.Currency = updatedSubstructure.CostProfile.Currency;
            substructure.CostProfile.EPAVersion = updatedSubstructure.CostProfile.EPAVersion;
            substructure.CostProfile.YearValues = updatedSubstructure.CostProfile.YearValues;
        }
    }
}
