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
                SharepointFileId = case_.SharepointFileId,
                SharepointFileName = case_.SharepointFileName,
            };

            CalculateCessationCost(case_, caseDto, projectDto);

            return caseDto;
        }

        private static void CalculateCessationCost(Case caseItem, CaseDto caseDto, ProjectDto projectDto)
        {
            var cessationWells = new CessationCostDto();
            var cessationOffshoreFacilities = new CessationCostDto();
            var productionProfileOil = projectDto.DrainageStrategies?.FirstOrDefault(ds => ds.Id == caseItem.DrainageStrategyLink)?.ProductionProfileOil;
            if (productionProfileOil != null)
            {
                var lastYear = productionProfileOil.StartYear + productionProfileOil.Values.Length - 1;
                var linkedWellsDrillingSchedule = projectDto.WellProjects?.FirstOrDefault(wp => wp.Id == caseItem.WellProjectLink)?.WellProjectWells?.Where(wpw => wpw.Count > 0).Select(wpw => wpw.DrillingSchedule);
                var pluggingAndAbandonment = projectDto.WellProjects?.FirstOrDefault(wp => wp.Id == caseItem.WellProjectLink)?.PluggingAndAbandonment;
                if (linkedWellsDrillingSchedule != null && pluggingAndAbandonment != null)
                {
                    var linkedWells = linkedWellsDrillingSchedule.Where(lwd => lwd != null && lwd.Values != null).SelectMany(lwd => lwd.Values);

                    var drilledWells = linkedWells.Sum();
                    var totalCost = drilledWells * (double)pluggingAndAbandonment;
                    cessationWells.StartYear = lastYear;
                    var cessationWellsValues = new double[] { totalCost / 2, totalCost / 2 };
                    cessationWells.Values = cessationWellsValues;
                }
                var surfCessationCost = projectDto.Surfs?.FirstOrDefault(s => s.Id == caseItem.SurfLink)?.CessationCost;
                if (surfCessationCost != null)
                {
                    cessationOffshoreFacilities.StartYear = lastYear + 1;
                    var cessationOffshoreFacilitiesValues = new double[] { (double)surfCessationCost / 2, (double)surfCessationCost / 2 };
                    cessationOffshoreFacilities.Values = cessationOffshoreFacilitiesValues;
                }
                var cessation = MergeCessationCosts(cessationWells, cessationOffshoreFacilities);

                caseDto.CessationCost = cessation;
            }
        }

        private static CessationCostDto? MergeCessationCosts(CessationCostDto t1, CessationCostDto t2)
        {
            var t1Year = t1.StartYear;
            var t2Year = t2.StartYear;
            var t1Values = t1.Values;
            var t2Values = t2.Values;
            if (t1Values == null || t1Values.Length == 0)
            {
                if (t2Values == null || t2Values.Length == 0)
                {
                    return null;
                }
                return t2;
            }
            if (t2Values == null || t2Values.Length == 0)
            {
                return t1;
            }

            var offset = t1Year < t2Year ? t2Year - t1Year : t1Year - t2Year;

            List<double> values;
            if (t1Year < t2Year)
            {
                values = MergeTimeSeries(t1Values.ToList(), t2Values.ToList(), offset);
            }
            else
            {
                values = MergeTimeSeries(t2Values.ToList(), t1Values.ToList(), offset);
            }

            var timeSeries = new CessationCostDto();
            timeSeries.StartYear = Math.Min(t1Year, t2Year);
            timeSeries.Values = values.ToArray();
            return timeSeries;
        }

        private static List<double> MergeTimeSeries(List<double> t1, List<double> t2, int offset)
        {
            var doubleList = new List<double>();
            if (offset > t1.Count)
            {
                doubleList.AddRange(t1);
                var zeros = offset - t1.Count;
                var zeroList = Enumerable.Repeat(0.0, zeros);
                doubleList.AddRange(zeroList);
                doubleList.AddRange(t2);
                return doubleList;
            }
            doubleList.AddRange(t1.Take(offset));
            if (t1.Count - offset == t2.Count)
            {
                doubleList.AddRange(t1.TakeLast(t1.Count - offset).Zip(t2, (x, y) => x + y));
            }
            else if (t1.Count - offset > t2.Count)
            {
                doubleList.AddRange(t1.TakeLast(t1.Count - offset).Zip(t2, (x, y) => x + y));
                var remaining = t1.Count - offset - t2.Count;
                doubleList.AddRange(t1.TakeLast(remaining));
            }
            else
            {
                doubleList.AddRange(t1.TakeLast(t1.Count - offset).Zip(t2, (x, y) => x + y));
                var remaining = t2.Count - (t1.Count - offset);
                doubleList.AddRange(t2.TakeLast(remaining));
            }
            return doubleList;
        }
    }
}
