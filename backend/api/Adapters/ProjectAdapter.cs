using System.Linq;

using api.Dtos;
using api.Models;
using api.Services;


namespace api.Adapters;

public static class ProjectAdapter
{

    public static Project Convert(ProjectDto projectDto)
    {
        var project = new Project
        {
            Name = projectDto.Name,
            CommonLibraryId = projectDto.CommonLibraryId,
            CreateDate = projectDto.CreateDate,
            CommonLibraryName = projectDto.CommonLibraryName,
            FusionProjectId = projectDto.FusionProjectId,
            Description = projectDto.Description,
            Country = projectDto.Country,
            ProjectCategory = projectDto.ProjectCategory,
            ProjectPhase = projectDto.ProjectPhase,
            Currency = projectDto.Currency,
            PhysicalUnit = projectDto.PhysUnit,
            Id = projectDto.ProjectId
        };

        return project;
    }
}