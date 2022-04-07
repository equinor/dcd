using api.Dtos;

namespace api.Adapters
{
    public static class STEACaseDtoBuilder
    {
        public static STEACaseDto Build(CaseDto c, ProjectDto p)
        {
            var sTEACaseDto = new STEACaseDto();
            sTEACaseDto.Name = c.Name;
            sTEACaseDto.IsDG4YearSet = c.DG4Date != DateTimeOffset.MinValue ? true : false;
            AddCapex(p, sTEACaseDto, c);
            AddExploration(p, sTEACaseDto, c);
            AddProductionSalesAndVolumes(p, sTEACaseDto, c);
            List<int> startYears = new List<int>();
            if (sTEACaseDto.Capex != null)
            {
                startYears.Add(sTEACaseDto.Capex.StartYear);
            }
            if (sTEACaseDto.Exploration != null)
            {
                startYears.Add(sTEACaseDto.Exploration.StartYear);
            }
            if (sTEACaseDto.ProductionAndSalesVolumes != null)
            {
                startYears.Add(sTEACaseDto.ProductionAndSalesVolumes.StartYear);
            }
            if (startYears.Count() != 0)
            {
                sTEACaseDto.StartYear = startYears.Min();
            }
            return sTEACaseDto;
        }

        private static void AddCapex(ProjectDto p, STEACaseDto sTEACaseDto, CaseDto c)
        {
            if (c.WellProjectLink == Guid.Empty && c.SubstructureLink == Guid.Empty &&
                c.SurfLink == Guid.Empty && c.TopsideLink == Guid.Empty && c.TransportLink == Guid.Empty)
            {
                return;
            }
            sTEACaseDto.Capex = new CapexDto();
            int dg4Year = c.DG4Date.Year;
            if (c.WellProjectLink != Guid.Empty)
            {
                WellProjectCostProfileDto? wellProjectCostProfileDto = p.WellProjects!.First(l => l.Id == c.WellProjectLink).CostProfile;
                if (wellProjectCostProfileDto != null)
                {
                    sTEACaseDto.Capex.Drilling = wellProjectCostProfileDto;
                    sTEACaseDto.Capex.Drilling.StartYear += dg4Year;
                    sTEACaseDto.Capex.AddValues(sTEACaseDto.Capex.Drilling);
                }
            }

            if (c.SubstructureLink != Guid.Empty ||
                c.SurfLink != Guid.Empty || c.TopsideLink != Guid.Empty || c.TransportLink != Guid.Empty)
            {
                sTEACaseDto.Capex.OffshoreFacilities = new OffshoreFacilitiesCostProfileDto();
                if (c.SubstructureLink != Guid.Empty)
                {
                    SubstructureCostProfileDto? substructureCostProfileDto = p.Substructures!.First(l => l.Id == c.SubstructureLink).CostProfile;
                    if (substructureCostProfileDto != null)
                    {
                        substructureCostProfileDto.StartYear += dg4Year;
                        sTEACaseDto.Capex.OffshoreFacilities.AddValues(substructureCostProfileDto);
                    }
                }
                if (c.SurfLink != Guid.Empty)
                {
                    SurfCostProfileDto? surfCostProfileDto = p.Surfs!.First(l => l.Id == c.SurfLink).CostProfile;
                    if (surfCostProfileDto != null)
                    {
                        surfCostProfileDto.StartYear += dg4Year;
                        sTEACaseDto.Capex.OffshoreFacilities.AddValues(surfCostProfileDto);
                    }

                }
                if (c.TopsideLink != Guid.Empty)
                {
                    TopsideCostProfileDto? topsideCostProfileDto = p.Topsides!.First(l => l.Id == c.TopsideLink).CostProfile;
                    if (topsideCostProfileDto != null)
                    {
                        topsideCostProfileDto.StartYear += dg4Year;
                        sTEACaseDto.Capex.OffshoreFacilities.AddValues(topsideCostProfileDto);
                    }

                }
                if (c.TransportLink != Guid.Empty)
                {
                    TransportCostProfileDto? transportCostProfileDto = p.Transports!.First(l => l.Id == c.TransportLink).CostProfile;
                    if (transportCostProfileDto != null)
                    {
                        transportCostProfileDto.StartYear += dg4Year;
                        sTEACaseDto.Capex.OffshoreFacilities.AddValues(transportCostProfileDto);
                    }
                }
                sTEACaseDto.Capex.AddValues(sTEACaseDto.Capex.OffshoreFacilities);
            }
        }

        private static void AddProductionSalesAndVolumes(ProjectDto p, STEACaseDto sTEACaseDto, CaseDto c)
        {
            int dg4Year = c.DG4Date.Year;
            if (c.DrainageStrategyLink != Guid.Empty)
            {
                sTEACaseDto.ProductionAndSalesVolumes = new ProductionAndSalesVolumesDto();
                DrainageStrategyDto drainageStrategyDto = p.DrainageStrategies!.First(d => d.Id == c.DrainageStrategyLink);
                List<int> startYearsProductionSalesAndVolumes = new List<int>();
                if (drainageStrategyDto.ProductionProfileOil != null && drainageStrategyDto.ProductionProfileOil.Values != null)
                {
                    sTEACaseDto.ProductionAndSalesVolumes.TotalAndAnnualOil = drainageStrategyDto.ProductionProfileOil;
                    startYearsProductionSalesAndVolumes.Add(drainageStrategyDto.ProductionProfileOil.StartYear += dg4Year);
                }

                if (drainageStrategyDto.NetSalesGas != null && drainageStrategyDto.NetSalesGas.Values != null)
                {
                    sTEACaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = drainageStrategyDto.NetSalesGas;
                    startYearsProductionSalesAndVolumes.Add(drainageStrategyDto.NetSalesGas.StartYear += dg4Year);
                }

                if (drainageStrategyDto.Co2Emissions != null && drainageStrategyDto.Co2Emissions.Values != null)
                {
                    sTEACaseDto.ProductionAndSalesVolumes.Co2Emissions = drainageStrategyDto.Co2Emissions;
                    startYearsProductionSalesAndVolumes.Add(drainageStrategyDto.Co2Emissions.StartYear += dg4Year);
                }
                if (startYearsProductionSalesAndVolumes.Count() > 0)
                {
                    sTEACaseDto.ProductionAndSalesVolumes.StartYear = startYearsProductionSalesAndVolumes.Min();
                }
            }
        }

        private static void AddExploration(ProjectDto p, STEACaseDto sTEACaseDto, CaseDto caseDto)
        {
            if (caseDto.ExplorationLink != Guid.Empty)
            {
                var exploration = p.Explorations!.First(e => e.Id == caseDto.ExplorationLink);
                if (exploration.CostProfile != null)
                {
                    sTEACaseDto.Exploration = exploration.CostProfile;
                    sTEACaseDto.Exploration.StartYear += caseDto.DG4Date.Year;
                }
            }
        }
    }
}
