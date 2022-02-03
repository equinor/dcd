using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public class DrainageStrategyAdapter
    {
        public DrainageStrategy Convert(DrainageStrategyDto drainageStrategyDto)
        {
            var drainageStrategy = new DrainageStrategy();
            drainageStrategy.Name = drainageStrategyDto.Name;
            drainageStrategy.Description = drainageStrategyDto.Description;
            drainageStrategy.ProjectId = drainageStrategyDto.ProjectId;
            drainageStrategy.NGLYield = drainageStrategyDto.NGLYield;
            drainageStrategy.ArtificialLift = drainageStrategyDto.ArtificialLift;
            drainageStrategy.ProducerCount = drainageStrategyDto.ProducerCount;
            drainageStrategy.GasInjectorCount = drainageStrategyDto.GasInjectorCount;
            drainageStrategy.WaterInjectorCount = drainageStrategyDto.WaterInjectorCount;
            drainageStrategy.ProductionProfileOil = Convert(drainageStrategyDto.ProductionProfileOil, drainageStrategy);
            drainageStrategy.ProductionProfileGas = Convert(drainageStrategyDto.ProductionProfileGas, drainageStrategy);
            drainageStrategy.ProductionProfileWater = Convert(drainageStrategyDto.ProductionProfileWater, drainageStrategy);
            drainageStrategy.ProductionProfileWaterInjection = Convert(drainageStrategyDto.ProductionProfileWaterInjection, drainageStrategy);
            drainageStrategy.FuelFlaringAndLosses = Convert(drainageStrategyDto.FuelFlaringAndLosses, drainageStrategy);
            drainageStrategy.NetSalesGas = Convert(drainageStrategyDto.NetSalesGas, drainageStrategy);
            drainageStrategy.Co2Emissions = Convert(drainageStrategyDto.Co2Emissions, drainageStrategy);
            return drainageStrategy;
        }

        private ProductionProfileOil Convert(ProductionProfileOilDto productionProfileOilDto, DrainageStrategy drainageStrategy)
        {
            return new ProductionProfileOil
            {
                DrainageStrategy = drainageStrategy,
                YearValues = productionProfileOilDto.YearValues
            };
        }

        private ProductionProfileGas Convert(ProductionProfileGasDto productionProfileGasDto, DrainageStrategy drainageStrategy)
        {
            return new ProductionProfileGas
            {
                DrainageStrategy = drainageStrategy,
                YearValues = productionProfileGasDto.YearValues
            };
        }

        private ProductionProfileWater Convert(ProductionProfileWaterDto productionProfileWaterDto, DrainageStrategy drainageStrategy)
        {
            return new ProductionProfileWater
            {
                DrainageStrategy = drainageStrategy,
                YearValues = productionProfileWaterDto.YearValues
            };
        }

        private ProductionProfileWaterInjection Convert(ProductionProfileWaterInjectionDto productionProfileWaterInjectionDto, DrainageStrategy drainageStrategy)
        {
            return new ProductionProfileWaterInjection
            {
                DrainageStrategy = drainageStrategy,
                YearValues = productionProfileWaterInjectionDto.YearValues
            };
        }

        private FuelFlaringAndLosses Convert(FuelFlaringAndLossesDto fuelFlaringAndLossesDto, DrainageStrategy drainageStrategy)
        {
            return new FuelFlaringAndLosses
            {
                DrainageStrategy = drainageStrategy,
                YearValues = fuelFlaringAndLossesDto.YearValues
            };
        }

        private NetSalesGas Convert(NetSalesGasDto netSalesGasDto, DrainageStrategy drainageStrategy)
        {
            return new NetSalesGas
            {
                DrainageStrategy = drainageStrategy,
                YearValues = netSalesGasDto.YearValues
            };
        }

        private Co2Emissions Convert(Co2EmissionsDto co2EmissionsDto, DrainageStrategy drainageStrategy)
        {
            return new Co2Emissions
            {
                DrainageStrategy = drainageStrategy,
                YearValues = co2EmissionsDto.YearValues
            };
        }
    }
}
