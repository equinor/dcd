using api.Features.Profiles.Dtos;
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
        asset.LastChangedDate = DateTime.UtcNow;
        asset.CessationCost = 0;
        asset.InfieldPipelineSystemLength = 0;
        asset.UmbilicalSystemLength = 00;
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

        SurfCostProfileService.AddOrUpdateSurfCostProfile(caseItem, new UpdateTimeSeriesCostDto());
    }

    public static void ImportSurf(List<Cell> cellData, Case caseItem)
    {
        List<string> costProfileCoords =
        [
            "J112",
            "K112",
            "L112",
            "M112",
            "N112",
            "O112",
            "P112"
        ];
        var costProfileStartYear = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Surf.CostProfileStartYear);
        var dG3Date = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.Surf.Dg3Date);
        var dG4Date = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.Surf.Dg4Date);
        var lengthProductionLine = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.Surf.LengthProductionLine);
        var lengthUmbilicalSystem = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.Surf.LengthUmbilicalSystem);
        var productionFlowLineInt = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Surf.ProductionFlowLineInt);
        var productionFlowLine = ParseHelpers.MapProductionFlowLine(productionFlowLineInt);
        var artificialLiftInt = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Surf.ArtificialLiftInt);
        var artificialLift = ParseHelpers.MapArtificialLift(artificialLiftInt);
        var riserCount = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Surf.RiserCount);
        var templateCount = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Surf.TemplateCount);
        var producerCount = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Surf.ProducerCount);
        var waterInjectorCount = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Surf.WaterInjectorCount);
        var gasInjectorCount = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Surf.GasInjectorCount);
        var cessationCost = ParseHelpers.ReadDoubleValue(cellData, ProspCellReferences.Surf.CessationCost);
        var costProfile = new UpdateTimeSeriesCostDto
        {
            Values = ParseHelpers.ReadDoubleValues(cellData, costProfileCoords),
            StartYear = costProfileStartYear - dG4Date.Year,
        };

        // Prosp meta data
        var versionDate = ParseHelpers.ReadDateValue(cellData, ProspCellReferences.Surf.VersionDate);
        var costYear = ParseHelpers.ReadIntValue(cellData, ProspCellReferences.Surf.CostYear);

        var asset = caseItem.Surf;

        asset.Source = Source.Prosp;
        asset.LastChangedDate = DateTime.UtcNow;
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

        SurfCostProfileService.AddOrUpdateSurfCostProfile(caseItem, costProfile);
    }
}
