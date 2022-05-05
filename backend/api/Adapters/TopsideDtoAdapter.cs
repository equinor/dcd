using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class TopsideDtoAdapter
    {
        public static TopsideDto Convert(Topside topside)
        {
            var topsideDto = new TopsideDto
            {
                Id = topside.Id,
                Name = topside.Name,
                ProjectId = topside.ProjectId,
                DryWeight = topside.DryWeight,
                DryWeightUnit = topside.DryWeightUnit,
                OilCapacity = topside.OilCapacity,
                OilCapacityUnit = topside.OilCapacityUnit,
                GasCapacity = topside.GasCapacity,
                GasCapacityUnit = topside.GasCapacityUnit,
                FacilitiesAvailability = topside.FacilitiesAvailability,
                ArtificialLift = topside.ArtificialLift,
                Maturity = topside.Maturity,
                CostProfile = Convert(topside.CostProfile),
                TopsideCessationCostProfileDto = Convert(topside.TopsideCessationCostProfile)
            };
            return topsideDto;
        }

        private static TopsideCostProfileDto? Convert(TopsideCostProfile? costProfile)
        {
            if (costProfile == null)
            {
                return null;
            }

            var topsideCostProfile = new TopsideCostProfileDto
            {
                Id = costProfile.Id,
                Currency = costProfile.Currency,
                EPAVersion = costProfile.EPAVersion,
                Values = costProfile.Values,
                StartYear = costProfile.StartYear
            };
            return topsideCostProfile;
        }

        private static TopsideCessationCostProfileDto? Convert(TopsideCessationCostProfile? topsideCessationCostProfile)
        {
            if (topsideCessationCostProfile == null)
            {
                return null;
            }

            TopsideCessationCostProfileDto topsideCostProfile = new TopsideCessationCostProfileDto
            {
                Id = topsideCessationCostProfile.Id,
                Currency = topsideCessationCostProfile.Currency,
                EPAVersion = topsideCessationCostProfile.EPAVersion,
                Values = topsideCessationCostProfile.Values,
                StartYear = topsideCessationCostProfile.StartYear
            };
            return topsideCostProfile;
        }
    }
}
