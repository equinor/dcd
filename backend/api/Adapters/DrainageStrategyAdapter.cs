using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class DrainageStrategyAdapter
    {
        public static DrainageStrategy Convert(DrainageStrategyDto drainageStrategyDto, PhysUnit unit)
        {
            var drainageStrategy = DrainagestrategyDtoToDrainagestrategy(null, drainageStrategyDto);

            if (drainageStrategyDto.ProductionProfileOil != null)
            {
                drainageStrategy.ProductionProfileOil = Convert(drainageStrategyDto.ProductionProfileOil, drainageStrategy, unit);
            }
            if (drainageStrategyDto.ProductionProfileGas != null)
            {
                drainageStrategy.ProductionProfileGas = Convert(drainageStrategyDto.ProductionProfileGas, drainageStrategy, unit);
            }
            if (drainageStrategyDto.ProductionProfileWater != null)
            {
                drainageStrategy.ProductionProfileWater = Convert(drainageStrategyDto.ProductionProfileWater, drainageStrategy, unit);
            }
            if (drainageStrategyDto.ProductionProfileWaterInjection != null)
            {
                drainageStrategy.ProductionProfileWaterInjection = Convert(drainageStrategyDto.ProductionProfileWaterInjection, drainageStrategy, unit);
            }
            if (drainageStrategyDto.FuelFlaringAndLosses != null)
            {
                drainageStrategy.FuelFlaringAndLosses = Convert(drainageStrategyDto.FuelFlaringAndLosses, drainageStrategy, unit);
            }
            if (drainageStrategyDto.NetSalesGas != null)
            {
                drainageStrategy.NetSalesGas = Convert(drainageStrategyDto.NetSalesGas, drainageStrategy, unit);
            }
            if (drainageStrategyDto.Co2Emissions != null)
            {
                drainageStrategy.Co2Emissions = Convert(drainageStrategyDto.Co2Emissions, drainageStrategy, unit);
            }
            if (drainageStrategyDto.ProductionProfileNGL != null)
            {
                drainageStrategy.ProductionProfileNGL = Convert(drainageStrategyDto.ProductionProfileNGL, drainageStrategy, unit);
            }
            return drainageStrategy;
        }

        public static DrainageStrategy ConvertExisting(DrainageStrategy existing, DrainageStrategyDto drainageStrategyDto, PhysUnit unit)
        {
            DrainagestrategyDtoToDrainagestrategy(existing, drainageStrategyDto);

            if (drainageStrategyDto.ProductionProfileOil != null)
            {
                existing.ProductionProfileOil = Convert(drainageStrategyDto.ProductionProfileOil, existing, unit);
            }
            if (drainageStrategyDto.ProductionProfileGas != null)
            {
                existing.ProductionProfileGas = Convert(drainageStrategyDto.ProductionProfileGas, existing, unit);
            }
            if (drainageStrategyDto.ProductionProfileWater != null)
            {
                existing.ProductionProfileWater = Convert(drainageStrategyDto.ProductionProfileWater, existing, unit);
            }
            if (drainageStrategyDto.ProductionProfileWaterInjection != null)
            {
                existing.ProductionProfileWaterInjection = Convert(drainageStrategyDto.ProductionProfileWaterInjection, existing, unit);
            }
            if (drainageStrategyDto.FuelFlaringAndLosses != null)
            {
                existing.FuelFlaringAndLosses = Convert(drainageStrategyDto.FuelFlaringAndLosses, existing, unit);
            }
            if (drainageStrategyDto.NetSalesGas != null)
            {
                existing.NetSalesGas = Convert(drainageStrategyDto.NetSalesGas, existing, unit);
            }
            if (drainageStrategyDto.Co2Emissions != null)
            {
                existing.Co2Emissions = Convert(drainageStrategyDto.Co2Emissions, existing, unit);
            }
            if (drainageStrategyDto.ProductionProfileNGL != null)
            {
                existing.ProductionProfileNGL = Convert(drainageStrategyDto.ProductionProfileNGL, existing, unit);
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

        private static double[] ConvertUnitValues(double[] values, PhysUnit unit, string type)
        {

            if (unit == PhysUnit.SI)
            {
                // Per now - the timeseriestypes which use millions are the same in both SI and Oilfield
                if (type.Equals("NetSalesGas") || type.Equals("FuelFlaringAndLosses") || type.Equals("ProfileGas"))
                {
                    // These types should be saved in billions
                    values = Array.ConvertAll(values, x => x * 1E9);
                }
                else
                {
                    // These types should be saved in millions
                    values = Array.ConvertAll(values, x => x * 1E6);
                }
            }
            else if ( unit == PhysUnit.OilField)
            {
                // Per now - the timeseriestypes which use millions are the same in both SI and Oilfield
                if (type.Equals("NetSalesGas") || type.Equals("FuelFlaringAndLosses") || type.Equals("ProfileGas"))
                {
                    // These types should be saved in billions
                    values = Array.ConvertAll(values, x => x * 1E9);
                }
                else
                {
                    // These types should be saved in millions
                    values = Array.ConvertAll(values, x => x * 1E6);
                }
            }

            return values;
        }

        private static ProductionProfileOil? Convert(ProductionProfileOilDto? productionProfileOilDto, DrainageStrategy drainageStrategy, PhysUnit unit)
        {
            var needToConvertValues = drainageStrategy?.ProductionProfileOil?.Values == null;
            if (productionProfileOilDto != null && drainageStrategy?.ProductionProfileOil?.Values != null && !drainageStrategy.ProductionProfileOil.Values.SequenceEqual(productionProfileOilDto.Values))
            {
                needToConvertValues = true;
            }
            var convertedTimeSeries = productionProfileOilDto == null || drainageStrategy == null ? null : new ProductionProfileOil
            {
                Id = productionProfileOilDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = productionProfileOilDto.StartYear,
                Values = needToConvertValues ? ConvertUnitValues(productionProfileOilDto.Values, unit, "ProfileOil") : productionProfileOilDto.Values,
        };
            return convertedTimeSeries;
        }

        private static ProductionProfileGas? Convert(ProductionProfileGasDto? productionProfileGasDto, DrainageStrategy drainageStrategy, PhysUnit unit)
        {
            var needToConvertValues = drainageStrategy?.ProductionProfileGas?.Values == null;
            if (productionProfileGasDto != null && drainageStrategy?.ProductionProfileGas?.Values != null && !drainageStrategy.ProductionProfileGas.Values.SequenceEqual(productionProfileGasDto.Values)) 
            {
                needToConvertValues = true;
            }
            var convertedTimeSeries = productionProfileGasDto == null || drainageStrategy == null ? null : new ProductionProfileGas
            {
                Id = productionProfileGasDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = productionProfileGasDto.StartYear,
                Values = needToConvertValues ? ConvertUnitValues(productionProfileGasDto.Values, unit, "ProfileGas") : productionProfileGasDto.Values,
            };
            return convertedTimeSeries;
        }

        private static ProductionProfileWater? Convert(ProductionProfileWaterDto? productionProfileWaterDto, DrainageStrategy drainageStrategy, PhysUnit unit)
        {
            var needToConvertValues = drainageStrategy?.ProductionProfileWater?.Values == null;
            if (productionProfileWaterDto != null && drainageStrategy?.ProductionProfileWater?.Values != null && !drainageStrategy.ProductionProfileWater.Values.SequenceEqual(productionProfileWaterDto.Values))
            {
                needToConvertValues = true;
            }
            var convertedTimeSeries = productionProfileWaterDto == null || drainageStrategy == null ? null : new ProductionProfileWater
            {
                Id = productionProfileWaterDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = productionProfileWaterDto.StartYear,
                Values = needToConvertValues ? ConvertUnitValues(productionProfileWaterDto.Values, unit, "ProfileWater") : productionProfileWaterDto.Values,
            };
            return convertedTimeSeries;
        }

        private static ProductionProfileWaterInjection? Convert(ProductionProfileWaterInjectionDto? productionProfileWaterInjectionDto, DrainageStrategy drainageStrategy, PhysUnit unit)
        {
            var needToConvertValues = drainageStrategy?.ProductionProfileWaterInjection?.Values == null;
            if (productionProfileWaterInjectionDto != null && drainageStrategy?.ProductionProfileWaterInjection?.Values != null && !drainageStrategy.ProductionProfileWaterInjection.Values.SequenceEqual(productionProfileWaterInjectionDto.Values))
            {
                needToConvertValues = true;
            }
            var convertedTimeSeries = productionProfileWaterInjectionDto == null || drainageStrategy == null ? null : new ProductionProfileWaterInjection
            {
                Id = productionProfileWaterInjectionDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = productionProfileWaterInjectionDto.StartYear,
                Values = needToConvertValues ? ConvertUnitValues(productionProfileWaterInjectionDto.Values, unit, "ProfileWaterInjection") : productionProfileWaterInjectionDto.Values,
            };
            return convertedTimeSeries;
        }

        private static FuelFlaringAndLosses? Convert(FuelFlaringAndLossesDto? fuelFlaringAndLossesDto, DrainageStrategy drainageStrategy, PhysUnit unit)
        {
            var needToConvertValues = drainageStrategy?.FuelFlaringAndLosses?.Values == null;
            if (fuelFlaringAndLossesDto != null && drainageStrategy?.FuelFlaringAndLosses?.Values != null && !drainageStrategy.FuelFlaringAndLosses.Values.SequenceEqual(fuelFlaringAndLossesDto.Values))
            {
                needToConvertValues = true;
            }
            var convertedTimeSeries = fuelFlaringAndLossesDto == null || drainageStrategy == null ? null : new FuelFlaringAndLosses
            {
                Id = fuelFlaringAndLossesDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = fuelFlaringAndLossesDto.StartYear,
                Values = needToConvertValues ? ConvertUnitValues(fuelFlaringAndLossesDto.Values, unit, "FuelFlaringAndLosses") : fuelFlaringAndLossesDto.Values,
            };
            return convertedTimeSeries;
        }

        private static NetSalesGas? Convert(NetSalesGasDto? netSalesGasDto, DrainageStrategy drainageStrategy, PhysUnit unit)
        {
            var needToConvertValues = drainageStrategy?.NetSalesGas?.Values == null;
            if (netSalesGasDto != null && drainageStrategy?.NetSalesGas?.Values != null && !drainageStrategy.NetSalesGas.Values.SequenceEqual(netSalesGasDto.Values))
            {
                needToConvertValues = true;
            }
            var convertedTimeSeries = netSalesGasDto == null || drainageStrategy == null ? null : new NetSalesGas
            {
                Id = netSalesGasDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = netSalesGasDto.StartYear,
                Values = needToConvertValues ? ConvertUnitValues(netSalesGasDto.Values, unit, "NetSalesGas") : netSalesGasDto.Values,
            };
            return convertedTimeSeries;
        }

        private static Co2Emissions? Convert(Co2EmissionsDto? co2EmissionsDto, DrainageStrategy drainageStrategy, PhysUnit unit)
        {
            var needToConvertValues = drainageStrategy?.Co2Emissions?.Values != null;
            if (co2EmissionsDto != null && drainageStrategy?.Co2Emissions?.Values != null && !drainageStrategy.Co2Emissions.Values.SequenceEqual(co2EmissionsDto.Values))
            {
                needToConvertValues = true;
            }
            var convertedTimeSeries = co2EmissionsDto == null || drainageStrategy == null ? null : new Co2Emissions
            {
                Id = co2EmissionsDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = co2EmissionsDto.StartYear,
                Values = needToConvertValues ? ConvertUnitValues(co2EmissionsDto.Values, unit, "Co2Emissions") : co2EmissionsDto.Values,
            };
            return convertedTimeSeries;
        }

        private static ProductionProfileNGL? Convert(ProductionProfileNGLDto? productionProfileNGLDto, DrainageStrategy drainageStrategy, PhysUnit unit)
        {
            var needToConvertValues = drainageStrategy?.ProductionProfileNGL?.Values != null;
            if (productionProfileNGLDto != null && drainageStrategy?.ProductionProfileNGL?.Values != null && !drainageStrategy.ProductionProfileNGL.Values.SequenceEqual(productionProfileNGLDto.Values))
            {
                needToConvertValues = true;
            }
            var convertedTimeSeries = productionProfileNGLDto == null || drainageStrategy == null ? null : new ProductionProfileNGL
            {
                Id = productionProfileNGLDto.Id,
                DrainageStrategy = drainageStrategy,
                StartYear = productionProfileNGLDto.StartYear,
                Values = needToConvertValues ? ConvertUnitValues(productionProfileNGLDto.Values, unit, "ProfileNGL") : productionProfileNGLDto.Values,
            };
            return convertedTimeSeries;
        }
    }
}
