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
        asset.DG3Date = null;
        asset.DG4Date = null;
        asset.ProspVersion = null;

        OnshorePowerSupplyCostProfileService.AddOrUpdateOnshorePowerSupplyCostProfile(caseItem, 0, []);
    }

    public static void ImportOnshorePowerSupply(List<Cell> cellData, Case caseItem)
    {
        List<string> costProfileCoords = ["J114", "K114", "L114", "M114", "N114", "O114", "P114"];

        var dG4Date = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.OnshorePowerSupply.Dg4Date);

        var costProfileStartYear = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.OnshorePowerSupply.CostProfileStartYear);
        var startYear = costProfileStartYear - dG4Date.Year;
        var values = ParseHelpers.ReadDoubleValues(cellData, costProfileCoords);

        OnshorePowerSupplyCostProfileService.AddOrUpdateOnshorePowerSupplyCostProfile(caseItem, startYear, values);
    }
}
