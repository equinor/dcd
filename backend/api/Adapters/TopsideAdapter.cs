using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class TopsideAdapter
    {
        public static Topside Convert(TopsideDto topsideDto)
        {
            var topside = new Topside();
            topside.Id = topsideDto.Id;
            topside.Name = topsideDto.Name;
            topside.ProjectId = topsideDto.ProjectId;
            topside.CostProfile = Convert(topsideDto.CostProfile, topside);
            topside.DryWeight = topsideDto.DryWeight;
            topside.OilCapacity = topsideDto.OilCapacity;
            topside.GasCapacity = topsideDto.GasCapacity;
            topside.FacilitiesAvailability = topsideDto.FacilitiesAvailability;
            topside.ArtificialLift = topsideDto.ArtificialLift;
            topside.Maturity = topsideDto.Maturity;
            return topside;
        }

        private static TopsideCostProfile Convert(TopsideCostProfileDto topsideCostProfileDto, Topside topside)
        {
            return new TopsideCostProfile
            {
                Id = topsideCostProfileDto.Id,
                Currency = topsideCostProfileDto.Currency,
                EPAVersion = topsideCostProfileDto.EPAVersion,
                Topside = topside,
                StartYear = topsideCostProfileDto.StartYear,
                Values = topsideCostProfileDto.Values
            };
        }
    }
}
