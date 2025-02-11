using api.Features.Assets.CaseAssets.Surfs;
using api.Features.Profiles.Dtos;
using api.Features.Prosp.Constants;
using api.Models;
using api.Models.Enums;

using DocumentFormat.OpenXml.Spreadsheet;

namespace api.Features.Prosp.Services.Assets.Surfs;

public class SurfProspService(UpdateSurfService updateSurfService, SurfCostProfileService surfCostProfileService)
{
    public async Task ClearImportedSurf(Case caseItem)
    {
        await updateSurfService.UpdateSurf(caseItem.ProjectId,
            caseItem.Id,
            new ProspUpdateSurfDto
            {
                Source = Source.ConceptApp
            });

        await surfCostProfileService.AddOrUpdateSurfCostProfile(caseItem.ProjectId, caseItem.Id, new UpdateTimeSeriesCostDto());
    }

    public async Task ImportSurf(List<Cell> cellData, Guid projectId, Case caseItem)
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

        var updatedSurfDto = new ProspUpdateSurfDto
        {
            ProductionFlowline = productionFlowLine,
            UmbilicalSystemLength = lengthUmbilicalSystem,
            InfieldPipelineSystemLength = lengthProductionLine,
            RiserCount = riserCount,
            TemplateCount = templateCount,
            ArtificialLift = artificialLift,
            Source = Source.Prosp,
            ProspVersion = versionDate,
            CostYear = costYear,
            DG3Date = dG3Date,
            DG4Date = dG4Date,
            ProducerCount = producerCount,
            GasInjectorCount = gasInjectorCount,
            WaterInjectorCount = waterInjectorCount,
            CessationCost = cessationCost,
        };

        await updateSurfService.UpdateSurf(projectId, caseItem.Id, updatedSurfDto);
        await surfCostProfileService.AddOrUpdateSurfCostProfile(projectId, caseItem.Id, costProfile);
    }
}
