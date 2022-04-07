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
                Maturity = topsideDto.Maturity
            };
            
            if (topsideDto.CostProfile != null) {
                topside.CostProfile = Convert(topsideDto.CostProfile, topside);
            }

            if (topsideDto.TopsideCessasionCostProfileDto != null) {
                topside.TopsideCessasionCostProfile = Convert(topsideDto.TopsideCessasionCostProfileDto, topside);
            }

            return topside;
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

        private static TopsideCessasionCostProfile? Convert(TopsideCessasionCostProfileDto? topsideCessasionCostProfileDto, Topside topside)
        {
            if (topsideCessasionCostProfileDto == null)
            {
                return null;
            }
            TopsideCessasionCostProfile topsideCessasionCostProfile = new TopsideCessasionCostProfile
            {
                Id = topsideCessasionCostProfileDto.Id,
                Currency = topsideCessasionCostProfileDto.Currency,
                EPAVersion = topsideCessasionCostProfileDto.EPAVersion,
                Topside = topside,
                StartYear = topsideCessasionCostProfileDto.StartYear,
                Values = topsideCessasionCostProfileDto.Values
            };

            return topsideCessasionCostProfile;
        }
    }
}
