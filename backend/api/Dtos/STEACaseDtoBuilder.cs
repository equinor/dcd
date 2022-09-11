using api.Dtos;

namespace api.Adapters;

public static class STEACaseDtoBuilder
{
    public static STEACaseDto Build(CaseDto c, ProjectDto p)
    {
        var sTEACaseDto = new STEACaseDto();
        sTEACaseDto.Name = c.Name;
        AddCapex(p, sTEACaseDto, c);
        AddExploration(p, sTEACaseDto, c);
        AddProductionSalesAndVolumes(p, sTEACaseDto, c);
        int[] startYearsCase =
        {
            sTEACaseDto.Exploration.StartYear, sTEACaseDto.ProductionAndSalesVolumes.StartYear,
            sTEACaseDto.Capex.StartYear
        };
        Array.Sort(startYearsCase);
        sTEACaseDto.StartYear = Array.Find(startYearsCase, e => e > 0);

        return sTEACaseDto;
    }

    private static void AddCapex(ProjectDto p, STEACaseDto sTEACaseDto, CaseDto c)
    {
        sTEACaseDto.Capex = new CapexDto();
        sTEACaseDto.Capex.Drilling = new WellProjectCostProfileDto();
        var dg4Year = c.DG4Date.Year;
        if (c.WellProjectLink != Guid.Empty)
        {
            var wellProjectCostProfileDto = p.WellProjects!.First(l => l.Id == c.WellProjectLink).CostProfile;
            if (wellProjectCostProfileDto != null)
            {
                sTEACaseDto.Capex.Drilling = wellProjectCostProfileDto;
                sTEACaseDto.Capex.Drilling.StartYear += dg4Year;
                sTEACaseDto.Capex.AddValues(sTEACaseDto.Capex.Drilling);
            }
        }

        sTEACaseDto.Capex.OffshoreFacilities = new OffshoreFacilitiesCostProfileDto();
        sTEACaseDto.Capex.CessationOffshoreFacilities = new CessationOffshoreFacilities();
        if (c.SubstructureLink != Guid.Empty)
        {
            var substructureCostProfileDto = p.Substructures!.First(l => l.Id == c.SubstructureLink);

            var substructureCostProfile = substructureCostProfileDto.CostProfile;
            if (substructureCostProfile != null)
            {
                substructureCostProfile.StartYear += dg4Year;
                sTEACaseDto.Capex.OffshoreFacilities.AddValues(substructureCostProfile);
            }

            var substructureCessationCostProfileDto = substructureCostProfileDto.CessationCostProfile;
            if (substructureCessationCostProfileDto != null)
            {
                substructureCessationCostProfileDto.StartYear += dg4Year;
                sTEACaseDto.Capex.CessationOffshoreFacilities.AddValues(substructureCessationCostProfileDto);
            }
        }

        if (c.SurfLink != Guid.Empty)
        {
            var surf = p.Surfs!.First(l => l.Id == c.SurfLink);
            var surfCostProfileDto = surf.CostProfile;
            if (surfCostProfileDto != null)
            {
                surfCostProfileDto.StartYear += dg4Year;
                sTEACaseDto.Capex.OffshoreFacilities.AddValues(surfCostProfileDto);
            }

            var surfCessationCostProfileDto = surf.CessationCostProfile;
            if (surfCessationCostProfileDto != null)
            {
                surfCessationCostProfileDto.StartYear += dg4Year;
                sTEACaseDto.Capex.CessationOffshoreFacilities.AddValues(surfCessationCostProfileDto);
            }
        }

        if (c.TopsideLink != Guid.Empty)
        {
            var topsideDto = p.Topsides!.First(l => l.Id == c.TopsideLink);
            var topsideCostProfileDto = topsideDto.CostProfile;
            if (topsideCostProfileDto != null)
            {
                topsideCostProfileDto.StartYear += dg4Year;
                sTEACaseDto.Capex.OffshoreFacilities.AddValues(topsideCostProfileDto);
            }

            var topsideCessationCostProfileDto = topsideDto.CessationCostProfile;
            if (topsideCessationCostProfileDto != null)
            {
                topsideCessationCostProfileDto.StartYear += dg4Year;
                sTEACaseDto.Capex.CessationOffshoreFacilities.AddValues(topsideCessationCostProfileDto);
            }
        }

        if (c.TransportLink != Guid.Empty)
        {
            var transportDto = p.Transports!.First(l => l.Id == c.TransportLink);
            var transportCostProfileDto = transportDto.CostProfile;
            if (transportCostProfileDto != null)
            {
                transportCostProfileDto.StartYear += dg4Year;
                sTEACaseDto.Capex.OffshoreFacilities.AddValues(transportCostProfileDto);
            }

            var transportCessationCostProfileDto = transportDto.CessationCostProfile;
            if (transportCessationCostProfileDto != null)
            {
                transportCessationCostProfileDto.StartYear += dg4Year;
                sTEACaseDto.Capex.CessationOffshoreFacilities.AddValues(transportCessationCostProfileDto);
            }
        }

        sTEACaseDto.Capex.AddValues(sTEACaseDto.Capex.OffshoreFacilities);
        sTEACaseDto.Capex.AddValues(sTEACaseDto.Capex.CessationOffshoreFacilities);
    }

    private static void AddProductionSalesAndVolumes(ProjectDto p, STEACaseDto sTEACaseDto, CaseDto c)
    {
        sTEACaseDto.ProductionAndSalesVolumes = new ProductionAndSalesVolumesDto();
        sTEACaseDto.ProductionAndSalesVolumes.TotalAndAnnualOil = new ProductionProfileOilDto();
        sTEACaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = new NetSalesGasDto();
        sTEACaseDto.ProductionAndSalesVolumes.Co2Emissions = new Co2EmissionsDto();
        var dg4Year = c.DG4Date.Year;
        if (c.DrainageStrategyLink != Guid.Empty)
        {
            var drainageStrategyDto = p.DrainageStrategies!.First(d => d.Id == c.DrainageStrategyLink);
            var startYearsProductionSalesAndVolumes = new List<int>();
            if (drainageStrategyDto.ProductionProfileOil != null)
            {
                sTEACaseDto.ProductionAndSalesVolumes.TotalAndAnnualOil = drainageStrategyDto.ProductionProfileOil;
                startYearsProductionSalesAndVolumes.Add(drainageStrategyDto.ProductionProfileOil.StartYear += dg4Year);
            }

            if (drainageStrategyDto.NetSalesGas != null)
            {
                sTEACaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = drainageStrategyDto.NetSalesGas;
                startYearsProductionSalesAndVolumes.Add(drainageStrategyDto.NetSalesGas.StartYear += dg4Year);
            }

            if (drainageStrategyDto.Co2Emissions != null)
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
        sTEACaseDto.Exploration = new ExplorationCostProfileDto();
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
