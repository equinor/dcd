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
                Id = projectDto.ProjectId,
            };

            project.ExplorationWellCosts = Convert(projectDto.ExplorationWellCosts);
            project.AppraisalWellCosts = Convert(projectDto.AppraisalWellCosts);
            project.DrillingWellCosts = Convert(projectDto.DrillingWellCosts);

            return project;
        }

        private static OperationalWellCosts Convert(OperationalWellCostsDto? operationalWellCostsDto)
        {
            if (operationalWellCostsDto == null)
            {
                return null!;
            }
            return new OperationalWellCosts
            {
                Id = operationalWellCostsDto.Id,
                RigUpgrading = operationalWellCostsDto.RigUpgrading,
                RigMobDemob = operationalWellCostsDto.RigMobDemob,
                ProjectDrillingCosts = operationalWellCostsDto.ProjectDrillingCosts,
                AnnualWellInterventionCostPerWell = operationalWellCostsDto.AnnualWellInterventionCostPerWell,
                PluggingAndAbandonment = operationalWellCostsDto.PluggingAndAbandonment,
            };
        }
    }
}
