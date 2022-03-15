using api.Adapters;
using api.Context;
using api.Dtos;
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
                        .Include(c => c.ProductionProfileGas)
                        .Include(c => c.ProductionProfileWater)
                        .Include(c => c.ProductionProfileWaterInjection)
                        .Include(c => c.FuelFlaringAndLosses)
                        .Include(c => c.NetSalesGas)
                        .Include(c => c.Co2Emissions)
                    .Where(d => d.Project.Id.Equals(projectId));
            }
            else
            {
                return new List<DrainageStrategy>();
            }
        }

        public ProjectDto CreateDrainageStrategy(DrainageStrategyDto drainageStrategyDto, Guid sourceCaseId)
        {
            var drainageStrategy = DrainageStrategyAdapter.Convert(drainageStrategyDto);
            var project = _projectService.GetProject(drainageStrategy.ProjectId);
            drainageStrategy.Project = project;
            _context.DrainageStrategies!.Add(drainageStrategy);
            _context.SaveChanges();
            SetCaseLink(drainageStrategy, sourceCaseId, project);
            return _projectService.GetProjectDto(drainageStrategy.ProjectId);
        }

        private void SetCaseLink(DrainageStrategy drainageStrategy, Guid sourceCaseId, Project project)
        {
            var case_ = project.Cases!.FirstOrDefault(o => o.Id == sourceCaseId);
            if (case_ == null)
            {
                throw new NotFoundInDBException(string.Format("Case {0} not found in database.", sourceCaseId));
            }
            case_.DrainageStrategyLink = drainageStrategy.Id;
            _context.SaveChanges();
        }

        public ProjectDto DeleteDrainageStrategy(Guid drainageStrategyId)
        {
            var drainageStrategy = GetDrainageStrategy(drainageStrategyId);
            _context.DrainageStrategies!.Remove(drainageStrategy);
            DeleteCaseLinks(drainageStrategyId);
            _context.SaveChanges();
            return _projectService.GetProjectDto(drainageStrategy.ProjectId);
        }

        private void DeleteCaseLinks(Guid drainageStrategyId)
        {
            foreach (Case c in _context.Cases!)
            {
                if (c.DrainageStrategyLink == drainageStrategyId)
                {
                    c.DrainageStrategyLink = Guid.Empty;
                }
            }
        }

        public ProjectDto UpdateDrainageStrategy(DrainageStrategyDto updatedDrainageStrategyDto)
        {
            var updatedDrainageStrategy = DrainageStrategyAdapter.Convert(updatedDrainageStrategyDto);
            _context.DrainageStrategies!.Update(updatedDrainageStrategy);
            _context.SaveChanges();
            return _projectService.GetProjectDto(updatedDrainageStrategy.ProjectId);
        }

        public DrainageStrategy GetDrainageStrategy(Guid drainageStrategyId)
        {
            var drainageStrategy = _context.DrainageStrategies!
                .Include(c => c.Project)
                .Include(c => c.ProductionProfileOil)
                .Include(c => c.ProductionProfileGas)
                .Include(c => c.ProductionProfileWater)
                .Include(c => c.ProductionProfileWaterInjection)
                .Include(c => c.FuelFlaringAndLosses)
                .Include(c => c.NetSalesGas)
                .Include(c => c.Co2Emissions)
                .FirstOrDefault(o => o.Id == drainageStrategyId);
            if (drainageStrategy == null)
            {
                throw new ArgumentException(string.Format("Drainage strategy {0} not found.", drainageStrategyId));
            }
            return drainageStrategy;
        }
    }
}
