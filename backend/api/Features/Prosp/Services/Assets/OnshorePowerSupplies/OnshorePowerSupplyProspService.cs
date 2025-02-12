using api.Features.Profiles.Dtos;
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
        asset.LastChangedDate = DateTime.UtcNow;
        asset.CostYear = 0;
        asset.DG3Date = null;
        asset.DG4Date = null;
        asset.ProspVersion = null;

        OnshorePowerSupplyCostProfileService.AddOrUpdateOnshorePowerSupplyCostProfile(caseItem, new UpdateTimeSeriesCostDto());
    }

    public static void ImportOnshorePowerSupply(List<Cell> cellData, Case caseItem)
    {
        List<string> costProfileCoords =
        [
            "J114",
            "K114",
            "L114",
            "M114",
            "N114",
            "O114",
            "P114"
        ];
        var costProfileStartYear = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.OnshorePowerSupply.CostProfileStartYear);
        var dG4Date = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.OnshorePowerSupply.Dg4Date);

        var costProfile = new UpdateTimeSeriesCostDto
        {
            Values = ParseHelpers.ReadDoubleValues(cellData, costProfileCoords),
            StartYear = costProfileStartYear - dG4Date.Year
        };

        OnshorePowerSupplyCostProfileService.AddOrUpdateOnshorePowerSupplyCostProfile(caseItem, costProfile);
    }
}
