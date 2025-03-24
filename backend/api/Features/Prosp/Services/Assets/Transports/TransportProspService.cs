using api.Features.Prosp.Constants;
using api.Models;
using api.Models.Enums;

using DocumentFormat.OpenXml.Spreadsheet;

namespace api.Features.Prosp.Services.Assets.Transports;

public static class TransportProspService
{
    public static void ClearImportedTransport(Case caseItem)
    {
        var asset = caseItem.Transport;

        asset.Source = Source.ConceptApp;
        asset.ProspVersion = null;
        asset.GasExportPipelineLength = 0;
        asset.OilExportPipelineLength = 0;
        asset.CostYear = 0;

        TransportCostProfileService.AddOrUpdateTransportCostProfile(caseItem, 0, []);
    }

    public static void ImportTransport(List<Cell> cellData, Case caseItem)
    {
        List<string> costProfileCoords = ["J113", "K113", "L113", "M113", "N113", "O113", "P113"];

        var firstYearInCostProfile = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.MainSheet.CostProfilesFirstYear);

        var versionDate = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.Transport.VersionDate);
        var costYear = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Transport.CostYear);
        var oilExportPipelineLength = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.Transport.OilExportPipelineLength);
        var gasExportPipelineLength = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.Transport.GasExportPipelineLength);

        var values = ParseHelpers.ReadDoubleValues(cellData, costProfileCoords);
        var startYear = firstYearInCostProfile - caseItem.Dg4Date.Year;

        var asset = caseItem.Transport;

        asset.Source = Source.Prosp;
        asset.ProspVersion = versionDate;
        asset.GasExportPipelineLength = gasExportPipelineLength;
        asset.OilExportPipelineLength = oilExportPipelineLength;
        asset.CostYear = costYear;

        TransportCostProfileService.AddOrUpdateTransportCostProfile(caseItem, startYear, values);
    }
}
