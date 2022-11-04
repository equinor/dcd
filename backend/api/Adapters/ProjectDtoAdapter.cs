using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class ProjectDtoAdapter
{
    public static ProjectDto Convert(Project project)
    {
        var projectDto = ProjectToProjectDto(project);

        if (project.Wells != null)
        {
            foreach (var well in project.Wells)
            {
                projectDto.Wells!.Add(WellDtoAdapter.Convert(well));
            }
        }

        if (project.Explorations != null)
        {
            projectDto.Explorations = new List<ExplorationDto>();
            foreach (var exploration in project.Explorations)
            {
                projectDto.Explorations.Add(ExplorationDtoAdapter.Convert(exploration));
            }
        }

        if (project.DrainageStrategies != null)
        {
            projectDto.DrainageStrategies = new List<DrainageStrategyDto>();
            foreach (var drainageStrategy in project.DrainageStrategies)
            {
                projectDto.DrainageStrategies.Add(
                    DrainageStrategyDtoAdapter.Convert(drainageStrategy, project.PhysicalUnit));
            }
        }

        if (project.WellProjects != null)
        {
            projectDto.WellProjects = new List<WellProjectDto>();
            foreach (var wellProject in project.WellProjects)
            {
                projectDto.WellProjects.Add(WellProjectDtoAdapter.Convert(wellProject));
            }
        }

        if (project.Substructures != null)
        {
            projectDto.Substructures = new List<SubstructureDto>();
            foreach (var substructure in project.Substructures)
            {
                projectDto.Substructures.Add(SubstructureDtoAdapter.Convert(substructure));
            }
        }

        if (project.Surfs != null)
        {
            projectDto.Surfs = new List<SurfDto>();
            foreach (var surf in project.Surfs)
            {
                projectDto.Surfs.Add(SurfDtoAdapter.Convert(surf));
            }
        }

        if (project.Topsides != null)
        {
            projectDto.Topsides = new List<TopsideDto>();
            foreach (var topside in project.Topsides)
            {
                projectDto.Topsides.Add(TopsideDtoAdapter.Convert(topside));
            }
        }

        if (project.Transports != null)
        {
            projectDto.Transports = new List<TransportDto>();
            foreach (var transport in project.Transports)
            {
                projectDto.Transports.Add(TransportDtoAdapter.Convert(transport));
            }
        }

        if (project.Cases != null)
        {
            foreach (var caseItem in project.Cases)
            {
                projectDto.Cases!.Add(CaseDtoAdapter.Convert(caseItem));
            }
        }

        return projectDto;
    }

    private static ProjectDto ProjectToProjectDto(Project project)
    {
        return new ProjectDto
        {
            ProjectId = project.Id,
            Name = project.Name,
            CommonLibraryId = project.CommonLibraryId,
            CommonLibraryName = project.CommonLibraryName,
            FusionProjectId = project.FusionProjectId,
            Description = project.Description,
            Country = project.Country,
            CreateDate = project.CreateDate,
            ProjectCategory = project.ProjectCategory,
            ProjectPhase = project.ProjectPhase,
            Currency = project.Currency,
            PhysUnit = project.PhysicalUnit,
            ExplorationOperationalWellCosts = ExplorationOperationalWellCostsDtoAdapter.Convert(project.ExplorationOperationalWellCosts),
            DevelopmentOperationalWellCosts = DevelopmentOperationalWellCostsDtoAdapter.Convert(project.DevelopmentOperationalWellCosts),
            Cases = new List<CaseDto>(),
            Wells = new List<WellDto>(),
            SharepointSiteUrl = project.SharepointSiteUrl,
            CO2RemovedFromGas = project.CO2RemovedFromGas,
            CO2EmissionFromFuelGas = project.CO2EmissionFromFuelGas,
            FlaredGasPerProducedVolume = project.FlaredGasPerProducedVolume,
            CO2EmissionsFromFlaredGas = project.CO2EmissionsFromFlaredGas,
            CO2Vented = project.CO2Vented,
            DailyEmissionFromDrillingRig = project.DailyEmissionFromDrillingRig,
            AverageDevelopmentDrillingDays = project.AverageDevelopmentDrillingDays
        };
    }
}
