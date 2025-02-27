using api.Features.Prosp.Constants;
using api.Models;
using api.Models.Enums;

using DocumentFormat.OpenXml.Spreadsheet;

namespace api.Features.Prosp.Services.Assets.Surfs;

public static class SurfProspService
{
    public static void ClearImportedSurf(Case caseItem)
    {
        var asset = caseItem.Surf;

        asset.Source = Source.ConceptApp;
        asset.CessationCost = 0;
        asset.InfieldPipelineSystemLength = 0;
        asset.UmbilicalSystemLength = 0;
        asset.ArtificialLift = ArtificialLift.NoArtificialLift;
        asset.RiserCount = 0;
        asset.TemplateCount = 0;
        asset.ProducerCount = 0;
        asset.GasInjectorCount = 0;
        asset.WaterInjectorCount = 0;
        asset.ProductionFlowline = ProductionFlowline.No_production_flowline;
        asset.CostYear = 0;
        asset.ApprovedBy = "";
        asset.DG3Date = null;
        asset.DG4Date = null;
        asset.ProspVersion = null;

        SurfCostProfileService.AddOrUpdateSurfCostProfile(caseItem, 0, []);
    }

    public static void ImportSurf(List<Cell> cellData, Case caseItem)
    {
        List<string> costProfileCoords = ["J112", "K112", "L112", "M112", "N112", "O112", "P112"];

        var dG3Date = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.Surf.Dg3Date);
        var dG4Date = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.Surf.Dg4Date);
        var lengthProductionLine = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.Surf.LengthProductionLine);
        var lengthUmbilicalSystem = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.Surf.LengthUmbilicalSystem);
        var productionFlowLine = ParseHelpers.MapProductionFlowLine(ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Surf.ProductionFlowLineInt));
        var artificialLift = ParseHelpers.MapArtificialLift(ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Surf.ArtificialLiftInt));
        var riserCount = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Surf.RiserCount);
        var templateCount = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Surf.TemplateCount);
        var producerCount = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Surf.ProducerCount);
        var waterInjectorCount = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Surf.WaterInjectorCount);
        var gasInjectorCount = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Surf.GasInjectorCount);
        var cessationCost = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.Surf.CessationCost);
        var versionDate = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.Surf.VersionDate);
        var costYear = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Surf.CostYear);

        var costProfileStartYear = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Surf.CostProfileStartYear);
        var startYear = costProfileStartYear - dG4Date.Year;
        var costProfileValues = ParseHelpers.ReadDoubleValues(cellData, costProfileCoords);

        var asset = caseItem.Surf;

        asset.Source = Source.Prosp;
        asset.CessationCost = cessationCost;
        asset.InfieldPipelineSystemLength = lengthProductionLine;
        asset.UmbilicalSystemLength = lengthUmbilicalSystem;
        asset.ArtificialLift = artificialLift;
        asset.RiserCount = riserCount;
        asset.TemplateCount = templateCount;
        asset.ProducerCount = producerCount;
        asset.GasInjectorCount = gasInjectorCount;
        asset.WaterInjectorCount = waterInjectorCount;
        asset.ProductionFlowline = productionFlowLine;
        asset.CostYear = costYear;
        asset.ApprovedBy = "";
        asset.DG3Date = dG3Date;
        asset.DG4Date = dG4Date;
        asset.ProspVersion = versionDate;

        SurfCostProfileService.AddOrUpdateSurfCostProfile(caseItem, startYear, costProfileValues);
    }
}
