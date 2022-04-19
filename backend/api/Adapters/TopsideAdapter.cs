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

            topside.CostProfile = Convert(topsideDto.CostProfile, topside);

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
            existing.CostProfile = Convert(topsideDto.CostProfile, existing);
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
    }
}
