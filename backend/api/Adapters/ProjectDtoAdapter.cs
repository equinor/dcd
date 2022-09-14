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
            foreach (var e in project.Explorations)
            {
                projectDto.Explorations.Add(ExplorationDtoAdapter.Convert(e));
            }
        }

        if (project.DrainageStrategies != null)
        {
            projectDto.DrainageStrategies = new List<DrainageStrategyDto>();
            foreach (var d in project.DrainageStrategies)
            {
                projectDto.DrainageStrategies.Add(DrainageStrategyDtoAdapter.Convert(d, project.PhysicalUnit));
            }
        }

        if (project.WellProjects != null)
        {
            projectDto.WellProjects = new List<WellProjectDto>();
            foreach (var w in project.WellProjects)
            {
                projectDto.WellProjects.Add(WellProjectDtoAdapter.Convert(w));
            }
        }

        if (project.Substructures != null)
        {
            projectDto.Substructures = new List<SubstructureDto>();
            foreach (var s in project.Substructures)
            {
                projectDto.Substructures.Add(SubstructureDtoAdapter.Convert(s));
            }
        }

        if (project.Surfs != null)
        {
            projectDto.Surfs = new List<SurfDto>();
            foreach (var s in project.Surfs)
            {
                projectDto.Surfs.Add(SurfDtoAdapter.Convert(s));
            }
        }

        if (project.Topsides != null)
        {
            projectDto.Topsides = new List<TopsideDto>();
            foreach (var t in project.Topsides)
            {
                projectDto.Topsides.Add(TopsideDtoAdapter.Convert(t));
            }
        }

        if (project.Transports != null)
        {
            projectDto.Transports = new List<TransportDto>();
            foreach (var t in project.Transports)
            {
                projectDto.Transports.Add(TransportDtoAdapter.Convert(t));
            }
        }

        if (project.Cases != null)
        {
            foreach (var c in project.Cases)
            {
                projectDto.Cases!.Add(CaseDtoAdapter.Convert(c, projectDto));
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

    public static void AddCapexToCasesYear(ProjectDto p)
    {
        foreach (var c in p.Cases!)
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

            CalculateCapexYear(p.WellProjects!.FirstOrDefault(l => l.Id == c.WellProjectLink)?.CostProfile);
            CalculateCapexYear(p.Substructures!.FirstOrDefault(l => l.Id == c.SubstructureLink)?.CostProfile);
            CalculateCapexYear(p.Substructures!.FirstOrDefault(l => l.Id == c.SubstructureLink)?.CessationCostProfile);
            CalculateCapexYear(p.Surfs!.FirstOrDefault(l => l.Id == c.SurfLink)?.CostProfile);
            CalculateCapexYear(p.Surfs!.FirstOrDefault(l => l.Id == c.SurfLink)?.CessationCostProfile);
            CalculateCapexYear(p.Topsides!.FirstOrDefault(l => l.Id == c.TopsideLink)?.CostProfile);
            CalculateCapexYear(p.Topsides!.FirstOrDefault(l => l.Id == c.TopsideLink)?.CessationCostProfile);
            CalculateCapexYear(p.Transports!.FirstOrDefault(l => l.Id == c.TransportLink)?.CostProfile);
            CalculateCapexYear(p.Transports!.FirstOrDefault(l => l.Id == c.TransportLink)?.CessationCostProfile);
            CalculateCapexYear(p.Explorations!.FirstOrDefault(l => l.Id == c.ExplorationLink)?.CostProfile);

            var lastYear = valuesDict.Keys.Count > 0 ? valuesDict.Keys.Max() : int.MinValue;

            for (var i = minYear ?? 0; i <= lastYear; i++)
            {
                if (!valuesDict.ContainsKey(i))
                {
                    valuesDict.Add(i, 0);
                }
            }

            c.CapexYear = new CapexYear
            {
                StartYear = minYear,
                Values = valuesDict.Values.ToArray()
            };
        }
    }

    public static void AddCapexToCases(ProjectDto p)
    {
        foreach (var c in p.Cases!)
        {
            c.Capex = 0;
            if (c.WellProjectLink != Guid.Empty)
            {
                var wellProject = p.WellProjects!.First(l => l.Id == c.WellProjectLink);
                if (wellProject.CostProfile != null)
                {
                    c.Capex += wellProject.CostProfile?.Sum ?? 0;
                }
            }

            if (c.SubstructureLink != Guid.Empty)
            {
                c.Capex += p.Substructures!.First(l => l.Id == c.SubstructureLink)?.CostProfile?.Sum ?? 0;
                c.Capex += p.Substructures!.First(l => l.Id == c.SubstructureLink)?.CessationCostProfile?.Sum ?? 0;
            }

            if (c.SurfLink != Guid.Empty)
            {
                c.Capex += p.Surfs!.First(l => l.Id == c.SurfLink)?.CostProfile?.Sum ?? 0;
                c.Capex += p.Surfs!.First(l => l.Id == c.SurfLink)?.CessationCostProfile?.Sum ?? 0;
            }

            if (c.TopsideLink != Guid.Empty)
            {
                c.Capex += p.Topsides!.First(l => l.Id == c.TopsideLink)?.CostProfile?.Sum ?? 0;
                c.Capex += p.Topsides!.First(l => l.Id == c.TopsideLink)?.CessationCostProfile?.Sum ?? 0;
            }

            if (c.TransportLink != Guid.Empty)
            {
                c.Capex += p.Transports!.First(l => l.Id == c.TransportLink)?.CostProfile?.Sum ?? 0;
                c.Capex += p.Transports!.First(l => l.Id == c.TransportLink)?.CessationCostProfile?.Sum ?? 0;
            }
        }
    }
}
