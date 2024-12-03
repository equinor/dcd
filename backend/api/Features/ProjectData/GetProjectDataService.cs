using api.Context;
using api.Dtos;
using api.Features.Assets.CaseAssets.DrainageStrategies.Dtos;
using api.Features.Assets.CaseAssets.Explorations.Dtos;
using api.Features.Assets.CaseAssets.Substructures.Dtos;
using api.Features.Assets.CaseAssets.Surfs.Dtos;
using api.Features.Assets.CaseAssets.Topsides.Dtos;
using api.Features.Assets.CaseAssets.Transports.Dtos;
using api.Features.Assets.CaseAssets.WellProjects.Dtos;
using api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts.Dtos;
using api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts.Dtos;
using api.Features.ProjectData.Dtos;
using api.Features.ProjectMembers.Get;
using api.Features.Revisions.Get;
using api.Features.Wells.Get;
using api.Models;
using api.Repositories;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace api.Features.ProjectData;

public class GetProjectDataService(IProjectWithAssetsRepository projectWithAssetsRepository, IMapper mapper, DcdDbContext context)
{
    public async Task<ProjectDataDto> GetProjectData(Guid projectId)
    {
        var project = await projectWithAssetsRepository.GetProjectWithCasesAndAssets(projectId);

        var projectMembers = await context.ProjectMembers
            .Where(x => x.ProjectId == projectId)
            .Select(x => new ProjectMemberDto
            {
                ProjectId = x.ProjectId,
                UserId = x.UserId,
                Role = x.Role
            })
            .ToListAsync();

        var revisionDetailsList = await context.RevisionDetails
            .Where(r => r.OriginalProjectId == projectId)
            .OrderBy(x => x.RevisionDate)
            .Select(x => new RevisionDetailsDto
            {
                Id = x.Id,
                OriginalProjectId = x.OriginalProjectId,
                RevisionId = x.RevisionId,
                RevisionName = x.RevisionName,
                RevisionDate = x.RevisionDate,
                Arena = x.Arena,
                Mdqc = x.Mdqc,
                Classification = x.Classification
            })
            .ToListAsync();

        return new ProjectDataDto
        {
            ProjectId = projectId,
            ProjectMembers = projectMembers,
            RevisionDetailsList = revisionDetailsList,
            CommonProjectAndRevisionData = MapToDto(project)
        };
    }

    public async Task<RevisionDataDto> GetRevisionData(Guid projectId, Guid revisionId)
    {
        var project = await projectWithAssetsRepository.GetRevisionWithCasesAndAssets(projectId, revisionId);

        var revisionDetails = await context.RevisionDetails
            .Where(r => r.OriginalProjectId == projectId)
            .Where(x => x.RevisionId == revisionId)
            .OrderBy(x => x.RevisionDate)
            .Select(x => new RevisionDetailsDto
            {
                Id = x.Id,
                OriginalProjectId = x.OriginalProjectId,
                RevisionId = x.RevisionId,
                RevisionName = x.RevisionName,
                RevisionDate = x.RevisionDate,
                Arena = x.Arena,
                Mdqc = x.Mdqc,
                Classification = x.Classification
            })
            .SingleAsync();

        return new RevisionDataDto
        {
            RevisionDetails = revisionDetails,
            CommonProjectAndRevisionData = MapToDto(project)
        };
    }

    private CommonProjectAndRevisionDto MapToDto(Project project)
    {
        return new CommonProjectAndRevisionDto
        {
            Id = project.Id,
            ModifyTime = project.Cases.Select(c => c.ModifyTime).Append(project.ModifyTime).Max(),
            Classification = project.Classification,
            Name = project.Name,
            CommonLibraryId = project.CommonLibraryId,
            FusionProjectId = project.FusionProjectId,
            ReferenceCaseId = project.ReferenceCaseId,
            CommonLibraryName = project.CommonLibraryName,
            Description = project.Description,
            Country = project.Country,
            Currency = project.Currency,
            PhysicalUnit = project.PhysicalUnit,
            CreateDate = project.CreateDate,
            ProjectPhase = project.ProjectPhase,
            InternalProjectPhase = project.InternalProjectPhase,
            ProjectCategory = project.ProjectCategory,
            CO2RemovedFromGas = project.CO2RemovedFromGas,
            CO2EmissionFromFuelGas = project.CO2EmissionFromFuelGas,
            FlaredGasPerProducedVolume = project.FlaredGasPerProducedVolume,
            CO2EmissionsFromFlaredGas = project.CO2EmissionsFromFlaredGas,
            CO2Vented = project.CO2Vented,
            DailyEmissionFromDrillingRig = project.DailyEmissionFromDrillingRig,
            AverageDevelopmentDrillingDays = project.AverageDevelopmentDrillingDays,
            OilPriceUSD = project.OilPriceUSD,
            GasPriceNOK = project.GasPriceNOK,
            DiscountRate = project.DiscountRate,
            ExchangeRateUSDToNOK = project.ExchangeRateUSDToNOK,
            ExplorationOperationalWellCosts = mapper.Map<ExplorationOperationalWellCostsDto>(project.ExplorationOperationalWellCosts),
            DevelopmentOperationalWellCosts = mapper.Map<DevelopmentOperationalWellCostsDto>(project.DevelopmentOperationalWellCosts),

            Cases = mapper.Map<List<CaseWithProfilesDto>>(project.Cases),
            Wells = mapper.Map<List<WellDto>>(project.Wells),
            Explorations = mapper.Map<List<ExplorationWithProfilesDto>>(project.Explorations),
            Surfs = mapper.Map<List<SurfWithProfilesDto>>(project.Surfs),
            Substructures = mapper.Map<List<SubstructureWithProfilesDto>>(project.Substructures),
            Topsides = mapper.Map<List<TopsideWithProfilesDto>>(project.Topsides),
            Transports = mapper.Map<List<TransportWithProfilesDto>>(project.Transports),
            DrainageStrategies = mapper.Map<List<DrainageStrategyWithProfilesDto>>(project.DrainageStrategies, opts => opts.Items["ConversionUnit"] = project.PhysicalUnit.ToString()),
            WellProjects = mapper.Map<List<WellProjectWithProfilesDto>>(project.WellProjects)
        };
    }
}
