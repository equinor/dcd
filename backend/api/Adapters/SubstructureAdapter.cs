using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class SubstructureAdapter
    {
        public static Substructure Convert(SubstructureDto substructureDto)
        {
            var substructure = new Substructure
            {
                Id = substructureDto.Id,
                ProjectId = substructureDto.ProjectId,
                Name = substructureDto.Name,
                DryWeight = substructureDto.DryWeight,
                Maturity = substructureDto.Maturity
            };
            if (substructureDto.CostProfile != null) {
                substructure.CostProfile = Convert(substructureDto.CostProfile, substructure);
            }

            if (substructureDto.SubstructureCessasionCostProfileDto != null) {
                substructure.SubstructureCessasionCostProfile = Convert(substructureDto.SubstructureCessasionCostProfileDto, substructure);
            }
            
            return substructure;
        }

        private static SubstructureCostProfile? Convert(SubstructureCostProfileDto? costprofile, Substructure substructure)
        {
            if (costprofile == null)
            {
                return null;
            }
            var substructureCostProfile = new SubstructureCostProfile
            {
                Substructure = substructure,
                Id = costprofile.Id,
                EPAVersion = costprofile.EPAVersion,
                Currency = costprofile.Currency,
                StartYear = costprofile.StartYear,
                Values = costprofile.Values
            };
            return substructureCostProfile;
        }

           private static SubstructureCessasionCostProfile? Convert(SubstructureCessasionCostProfileDto? substructureCessasionCostProfileDto, Substructure substructure)
        {
            if (substructureCessasionCostProfileDto == null)
            {
                return null;
            }
            SubstructureCessasionCostProfile substructureCessasionCostProfile = new SubstructureCessasionCostProfile
            {
                Substructure = substructure,
                Id = substructureCessasionCostProfileDto.Id,
                EPAVersion = substructureCessasionCostProfileDto.EPAVersion,
                Currency = substructureCessasionCostProfileDto.Currency,
                StartYear = substructureCessasionCostProfileDto.StartYear,
                Values = substructureCessasionCostProfileDto.Values
            };
            return substructureCessasionCostProfile;
        }
    }
}
