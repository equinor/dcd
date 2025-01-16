using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.Profiles.DrainageStrategies.AdditionalProductionProfileGases.Dtos;
using api.Features.Profiles.DrainageStrategies.AdditionalProductionProfileOils.Dtos;
using api.Features.Profiles.DrainageStrategies.ProductionProfileOils.Dtos;
using api.Models;

namespace api.Features.Stea.Dtos;

public static class SteaCaseDtoBuilder
{
    public static SteaCaseDto Build(Case caseItem, SteaDbData steaDbData)
    {
        var steaCaseDto = new SteaCaseDto
        {
            Name = caseItem.Name
        };

        AddStudyCost(steaCaseDto, caseItem);
        AddOpexCost(steaCaseDto, caseItem);
        AddCapex(steaDbData, steaCaseDto, caseItem);
        AddCessationCost(steaCaseDto, caseItem);
        AddExploration(steaDbData, steaCaseDto, caseItem);
        AddProductionSalesAndVolumes(steaDbData, steaCaseDto, caseItem);

        steaCaseDto.StartYear = new[]
        {
            steaCaseDto.Exploration.StartYear,
            steaCaseDto.ProductionAndSalesVolumes.StartYear,
            steaCaseDto.Capex.StartYear,
            steaCaseDto.StudyCostProfile.StartYear,
            steaCaseDto.OpexCostProfile.StartYear,
            steaCaseDto.Capex.CessationCost.StartYear
        }.Where(x => x > 1).Min();

        return steaCaseDto;
    }

    private static void AddOpexCost(SteaCaseDto steaCaseDto, Case caseDto)
    {
        var costProfileDtos = new List<TimeSeriesCostDto>();

        if (caseDto.HistoricCostCostProfile != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseDto.HistoricCostCostProfile));
        }

        if (caseDto.WellInterventionCostProfileOverride?.Override == true)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseDto.WellInterventionCostProfileOverride));
        }
        else if (caseDto.WellInterventionCostProfile != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseDto.WellInterventionCostProfile));
        }

        if (caseDto.OffshoreFacilitiesOperationsCostProfileOverride?.Override == true)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseDto.OffshoreFacilitiesOperationsCostProfileOverride));
        }
        else if (caseDto.OffshoreFacilitiesOperationsCostProfile != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseDto.OffshoreFacilitiesOperationsCostProfile));
        }

        if (caseDto.OnshoreRelatedOPEXCostProfile != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseDto.OnshoreRelatedOPEXCostProfile));
        }

        if (caseDto.AdditionalOPEXCostProfile != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseDto.AdditionalOPEXCostProfile));
        }

        var costProfile = TimeSeriesCostDto.MergeCostProfilesList(costProfileDtos);

        steaCaseDto.OpexCostProfile = new OpexCostProfileDto
        {
            StartYear = costProfile.StartYear + caseDto.DG4Date.Year,
            Values = costProfile.Values,
        };
    }

    private static void AddStudyCost(SteaCaseDto steaCaseDto, Case caseItem)
    {
        var costProfileDtos = new List<TimeSeriesCostDto>();

        if (caseItem.TotalFeasibilityAndConceptStudiesOverride?.Override == true)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseItem.TotalFeasibilityAndConceptStudiesOverride));
        }
        else if (caseItem.TotalFeasibilityAndConceptStudies != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseItem.TotalFeasibilityAndConceptStudies));
        }

        if (caseItem.TotalFEEDStudiesOverride?.Override == true)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseItem.TotalFEEDStudiesOverride));
        }
        else if (caseItem.TotalFEEDStudies != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseItem.TotalFEEDStudies));
        }

        if (caseItem.TotalOtherStudiesCostProfile?.Values.Length > 0)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseItem.TotalOtherStudiesCostProfile));
        }

        var costProfile = TimeSeriesCostDto.MergeCostProfilesList(costProfileDtos);

        steaCaseDto.StudyCostProfile = new StudyCostProfileDto
        {
            StartYear = costProfile.StartYear + caseItem.DG4Date.Year,
            Values = costProfile.Values,
        };
    }

    private static void AddCessationCost(SteaCaseDto steaCaseDto, Case caseItem)
    {
        var costProfileDtos = new List<TimeSeriesCostDto>();

        if (caseItem.CessationWellsCostOverride?.Override == true)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseItem.CessationWellsCostOverride));
        }
        else if (caseItem.CessationWellsCost != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseItem.CessationWellsCost));
        }

        if (caseItem.CessationOffshoreFacilitiesCostOverride?.Override == true)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseItem.CessationOffshoreFacilitiesCostOverride));
        }
        else if (caseItem.CessationOffshoreFacilitiesCost != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseItem.CessationOffshoreFacilitiesCost));
        }

        if (caseItem.CessationOnshoreFacilitiesCostProfile != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseItem.CessationOnshoreFacilitiesCostProfile));
        }

        var costProfile = TimeSeriesCostDto.MergeCostProfilesList(costProfileDtos);

        steaCaseDto.Capex.CessationCost = new CessationCostDto
        {
            StartYear = costProfile.StartYear + caseItem.DG4Date.Year,
            Values = costProfile.Values,
        };
    }

    private static void AddCapex(SteaDbData steaDbData, SteaCaseDto steaCaseDto, Case caseItem)
    {
        steaCaseDto.Capex = new CapexDto
        {
            Drilling = new TimeSeriesCostDto()
        };

        var dg4Year = caseItem.DG4Date.Year;

        var wellProjectDto = steaDbData.WellProjects.First(l => l.Id == caseItem.WellProjectLink);

        var costProfileDtos = new List<TimeSeriesCostDto>();

        if (wellProjectDto.OilProducerCostProfileOverride?.Override == true)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(wellProjectDto.OilProducerCostProfileOverride));
        }
        else if (wellProjectDto.OilProducerCostProfile != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(wellProjectDto.OilProducerCostProfile));
        }

        if (wellProjectDto.GasProducerCostProfileOverride?.Override == true)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(wellProjectDto.GasProducerCostProfileOverride));
        }
        else if (wellProjectDto.GasProducerCostProfile != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(wellProjectDto.GasProducerCostProfile));
        }

        if (wellProjectDto.WaterInjectorCostProfileOverride?.Override == true)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(wellProjectDto.WaterInjectorCostProfileOverride));
        }
        else if (wellProjectDto.WaterInjectorCostProfile != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(wellProjectDto.WaterInjectorCostProfile));
        }

        if (wellProjectDto.GasInjectorCostProfileOverride?.Override == true)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(wellProjectDto.GasInjectorCostProfileOverride));
        }
        else if (wellProjectDto.GasInjectorCostProfile != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(wellProjectDto.GasInjectorCostProfile));
        }

        var costProfile = TimeSeriesCostDto.MergeCostProfilesList(costProfileDtos);
        costProfile.StartYear += dg4Year;

        steaCaseDto.Capex.Drilling = costProfile;
        steaCaseDto.Capex.AddValues(costProfile);

        steaCaseDto.Capex.OffshoreFacilities = new OffshoreFacilitiesCostProfileDto();
        var substructureDto = steaDbData.Substructures.First(l => l.Id == caseItem.SubstructureLink);

        if (substructureDto.CostProfileOverride?.Override == true)
        {
            substructureDto.CostProfileOverride.StartYear += dg4Year;
            steaCaseDto.Capex.OffshoreFacilities.AddValues(new TimeSeriesCostDto(substructureDto.CostProfileOverride));
        }
        else if (substructureDto.CostProfile != null)
        {
            substructureDto.CostProfile.StartYear += dg4Year;
            steaCaseDto.Capex.OffshoreFacilities.AddValues(new TimeSeriesCostDto(substructureDto.CostProfile));
        }

        var surfDto = steaDbData.Surfs.First(l => l.Id == caseItem.SurfLink);

        if (surfDto.CostProfileOverride?.Override == true)
        {
            surfDto.CostProfileOverride.StartYear += dg4Year;
            steaCaseDto.Capex.OffshoreFacilities.AddValues(new TimeSeriesCostDto(surfDto.CostProfileOverride));
        }
        else if (surfDto.CostProfile != null)
        {
            surfDto.CostProfile.StartYear += dg4Year;
            steaCaseDto.Capex.OffshoreFacilities.AddValues(new TimeSeriesCostDto(surfDto.CostProfile));
        }

        var topsideDto = steaDbData.Topsides.First(l => l.Id == caseItem.TopsideLink);

        if (topsideDto.CostProfileOverride?.Override == true)
        {
            topsideDto.CostProfileOverride.StartYear += dg4Year;
            steaCaseDto.Capex.OffshoreFacilities.AddValues(new TimeSeriesCostDto(topsideDto.CostProfileOverride));
        }
        else if (topsideDto.CostProfile != null)
        {
            topsideDto.CostProfile.StartYear += dg4Year;
            steaCaseDto.Capex.OffshoreFacilities.AddValues(new TimeSeriesCostDto(topsideDto.CostProfile));
        }

        var transportDto = steaDbData.Transports.First(l => l.Id == caseItem.TransportLink);

        if (transportDto.CostProfileOverride?.Override == true)
        {
            transportDto.CostProfileOverride.StartYear += dg4Year;
            steaCaseDto.Capex.OffshoreFacilities.AddValues(new TimeSeriesCostDto(transportDto.CostProfileOverride));
        }
        else if (transportDto.CostProfile != null)
        {
            transportDto.CostProfile.StartYear += dg4Year;
            steaCaseDto.Capex.OffshoreFacilities.AddValues(new TimeSeriesCostDto(transportDto.CostProfile));
        }

        var onshorePowerSupplyDto = steaDbData.OnshorePowerSupplies.First(l => l.Id == caseItem.OnshorePowerSupplyLink);

        if (onshorePowerSupplyDto.CostProfileOverride?.Override == true)
        {
            onshorePowerSupplyDto.CostProfileOverride.StartYear += dg4Year;
            steaCaseDto.Capex.OnshorePowerSupplyCost.AddValues(new TimeSeriesCostDto(onshorePowerSupplyDto.CostProfileOverride));
        }
        else if (onshorePowerSupplyDto.CostProfile != null)
        {
            onshorePowerSupplyDto.CostProfile.StartYear += dg4Year;
            steaCaseDto.Capex.OnshorePowerSupplyCost.AddValues(new TimeSeriesCostDto(onshorePowerSupplyDto.CostProfile));
        }

        steaCaseDto.Capex.AddValues(steaCaseDto.Capex.OffshoreFacilities);
        steaCaseDto.Capex.AddValues(steaCaseDto.Capex.OnshorePowerSupplyCost);
    }

    private static void AddProductionSalesAndVolumes(SteaDbData steaDbData, SteaCaseDto steaCaseDto, Case caseItem)
    {
        steaCaseDto.ProductionAndSalesVolumes = new ProductionAndSalesVolumesDto
        {
            TotalAndAnnualOil = new ProductionProfileOilDto(),
            TotalAndAnnualSalesGas = new NetSalesGasDto(),
            Co2Emissions = new Co2EmissionsDto(),
            AdditionalOil = new AdditionalProductionProfileOilDto(),
            AdditionalGas = new AdditionalProductionProfileGasDto()
        };

        var dg4Year = caseItem.DG4Date.Year;

        var drainageStrategyDto = steaDbData.DrainageStrategies.First(d => d.Id == caseItem.DrainageStrategyLink);
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
                StartYear = mergedOilProfile.StartYear + dg4Year,
                Values = mergedOilProfile.Values,
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualOil.StartYear);
        }

        if (drainageStrategyDto.NetSalesGasOverride?.Override == true)
        {
            steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = new NetSalesGasDto
            {
                StartYear = drainageStrategyDto.NetSalesGasOverride.StartYear + dg4Year,
                Values = drainageStrategyDto.NetSalesGasOverride.Values,
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas.StartYear);
        }
        else if (drainageStrategyDto.NetSalesGas != null)
        {
            steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = new NetSalesGasDto
            {
                StartYear = drainageStrategyDto.NetSalesGas.StartYear + dg4Year,
                Values = drainageStrategyDto.NetSalesGas.Values
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas.StartYear);
        }

        if (drainageStrategyDto.ImportedElectricityOverride?.Override == true)
        {
            steaCaseDto.ProductionAndSalesVolumes.ImportedElectricity = new ImportedElectricityDto
            {
                StartYear = drainageStrategyDto.ImportedElectricityOverride.StartYear + dg4Year,
                Values = drainageStrategyDto.ImportedElectricityOverride.Values,
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.ImportedElectricity.StartYear);
        }
        else if (drainageStrategyDto.ImportedElectricity != null)
        {
            steaCaseDto.ProductionAndSalesVolumes.ImportedElectricity = new ImportedElectricityDto
            {
                StartYear = drainageStrategyDto.ImportedElectricity.StartYear + dg4Year,
                Values = drainageStrategyDto.ImportedElectricity.Values
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.ImportedElectricity.StartYear);
        }

        if (drainageStrategyDto.Co2EmissionsOverride?.Override == true)
        {
            steaCaseDto.ProductionAndSalesVolumes.Co2Emissions = new Co2EmissionsDto
            {
                StartYear = drainageStrategyDto.Co2EmissionsOverride.StartYear + dg4Year,
                Values = drainageStrategyDto.Co2EmissionsOverride.Values,
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.Co2Emissions.StartYear);
        }
        else if (drainageStrategyDto.Co2Emissions != null)
        {
            steaCaseDto.ProductionAndSalesVolumes.Co2Emissions = new Co2EmissionsDto
            {
                StartYear = drainageStrategyDto.Co2Emissions.StartYear + dg4Year,
                Values = drainageStrategyDto.Co2Emissions.Values
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.Co2Emissions.StartYear);
        }

        if (startYearsProductionSalesAndVolumes.Count > 0)
        {
            steaCaseDto.ProductionAndSalesVolumes.StartYear = startYearsProductionSalesAndVolumes.Min();
        }

        if (drainageStrategyDto.AdditionalProductionProfileOil != null)
        {
            steaCaseDto.ProductionAndSalesVolumes.AdditionalOil = new AdditionalProductionProfileOilDto
            {
                StartYear = drainageStrategyDto.AdditionalProductionProfileOil.StartYear + dg4Year,
                Values = drainageStrategyDto.AdditionalProductionProfileOil.Values,
            };
        }

        if (drainageStrategyDto.AdditionalProductionProfileGas != null)
        {
            steaCaseDto.ProductionAndSalesVolumes.AdditionalGas = new AdditionalProductionProfileGasDto
            {
                StartYear = drainageStrategyDto.AdditionalProductionProfileGas.StartYear + dg4Year,
                Values = drainageStrategyDto.AdditionalProductionProfileGas.Values,
            };
        }
    }

    private static void AddExploration(SteaDbData steaDbData, SteaCaseDto steaCaseDto, Case caseItem)
    {
        steaCaseDto.Exploration = new TimeSeriesCostDto();

        var exploration = steaDbData.Explorations.First(e => e.Id == caseItem.ExplorationLink);

        var costProfileDtos = new List<TimeSeriesCostDto>();
        if (exploration.ExplorationWellCostProfile?.Values.Length > 0)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(exploration.ExplorationWellCostProfile));
        }
        if (exploration.AppraisalWellCostProfile?.Values.Length > 0)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(exploration.AppraisalWellCostProfile));
        }
        if (exploration.SidetrackCostProfile?.Values.Length > 0)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(exploration.SidetrackCostProfile));
        }
        if (exploration.GAndGAdminCostOverride?.Override == true)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(exploration.GAndGAdminCostOverride));
        }
        else if (exploration.GAndGAdminCost != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(exploration.GAndGAdminCost));
        }
        if (exploration.SeismicAcquisitionAndProcessing?.Values.Length > 0)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(exploration.SeismicAcquisitionAndProcessing));
        }
        if (exploration.CountryOfficeCost?.Values.Length > 0)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(exploration.CountryOfficeCost));
        }

        steaCaseDto.Exploration = TimeSeriesCostDto.MergeCostProfilesList(costProfileDtos);
        steaCaseDto.Exploration.StartYear += caseItem.DG4Date.Year;
    }
}
