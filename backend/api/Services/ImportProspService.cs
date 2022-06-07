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
        private const string SURFPRODUCTIONLINE = "K35";
        private const string SURFUMBILICALLINE = "K37";
        private const string SURFRISERCOUNT = "K36";
        private const string SURFCESSATIONCOST = "K88";
        private const string SURFARTIFICIALLIFT = "E48";
        private const string SURFTEMPLATECOUNT = "K32";
        private readonly List<string> CostProfileMain = new() {"J112", "K112", "M112", "N112", "O112", "P112"};
        private readonly List<string> CostProfileYearMain = new() {"J102", "K102", "M102", "N102", "O102", "P102"};



        public ImportProspService(ProjectService projectService, ILoggerFactory loggerFactory, SurfService surfService)
        {
            _projectService = projectService;
            _logger = loggerFactory.CreateLogger<ImportProspService>();
            _surfService = surfService;
        }

        public ProjectDto ImportProsp(IFormFile file, Guid sourceCaseId, Guid projectId)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            using SpreadsheetDocument document = SpreadsheetDocument.Open(ms, false);
            var workbookPart = document.WorkbookPart;
            var mainSheet = workbookPart?.Workbook.Descendants<Sheet>()
                                                  .FirstOrDefault(x => x.Name?.ToString()?.ToLower() == SHEETNAME);

            foreach(var child in workbookPart?.Workbook?.Descendants<Sheet>()?.Where(x => x.Name?.ToString()?.ToLower() == SHEETNAME)){
                Console.WriteLine(child.Name?.ToString()?.ToLower());
                var wsPart = (WorksheetPart)workbookPart.GetPartById(child?.Id);

                var cellData = wsPart.Worksheet.Descendants<Cell>();
                var surfValues = new List<double>();

                foreach(var cell in cellData.Where(c => CostProfileMain.Contains(c?.CellReference))){
                    Console.WriteLine(cell.CellValue);
                    var data = Double.TryParse(cell.CellValue?.InnerText, out var numb);
                    surfValues.Add(numb);
                }

                // add list of metadata 
                // var metadataCoordinates = new() {SURFCESSATIONCOST};

                var timeSeriesCost = new TimeSeriesCost{
                    Values = surfValues.ToArray(),
                };
                var profileCost = new SurfCostProfile
                {
                    Values = timeSeriesCost.Values
                };

                var newSurf = new Surf
                {
                    Name = "ImportedSurf",
                    CostProfile = profileCost,
                    ProjectId = projectId,
                };

                var dto = SurfDtoAdapter.Convert(newSurf);
                
                var project = _surfService.CreateSurf(dto, sourceCaseId);

                return project;
            }

            if (mainSheet == null)
            {
                throw new ArgumentException("sheetname is not found in workbook");
            }
             return null;
            // GetCellValue(workbookPart, mainSheet);
            // var topdsideDG3FromSheet = mainSheet?.Descendants<Cell>().FirstOrDefault(x => x.CellReference == "G103")?.CellValue;
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
