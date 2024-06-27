using api.Dtos;

namespace api.Adapters;

public static class STEACaseDtoBuilder
{
    public static STEACaseDto Build(CaseWithProfilesDto caseDto, ProjectDto projectDto)
    {
        var sTEACaseDto = new STEACaseDto
        {
            Name = caseDto.Name
        };
        AddStudyCost(sTEACaseDto, caseDto);
        AddOpexCost(sTEACaseDto, caseDto);
        AddCapex(projectDto, sTEACaseDto, caseDto);
        AddCessationCost(sTEACaseDto, caseDto);
        AddExploration(projectDto, sTEACaseDto, caseDto);
        AddProductionSalesAndVolumes(projectDto, sTEACaseDto, caseDto);
        var startYearsCase = new int[] { sTEACaseDto.Exploration.StartYear, sTEACaseDto.ProductionAndSalesVolumes.StartYear,
            sTEACaseDto.Capex.StartYear, sTEACaseDto.StudyCostProfile.StartYear, sTEACaseDto.OpexCostProfile.StartYear, sTEACaseDto.Capex.CessationCost.StartYear };
        Array.Sort(startYearsCase);
        sTEACaseDto.StartYear = Array.Find(startYearsCase, e => e > 1);

        return sTEACaseDto;
    }

    private static void AddOpexCost(STEACaseDto sTEACaseDto, CaseWithProfilesDto caseDto)
    {
        var costProfileDtos = new List<TimeSeriesCostDto>();
        if (caseDto.HistoricCostCostProfile != null)
        {
            costProfileDtos.Add(caseDto.HistoricCostCostProfile);
        }
        if (caseDto.WellInterventionCostProfileOverride?.Override == true)
        {
            costProfileDtos.Add(caseDto.WellInterventionCostProfileOverride);
        }
        else if (caseDto.WellInterventionCostProfile != null)
        {
            costProfileDtos.Add(caseDto.WellInterventionCostProfile);
        }

        if (caseDto.OffshoreFacilitiesOperationsCostProfileOverride?.Override == true)
        {
            costProfileDtos.Add(caseDto.OffshoreFacilitiesOperationsCostProfileOverride);
        }
        else if (caseDto.OffshoreFacilitiesOperationsCostProfile != null)
        {
            costProfileDtos.Add(caseDto.OffshoreFacilitiesOperationsCostProfile);
        }
        if (caseDto.OnshoreRelatedOPEXCostProfile != null)
        {
            costProfileDtos.Add(caseDto.OnshoreRelatedOPEXCostProfile);
        }
        if (caseDto.AdditionalOPEXCostProfile != null)
        {
            costProfileDtos.Add(caseDto.AdditionalOPEXCostProfile);
        }

        var costProfile = TimeSeriesCostDto.MergeCostProfilesList(costProfileDtos);

        var opexCost = new OpexCostProfileDto
        {
            StartYear = costProfile.StartYear,
            Values = costProfile.Values,
        };
        sTEACaseDto.OpexCostProfile = opexCost;
        sTEACaseDto.OpexCostProfile.StartYear += caseDto.DG4Date.Year;
    }

    private static void AddStudyCost(STEACaseDto sTEACaseDto, CaseWithProfilesDto caseDto)
    {
        var costProfileDtos = new List<TimeSeriesCostDto>();
        if (caseDto.TotalFeasibilityAndConceptStudiesOverride?.Override == true)
        {
            costProfileDtos.Add(caseDto.TotalFeasibilityAndConceptStudiesOverride);
        }
        else if (caseDto.TotalFeasibilityAndConceptStudies != null)
        {
            costProfileDtos.Add(caseDto.TotalFeasibilityAndConceptStudies);
        }
        if (caseDto.TotalFEEDStudiesOverride?.Override == true)
        {
            costProfileDtos.Add(caseDto.TotalFEEDStudiesOverride);
        }
        else if (caseDto.TotalFEEDStudies != null)
        {
            costProfileDtos.Add(caseDto.TotalFEEDStudies);
        }
        if (caseDto.TotalOtherStudiesCostProfile?.Values.Length > 0)
        {
            costProfileDtos.Add(caseDto.TotalOtherStudiesCostProfile);
        }

        var costProfile = TimeSeriesCostDto.MergeCostProfilesList(costProfileDtos);
        var studyCost = new StudyCostProfileDto
        {
            StartYear = costProfile.StartYear,
            Values = costProfile.Values,
        };
        sTEACaseDto.StudyCostProfile = studyCost;
        sTEACaseDto.StudyCostProfile.StartYear += caseDto.DG4Date.Year;
    }

    private static void AddCessationCost(STEACaseDto sTEACaseDto, CaseWithProfilesDto caseDto)
    {
        var costProfileDtos = new List<TimeSeriesCostDto>();
        if (caseDto.CessationWellsCostOverride?.Override == true)
        {
            costProfileDtos.Add(caseDto.CessationWellsCostOverride);
        }
        else if (caseDto.CessationWellsCost != null)
        {
            costProfileDtos.Add(caseDto.CessationWellsCost);
        }

        if (caseDto.CessationOffshoreFacilitiesCostOverride?.Override == true)
        {
            costProfileDtos.Add(caseDto.CessationOffshoreFacilitiesCostOverride);
        }
        else if (caseDto.CessationOffshoreFacilitiesCost != null)
        {
            costProfileDtos.Add(caseDto.CessationOffshoreFacilitiesCost);
        }

        if (caseDto.CessationOnshoreFacilitiesCostProfile != null)
        {
            costProfileDtos.Add(caseDto.CessationOnshoreFacilitiesCostProfile);
        }

        var costProfile = TimeSeriesCostDto.MergeCostProfilesList(costProfileDtos);
        var cessationCost = new CessationCostDto
        {
            StartYear = costProfile.StartYear,
            Values = costProfile.Values,
        };
        sTEACaseDto.Capex.CessationCost = cessationCost;
        sTEACaseDto.Capex.CessationCost.StartYear += caseDto.DG4Date.Year;
    }

    private static void AddCapex(ProjectDto p, STEACaseDto sTEACaseDto, CaseWithProfilesDto c)
    {
        sTEACaseDto.Capex = new CapexDto
        {
            Drilling = new TimeSeriesCostDto()
        };
        int dg4Year = c.DG4Date.Year;
        if (c.WellProjectLink != Guid.Empty)
        {
            var wellProjectDto = p.WellProjects!.First(l => l.Id == c.WellProjectLink);

            var costProfileDtos = new List<TimeSeriesCostDto>();

            if (wellProjectDto.OilProducerCostProfileOverride?.Override == true)
            {
                costProfileDtos.Add(wellProjectDto.OilProducerCostProfileOverride);
            }
            else if (wellProjectDto.OilProducerCostProfile != null)
            {
                costProfileDtos.Add(wellProjectDto.OilProducerCostProfile);
            }

            if (wellProjectDto.GasProducerCostProfileOverride?.Override == true)
            {
                costProfileDtos.Add(wellProjectDto.GasProducerCostProfileOverride);
            }
            else if (wellProjectDto.GasProducerCostProfile != null)
            {
                costProfileDtos.Add(wellProjectDto.GasProducerCostProfile);
            }

            if (wellProjectDto.WaterInjectorCostProfileOverride?.Override == true)
            {
                costProfileDtos.Add(wellProjectDto.WaterInjectorCostProfileOverride);
            }
            else if (wellProjectDto.WaterInjectorCostProfile != null)
            {
                costProfileDtos.Add(wellProjectDto.WaterInjectorCostProfile);
            }

            if (wellProjectDto.GasInjectorCostProfileOverride?.Override == true)
            {
                costProfileDtos.Add(wellProjectDto.GasInjectorCostProfileOverride);
            }
            else if (wellProjectDto.GasInjectorCostProfile != null)
            {
                costProfileDtos.Add(wellProjectDto.GasInjectorCostProfile);
            }

            var costProfile = TimeSeriesCostDto.MergeCostProfilesList(costProfileDtos);

            sTEACaseDto.Capex.Drilling = costProfile;
            sTEACaseDto.Capex.Drilling.StartYear += dg4Year;
            sTEACaseDto.Capex.AddValues(sTEACaseDto.Capex.Drilling);
        }

        sTEACaseDto.Capex.OffshoreFacilities = new OffshoreFacilitiesCostProfileDto();
        if (c.SubstructureLink != Guid.Empty)
        {
            var substructureDto = p.Substructures!.First(l => l.Id == c.SubstructureLink);

            if (substructureDto.CostProfileOverride?.Override == true)
            {
                var costProfile = substructureDto.CostProfileOverride;
                costProfile.StartYear += dg4Year;
                sTEACaseDto.Capex.OffshoreFacilities.AddValues(costProfile);
            }
            else if (substructureDto.CostProfile != null)
            {
                var costProfile = substructureDto.CostProfile;
                costProfile.StartYear += dg4Year;
                sTEACaseDto.Capex.OffshoreFacilities.AddValues(costProfile);
            }
        }

        if (c.SurfLink != Guid.Empty)
        {
            var surfDto = p.Surfs!.First(l => l.Id == c.SurfLink);

            if (surfDto.CostProfileOverride?.Override == true)
            {
                var costProfile = surfDto.CostProfileOverride;
                costProfile.StartYear += dg4Year;
                sTEACaseDto.Capex.OffshoreFacilities.AddValues(costProfile);
            }
            else if (surfDto.CostProfile != null)
            {
                var costProfile = surfDto.CostProfile;
                costProfile.StartYear += dg4Year;
                sTEACaseDto.Capex.OffshoreFacilities.AddValues(costProfile);
            }
        }

        if (c.TopsideLink != Guid.Empty)
        {
            var topsideDto = p.Topsides!.First(l => l.Id == c.TopsideLink);

            if (topsideDto.CostProfileOverride?.Override == true)
            {
                var costProfile = topsideDto.CostProfileOverride;
                costProfile.StartYear += dg4Year;
                sTEACaseDto.Capex.OffshoreFacilities.AddValues(costProfile);
            }
            else if (topsideDto.CostProfile != null)
            {
                var costProfile = topsideDto.CostProfile;
                costProfile.StartYear += dg4Year;
                sTEACaseDto.Capex.OffshoreFacilities.AddValues(costProfile);
            }
        }

        if (c.TransportLink != Guid.Empty)
        {
            var transportDto = p.Transports!.First(l => l.Id == c.TransportLink);

            if (transportDto.CostProfileOverride?.Override == true)
            {
                var costProfile = transportDto.CostProfileOverride;
                costProfile.StartYear += dg4Year;
                sTEACaseDto.Capex.OffshoreFacilities.AddValues(costProfile);
            }
            else if (transportDto.CostProfile != null)
            {
                var costProfile = transportDto.CostProfile;
                costProfile.StartYear += dg4Year;
                sTEACaseDto.Capex.OffshoreFacilities.AddValues(costProfile);
            }
        }

        sTEACaseDto.Capex.AddValues(sTEACaseDto.Capex.OffshoreFacilities);
    }

    private static void AddProductionSalesAndVolumes(ProjectDto p, STEACaseDto sTEACaseDto, CaseWithProfilesDto c)
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
            var drainageStrategyDto = p.DrainageStrategies!.First(d => d.Id == c.DrainageStrategyLink);
            var startYearsProductionSalesAndVolumes = new List<int>();
            if (drainageStrategyDto.ProductionProfileOil != null)
            {
                sTEACaseDto.ProductionAndSalesVolumes.TotalAndAnnualOil = drainageStrategyDto.ProductionProfileOil;
                startYearsProductionSalesAndVolumes.Add(drainageStrategyDto.ProductionProfileOil.StartYear += dg4Year);
            }

            if (drainageStrategyDto.NetSalesGasOverride?.Override == true)
            {
                var productionProfile = new NetSalesGasDto
                {
                    StartYear = drainageStrategyDto.NetSalesGasOverride.StartYear,
                    Values = drainageStrategyDto.NetSalesGasOverride.Values,
                };
                sTEACaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = productionProfile;
                startYearsProductionSalesAndVolumes.Add(productionProfile.StartYear += dg4Year);
            }
            else if (drainageStrategyDto.NetSalesGas != null)
            {
                sTEACaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = drainageStrategyDto.NetSalesGas;
                startYearsProductionSalesAndVolumes.Add(drainageStrategyDto.NetSalesGas.StartYear += dg4Year);
            }

            if (drainageStrategyDto.ImportedElectricityOverride?.Override == true)
            {
                var productionProfile = new ImportedElectricityDto
                {
                    StartYear = drainageStrategyDto.ImportedElectricityOverride.StartYear,
                    Values = drainageStrategyDto.ImportedElectricityOverride.Values,
                };
                sTEACaseDto.ProductionAndSalesVolumes.ImportedElectricity = productionProfile;
                startYearsProductionSalesAndVolumes.Add(productionProfile.StartYear += dg4Year);
            }
            else if (drainageStrategyDto.ImportedElectricity != null)
            {
                sTEACaseDto.ProductionAndSalesVolumes.ImportedElectricity = drainageStrategyDto.ImportedElectricity;
                startYearsProductionSalesAndVolumes.Add(drainageStrategyDto.ImportedElectricity.StartYear += dg4Year);
            }

            if (drainageStrategyDto.Co2EmissionsOverride?.Override == true)
            {
                var profile = new Co2EmissionsDto
                {
                    StartYear = drainageStrategyDto.Co2EmissionsOverride.StartYear,
                    Values = drainageStrategyDto.Co2EmissionsOverride.Values,
                };
                sTEACaseDto.ProductionAndSalesVolumes.Co2Emissions = profile;
                startYearsProductionSalesAndVolumes.Add(profile.StartYear += dg4Year);
            }
            else if (drainageStrategyDto.Co2Emissions != null)
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

    private static void AddExploration(ProjectDto p, STEACaseDto sTEACaseDto, CaseWithProfilesDto caseDto)
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
                if (exploration.GAndGAdminCostOverride?.Override == true)
                {
                    costProfileDtos.Add(exploration.GAndGAdminCostOverride);
                }
                else if (exploration.GAndGAdminCost != null)
                {
                    costProfileDtos.Add(exploration.GAndGAdminCost);
                }
                if (exploration.SeismicAcquisitionAndProcessing?.Values.Length > 0)
                {
                    costProfileDtos.Add(exploration.SeismicAcquisitionAndProcessing);
                }
                if (exploration.CountryOfficeCost?.Values.Length > 0)
                {
                    costProfileDtos.Add(exploration.CountryOfficeCost);
                }

                var costProfile = TimeSeriesCostDto.MergeCostProfilesList(costProfileDtos);

                sTEACaseDto.Exploration = costProfile;
                sTEACaseDto.Exploration.StartYear += caseDto.DG4Date.Year;
            }
        }
    }
}
