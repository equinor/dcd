using api.Dtos;
using api.Models;
using api.Services;

namespace api.Adapters
{

    public static class TransportAdapter
    {

        public static Transport Convert(TransportDto transportDto)
        {
            var transport = new Transport();
            transport.ProjectId = transportDto.ProjectId;
            transport.Name = transportDto.Name;
            transport.Maturity = transportDto.Maturity;
            transport.GasExportPipelineLength = transportDto.GasExportPipelineLength;
            transport.OilExportPipelineLength = transportDto.OilExportPipelineLength;
            transport.CostProfile = Convert(transportDto.CostProfile, transport);
            return transport;
        }

        private static TransportCostProfile Convert(TransportCostProfileDto costprofile, Transport transport)
        {
            var transportCostProfile = new TransportCostProfile();
            transportCostProfile.Currency = costprofile.Currency;
            transportCostProfile.EPAVersion = costprofile.EPAVersion;
            transportCostProfile.StartYear = costprofile.StartYear;
            transportCostProfile.Values = costprofile.Values;
            transportCostProfile.Transport = transport;
            return transportCostProfile;
        }
    }
}
