using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Stea.Dtos;

public static class SteaCaseDtoBuilder
{
    public static SteaCaseDto Build(Case caseItem)
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
        var costProfileDtos = new List<TimeSeries>
        {
            new(caseDto.GetProfileOrNull(ProfileTypes.HistoricCostCostProfile)),
            new(caseDto.GetProfileOrNull(ProfileTypes.OnshoreRelatedOPEXCostProfile)),
            new(caseDto.GetProfileOrNull(ProfileTypes.AdditionalOPEXCostProfile)),

            caseDto.GetProfileOrNull(ProfileTypes.WellInterventionCostProfileOverride)?.Override == true
                ? new TimeSeries(caseDto.GetProfileOrNull(ProfileTypes.WellInterventionCostProfileOverride))
                : new TimeSeries(caseDto.GetProfileOrNull(ProfileTypes.WellInterventionCostProfile)),

            caseDto.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride)?.Override == true
                ? new TimeSeries(caseDto.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride))
                : new TimeSeries(caseDto.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfile))
        };

        var dto = TimeSeriesMerger.MergeTimeSeries(costProfileDtos);
        dto.StartYear += caseDto.DG4Date.Year;

        steaCaseDto.OpexCostProfile = dto;
    }

    private static void AddStudyCost(SteaCaseDto steaCaseDto, Case caseItem)
    {
        var costProfileDtos = new List<TimeSeries>
        {
            new(caseItem.GetProfileOrNull(ProfileTypes.TotalOtherStudiesCostProfile)),

            caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudiesOverride)?.Override == true
                ? new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudiesOverride))
                : new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudies)),

            caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudiesOverride)?.Override == true
                ? new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudiesOverride))
                : new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudies))
        };

        var dto = TimeSeriesMerger.MergeTimeSeries(costProfileDtos);
        dto.StartYear += caseItem.DG4Date.Year;

        steaCaseDto.StudyCostProfile = dto;
    }

    private static void AddCessationCost(SteaCaseDto steaCaseDto, Case caseItem)
    {
        var costProfileDtos = new List<TimeSeries>
        {
            new(caseItem.GetProfileOrNull(ProfileTypes.CessationOnshoreFacilitiesCostProfile)),

            caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCostOverride)?.Override == true
                ? new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCostOverride))
                : new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCost)),

            caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCostOverride)?.Override == true
                ? new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCostOverride))
                : new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCost))
        };

        var dto = TimeSeriesMerger.MergeTimeSeries(costProfileDtos);
        dto.StartYear += caseItem.DG4Date.Year;

        steaCaseDto.Capex.CessationCost = dto;
    }

    private static void AddCapex(SteaCaseDto steaCaseDto, Case caseItem)
    {
        steaCaseDto.Capex = new CapexDto
        {
            Drilling = new TimeSeries()
        };

        var costProfileDtos = new List<TimeSeries>
        {
            caseItem.GetProfileOrNull(ProfileTypes.OilProducerCostProfileOverride)?.Override == true
                ? new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.OilProducerCostProfileOverride))
                : new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.OilProducerCostProfile)),

            caseItem.GetProfileOrNull(ProfileTypes.GasProducerCostProfileOverride)?.Override == true
                ? new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.GasProducerCostProfileOverride))
                : new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.GasProducerCostProfile)),

            caseItem.GetProfileOrNull(ProfileTypes.WaterInjectorCostProfileOverride)?.Override == true
                ? new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.WaterInjectorCostProfileOverride))
                : new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.WaterInjectorCostProfile)),

            caseItem.GetProfileOrNull(ProfileTypes.GasInjectorCostProfileOverride)?.Override == true
                ? new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.GasInjectorCostProfileOverride))
                : new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.GasInjectorCostProfile)),

            caseItem.GetProfileOrNull(ProfileTypes.DevelopmentRigUpgradingCostProfileOverride)?.Override == true
                ? new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.DevelopmentRigUpgradingCostProfileOverride))
                : new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.DevelopmentRigUpgradingCostProfile)),

            caseItem.GetProfileOrNull(ProfileTypes.DevelopmentRigMobDemobOverride)?.Override == true
                ? new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.DevelopmentRigMobDemobOverride))
                : new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.DevelopmentRigMobDemob))
        };

        var costProfile = TimeSeriesMerger.MergeTimeSeries(costProfileDtos);
        costProfile.StartYear += caseItem.DG4Date.Year;

        steaCaseDto.Capex.Drilling = costProfile;
        TimeSeriesMerger.AddValues(steaCaseDto.Capex.Summary, costProfile);

        steaCaseDto.Capex.OffshoreFacilities = new TimeSeries();

        if (caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfileOverride)?.Override == true)
        {
            var dto = new TimeSeries(caseItem.GetProfile(ProfileTypes.SubstructureCostProfileOverride));
            dto.StartYear += caseItem.DG4Date.Year;

            TimeSeriesMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, dto);
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfile) != null)
        {
            var dto = new TimeSeries(caseItem.GetProfile(ProfileTypes.SubstructureCostProfile));
            dto.StartYear += caseItem.DG4Date.Year;

            TimeSeriesMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, dto);
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfileOverride)?.Override == true)
        {
            var dto = new TimeSeries(caseItem.GetProfile(ProfileTypes.SurfCostProfileOverride));
            dto.StartYear += caseItem.DG4Date.Year;

            TimeSeriesMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, dto);
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfile) != null)
        {
            var dto = new TimeSeries(caseItem.GetProfile(ProfileTypes.SurfCostProfile));
            dto.StartYear += caseItem.DG4Date.Year;

            TimeSeriesMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, dto);
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfileOverride)?.Override == true)
        {
            var dto = new TimeSeries(caseItem.GetProfile(ProfileTypes.TopsideCostProfileOverride));
            dto.StartYear += caseItem.DG4Date.Year;

            TimeSeriesMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, dto);
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfile) != null)
        {
            var dto = new TimeSeries(caseItem.GetProfile(ProfileTypes.TopsideCostProfile));
            dto.StartYear += caseItem.DG4Date.Year;

            TimeSeriesMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, dto);
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfileOverride)?.Override == true)
        {
            var dto = new TimeSeries(caseItem.GetProfile(ProfileTypes.TransportCostProfileOverride));
            dto.StartYear += caseItem.DG4Date.Year;

            TimeSeriesMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, dto);
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfile) != null)
        {
            var dto = new TimeSeries(caseItem.GetProfile(ProfileTypes.TransportCostProfile));
            dto.StartYear += caseItem.DG4Date.Year;

            TimeSeriesMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, dto);
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfileOverride)?.Override == true)
        {
            var dto = new TimeSeries(caseItem.GetProfile(ProfileTypes.OnshorePowerSupplyCostProfileOverride));
            dto.StartYear += caseItem.DG4Date.Year;

            TimeSeriesMerger.AddValues(steaCaseDto.Capex.OnshorePowerSupplyCost, dto);
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfile) != null)
        {
            var dto = new TimeSeries(caseItem.GetProfile(ProfileTypes.OnshorePowerSupplyCostProfile));
            dto.StartYear += caseItem.DG4Date.Year;

            TimeSeriesMerger.AddValues(steaCaseDto.Capex.OnshorePowerSupplyCost, dto);
        }

        TimeSeriesMerger.AddValues(steaCaseDto.Capex.Summary, steaCaseDto.Capex.OffshoreFacilities);
        TimeSeriesMerger.AddValues(steaCaseDto.Capex.Summary, steaCaseDto.Capex.OnshorePowerSupplyCost);
    }

    private static void AddProductionSalesAndVolumes(SteaCaseDto steaCaseDto, Case caseItem)
    {
        steaCaseDto.ProductionAndSalesVolumes = new ProductionAndSalesVolumesDto
        {
            TotalAndAnnualOil = new TimeSeries(),
            TotalAndAnnualSalesGas = new TimeSeries(),
            Co2Emissions = new TimeSeries(),
            AdditionalOil = new TimeSeries(),
            AdditionalGas = new TimeSeries()
        };

        var startYearsProductionSalesAndVolumes = new List<int>();

        if (caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil) != null ||
            caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil) != null)
        {
            var oilProfile = new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil));
            var additionalOilProfile = new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil));

            var dto = TimeSeriesMerger.MergeTimeSeries(oilProfile, additionalOilProfile);
            dto.StartYear = caseItem.DG4Date.Year;

            steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualOil = dto;
            startYearsProductionSalesAndVolumes.Add(dto.StartYear);
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.NetSalesGasOverride)?.Override == true)
        {
            var dto = new TimeSeries(caseItem.GetProfile(ProfileTypes.NetSalesGasOverride));
            dto.StartYear += caseItem.DG4Date.Year;

            steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = dto;
            startYearsProductionSalesAndVolumes.Add(dto.StartYear);
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.NetSalesGas) != null)
        {
            var dto = new TimeSeries(caseItem.GetProfile(ProfileTypes.NetSalesGas));
            dto.StartYear += caseItem.DG4Date.Year;

            steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = dto;
            startYearsProductionSalesAndVolumes.Add(dto.StartYear);
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.ImportedElectricityOverride)?.Override == true)
        {
            var dto = new TimeSeries(caseItem.GetProfile(ProfileTypes.ImportedElectricityOverride));
            dto.StartYear += caseItem.DG4Date.Year;

            steaCaseDto.ProductionAndSalesVolumes.ImportedElectricity = dto;
            startYearsProductionSalesAndVolumes.Add(dto.StartYear);
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.ImportedElectricity) != null)
        {
            var dto = new TimeSeries(caseItem.GetProfile(ProfileTypes.ImportedElectricity));
            dto.StartYear += caseItem.DG4Date.Year;

            steaCaseDto.ProductionAndSalesVolumes.ImportedElectricity = dto;
            startYearsProductionSalesAndVolumes.Add(dto.StartYear);
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.Co2EmissionsOverride)?.Override == true)
        {
            var dto = new TimeSeries(caseItem.GetProfile(ProfileTypes.Co2EmissionsOverride));
            dto.StartYear += caseItem.DG4Date.Year;

            steaCaseDto.ProductionAndSalesVolumes.Co2Emissions = dto;
            startYearsProductionSalesAndVolumes.Add(dto.StartYear);
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.Co2Emissions) != null)
        {
            var dto = new TimeSeries(caseItem.GetProfile(ProfileTypes.Co2Emissions));
            dto.StartYear += caseItem.DG4Date.Year;

            steaCaseDto.ProductionAndSalesVolumes.Co2Emissions = dto;
            startYearsProductionSalesAndVolumes.Add(dto.StartYear);
        }

        if (startYearsProductionSalesAndVolumes.Count > 0)
        {
            steaCaseDto.ProductionAndSalesVolumes.StartYear = startYearsProductionSalesAndVolumes.Min();
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil) != null)
        {
            var dto = new TimeSeries(caseItem.GetProfile(ProfileTypes.AdditionalProductionProfileOil));
            dto.StartYear += caseItem.DG4Date.Year;

            steaCaseDto.ProductionAndSalesVolumes.AdditionalOil = dto;
        }

        var additionalProductionProfileGasProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas);

        if (additionalProductionProfileGasProfile != null)
        {
            var dto = new TimeSeries(additionalProductionProfileGasProfile);
            dto.StartYear += caseItem.DG4Date.Year;

            steaCaseDto.ProductionAndSalesVolumes.AdditionalGas = dto;
        }
    }

    private static void AddExploration(SteaCaseDto steaCaseDto, Case caseItem)
    {
        steaCaseDto.Exploration = new TimeSeries();

        var costProfileDtos = new List<TimeSeries>
        {
            new(caseItem.GetProfileOrNull(ProfileTypes.SidetrackCostProfile)),
            new(caseItem.GetProfileOrNull(ProfileTypes.ProjectSpecificDrillingCostProfile)),
            new(caseItem.GetProfileOrNull(ProfileTypes.SeismicAcquisitionAndProcessing)),
            new(caseItem.GetProfileOrNull(ProfileTypes.CountryOfficeCost)),

            caseItem.GetProfileOrNull(ProfileTypes.ExplorationWellCostProfile)?.Values.Length > 0
                ? new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.ExplorationWellCostProfile))
                : new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.AppraisalWellCostProfile)),

            caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCostOverride)?.Override == true
                ? new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCostOverride))
                : new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCost)),

            caseItem.GetProfileOrNull(ProfileTypes.ExplorationRigUpgradingCostProfileOverride)?.Override == true
                ? new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.ExplorationRigUpgradingCostProfileOverride))
                : new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.ExplorationRigUpgradingCostProfile)),

            caseItem.GetProfileOrNull(ProfileTypes.ExplorationRigMobDemobOverride)?.Override == true
                ? new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.ExplorationRigMobDemobOverride))
                : new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.ExplorationRigMobDemob))
        };

        steaCaseDto.Exploration = TimeSeriesMerger.MergeTimeSeries(costProfileDtos);
        steaCaseDto.Exploration.StartYear += caseItem.DG4Date.Year;
    }
}
