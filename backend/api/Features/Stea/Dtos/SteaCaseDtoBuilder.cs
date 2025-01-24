using api.Features.Profiles;
using api.Features.Profiles.Dtos;
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
            steaCaseDto.Capex.Summary.StartYear,
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

        var costProfile = TimeSeriesCostMerger.MergeCostProfilesList(costProfileDtos);

        steaCaseDto.OpexCostProfile = new TimeSeriesCostDto
        {
            StartYear = costProfile.StartYear + caseDto.DG4Date.Year,
            Values = costProfile.Values
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

        var costProfile = TimeSeriesCostMerger.MergeCostProfilesList(costProfileDtos);

        steaCaseDto.StudyCostProfile = new TimeSeriesCostDto
        {
            StartYear = costProfile.StartYear + caseItem.DG4Date.Year,
            Values = costProfile.Values
        };
    }

    private static void AddCessationCost(SteaCaseDto steaCaseDto, Case caseItem)
    {
        var costProfileDtos = new List<TimeSeriesCostDto>();

        if (caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCostOverride)?.Override == true)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.CessationWellsCostOverride)));
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCost) != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.CessationWellsCost)));
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCostOverride)?.Override == true)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.CessationOffshoreFacilitiesCostOverride)));
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCost) != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.CessationOffshoreFacilitiesCost)));
        }

        if (caseItem.CessationOnshoreFacilitiesCostProfile != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(caseItem.CessationOnshoreFacilitiesCostProfile));
        }

        var costProfile = TimeSeriesCostMerger.MergeCostProfilesList(costProfileDtos);

        steaCaseDto.Capex.CessationCost = new TimeSeriesCostDto
        {
            StartYear = costProfile.StartYear + caseItem.DG4Date.Year,
            Values = costProfile.Values
        };
    }

    private static void AddCapex(SteaDbData steaDbData, SteaCaseDto steaCaseDto, Case caseItem)
    {
        steaCaseDto.Capex = new CapexDto
        {
            Drilling = new TimeSeriesCostDto()
        };

        var dg4Year = caseItem.DG4Date.Year;

        var wellProject = steaDbData.WellProjects.First(l => l.Id == caseItem.WellProjectLink);

        var costProfileDtos = new List<TimeSeriesCostDto>();

        if (wellProject.OilProducerCostProfileOverride?.Override == true)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(wellProject.OilProducerCostProfileOverride));
        }
        else if (wellProject.OilProducerCostProfile != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(wellProject.OilProducerCostProfile));
        }

        if (wellProject.GasProducerCostProfileOverride?.Override == true)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(wellProject.GasProducerCostProfileOverride));
        }
        else if (wellProject.GasProducerCostProfile != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(wellProject.GasProducerCostProfile));
        }

        if (wellProject.WaterInjectorCostProfileOverride?.Override == true)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(wellProject.WaterInjectorCostProfileOverride));
        }
        else if (wellProject.WaterInjectorCostProfile != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(wellProject.WaterInjectorCostProfile));
        }

        if (wellProject.GasInjectorCostProfileOverride?.Override == true)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(wellProject.GasInjectorCostProfileOverride));
        }
        else if (wellProject.GasInjectorCostProfile != null)
        {
            costProfileDtos.Add(new TimeSeriesCostDto(wellProject.GasInjectorCostProfile));
        }

        var costProfile = TimeSeriesCostMerger.MergeCostProfilesList(costProfileDtos);
        costProfile.StartYear += dg4Year;

        steaCaseDto.Capex.Drilling = costProfile;
        TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.Summary, costProfile);

        steaCaseDto.Capex.OffshoreFacilities = new TimeSeriesCostDto();
        var substructure = steaDbData.Substructures.First(l => l.Id == caseItem.SubstructureLink);

        if (substructure.CostProfileOverride?.Override == true)
        {
            substructure.CostProfileOverride.StartYear += dg4Year;
            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, new TimeSeriesCostDto(substructure.CostProfileOverride));
        }
        else if (substructure.CostProfile != null)
        {
            substructure.CostProfile.StartYear += dg4Year;
            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, new TimeSeriesCostDto(substructure.CostProfile));
        }

        var surf = steaDbData.Surfs.First(l => l.Id == caseItem.SurfLink);

        if (surf.CostProfileOverride?.Override == true)
        {
            surf.CostProfileOverride.StartYear += dg4Year;
            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, new TimeSeriesCostDto(surf.CostProfileOverride));
        }
        else if (surf.CostProfile != null)
        {
            surf.CostProfile.StartYear += dg4Year;
            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, new TimeSeriesCostDto(surf.CostProfile));
        }

        var topside = steaDbData.Topsides.First(l => l.Id == caseItem.TopsideLink);

        if (topside.CostProfileOverride?.Override == true)
        {
            topside.CostProfileOverride.StartYear += dg4Year;
            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, new TimeSeriesCostDto(topside.CostProfileOverride));
        }
        else if (topside.CostProfile != null)
        {
            topside.CostProfile.StartYear += dg4Year;
            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, new TimeSeriesCostDto(topside.CostProfile));
        }

        var transport = steaDbData.Transports.First(l => l.Id == caseItem.TransportLink);

        if (transport.CostProfileOverride?.Override == true)
        {
            transport.CostProfileOverride.StartYear += dg4Year;
            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, new TimeSeriesCostDto(transport.CostProfileOverride));
        }
        else if (transport.CostProfile != null)
        {
            transport.CostProfile.StartYear += dg4Year;
            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, new TimeSeriesCostDto(transport.CostProfile));
        }

        var onshorePowerSupply = steaDbData.OnshorePowerSupplies.First(l => l.Id == caseItem.OnshorePowerSupplyLink);

        if (onshorePowerSupply.CostProfileOverride?.Override == true)
        {
            onshorePowerSupply.CostProfileOverride.StartYear += dg4Year;
            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OnshorePowerSupplyCost, new TimeSeriesCostDto(onshorePowerSupply.CostProfileOverride));
        }
        else if (onshorePowerSupply.CostProfile != null)
        {
            onshorePowerSupply.CostProfile.StartYear += dg4Year;
            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OnshorePowerSupplyCost, new TimeSeriesCostDto(onshorePowerSupply.CostProfile));
        }

        TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.Summary, steaCaseDto.Capex.OffshoreFacilities);
        TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.Summary, steaCaseDto.Capex.OnshorePowerSupplyCost);
    }

    private static void AddProductionSalesAndVolumes(SteaDbData steaDbData, SteaCaseDto steaCaseDto, Case caseItem)
    {
        steaCaseDto.ProductionAndSalesVolumes = new ProductionAndSalesVolumesDto
        {
            TotalAndAnnualOil = new TimeSeriesVolumeDto(),
            TotalAndAnnualSalesGas = new TimeSeriesVolumeDto(),
            Co2Emissions = new TimeSeriesMassDto(),
            AdditionalOil = new TimeSeriesVolumeDto(),
            AdditionalGas = new TimeSeriesVolumeDto()
        };

        var dg4Year = caseItem.DG4Date.Year;

        var drainageStrategy = steaDbData.DrainageStrategies.First(d => d.Id == caseItem.DrainageStrategyLink);
        var startYearsProductionSalesAndVolumes = new List<int>();

        if (drainageStrategy.ProductionProfileOil != null || drainageStrategy.AdditionalProductionProfileOil != null)
        {
            var oilProfile = drainageStrategy.ProductionProfileOil != null
                ? new TimeSeriesCostDto
                {
                    StartYear = drainageStrategy.ProductionProfileOil.StartYear,
                    Values = drainageStrategy.ProductionProfileOil.Values
                }
                : new TimeSeriesCostDto { Values = [], StartYear = 0 };

            var additionalOilProfile = drainageStrategy.AdditionalProductionProfileOil != null
                ? new TimeSeriesCostDto
                {
                    StartYear = drainageStrategy.AdditionalProductionProfileOil.StartYear,
                    Values = drainageStrategy.AdditionalProductionProfileOil.Values
                }
                : new TimeSeriesCostDto { Values = [], StartYear = 0 };

            var mergedOilProfile = TimeSeriesCostMerger.MergeCostProfiles(oilProfile, additionalOilProfile);

            steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualOil = new TimeSeriesVolumeDto
            {
                StartYear = mergedOilProfile.StartYear + dg4Year,
                Values = mergedOilProfile.Values
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualOil.StartYear);
        }

        if (drainageStrategy.NetSalesGasOverride?.Override == true)
        {
            steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = new TimeSeriesVolumeDto
            {
                StartYear = drainageStrategy.NetSalesGasOverride.StartYear + dg4Year,
                Values = drainageStrategy.NetSalesGasOverride.Values
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas.StartYear);
        }
        else if (drainageStrategy.NetSalesGas != null)
        {
            steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = new TimeSeriesVolumeDto
            {
                StartYear = drainageStrategy.NetSalesGas.StartYear + dg4Year,
                Values = drainageStrategy.NetSalesGas.Values
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas.StartYear);
        }

        if (drainageStrategy.ImportedElectricityOverride?.Override == true)
        {
            steaCaseDto.ProductionAndSalesVolumes.ImportedElectricity = new TimeSeriesEnergyDto
            {
                StartYear = drainageStrategy.ImportedElectricityOverride.StartYear + dg4Year,
                Values = drainageStrategy.ImportedElectricityOverride.Values
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.ImportedElectricity.StartYear);
        }
        else if (drainageStrategy.ImportedElectricity != null)
        {
            steaCaseDto.ProductionAndSalesVolumes.ImportedElectricity = new TimeSeriesEnergyDto
            {
                StartYear = drainageStrategy.ImportedElectricity.StartYear + dg4Year,
                Values = drainageStrategy.ImportedElectricity.Values
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.ImportedElectricity.StartYear);
        }

        if (drainageStrategy.Co2EmissionsOverride?.Override == true)
        {
            steaCaseDto.ProductionAndSalesVolumes.Co2Emissions = new TimeSeriesMassDto
            {
                StartYear = drainageStrategy.Co2EmissionsOverride.StartYear + dg4Year,
                Values = drainageStrategy.Co2EmissionsOverride.Values
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.Co2Emissions.StartYear);
        }
        else if (drainageStrategy.Co2Emissions != null)
        {
            steaCaseDto.ProductionAndSalesVolumes.Co2Emissions = new TimeSeriesMassDto
            {
                StartYear = drainageStrategy.Co2Emissions.StartYear + dg4Year,
                Values = drainageStrategy.Co2Emissions.Values
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.Co2Emissions.StartYear);
        }

        if (startYearsProductionSalesAndVolumes.Count > 0)
        {
            steaCaseDto.ProductionAndSalesVolumes.StartYear = startYearsProductionSalesAndVolumes.Min();
        }

        if (drainageStrategy.AdditionalProductionProfileOil != null)
        {
            steaCaseDto.ProductionAndSalesVolumes.AdditionalOil = new TimeSeriesVolumeDto
            {
                StartYear = drainageStrategy.AdditionalProductionProfileOil.StartYear + dg4Year,
                Values = drainageStrategy.AdditionalProductionProfileOil.Values
            };
        }

        if (drainageStrategy.AdditionalProductionProfileGas != null)
        {
            steaCaseDto.ProductionAndSalesVolumes.AdditionalGas = new TimeSeriesVolumeDto
            {
                StartYear = drainageStrategy.AdditionalProductionProfileGas.StartYear + dg4Year,
                Values = drainageStrategy.AdditionalProductionProfileGas.Values
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

        steaCaseDto.Exploration = TimeSeriesCostMerger.MergeCostProfilesList(costProfileDtos);
        steaCaseDto.Exploration.StartYear += caseItem.DG4Date.Year;
    }
}
