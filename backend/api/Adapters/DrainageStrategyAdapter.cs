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
                StartYear = productionProfileOilDto.StartYear,
                Values = productionProfileOilDto.Values
            };
        }

        private ProductionProfileGas Convert(ProductionProfileGasDto productionProfileGasDto, DrainageStrategy drainageStrategy)
        {
            return new ProductionProfileGas
            {
                DrainageStrategy = drainageStrategy,
                StartYear = productionProfileGasDto.StartYear,
                Values = productionProfileGasDto.Values
            };
        }

        private ProductionProfileWater Convert(ProductionProfileWaterDto productionProfileWaterDto, DrainageStrategy drainageStrategy)
        {
            return new ProductionProfileWater
            {
                DrainageStrategy = drainageStrategy,
                StartYear = productionProfileWaterDto.StartYear,
                Values = productionProfileWaterDto.Values
            };
        }

        private ProductionProfileWaterInjection Convert(ProductionProfileWaterInjectionDto productionProfileWaterInjectionDto, DrainageStrategy drainageStrategy)
        {
            return new ProductionProfileWaterInjection
            {
                DrainageStrategy = drainageStrategy,
                StartYear = productionProfileWaterInjectionDto.StartYear,
                Values = productionProfileWaterInjectionDto.Values
            };
        }

        private FuelFlaringAndLosses Convert(FuelFlaringAndLossesDto fuelFlaringAndLossesDto, DrainageStrategy drainageStrategy)
        {
            return new FuelFlaringAndLosses
            {
                DrainageStrategy = drainageStrategy,
                StartYear = fuelFlaringAndLossesDto.StartYear,
                Values = fuelFlaringAndLossesDto.Values
            };
        }

        private NetSalesGas Convert(NetSalesGasDto netSalesGasDto, DrainageStrategy drainageStrategy)
        {
            return new NetSalesGas
            {
                DrainageStrategy = drainageStrategy,
                StartYear = netSalesGasDto.StartYear,
                Values = netSalesGasDto.Values
            };
        }

        private Co2Emissions Convert(Co2EmissionsDto co2EmissionsDto, DrainageStrategy drainageStrategy)
        {
            return new Co2Emissions
            {
                DrainageStrategy = drainageStrategy,
                StartYear = co2EmissionsDto.StartYear,
                Values = co2EmissionsDto.Values
            };
        }
    }
}
