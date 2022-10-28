using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class ProjectAdapter
{
    public static Project Convert(ProjectDto projectDto)
    {
        var project = new Project
        {
            Name = projectDto.Name,
            CommonLibraryId = projectDto.CommonLibraryId,
            CommonLibraryName = projectDto.CommonLibraryName,
            FusionProjectId = projectDto.FusionProjectId,
            Description = projectDto.Description,
            Country = projectDto.Country,
            ProjectCategory = projectDto.ProjectCategory,
            ProjectPhase = projectDto.ProjectPhase,
            Currency = projectDto.Currency,
            PhysicalUnit = projectDto.PhysUnit,
            Id = projectDto.ProjectId,
            SharepointSiteUrl = projectDto.SharepointSiteUrl,
            ExplorationOperationalWellCosts = ExplorationOperationalWellCostsAdapter.Convert(projectDto.ExplorationOperationalWellCosts),
            DevelopmentOperationalWellCosts = DevelopmentOperationalWellCostsAdapter.Convert(projectDto.DevelopmentOperationalWellCosts)
        };

        return project;
    }

    public static void ConvertExisting(Project existing, ProjectDto projectDto)
    {
        existing.Name = projectDto.Name;
        existing.CommonLibraryId = projectDto.CommonLibraryId;
        existing.CommonLibraryName = projectDto.CommonLibraryName;
        existing.FusionProjectId = projectDto.FusionProjectId;
        existing.Description = projectDto.Description;
        existing.Country = projectDto.Country;
        existing.ProjectCategory = projectDto.ProjectCategory;
        existing.ProjectPhase = projectDto.ProjectPhase;
        existing.Currency = projectDto.Currency;
        existing.PhysicalUnit = projectDto.PhysUnit;
        existing.Id = projectDto.ProjectId;
        existing.SharepointSiteUrl = projectDto.SharepointSiteUrl;
    }
}
