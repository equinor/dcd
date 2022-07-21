
using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class CaseDtoAdapter
    {
        public static CaseDto Convert(Case case_, ProjectDto projectDto)
        {
            var caseDto = new CaseDto
            {
                Id = case_.Id,
                ProjectId = case_.ProjectId,
                Name = case_.Name,
                Description = case_.Description,
                ReferenceCase = case_.ReferenceCase,
                DG0Date = case_.DG0Date,
                DG1Date = case_.DG1Date,
                DG2Date = case_.DG2Date,
                DG3Date = case_.DG3Date,
                DG4Date = case_.DG4Date,
                CreateTime = case_.CreateTime,
                ModifyTime = case_.ModifyTime,
                DrainageStrategyLink = case_.DrainageStrategyLink,
                WellProjectLink = case_.WellProjectLink,
                SurfLink = case_.SurfLink,
                SubstructureLink = case_.SubstructureLink,
                TopsideLink = case_.TopsideLink,
                TransportLink = case_.TransportLink,
                ExplorationLink = case_.ExplorationLink,
                ArtificialLift = case_.ArtificialLift,
                ProductionStrategyOverview = case_.ProductionStrategyOverview,
                ProducerCount = case_.ProducerCount,
                GasInjectorCount = case_.GasInjectorCount,
                WaterInjectorCount = case_.WaterInjectorCount,
                FacilitiesAvailability = case_.FacilitiesAvailability,
            };

            CalculateCessationCost(case_, caseDto, projectDto);

            return caseDto;
        }

        private static void CalculateCessationCost(Case caseItem, CaseDto caseDto, ProjectDto projectDto)
        {
            var cessationWells = new CessationCost();
            var cessationOffshoreFacilities = new CessationCost();
            // Find last year of production
            // Drainage strategy -> Production profile oil, last year
            // var lastYear = projectDto.DrainageStrategies?.FirstOrDefault(ds => ds.Id == caseItem.DrainageStrategyLink)?.ProductionProfileOil;
            var productionProfileOil = projectDto.DrainageStrategies?.FirstOrDefault(ds => ds.Id == caseItem.DrainageStrategyLink)?.ProductionProfileOil;
            if (productionProfileOil != null)
            {
                var lastYear = productionProfileOil.StartYear + productionProfileOil.Values.Length - 1;
                // Plugging and abandonment (Well project) * Sum of drilled wells.
                // Divide cost on last year of production: 50%
                // And last year + 1 : 50%
                var drilledWells = caseItem.Wells?.Count;
                var pluggingAndAbandonment = projectDto.WellProjects?.FirstOrDefault(wp => wp.Id == caseItem.WellProjectLink)?.PluggingAndAbandonment;
                if (drilledWells != null && pluggingAndAbandonment != null)
                {
                    var totalCost = (int)drilledWells * (double)pluggingAndAbandonment;
                    cessationWells.StartYear = lastYear;
                    var cessationWellsValues = new double[2] { totalCost / 2, totalCost / 2 };
                    cessationWells.Values = cessationWellsValues;
                }
                var surfCessationCost = projectDto.Surfs?.FirstOrDefault(s => s.Id == caseItem.SurfLink)?.CessationCost;
                if (surfCessationCost != null)
                {
                    cessationOffshoreFacilities.StartYear = lastYear + 1;
                    var cessationOffshoreFacilitiesValues = new double[2] { (double)surfCessationCost / 2, (double)surfCessationCost / 2 };
                    cessationOffshoreFacilities.Values = cessationOffshoreFacilitiesValues;
                }
                var cessation = MergeCessationCosts(cessationWells, cessationOffshoreFacilities);

                caseDto.CessationCost = cessation;
            }
        }

        private static CessationCost? MergeCessationCosts(CessationCost c1, CessationCost c2)
        {
            var c1Year = c1.StartYear;
            var c2Year = c2.StartYear;
            var c1Values = c1.Values;
            var c2Values = c2.Values;
            if (c1Values.Length == 0)
            {
                if (c2.Values.Length == 0)
                {
                    return null;
                }
                return c2;
            }
            if (c2Values.Length == 0)
            {
                return c1;
            }

            var values = new List<double>();
            if (c1Year < c2Year)
            {
                values.AddRange(c1Values);
                var offset = c2Year - c1Year;
                values.InsertRange(offset, c2Values);
            }
            else
            {
                values.AddRange(c2Values);
                var offset = c1Year - c2Year;
                values.InsertRange(offset, c1Values);
            }

            var cessation = new CessationCost();
            cessation.StartYear = Math.Min(c1Year, c2Year);
            cessation.Values = values.ToArray();
            return cessation;
        }
    }
}
