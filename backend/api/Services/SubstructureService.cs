using api.Context;
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

        public Substructure CreateSubstructure(Substructure substructure)
        {
            var project = _projectService.GetProject(substructure.ProjectId);
            substructure.Project = project;
            var result = _context.Substructures!.Add(substructure);
            _context.SaveChanges();
            return result.Entity;
        }

        public bool DeleteSubstructure(Guid substructureId)
        {
            var substructure = GetSubstructure(substructureId);
            _context.Substructures!.Remove(substructure);
            _context.SaveChanges();
            return true;
        }

        public Substructure UpdateSubstructure(Guid substructureId, Substructure updatedSubstructure)
        {
            var substructure = GetSubstructure(substructureId);
            CopyData(substructure, updatedSubstructure);
            var result = _context.Substructures!.Update(substructure);
            _context.SaveChanges();
            return result.Entity;
        }

        private Substructure GetSubstructure(Guid substructureId)
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
