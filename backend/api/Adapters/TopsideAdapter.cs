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
                DryWeightUnit = topsideDto.DryWeightUnit,
                OilCapacity = topsideDto.OilCapacity,
                OilCapacityUnit = topsideDto.OilCapacityUnit,
                GasCapacity = topsideDto.GasCapacity,
                GasCapacityUnit = topsideDto.GasCapacityUnit,
                FacilitiesAvailability = topsideDto.FacilitiesAvailability,
                ArtificialLift = topsideDto.ArtificialLift,
                Maturity = topsideDto.Maturity
            };

            if (topsideDto.CostProfile != null)
            {
                topside.CostProfile = Convert(topsideDto.CostProfile, topside);
            }

            if (topsideDto.TopsideCessationCostProfileDto != null)
            {
                topside.TopsideCessationCostProfile = Convert(topsideDto.TopsideCessationCostProfileDto, topside);
            }

            return topside;
        }

        public static void ConvertExisting(Topside existing, TopsideDto topsideDto)
        {
            existing.Id = topsideDto.Id;
            existing.Name = topsideDto.Name;
            existing.ProjectId = topsideDto.ProjectId;
            existing.DryWeight = topsideDto.DryWeight;
            existing.DryWeightUnit = topsideDto.DryWeightUnit;
            existing.OilCapacity = topsideDto.OilCapacity;
            existing.OilCapacityUnit = topsideDto.OilCapacityUnit;
            existing.GasCapacity = topsideDto.GasCapacity;
            existing.GasCapacityUnit = topsideDto.GasCapacityUnit;
            existing.FacilitiesAvailability = topsideDto.FacilitiesAvailability;
            existing.ArtificialLift = topsideDto.ArtificialLift;
            existing.Maturity = topsideDto.Maturity;
            existing.CostProfile = Convert(topsideDto.CostProfile, existing);
            existing.TopsideCessationCostProfile = Convert(topsideDto.TopsideCessationCostProfileDto, existing);
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
