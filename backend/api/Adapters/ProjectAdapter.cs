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

            project.ExplorationOperationalWellCosts = Convert(projectDto.ExplorationOperationalWellCosts);
            project.DevelopmentOperationalWellCosts = Convert(projectDto.DevelopmentOperationalWellCosts);

            return project;
        }

        public static ExplorationOperationalWellCosts Convert(ExplorationOperationalWellCostsDto? explorationOperationalWellCostsDto)
        {
            if (explorationOperationalWellCostsDto == null)
            {
                return null!;
            }
            return new ExplorationOperationalWellCosts
            {
                Id = explorationOperationalWellCostsDto.Id,
                RigUpgrading = explorationOperationalWellCostsDto.RigUpgrading,
                ExplorationRigMobDemob = explorationOperationalWellCostsDto.ExplorationRigMobDemob,
                ExplorationProjectDrillingCosts = explorationOperationalWellCostsDto.ExplorationProjectDrillingCosts,
                AppraisalRigMobDemob = explorationOperationalWellCostsDto.AppraisalRigMobDemob,
                AppraisalProjectDrillingCosts = explorationOperationalWellCostsDto.AppraisalProjectDrillingCosts,
            };
        }

        public static DevelopmentOperationalWellCosts Convert(DevelopmentOperationalWellCostsDto? developmentOperationalWellCostsDto)
        {
            if (developmentOperationalWellCostsDto == null)
            {
                return null!;
            }
            return new DevelopmentOperationalWellCosts
            {
                Id = developmentOperationalWellCostsDto.Id,
                RigUpgrading = developmentOperationalWellCostsDto.RigUpgrading,
                RigMobDemob = developmentOperationalWellCostsDto.RigMobDemob,
                AnnualWellInterventionCostPerWell = developmentOperationalWellCostsDto.AnnualWellInterventionCostPerWell,
                PluggingAndAbandonment = developmentOperationalWellCostsDto.PluggingAndAbandonment,
            };
        }
    }
}
