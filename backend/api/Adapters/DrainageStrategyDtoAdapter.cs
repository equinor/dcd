using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class DrainageStrategyDtoAdapter
    {
        public static DrainageStrategyDto Convert(DrainageStrategy drainageStrategy, PhysUnit unit)
        {
            var drainageStrategyDto = new DrainageStrategyDto
            {
                Id = drainageStrategy.Id,
                Name = drainageStrategy.Name,
                Description = drainageStrategy.Description,
                ProjectId = drainageStrategy.ProjectId,
                ProjectUnit = drainageStrategy.ProjectUnit,
                NGLYield = drainageStrategy.NGLYield,
                ArtificialLift = drainageStrategy.ArtificialLift,
                ProducerCount = drainageStrategy.ProducerCount,
                GasInjectorCount = drainageStrategy.GasInjectorCount,
                WaterInjectorCount = drainageStrategy.WaterInjectorCount,
                FacilitiesAvailability = drainageStrategy.FacilitiesAvailability,
                ProductionProfileOil = Convert(drainageStrategy.ProductionProfileOil, unit),
                ProductionProfileGas = Convert(drainageStrategy.ProductionProfileGas, unit),
                ProductionProfileWater = Convert(drainageStrategy.ProductionProfileWater, unit),
                ProductionProfileWaterInjection = Convert(drainageStrategy.ProductionProfileWaterInjection, unit),
                FuelFlaringAndLosses = Convert(drainageStrategy.FuelFlaringAndLosses, unit),
                NetSalesGas = Convert(drainageStrategy.NetSalesGas, unit),
                Co2Emissions = Convert(drainageStrategy.Co2Emissions, unit),
                ProductionProfileNGL = Convert(drainageStrategy.ProductionProfileNGL, unit)
            };
            return drainageStrategyDto;
        }

        private static double[] ConvertUnitValues(double[] values, PhysUnit unit, string type)
        {
            string[] MTPA_Units = { nameof(Co2Emissions), nameof(ProductionProfileNGL) };
            string[] BBL_Units = { nameof(ProductionProfileOil), nameof(ProductionProfileWater), nameof(ProductionProfileWaterInjection) };
            string[] SCF_Units = { nameof(ProductionProfileGas), nameof(FuelFlaringAndLosses), nameof(NetSalesGas) };

            // Per now - the timeseriestypes which use millions are the same in both SI and Oilfield
            if (SCF_Units.Contains(type))
            {
                // Trim zeroes for SCF when sending back to frontend
                values = Array.ConvertAll(values, x => x / 1E9);
            }
            else
            {
                // Trim zeroes for BBL when sending back to frontend
                values = Array.ConvertAll(values, x => x / 1E6);
            }
            
            // If Oilfield is selected, convert to respective values 
            if (unit == PhysUnit.OilField && !MTPA_Units.Contains(type))
            {
                if (BBL_Units.Contains(type))
                {
                    // Unit: From baseunit Sm3 to BBL
                    values = Array.ConvertAll(values, x => x * 6.290);
                }
                else if (SCF_Units.Contains(type))
                {
                    // Unit: From baseunit Sm3 to SCF
                    values = Array.ConvertAll(values, x => x * 35.315);
                }
            }
            return values;
        }

        private static ProductionProfileOilDto? Convert(ProductionProfileOil? productionProfileOil, PhysUnit unit)
        {
            if (productionProfileOil != null)
            {
                return new ProductionProfileOilDto
                {
                    Id = productionProfileOil.Id,
                    StartYear = productionProfileOil.StartYear,
                    Values = ConvertUnitValues(productionProfileOil.Values, unit, nameof(ProductionProfileOil))
                };
            }
            return null;
        }

        private static ProductionProfileGasDto? Convert(ProductionProfileGas? productionProfileGas, PhysUnit unit)
        {
            if (productionProfileGas != null)
            {
                return new ProductionProfileGasDto
                {
                    Id = productionProfileGas.Id,
                    StartYear = productionProfileGas.StartYear,
                    Values = ConvertUnitValues(productionProfileGas.Values, unit, nameof(ProductionProfileGas))
                };
            }
            return null;
        }

        private static ProductionProfileWaterDto? Convert(ProductionProfileWater? productionProfileWater, PhysUnit unit)
        {
            if (productionProfileWater != null)
            {
                return new ProductionProfileWaterDto
                {
                    Id = productionProfileWater.Id,
                    StartYear = productionProfileWater.StartYear,
                    Values = ConvertUnitValues(productionProfileWater.Values, unit, nameof(ProductionProfileWater))
                };
            }
            return null;
        }

        private static ProductionProfileWaterInjectionDto? Convert(ProductionProfileWaterInjection? productionProfileWaterInjection, PhysUnit unit)
        {
            if (productionProfileWaterInjection != null)
            {
                return new ProductionProfileWaterInjectionDto
                {
                    Id = productionProfileWaterInjection.Id,
                    StartYear = productionProfileWaterInjection.StartYear,
                    Values = ConvertUnitValues(productionProfileWaterInjection.Values, unit, nameof(ProductionProfileWaterInjection))
                };
            }
            return null;
        }

        private static FuelFlaringAndLossesDto? Convert(FuelFlaringAndLosses? fuelFlaringAndLosses, PhysUnit unit)
        {
            if (fuelFlaringAndLosses != null)
            {
                return new FuelFlaringAndLossesDto
                {
                    Id = fuelFlaringAndLosses.Id,
                    StartYear = fuelFlaringAndLosses.StartYear,
                    Values = ConvertUnitValues(fuelFlaringAndLosses.Values, unit, nameof(FuelFlaringAndLosses))
                };
            }
            return null;
        }

        private static NetSalesGasDto? Convert(NetSalesGas? netSalesGas, PhysUnit unit)
        {
            if (netSalesGas != null)
            {
                return new NetSalesGasDto
                {
                    Id = netSalesGas.Id,
                    StartYear = netSalesGas.StartYear,
                    Values = ConvertUnitValues(netSalesGas.Values, unit, nameof(NetSalesGas))
                };
            }
            return null;
        }

        private static Co2EmissionsDto? Convert(Co2Emissions? co2Emissions, PhysUnit unit)
        {
            if (co2Emissions != null)
            {
                return new Co2EmissionsDto
                {
                    Id = co2Emissions.Id,
                    StartYear = co2Emissions.StartYear,
                    Values = ConvertUnitValues(co2Emissions.Values, unit, nameof(Co2Emissions))
                };
            }
            return null;
        }

        private static ProductionProfileNGLDto? Convert(ProductionProfileNGL? productionProfileNGL, PhysUnit unit)
        {
            if (productionProfileNGL != null)
            {
                return new ProductionProfileNGLDto
                {
                    Id = productionProfileNGL.Id,
                    StartYear = productionProfileNGL.StartYear,
                    Values = ConvertUnitValues(productionProfileNGL.Values, unit, nameof(ProductionProfileNGL))
                };
            }
            return null;
        }
    }
}
