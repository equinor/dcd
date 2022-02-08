using api.Dtos;
using api.Models;

namespace api.Adapters
{

    public static class TransportDtoAdapter
    {

        public static TransportDto Convert(Transport transportDto)
        {
            var transport = new TransportDto();
            transport.ProjectId = transportDto.ProjectId;
            transport.Name = transportDto.Name;
            transport.Maturity = transportDto.Maturity;
            transport.CostProfile = Convert(transportDto.CostProfile);
            return transport;
        }

        private static TransportCostProfileDto Convert(TransportCostProfile costprofile)
        {
            var transportCostProfile = new TransportCostProfileDto();
            transportCostProfile.Currency = costprofile.Currency;
            transportCostProfile.EPAVersion = costprofile.EPAVersion;
            transportCostProfile.YearValues = costprofile.YearValues;
            return transportCostProfile;
        }
    }
}
