using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class SurfAdapter
    {

        public static Surf Convert(SurfDto surfDto)
        {
            var surf = new Surf
            {
                Id = surfDto.Id,
                ProjectId = surfDto.ProjectId,
                Name = surfDto.Name,
                ArtificialLift = surfDto.ArtificialLift,
                Maturity = surfDto.Maturity,
                InfieldPipelineSystemLength = surfDto.InfieldPipelineSystemLength,
                UmbilicalSystemLength = surfDto.UmbilicalSystemLength,
                ProductionFlowline = surfDto.ProductionFlowline,
                RiserCount = surfDto.RiserCount,
                TemplateCount = surfDto.TemplateCount,
                ProducerCount = surfDto.ProducerCount,
                GasInjectorCount = surfDto.GasInjectorCount,
                WaterInjectorCount = surfDto.WaterInjectorCount,
                Currency = surfDto.Currency,
                LastChangedDate = surfDto.LastChangedDate,
                CostYear = surfDto.CostYear,
                Source = surfDto.Source,
                ProspVersion = surfDto.ProspVersion
            };

            surf.CostProfile = Convert(surfDto.CostProfile, surf);
            surf.CessationCostProfile = Convert(surfDto.CessationCostProfile, surf);

            return surf;
        }

        public static void ConvertExisting(Surf existing, SurfDto surfDto)
        {
            existing.Id = surfDto.Id;
            existing.ProjectId = surfDto.ProjectId;
            existing.Name = surfDto.Name;
            existing.ArtificialLift = surfDto.ArtificialLift;
            existing.Maturity = surfDto.Maturity;
            existing.InfieldPipelineSystemLength = surfDto.InfieldPipelineSystemLength;
            existing.UmbilicalSystemLength = surfDto.UmbilicalSystemLength;
            existing.ProductionFlowline = surfDto.ProductionFlowline;
            existing.RiserCount = surfDto.RiserCount;
            existing.TemplateCount = surfDto.TemplateCount;
            existing.ProducerCount = surfDto.ProducerCount;
            existing.GasInjectorCount = surfDto.GasInjectorCount;
            existing.WaterInjectorCount = surfDto.GasInjectorCount;
            existing.Currency = surfDto.Currency;
            existing.CostProfile = Convert(surfDto.CostProfile, existing);
            existing.CessationCostProfile = Convert(surfDto.CessationCostProfile, existing);
            existing.LastChangedDate = surfDto.LastChangedDate;
            existing.CostYear = surfDto.CostYear;
            existing.Source = surfDto.Source;
            existing.ProspVersion = surfDto.ProspVersion;
        }

        private static SurfCostProfile? Convert(SurfCostProfileDto? costprofile, Surf surf)
        {
            if (costprofile == null)
            {
                return null;
            }
            var surfCostProfile = new SurfCostProfile
            {
                Id = costprofile.Id,
                Currency = costprofile.Currency,
                EPAVersion = costprofile.EPAVersion,
                StartYear = costprofile.StartYear,
                Values = costprofile.Values,
                Surf = surf
            };
            return surfCostProfile;
        }

        private static SurfCessationCostProfile? Convert(SurfCessationCostProfileDto? cessationCostProfileDto, Surf surf)
        {

            if (cessationCostProfileDto == null)
            {
                return null;
            }

            SurfCessationCostProfile surfCessationCostProfile = new SurfCessationCostProfile
            {
                Id = cessationCostProfileDto.Id,
                Currency = cessationCostProfileDto.Currency,
                EPAVersion = cessationCostProfileDto.EPAVersion,
                StartYear = cessationCostProfileDto.StartYear,
                Values = cessationCostProfileDto.Values,
                Surf = surf
            };

            return surfCessationCostProfile;
        }


    }
}
