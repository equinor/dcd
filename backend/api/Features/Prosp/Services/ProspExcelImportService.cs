using api.Context;
using api.Features.Profiles;
using api.Features.Prosp.Services.Assets.OnshorePowerSupplies;
using api.Features.Prosp.Services.Assets.Substructures;
using api.Features.Prosp.Services.Assets.Surfs;
using api.Features.Prosp.Services.Assets.Topsides;
using api.Features.Prosp.Services.Assets.Transports;
using api.Models;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Prosp.Services;

public class ProspExcelImportService(DcdDbContext context)
{
    private const string SheetName = "main";

    public async Task ClearImportedProspData(Guid projectId, List<Guid> caseIds)
    {
        var caseItems = await LoadCaseData(projectId, caseIds);

        foreach (var caseItem in caseItems)
        {
            caseItem.UpdatedUtc = DateTime.UtcNow;
            caseItem.SharepointFileId = null;
            caseItem.SharepointFileName = null;
            caseItem.SharepointFileUrl = null;

            SurfProspService.ClearImportedSurf(caseItem);
            TopsideProspService.ClearImportedTopside(caseItem);
            SubstructureProspService.ClearImportedSubstructure(caseItem);
            TransportProspService.ClearImportedTransport(caseItem);
            OnshorePowerSupplyProspService.ClearImportedOnshorePowerSupply(caseItem);
        }

        await context.SaveChangesAsync();
    }

    public async Task ImportProsp(Stream stream, Guid projectId, Guid caseId, string sharepointFileId, string sharepointFileName)
    {
        using var document = SpreadsheetDocument.Open(stream, false);

        var mainSheetId = document.WorkbookPart!
            .Workbook
            .Descendants<Sheet>()
            .Single(x => x.Name?.ToString()?.ToLower() == SheetName)
            .Id;

        var worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(mainSheetId!);
        var cellData = worksheetPart.Worksheet.Descendants<Cell>().ToList();

        var caseItem = (await LoadCaseData(projectId, caseId)).Single();

        caseItem.SharepointFileId = sharepointFileId;
        caseItem.SharepointFileName = sharepointFileName;
        caseItem.SharepointFileUrl = null;

        SurfProspService.ImportSurf(cellData, caseItem);
        TopsideProspService.ImportTopside(cellData, caseItem);
        SubstructureProspService.ImportSubstructure(cellData, caseItem);
        TransportProspService.ImportTransport(cellData, caseItem);
        OnshorePowerSupplyProspService.ImportOnshorePowerSupply(cellData, caseItem);
    }

    private async Task<List<Case>> LoadCaseData(Guid projectId, params List<Guid> caseIds)
    {
        var profileTypes = new List<string>
        {
            ProfileTypes.OnshorePowerSupplyCostProfile, ProfileTypes.OnshorePowerSupplyCostProfileOverride,
            ProfileTypes.TransportCostProfile, ProfileTypes.TransportCostProfileOverride,
            ProfileTypes.SubstructureCostProfile, ProfileTypes.SubstructureCostProfileOverride,
            ProfileTypes.TopsideCostProfile, ProfileTypes.TopsideCostProfileOverride,
            ProfileTypes.SurfCostProfile, ProfileTypes.SurfCostProfileOverride
        };

        var caseItems = await context.Cases
            .Include(x => x.OnshorePowerSupply)
            .Include(x => x.Transport)
            .Include(x => x.Substructure)
            .Include(x => x.Topside)
            .Include(x => x.Surf)
            .Where(x => x.ProjectId == projectId && caseIds.Contains(x.Id))
            .ToListAsync();

        await context.TimeSeriesProfiles
            .Where(x => caseIds.Contains(x.CaseId))
            .Where(x => profileTypes.Contains(x.ProfileType))
            .LoadAsync();

        return caseItems;
    }
}
