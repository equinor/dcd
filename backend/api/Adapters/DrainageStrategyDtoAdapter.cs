using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class DrainageStrategyDtoAdapter
    {
        public static DrainageStrategyDto Convert(DrainageStrategy drainageStrategy)
        {
            var drainageStrategyDto = new DrainageStrategyDto();
            drainageStrategyDto.Id = drainageStrategy.Id;
            drainageStrategyDto.Name = drainageStrategy.Name;
            drainageStrategyDto.Description = drainageStrategy.Description;
            drainageStrategyDto.ProjectId = drainageStrategy.ProjectId;
            drainageStrategyDto.NGLYield = drainageStrategy.NGLYield;
            drainageStrategyDto.ArtificialLift = drainageStrategy.ArtificialLift;
            drainageStrategyDto.ProducerCount = drainageStrategy.ProducerCount;
            drainageStrategyDto.GasInjectorCount = drainageStrategy.GasInjectorCount;
            drainageStrategyDto.WaterInjectorCount = drainageStrategy.WaterInjectorCount;
            drainageStrategyDto.ProductionProfileOil = Convert(drainageStrategy.ProductionProfileOil)!;
            drainageStrategyDto.ProductionProfileGas = Convert(drainageStrategy.ProductionProfileGas)!;
            drainageStrategyDto.ProductionProfileWater = Convert(drainageStrategy.ProductionProfileWater)!;
            drainageStrategyDto.ProductionProfileWaterInjection = Convert(drainageStrategy.ProductionProfileWaterInjection)!;
            drainageStrategyDto.FuelFlaringAndLosses = Convert(drainageStrategy.FuelFlaringAndLosses)!;
            drainageStrategyDto.NetSalesGas = Convert(drainageStrategy.NetSalesGas)!;
            drainageStrategyDto.Co2Emissions = Convert(drainageStrategy.Co2Emissions)!;
            return drainageStrategyDto;
        }

        private static ProductionProfileOilDto? Convert(ProductionProfileOil? productionProfileOil)
        {
            if (productionProfileOil != null)
            {
                return new ProductionProfileOilDto
                {
                    Id = productionProfileOil.Id,
                    StartYear = productionProfileOil.StartYear,
                    Values = productionProfileOil.Values
                };
            }
            return null;
        }

        private static ProductionProfileGasDto? Convert(ProductionProfileGas? productionProfileGas)
        {
            if (productionProfileGas != null)
            {
                return new ProductionProfileGasDto
                {
                    Id = productionProfileGas.Id,
                    StartYear = productionProfileGas.StartYear,
                    Values = productionProfileGas.Values
                };
            }
            return null;
        }

        private static ProductionProfileWaterDto? Convert(ProductionProfileWater? productionProfileWater)
        {
            if (productionProfileWater != null)
            {
                return new ProductionProfileWaterDto
                {
                    Id = productionProfileWater.Id,
                    StartYear = productionProfileWater.StartYear,
                    Values = productionProfileWater.Values
                };
            }
            return null;
        }

        private static ProductionProfileWaterInjectionDto? Convert(ProductionProfileWaterInjection? productionProfileWaterInjection)
        {
            if (productionProfileWaterInjection != null)
            {
                return new ProductionProfileWaterInjectionDto
                {
                    Id = productionProfileWaterInjection.Id,
                    StartYear = productionProfileWaterInjection.StartYear,
                    Values = productionProfileWaterInjection.Values
                };
            }
            return null;
        }

        private static FuelFlaringAndLossesDto? Convert(FuelFlaringAndLosses? fuelFlaringAndLosses)
        {
            if (fuelFlaringAndLosses != null)
            {
                return new FuelFlaringAndLossesDto
                {
                    Id = fuelFlaringAndLosses.Id,
                    StartYear = fuelFlaringAndLosses.StartYear,
                    Values = fuelFlaringAndLosses.Values
                };
            }
            return null;
        }

        private static NetSalesGasDto? Convert(NetSalesGas? netSalesGas)
        {
            if (netSalesGas != null)
            {
                return new NetSalesGasDto
                {
                    Id = netSalesGas.Id,
                    StartYear = netSalesGas.StartYear,
                    Values = netSalesGas.Values
                };
            }
            return null;
        }

        private static Co2EmissionsDto? Convert(Co2Emissions? co2Emissions)
        {
            if (co2Emissions != null)
            {
                return new Co2EmissionsDto
                {
                    Id = co2Emissions.Id,
                    StartYear = co2Emissions.StartYear,
                    Values = co2Emissions.Values
                };
            }
            return null;
        }
    }
}
