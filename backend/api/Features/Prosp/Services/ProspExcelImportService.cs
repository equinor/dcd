using api.Context;
using api.Features.Cases.Recalculation;
using api.Features.Prosp.Services.Assets.OnshorePowerSupplies;
using api.Features.Prosp.Services.Assets.Substructures;
using api.Features.Prosp.Services.Assets.Surfs;
using api.Features.Prosp.Services.Assets.Topsides;
using api.Features.Prosp.Services.Assets.Transports;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Prosp.Services;

public class ProspExcelImportService(
    DcdDbContext context,
    SurfProspService surfProspService,
    TopsideProspService topsideProspService,
    SubstructureProspService substructureProspService,
    TransportProspService transportProspService,
    OnshorePowerSupplyProspService onshorePowerSupplyProspService,
    RecalculationService recalculationService)
{
    private const string SheetName = "main";

    public async Task ImportProsp(Stream stream,
        Guid caseId,
        Guid projectId,
        string sharepointFileId,
        string sharepointFileName)
    {
        using var document = SpreadsheetDocument.Open(stream, false);

        if (document.WorkbookPart == null)
        {
            return;
        }

        var mainSheet = document.WorkbookPart
            .Workbook
            .Descendants<Sheet>()
            .FirstOrDefault(x => x.Name?.ToString()?.ToLower() == SheetName);

        if (mainSheet?.Id == null)
        {
            return;
        }

        var cellData = ((WorksheetPart)document.WorkbookPart.GetPartById(mainSheet.Id!)).Worksheet.Descendants<Cell>().ToList();

        var caseItem = await context.Cases
            .Include(c => c.TimeSeriesProfiles)
            .SingleAsync(c => c.Id == caseId);

        caseItem.SharepointFileId = sharepointFileId;
        caseItem.SharepointFileName = sharepointFileName;
        caseItem.SharepointFileUrl = null;

        await surfProspService.ImportSurf(cellData, projectId, caseItem);
        await topsideProspService.ImportTopside(cellData, projectId, caseItem);
        await substructureProspService.ImportSubstructure(cellData, projectId, caseItem);
        await transportProspService.ImportTransport(cellData, projectId, caseItem);
        await onshorePowerSupplyProspService.ImportOnshorePowerSupply(cellData, projectId, caseItem);

        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }

    public async Task ClearImportedProspData(Guid caseId)
    {
        var caseItem = await context.Cases.SingleAsync(x => x.Id == caseId);

        caseItem.SharepointFileId = null;
        caseItem.SharepointFileName = null;
        caseItem.SharepointFileUrl = null;

        await surfProspService.ClearImportedSurf(caseItem);
        await topsideProspService.ClearImportedTopside(caseItem);
        await substructureProspService.ClearImportedSubstructure(caseItem);
        await transportProspService.ClearImportedTransport(caseItem);
        await onshorePowerSupplyProspService.ClearImportedOnshorePowerSupply(caseItem);

        await recalculationService.SaveChangesAndRecalculateCase(caseId);
    }
}
