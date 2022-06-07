using System.Linq;

using api.Adapters;
using api.Dtos;
using api.Models;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace api.Services
{
    public class ImportProspService
    {
        private readonly ProjectService _projectService;
        private readonly ILogger<ImportProspService> _logger;
        private readonly SurfService _surfService;
        private const string SHEETNAME = "main";

        public ImportProspService(ProjectService projectService, ILoggerFactory loggerFactory, SurfService surfService)
        {
            _projectService = projectService;
            _logger = loggerFactory.CreateLogger<ImportProspService>();
            _surfService = surfService;
        }

        public double ReadDoubleValue(IEnumerable<Cell> cellData, string coordinate)
        {
            if (double.TryParse(cellData.FirstOrDefault(c => c.CellReference == coordinate)?.CellValue?.InnerText, out var value))
            {
                return value;
            }
            return 0;
        }

        public int ReadIntValue(IEnumerable<Cell> cellData, string coordinate)
        {
            if (int.TryParse(cellData.FirstOrDefault(c => c.CellReference == coordinate)?.CellValue?.InnerText, out var value))
            {
                return value;
            }
            return -1;
        }

        public double[] ReadDoubleValues(IEnumerable<Cell> cellData, List<string> coordinates)
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

        public DateTime ReadDateValue(IEnumerable<Cell> cellData, string coordinate)
        {
            if (double.TryParse(cellData.FirstOrDefault(c => c.CellReference == coordinate)?.CellValue?.InnerText, out var value))
            {
                return DateTime.FromOADate(value);
            }
            return new DateTime(1900, 1, 1);
        }

        public void ImportSurf(IEnumerable<Cell> cellData, Guid sourceCaseId, Guid projectId)
        {
            List<string> costProfileCoords = new() { "J112", "K112", "L112", "M112", "N112", "O112", "P112" };

            var costProfileStartYear = ReadIntValue(cellData, "J103");

            var dG4Date = ReadDateValue(cellData, "F112");

            var dG3Date = ReadDateValue(cellData, "G112");

            var lengthProductionLine = ReadDoubleValue(cellData, "K35");

            var lengthUmbilicalSystem = ReadDoubleValue(cellData, "K37");

            var productionFlowlineInt = ReadIntValue(cellData, "E50");
            var productionFlowline = MapProductionFlowLine(productionFlowlineInt);

            var artificialLiftInt = ReadIntValue(cellData, "E48");
            var artificialLift = MapArtificialLift(artificialLiftInt);

            var riserCount = ReadIntValue(cellData, "K36");

            var templateCount = ReadIntValue(cellData, "K32");

            var cessationCost = ReadDoubleValue(cellData, "K88");


            var costProfile = new SurfCostProfile
            {
                Values = ReadDoubleValues(cellData, costProfileCoords),
                StartYear = dG4Date.Year - costProfileStartYear
            };

            var newSurf = new Surf
            {
                Name = "ImportedSurf",
                CostProfile = costProfile,
                ProjectId = projectId,
                ProductionFlowline = productionFlowline,
                UmbilicalSystemLength = lengthUmbilicalSystem,
                InfieldPipelineSystemLength = lengthProductionLine,
                RiserCount = riserCount,
                TemplateCount = templateCount,
                ArtificialLift = artificialLift,
            };
            var dto = SurfDtoAdapter.Convert(newSurf);

            _surfService.CreateSurf(dto, sourceCaseId);
        }

        public void ImportTopside(IEnumerable<Cell> cellData, Guid sourceCaseId, Guid projectId) {
            List<string> costProfileCoords = new() { "J104", "K104", "L104", "M104", "N104", "O104", "P104" };

            var costProfileStartYear = ReadIntValue(cellData, "J103");

            var dG4Date = ReadDateValue(cellData, "F104");

            var dG3Date = ReadDateValue(cellData, "G104");

            var lengthProductionLine = ReadDoubleValue(cellData, "K35");

            var lengthUmbilicalSystem = ReadDoubleValue(cellData, "K37");

            var productionFlowlineInt = ReadIntValue(cellData, "E50");
            var productionFlowline = MapProductionFlowLine(productionFlowlineInt);

            var artificialLiftInt = ReadIntValue(cellData, "E48");
            var artificialLift = MapArtificialLift(artificialLiftInt);

            var riserCount = ReadIntValue(cellData, "K36");

            var templateCount = ReadIntValue(cellData, "K32");

            var cessationCost = ReadDoubleValue(cellData, "K88");


            var costProfile = new SurfCostProfile
            {
                Values = ReadDoubleValues(cellData, costProfileCoords),
                StartYear = dG4Date.Year - costProfileStartYear
            };

            var newSurf = new Surf
            {
                Name = "ImportedSurf",
                CostProfile = costProfile,
                ProjectId = projectId,
                ProductionFlowline = productionFlowline,
                UmbilicalSystemLength = lengthUmbilicalSystem,
                InfieldPipelineSystemLength = lengthProductionLine,
                RiserCount = riserCount,
                TemplateCount = templateCount,
                ArtificialLift = artificialLift,
            };
            var dto = SurfDtoAdapter.Convert(newSurf);

            _surfService.CreateSurf(dto, sourceCaseId);
        }

        public ProjectDto ImportProsp(IFormFile file, Guid sourceCaseId, Guid projectId, Dictionary<string, bool> assets)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            using SpreadsheetDocument document = SpreadsheetDocument.Open(ms, false);
            var workbookPart = document.WorkbookPart;
            var mainSheet = workbookPart?.Workbook.Descendants<Sheet>()
                                                  .FirstOrDefault(x => x.Name?.ToString()?.ToLower() == SHEETNAME);

            var wsPart = (WorksheetPart)workbookPart.GetPartById(mainSheet?.Id);

            var cellData = wsPart.Worksheet.Descendants<Cell>();

            if (assets["Surf"])
            {
                ImportSurf(cellData, sourceCaseId, projectId);
            }
            if (assets["Topside"])
            {
                // ImportTopside(cellData, sourceCaseId, projectId);
            }
            if (assets["Substructure"])
            {
                // ImportSurf(cellData, sourceCaseId, projectId);
            }
            if (assets["Transport"])
            {
                // ImportSurf(cellData, sourceCaseId, projectId);
            }

            return _projectService.GetProjectDto(projectId);
        }

        public ArtificialLift MapArtificialLift(int importValue)
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

        public ProductionFlowline MapProductionFlowLine(int importValue)
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

        public string GetCellValue(WorkbookPart? workbookPart, Sheet? mainSheet)
        {
            string value = null;
            var theCell = mainSheet?.Descendants<Cell>().FirstOrDefault(x => x.CellReference == "A1") ?? new Cell();

            if (theCell.InnerText.Length > 0)
            {

                value = theCell.InnerText;
                switch (theCell.DataType?.Value)
                {
                    case CellValues.SharedString:

                        var stringTable = workbookPart?.GetPartsOfType<SharedStringTablePart>()
                                                       .FirstOrDefault();

                        if (stringTable != null)
                        {
                            value = stringTable.SharedStringTable.ElementAt(int.Parse(value)).InnerText;
                        }
                        break;

                    case CellValues.Boolean:
                        value = value switch
                        {
                            "0" => "False",
                            _ => "True",
                        };
                        break;
                }
            }

            return value;
        }
    }
}
