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

        private TopsideCostProfile Convert(TopsideCostProfileDto? costProfileDto, Topside topside)
        {
            if (costProfileDto == null)
            {
                return null!;
            }
            return new TopsideCostProfile
            {
                Currency = costProfileDto.Currency,
                EPAVersion = costProfileDto.EPAVersion,
                Topside = topside,
                YearValues = costProfileDto.YearValues,
            };
        }
    }
}
