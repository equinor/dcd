using api.Dtos;

namespace api.Adapters
{
    public static class STEACaseDtoBuilder
    {
        public static STEACaseDto Build(CaseDto c, ProjectDto p)
        {
            var sTEACaseDto = new STEACaseDto();
            sTEACaseDto.Name = c.Name;

            AddCapex(p, sTEACaseDto, c);
            AddExploration(p, sTEACaseDto, c);
            AddProductionSalesAndVolumes(p, sTEACaseDto, c);
            int[] startYearsCase = new int[] { sTEACaseDto.Exploration.StartYear, sTEACaseDto.ProductionAndSalesVolumes.StartYear,
                        sTEACaseDto.Capex.StartYear };
            Array.Sort(startYearsCase);
            sTEACaseDto.StartYear = Array.Find(startYearsCase, e => e > 0);

            return sTEACaseDto;
        }

        private static void AddCapex(ProjectDto p, STEACaseDto sTEACaseDto, CaseDto c)
        {
            sTEACaseDto.Capex = new CapexDto();
            if (c.WellProjectLink != Guid.Empty)
            {
                sTEACaseDto.Capex.Drilling = p.WellProjects.FirstOrDefault(l => l.Id == c.WellProjectLink)!.CostProfile;
                sTEACaseDto.Capex.AddValues(sTEACaseDto.Capex.Drilling);
            }
            sTEACaseDto.Capex.OffshoreFacilities = new OffshoreFacilitiesCostProfileDto();
            if (c.SubstructureLink != Guid.Empty)
            {
                sTEACaseDto.Capex.OffshoreFacilities.AddValues(p.Substructures.FirstOrDefault(l => l.Id == c.SubstructureLink)!.CostProfile);
            }
            if (c.SurfLink != Guid.Empty)
            {
                sTEACaseDto.Capex.OffshoreFacilities.AddValues(p.Surfs.FirstOrDefault(l => l.Id == c.SurfLink)!.CostProfile);
            }
            if (c.TopsideLink != Guid.Empty)
            {
                sTEACaseDto.Capex.OffshoreFacilities.AddValues(p.Topsides.FirstOrDefault(l => l.Id == c.TopsideLink)!.CostProfile);
            }
            if (c.TransportLink != Guid.Empty)
            {
                sTEACaseDto.Capex.OffshoreFacilities.AddValues(p.Transports.FirstOrDefault(l => l.Id == c.TransportLink)!.CostProfile);
            }

            sTEACaseDto.Capex.AddValues(sTEACaseDto.Capex.OffshoreFacilities);
        }

        private static void AddProductionSalesAndVolumes(ProjectDto p, STEACaseDto sTEACaseDto, CaseDto c)
        {
            sTEACaseDto.ProductionAndSalesVolumes = new ProductionAndSalesVolumesDto();
            if (c.DrainageStrategyLink != Guid.Empty)
            {
                DrainageStrategyDto drainageStrategyDto = p.DrainageStrategies.FirstOrDefault(d => d.Id == c.DrainageStrategyLink)!;
                sTEACaseDto.ProductionAndSalesVolumes.TotalAndAnnualOil = drainageStrategyDto.ProductionProfileOil == null
                ? new ProductionProfileOilDto() : drainageStrategyDto.ProductionProfileOil;
                sTEACaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = drainageStrategyDto.NetSalesGas == null
                ? new NetSalesGasDto() : drainageStrategyDto.NetSalesGas;
                sTEACaseDto.ProductionAndSalesVolumes.Co2Emissions = drainageStrategyDto.Co2Emissions == null
                ? new Co2EmissionsDto() : drainageStrategyDto.Co2Emissions;

                int[] startYearsProductionSalesAndVolumes = new int[] {
                    sTEACaseDto.ProductionAndSalesVolumes.Co2Emissions.StartYear,
                    sTEACaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas.StartYear,
                    sTEACaseDto.ProductionAndSalesVolumes.TotalAndAnnualOil.StartYear
                };
                Array.Sort(startYearsProductionSalesAndVolumes);
                sTEACaseDto.ProductionAndSalesVolumes.StartYear = Array.Find(startYearsProductionSalesAndVolumes, e => e > 0);
            }
        }
        private static void AddExploration(ProjectDto p, STEACaseDto sTEACaseDto, CaseDto caseDto)
        {
            sTEACaseDto.Exploration = new ExplorationCostProfileDto();
            if (caseDto.ExplorationLink != Guid.Empty)
            {
                sTEACaseDto.Exploration = p.Explorations.FirstOrDefault(e => e.Id == caseDto.ExplorationLink)!.CostProfile;
            }
        }
    }
}
