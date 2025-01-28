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
        AddCapex(steaCaseDto, caseItem);
        AddCessationCost(steaCaseDto, caseItem);
        AddExploration(steaCaseDto, caseItem);
        AddProductionSalesAndVolumes(steaCaseDto, caseItem);

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

    private static void AddCapex(SteaCaseDto steaCaseDto, Case caseItem)
    {
        steaCaseDto.Capex = new CapexDto
        {
            Drilling = new TimeSeriesCostDto()
        };

        var dg4Year = caseItem.DG4Date.Year;

        var costProfileDtos = new List<TimeSeriesCostDto>();

        if (caseItem.GetProfileOrNull(ProfileTypes.OilProducerCostProfileOverride)?.Override == true)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.OilProducerCostProfileOverride)));
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.OilProducerCostProfile) != null)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.OilProducerCostProfile)));
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.GasProducerCostProfileOverride)?.Override == true)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.GasProducerCostProfileOverride)));
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.GasProducerCostProfile) != null)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.GasProducerCostProfile)));
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.WaterInjectorCostProfileOverride)?.Override == true)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.WaterInjectorCostProfileOverride)));
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.WaterInjectorCostProfile) != null)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.WaterInjectorCostProfile)));
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.GasInjectorCostProfileOverride)?.Override == true)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.GasInjectorCostProfileOverride)));
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.GasInjectorCostProfile) != null)
        {
            costProfileDtos.Add(ToTimeSeries(caseItem.GetProfile(ProfileTypes.GasInjectorCostProfile)));
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

    private static void AddProductionSalesAndVolumes(SteaCaseDto steaCaseDto, Case caseItem)
    {
        steaCaseDto.ProductionAndSalesVolumes = new ProductionAndSalesVolumesDto
        {
            TotalAndAnnualOil = new TimeSeriesCostDto(),
            TotalAndAnnualSalesGas = new TimeSeriesCostDto(),
            Co2Emissions = new TimeSeriesCostDto(),
            AdditionalOil = new TimeSeriesCostDto(),
            AdditionalGas = new TimeSeriesCostDto()
        };

        var dg4Year = caseItem.DG4Date.Year;

        var startYearsProductionSalesAndVolumes = new List<int>();

        var productionProfileOilProfile = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil);
        var additionalProductionProfileOilProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil);

        if (productionProfileOilProfile != null || additionalProductionProfileOilProfile != null)
        {
            var oilProfile = productionProfileOilProfile != null
                ? new TimeSeriesCostDto
                {
                    StartYear = productionProfileOilProfile.StartYear,
                    Values = productionProfileOilProfile.Values
                }
                : new TimeSeriesCostDto { Values = [], StartYear = 0 };

            var additionalOilProfile = additionalProductionProfileOilProfile != null
                ? new TimeSeriesCostDto
                {
                    StartYear = additionalProductionProfileOilProfile.StartYear,
                    Values = additionalProductionProfileOilProfile.Values
                }
                : new TimeSeriesCostDto { Values = [], StartYear = 0 };

            var mergedOilProfile = TimeSeriesCostMerger.MergeCostProfiles(oilProfile, additionalOilProfile);

            steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualOil = new TimeSeriesCostDto
            {
                StartYear = mergedOilProfile.StartYear + dg4Year,
                Values = mergedOilProfile.Values
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualOil.StartYear);
        }

        var netSalesGasOverrideProfile = caseItem.GetProfileOrNull(ProfileTypes.NetSalesGasOverride);
        var netSalesGasProfile = caseItem.GetProfileOrNull(ProfileTypes.NetSalesGas);

        if (netSalesGasOverrideProfile?.Override == true)
        {
            steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = new TimeSeriesCostDto
            {
                StartYear = netSalesGasOverrideProfile.StartYear + dg4Year,
                Values = netSalesGasOverrideProfile.Values
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas.StartYear);
        }
        else if (netSalesGasProfile != null)
        {
            steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = new TimeSeriesCostDto
            {
                StartYear = netSalesGasProfile.StartYear + dg4Year,
                Values = netSalesGasProfile.Values
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas.StartYear);
        }

        var importedElectricityOverrideProfile = caseItem.GetProfileOrNull(ProfileTypes.ImportedElectricityOverride);
        var importedElectricityProfile = caseItem.GetProfileOrNull(ProfileTypes.ImportedElectricity);

        if (importedElectricityOverrideProfile?.Override == true)
        {
            steaCaseDto.ProductionAndSalesVolumes.ImportedElectricity = new TimeSeriesCostDto
            {
                StartYear = importedElectricityOverrideProfile.StartYear + dg4Year,
                Values = importedElectricityOverrideProfile.Values
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.ImportedElectricity.StartYear);
        }
        else if (importedElectricityProfile != null)
        {
            steaCaseDto.ProductionAndSalesVolumes.ImportedElectricity = new TimeSeriesCostDto
            {
                StartYear = importedElectricityProfile.StartYear + dg4Year,
                Values = importedElectricityProfile.Values
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.ImportedElectricity.StartYear);
        }

        var co2EmissionsOverrideProfile = caseItem.GetProfileOrNull(ProfileTypes.Co2EmissionsOverride);
        var co2EmissionsProfile = caseItem.GetProfileOrNull(ProfileTypes.Co2Emissions);

        if (co2EmissionsOverrideProfile?.Override == true)
        {
            steaCaseDto.ProductionAndSalesVolumes.Co2Emissions = new TimeSeriesCostDto
            {
                StartYear = co2EmissionsOverrideProfile.StartYear + dg4Year,
                Values = co2EmissionsOverrideProfile.Values
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.Co2Emissions.StartYear);
        }
        else if (co2EmissionsProfile != null)
        {
            steaCaseDto.ProductionAndSalesVolumes.Co2Emissions = new TimeSeriesCostDto
            {
                StartYear = co2EmissionsProfile.StartYear + dg4Year,
                Values = co2EmissionsProfile.Values
            };
            startYearsProductionSalesAndVolumes.Add(steaCaseDto.ProductionAndSalesVolumes.Co2Emissions.StartYear);
        }

        if (startYearsProductionSalesAndVolumes.Count > 0)
        {
            steaCaseDto.ProductionAndSalesVolumes.StartYear = startYearsProductionSalesAndVolumes.Min();
        }

        if (additionalProductionProfileOilProfile != null)
        {
            steaCaseDto.ProductionAndSalesVolumes.AdditionalOil = new TimeSeriesCostDto
            {
                StartYear = additionalProductionProfileOilProfile.StartYear + dg4Year,
                Values = additionalProductionProfileOilProfile.Values
            };
        }

        var additionalProductionProfileGasProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas);

        if (additionalProductionProfileGasProfile != null)
        {
            steaCaseDto.ProductionAndSalesVolumes.AdditionalGas = new TimeSeriesCostDto
            {
                StartYear = additionalProductionProfileGasProfile.StartYear + dg4Year,
                Values = additionalProductionProfileGasProfile.Values
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
}
