using api.Dtos;
using api.Models;

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
        int[] startYearsCase = new int[] { sTEACaseDto.Exploration.StartYear, sTEACaseDto.ProductionAndSalesVolumes.StartYear,
            sTEACaseDto.Capex.StartYear };
        Array.Sort(startYearsCase);
        sTEACaseDto.StartYear = Array.Find(startYearsCase, e => e > 0);

        return sTEACaseDto;
    }

    private static void AddCapex(ProjectDto p, STEACaseDto sTEACaseDto, CaseDto c)
    {
        sTEACaseDto.Capex = new CapexDto
        {
            Drilling = new TimeSeriesCostDto()
        };
        int dg4Year = c.DG4Date.Year;
        if (c.WellProjectLink != Guid.Empty)
        {
            var wellProject = p.WellProjects!.First(l => l.Id == c.WellProjectLink);
            if (wellProject != null)
            {
                var costProfileDtos = new List<TimeSeriesCostDto>();
                if (wellProject.OilProducerCostProfile?.Values.Length > 0)
                {
                    costProfileDtos.Add(wellProject.OilProducerCostProfile);
                }
                if (wellProject.GasProducerCostProfile?.Values.Length > 0)
                {
                    costProfileDtos.Add(wellProject.GasProducerCostProfile);
                }
                if (wellProject.WaterInjectorCostProfile?.Values.Length > 0)
                {
                    costProfileDtos.Add(wellProject.WaterInjectorCostProfile);
                }
                if (wellProject.GasInjectorCostProfile?.Values.Length > 0)
                {
                    costProfileDtos.Add(wellProject.GasInjectorCostProfile);
                }
                var costProfile = TimeSeriesCostDto.MergeCostProfilesList(costProfileDtos);

                sTEACaseDto.Capex.Drilling = costProfile;
                sTEACaseDto.Capex.Drilling.StartYear += dg4Year;
                sTEACaseDto.Capex.AddValues(sTEACaseDto.Capex.Drilling);
            }
        }
        sTEACaseDto.Capex.OffshoreFacilities = new OffshoreFacilitiesCostProfileDto();
        sTEACaseDto.Capex.CessationOffshoreFacilities = new CessationOffshoreFacilities();
        if (c.SubstructureLink != Guid.Empty)
        {
            SubstructureDto? substructureCostProfileDto = p.Substructures!.First(l => l.Id == c.SubstructureLink);

            SubstructureCostProfileDto? substructureCostProfile = substructureCostProfileDto.CostProfile;
            if (substructureCostProfile != null)
            {
                substructureCostProfile.StartYear += dg4Year;
                sTEACaseDto.Capex.OffshoreFacilities.AddValues(substructureCostProfile);
            }

            SubstructureCessationCostProfileDto? substructureCessationCostProfileDto = substructureCostProfileDto.CessationCostProfile;
            if (substructureCessationCostProfileDto != null)
            {
                substructureCessationCostProfileDto.StartYear += dg4Year;
                sTEACaseDto.Capex.CessationOffshoreFacilities.AddValues(substructureCessationCostProfileDto);
            }
        }
        if (c.SurfLink != Guid.Empty)
        {
            SurfDto? surf = p.Surfs!.First(l => l.Id == c.SurfLink);
            SurfCostProfileDto? surfCostProfileDto = surf.CostProfile;
            if (surfCostProfileDto != null)
            {
                surfCostProfileDto.StartYear += dg4Year;
                sTEACaseDto.Capex.OffshoreFacilities.AddValues(surfCostProfileDto);
            }

            SurfCessationCostProfileDto? surfCessationCostProfileDto = surf.CessationCostProfile;
            if (surfCessationCostProfileDto != null)
            {
                surfCessationCostProfileDto.StartYear += dg4Year;
                sTEACaseDto.Capex.CessationOffshoreFacilities.AddValues(surfCessationCostProfileDto);
            }
        }
        if (c.TopsideLink != Guid.Empty)
        {
            TopsideDto? topsideDto = p.Topsides!.First(l => l.Id == c.TopsideLink);
            TopsideCostProfileDto? topsideCostProfileDto = topsideDto.CostProfile;
            if (topsideCostProfileDto != null)
            {
                topsideCostProfileDto.StartYear += dg4Year;
                sTEACaseDto.Capex.OffshoreFacilities.AddValues(topsideCostProfileDto);
            }

            TopsideCessationCostProfileDto? topsideCessationCostProfileDto = topsideDto.CessationCostProfile;
            if (topsideCessationCostProfileDto != null)
            {
                topsideCessationCostProfileDto.StartYear += dg4Year;
                sTEACaseDto.Capex.CessationOffshoreFacilities.AddValues(topsideCessationCostProfileDto);
            }
        }
        if (c.TransportLink != Guid.Empty)
        {
            TransportDto? transportDto = p.Transports!.First(l => l.Id == c.TransportLink);
            TransportCostProfileDto? transportCostProfileDto = transportDto.CostProfile;
            if (transportCostProfileDto != null)
            {
                transportCostProfileDto.StartYear += dg4Year;
                sTEACaseDto.Capex.OffshoreFacilities.AddValues(transportCostProfileDto);
            }

            TransportCessationCostProfileDto? transportCessationCostProfileDto = transportDto.CessationCostProfile;
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
        sTEACaseDto.ProductionAndSalesVolumes = new ProductionAndSalesVolumesDto
        {
            TotalAndAnnualOil = new ProductionProfileOilDto(),
            TotalAndAnnualSalesGas = new NetSalesGasDto(),
            Co2Emissions = new Co2EmissionsDto()
        };
        int dg4Year = c.DG4Date.Year;
        if (c.DrainageStrategyLink != Guid.Empty)
        {
            DrainageStrategyDto drainageStrategyDto = p.DrainageStrategies!.First(d => d.Id == c.DrainageStrategyLink);
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
            if (startYearsProductionSalesAndVolumes.Count > 0)
            {
                sTEACaseDto.ProductionAndSalesVolumes.StartYear = startYearsProductionSalesAndVolumes.Min();
            }
        }
    }

    private static void AddExploration(ProjectDto p, STEACaseDto sTEACaseDto, CaseDto caseDto)
    {
        sTEACaseDto.Exploration = new TimeSeriesCostDto();
        if (caseDto.ExplorationLink != Guid.Empty)
        {
            var exploration = p.Explorations!.First(e => e.Id == caseDto.ExplorationLink);
            if (exploration != null)
            {
                var costProfileDtos = new List<TimeSeriesCostDto>();
                if (exploration.ExplorationWellCostProfile?.Values.Length > 0)
                {
                    costProfileDtos.Add(exploration.ExplorationWellCostProfile);
                }
                if (exploration.AppraisalWellCostProfile?.Values.Length > 0)
                {
                    costProfileDtos.Add(exploration.AppraisalWellCostProfile);
                }
                if (exploration.SidetrackCostProfile?.Values.Length > 0)
                {
                    costProfileDtos.Add(exploration.SidetrackCostProfile);
                }

                var costProfile = TimeSeriesCostDto.MergeCostProfilesList(costProfileDtos);

                sTEACaseDto.Exploration = costProfile;
                sTEACaseDto.Exploration.StartYear += caseDto.DG4Date.Year;
            }
        }
    }
}
