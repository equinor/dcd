using api.Dtos;
using api.Helpers.Prosp;
using api.Models;

using AutoMapper;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using Surf = api.Helpers.Prosp.Surf;
using Transport = api.Helpers.Prosp.Transport;

namespace api.Services;

public class ProspExcelImportService
{
    private const string SheetName = "main";
    private readonly ICaseService _caseService;
    private readonly Prosp _prospConfig;
    private readonly ISubstructureService _substructureService;
    private readonly ISurfService _surfService;
    private readonly ITopsideService _topsideService;
    private readonly ITransportService _transportService;
    private readonly ISubstructureTimeSeriesService _substructureTimeSeriesService;
    private readonly ISurfTimeSeriesService _surfTimeSeriesService;
    private readonly ITopsideTimeSeriesService _topsideTimeSeriesService;
    private readonly ITransportTimeSeriesService _transportTimeSeriesService;
    private readonly IMapper _mapper;


    public ProspExcelImportService(
        ICaseService caseService,
        ILoggerFactory loggerFactory,
        ISurfService surfService,
        ISubstructureService substructureService,
        ITopsideService topsideService,
        ITransportService transportService,
        ISubstructureTimeSeriesService substructureTimeSeriesService,
        ISurfTimeSeriesService surfTimeSeriesService,
        ITopsideTimeSeriesService topsideTimeSeriesService,
        ITransportTimeSeriesService transportTimeSeriesService,
        IConfiguration config,
        IMapper mapper
    )
    {
        loggerFactory.CreateLogger<ProspExcelImportService>();
        _surfService = surfService;
        _substructureService = substructureService;
        _topsideService = topsideService;
        _transportService = transportService;
        _substructureTimeSeriesService = substructureTimeSeriesService;
        _surfTimeSeriesService = surfTimeSeriesService;
        _topsideTimeSeriesService = topsideTimeSeriesService;
        _transportTimeSeriesService = transportTimeSeriesService;
        _prospConfig = CreateConfig(config);
        _caseService = caseService;
        _mapper = mapper;
    }

    private Prosp CreateConfig(IConfiguration config)
    {
        var prospImportConfig = config.GetSection("FileImportSettings:Prosp").Get<Prosp>();
        if (prospImportConfig == null)
        {
            throw new Exception("Prosp import settings not found in appsettings.json");
        }
        prospImportConfig.Surf = config.GetSection("FileImportSettings:Prosp:Surf").Get<Surf>() ?? new Surf();
        prospImportConfig.SubStructure = config.GetSection("FileImportSettings:Prosp:SubStructure").Get<SubStructure>() ?? new SubStructure();
        prospImportConfig.TopSide = config.GetSection("FileImportSettings:Prosp:TopSide").Get<TopSide>() ?? new TopSide();
        prospImportConfig.Transport = config.GetSection("FileImportSettings:Prosp:Transport").Get<Transport>() ?? new Transport();
        return prospImportConfig;
    }

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
        foreach (var cell in cellData.Where(c => c?.CellReference != null && coordinates.Contains(c.CellReference!)))
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

    private async Task ImportSurf(List<Cell> cellData, Guid sourceCaseId, Guid projectId)
    {
        List<string> costProfileCoords = new()
        {
            "J112",
            "K112",
            "L112",
            "M112",
            "N112",
            "O112",
            "P112",
        };
        var costProfileStartYear = ReadIntValue(cellData, _prospConfig.Surf.costProfileStartYear);
        var dG3Date = ReadDateValue(cellData, _prospConfig.Surf.dG3Date);
        var dG4Date = ReadDateValue(cellData, _prospConfig.Surf.dG4Date);
        var lengthProductionLine = ReadDoubleValue(cellData, _prospConfig.Surf.lengthProductionLine);
        var lengthUmbilicalSystem = ReadDoubleValue(cellData, _prospConfig.Surf.lengthUmbilicalSystem);
        var productionFlowLineInt = ReadIntValue(cellData, _prospConfig.Surf.productionFlowLineInt);
        var productionFlowLine = MapProductionFlowLine(productionFlowLineInt);
        var artificialLiftInt = ReadIntValue(cellData, _prospConfig.Surf.artificialLiftInt);
        var artificialLift = MapArtificialLift(artificialLiftInt);
        var riserCount = ReadIntValue(cellData, _prospConfig.Surf.riserCount);
        var templateCount = ReadIntValue(cellData, _prospConfig.Surf.templateCount);
        var producerCount = ReadIntValue(cellData, _prospConfig.Surf.producerCount);
        var waterInjectorCount = ReadIntValue(cellData, _prospConfig.Surf.waterInjectorCount);
        var gasInjectorCount = ReadIntValue(cellData, _prospConfig.Surf.gasInjectorCount);
        var cessationCost = ReadDoubleValue(cellData, _prospConfig.Surf.cessationCost);
        var costProfile = new UpdateSurfCostProfileDto
        {
            Values = ReadDoubleValues(cellData, costProfileCoords),
            StartYear = costProfileStartYear - dG4Date.Year,
        };

        // Prosp meta data
        var versionDate = ReadDateValue(cellData, _prospConfig.Surf.versionDate);
        var costYear = ReadIntValue(cellData, _prospConfig.Surf.costYear);
        var importedCurrency = ReadIntValue(cellData, _prospConfig.Surf.importedCurrency);
        var currency = importedCurrency == 1 ? Currency.NOK :
            importedCurrency == 2 ? Currency.USD : 0;
        var surfLink = (await _caseService.GetCase(sourceCaseId)).SurfLink;

        var updatedSurfDto = new PROSPUpdateSurfDto
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

        await _surfService.UpdateSurf(projectId, sourceCaseId, surfLink, updatedSurfDto);
        await _surfTimeSeriesService.AddOrUpdateSurfCostProfile(projectId, sourceCaseId, surfLink, costProfile);
    }

    private async Task ImportTopside(List<Cell> cellData, Guid sourceCaseId, Guid projectId)
    {
        List<string> costProfileCoords = new()
        {
            "J104",
            "K104",
            "L104",
            "M104",
            "N104",
            "O104",
            "P104",
        };
        var costProfileStartYear = ReadIntValue(cellData, _prospConfig.TopSide.costProfileStartYear);
        var dG3Date = ReadDateValue(cellData, _prospConfig.TopSide.dG3Date);
        var dG4Date = ReadDateValue(cellData, _prospConfig.TopSide.dG4Date);
        var artificialLiftInt = ReadIntValue(cellData, _prospConfig.TopSide.artificialLiftInt);
        var artificialLift = MapArtificialLift(artificialLiftInt);
        var dryWeight = ReadDoubleValue(cellData, _prospConfig.TopSide.dryWeight);
        var fuelConsumption = ReadDoubleValue(cellData, _prospConfig.TopSide.fuelConsumption);
        var flaredGas = ReadDoubleValue(cellData, _prospConfig.TopSide.flaredGas);
        var oilCapacity = ReadDoubleValue(cellData, _prospConfig.TopSide.oilCapacity);
        var gasCapacity = ReadDoubleValue(cellData, _prospConfig.TopSide.gasCapacity);
        var waterInjectorCapacity = ReadDoubleValue(cellData, _prospConfig.TopSide.waterInjectionCapacity);
        var producerCount = ReadIntValue(cellData, _prospConfig.TopSide.producerCount);
        var gasInjectorCount = ReadIntValue(cellData, _prospConfig.TopSide.gasInjectorCount);
        var waterInjectorCount = ReadIntValue(cellData, _prospConfig.TopSide.waterInjectorCount);
        var cO2ShareOilProfile = ReadDoubleValue(cellData, _prospConfig.TopSide.cO2ShareOilProfile);
        var cO2ShareGasProfile = ReadDoubleValue(cellData, _prospConfig.TopSide.cO2ShareGasProfile);
        var cO2ShareWaterInjectionProfile =
            ReadDoubleValue(cellData, _prospConfig.TopSide.cO2ShareWaterInjectionProfile);
        var cO2OnMaxOilProfile = ReadDoubleValue(cellData, _prospConfig.TopSide.cO2OnMaxOilProfile);
        var cO2OnMaxGasProfile = ReadDoubleValue(cellData, _prospConfig.TopSide.cO2OnMaxGasProfile);
        var cO2OnMaxWaterInjectionProfile =
            ReadDoubleValue(cellData, _prospConfig.TopSide.cO2OnMaxWaterInjectionProfile);
        var facilityOpex = ReadDoubleValue(cellData, _prospConfig.TopSide.facilityOpex);
        var costProfile = new UpdateTopsideCostProfileDto
        {
            Values = ReadDoubleValues(cellData, costProfileCoords),
            StartYear = costProfileStartYear - dG4Date.Year,
        };
        var peakElectricityImported = ReadDoubleValue(cellData, _prospConfig.TopSide.peakElectricityImported);
        // Prosp meta data
        var versionDate = ReadDateValue(cellData, _prospConfig.TopSide.versionDate);
        var costYear = ReadIntValue(cellData, _prospConfig.TopSide.costYear);
        var importedCurrency = ReadIntValue(cellData, _prospConfig.TopSide.importedCurrency);
        var currency = importedCurrency == 1 ? Currency.NOK :
            importedCurrency == 2 ? Currency.USD : 0;
        var topsideLink = (await _caseService.GetCase(sourceCaseId)).TopsideLink;
        var updateTopsideDto = new PROSPUpdateTopsideDto
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
            PeakElectricityImported = peakElectricityImported,
        };

        await _topsideService.UpdateTopside(projectId, sourceCaseId, topsideLink, updateTopsideDto);
        await _topsideTimeSeriesService.AddOrUpdateTopsideCostProfile(projectId, sourceCaseId, topsideLink, costProfile);
    }

    private async Task ImportSubstructure(List<Cell> cellData, Guid sourceCaseId, Guid projectId)
    {
        List<string> costProfileCoords = new()
        {
            "J105",
            "K105",
            "L105",
            "M105",
            "N105",
            "O105",
            "P105",
        };
        var costProfileStartYear = ReadIntValue(cellData, _prospConfig.SubStructure.costProfileStartYear);
        var dG3Date = ReadDateValue(cellData, _prospConfig.SubStructure.dG3Date);
        var dG4Date = ReadDateValue(cellData, _prospConfig.SubStructure.dG4Date);
        var dryWeight = ReadDoubleValue(cellData, _prospConfig.SubStructure.dryWeight);
        var conceptInt = ReadIntValue(cellData, _prospConfig.SubStructure.conceptInt);
        var concept = MapSubstructureConcept(conceptInt);
        var costProfile = new UpdateSubstructureCostProfileDto
        {
            Values = ReadDoubleValues(cellData, costProfileCoords),
            StartYear = costProfileStartYear - dG4Date.Year,
        };

        // Prosp meta data
        var versionDate = ReadDateValue(cellData, _prospConfig.SubStructure.versionDate);
        var costYear = ReadIntValue(cellData, _prospConfig.SubStructure.costYear);
        var importedCurrency = ReadIntValue(cellData, _prospConfig.SubStructure.importedCurrency);
        var currency = importedCurrency == 1 ? Currency.NOK :
            importedCurrency == 2 ? Currency.USD : 0;
        var substructureLink = (await _caseService.GetCase(sourceCaseId)).SubstructureLink;
        var updateSubstructureDto = new PROSPUpdateSubstructureDto
        {
            DryWeight = dryWeight,
            Concept = concept,
            DG3Date = dG3Date,
            DG4Date = dG4Date,
            Source = Source.Prosp,
            ProspVersion = versionDate,
            Currency = currency,
            CostYear = costYear,
        };

        await _substructureService.UpdateSubstructure(projectId, sourceCaseId, substructureLink, updateSubstructureDto);
        await _substructureTimeSeriesService.AddOrUpdateSubstructureCostProfile(projectId, sourceCaseId, substructureLink, costProfile);
    }

    private async Task ImportTransport(List<Cell> cellData, Guid sourceCaseId, Guid projectId)
    {
        List<string> costProfileCoords = new()
        {
            "J113",
            "K113",
            "L113",
            "M113",
            "N113",
            "O113",
            "P113",
        };
        var costProfileStartYear = ReadIntValue(cellData, _prospConfig.Transport.costProfileStartYear);
        var dG3Date = ReadDateValue(cellData, _prospConfig.Transport.dG3Date);
        var dG4Date = ReadDateValue(cellData, _prospConfig.Transport.dG4Date);
        var costProfile = new UpdateTransportCostProfileDto
        {
            Values = ReadDoubleValues(cellData, costProfileCoords),
            StartYear = costProfileStartYear - dG4Date.Year,
        };

        // Prosp meta data
        var versionDate = ReadDateValue(cellData, _prospConfig.Transport.versionDate);
        var costYear = ReadIntValue(cellData, _prospConfig.Transport.costYear);
        var importedCurrency = ReadIntValue(cellData, _prospConfig.Transport.importedCurrency);
        var oilExportPipelineLength = ReadDoubleValue(cellData, _prospConfig.Transport.oilExportPipelineLength);
        var gasExportPipelineLength = ReadDoubleValue(cellData, _prospConfig.Transport.gasExportPipelineLength);
        var currency = importedCurrency == 1 ? Currency.NOK :
            importedCurrency == 2 ? Currency.USD : 0;
        var transportLink = (await _caseService.GetCase(sourceCaseId)).TransportLink;
        var updateTransportDto = new PROSPUpdateTransportDto
        {
            DG3Date = dG3Date,
            DG4Date = dG4Date,
            Source = Source.Prosp,
            ProspVersion = versionDate,
            Currency = currency,
            CostYear = costYear,
            OilExportPipelineLength = oilExportPipelineLength,
            GasExportPipelineLength = gasExportPipelineLength,
        };

        await _transportService.UpdateTransport(projectId, sourceCaseId, transportLink, updateTransportDto);
        await _transportTimeSeriesService.AddOrUpdateTransportCostProfile(projectId, sourceCaseId, transportLink, costProfile);
    }

    public async Task ImportProsp(Stream stream, Guid sourceCaseId, Guid projectId, Dictionary<string, bool> assets,
        string sharepointFileId, string? sharepointFileName, string? sharepointFileUrl)
    {
        using var document = SpreadsheetDocument.Open(stream, false);
        var workbookPart = document.WorkbookPart;
        var mainSheet = workbookPart?.Workbook.Descendants<Sheet>()
            .FirstOrDefault(x => x.Name?.ToString()?.ToLower() == SheetName);

        var caseItem = await _caseService.GetCase(sourceCaseId);
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
                    await ImportSurf(parsedData, sourceCaseId, projectId);
                }
                else
                {
                    await ClearImportedSurf(caseItem);
                }

                if (assets["Topside"])
                {
                    await ImportTopside(parsedData, sourceCaseId, projectId);
                }
                else
                {
                    await ClearImportedTopside(caseItem);
                }

                if (assets["Substructure"])
                {
                    await ImportSubstructure(parsedData, sourceCaseId, projectId);
                }
                else
                {
                    await ClearImportedSubstructure(caseItem);
                }

                if (assets["Transport"])
                {
                    await ImportTransport(parsedData, sourceCaseId, projectId);
                }
                else
                {
                    await ClearImportedTransport(caseItem);
                }
            }

            var caseDto = new PROSPUpdateCaseDto
            {
                SharepointFileId = sharepointFileId,
                SharepointFileName = sharepointFileName,
                SharepointFileUrl = sharepointFileUrl,
            };

            await _caseService.UpdateCase(projectId, sourceCaseId, caseDto);
        }
    }

    public async Task ClearImportedProspData(Guid sourceCaseId, Guid projectId)
    {
        var caseItem = await _caseService.GetCase(sourceCaseId);
        caseItem.SharepointFileId = null;
        caseItem.SharepointFileName = null;
        caseItem.SharepointFileUrl = null;

        await ClearImportedSurf(caseItem);
        await ClearImportedTopside(caseItem);
        await ClearImportedSubstructure(caseItem);
        await ClearImportedTransport(caseItem);

        var caseDto = _mapper.Map<PROSPUpdateCaseDto>(caseItem);

        if (caseDto == null)
        {
            throw new Exception();
        }

        await _caseService.UpdateCase(projectId, sourceCaseId, caseDto);
    }

    private async Task ClearImportedSurf(Case caseItem)
    {
        var surfLink = caseItem.SurfLink;
        var dto = new PROSPUpdateSurfDto
        {
            Source = Source.ConceptApp,
        };

        var costProfileDto = new UpdateSurfCostProfileDto();

        await _surfService.UpdateSurf(caseItem.ProjectId, caseItem.Id, surfLink, dto);
        await _surfTimeSeriesService.AddOrUpdateSurfCostProfile(caseItem.ProjectId, caseItem.Id, surfLink, costProfileDto);
    }

    private async Task ClearImportedTopside(Case caseItem)
    {
        var topsideLink = caseItem.TopsideLink;
        var dto = new PROSPUpdateTopsideDto
        {
            Source = Source.ConceptApp,
        };

        var costProfileDto = new UpdateTopsideCostProfileDto();


        await _topsideService.UpdateTopside(caseItem.ProjectId, caseItem.Id, topsideLink, dto);
        await _topsideTimeSeriesService.AddOrUpdateTopsideCostProfile(caseItem.ProjectId, caseItem.Id, topsideLink, costProfileDto);
    }

    private async Task ClearImportedSubstructure(Case caseItem)
    {
        var substructureLink = caseItem.SubstructureLink;
        var dto = new PROSPUpdateSubstructureDto
        {
            Source = Source.ConceptApp,
        };

        var costProfileDto = new UpdateSubstructureCostProfileDto();

        await _substructureService.UpdateSubstructure(caseItem.ProjectId, caseItem.Id, substructureLink, dto);
        await _substructureTimeSeriesService.AddOrUpdateSubstructureCostProfile(caseItem.ProjectId, caseItem.Id, substructureLink, costProfileDto);
    }

    private async Task ClearImportedTransport(Case caseItem)
    {
        var transportLink = caseItem.TransportLink;
        var dto = new PROSPUpdateTransportDto
        {
            Source = Source.ConceptApp,
        };

        var costProfileDto = new UpdateTransportCostProfileDto();

        await _transportService.UpdateTransport(caseItem.ProjectId, caseItem.Id, transportLink, dto);
        await _transportTimeSeriesService.AddOrUpdateTransportCostProfile(caseItem.ProjectId, caseItem.Id, transportLink, costProfileDto);
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
            _ => Concept.NO_CONCEPT,
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
            _ => ArtificialLift.NoArtificialLift,
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
            _ => ProductionFlowline.No_production_flowline,
        };
    }
}
