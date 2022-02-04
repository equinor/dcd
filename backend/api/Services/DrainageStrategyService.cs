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

        public Project CreateDrainageStrategy(DrainageStrategy drainageStrategy, Guid sourceCaseId)
        {
            var project = _projectService.GetProject(drainageStrategy.ProjectId);
            drainageStrategy.Project = project;
            _context.DrainageStrategies!.Add(drainageStrategy);
            _context.SaveChanges();
            SetCaseLink(drainageStrategy, sourceCaseId, project);
            return _projectService.GetProject(drainageStrategy.ProjectId);
        }

        private void SetCaseLink(DrainageStrategy drainageStrategy, Guid sourceCaseId, Project project)
        {
            var case_ = project.Cases.FirstOrDefault(o => o.Id == sourceCaseId);
            if (case_ == null)
            {
                throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
            }
            case_.DrainageStrategyLink = drainageStrategy.Id;
            _context.SaveChanges();
        }

        public Project DeleteDrainageStrategy(Guid drainageStrategyId)
        {
            var drainageStrategy = GetDrainageStrategy(drainageStrategyId);
            _context.DrainageStrategies!.Remove(drainageStrategy);
            _context.SaveChanges();
            return _projectService.GetProject(drainageStrategy.ProjectId);
        }

        public Project UpdateDrainageStrategy(Guid drainageStrategyId, DrainageStrategy updatedDrainageStrategy)
        {
            var drainageStrategy = GetDrainageStrategy(drainageStrategyId);
            CopyData(drainageStrategy, updatedDrainageStrategy);
            _context.DrainageStrategies!.Update(drainageStrategy);
            _context.SaveChanges();
            return _projectService.GetProject(drainageStrategy.ProjectId);
        }

        private DrainageStrategy GetDrainageStrategy(Guid drainageStrategyId)
        {
            var drainageStrategy = _context.DrainageStrategies!
                .Include(c => c.Project)
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
                .FirstOrDefault(o => o.Id == drainageStrategyId);
            if (drainageStrategy == null)
            {
                throw new ArgumentException(string.Format("Drainage strategy {0} not found.", drainageStrategyId));
            }
            return drainageStrategy;
        }

        private static void CopyData(DrainageStrategy drainageStrategy, DrainageStrategy updatedDrainageStrategy)
        {
            drainageStrategy.Name = updatedDrainageStrategy.Name;
            drainageStrategy.Description = updatedDrainageStrategy.Description;
            drainageStrategy.NGLYield = updatedDrainageStrategy.NGLYield;
            drainageStrategy.ProducerCount = updatedDrainageStrategy.ProducerCount;
            drainageStrategy.GasInjectorCount = updatedDrainageStrategy.GasInjectorCount;
            drainageStrategy.WaterInjectorCount = updatedDrainageStrategy.WaterInjectorCount;
            drainageStrategy.ArtificialLift = updatedDrainageStrategy.ArtificialLift;
            drainageStrategy.ProductionProfileOil = updatedDrainageStrategy.ProductionProfileOil;
            drainageStrategy.ProductionProfileGas = updatedDrainageStrategy.ProductionProfileGas;
            drainageStrategy.ProductionProfileWater = updatedDrainageStrategy.ProductionProfileWater;
            drainageStrategy.ProductionProfileWaterInjection = updatedDrainageStrategy.ProductionProfileWaterInjection;
            drainageStrategy.FuelFlaringAndLosses = updatedDrainageStrategy.FuelFlaringAndLosses;
            drainageStrategy.NetSalesGas = updatedDrainageStrategy.NetSalesGas;
            drainageStrategy.Co2Emissions = updatedDrainageStrategy.Co2Emissions;
        }
    }
}
