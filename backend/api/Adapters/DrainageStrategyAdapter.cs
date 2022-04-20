using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class DrainageStrategyAdapter
    {
        public static DrainageStrategy Convert(DrainageStrategyDto drainageStrategyDto)
        {
            var drainageStrategy = DrainagestrategyDtoToDrainagestrategy(null, drainageStrategyDto);

            if (drainageStrategyDto.ProductionProfileOil != null)
            {
                drainageStrategy.ProductionProfileOil = Convert(drainageStrategyDto.ProductionProfileOil, drainageStrategy);
            }
            if (drainageStrategyDto.ProductionProfileGas != null)
            {
                drainageStrategy.ProductionProfileGas = Convert(drainageStrategyDto.ProductionProfileGas, drainageStrategy);
            }
            if (drainageStrategyDto.ProductionProfileWater != null)
            {
                drainageStrategy.ProductionProfileWater = Convert(drainageStrategyDto.ProductionProfileWater, drainageStrategy);
            }
            if (drainageStrategyDto.ProductionProfileWaterInjection != null)
            {
                drainageStrategy.ProductionProfileWaterInjection = Convert(drainageStrategyDto.ProductionProfileWaterInjection, drainageStrategy);
            }
            if (drainageStrategyDto.FuelFlaringAndLosses != null)
            {
                drainageStrategy.FuelFlaringAndLosses = Convert(drainageStrategyDto.FuelFlaringAndLosses, drainageStrategy);
            }
            if (drainageStrategyDto.NetSalesGas != null)
            {
                drainageStrategy.NetSalesGas = Convert(drainageStrategyDto.NetSalesGas, drainageStrategy);
            }
            if (drainageStrategyDto.Co2Emissions != null)
            {
                drainageStrategy.Co2Emissions = Convert(drainageStrategyDto.Co2Emissions, drainageStrategy);
            }
            return drainageStrategy;
        }

        public static DrainageStrategy ConvertExisting(DrainageStrategy existing, DrainageStrategyDto drainageStrategyDto)
        {
            DrainagestrategyDtoToDrainagestrategy(existing, drainageStrategyDto);

            if (drainageStrategyDto.ProductionProfileOil != null)
            {
                existing.ProductionProfileOil = Convert(drainageStrategyDto.ProductionProfileOil, existing);
            }
            if (drainageStrategyDto.ProductionProfileGas != null)
            {
                existing.ProductionProfileGas = Convert(drainageStrategyDto.ProductionProfileGas, existing);
            }
            if (drainageStrategyDto.ProductionProfileWater != null)
            {
                existing.ProductionProfileWater = Convert(drainageStrategyDto.ProductionProfileWater, existing);
            }
            if (drainageStrategyDto.ProductionProfileWaterInjection != null)
            {
                existing.ProductionProfileWaterInjection = Convert(drainageStrategyDto.ProductionProfileWaterInjection, existing);
            }
            if (drainageStrategyDto.FuelFlaringAndLosses != null)
            {
                existing.FuelFlaringAndLosses = Convert(drainageStrategyDto.FuelFlaringAndLosses, existing);
            }
            if (drainageStrategyDto.NetSalesGas != null)
            {
                existing.NetSalesGas = Convert(drainageStrategyDto.NetSalesGas, existing);
            }
            if (drainageStrategyDto.Co2Emissions != null)
            {
                existing.Co2Emissions = Convert(drainageStrategyDto.Co2Emissions, existing);
            }
            return existing;
        }

        private static DrainageStrategy DrainagestrategyDtoToDrainagestrategy(DrainageStrategy? existing, DrainageStrategyDto drainageStrategyDto)
        {
            if (existing == null)
            {
                return new DrainageStrategy
                {
                    Id = drainageStrategyDto.Id,
                    Name = drainageStrategyDto.Name,
                    Description = drainageStrategyDto.Description,
                    ProjectId = drainageStrategyDto.ProjectId,
                    NGLYield = drainageStrategyDto.NGLYield,
                    ArtificialLift = drainageStrategyDto.ArtificialLift,
                    ProducerCount = drainageStrategyDto.ProducerCount,
                    GasInjectorCount = drainageStrategyDto.GasInjectorCount,
                    WaterInjectorCount = drainageStrategyDto.WaterInjectorCount
                };
            }
            existing.Id = drainageStrategyDto.Id;
            existing.Name = drainageStrategyDto.Name;
            existing.Description = drainageStrategyDto.Description;
            existing.ProjectId = drainageStrategyDto.ProjectId;
            existing.NGLYield = drainageStrategyDto.NGLYield;
            existing.ArtificialLift = drainageStrategyDto.ArtificialLift;
            existing.ProducerCount = drainageStrategyDto.ProducerCount;
            existing.GasInjectorCount = drainageStrategyDto.GasInjectorCount;
            existing.WaterInjectorCount = drainageStrategyDto.WaterInjectorCount;

            return existing;
        }

        private static ProductionProfileOil? Convert(ProductionProfileOilDto? productionProfileOilDto, DrainageStrategy drainageStrategy)
        {
            return productionProfileOilDto == null ? null : new ProductionProfileOil
            {
                Id = productionProfileOilDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = productionProfileOilDto.StartYear,
                Values = productionProfileOilDto.Values
            };
        }

        private static ProductionProfileGas? Convert(ProductionProfileGasDto? productionProfileGasDto, DrainageStrategy drainageStrategy)
        {
            return productionProfileGasDto == null ? null : new ProductionProfileGas
            {
                Id = productionProfileGasDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = productionProfileGasDto.StartYear,
                Values = productionProfileGasDto.Values
            };
        }

        private static ProductionProfileWater? Convert(ProductionProfileWaterDto? productionProfileWaterDto, DrainageStrategy drainageStrategy)
        {
            return productionProfileWaterDto == null ? null : new ProductionProfileWater
            {
                Id = productionProfileWaterDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = productionProfileWaterDto.StartYear,
                Values = productionProfileWaterDto.Values
            };
        }

        private static ProductionProfileWaterInjection? Convert(ProductionProfileWaterInjectionDto? productionProfileWaterInjectionDto, DrainageStrategy drainageStrategy)
        {
            return productionProfileWaterInjectionDto == null ? null : new ProductionProfileWaterInjection
            {
                Id = productionProfileWaterInjectionDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = productionProfileWaterInjectionDto.StartYear,
                Values = productionProfileWaterInjectionDto.Values
            };
        }

        private static FuelFlaringAndLosses? Convert(FuelFlaringAndLossesDto? fuelFlaringAndLossesDto, DrainageStrategy drainageStrategy)
        {
            return fuelFlaringAndLossesDto == null ? null : new FuelFlaringAndLosses
            {
                Id = fuelFlaringAndLossesDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = fuelFlaringAndLossesDto.StartYear,
                Values = fuelFlaringAndLossesDto.Values
            };
        }

        private static NetSalesGas? Convert(NetSalesGasDto? netSalesGasDto, DrainageStrategy drainageStrategy)
        {
            return netSalesGasDto == null ? null : new NetSalesGas
            {
                Id = netSalesGasDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = netSalesGasDto.StartYear,
                Values = netSalesGasDto.Values
            };
        }

        private static Co2Emissions? Convert(Co2EmissionsDto? co2EmissionsDto, DrainageStrategy drainageStrategy)
        {
            return co2EmissionsDto == null ? null : new Co2Emissions
            {
                Id = co2EmissionsDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = co2EmissionsDto.StartYear,
                Values = co2EmissionsDto.Values
            };
        }
    }
}
