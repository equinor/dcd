using api.Dtos;
using api.Excel;
using api.Services;

using ClosedXML.Excel;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class STEAController : ControllerBase
    {
        private STEAService _sTEAService;
        private readonly ILogger<STEAController> _logger;

        public STEAController(ILogger<STEAController> logger, STEAService sTEAService)
        {
            _logger = logger;
            _sTEAService = sTEAService;
        }

        [HttpGet("{ProjectId}", Name = "GetInputToSTEA")]
        public STEAProjectDto GetInputToSTEA(Guid ProjectId)
        {
            return _sTEAService.GetInputToSTEA(ProjectId);
        }

        [HttpPost("{ProjectId}", Name = "ExcelToSTEA")]
        public FileResult ExcelToSTEA(Guid ProjectId)
        {

            var project = GetInputToSTEA(ProjectId);
            List<BusinessCase> businessCases = ExportToSTEA.export(project);
            string filename = project.Name + "ExportToSTEA.xlsx";
            return File(ExcelFile(businessCases, project.Name).ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }

        private MemoryStream ExcelFile(List<BusinessCase> businessCases, string projectName)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Input to STEA");
            ws.Cell("B2").Value = projectName;
            foreach (BusinessCase businessCase in businessCases)
            {
                foreach (ExcelTableCell etc in businessCase.Header)
                {
                    ws.Cell(etc.CellNo).Value = etc.Value;
                }
                foreach (ExcelTableCell etc in businessCase.Exploration)
                {
                    ws.Cell(etc.CellNo).Value = etc.Value;
                }
                foreach (ExcelTableCell etc in businessCase.Capex)
                {
                    ws.Cell(etc.CellNo).Value = etc.Value;
                }
                foreach (ExcelTableCell etc in businessCase.Drilling)
                {
                    ws.Cell(etc.CellNo).Value = etc.Value;
                }
                foreach (ExcelTableCell etc in businessCase.OffshoreFacilites)
                {
                    ws.Cell(etc.CellNo).Value = etc.Value;
                }
                ws.Cell(businessCase.ProductionAndSalesVolumes.CellNo).Value = businessCase.ProductionAndSalesVolumes.Value;
                foreach (ExcelTableCell etc in businessCase.TotalAndAnnualOil)
                {
                    ws.Cell(etc.CellNo).Value = etc.Value;
                }
                foreach (ExcelTableCell etc in businessCase.NetSalesGas)
                {
                    ws.Cell(etc.CellNo).Value = etc.Value;
                }
                foreach (ExcelTableCell etc in businessCase.Co2Emissions)
                {
                    ws.Cell(etc.CellNo).Value = etc.Value;
                }
            }
            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return stream;
            }
        }
    }
}
