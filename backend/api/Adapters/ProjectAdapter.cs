using api.Dtos;
using api.Models;
using api.Services;

namespace api.Adapters
{
    public class ProjectAdapter
    {

        public Project Convert(ProjectDto projectDto)
        {
            var project = new Project();
            project.Name = projectDto.Name;
            project.Description = projectDto.Description;
            project.Country = projectDto.Country;
            project.ProjectCategory = projectDto.ProjectCategory;
            project.ProjectPhase = projectDto.ProjectPhase;

            return project;
        }
    }
}
