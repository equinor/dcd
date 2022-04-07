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
                OilCapacity = topside.OilCapacity,
                GasCapacity = topside.GasCapacity,
                FacilitiesAvailability = topside.FacilitiesAvailability,
                ArtificialLift = topside.ArtificialLift,
                Maturity = topside.Maturity,
                CostProfile = Convert(topside.CostProfile),
                TopsideCessasionCostProfileDto = Convert(topside.TopsideCessasionCostProfile)
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

         private static TopsideCessasionCostProfileDto? Convert(TopsideCessasionCostProfile? topsideCessasionCostProfile)
        {
            if (topsideCessasionCostProfile == null)
            {
                return null;
            }

            TopsideCessasionCostProfileDto topsideCostProfile = new TopsideCessasionCostProfileDto
            {
                Id = topsideCessasionCostProfile.Id,
                Currency = topsideCessasionCostProfile.Currency,
                EPAVersion = topsideCessasionCostProfile.EPAVersion,
                Values = topsideCessasionCostProfile.Values,
                StartYear = topsideCessasionCostProfile.StartYear
            };
            return topsideCostProfile;
        }
    }
}
