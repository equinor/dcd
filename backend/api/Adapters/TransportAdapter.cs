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
                Currency = transportDto.Currency
            };

            if (transportDto.CostProfile != null)
            {
                transport.CostProfile = Convert(transportDto.CostProfile, transport);
            }

            if (transportDto.CessationCostProfile != null)
            {
                transport.CessationCostProfile = Convert(transportDto.CessationCostProfile, transport);
            }
            return transport;
        }

        public static void ConvertExisting(Transport existing, TransportDto transportDto)
        {
            existing.Id = transportDto.Id;
            existing.ProjectId = transportDto.ProjectId;
            existing.Name = transportDto.Name;
            existing.Maturity = transportDto.Maturity;
            existing.GasExportPipelineLength = transportDto.GasExportPipelineLength;
            existing.OilExportPipelineLength = transportDto.OilExportPipelineLength;
            existing.Currency = transportDto.Currency;
            existing.CostProfile = Convert(transportDto.CostProfile, existing);
            existing.CessationCostProfile = Convert(transportDto.CessationCostProfile, existing);
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

        private static TransportCessationCostProfile? Convert(TransportCessationCostProfileDto? transportCessationCostProfileDto, Transport transport)
        {
            if (transportCessationCostProfileDto == null)
            {
                return null;
            }

            TransportCessationCostProfile transportCessationCostProfile = new TransportCessationCostProfile
            {
                Id = transportCessationCostProfileDto.Id,
                Currency = transportCessationCostProfileDto.Currency,
                EPAVersion = transportCessationCostProfileDto.EPAVersion,
                Transport = transport,
                StartYear = transportCessationCostProfileDto.StartYear,
                Values = transportCessationCostProfileDto.Values,
            };

            return transportCessationCostProfile;
        }
    }
}
