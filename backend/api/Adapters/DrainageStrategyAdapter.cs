using api.Dtos;
using api.Models;
using api.Services;

namespace api.Adapters
{
    public class DrainageStrategyAdapter
    {
        private readonly IProjectService _projectService = null!;

        public DrainageStrategyAdapter(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public DrainageStrategy Convert(DrainageStrategyDto drainageStrategyDto)
        {
            var drainageStrategy = new DrainageStrategy();
            drainageStrategy.Name = drainageStrategyDto.Name;
            drainageStrategy.Description = drainageStrategyDto.Description;
            drainageStrategy.Project = _projectService.GetProject(drainageStrategyDto.ProjectId);
            drainageStrategy.NGLYield = drainageStrategyDto.NGLYield;
            drainageStrategy.ArtificialLift = drainageStrategyDto.ArtificialLift;
            drainageStrategy.ProductionProfileOil = Convert(drainageStrategyDto.ProductionProfileOil, drainageStrategy);
            drainageStrategy.ProductionProfileGas = Convert(drainageStrategyDto.ProductionProfileGas, drainageStrategy);
            drainageStrategy.ProductionProfileWater = Convert(drainageStrategyDto.ProductionProfileWater, drainageStrategy);
            drainageStrategy.ProductionProfileWaterInjection = Convert(drainageStrategyDto.ProductionProfileWaterInjection, drainageStrategy);
            drainageStrategy.FuelFlaringAndLosses = Convert(drainageStrategyDto.FuelFlaringAndLosses, drainageStrategy);
            drainageStrategy.NetSalesGas = Convert(drainageStrategyDto.NetSalesGas, drainageStrategy);
            drainageStrategy.Co2Emissions = Convert(drainageStrategyDto.Co2Emissions, drainageStrategy);
            return drainageStrategy;
        }

        private ProductionProfileOil Convert(ProductionProfileOilDto? productionProfileOilDto, DrainageStrategy drainageStrategy)
        {
            if (productionProfileOilDto == null)
            {
                throw new ArgumentException("ProductionProfileOil is required.");
            }
            return new ProductionProfileOil
            {
                DrainageStrategy = drainageStrategy,
                YearValues = productionProfileOilDto.YearValues
            };
        }

        private ProductionProfileGas Convert(ProductionProfileGasDto? productionProfileGasDto, DrainageStrategy drainageStrategy)
        {
            if (productionProfileGasDto == null)
            {
                throw new ArgumentException("ProductionProfileGas is required.");
            }
            return new ProductionProfileGas
            {
                DrainageStrategy = drainageStrategy,
                YearValues = productionProfileGasDto.YearValues
            };
        }

        private ProductionProfileWater Convert(ProductionProfileWaterDto? productionProfileWaterDto, DrainageStrategy drainageStrategy)
        {
            if (productionProfileWaterDto == null)
            {
                throw new ArgumentException("ProductionProfileWater is required.");
            }
            return new ProductionProfileWater
            {
                DrainageStrategy = drainageStrategy,
                YearValues = productionProfileWaterDto.YearValues
            };
        }

        private ProductionProfileWaterInjection Convert(ProductionProfileWaterInjectionDto? productionProfileWaterInjectionDto, DrainageStrategy drainageStrategy)
        {
            if (productionProfileWaterInjectionDto == null)
            {
                throw new ArgumentException("ProductionProfileWaterInjection is required.");
            }
            return new ProductionProfileWaterInjection
            {
                DrainageStrategy = drainageStrategy,
                YearValues = productionProfileWaterInjectionDto.YearValues
            };
        }

        private FuelFlaringAndLosses Convert(FuelFlaringAndLossesDto? fuelFlaringAndLossesDto, DrainageStrategy drainageStrategy)
        {
            if (fuelFlaringAndLossesDto == null)
            {
                throw new ArgumentException("FuelFlaringAndLosses is required.");
            }
            return new FuelFlaringAndLosses
            {
                DrainageStrategy = drainageStrategy,
                YearValues = fuelFlaringAndLossesDto.YearValues
            };
        }

        private NetSalesGas Convert(NetSalesGasDto? netSalesGasDto, DrainageStrategy drainageStrategy)
        {
            if (netSalesGasDto == null)
            {
                throw new ArgumentException("NetSalesGas is required.");
            }
            return new NetSalesGas
            {
                DrainageStrategy = drainageStrategy,
                YearValues = netSalesGasDto.YearValues
            };
        }

        private Co2Emissions Convert(Co2EmissionsDto? co2EmissionsDto, DrainageStrategy drainageStrategy)
        {
            if (co2EmissionsDto == null)
            {
                throw new ArgumentException("Co2Emissions is required.");
            }
            return new Co2Emissions
            {
                DrainageStrategy = drainageStrategy,
                YearValues = co2EmissionsDto.YearValues
            };
        }
    }
}
