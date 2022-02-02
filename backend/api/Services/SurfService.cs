using System.Linq;

using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class SurfService : ISurfService
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
                        .ThenInclude(c => c.YearValues)
                    .Include(c => c.Maturity)
                    .Include(c => c.UmbilicalSystemLength)
                    .Include(c => c.ProductionFlowline)
                    .Include(c => c.ArtificialLift)
                    .Include(c => c.Project);

            }
            else
            {
                return new List<Surf>();
            }
        }

        public Surf UpdateSurf(Surf surf)
        {
            if (_context.Surfs != null)
            {
                var result = _context.Surfs.Update(surf);
                _context.SaveChanges();
                return result.Entity;
            }
            else
            {
                throw new Exception(); //TODO FIX EXCEPTION
            }
        }

        public Surf CreateSurf(Surf surf)
        {
            if (_context.Surfs != null)
            {
                var project = _projectService.GetProject(surf.ProjectId);
                _projectService.AddSurfsToProject(project, surf);
                var result = _context.Surfs.Add(surf);
                _context.SaveChanges();
                return result.Entity;
            }
            else
            {
                throw new Exception(); //TODO FIX EXCEPTION
            }
        }

        public bool DeleteSurf(Surf surf)
        {
            if (_context.Surfs != null)
            {
                _context.Surfs.Remove(surf);
                _context.SaveChanges();
                return true;
            }
            else
            {
                throw new Exception(); //TODO FIX EXCEPTION
            }
        }




    }
}
