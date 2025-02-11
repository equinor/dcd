using api.Features.Assets.CaseAssets.Substructures;
using api.Features.Profiles.Dtos;
using api.Features.Prosp.Constants;
using api.Models;
using api.Models.Enums;

using DocumentFormat.OpenXml.Spreadsheet;

namespace api.Features.Prosp.Services.Assets.Substructures;

public class SubstructureProspService(UpdateSubstructureService updateSubstructureService, SubstructureCostProfileService substructureCostProfileService)
{
    public async Task ClearImportedSubstructure(Case caseItem)
    {
        await updateSubstructureService.UpdateSubstructure(caseItem.ProjectId,
            caseItem.Id,
            new ProspUpdateSubstructureDto
            {
                Source = Source.ConceptApp
            });

        await substructureCostProfileService.AddOrUpdateSubstructureCostProfile(caseItem.ProjectId, caseItem.Id, new UpdateTimeSeriesCostDto());
    }

    public async Task ImportSubstructure(List<Cell> cellData, Guid projectId, Case caseItem)
    {
        List<string> costProfileCoords =
        [
            "J105",
            "K105",
            "L105",
            "M105",
            "N105",
            "O105",
            "P105"
        ];
        var costProfileStartYear = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.SubStructure.CostProfileStartYear);
        var dG3Date = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.SubStructure.Dg3Date);
        var dG4Date = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.SubStructure.Dg4Date);
        var dryWeight = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.SubStructure.DryWeight);
        var conceptInt = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.SubStructure.ConceptInt);
        var concept = ParseHelpers.MapSubstructureConcept(conceptInt);
        var costProfile = new UpdateTimeSeriesCostDto
        {
            Values = ParseHelpers.ReadDoubleValues(cellData, costProfileCoords),
            StartYear = costProfileStartYear - dG4Date.Year
        };

        // Prosp meta data
        var versionDate = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.SubStructure.VersionDate);
        var costYear = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.SubStructure.CostYear);

        var updateSubstructureDto = new ProspUpdateSubstructureDto
        {
            DryWeight = dryWeight,
            Concept = concept,
            DG3Date = dG3Date,
            DG4Date = dG4Date,
            Source = Source.Prosp,
            ProspVersion = versionDate,
            CostYear = costYear
        };

        await updateSubstructureService.UpdateSubstructure(projectId, caseItem.Id, updateSubstructureDto);
        await substructureCostProfileService.AddOrUpdateSubstructureCostProfile(projectId, caseItem.Id, costProfile);
    }
}
