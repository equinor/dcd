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
        AddExploration(steaCaseDto, caseItem);
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

        if (caseDto.GetProfileOrNull(ProfileTypes.HistoricCostCostProfile) != null)
        {
            costProfileDtos.Add(ToTimeSeries(caseDto.GetProfile(ProfileTypes.HistoricCostCostProfile)));
        }

        if (caseDto.GetProfileOrNull(ProfileTypes.WellInterventionCostProfileOverride)?.Override == true)
        {
            costProfileDtos.Add(ToTimeSeries(caseDto.GetProfile(ProfileTypes.WellInterventionCostProfileOverride)));
        }
        else if (caseDto.GetProfileOrNull(ProfileTypes.WellInterventionCostProfile) != null)
        {
            costProfileDtos.Add(ToTimeSeries(caseDto.GetProfile(ProfileTypes.WellInterventionCostProfile)));
        }

        if (caseDto.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride)?.Override == true)
        {
            costProfileDtos.Add(ToTimeSeries(caseDto.GetProfile(ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride)));
        }
        else if (caseDto.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfile) != null)
        {
            costProfileDtos.Add(ToTimeSeries(caseDto.GetProfile(ProfileTypes.OffshoreFacilitiesOperationsCostProfile)));
        }

        if (caseDto.GetProfileOrNull(ProfileTypes.OnshoreRelatedOPEXCostProfile) != null)
        {
            costProfileDtos.Add(ToTimeSeries(caseDto.GetProfile(ProfileTypes.OnshoreRelatedOPEXCostProfile)));
        }

        if (caseDto.GetProfileOrNull(ProfileTypes.AdditionalOPEXCostProfile) != null)
        {
            costProfileDtos.Add(ToTimeSeries(caseDto.GetProfile(ProfileTypes.AdditionalOPEXCostProfile)));
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

        if (caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudiesOverride)?.Override == true)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.TotalFeasibilityAndConceptStudiesOverride)));
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudies) != null)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.TotalFeasibilityAndConceptStudies)));
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudiesOverride)?.Override == true)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.TotalFEEDStudiesOverride)));
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudies) != null)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.TotalFEEDStudies)));
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.TotalOtherStudiesCostProfile)?.Values.Length > 0)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.TotalOtherStudiesCostProfile)));
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
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.CessationWellsCostOverride)));
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCost) != null)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.CessationWellsCost)));
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCostOverride)?.Override == true)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.CessationOffshoreFacilitiesCostOverride)));
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCost) != null)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.CessationOffshoreFacilitiesCost)));
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.CessationOnshoreFacilitiesCostProfile) != null)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.CessationOnshoreFacilitiesCostProfile)));
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
            costProfileDtos.Add(ToTimeSeries(wellProject.OilProducerCostProfileOverride));
        }
        else if (wellProject.OilProducerCostProfile != null)
        {
            costProfileDtos.Add(ToTimeSeries(wellProject.OilProducerCostProfile));
        }

        if (wellProject.GasProducerCostProfileOverride?.Override == true)
        {
            costProfileDtos.Add(ToTimeSeries(wellProject.GasProducerCostProfileOverride));
        }
        else if (wellProject.GasProducerCostProfile != null)
        {
            costProfileDtos.Add(ToTimeSeries(wellProject.GasProducerCostProfile));
        }

        if (wellProject.WaterInjectorCostProfileOverride?.Override == true)
        {
            costProfileDtos.Add(ToTimeSeries(wellProject.WaterInjectorCostProfileOverride));
        }
        else if (wellProject.WaterInjectorCostProfile != null)
        {
            costProfileDtos.Add(ToTimeSeries(wellProject.WaterInjectorCostProfile));
        }

        if (wellProject.GasInjectorCostProfileOverride?.Override == true)
        {
            costProfileDtos.Add(ToTimeSeries(wellProject.GasInjectorCostProfileOverride));
        }
        else if (wellProject.GasInjectorCostProfile != null)
        {
            costProfileDtos.Add(ToTimeSeries(wellProject.GasInjectorCostProfile));
        }

        var costProfile = TimeSeriesCostMerger.MergeCostProfilesList(costProfileDtos);
        costProfile.StartYear += dg4Year;

        steaCaseDto.Capex.Drilling = costProfile;
        TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.Summary, costProfile);

        steaCaseDto.Capex.OffshoreFacilities = new TimeSeriesCostDto();

        var substructureCostProfileOverride = caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfileOverride);
        var substructureCostProfile = caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfile);

        if (substructureCostProfileOverride?.Override == true)
        {
            substructureCostProfileOverride.StartYear += dg4Year;
            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, ToTimeSeries(substructureCostProfileOverride));
        }
        else if (substructureCostProfile != null)
        {
            substructureCostProfile.StartYear += dg4Year;
            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, ToTimeSeries(substructureCostProfile));
        }

        var surfCostProfileOverride = caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfileOverride);
        var surfCostProfile = caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfile);

        if (surfCostProfileOverride?.Override == true)
        {
            surfCostProfileOverride.StartYear += dg4Year;
            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, ToTimeSeries(surfCostProfileOverride));
        }
        else if (surfCostProfile != null)
        {
            surfCostProfile.StartYear += dg4Year;
            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, ToTimeSeries(surfCostProfile));
        }

        var topsideCostProfileOverride = caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfileOverride);
        var topsideCostProfile = caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfile);

        if (topsideCostProfileOverride?.Override == true)
        {
            topsideCostProfileOverride.StartYear += dg4Year;
            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, ToTimeSeries(topsideCostProfileOverride));
        }
        else if (topsideCostProfile != null)
        {
            topsideCostProfile.StartYear += dg4Year;
            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, ToTimeSeries(topsideCostProfile));
        }

        var transportCostProfileOverride = caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfileOverride);
        var transportCostProfile = caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfile);

        if (transportCostProfileOverride?.Override == true)
        {
            transportCostProfileOverride.StartYear += dg4Year;
            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, ToTimeSeries(transportCostProfileOverride));
        }
        else if (transportCostProfile != null)
        {
            transportCostProfile.StartYear += dg4Year;
            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, ToTimeSeries(transportCostProfile));
        }

        var onshorePowerSupplyCostProfileOverride = caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfileOverride);
        var onshorePowerSupplyCostProfile = caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfile);

        if (onshorePowerSupplyCostProfileOverride?.Override == true)
        {
            onshorePowerSupplyCostProfileOverride.StartYear += dg4Year;
            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OnshorePowerSupplyCost, ToTimeSeries(onshorePowerSupplyCostProfileOverride));
        }
        else if (onshorePowerSupplyCostProfile != null)
        {
            onshorePowerSupplyCostProfile.StartYear += dg4Year;
            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OnshorePowerSupplyCost, ToTimeSeries(onshorePowerSupplyCostProfile));
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

    private static void AddExploration(SteaCaseDto steaCaseDto, Case caseItem)
    {
        steaCaseDto.Exploration = new TimeSeriesCostDto();

        var costProfileDtos = new List<TimeSeriesCostDto>();
        if (caseItem.GetProfileOrNull(ProfileTypes.ExplorationWellCostProfile)?.Values.Length > 0)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.ExplorationWellCostProfile)));
        }
        if (caseItem.GetProfileOrNull(ProfileTypes.AppraisalWellCostProfile)?.Values.Length > 0)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.AppraisalWellCostProfile)));
        }
        if (caseItem.GetProfileOrNull(ProfileTypes.SidetrackCostProfile)?.Values.Length > 0)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.SidetrackCostProfile)));
        }
        if (caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCostOverride)?.Override == true)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.GAndGAdminCostOverride)));
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCost) != null)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.GAndGAdminCost)));
        }
        if (caseItem.GetProfileOrNull(ProfileTypes.SeismicAcquisitionAndProcessing)?.Values.Length > 0)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.SeismicAcquisitionAndProcessing)));
        }
        if (caseItem.GetProfileOrNull(ProfileTypes.CountryOfficeCost)?.Values.Length > 0)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.CountryOfficeCost)));
        }

        steaCaseDto.Exploration = TimeSeriesCostMerger.MergeCostProfilesList(costProfileDtos);
        steaCaseDto.Exploration.StartYear += caseItem.DG4Date.Year;
    }

    private static TimeSeriesCostDto ToTimeSeries(TimeSeriesProfile timeSeriesProfile) => new()
    {
        Id = timeSeriesProfile.Id,
        StartYear = timeSeriesProfile.StartYear,
        Values = timeSeriesProfile.Values
    };

    private static TimeSeriesCostDto ToTimeSeries(TimeSeriesCost timeSeriesCost) => new()
    {
        StartYear = timeSeriesCost.StartYear,
        Values = timeSeriesCost.Values ?? []
    };
}
