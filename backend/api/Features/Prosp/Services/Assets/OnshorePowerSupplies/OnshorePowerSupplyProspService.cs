using api.Features.Assets.CaseAssets.OnshorePowerSupplies;
using api.Features.Profiles.Dtos;
using api.Features.Prosp.Constants;
using api.Models;
using api.Models.Enums;

using DocumentFormat.OpenXml.Spreadsheet;

namespace api.Features.Prosp.Services.Assets.OnshorePowerSupplies;

public class OnshorePowerSupplyProspService(
    UpdateOnshorePowerSupplyService updateOnshorePowerSupplyService,
    OnshorePowerSupplyCostProfileService onshorePowerSupplyCostProfileService)
{
    public async Task ClearImportedOnshorePowerSupply(Case caseItem)
    {
        await updateOnshorePowerSupplyService.UpdateOnshorePowerSupply(caseItem.ProjectId,
            caseItem.Id,
            new ProspUpdateOnshorePowerSupplyDto
            {
                Source = Source.ConceptApp
            });

        await onshorePowerSupplyCostProfileService.AddOrUpdateOnshorePowerSupplyCostProfile(caseItem.ProjectId, caseItem.Id, new UpdateTimeSeriesCostDto());
    }

    public async Task ImportOnshorePowerSupply(List<Cell> cellData, Guid projectId, Case caseItem)
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
            StartYear = costProfileStartYear - dG4Date.Year,
        };

        await onshorePowerSupplyCostProfileService.AddOrUpdateOnshorePowerSupplyCostProfile(projectId, caseItem.Id, costProfile);
    }
}
