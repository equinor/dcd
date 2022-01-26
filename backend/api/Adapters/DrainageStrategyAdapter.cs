using api.Dtos;
using api.Models;
using api.Services;

namespace api.Adapters
{
    public class DrainageStrategyAdapter
    {
        private ProjectService _projectService = null!;

        public DrainageStrategyAdapter(ProjectService projectService) {
            _projectService = projectService;
        }

        public DrainageStrategy Convert(DrainageStrategyDto drainageStrategyDto) {
            var drainageStrategy = new DrainageStrategy();
            drainageStrategy.Name = drainageStrategyDto.Name;
            drainageStrategy.Project = _projectService.GetProject(drainageStrategyDto.ProjectId);
            drainageStrategy.NGLYield = drainageStrategyDto.NGLYield;
            drainageStrategy.ProductionProfileOil = Convert(drainageStrategyDto.ProductionProfileOil, drainageStrategy);
            drainageStrategy.ProductionProfileGas = Convert(drainageStrategyDto.ProductionProfileGas, drainageStrategy);
            drainageStrategy.ProductionProfileWater = Convert(drainageStrategyDto.ProductionProfileWater, drainageStrategy);
            return drainageStrategy;
        }

        private ProductionProfileOil? Convert(ProductionProfileOilDto? productionProfileOilDto, DrainageStrategy drainageStrategy) {
            if (productionProfileOilDto == null)
                return null;
            return new ProductionProfileOil {
                DrainageStrategy = drainageStrategy,
                YearValues = productionProfileOilDto.YearValues
            };
        }

        private ProductionProfileGas? Convert(ProductionProfileGasDto? productionProfileGasDto, DrainageStrategy drainageStrategy) {
            if (productionProfileGasDto == null)
                return null;
            return new ProductionProfileGas {
                DrainageStrategy = drainageStrategy,
                YearValues = productionProfileGasDto.YearValues
            };
        }

        private ProductionProfileWater? Convert(ProductionProfileWaterDto? productionProfileWaterDto, DrainageStrategy drainageStrategy) {
            if (productionProfileWaterDto == null)
                return null;
            return new ProductionProfileWater {
                DrainageStrategy = drainageStrategy,
                YearValues = productionProfileWaterDto.YearValues
            };
        }
    }
}