using api.Dtos;
using api.Models;

namespace api.Adapters
{

    public static class TransportAdapter
    {

        public static Transport Convert(TransportDto transportDto)
        {
            var transport = new Transport();
            transport.Id = transportDto.Id;
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
            return new TransportCostProfile()
            {
                Transport = transport,
                Id = costprofile.Id,
                Currency = costprofile.Currency,
                EPAVersion = costprofile.EPAVersion,
                StartYear = costprofile.StartYear,
                Values = costprofile.Values,
            };
        }
    }
}
