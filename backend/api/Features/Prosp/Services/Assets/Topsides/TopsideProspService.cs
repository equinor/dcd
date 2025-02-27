using api.Features.Prosp.Constants;
using api.Models;
using api.Models.Enums;

using DocumentFormat.OpenXml.Spreadsheet;

namespace api.Features.Prosp.Services.Assets.Topsides;

public static class TopsideProspService
{
    public static void ClearImportedTopside(Case caseItem)
    {
        var asset = caseItem.Topside;

        asset.Source = Source.ConceptApp;
        asset.DryWeight = 0;
        asset.OilCapacity = 0;
        asset.GasCapacity = 0;
        asset.WaterInjectionCapacity = 0;
        asset.ArtificialLift = ArtificialLift.NoArtificialLift;
        asset.FuelConsumption = 0;
        asset.FlaredGas = 0;
        asset.ProducerCount = 0;
        asset.GasInjectorCount = 0;
        asset.WaterInjectorCount = 0;
        asset.CO2ShareOilProfile = 0;
        asset.CO2ShareGasProfile = 0;
        asset.CO2ShareWaterInjectionProfile = 0;
        asset.CO2OnMaxOilProfile = 0;
        asset.CO2OnMaxGasProfile = 0;
        asset.CO2OnMaxWaterInjectionProfile = 0;
        asset.CostYear = 0;
        asset.FacilityOpex = 0;
        asset.PeakElectricityImported = 0;
        asset.ProspVersion = null;

        TopsideCostProfileService.AddOrUpdateTopsideCostProfile(caseItem, 0, []);
    }

    public static void ImportTopside(List<Cell> cellData, Case caseItem)
    {
        List<string> costProfileCoords = ["J104", "K104", "L104", "M104", "N104", "O104", "P104"];

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
        var peakElectricityImported = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.TopSide.PeakElectricityImported);
        var versionDate = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.TopSide.VersionDate);
        var costYear = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.TopSide.CostYear);

        var costProfileStartYear = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.TopSide.CostProfileStartYear);
        var startYear = costProfileStartYear - dG4Date.Year;
        var values = ParseHelpers.ReadDoubleValues(cellData, costProfileCoords);

        var asset = caseItem.Topside;

        asset.Source = Source.Prosp;
        asset.DryWeight = dryWeight;
        asset.OilCapacity = oilCapacity;
        asset.GasCapacity = gasCapacity;
        asset.WaterInjectionCapacity = waterInjectorCapacity;
        asset.ArtificialLift = artificialLift;
        asset.FuelConsumption = fuelConsumption;
        asset.FlaredGas = flaredGas;
        asset.ProducerCount = producerCount;
        asset.GasInjectorCount = gasInjectorCount;
        asset.WaterInjectorCount = waterInjectorCount;
        asset.CO2ShareOilProfile = cO2ShareOilProfile;
        asset.CO2ShareGasProfile = cO2ShareGasProfile;
        asset.CO2ShareWaterInjectionProfile = cO2ShareWaterInjectionProfile;
        asset.CO2OnMaxOilProfile = cO2OnMaxOilProfile;
        asset.CO2OnMaxGasProfile = cO2OnMaxGasProfile;
        asset.CO2OnMaxWaterInjectionProfile = cO2OnMaxWaterInjectionProfile;
        asset.CostYear = costYear;
        asset.FacilityOpex = facilityOpex;
        asset.PeakElectricityImported = peakElectricityImported;
        asset.ProspVersion = versionDate;

        TopsideCostProfileService.AddOrUpdateTopsideCostProfile(caseItem, startYear, values);
    }
}
