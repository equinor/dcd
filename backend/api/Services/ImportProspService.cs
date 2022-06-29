using api.Adapters;
using api.Dtos;
using api.Models;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace api.Services
{
    public class ImportProspService
    {
        private const string SheetName = "main";
        private readonly ProjectService _projectService;
        private readonly SubstructureService _substructureService;
        private readonly SurfService _surfService;
        private readonly TopsideService _topsideService;
        private readonly TransportService _transportService;

        public ImportProspService(ProjectService projectService, ILoggerFactory loggerFactory, SurfService surfService,
            SubstructureService substructureService, TopsideService topsideService, TransportService transportService)
        {
            _projectService = projectService;
            loggerFactory.CreateLogger<ImportProspService>();
            _surfService = surfService;
            _substructureService = substructureService;
            _topsideService = topsideService;
            _transportService = transportService;
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

            return -1;
        }

        private static double[] ReadDoubleValues(IEnumerable<Cell> cellData, List<string> coordinates)
        {
            var values = new List<double>();
            foreach (var cell in cellData.Where(c => coordinates.Contains(c.CellReference)))
            {
                if (double.TryParse(cell.CellValue?.InnerText.Replace(',', '.'), out var value))
                {
                    values.Add(value);
                }
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

        private void ImportSurf(List<Cell> cellData, Guid sourceCaseId, Guid projectId)
        {
            List<string> costProfileCoords = new()
            {
                "J112",
                "K112",
                "L112",
                "M112",
                "N112",
                "O112",
                "P112"
            };
            var costProfileStartYear = ReadIntValue(cellData, "J103");
            var dG3Date = ReadDateValue(cellData, "F112");
            var dG4Date = ReadDateValue(cellData, "G112");
            var lengthProductionLine = ReadDoubleValue(cellData, "K35");
            var lengthUmbilicalSystem = ReadDoubleValue(cellData, "K37");
            var productionFlowLineInt = ReadIntValue(cellData, "E50");
            var productionFlowLine = MapProductionFlowLine(productionFlowLineInt);
            var artificialLiftInt = ReadIntValue(cellData, "E48");
            var artificialLift = MapArtificialLift(artificialLiftInt);
            var riserCount = ReadIntValue(cellData, "K36");
            var templateCount = ReadIntValue(cellData, "K32");
            var producerCount = ReadIntValue(cellData, "E38");
            var waterInjectorCount = ReadIntValue(cellData, "E39");
            var gasInjectorCount = ReadIntValue(cellData, "E40");

            //TODO: Add cessation cost from PROSP after feedback from PO
            // var cessationCost = ReadDoubleValue(parsedData, "K88");
            var costProfile = new SurfCostProfile
            {
                Values = ReadDoubleValues(cellData, costProfileCoords),
                StartYear = costProfileStartYear - dG4Date.Year
            };

            // Prosp meta data
            var versionDate = ReadDateValue(cellData, "I4");
            var costYear = ReadIntValue(cellData, "K4");
            var importedCurrency = ReadIntValue(cellData, "E8");
            var currency = importedCurrency == 1 ? Currency.NOK :
                importedCurrency == 2 ? Currency.USD : 0;
            var newSurf = new Surf
            {
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
                WaterInjectorCount = waterInjectorCount
            };
            var dto = SurfDtoAdapter.Convert(newSurf);
            _surfService.CreateSurf(dto, sourceCaseId);
        }

        private void ImportTopside(List<Cell> cellData, Guid sourceCaseId, Guid projectId)
        {
            List<string> costProfileCoords = new()
            {
                "J104",
                "K104",
                "L104",
                "M104",
                "N104",
                "O104",
                "P104"
            };
            var costProfileStartYear = ReadIntValue(cellData, "J103");
            var dG3Date = ReadDateValue(cellData, "F104");
            var dG4Date = ReadDateValue(cellData, "G104");
            var artificialLiftInt = ReadIntValue(cellData, "E42");
            var artificialLift = MapArtificialLift(artificialLiftInt);
            var dryWeight = ReadDoubleValue(cellData, "J10");
            var fuelConsumption = ReadDoubleValue(cellData, "K92");
            var flaredGas = ReadDoubleValue(cellData, "K93");
            var oilCapacity = ReadDoubleValue(cellData, "E27");
            var gasCapacity = ReadDoubleValue(cellData, "E29");
            var producerCount = ReadIntValue(cellData, "E38");
            var gasInjectorCount = ReadIntValue(cellData, "E40");
            var waterInjectorCount = ReadIntValue(cellData, "E39");
            var cO2ShareOilProfile = ReadDoubleValue(cellData, "J99");
            var cO2ShareGasProfile = ReadDoubleValue(cellData, "J100");
            var cO2ShareWaterInjectionProfile = ReadDoubleValue(cellData, "J101");
            var cO2OnMaxOilProfile = ReadDoubleValue(cellData, "L99");
            var cO2OnMaxGasProfile = ReadDoubleValue(cellData, "L100");
            var cO2OnMaxWaterInjectionProfile = ReadDoubleValue(cellData, "L101");
            var facilityOpex = ReadDoubleValue(cellData, "L80");
            var costProfile = new TopsideCostProfile
            {
                Values = ReadDoubleValues(cellData, costProfileCoords),
                StartYear = costProfileStartYear - dG4Date.Year
            };

            // Prosp meta data
            var versionDate = ReadDateValue(cellData, "I4");
            var costYear = ReadIntValue(cellData, "K4");
            var importedCurrency = ReadIntValue(cellData, "E8");
            var currency = importedCurrency == 1 ? Currency.NOK :
                importedCurrency == 2 ? Currency.USD : 0;
            var newTopside = new Topside
            {
                Name = "ImportedTopside",
                CostProfile = costProfile,
                ProjectId = projectId,
                DG3Date = dG3Date,
                DG4Date = dG4Date,
                DryWeight = dryWeight,
                OilCapacity = oilCapacity,
                GasCapacity = gasCapacity,
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
                FacilityOpex = facilityOpex
            };
            var dto = TopsideDtoAdapter.Convert(newTopside);
            _topsideService.CreateTopside(dto, sourceCaseId);
        }

        private void ImportSubstructure(List<Cell> cellData, Guid sourceCaseId, Guid projectId)
        {
            List<string> costProfileCoords = new()
            {
                "J105",
                "K105",
                "L105",
                "M105",
                "N105",
                "O105",
                "P105"
            };
            var costProfileStartYear = ReadIntValue(cellData, "J103");
            var dG3Date = ReadDateValue(cellData, "F105");
            var dG4Date = ReadDateValue(cellData, "G105");
            var dryWeight = ReadDoubleValue(cellData, "J19");
            var conceptInt = ReadIntValue(cellData, "E62");
            var concept = MapSubstructureConcept(conceptInt);
            var costProfile = new SubstructureCostProfile
            {
                Values = ReadDoubleValues(cellData, costProfileCoords),
                StartYear = costProfileStartYear - dG4Date.Year
            };

            // Prosp meta data
            var versionDate = ReadDateValue(cellData, "I4");
            var costYear = ReadIntValue(cellData, "K4");
            var importedCurrency = ReadIntValue(cellData, "E8");
            var currency = importedCurrency == 1 ? Currency.NOK :
                importedCurrency == 2 ? Currency.USD : 0;
            var newSubstructure = new Substructure
            {
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
                Maturity = Maturity.A
            };
            _substructureService.CreateSubstructure(newSubstructure, sourceCaseId);
        }

        private void ImportTransport(List<Cell> cellData, Guid sourceCaseId, Guid projectId)
        {
            List<string> costProfileCoords = new()
            {
                "J113",
                "K113",
                "L113",
                "M113",
                "N113",
                "O113",
                "P113"
            };
            var costProfileStartYear = ReadIntValue(cellData, "J103");
            var dG3Date = ReadDateValue(cellData, "F113");
            var dG4Date = ReadDateValue(cellData, "G113");
            var costProfile = new TransportCostProfile
            {
                Values = ReadDoubleValues(cellData, costProfileCoords),
                StartYear = costProfileStartYear - dG4Date.Year
            };

            // Prosp meta data
            var versionDate = ReadDateValue(cellData, "I4");
            var costYear = ReadIntValue(cellData, "K4");
            var oilExportPipelineLength = ReadIntValue(cellData, "E59");
            var gasExportPipelineLength = ReadIntValue(cellData, "E60");
            var importedCurrency = ReadIntValue(cellData, "E8");
            var currency = importedCurrency == 1 ? Currency.NOK :
                importedCurrency == 2 ? Currency.USD : 0;
            var newTransport = new Transport
            {
                Name = "ImportedTransport",
                CostProfile = costProfile,
                ProjectId = projectId,
                DG3Date = dG3Date,
                DG4Date = dG4Date,
                Source = Source.Prosp,
                ProspVersion = versionDate,
                Currency = currency,
                CostYear = costYear,
                Maturity = Maturity.A,
                OilExportPipelineLength = oilExportPipelineLength,
                GasExportPipelineLength = gasExportPipelineLength
            };
            var dto = TransportDtoAdapter.Convert(newTransport);
            _transportService.CreateTransport(dto, sourceCaseId);
        }

        public ProjectDto ImportProsp(IFormFile file, Guid sourceCaseId, Guid projectId, Dictionary<string, bool> assets)
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
                        ImportSurf(parsedData, sourceCaseId, projectId);
                    }

                    if (assets["Topside"])
                    {
                        ImportTopside(parsedData, sourceCaseId, projectId);
                    }

                    if (assets["Substructure"])
                    {
                        ImportSubstructure(parsedData, sourceCaseId, projectId);
                    }

                    if (assets["Transport"])
                    {
                        ImportTransport(parsedData, sourceCaseId, projectId);
                    }
                }
            }

            return _projectService.GetProjectDto(projectId);
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
}
