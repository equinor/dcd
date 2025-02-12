using api.Features.Profiles.Dtos;
using api.Features.Prosp.Constants;
using api.Models;
using api.Models.Enums;

using DocumentFormat.OpenXml.Spreadsheet;

namespace api.Features.Prosp.Services.Assets.Substructures;

public static class SubstructureProspService
{
    public static void ClearImportedSubstructure(Case caseItem)
    {
        var asset = caseItem.Substructure;

        asset.Source = Source.ConceptApp;
        asset.LastChangedDate = DateTime.UtcNow;
        asset.DryWeight = 0;
        asset.CostYear = 0;
        asset.Concept = Concept.NO_CONCEPT;
        asset.DG3Date = null;
        asset.DG4Date = null;
        asset.ProspVersion = null;

        SubstructureCostProfileService.AddOrUpdateSubstructureCostProfile(caseItem, new UpdateTimeSeriesCostDto());
    }

    public static void ImportSubstructure(List<Cell> cellData, Case caseItem)
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

        var versionDate = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.SubStructure.VersionDate);
        var costYear = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.SubStructure.CostYear);

        var asset = caseItem.Substructure;

        asset.Source = Source.Prosp;
        asset.LastChangedDate = DateTime.UtcNow;
        asset.DryWeight = dryWeight;
        asset.CostYear = costYear;
        asset.Concept = concept;
        asset.DG3Date = dG3Date;
        asset.DG4Date = dG4Date;
        asset.ProspVersion = versionDate;

        SubstructureCostProfileService.AddOrUpdateSubstructureCostProfile(caseItem, costProfile);
    }
}
