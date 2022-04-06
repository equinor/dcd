using api.Dtos;
using api.Models;

namespace api.Adapters
{

    public static class TransportAdapter
    {

        public static Transport Convert(TransportDto transportDto)
        {
            var transport = new Transport 
            {
                Id = transportDto.Id,
                ProjectId = transportDto.ProjectId,
                Name = transportDto.Name,
                Maturity = transportDto.Maturity,
                GasExportPipelineLength = transportDto.GasExportPipelineLength,
                OilExportPipelineLength = transportDto.OilExportPipelineLength,
            };
            transport.CostProfile = Convert(transportDto.CostProfile, transport);
            return transport;
        }

        private static TransportCostProfile? Convert(TransportCostProfileDto? costprofile, Transport transport)
        {
            if (costprofile == null)
            {
                return null;
            }

            var transportCostProfile = new TransportCostProfile
            {
                Id = costprofile.Id,
                Currency = costprofile.Currency,
                EPAVersion = costprofile.EPAVersion,
                Transport = transport,
                StartYear = costprofile.StartYear,
                Values = costprofile.Values,
            };

            return transportCostProfile;
        }
    }
}
