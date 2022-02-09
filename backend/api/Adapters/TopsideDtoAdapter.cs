using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class TopsideDtoAdapter
    {
        public static TopsideDto Convert(Topside topside)
        {
            var topsideDto = new TopsideDto();
            topsideDto.Id = topside.Id;
            topsideDto.Name = topside.Name;
            topsideDto.ProjectId = topside.ProjectId;
            topsideDto.CostProfile = Convert(topside.CostProfile);
            topsideDto.DryWeight = topside.DryWeight;
            topsideDto.OilCapacity = topside.OilCapacity;
            topsideDto.GasCapacity = topside.GasCapacity;
            topsideDto.FacilitiesAvailability = topside.FacilitiesAvailability;
            topsideDto.ArtificialLift = topside.ArtificialLift;
            topsideDto.Maturity = topside.Maturity;
            return topsideDto;
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
