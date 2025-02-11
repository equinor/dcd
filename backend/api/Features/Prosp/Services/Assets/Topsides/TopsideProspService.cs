using api.Features.Assets.CaseAssets.Topsides;
using api.Features.Profiles.Dtos;
using api.Features.Prosp.Constants;
using api.Models;
using api.Models.Enums;

using DocumentFormat.OpenXml.Spreadsheet;

namespace api.Features.Prosp.Services.Assets.Topsides;

public class TopsideProspService(UpdateTopsideService updateTopsideService, TopsideCostProfileService topsideCostProfileService)
{
    public async Task ClearImportedTopside(Case caseItem)
    {
        await updateTopsideService.UpdateTopside(caseItem.ProjectId,
            caseItem.Id,
            new ProspUpdateTopsideDto
            {
                Source = Source.ConceptApp
            });

        await topsideCostProfileService.AddOrUpdateTopsideCostProfile(caseItem.ProjectId, caseItem.Id, new UpdateTimeSeriesCostDto());
    }

    public async Task ImportTopside(List<Cell> cellData, Guid projectId, Case caseItem)
    {
        List<string> costProfileCoords =
        [
            "J104",
            "K104",
            "L104",
            "M104",
            "N104",
            "O104",
            "P104"
        ];
        var costProfileStartYear = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.TopSide.CostProfileStartYear);
        var dG3Date = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.TopSide.Dg3Date);
        var dG4Date = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.TopSide.Dg4Date);
        var artificialLiftInt = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.TopSide.ArtificialLiftInt);
        var artificialLift = ParseHelpers.MapArtificialLift(artificialLiftInt);
        var dryWeight = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.TopSide.DryWeight);
        var fuelConsumption = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.TopSide.FuelConsumption);
        var flaredGas = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.TopSide.FlaredGas);
        var oilCapacity = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.TopSide.OilCapacity);
        var gasCapacity = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.TopSide.GasCapacity);
        var waterInjectorCapacity = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.TopSide.WaterInjectionCapacity);
        var producerCount = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.TopSide.ProducerCount);
        var gasInjectorCount = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.TopSide.GasInjectorCount);
        var waterInjectorCount = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.TopSide.WaterInjectorCount);
        var cO2ShareOilProfile = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.TopSide.Co2ShareOilProfile);
        var cO2ShareGasProfile = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.TopSide.Co2ShareGasProfile);
        var cO2ShareWaterInjectionProfile = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.TopSide.Co2ShareWaterInjectionProfile);
        var cO2OnMaxOilProfile = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.TopSide.Co2OnMaxOilProfile);
        var cO2OnMaxGasProfile = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.TopSide.Co2OnMaxGasProfile);
        var cO2OnMaxWaterInjectionProfile = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.TopSide.Co2OnMaxWaterInjectionProfile);
        var facilityOpex = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.TopSide.FacilityOpex);
        var costProfile = new UpdateTimeSeriesCostDto
        {
            Values = ParseHelpers.ReadDoubleValues(cellData, costProfileCoords),
            StartYear = costProfileStartYear - dG4Date.Year,
        };
        var peakElectricityImported = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.TopSide.PeakElectricityImported);
        // Prosp meta data
        var versionDate = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.TopSide.VersionDate);
        var costYear = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.TopSide.CostYear);

        var updateTopsideDto = new ProspUpdateTopsideDto
        {
            DG3Date = dG3Date,
            DG4Date = dG4Date,
            DryWeight = dryWeight,
            OilCapacity = oilCapacity,
            GasCapacity = gasCapacity,
            WaterInjectionCapacity = waterInjectorCapacity,
            ArtificialLift = artificialLift,
            ProducerCount = producerCount,
            WaterInjectorCount = waterInjectorCount,
            GasInjectorCount = gasInjectorCount,
            FuelConsumption = fuelConsumption,
            FlaredGas = flaredGas,
            CO2ShareOilProfile = cO2ShareOilProfile,
            CO2ShareGasProfile = cO2ShareGasProfile,
            CO2ShareWaterInjectionProfile = cO2ShareWaterInjectionProfile,
            CO2OnMaxOilProfile = cO2OnMaxOilProfile,
            CO2OnMaxGasProfile = cO2OnMaxGasProfile,
            CO2OnMaxWaterInjectionProfile = cO2OnMaxWaterInjectionProfile,
            Source = Source.Prosp,
            ProspVersion = versionDate,
            CostYear = costYear,
            FacilityOpex = facilityOpex,
            PeakElectricityImported = peakElectricityImported
        };

        await updateTopsideService.UpdateTopside(projectId, caseItem.Id, updateTopsideDto);
        await topsideCostProfileService.AddOrUpdateTopsideCostProfile(projectId, caseItem.Id, costProfile);
    }
}
