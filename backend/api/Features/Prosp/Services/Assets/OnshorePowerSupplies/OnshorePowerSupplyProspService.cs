using api.Features.Prosp.Constants;
using api.Models;
using api.Models.Enums;

using DocumentFormat.OpenXml.Spreadsheet;

namespace api.Features.Prosp.Services.Assets.OnshorePowerSupplies;

public static class OnshorePowerSupplyProspService
{
    public static void ClearImportedOnshorePowerSupply(Case caseItem)
    {
        var asset = caseItem.OnshorePowerSupply;

        asset.Source = Source.ConceptApp;
        asset.CostYear = 0;
        asset.ProspVersion = null;

        OnshorePowerSupplyCostProfileService.AddOrUpdateOnshorePowerSupplyCostProfile(caseItem, 0, [], true);
    }

    public static void ImportOnshorePowerSupply(List<Cell> cellData, Case caseItem)
    {
        List<string> costProfileCoords = ["J114", "K114", "L114", "M114", "N114", "O114", "P114"];

        var firstYearInCostProfile = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.MainSheet.CostProfilesFirstYear);

        var startYear = firstYearInCostProfile - caseItem.Dg4Date.Year;
        var values = ParseHelpers.ReadDoubleValues(cellData, costProfileCoords);

        OnshorePowerSupplyCostProfileService.AddOrUpdateOnshorePowerSupplyCostProfile(caseItem, startYear, values, false);
    }
}
