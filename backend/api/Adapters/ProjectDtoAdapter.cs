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
                projectDto.Cases!.Add(CaseDtoAdapter.Convert(caseItem, projectDto));
            }
        }

        if (projectDto.Cases != null)
        {
            AddCapexToCases(projectDto);
            AddCapexToCasesYear(projectDto);
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
            Cases = new List<CaseDto>(),
            Wells = new List<WellDto>(),
            SharepointSiteUrl = project.SharepointSiteUrl
        };
    }

    public static void AddCapexToCasesYear(ProjectDto projectDto)
    {
        foreach (var caseDto in projectDto.Cases!)
        {
            int? minYear = null;
            var valuesDict = new SortedDictionary<int, double>();

            void CalculateCapexYear(TimeSeriesDto<double>? timeSeries)
            {
                if (timeSeries == null)
                {
                    return;
                }

                if (minYear == null ? timeSeries.StartYear < int.MaxValue : timeSeries.StartYear < minYear)
                {
                    minYear = timeSeries.StartYear;
                }

                for (var i = 0; i < timeSeries.Values.Length; i++)
                {
                    if (valuesDict.ContainsKey(timeSeries.StartYear + i))
                    {
                        valuesDict[timeSeries.StartYear + i] += timeSeries.Values[i];
                    }
                    else
                    {
                        valuesDict.Add(timeSeries.StartYear + i, timeSeries.Values[i]);
                    }
                }
            }

            CalculateCapexYear(projectDto.WellProjects!
                .FirstOrDefault(wellProjectDto => wellProjectDto.Id == caseDto.WellProjectLink)?.CostProfile);
            CalculateCapexYear(projectDto.Substructures!
                .FirstOrDefault(substructureDto => substructureDto.Id == caseDto.SubstructureLink)?.CostProfile);
            CalculateCapexYear(projectDto.Substructures!
                .FirstOrDefault(substructureDto => substructureDto.Id == caseDto.SubstructureLink)
                ?.CessationCostProfile);
            CalculateCapexYear(projectDto.Surfs!.FirstOrDefault(surfDto => surfDto.Id == caseDto.SurfLink)
                ?.CostProfile);
            CalculateCapexYear(projectDto.Surfs!.FirstOrDefault(surfDto => surfDto.Id == caseDto.SurfLink)
                ?.CessationCostProfile);
            CalculateCapexYear(projectDto.Topsides!.FirstOrDefault(topsideDto => topsideDto.Id == caseDto.TopsideLink)
                ?.CostProfile);
            CalculateCapexYear(projectDto.Topsides!.FirstOrDefault(topsideDto => topsideDto.Id == caseDto.TopsideLink)
                ?.CessationCostProfile);
            CalculateCapexYear(projectDto.Transports!
                .FirstOrDefault(transportDto => transportDto.Id == caseDto.TransportLink)?.CostProfile);
            CalculateCapexYear(
                projectDto.Transports!.FirstOrDefault(transportDto => transportDto.Id == caseDto.TransportLink)
                    ?.CessationCostProfile);
            CalculateCapexYear(projectDto.Explorations!
                .FirstOrDefault(explorationDto => explorationDto.Id == caseDto.ExplorationLink)?.CostProfile);

            var lastYear = valuesDict.Keys.Count > 0 ? valuesDict.Keys.Max() : int.MinValue;

            for (var i = minYear ?? 0; i <= lastYear; i++)
            {
                if (!valuesDict.ContainsKey(i))
                {
                    valuesDict.Add(i, 0);
                }
            }

            caseDto.CapexYear = new CapexYear
            {
                StartYear = minYear,
                Values = valuesDict.Values.ToArray()
            };
        }
    }

    public static void AddCapexToCases(ProjectDto projectDto)
    {
        foreach (var caseDto in projectDto.Cases!)
        {
            caseDto.Capex = 0;
            if (caseDto.WellProjectLink != Guid.Empty)
            {
                var wellProject =
                    projectDto.WellProjects!.First(wellProjectDto => wellProjectDto.Id == caseDto.WellProjectLink);
                if (wellProject.CostProfile != null)
                {
                    caseDto.Capex += wellProject.CostProfile?.Sum ?? 0;
                }
            }

            if (caseDto.SubstructureLink != Guid.Empty)
            {
                caseDto.Capex += projectDto.Substructures!
                    .First(substructureDto => substructureDto.Id == caseDto.SubstructureLink)?.CostProfile?.Sum ?? 0;
                caseDto.Capex += projectDto.Substructures!
                    .First(substructureDto => substructureDto.Id == caseDto.SubstructureLink)?.CessationCostProfile
                    ?.Sum ?? 0;
            }

            if (caseDto.SurfLink != Guid.Empty)
            {
                caseDto.Capex += projectDto.Surfs!.First(surfDto => surfDto.Id == caseDto.SurfLink)?.CostProfile?.Sum ??
                                 0;
                caseDto.Capex += projectDto.Surfs!.First(surfDto => surfDto.Id == caseDto.SurfLink)
                    ?.CessationCostProfile?.Sum ?? 0;
            }

            if (caseDto.TopsideLink != Guid.Empty)
            {
                caseDto.Capex += projectDto.Topsides!.First(topsideDto => topsideDto.Id == caseDto.TopsideLink)
                    ?.CostProfile?.Sum ?? 0;
                caseDto.Capex += projectDto.Topsides!.First(topsideDto => topsideDto.Id == caseDto.TopsideLink)
                    ?.CessationCostProfile?.Sum ?? 0;
            }

            if (caseDto.TransportLink != Guid.Empty)
            {
                caseDto.Capex += projectDto.Transports!.First(transportDto => transportDto.Id == caseDto.TransportLink)
                    ?.CostProfile?.Sum ?? 0;
                caseDto.Capex += projectDto.Transports!.First(transportDto => transportDto.Id == caseDto.TransportLink)
                    ?.CessationCostProfile?.Sum ?? 0;
            }
        }
    }
}
