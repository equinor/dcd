using api.Features.Assets.CaseAssets.DrainageStrategies.Dtos;
using api.Features.CaseProfiles.Dtos;
using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Models;

namespace api.Features.Stea.Dtos;

public static class SteaCaseDtoBuilder
{
    public static SteaCaseDto Build(CaseWithProfilesDto caseDto, ProjectWithAssetsWrapperDto projectWithAssetsWrapperDto)
    {
        var steaCaseDto = new SteaCaseDto
        {
            Name = caseDto.Name
        };

        AddStudyCost(steaCaseDto, caseDto);
        AddOpexCost(steaCaseDto, caseDto);
        AddCapex(projectWithAssetsWrapperDto, steaCaseDto, caseDto);
        AddCessationCost(steaCaseDto, caseDto);
        AddExploration(projectWithAssetsWrapperDto, steaCaseDto, caseDto);
        AddProductionSalesAndVolumes(projectWithAssetsWrapperDto, steaCaseDto, caseDto);

        var startYearsCase = new[]
        {
            steaCaseDto.Exploration.StartYear,
            steaCaseDto.ProductionAndSalesVolumes.StartYear,
            steaCaseDto.Capex.StartYear,
            steaCaseDto.StudyCostProfile.StartYear,
            steaCaseDto.OpexCostProfile.StartYear,
            steaCaseDto.Capex.CessationCost.StartYear
        };

        Array.Sort(startYearsCase);

        steaCaseDto.StartYear = Array.Find(startYearsCase, e => e > 1);

        return steaCaseDto;
    }

    private static void AddOpexCost(SteaCaseDto steaCaseDto, CaseWithProfilesDto caseDto)
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
        steaCaseDto.OpexCostProfile = opexCost;
        steaCaseDto.OpexCostProfile.StartYear += caseDto.DG4Date.Year;
    }

    private static void AddStudyCost(SteaCaseDto steaCaseDto, CaseWithProfilesDto caseDto)
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
        steaCaseDto.StudyCostProfile = studyCost;
        steaCaseDto.StudyCostProfile.StartYear += caseDto.DG4Date.Year;
    }

    private static void AddCessationCost(SteaCaseDto steaCaseDto, CaseWithProfilesDto caseDto)
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
        steaCaseDto.Capex.CessationCost = cessationCost;
        steaCaseDto.Capex.CessationCost.StartYear += caseDto.DG4Date.Year;
    }

    private static void AddCapex(ProjectWithAssetsWrapperDto projectWithAssetsWrapperDto, SteaCaseDto steaCaseDto, CaseWithProfilesDto c)
    {
        steaCaseDto.Capex = new CapexDto
        {
            Drilling = new TimeSeriesCostDto()
        };

        var dg4Year = c.DG4Date.Year;

        if (c.WellProjectLink != Guid.Empty)
        {
            var wellProjectDto = projectWithAssetsWrapperDto.WellProjects!.First(l => l.Id == c.WellProjectLink);

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

            steaCaseDto.Capex.Drilling = costProfile;
            steaCaseDto.Capex.Drilling.StartYear += dg4Year;
            steaCaseDto.Capex.AddValues(steaCaseDto.Capex.Drilling);
        }

        steaCaseDto.Capex.OffshoreFacilities = new OffshoreFacilitiesCostProfileDto();
        if (c.SubstructureLink != Guid.Empty)
        {
            var substructureDto = projectWithAssetsWrapperDto.Substructures.First(l => l.Id == c.SubstructureLink);

            if (substructureDto.CostProfileOverride?.Override == true)
            {
                var costProfile = substructureDto.CostProfileOverride;
                costProfile.StartYear += dg4Year;
                steaCaseDto.Capex.OffshoreFacilities.AddValues(costProfile);
            }
            else if (substructureDto.CostProfile != null)
            {
                var costProfile = substructureDto.CostProfile;
                costProfile.StartYear += dg4Year;
                steaCaseDto.Capex.OffshoreFacilities.AddValues(costProfile);
            }
        }

        if (c.SurfLink != Guid.Empty)
        {
            var surfDto = projectWithAssetsWrapperDto.Surfs.First(l => l.Id == c.SurfLink);

            if (surfDto.CostProfileOverride?.Override == true)
            {
                var costProfile = surfDto.CostProfileOverride;
                costProfile.StartYear += dg4Year;
                steaCaseDto.Capex.OffshoreFacilities.AddValues(costProfile);
            }
            else if (surfDto.CostProfile != null)
            {
                var costProfile = surfDto.CostProfile;
                costProfile.StartYear += dg4Year;
                steaCaseDto.Capex.OffshoreFacilities.AddValues(costProfile);
            }
        }

        if (c.TopsideLink != Guid.Empty)
        {
            var topsideDto = projectWithAssetsWrapperDto.Topsides.First(l => l.Id == c.TopsideLink);

            if (topsideDto.CostProfileOverride?.Override == true)
            {
                var costProfile = topsideDto.CostProfileOverride;
                costProfile.StartYear += dg4Year;
                steaCaseDto.Capex.OffshoreFacilities.AddValues(costProfile);
            }
            else if (topsideDto.CostProfile != null)
            {
                var costProfile = topsideDto.CostProfile;
                costProfile.StartYear += dg4Year;
                steaCaseDto.Capex.OffshoreFacilities.AddValues(costProfile);
            }
        }

        if (c.TransportLink != Guid.Empty)
        {
            var transportDto = projectWithAssetsWrapperDto.Transports.First(l => l.Id == c.TransportLink);

            if (transportDto.CostProfileOverride?.Override == true)
            {
                var costProfile = transportDto.CostProfileOverride;
                costProfile.StartYear += dg4Year;
                steaCaseDto.Capex.OffshoreFacilities.AddValues(costProfile);
            }
            else if (transportDto.CostProfile != null)
            {
                var costProfile = transportDto.CostProfile;
                costProfile.StartYear += dg4Year;
                steaCaseDto.Capex.OffshoreFacilities.AddValues(costProfile);
            }
        }

        if (c.OnshorePowerSupplyLink != Guid.Empty)
        {
            var onshorePowerSupplyDto = projectWithAssetsWrapperDto.OnshorePowerSupplies.First(l => l.Id == c.OnshorePowerSupplyLink);

            if (onshorePowerSupplyDto.CostProfileOverride?.Override == true)
            {
                var costProfile = onshorePowerSupplyDto.CostProfileOverride;
                costProfile.StartYear += dg4Year;
                steaCaseDto.Capex.OnshorePowerSupplyCost.AddValues(costProfile);
            }
            else if (onshorePowerSupplyDto.CostProfile != null)
            {
                var costProfile = onshorePowerSupplyDto.CostProfile;
                costProfile.StartYear += dg4Year;
                steaCaseDto.Capex.OnshorePowerSupplyCost.AddValues(costProfile);
            }
        }
        steaCaseDto.Capex.AddValues(steaCaseDto.Capex.OffshoreFacilities);
        steaCaseDto.Capex.AddValues(steaCaseDto.Capex.OnshorePowerSupplyCost);

    }

    private static void AddProductionSalesAndVolumes(ProjectWithAssetsWrapperDto projectWithAssetsWrapperDto, SteaCaseDto steaCaseDto, CaseWithProfilesDto c)
    {
        steaCaseDto.ProductionAndSalesVolumes = new ProductionAndSalesVolumesDto
        {
            TotalAndAnnualOil = new ProductionProfileOilDto(),
            TotalAndAnnualSalesGas = new NetSalesGasDto(),
            Co2Emissions = new Co2EmissionsDto(),
            AdditionalOil = new AdditionalProductionProfileOilDto(),
            AdditionalGas = new AdditionalProductionProfileGasDto(),
        };

        var dg4Year = c.DG4Date.Year;

        if (c.DrainageStrategyLink != Guid.Empty)
        {
            var drainageStrategyDto = projectWithAssetsWrapperDto.DrainageStrategies.First(d => d.Id == c.DrainageStrategyLink);
            var startYearsProductionSalesAndVolumes = new List<int>();

            if (drainageStrategyDto.ProductionProfileOil != null || drainageStrategyDto.AdditionalProductionProfileOil != null)
            {
                var oilProfile = drainageStrategyDto.ProductionProfileOil != null
                    ? new TimeSeriesCost
                    {
                        StartYear = drainageStrategyDto.ProductionProfileOil.StartYear,
                        Values = drainageStrategyDto.ProductionProfileOil.Values,
                    }
                    : new TimeSeriesCost { Values = [], StartYear = 0 };

                var additionalOilProfile = drainageStrategyDto.AdditionalProductionProfileOil != null
                    ? new TimeSeriesCost
                    {
                        StartYear = drainageStrategyDto.AdditionalProductionProfileOil.StartYear,
                        Values = drainageStrategyDto.AdditionalProductionProfileOil.Values,
                    }
                    : new TimeSeriesCost { Values = [], StartYear = 0 };

                var mergedOilProfile = TimeSeriesCost.MergeCostProfiles(oilProfile, additionalOilProfile);

                steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualOil = new ProductionProfileOilDto
                {
                    Id = mergedOilProfile.Id,
                    StartYear = mergedOilProfile.StartYear + dg4Year,
                    Values = mergedOilProfile.Values,
                };
                startYearsProductionSalesAndVolumes.Add(mergedOilProfile.StartYear + dg4Year);
            }

            if (drainageStrategyDto.NetSalesGasOverride?.Override == true)
            {
                var productionProfile = new NetSalesGasDto
                {
                    StartYear = drainageStrategyDto.NetSalesGasOverride.StartYear,
                    Values = drainageStrategyDto.NetSalesGasOverride.Values,
                };
                steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = productionProfile;
                startYearsProductionSalesAndVolumes.Add(productionProfile.StartYear += dg4Year);
            }
            else if (drainageStrategyDto.NetSalesGas != null)
            {
                steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = drainageStrategyDto.NetSalesGas;
                startYearsProductionSalesAndVolumes.Add(drainageStrategyDto.NetSalesGas.StartYear += dg4Year);
            }

            if (drainageStrategyDto.ImportedElectricityOverride?.Override == true)
            {
                var productionProfile = new ImportedElectricityDto
                {
                    StartYear = drainageStrategyDto.ImportedElectricityOverride.StartYear,
                    Values = drainageStrategyDto.ImportedElectricityOverride.Values,
                };
                steaCaseDto.ProductionAndSalesVolumes.ImportedElectricity = productionProfile;
                startYearsProductionSalesAndVolumes.Add(productionProfile.StartYear += dg4Year);
            }
            else if (drainageStrategyDto.ImportedElectricity != null)
            {
                steaCaseDto.ProductionAndSalesVolumes.ImportedElectricity = drainageStrategyDto.ImportedElectricity;
                startYearsProductionSalesAndVolumes.Add(drainageStrategyDto.ImportedElectricity.StartYear += dg4Year);
            }

            if (drainageStrategyDto.Co2EmissionsOverride?.Override == true)
            {
                var profile = new Co2EmissionsDto
                {
                    StartYear = drainageStrategyDto.Co2EmissionsOverride.StartYear,
                    Values = drainageStrategyDto.Co2EmissionsOverride.Values,
                };
                steaCaseDto.ProductionAndSalesVolumes.Co2Emissions = profile;
                startYearsProductionSalesAndVolumes.Add(profile.StartYear += dg4Year);
            }
            else if (drainageStrategyDto.Co2Emissions != null)
            {
                steaCaseDto.ProductionAndSalesVolumes.Co2Emissions = drainageStrategyDto.Co2Emissions;
                startYearsProductionSalesAndVolumes.Add(drainageStrategyDto.Co2Emissions.StartYear += dg4Year);
            }

            if (startYearsProductionSalesAndVolumes.Count > 0)
            {
                steaCaseDto.ProductionAndSalesVolumes.StartYear = startYearsProductionSalesAndVolumes.Min();
            }

            if (drainageStrategyDto.AdditionalProductionProfileOil != null)
            {
                var additionalOilProfile = new AdditionalProductionProfileOilDto
                {
                    StartYear = drainageStrategyDto.AdditionalProductionProfileOil.StartYear + dg4Year,
                    Values = drainageStrategyDto.AdditionalProductionProfileOil.Values,
                };
                steaCaseDto.ProductionAndSalesVolumes.AdditionalOil = additionalOilProfile;
            }

            if (drainageStrategyDto.AdditionalProductionProfileGas != null)
            {
                var additionalGasProfile = new AdditionalProductionProfileGasDto
                {
                    StartYear = drainageStrategyDto.AdditionalProductionProfileGas.StartYear + dg4Year,
                    Values = drainageStrategyDto.AdditionalProductionProfileGas.Values,
                };
                steaCaseDto.ProductionAndSalesVolumes.AdditionalGas = additionalGasProfile;
            }
        }
    }

    private static void AddExploration(ProjectWithAssetsWrapperDto projectWithAssetsWrapperDto, SteaCaseDto steaCaseDto, CaseWithProfilesDto caseDto)
    {
        steaCaseDto.Exploration = new TimeSeriesCostDto();

        if (caseDto.ExplorationLink != Guid.Empty)
        {
            var exploration = projectWithAssetsWrapperDto.Explorations.First(e => e.Id == caseDto.ExplorationLink);

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

                steaCaseDto.Exploration = costProfile;
                steaCaseDto.Exploration.StartYear += caseDto.DG4Date.Year;
            }
        }
    }
}
