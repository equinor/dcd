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
        asset.DryWeight = 0;
        asset.CostYear = 0;
        asset.Concept = Concept.NoConcept;
        asset.ProspVersion = null;

        SubstructureCostProfileService.AddOrUpdateSubstructureCostProfile(caseItem, 0, []);
    }

    public static void ImportSubstructure(List<Cell> cellData, Case caseItem)
    {
        List<string> costProfileCoords = ["J105", "K105", "L105", "M105", "N105", "O105", "P105"];

        var dG4Date = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.Substructure.Dg4Date);
        var dryWeight = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.Substructure.DryWeight);
        var concept = ParseHelpers.MapSubstructureConcept(ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Substructure.ConceptInt));
        var versionDate = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.Substructure.VersionDate);
        var costYear = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Substructure.CostYear);

        var costProfileStartYear = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Substructure.CostProfileStartYear);
        var values = ParseHelpers.ReadDoubleValues(cellData, costProfileCoords);
        var startYear = costProfileStartYear - dG4Date.Year;

        var asset = caseItem.Substructure;

        asset.Source = Source.Prosp;
        asset.DryWeight = dryWeight;
        asset.CostYear = costYear;
        asset.Concept = concept;
        asset.ProspVersion = versionDate;

        SubstructureCostProfileService.AddOrUpdateSubstructureCostProfile(caseItem, startYear, values);
    }
}
