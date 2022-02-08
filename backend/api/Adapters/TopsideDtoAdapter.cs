using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class TopsideDtoAdapter
    {
        public static TopsideDto Convert(Topside topside_)
        {
            var topside = new TopsideDto();
            topside.Name = topside_.Name;
            topside.ProjectId = topside_.ProjectId;
            topside.CostProfile = Convert(topside_.CostProfile);
            topside.DryWeight = topside_.DryWeight;
            topside.OilCapacity = topside_.OilCapacity;
            topside.GasCapacity = topside_.GasCapacity;
            topside.FacilitiesAvailability = topside_.FacilitiesAvailability;
            topside.ArtificialLift = topside_.ArtificialLift;
            topside.Maturity = topside_.Maturity;
            return topside;
        }

        private static TopsideCostProfileDto Convert(TopsideCostProfile topsideCostProfile)
        {
            return new TopsideCostProfileDto
            {
                Currency = topsideCostProfile.Currency,
                EPAVersion = topsideCostProfile.EPAVersion,
                YearValues = topsideCostProfile.YearValues
            };
        }
    }
}
