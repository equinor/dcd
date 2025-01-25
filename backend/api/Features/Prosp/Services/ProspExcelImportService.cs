using api.Context;
using api.Context.Extensions;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies;
using api.Features.Assets.CaseAssets.Substructures;
using api.Features.Assets.CaseAssets.Surfs;
using api.Features.Assets.CaseAssets.Topsides;
using api.Features.Assets.CaseAssets.Transports;
using api.Features.Cases.Recalculation;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.OnshorePowerSupplies.OnshorePowerSupplyCostProfiles;
using api.Features.Profiles.Substructures.SubstructureCostProfiles;
using api.Features.Profiles.Surfs.SurfCostProfiles;
using api.Features.Profiles.Topsides.TopsideCostProfiles;
using api.Features.Profiles.Transports.TransportCostProfiles;
using api.Features.Prosp.Constants;
using api.Features.Prosp.Models;
using api.Models;
using api.Models.Enums;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Prosp.Services;

public class ProspExcelImportService(
    DcdDbContext context,
    UpdateSurfService updateSurfService,
    UpdateSubstructureService updateSubstructureService,
    UpdateTopsideService updateTopsideService,
    UpdateTransportService updateTransportService,
    UpdateOnshorePowerSupplyService updateOnshorePowerSupplyService,
    SubstructureCostProfileService substructureCostProfileService,
    SurfCostProfileService surfCostProfileService,
    TopsideCostProfileService topsideCostProfileService,
    OnshorePowerSupplyCostProfileService onshorePowerSupplyCostProfileService,
    IRecalculationService recalculationService,
    TransportCostProfileService transportCostProfileService)
{
    private const string SheetName = "main";

    private static double ReadDoubleValue(IEnumerable<Cell> cellData, string coordinate)
    {
        if (double.TryParse(cellData.FirstOrDefault(c => c.CellReference == coordinate)?.CellValue?.InnerText,
                out var value))
        {
            return Math.Round(value, 3);
        }

        return 0;
    }

    private static int ReadIntValue(IEnumerable<Cell> cellData, string coordinate)
    {
        if (int.TryParse(cellData.FirstOrDefault(c => c.CellReference == coordinate)?.CellValue?.InnerText,
                out var value))
        {
            return value;
        }

        return 0;
    }

    private static double[] ReadDoubleValues(IEnumerable<Cell> cellData, List<string> coordinates)
    {
        var values = new List<double>();
        foreach (var cell in cellData.Where(c => c.CellReference != null && coordinates.Contains(c.CellReference!)))
            if (double.TryParse(cell.CellValue?.InnerText.Replace(',', '.'), out var value))
            {
                values.Add(value);
            }

        return values.ToArray();
    }

    private static DateTime ReadDateValue(IEnumerable<Cell> cellData, string coordinate)
    {
        if (double.TryParse(cellData.FirstOrDefault(c => c.CellReference == coordinate)?.CellValue?.InnerText,
                out var value))
        {
            return DateTime.FromOADate(value);
        }

        return new DateTime(1900, 1, 1);
    }

    private async Task ImportSurf(List<Cell> cellData, Guid projectId, Case caseItem)
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
        var costProfileStartYear = ReadIntValue(cellData, ProspCellReferences.Surf.CostProfileStartYear);
        var dG3Date = ReadDateValue(cellData, ProspCellReferences.Surf.Dg3Date);
        var dG4Date = ReadDateValue(cellData, ProspCellReferences.Surf.Dg4Date);
        var lengthProductionLine = ReadDoubleValue(cellData, ProspCellReferences.Surf.LengthProductionLine);
        var lengthUmbilicalSystem = ReadDoubleValue(cellData, ProspCellReferences.Surf.LengthUmbilicalSystem);
        var productionFlowLineInt = ReadIntValue(cellData, ProspCellReferences.Surf.ProductionFlowLineInt);
        var productionFlowLine = MapProductionFlowLine(productionFlowLineInt);
        var artificialLiftInt = ReadIntValue(cellData, ProspCellReferences.Surf.ArtificialLiftInt);
        var artificialLift = MapArtificialLift(artificialLiftInt);
        var riserCount = ReadIntValue(cellData, ProspCellReferences.Surf.RiserCount);
        var templateCount = ReadIntValue(cellData, ProspCellReferences.Surf.TemplateCount);
        var producerCount = ReadIntValue(cellData, ProspCellReferences.Surf.ProducerCount);
        var waterInjectorCount = ReadIntValue(cellData, ProspCellReferences.Surf.WaterInjectorCount);
        var gasInjectorCount = ReadIntValue(cellData, ProspCellReferences.Surf.GasInjectorCount);
        var cessationCost = ReadDoubleValue(cellData, ProspCellReferences.Surf.CessationCost);
        var costProfile = new UpdateTimeSeriesCostDto
        {
            Values = ReadDoubleValues(cellData, costProfileCoords),
            StartYear = costProfileStartYear - dG4Date.Year,
        };

        // Prosp meta data
        var versionDate = ReadDateValue(cellData, ProspCellReferences.Surf.VersionDate);
        var costYear = ReadIntValue(cellData, ProspCellReferences.Surf.CostYear);
        var importedCurrency = ReadIntValue(cellData, ProspCellReferences.Surf.ImportedCurrency);
        var currency = importedCurrency == 1 ? Currency.NOK :
            importedCurrency == 2 ? Currency.USD : 0;

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
            Currency = currency,
            CostYear = costYear,
            DG3Date = dG3Date,
            DG4Date = dG4Date,
            ProducerCount = producerCount,
            GasInjectorCount = gasInjectorCount,
            WaterInjectorCount = waterInjectorCount,
            CessationCost = cessationCost,
        };

        await updateSurfService.UpdateSurf(projectId, caseItem.Id, caseItem.SurfLink, updatedSurfDto);
        await surfCostProfileService.AddOrUpdateSurfCostProfile(projectId, caseItem.Id, caseItem.SurfLink, costProfile);
    }

    private async Task ImportTopside(List<Cell> cellData, Guid projectId, Case caseItem)
    {
        List<string> costProfileCoords =
        [
            "J104",
            "K104",
            "L104",
            "M104",
            "N104",
            "O104",
            "P104"
        ];
        var costProfileStartYear = ReadIntValue(cellData, ProspCellReferences.TopSide.CostProfileStartYear);
        var dG3Date = ReadDateValue(cellData, ProspCellReferences.TopSide.Dg3Date);
        var dG4Date = ReadDateValue(cellData, ProspCellReferences.TopSide.Dg4Date);
        var artificialLiftInt = ReadIntValue(cellData, ProspCellReferences.TopSide.ArtificialLiftInt);
        var artificialLift = MapArtificialLift(artificialLiftInt);
        var dryWeight = ReadDoubleValue(cellData, ProspCellReferences.TopSide.DryWeight);
        var fuelConsumption = ReadDoubleValue(cellData, ProspCellReferences.TopSide.FuelConsumption);
        var flaredGas = ReadDoubleValue(cellData, ProspCellReferences.TopSide.FlaredGas);
        var oilCapacity = ReadDoubleValue(cellData, ProspCellReferences.TopSide.OilCapacity);
        var gasCapacity = ReadDoubleValue(cellData, ProspCellReferences.TopSide.GasCapacity);
        var waterInjectorCapacity = ReadDoubleValue(cellData, ProspCellReferences.TopSide.WaterInjectionCapacity);
        var producerCount = ReadIntValue(cellData, ProspCellReferences.TopSide.ProducerCount);
        var gasInjectorCount = ReadIntValue(cellData, ProspCellReferences.TopSide.GasInjectorCount);
        var waterInjectorCount = ReadIntValue(cellData, ProspCellReferences.TopSide.WaterInjectorCount);
        var cO2ShareOilProfile = ReadDoubleValue(cellData, ProspCellReferences.TopSide.Co2ShareOilProfile);
        var cO2ShareGasProfile = ReadDoubleValue(cellData, ProspCellReferences.TopSide.Co2ShareGasProfile);
        var cO2ShareWaterInjectionProfile =
            ReadDoubleValue(cellData, ProspCellReferences.TopSide.Co2ShareWaterInjectionProfile);
        var cO2OnMaxOilProfile = ReadDoubleValue(cellData, ProspCellReferences.TopSide.Co2OnMaxOilProfile);
        var cO2OnMaxGasProfile = ReadDoubleValue(cellData, ProspCellReferences.TopSide.Co2OnMaxGasProfile);
        var cO2OnMaxWaterInjectionProfile =
            ReadDoubleValue(cellData, ProspCellReferences.TopSide.Co2OnMaxWaterInjectionProfile);
        var facilityOpex = ReadDoubleValue(cellData, ProspCellReferences.TopSide.FacilityOpex);
        var costProfile = new UpdateTimeSeriesCostDto
        {
            Values = ReadDoubleValues(cellData, costProfileCoords),
            StartYear = costProfileStartYear - dG4Date.Year,
        };
        var peakElectricityImported = ReadDoubleValue(cellData, ProspCellReferences.TopSide.PeakElectricityImported);
        // Prosp meta data
        var versionDate = ReadDateValue(cellData, ProspCellReferences.TopSide.VersionDate);
        var costYear = ReadIntValue(cellData, ProspCellReferences.TopSide.CostYear);
        var importedCurrency = ReadIntValue(cellData, ProspCellReferences.TopSide.ImportedCurrency);
        var currency = importedCurrency == 1 ? Currency.NOK :
            importedCurrency == 2 ? Currency.USD : 0;
        var updateTopsideDto = new ProspUpdateTopsideDto
        {
            DG3Date = dG3Date,
            DG4Date = dG4Date,
            DryWeight = dryWeight,
            OilCapacity = oilCapacity,
            GasCapacity = gasCapacity,
            WaterInjectionCapacity = waterInjectorCapacity,
            ArtificialLift = artificialLift,
            ProducerCount = producerCount,
            WaterInjectorCount = waterInjectorCount,
            GasInjectorCount = gasInjectorCount,
            FuelConsumption = fuelConsumption,
            FlaredGas = flaredGas,
            CO2ShareOilProfile = cO2ShareOilProfile,
            CO2ShareGasProfile = cO2ShareGasProfile,
            CO2ShareWaterInjectionProfile = cO2ShareWaterInjectionProfile,
            CO2OnMaxOilProfile = cO2OnMaxOilProfile,
            CO2OnMaxGasProfile = cO2OnMaxGasProfile,
            CO2OnMaxWaterInjectionProfile = cO2OnMaxWaterInjectionProfile,
            Source = Source.Prosp,
            ProspVersion = versionDate,
            Currency = currency,
            CostYear = costYear,
            FacilityOpex = facilityOpex,
            PeakElectricityImported = peakElectricityImported
        };

        await updateTopsideService.UpdateTopside(projectId, caseItem.Id, caseItem.TopsideLink, updateTopsideDto);
        await topsideCostProfileService.AddOrUpdateTopsideCostProfile(projectId, caseItem.Id, caseItem.TopsideLink, costProfile);
    }

    private async Task ImportSubstructure(List<Cell> cellData, Guid projectId, Case caseItem)
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
        var costProfileStartYear = ReadIntValue(cellData, ProspCellReferences.SubStructure.CostProfileStartYear);
        var dG3Date = ReadDateValue(cellData, ProspCellReferences.SubStructure.Dg3Date);
        var dG4Date = ReadDateValue(cellData, ProspCellReferences.SubStructure.Dg4Date);
        var dryWeight = ReadDoubleValue(cellData, ProspCellReferences.SubStructure.DryWeight);
        var conceptInt = ReadIntValue(cellData, ProspCellReferences.SubStructure.ConceptInt);
        var concept = MapSubstructureConcept(conceptInt);
        var costProfile = new UpdateTimeSeriesCostDto
        {
            Values = ReadDoubleValues(cellData, costProfileCoords),
            StartYear = costProfileStartYear - dG4Date.Year
        };

        // Prosp meta data
        var versionDate = ReadDateValue(cellData, ProspCellReferences.SubStructure.VersionDate);
        var costYear = ReadIntValue(cellData, ProspCellReferences.SubStructure.CostYear);
        var importedCurrency = ReadIntValue(cellData, ProspCellReferences.SubStructure.ImportedCurrency);
        var currency = importedCurrency == 1 ? Currency.NOK :
            importedCurrency == 2 ? Currency.USD : 0;
        var updateSubstructureDto = new ProspUpdateSubstructureDto
        {
            DryWeight = dryWeight,
            Concept = concept,
            DG3Date = dG3Date,
            DG4Date = dG4Date,
            Source = Source.Prosp,
            ProspVersion = versionDate,
            Currency = currency,
            CostYear = costYear
        };

        await updateSubstructureService.UpdateSubstructure(projectId, caseItem.Id, caseItem.SubstructureLink, updateSubstructureDto);
        await substructureCostProfileService.AddOrUpdateSubstructureCostProfile(projectId, caseItem.Id, caseItem.SubstructureLink, costProfile);
    }

    private async Task ImportTransport(List<Cell> cellData, Guid projectId, Case caseItem)
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
        var costProfileStartYear = ReadIntValue(cellData, ProspCellReferences.Transport.CostProfileStartYear);
        var dG3Date = ReadDateValue(cellData, ProspCellReferences.Transport.Dg3Date);
        var dG4Date = ReadDateValue(cellData, ProspCellReferences.Transport.Dg4Date);
        var costProfile = new UpdateTimeSeriesCostDto
        {
            Values = ReadDoubleValues(cellData, costProfileCoords),
            StartYear = costProfileStartYear - dG4Date.Year,
        };
        // Prosp meta data
        var versionDate = ReadDateValue(cellData, ProspCellReferences.Transport.VersionDate);
        var costYear = ReadIntValue(cellData, ProspCellReferences.Transport.CostYear);
        var importedCurrency = ReadIntValue(cellData, ProspCellReferences.Transport.ImportedCurrency);
        var oilExportPipelineLength = ReadDoubleValue(cellData, ProspCellReferences.Transport.OilExportPipelineLength);
        var gasExportPipelineLength = ReadDoubleValue(cellData, ProspCellReferences.Transport.GasExportPipelineLength);
        var currency = importedCurrency == 1 ? Currency.NOK :
            importedCurrency == 2 ? Currency.USD : 0;
        var updateTransportDto = new ProspUpdateTransportDto
        {
            DG3Date = dG3Date,
            DG4Date = dG4Date,
            Source = Source.Prosp,
            ProspVersion = versionDate,
            Currency = currency,
            CostYear = costYear,
            OilExportPipelineLength = oilExportPipelineLength,
            GasExportPipelineLength = gasExportPipelineLength
        };

        await updateTransportService.UpdateTransport(projectId, caseItem.Id, caseItem.TransportLink, updateTransportDto);
        await transportCostProfileService.AddOrUpdateTransportCostProfile(projectId, caseItem.Id, caseItem.TransportLink, costProfile);
    }

    private async Task ImportOnshorePowerSupply(List<Cell> cellData, Guid projectId, Case caseItem)
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
        var costProfileStartYear = ReadIntValue(cellData, ProspCellReferences.OnshorePowerSupply.CostProfileStartYear);
        var dG4Date = ReadDateValue(cellData, ProspCellReferences.OnshorePowerSupply.Dg4Date);

        var costProfile = new UpdateTimeSeriesCostDto
        {
            Values = ReadDoubleValues(cellData, costProfileCoords),
            StartYear = costProfileStartYear - dG4Date.Year,
        };

        await onshorePowerSupplyCostProfileService.AddOrUpdateOnshorePowerSupplyCostProfile(projectId, caseItem.Id, caseItem.OnshorePowerSupplyLink, costProfile);
    }

    public async Task ImportProsp(Stream stream,
        Guid sourceCaseId,
        Guid projectId,
        Dictionary<string, bool> assets,
        string sharepointFileId,
        string? sharepointFileName,
        string? sharepointFileUrl)
    {
        using var document = SpreadsheetDocument.Open(stream, false);
        var workbookPart = document.WorkbookPart;
        var mainSheet = workbookPart?.Workbook.Descendants<Sheet>()
            .FirstOrDefault(x => x.Name?.ToString()?.ToLower() == SheetName);

        var caseItem = await context.Cases
            .Include(c => c.TimeSeriesProfiles)
            .Include(c => c.TotalOtherStudiesCostProfile)
            .Include(c => c.HistoricCostCostProfile)
            .Include(c => c.OnshoreRelatedOPEXCostProfile)
            .Include(c => c.AdditionalOPEXCostProfile)
            .Include(c => c.CalculatedTotalIncomeCostProfile)
            .Include(c => c.CalculatedTotalCostCostProfile)
            .SingleAsync(c => c.Id == sourceCaseId);

        caseItem.SharepointFileId = sharepointFileId;
        caseItem.SharepointFileName = sharepointFileName;
        caseItem.SharepointFileUrl = sharepointFileUrl;

        if (mainSheet?.Id != null && workbookPart != null)
        {
            var wsPart = (WorksheetPart)workbookPart.GetPartById(mainSheet.Id!);
            var cellData = wsPart?.Worksheet.Descendants<Cell>();

            if (cellData != null)
            {
                var parsedData = cellData.ToList();
                if (assets["Surf"])
                {
                    await ImportSurf(parsedData, projectId, caseItem);
                }
                else
                {
                    await ClearImportedSurf(caseItem);
                }

                if (assets["Topside"])
                {
                    await ImportTopside(parsedData, projectId, caseItem);
                }
                else
                {
                    await ClearImportedTopside(caseItem);
                }

                if (assets["Substructure"])
                {
                    await ImportSubstructure(parsedData, projectId, caseItem);
                }
                else
                {
                    await ClearImportedSubstructure(caseItem);
                }

                if (assets["Transport"])
                {
                    await ImportTransport(parsedData, projectId, caseItem);
                }
                else
                {
                    await ClearImportedTransport(caseItem);
                }

                if (assets["OnshorePowerSupply"])
                {
                    await ImportOnshorePowerSupply(parsedData, projectId, caseItem);
                }
                else
                {
                    await ClearImportedOnshorePowerSupply(caseItem);
                }
            }

            var caseDto = new ProspUpdateCaseDto
            {
                SharepointFileId = sharepointFileId,
                SharepointFileName = sharepointFileName,
                SharepointFileUrl = sharepointFileUrl
            };

            await UpdateCase(projectId, sourceCaseId, caseDto);
        }
    }

    public async Task ClearImportedProspData(Guid sourceCaseId, Guid projectId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var caseItem = await context.Cases.SingleAsync(x => x.Id == sourceCaseId);

        caseItem.SharepointFileId = null;
        caseItem.SharepointFileName = null;
        caseItem.SharepointFileUrl = null;

        await ClearImportedSurf(caseItem);
        await ClearImportedTopside(caseItem);
        await ClearImportedSubstructure(caseItem);
        await ClearImportedTransport(caseItem);
        await ClearImportedOnshorePowerSupply(caseItem);

        var caseDto = new ProspUpdateCaseDto
        {
            SharepointFileId = caseItem.SharepointFileId,
            SharepointFileName = caseItem.SharepointFileName,
            SharepointFileUrl = caseItem.SharepointFileUrl
        };

        await UpdateCase(projectPk, sourceCaseId, caseDto);
    }

    private async Task UpdateCase(Guid projectPk, Guid caseId, ProspUpdateCaseDto updatedCaseDto)
    {
        var existingCase = await context.Cases
            .Where(x => x.ProjectId == projectPk)
            .Where(x => x.Id == caseId)
            .SingleAsync();

        existingCase.SharepointFileId = updatedCaseDto.SharepointFileId;
        existingCase.SharepointFileName = updatedCaseDto.SharepointFileName;
        existingCase.SharepointFileUrl = updatedCaseDto.SharepointFileUrl;

        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }

    private async Task ClearImportedSurf(Case caseItem)
    {
        await updateSurfService.UpdateSurf(caseItem.ProjectId,
            caseItem.Id,
            caseItem.SurfLink,
            new ProspUpdateSurfDto
            {
                Source = Source.ConceptApp
            });

        await surfCostProfileService.AddOrUpdateSurfCostProfile(caseItem.ProjectId,
            caseItem.Id,
            caseItem.SurfLink,
            new UpdateTimeSeriesCostDto());
    }

    private async Task ClearImportedTopside(Case caseItem)
    {
        await updateTopsideService.UpdateTopside(caseItem.ProjectId,
            caseItem.Id,
            caseItem.TopsideLink,
            new ProspUpdateTopsideDto
            {
                Source = Source.ConceptApp
            });

        await topsideCostProfileService.AddOrUpdateTopsideCostProfile(caseItem.ProjectId,
            caseItem.Id,
            caseItem.TopsideLink,
            new UpdateTimeSeriesCostDto());
    }

    private async Task ClearImportedSubstructure(Case caseItem)
    {
        await updateSubstructureService.UpdateSubstructure(caseItem.ProjectId,
            caseItem.Id,
            caseItem.SubstructureLink,
            new ProspUpdateSubstructureDto
            {
                Source = Source.ConceptApp
            });

        await substructureCostProfileService.AddOrUpdateSubstructureCostProfile(caseItem.ProjectId,
            caseItem.Id,
            caseItem.SubstructureLink,
            new UpdateTimeSeriesCostDto());
    }

    private async Task ClearImportedTransport(Case caseItem)
    {
        await updateTransportService.UpdateTransport(caseItem.ProjectId,
            caseItem.Id,
            caseItem.TransportLink,
            new ProspUpdateTransportDto
            {
                Source = Source.ConceptApp
            });

        await transportCostProfileService.AddOrUpdateTransportCostProfile(caseItem.ProjectId,
            caseItem.Id,
            caseItem.TransportLink,
            new UpdateTimeSeriesCostDto());
    }

    private async Task ClearImportedOnshorePowerSupply(Case caseItem)
    {
        await updateOnshorePowerSupplyService.UpdateOnshorePowerSupply(caseItem.ProjectId,
            caseItem.Id,
            caseItem.OnshorePowerSupplyLink,
            new ProspUpdateOnshorePowerSupplyDto
            {
                Source = Source.ConceptApp
            });

        await onshorePowerSupplyCostProfileService.AddOrUpdateOnshorePowerSupplyCostProfile(caseItem.ProjectId,
            caseItem.Id,
            caseItem.OnshorePowerSupplyLink,
            new UpdateTimeSeriesCostDto());
    }

    private static Concept MapSubstructureConcept(int importValue)
    {
        return importValue switch
        {
            0 => Concept.TIE_BACK,
            1 => Concept.JACKET,
            2 => Concept.GBS,
            3 => Concept.TLP,
            4 => Concept.SPAR,
            5 => Concept.SEMI,
            6 => Concept.CIRCULAR_BARGE,
            7 => Concept.BARGE,
            8 => Concept.FPSO,
            9 => Concept.TANKER,
            10 => Concept.JACK_UP,
            11 => Concept.SUBSEA_TO_SHORE,
            _ => Concept.NO_CONCEPT
        };
    }

    private static ArtificialLift MapArtificialLift(int importValue)
    {
        return importValue switch
        {
            0 => ArtificialLift.NoArtificialLift,
            1 => ArtificialLift.GasLift,
            2 => ArtificialLift.ElectricalSubmergedPumps,
            3 => ArtificialLift.SubseaBoosterPumps,
            _ => ArtificialLift.NoArtificialLift
        };
    }

    private static ProductionFlowline MapProductionFlowLine(int importValue)
    {
        return importValue switch
        {
            1 => ProductionFlowline.Carbon,
            2 => ProductionFlowline.SSClad,
            3 => ProductionFlowline.Cr13,
            11 => ProductionFlowline.Carbon_Insulation,
            12 => ProductionFlowline.SSClad_Insulation,
            13 => ProductionFlowline.Cr13_Insulation,
            21 => ProductionFlowline.Carbon_Insulation_DEH,
            22 => ProductionFlowline.SSClad_Insulation_DEH,
            23 => ProductionFlowline.Cr13_Insulation_DEH,
            31 => ProductionFlowline.Carbon_PIP,
            32 => ProductionFlowline.SSClad_PIP,
            33 => ProductionFlowline.Cr13_PIP,
            41 => ProductionFlowline.HDPELinedCS,
            _ => ProductionFlowline.No_production_flowline
        };
    }
}
