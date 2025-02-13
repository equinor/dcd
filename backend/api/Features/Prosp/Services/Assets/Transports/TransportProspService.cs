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
        asset.LastChangedDate = DateTime.UtcNow;
        asset.ProspVersion = null;
        asset.GasExportPipelineLength = 0;
        asset.OilExportPipelineLength = 0;
        asset.CostYear = 0;
        asset.DG3Date = null;
        asset.DG4Date = null;

        TransportCostProfileService.AddOrUpdateTransportCostProfile(caseItem, 0, []);
    }

    public static void ImportTransport(List<Cell> cellData, Case caseItem)
    {
        List<string> costProfileCoords = ["J113", "K113", "L113", "M113", "N113", "O113", "P113"];

        var dG3Date = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.Transport.Dg3Date);
        var dG4Date = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.Transport.Dg4Date);
        var versionDate = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.Transport.VersionDate);
        var costYear = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Transport.CostYear);
        var oilExportPipelineLength = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.Transport.OilExportPipelineLength);
        var gasExportPipelineLength = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.Transport.GasExportPipelineLength);

        var costProfileStartYear = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Transport.CostProfileStartYear);
        var values = ParseHelpers.ReadDoubleValues(cellData, costProfileCoords);
        var startYear = costProfileStartYear - dG4Date.Year;

        var asset = caseItem.Transport;

        asset.Source = Source.Prosp;
        asset.LastChangedDate = DateTime.UtcNow;
        asset.ProspVersion = versionDate;
        asset.GasExportPipelineLength = gasExportPipelineLength;
        asset.OilExportPipelineLength = oilExportPipelineLength;
        asset.CostYear = costYear;
        asset.DG3Date = dG3Date;
        asset.DG4Date = dG4Date;

        TransportCostProfileService.AddOrUpdateTransportCostProfile(caseItem, startYear, values);
    }
}
