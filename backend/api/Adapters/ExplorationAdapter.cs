using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class ExplorationAdapter
    {

        public static Exploration Convert(ExplorationDto explorationDto)
        {
            var exploration = new Exploration
            {
                Id = explorationDto.Id,
                ProjectId = explorationDto.ProjectId,
                Name = explorationDto.Name,
                RigMobDemob = explorationDto.RigMobDemob,
                Currency = explorationDto.Currency,
            };
            exploration.CostProfile = Convert(explorationDto.CostProfile, exploration);
            exploration.GAndGAdminCost = Convert(explorationDto.GAndGAdminCost, exploration);
            exploration.SeismicAcquisitionAndProcessing = Convert(explorationDto.SeismicAcquisitionAndProcessing, exploration);
            exploration.CountryOfficeCost = Convert(explorationDto.CountryOfficeCost, exploration);
            return exploration;
        }

        public static void ConvertExisting(Exploration existing, ExplorationDto explorationDto)
        {
            existing.Id = explorationDto.Id;
            existing.ProjectId = explorationDto.ProjectId;
            existing.Name = explorationDto.Name;
            existing.RigMobDemob = explorationDto.RigMobDemob;
            existing.Currency = explorationDto.Currency;
            existing.CostProfile = Convert(explorationDto.CostProfile, existing);
            existing.GAndGAdminCost = Convert(explorationDto.GAndGAdminCost, existing);
            existing.SeismicAcquisitionAndProcessing = Convert(explorationDto.SeismicAcquisitionAndProcessing, existing);
            existing.CountryOfficeCost = Convert(explorationDto.CountryOfficeCost, existing);
        }

        private static ExplorationCostProfile Convert(ExplorationCostProfileDto? costProfileDto, Exploration exploration)
        {
            if (costProfileDto == null)
            {
                return null!;
            }
            return new ExplorationCostProfile
            {
                Id = costProfileDto.Id,
                Currency = costProfileDto.Currency,
                EPAVersion = costProfileDto.EPAVersion,
                Exploration = exploration,
                StartYear = costProfileDto.StartYear,
                Values = costProfileDto.Values,
                Override = costProfileDto.Override,
            };
        }

        private static GAndGAdminCost Convert(GAndGAdminCostDto? gAndGAdminCostDto, Exploration exploration)
        {
            if (gAndGAdminCostDto == null)
            {
                return null!;
            }
            return new GAndGAdminCost
            {
                Id = gAndGAdminCostDto.Id,
                Currency = gAndGAdminCostDto.Currency,
                EPAVersion = gAndGAdminCostDto.EPAVersion,
                Exploration = exploration,
                Values = gAndGAdminCostDto.Values,
                StartYear = gAndGAdminCostDto.StartYear
            };
        }

        private static SeismicAcquisitionAndProcessing Convert(SeismicAcquisitionAndProcessingDto? seismicAcquisitionAndProcessing, Exploration exploration)
        {
            if (seismicAcquisitionAndProcessing == null)
            {
                return null!;
            }
            return new SeismicAcquisitionAndProcessing
            {
                Id = seismicAcquisitionAndProcessing.Id,
                Currency = seismicAcquisitionAndProcessing.Currency,
                EPAVersion = seismicAcquisitionAndProcessing.EPAVersion,
                Exploration = exploration,
                Values = seismicAcquisitionAndProcessing.Values,
                StartYear = seismicAcquisitionAndProcessing.StartYear
            };
        }

        private static CountryOfficeCost Convert(CountryOfficeCostDto? countryOfficeCost, Exploration exploration)
        {
            if (countryOfficeCost == null)
            {
                return null!;
            }
            return new CountryOfficeCost
            {
                Id = countryOfficeCost.Id,
                Currency = countryOfficeCost.Currency,
                EPAVersion = countryOfficeCost.EPAVersion,
                Exploration = exploration,
                Values = countryOfficeCost.Values,
                StartYear = countryOfficeCost.StartYear
            };
        }
    }
}
