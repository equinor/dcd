using api.Features.Assets.CaseAssets.Transports;
using api.Features.Profiles.Dtos;
using api.Features.Prosp.Constants;
using api.Models;
using api.Models.Enums;

using DocumentFormat.OpenXml.Spreadsheet;

namespace api.Features.Prosp.Services.Assets.Transports;

public class TransportProspService(UpdateTransportService updateTransportService, TransportCostProfileService transportCostProfileService)
{
    public async Task ClearImportedTransport(Case caseItem)
    {
        await updateTransportService.UpdateTransport(caseItem.ProjectId,
            caseItem.Id,
            new ProspUpdateTransportDto
            {
                Source = Source.ConceptApp
            });

        await transportCostProfileService.AddOrUpdateTransportCostProfile(caseItem.ProjectId, caseItem.Id, new UpdateTimeSeriesCostDto());
    }

    public async Task ImportTransport(List<Cell> cellData, Guid projectId, Case caseItem)
    {
        List<string> costProfileCoords =
        [
            "J113",
            "K113",
            "L113",
            "M113",
            "N113",
            "O113",
            "P113"
        ];
        var costProfileStartYear = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Transport.CostProfileStartYear);
        var dG3Date = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.Transport.Dg3Date);
        var dG4Date = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.Transport.Dg4Date);
        var costProfile = new UpdateTimeSeriesCostDto
        {
            Values = ParseHelpers.ReadDoubleValues(cellData, costProfileCoords),
            StartYear = costProfileStartYear - dG4Date.Year,
        };
        // Prosp meta data
        var versionDate = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.Transport.VersionDate);
        var costYear = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Transport.CostYear);
        var oilExportPipelineLength = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.Transport.OilExportPipelineLength);
        var gasExportPipelineLength = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.Transport.GasExportPipelineLength);

        var updateTransportDto = new ProspUpdateTransportDto
        {
            DG3Date = dG3Date,
            DG4Date = dG4Date,
            Source = Source.Prosp,
            ProspVersion = versionDate,
            CostYear = costYear,
            OilExportPipelineLength = oilExportPipelineLength,
            GasExportPipelineLength = gasExportPipelineLength
        };

        await updateTransportService.UpdateTransport(projectId, caseItem.Id, updateTransportDto);
        await transportCostProfileService.AddOrUpdateTransportCostProfile(projectId, caseItem.Id, costProfile);
    }
}
