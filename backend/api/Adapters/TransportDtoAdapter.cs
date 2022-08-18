using api.Dtos;
using api.Models;

namespace api.Adapters
{

    public static class TransportDtoAdapter
    {

        public static TransportDto Convert(Transport transport)
        {
            var transportDto = new TransportDto
            {
                Id = transport.Id,
                ProjectId = transport.ProjectId,
                Name = transport.Name,
                Maturity = transport.Maturity,
                GasExportPipelineLength = transport.GasExportPipelineLength,
                OilExportPipelineLength = transport.OilExportPipelineLength,
                Currency = transport.Currency,
                LastChangedDate = transport.LastChangedDate,
                CostYear = transport.CostYear,
                Source = transport.Source,
                ProspVersion = transport.ProspVersion,
                CostProfile = Convert(transport.CostProfile),
                CessationCostProfile = Convert(transport.CessationCostProfile),
                DG3Date = transport.DG3Date,
                DG4Date = transport.DG4Date
            };
            return transportDto;
        }

        private static TransportCostProfileDto? Convert(TransportCostProfile? costprofile)
        {
            if (costprofile == null)
            {
                return null;
            }
            return new TransportCostProfileDto()
            {
                Id = costprofile.Id,
                Currency = costprofile.Currency,
                EPAVersion = costprofile.EPAVersion,
                StartYear = costprofile.StartYear,
                Values = costprofile.Values,
            };
        }

        private static TransportCessationCostProfileDto? Convert(TransportCessationCostProfile? transportCessationCostProfile)
        {
            if (transportCessationCostProfile == null)
            {
                return null;
            }
            return new TransportCessationCostProfileDto()
            {
                Id = transportCessationCostProfile.Id,
                Currency = transportCessationCostProfile.Currency,
                EPAVersion = transportCessationCostProfile.EPAVersion,
                StartYear = transportCessationCostProfile.StartYear,
                Values = transportCessationCostProfile.Values,
            };
        }
    }
}
