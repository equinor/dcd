using System.Linq;

using api.Dtos;
using api.Models;
using api.Services;


namespace api.Adapters
{
    public static class ProjectAdapter
    {

        public static Project Convert(ProjectDto projectDto)
        {
            var project = new Project();
            project.Name = projectDto.Name;
            project.CommonLibraryId = projectDto.CommonLibraryId;
            project.CreateDate = DateTimeOffset.UtcNow;
            project.CommonLibraryName = projectDto.CommonLibraryName;
            project.Description = projectDto.Description;
            project.Country = projectDto.Country;
            project.ProjectCategory = projectDto.ProjectCategory;
            project.ProjectPhase = projectDto.ProjectPhase;

            return project;
        }
    }
}
