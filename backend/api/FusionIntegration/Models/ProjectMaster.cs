namespace Api.Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Api.Services.FusionIntegration.Models;

    /// <summary>
    /// The Pitstop domain model of a ProjectMaster.
    /// </summary>
    public class ProjectMaster
    {
        public ProjectMaster(
            FusionProjectMaster fusionProjectMaster,
            Guid? projectDirectorId,
            bool hasOrgChart)
        {
            Id = fusionProjectMaster.Identity;
            Description = fusionProjectMaster.Description;
            Portfolio = fusionProjectMaster.PortfolioOrganizationalUnit;
            // ProjectPhase = ProjectPhaseParser.ParseOrDefault(fusionProjectMaster.Phase);
            // DecisionGateDates = decisionGateDates;
            ProjectDirectorId = projectDirectorId;
            DocumentManagementId = fusionProjectMaster.DocumentManagementId;
            HasOrgChart = hasOrgChart;

            // Currently CommonLib is giving us a single category for the ProjectMaster, but will likely change in the future.
            // For now, create a list with the single Category, if the Category can be parsed.
            // Categories = ProjectCategoryParser.CanParse(fusionProjectMaster.ProjectCategory)
            //     ? new List<ProjectCategory> { ProjectCategoryParser.ParseExact(fusionProjectMaster.ProjectCategory!) }
            //     : new List<ProjectCategory>();
        }

        public ProjectMaster()
        {
        }

        // public ProjectMaster(ProjectMasterEntity projectMasterEntity)
        // {
        //     Id = projectMasterEntity.ProjectMasterId;
        //     Description = projectMasterEntity.Description;
        //     ProjectPhase = projectMasterEntity.ProjectPhase;
        //     Portfolio = projectMasterEntity.Portfolio;
        //     Categories = projectMasterEntity.Categories.ToList();
        // }

        public Guid Id { get; set; }

        public string? Description { get; set; }

        public ProjectPhase? ProjectPhase { get; set; }

        public string? Portfolio { get; set; }

        // public IList<ProjectCategory> Categories { get; set; } = new List<ProjectCategory>();

        public Guid? ProjectDirectorId { get; set; }

        // public DecisionGateDates? DecisionGateDates { get; set; }

        public string? DocumentManagementId { get; set; }

        public bool? HasOrgChart { get; set; }

        // public Dictionary<ProjectPhase, DateTimeOffset?> ProjectPhasesWithDecisionGates
        // {
        //     get
        //     {
        //         return new()
        //         {
        //             { Models.ProjectPhase.BusinessPlanning, DecisionGateDates?.DG1 },
        //             { Models.ProjectPhase.ConceptPlanning, DecisionGateDates?.DG2 },
        //             { Models.ProjectPhase.Definition, DecisionGateDates?.DG3 },
        //             { Models.ProjectPhase.Execution, DecisionGateDates?.DG4 },
        //         };
        //     }
        // }
    }
}
