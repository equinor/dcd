using System.Linq;

using api.Context;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class DrainageStrategyService
    {
        private readonly DcdDbContext _context;

        public DrainageStrategyService(DcdDbContext context)
        {
            _context = context;
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
    }
}

