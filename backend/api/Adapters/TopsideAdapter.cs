using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class TopsideAdapter
    {
        public static Topside Convert(TopsideDto topsideDto)
        {
            var topside = new Topside
            {
                Id = topsideDto.Id,
                Name = topsideDto.Name,
                ProjectId = topsideDto.ProjectId,
                DryWeight = topsideDto.DryWeight,
                OilCapacity = topsideDto.OilCapacity,
                GasCapacity = topsideDto.GasCapacity,
                FacilitiesAvailability = topsideDto.FacilitiesAvailability,
                ArtificialLift = topsideDto.ArtificialLift,
                Maturity = topsideDto.Maturity,
                Currency = topsideDto.Currency,
                FuelConsumption = topsideDto.FuelConsumption,
                FlaredGas = topsideDto.FlaredGas,
                CO2ShareOilProfile = topsideDto.CO2ShareOilProfile,
                CO2ShareGasProfile = topsideDto.CO2ShareGasProfile,
                CO2ShareWaterInjectionProfile = topsideDto.CO2ShareWaterInjectionProfile,
                CO2OnMaxOilProfile = topsideDto.CO2OnMaxOilProfile,
                CO2OnMaxGasProfile = topsideDto.CO2OnMaxGasProfile,
                CO2OnMaxWaterInjectionProfile = topsideDto.CO2OnMaxWaterInjectionProfile,
                CostYear = topsideDto.CostYear,
                ProspVersion = topsideDto.ProspVersion,
                LastChanged = topsideDto.LastChanged,
                Source = topsideDto.Source
            };

            if (topsideDto.CostProfile != null)
            {
                topside.CostProfile = Convert(topsideDto.CostProfile, topside);
            }

            if (topsideDto.CessationCostProfile != null)
            {
                topside.CessationCostProfile = Convert(topsideDto.CessationCostProfile, topside);
            }

            return topside;
        }

        public static void ConvertExisting(Topside existing, TopsideDto topsideDto)
        {
            existing.Id = topsideDto.Id;
            existing.Name = topsideDto.Name;
            existing.ProjectId = topsideDto.ProjectId;
            existing.DryWeight = topsideDto.DryWeight;
            existing.OilCapacity = topsideDto.OilCapacity;
            existing.GasCapacity = topsideDto.GasCapacity;
            existing.FacilitiesAvailability = topsideDto.FacilitiesAvailability;
            existing.ArtificialLift = topsideDto.ArtificialLift;
            existing.Maturity = topsideDto.Maturity;
            existing.Currency = topsideDto.Currency;
            existing.CostProfile = Convert(topsideDto.CostProfile, existing);
            existing.CessationCostProfile = Convert(topsideDto.CessationCostProfile, existing);
            existing.FuelConsumption = topsideDto.FuelConsumption;
            existing.FlaredGas = topsideDto.FlaredGas;
            existing.CO2ShareOilProfile = topsideDto.CO2ShareOilProfile;
            existing.CO2ShareGasProfile = topsideDto.CO2ShareGasProfile;
            existing.CO2ShareWaterInjectionProfile = topsideDto.CO2ShareWaterInjectionProfile;
            existing.CO2OnMaxOilProfile = topsideDto.CO2OnMaxOilProfile;
            existing.CO2OnMaxGasProfile = topsideDto.CO2OnMaxGasProfile;
            existing.CO2OnMaxWaterInjectionProfile = topsideDto.CO2OnMaxWaterInjectionProfile;
            existing.CostYear = topsideDto.CostYear;
            existing.ProspVersion = topsideDto.ProspVersion;
            existing.LastChanged = topsideDto.LastChanged;
            existing.Source = topsideDto.Source;
        }
        private static TopsideCostProfile? Convert(TopsideCostProfileDto? costprofile, Topside topside)
        {
            if (costprofile == null)
            {
                return null;
            }
            var topsideCostProfile = new TopsideCostProfile
            {
                Id = costprofile.Id,
                Currency = costprofile.Currency,
                EPAVersion = costprofile.EPAVersion,
                Topside = topside,
                StartYear = costprofile.StartYear,
                Values = costprofile.Values
            };

            return topsideCostProfile;
        }

        private static TopsideCessationCostProfile? Convert(TopsideCessationCostProfileDto? topsideCessationCostProfileDto, Topside topside)
        {
            if (topsideCessationCostProfileDto == null)
            {
                return null;
            }
            TopsideCessationCostProfile topsideCessationCostProfile = new TopsideCessationCostProfile
            {
                Id = topsideCessationCostProfileDto.Id,
                Currency = topsideCessationCostProfileDto.Currency,
                EPAVersion = topsideCessationCostProfileDto.EPAVersion,
                Topside = topside,
                StartYear = topsideCessationCostProfileDto.StartYear,
                Values = topsideCessationCostProfileDto.Values
            };

            return topsideCessationCostProfile;
        }
    }
}
