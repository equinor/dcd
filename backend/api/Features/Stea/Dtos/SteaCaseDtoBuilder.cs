using api.Features.Profiles;
using api.Features.Profiles.Dtos;
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
        var costProfileDtos = new List<TimeSeriesCostDto>
        {
            new(caseDto.GetProfileOrNull(ProfileTypes.HistoricCostCostProfile)),
            new(caseDto.GetProfileOrNull(ProfileTypes.OnshoreRelatedOPEXCostProfile)),
            new(caseDto.GetProfileOrNull(ProfileTypes.AdditionalOPEXCostProfile)),

            caseDto.GetProfileOrNull(ProfileTypes.WellInterventionCostProfileOverride)?.Override == true
                ? new TimeSeriesCostDto(caseDto.GetProfileOrNull(ProfileTypes.WellInterventionCostProfileOverride))
                : new TimeSeriesCostDto(caseDto.GetProfileOrNull(ProfileTypes.WellInterventionCostProfile)),

            caseDto.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride)?.Override == true
                ? new TimeSeriesCostDto(caseDto.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride))
                : new TimeSeriesCostDto(caseDto.GetProfileOrNull(ProfileTypes.OffshoreFacilitiesOperationsCostProfile))
        };

        var dto = TimeSeriesCostMerger.MergeCostProfilesList(costProfileDtos);
        dto.StartYear += caseDto.DG4Date.Year;

        steaCaseDto.OpexCostProfile = dto;
    }

    private static void AddStudyCost(SteaCaseDto steaCaseDto, Case caseItem)
    {
        var costProfileDtos = new List<TimeSeriesCostDto>
        {
            new(caseItem.GetProfileOrNull(ProfileTypes.TotalOtherStudiesCostProfile)),

            caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudiesOverride)?.Override == true
                ? new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudiesOverride))
                : new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.TotalFeasibilityAndConceptStudies)),

            caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudiesOverride)?.Override == true
                ? new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudiesOverride))
                : new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.TotalFEEDStudies))
        };

        var dto = TimeSeriesCostMerger.MergeCostProfilesList(costProfileDtos);
        dto.StartYear += caseItem.DG4Date.Year;

        steaCaseDto.StudyCostProfile = dto;
    }

    private static void AddCessationCost(SteaCaseDto steaCaseDto, Case caseItem)
    {
        var costProfileDtos = new List<TimeSeriesCostDto>
        {
            new(caseItem.GetProfileOrNull(ProfileTypes.CessationOnshoreFacilitiesCostProfile)),

            caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCostOverride)?.Override == true
                ? new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCostOverride))
                : new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.CessationWellsCost)),

            caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCostOverride)?.Override == true
                ? new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCostOverride))
                : new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.CessationOffshoreFacilitiesCost))
        };

        var dto = TimeSeriesCostMerger.MergeCostProfilesList(costProfileDtos);
        dto.StartYear += caseItem.DG4Date.Year;

        steaCaseDto.Capex.CessationCost = dto;
    }

    private static void AddCapex(SteaCaseDto steaCaseDto, Case caseItem)
    {
        steaCaseDto.Capex = new CapexDto
        {
            Drilling = new TimeSeriesCostDto()
        };

        var costProfileDtos = new List<TimeSeriesCostDto>
        {
            caseItem.GetProfileOrNull(ProfileTypes.OilProducerCostProfileOverride)?.Override == true
                ? new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.OilProducerCostProfileOverride))
                : new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.OilProducerCostProfile)),

            caseItem.GetProfileOrNull(ProfileTypes.GasProducerCostProfileOverride)?.Override == true
                ? new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.GasProducerCostProfileOverride))
                : new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.GasProducerCostProfile)),

            caseItem.GetProfileOrNull(ProfileTypes.WaterInjectorCostProfileOverride)?.Override == true
                ? new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.WaterInjectorCostProfileOverride))
                : new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.WaterInjectorCostProfile)),

            caseItem.GetProfileOrNull(ProfileTypes.GasInjectorCostProfileOverride)?.Override == true
                ? new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.GasInjectorCostProfileOverride))
                : new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.GasInjectorCostProfile)),

            caseItem.GetProfileOrNull(ProfileTypes.DevelopmentRigUpgradingCostProfileOverride)?.Override == true
                ? new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.DevelopmentRigUpgradingCostProfileOverride))
                : new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.DevelopmentRigUpgradingCostProfile)),

            caseItem.GetProfileOrNull(ProfileTypes.DevelopmentRigMobDemobOverride)?.Override == true
                ? new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.DevelopmentRigMobDemobOverride))
                : new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.DevelopmentRigMobDemob))
        };

        var costProfile = TimeSeriesCostMerger.MergeCostProfilesList(costProfileDtos);
        costProfile.StartYear += caseItem.DG4Date.Year;

        steaCaseDto.Capex.Drilling = costProfile;
        TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.Summary, costProfile);

        steaCaseDto.Capex.OffshoreFacilities = new TimeSeriesCostDto();

        if (caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfileOverride)?.Override == true)
        {
            var dto = new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.SubstructureCostProfileOverride));
            dto.StartYear += caseItem.DG4Date.Year;

            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, dto);
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.SubstructureCostProfile) != null)
        {
            var dto = new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.SubstructureCostProfile));
            dto.StartYear += caseItem.DG4Date.Year;

            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, dto);
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfileOverride)?.Override == true)
        {
            var dto = new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.SurfCostProfileOverride));
            dto.StartYear += caseItem.DG4Date.Year;

            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, dto);
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.SurfCostProfile) != null)
        {
            var dto = new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.SurfCostProfile));
            dto.StartYear += caseItem.DG4Date.Year;

            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, dto);
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfileOverride)?.Override == true)
        {
            var dto = new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.TopsideCostProfileOverride));
            dto.StartYear += caseItem.DG4Date.Year;

            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, dto);
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.TopsideCostProfile) != null)
        {
            var dto = new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.TopsideCostProfile));
            dto.StartYear += caseItem.DG4Date.Year;

            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, dto);
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfileOverride)?.Override == true)
        {
            var dto = new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.TransportCostProfileOverride));
            dto.StartYear += caseItem.DG4Date.Year;

            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, dto);
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.TransportCostProfile) != null)
        {
            var dto = new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.TransportCostProfile));
            dto.StartYear += caseItem.DG4Date.Year;

            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, dto);
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfileOverride)?.Override == true)
        {
            var dto = new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.OnshorePowerSupplyCostProfileOverride));
            dto.StartYear += caseItem.DG4Date.Year;

            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OnshorePowerSupplyCost, dto);
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.OnshorePowerSupplyCostProfile) != null)
        {
            var dto = new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.OnshorePowerSupplyCostProfile));
            dto.StartYear += caseItem.DG4Date.Year;

            TimeSeriesCostMerger.AddValues(steaCaseDto.Capex.OnshorePowerSupplyCost, dto);
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

        var startYearsProductionSalesAndVolumes = new List<int>();

        if (caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil) != null ||
            caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil) != null)
        {
            var oilProfile = new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil));
            var additionalOilProfile = new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil));

            var dto = TimeSeriesCostMerger.MergeCostProfiles(oilProfile, additionalOilProfile);
            dto.StartYear = caseItem.DG4Date.Year;

            steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualOil = dto;
            startYearsProductionSalesAndVolumes.Add(dto.StartYear);
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.NetSalesGasOverride)?.Override == true)
        {
            var dto = new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.NetSalesGasOverride));
            dto.StartYear += caseItem.DG4Date.Year;

            steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = dto;
            startYearsProductionSalesAndVolumes.Add(dto.StartYear);
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.NetSalesGas) != null)
        {
            var dto = new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.NetSalesGas));
            dto.StartYear += caseItem.DG4Date.Year;

            steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = dto;
            startYearsProductionSalesAndVolumes.Add(dto.StartYear);
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.ImportedElectricityOverride)?.Override == true)
        {
            var dto = new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.ImportedElectricityOverride));
            dto.StartYear += caseItem.DG4Date.Year;

            steaCaseDto.ProductionAndSalesVolumes.ImportedElectricity = dto;
            startYearsProductionSalesAndVolumes.Add(dto.StartYear);
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.ImportedElectricity) != null)
        {
            var dto = new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.ImportedElectricity));
            dto.StartYear += caseItem.DG4Date.Year;

            steaCaseDto.ProductionAndSalesVolumes.ImportedElectricity = dto;
            startYearsProductionSalesAndVolumes.Add(dto.StartYear);
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.Co2EmissionsOverride)?.Override == true)
        {
            var dto = new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.Co2EmissionsOverride));
            dto.StartYear += caseItem.DG4Date.Year;

            steaCaseDto.ProductionAndSalesVolumes.Co2Emissions = dto;
            startYearsProductionSalesAndVolumes.Add(dto.StartYear);
        }
        else if (caseItem.GetProfileOrNull(ProfileTypes.Co2Emissions) != null)
        {
            var dto = new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.Co2Emissions));
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
            var dto = new TimeSeriesCostDto(caseItem.GetProfile(ProfileTypes.AdditionalProductionProfileOil));
            dto.StartYear += caseItem.DG4Date.Year;

            steaCaseDto.ProductionAndSalesVolumes.AdditionalOil = dto;
        }

        var additionalProductionProfileGasProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas);

        if (additionalProductionProfileGasProfile != null)
        {
            var dto = new TimeSeriesCostDto(additionalProductionProfileGasProfile);
            dto.StartYear += caseItem.DG4Date.Year;

            steaCaseDto.ProductionAndSalesVolumes.AdditionalGas = dto;
        }
    }

    private static void AddExploration(SteaCaseDto steaCaseDto, Case caseItem)
    {
        steaCaseDto.Exploration = new TimeSeriesCostDto();

        var costProfileDtos = new List<TimeSeriesCostDto>
        {
            new(caseItem.GetProfileOrNull(ProfileTypes.SidetrackCostProfile)),
            new(caseItem.GetProfileOrNull(ProfileTypes.ProjectSpecificDrillingCostProfile)),
            new(caseItem.GetProfileOrNull(ProfileTypes.SeismicAcquisitionAndProcessing)),
            new(caseItem.GetProfileOrNull(ProfileTypes.CountryOfficeCost)),

            caseItem.GetProfileOrNull(ProfileTypes.ExplorationWellCostProfile)?.Values.Length > 0
                ? new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.ExplorationWellCostProfile))
                : new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.AppraisalWellCostProfile)),

            caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCostOverride)?.Override == true
                ? new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCostOverride))
                : new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.GAndGAdminCost)),

            caseItem.GetProfileOrNull(ProfileTypes.ExplorationRigUpgradingCostProfileOverride)?.Override == true
                ? new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.ExplorationRigUpgradingCostProfileOverride))
                : new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.ExplorationRigUpgradingCostProfile)),

            caseItem.GetProfileOrNull(ProfileTypes.ExplorationRigMobDemobOverride)?.Override == true
                ? new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.ExplorationRigMobDemobOverride))
                : new TimeSeriesCostDto(caseItem.GetProfileOrNull(ProfileTypes.ExplorationRigMobDemob))
        };

        steaCaseDto.Exploration = TimeSeriesCostMerger.MergeCostProfilesList(costProfileDtos);
        steaCaseDto.Exploration.StartYear += caseItem.DG4Date.Year;
    }
}
