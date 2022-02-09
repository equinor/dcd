using api.Dtos;
using api.Models;

namespace api.Adapters
{

    public static class TransportDtoAdapter
    {

        public static TransportDto Convert(Transport transport)
        {
            var transportDto = new TransportDto();
            transportDto.Id = transport.Id;
            transportDto.ProjectId = transport.ProjectId;
            transportDto.Name = transport.Name;
            transportDto.Maturity = transport.Maturity;
            transportDto.CostProfile = Convert(transport.CostProfile);
            return transportDto;
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
