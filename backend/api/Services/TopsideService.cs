using api.Context;
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
                            .ThenInclude(c => c.YearValues)
                    .Where(c => c.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<Topside>();
            }
        }

        public Project CreateTopside(Topside topside)
        {
            var project = _projectService.GetProject(topside.ProjectId);
            topside.Project = project;
            _context.Topsides!.Add(topside);
            _context.SaveChanges();
            return _projectService.GetProject(project.Id);
        }

        public Project DeleteTopside(Guid topsideId)
        {
            var topside = GetTopside(topsideId);
            _context.Topsides!.Remove(topside);
            _context.SaveChanges();
            return _projectService.GetProject(topside.ProjectId);
        }

        public Project UpdateTopside(Guid topsideId, Topside updatedTopside)
        {
            var topside = GetTopside(topsideId);
            CopyData(topside, updatedTopside);
            _context.Topsides!.Update(topside);
            _context.SaveChanges();
            return _projectService.GetProject(topside.ProjectId);
        }

        private Topside GetTopside(Guid topsideId)
        {
            var topside = _context.Topsides!
                .Include(c => c.Project)
                .Include(c => c.CostProfile)
                    .ThenInclude(c => c.YearValues)
                .FirstOrDefault(o => o.Id == topsideId);
            if (topside == null)
            {
                throw new ArgumentException(string.Format("Topside {0} not found.", topsideId));
            }
            return topside;
        }

        private static void CopyData(Topside topside, Topside updatedTopside)
        {
            topside.Name = updatedTopside.Name;
            topside.DryWeight = updatedTopside.DryWeight;
            topside.OilCapacity = updatedTopside.OilCapacity;
            topside.GasCapacity = updatedTopside.GasCapacity;
            topside.FacilitiesAvailability = updatedTopside.FacilitiesAvailability;
            topside.ArtificialLift = updatedTopside.ArtificialLift;
            topside.Maturity = updatedTopside.Maturity;
            topside.CostProfile.Currency = updatedTopside.CostProfile.Currency;
            topside.CostProfile.EPAVersion = updatedTopside.CostProfile.EPAVersion;
            topside.CostProfile.YearValues = updatedTopside.CostProfile.YearValues;
        }
    }
}
