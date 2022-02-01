using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class DrainageStrategyService
    {
        private readonly DcdDbContext _context;
        private readonly ProjectService _projectService;

        public DrainageStrategyService(DcdDbContext context, ProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        public IEnumerable<DrainageStrategy> GetDrainageStrategies(Guid projectId)
        {
            if (_context.DrainageStrategies != null)
            {
                return _context.DrainageStrategies
                        .Include(c => c.ProductionProfileOil)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.ProductionProfileGas)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.ProductionProfileWater)
                           .ThenInclude(c => c.YearValues)
                        .Include(c => c.ProductionProfileWaterInjection)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.FuelFlaringAndLosses)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.NetSalesGas)
                            .ThenInclude(c => c.YearValues)
                        .Include(c => c.Co2Emissions)
                            .ThenInclude(c => c.YearValues)
                    .Where(d => d.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<DrainageStrategy>();
            }
        }

        public DrainageStrategy CreateDrainageStrategy(DrainageStrategy drainageStrategy)
        {
            var result = _context.DrainageStrategies!.Add(drainageStrategy);
            _context.SaveChanges();
            return result.Entity;
        }
    }
}
