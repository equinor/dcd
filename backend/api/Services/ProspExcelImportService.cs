using api.Adapters;
using api.Dtos;
using api.Helpers;
using api.Models;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using Surf = api.Helpers.Surf;
using Transport = api.Helpers.Transport;

namespace api.Services;

public class ProspExcelImportService
{
    private const string SheetName = "main";
    private readonly ICaseService _caseService;
    private readonly IProjectService _projectService;
    private readonly Prosp _prospConfig;
    private readonly ISubstructureService _substructureService;
    private readonly ISurfService _surfService;
    private readonly ITopsideService _topsideService;
    private readonly ITransportService _transportService;


    public ProspExcelImportService(IProjectService projectService, ICaseService caseService, ILoggerFactory loggerFactory,
        ISurfService surfService,
        ISubstructureService substructureService, ITopsideService topsideService, ITransportService transportService,
        IConfiguration config)
    {
        _projectService = projectService;
        loggerFactory.CreateLogger<ProspExcelImportService>();
        _surfService = surfService;
        _substructureService = substructureService;
        _topsideService = topsideService;
        _transportService = transportService;
        _prospConfig = CreateConfig(config);
        _caseService = caseService;
    }

    private Prosp CreateConfig(IConfiguration config)
    {
        var prospImportConfig = config.GetSection("FileImportSettings:Prosp").Get<Prosp>();
        prospImportConfig.Surf = config.GetSection("FileImportSettings:Prosp:Surf").Get<Surf>();
        prospImportConfig.SubStructure = config.GetSection("FileImportSettings:Prosp:SubStructure").Get<SubStructure>();
        prospImportConfig.TopSide = config.GetSection("FileImportSettings:Prosp:TopSide").Get<TopSide>();
        prospImportConfig.Transport = config.GetSection("FileImportSettings:Prosp:Transport").Get<Transport>();
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
        foreach (var cell in cellData.Where(c => coordinates.Contains(c.CellReference)))
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
        var costProfile = new SurfCostProfile
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
        var newSurf = new Models.Surf
        {
            Id = surfLink,
            Name = "ImportedSurf",
            CostProfile = costProfile,
            ProjectId = projectId,
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
            Maturity = Maturity.A,
            DG3Date = dG3Date,
            DG4Date = dG4Date,
            ProducerCount = producerCount,
            GasInjectorCount = gasInjectorCount,
            WaterInjectorCount = waterInjectorCount,
            CessationCost = cessationCost,
        };

        var dto = SurfDtoAdapter.Convert(newSurf);

        if (surfLink != Guid.Empty)
        {
            await _surfService.UpdateSurf(dto);
        }
        else
        {
            await _surfService.CreateSurf(dto, sourceCaseId);
        }
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
        var costProfile = new TopsideCostProfile
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
        var newTopside = new Topside
        {
            Id = topsideLink,
            Name = "ImportedTopside",
            CostProfile = costProfile,
            ProjectId = projectId,
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
            Maturity = Maturity.A,
            FacilityOpex = facilityOpex,
            PeakElectricityImported = peakElectricityImported,
        };
        var dto = TopsideDtoAdapter.Convert(newTopside);
        if (topsideLink != Guid.Empty)
        {
            await _topsideService.UpdateTopside(dto);
        }
        else
        {
            await _topsideService.CreateTopside(dto, sourceCaseId);
        }
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
        var costProfile = new SubstructureCostProfile
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
        var newSubstructure = new Substructure
        {
            Id = substructureLink,
            Name = "ImportedSubstructure",
            CostProfile = costProfile,
            ProjectId = projectId,
            DryWeight = dryWeight,
            Concept = concept,
            DG3Date = dG3Date,
            DG4Date = dG4Date,
            Source = Source.Prosp,
            ProspVersion = versionDate,
            Currency = currency,
            CostYear = costYear,
            Maturity = Maturity.A,
        };

        if (substructureLink != Guid.Empty)
        {
            var dto = SubstructureDtoAdapter.Convert(newSubstructure);
            await _substructureService.UpdateSubstructure(dto);
        }
        else
        {
            await _substructureService.CreateSubstructure(newSubstructure, sourceCaseId);
        }
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
        var costProfile = new TransportCostProfile
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
        var newTransport = new Models.Transport
        {
            Id = transportLink,
            Name = "ImportedTransport",
            CostProfile = costProfile,
            ProjectId = projectId,
            DG3Date = dG3Date,
            DG4Date = dG4Date,
            Source = Source.Prosp,
            ProspVersion = versionDate,
            Currency = currency,
            CostYear = costYear,
            OilExportPipelineLength = oilExportPipelineLength,
            GasExportPipelineLength = gasExportPipelineLength,
            Maturity = Maturity.A,
        };
        var dto = TransportDtoAdapter.Convert(newTransport);
        if (transportLink != Guid.Empty)
        {
            await _transportService.UpdateTransport(dto);
        }
        else
        {
            await _transportService.CreateTransport(dto, sourceCaseId);
        }
    }

    public async Task<ProjectDto> ImportProsp(IFormFile file, Guid sourceCaseId, Guid projectId, Dictionary<string, bool> assets)
    {
        using var ms = new MemoryStream();
        file.CopyTo(ms);
        using var document = SpreadsheetDocument.Open(ms, false);
        var workbookPart = document.WorkbookPart;
        var mainSheet = workbookPart?.Workbook.Descendants<Sheet>()
            .FirstOrDefault(x => x.Name?.ToString()?.ToLower() == SheetName);

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

                if (assets["Topside"])
                {
                    await ImportTopside(parsedData, sourceCaseId, projectId);
                }

                if (assets["Substructure"])
                {
                    await ImportSubstructure(parsedData, sourceCaseId, projectId);
                }

                if (assets["Transport"])
                {
                    await ImportTransport(parsedData, sourceCaseId, projectId);
                }
            }
        }

        return await _projectService.GetProjectDto(projectId);
    }

    public async Task<ProjectDto> ImportProsp(Stream stream, Guid sourceCaseId, Guid projectId, Dictionary<string, bool> assets,
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
                    ClearImportedSurf(caseItem);
                }

                if (assets["Topside"])
                {
                    await ImportTopside(parsedData, sourceCaseId, projectId);
                }
                else
                {
                    ClearImportedTopside(caseItem);
                }

                if (assets["Substructure"])
                {
                    await ImportSubstructure(parsedData, sourceCaseId, projectId);
                }
                else
                {
                    ClearImportedSubstructure(caseItem);
                }

                if (assets["Transport"])
                {
                    await ImportTransport(parsedData, sourceCaseId, projectId);
                }
                else
                {
                    ClearImportedTransport(caseItem);
                }
            }

            var caseDto = CaseDtoAdapter.Convert(caseItem);
            return await _caseService.UpdateCase(caseDto);
        }

        return await _projectService.GetProjectDto(projectId);
    }

    public async Task ClearImportedProspData(Guid sourceCaseId, Guid projectId)
    {
        var caseItem = await _caseService.GetCase(sourceCaseId);
        caseItem.SharepointFileId = null;
        caseItem.SharepointFileName = null;
        caseItem.SharepointFileUrl = null;

        ClearImportedSurf(caseItem);
        ClearImportedTopside(caseItem);
        ClearImportedSubstructure(caseItem);
        ClearImportedTransport(caseItem);

        var caseDto = CaseDtoAdapter.Convert(caseItem);
        await _caseService.UpdateCase(caseDto);
    }

    private void ClearImportedSurf(Case caseItem)
    {
        var surfLink = caseItem.SurfLink;
        var surf = new Models.Surf
        {
            Id = surfLink,
            CostProfile = null,
            ProjectId = caseItem.ProjectId,
            Source = Source.ConceptApp,
        };

        var dto = SurfDtoAdapter.Convert(surf);

        if (surfLink != Guid.Empty)
        {
            _surfService.UpdateSurf(dto);
        }
    }

    private void ClearImportedTopside(Case caseItem)
    {
        var topsideLink = caseItem.TopsideLink;
        var topside = new Topside
        {
            Id = topsideLink,
            CostProfile = null,
            ProjectId = caseItem.ProjectId,
            Source = Source.ConceptApp,
        };

        var dto = TopsideDtoAdapter.Convert(topside);

        if (topsideLink != Guid.Empty)
        {
            _topsideService.UpdateTopside(dto);
        }
    }

    private void ClearImportedSubstructure(Case caseItem)
    {
        var substructureLink = caseItem.SubstructureLink;
        var substructure = new Substructure
        {
            Id = substructureLink,
            CostProfile = null,
            ProjectId = caseItem.ProjectId,
            Source = Source.ConceptApp,
        };

        var dto = SubstructureDtoAdapter.Convert(substructure);

        if (substructureLink != Guid.Empty)
        {
            _substructureService.UpdateSubstructure(dto);
        }
    }

    private void ClearImportedTransport(Case caseItem)
    {
        var transportLink = caseItem.TransportLink;
        var transport = new Models.Transport
        {
            Id = transportLink,
            CostProfile = null,
            ProjectId = caseItem.ProjectId,
            Source = Source.ConceptApp,
        };

        var dto = TransportDtoAdapter.Convert(transport);

        if (transportLink != Guid.Empty)
        {
            _transportService.UpdateTransport(dto);
        }
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
