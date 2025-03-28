using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;
using api.Models.Enums;

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
            new(caseDto.GetProfileOrNull(ProfileTypes.OnshoreRelatedOpexCostProfile)),
            new(caseDto.GetProfileOrNull(ProfileTypes.AdditionalOpexCostProfile)),

            new(caseDto.GetOverrideProfileOrProfile(ProfileTypes.WellInterventionCostProfile)),

            new(caseDto.GetOverrideProfileOrProfile(ProfileTypes.OffshoreFacilitiesOperationsCostProfile))
        };

        var dto = TimeSeriesMerger.MergeTimeSeries(costProfileDtos);
        dto.StartYear += caseDto.Dg4Date.Year;

        steaCaseDto.OpexCostProfile = dto;
    }

    private static void AddStudyCost(SteaCaseDto steaCaseDto, Case caseItem)
    {
        var costProfileDtos = new List<TimeSeries>
        {
            new(caseItem.GetProfileOrNull(ProfileTypes.TotalOtherStudiesCostProfile)),

            new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.TotalFeasibilityAndConceptStudies)),

            new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.TotalFeedStudies))
        };

        var dto = TimeSeriesMerger.MergeTimeSeries(costProfileDtos);
        dto.StartYear += caseItem.Dg4Date.Year;

        steaCaseDto.StudyCostProfile = dto;
    }

    private static void AddCessationCost(SteaCaseDto steaCaseDto, Case caseItem)
    {
        var costProfileDtos = new List<TimeSeries>
        {
            new(caseItem.GetProfileOrNull(ProfileTypes.CessationOnshoreFacilitiesCostProfile)),

            new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.CessationWellsCost)),

            new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.CessationOffshoreFacilitiesCost))
        };

        var dto = TimeSeriesMerger.MergeTimeSeries(costProfileDtos);
        dto.StartYear += caseItem.Dg4Date.Year;

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
            new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.OilProducerCostProfile)),

            new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.GasProducerCostProfile)),

            new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.WaterInjectorCostProfile)),

            new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.GasInjectorCostProfile)),

            new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.DevelopmentRigUpgradingCostProfile)),

            new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.DevelopmentRigMobDemob))
        };

        costProfileDtos.AddRange(caseItem.Campaigns
                                     .Where(x => x.CampaignType == CampaignType.DevelopmentCampaign)
                                     .Select(campaign => new TimeSeries
                                     {
                                         StartYear = campaign.RigUpgradingCostStartYear,
                                         Values = campaign.RigUpgradingCostValues.Select(x => x * campaign.RigUpgradingCost).ToArray()
                                     }));

        costProfileDtos.AddRange(caseItem.Campaigns
                                     .Where(x => x.CampaignType == CampaignType.DevelopmentCampaign)
                                     .Select(campaign => new TimeSeries
                                     {
                                         StartYear = campaign.RigMobDemobCostStartYear,
                                         Values = campaign.RigMobDemobCostValues.Select(x => x * campaign.RigMobDemobCost).ToArray()
                                     }));

        var costProfile = TimeSeriesMerger.MergeTimeSeries(costProfileDtos);
        costProfile.StartYear += caseItem.Dg4Date.Year;

        steaCaseDto.Capex.Drilling = costProfile;
        TimeSeriesMerger.AddValues(steaCaseDto.Capex.Summary, costProfile);

        steaCaseDto.Capex.OffshoreFacilities = new TimeSeries();

        var substructureCostProfileDto = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.SubstructureCostProfile));

        substructureCostProfileDto.StartYear += caseItem.Dg4Date.Year;
        TimeSeriesMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, substructureCostProfileDto);

        var surfCostProfileDto = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.SurfCostProfile));

        surfCostProfileDto.StartYear += caseItem.Dg4Date.Year;
        TimeSeriesMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, surfCostProfileDto);

        var topsideCostProfileDto = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.TopsideCostProfile));

        topsideCostProfileDto.StartYear += caseItem.Dg4Date.Year;
        TimeSeriesMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, topsideCostProfileDto);

        var transportCostProfile = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.TransportCostProfile));

        transportCostProfile.StartYear += caseItem.Dg4Date.Year;
        TimeSeriesMerger.AddValues(steaCaseDto.Capex.OffshoreFacilities, transportCostProfile);

        var onshorePowerSupplyCostProfileDto = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.OnshorePowerSupplyCostProfile));

        onshorePowerSupplyCostProfileDto.StartYear += caseItem.Dg4Date.Year;
        TimeSeriesMerger.AddValues(steaCaseDto.Capex.OnshorePowerSupplyCost, onshorePowerSupplyCostProfileDto);

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
            var dto = TimeSeriesMerger.MergeTimeSeries(
                new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileOil)),
                new TimeSeries(caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil))
            );

            dto.StartYear = caseItem.Dg4Date.Year;
            steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualOil = dto;
            startYearsProductionSalesAndVolumes.Add(dto.StartYear);
        }

        var netSalesGasDto = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.NetSalesGas));

        if (netSalesGasDto.Values.Length > 0)
        {
            netSalesGasDto.StartYear += caseItem.Dg4Date.Year;
            steaCaseDto.ProductionAndSalesVolumes.TotalAndAnnualSalesGas = netSalesGasDto;
            startYearsProductionSalesAndVolumes.Add(netSalesGasDto.StartYear);
        }

        var importedElectricityDto = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.ImportedElectricity));

        if (importedElectricityDto.Values.Length > 0)
        {
            importedElectricityDto.StartYear += caseItem.Dg4Date.Year;
            steaCaseDto.ProductionAndSalesVolumes.ImportedElectricity = importedElectricityDto;
            startYearsProductionSalesAndVolumes.Add(importedElectricityDto.StartYear);
        }

        var co2EmissionsDto = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.Co2Emissions));

        if (co2EmissionsDto.Values.Length > 0)
        {
            co2EmissionsDto.StartYear += caseItem.Dg4Date.Year;
            steaCaseDto.ProductionAndSalesVolumes.Co2Emissions = co2EmissionsDto;
            startYearsProductionSalesAndVolumes.Add(co2EmissionsDto.StartYear);
        }

        if (startYearsProductionSalesAndVolumes.Count > 0)
        {
            steaCaseDto.ProductionAndSalesVolumes.StartYear = startYearsProductionSalesAndVolumes.Min();
        }

        if (caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileOil) != null)
        {
            var dto = new TimeSeries(caseItem.GetProfile(ProfileTypes.AdditionalProductionProfileOil));
            dto.StartYear += caseItem.Dg4Date.Year;

            steaCaseDto.ProductionAndSalesVolumes.AdditionalOil = dto;
        }

        var additionalProductionProfileGasProfile = caseItem.GetProfileOrNull(ProfileTypes.AdditionalProductionProfileGas);

        if (additionalProductionProfileGasProfile != null)
        {
            var dto = new TimeSeries(additionalProductionProfileGasProfile);
            dto.StartYear += caseItem.Dg4Date.Year;

            steaCaseDto.ProductionAndSalesVolumes.AdditionalGas = dto;
        }
    }

    private static void AddExploration(SteaCaseDto steaCaseDto, Case caseItem)
    {
        steaCaseDto.Exploration = new TimeSeries();

        var costProfileDtos = new List<TimeSeries>
        {
            new(caseItem.GetProfileOrNull(ProfileTypes.ProjectSpecificDrillingCostProfile)),
            new(caseItem.GetProfileOrNull(ProfileTypes.SeismicAcquisitionAndProcessing)),
            new(caseItem.GetProfileOrNull(ProfileTypes.CountryOfficeCost)),

            new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.ExplorationWellCostProfile)),
            new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.AppraisalWellCostProfile)),
            new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.SidetrackCostProfile)),

            new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.GAndGAdminCost)),
            new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.ExplorationRigUpgradingCostProfile)),
            new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.ExplorationRigMobDemob))
        };

        costProfileDtos.AddRange(caseItem.Campaigns
                                     .Where(x => x.CampaignType == CampaignType.ExplorationCampaign)
                                     .Select(campaign => new TimeSeries
                                     {
                                         StartYear = campaign.RigUpgradingCostStartYear,
                                         Values = campaign.RigUpgradingCostValues.Select(x => x * campaign.RigUpgradingCost).ToArray()
                                     }));

        costProfileDtos.AddRange(caseItem.Campaigns
                                     .Where(x => x.CampaignType == CampaignType.ExplorationCampaign)
                                     .Select(campaign => new TimeSeries
                                     {
                                         StartYear = campaign.RigMobDemobCostStartYear,
                                         Values = campaign.RigMobDemobCostValues.Select(x => x * campaign.RigMobDemobCost).ToArray()
                                     }));

        steaCaseDto.Exploration = TimeSeriesMerger.MergeTimeSeries(costProfileDtos);
        steaCaseDto.Exploration.StartYear += caseItem.Dg4Date.Year;
    }
}
