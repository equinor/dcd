using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public class TopsideAdapter
    {
        public Topside Convert(TopsideDto topsideDto)
        {
            var topside = new Topside();
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

        private TopsideCostProfile Convert(TopsideCostProfileDto topsideCostProfileDto, Topside topside)
        {
            return new TopsideCostProfile
            {
                Currency = topsideCostProfileDto.Currency,
                EPAVersion = topsideCostProfileDto.EPAVersion,
                Topside = topside,
                YearValues = topsideCostProfileDto.YearValues
            };
        }
    }
}
