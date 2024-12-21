using api.Context;
using api.Context.Extensions;
using api.Features.CaseProfiles.Dtos;
using api.Features.CaseProfiles.Repositories;
using api.Features.Stea.Dtos;
using api.Features.Stea.ExcelExport;
using api.Models;

using AutoMapper;

namespace api.Features.Stea;

public class SteaService(DcdDbContext context,
    ILogger<SteaService> logger,
    IProjectWithCasesAndAssetsRepository projectWithCasesAndAssetsRepository, IMapper mapper)
{
    public async Task<(byte[] excelFileContents, string filename)> GetExcelFile(Guid projectId)
    {
        var project = await GetInputToStea(projectId);
        var businessCases = ExportToExcelService.CreateExcelCells(project);
        var bytes = ExportToExcelService.WriteExcelCellsToExcelDocument(businessCases, project.Name);

        return (bytes, project.Name + "ExportToSTEA.xlsx");
    }

    public async Task<SteaProjectDto> GetInputToStea(Guid projectId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectIdOrRevisionId(projectId);

        var project = await projectWithCasesAndAssetsRepository.GetProjectWithCasesAndAssets(projectPk);

        var projectDto = mapper.Map<Project, ProjectWithAssetsDto>(project, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString());

        if (projectDto == null)
        {
            logger.LogError("Failed to map project to dto");
            throw new Exception("Failed to map project to dto");
        }

        var steaCaseDtos = new List<SteaCaseDto>();

        foreach (var c in project.Cases)
        {
            if (c.Archived)
            {
                continue;
            }

            var caseDto = mapper.Map<CaseWithProfilesDto>(c);

            if (caseDto == null)
            {
                logger.LogError("Failed to map case to dto");
                throw new Exception("Failed to map case to dto");
            }

            steaCaseDtos.Add(SteaCaseDtoBuilder.Build(caseDto, projectDto));
        }

        return SteaProjectDtoBuilder.Build(projectDto, steaCaseDtos);
    }
}
