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
            substructure.CostProfile = Convert(substructureDto.CostProfile, substructure);
            return substructure;
        }

        public static void ConvertExisting(Substructure existing, SubstructureDto substructureDto)
        {
            existing.Id = substructureDto.Id;
            existing.ProjectId = substructureDto.ProjectId;
            existing.Name = substructureDto.Name;
            existing.DryWeight = substructureDto.DryWeight;
            existing.Maturity = substructureDto.Maturity;

            existing.CostProfile = Convert(substructureDto.CostProfile, existing);
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
    }
}
