namespace Api.Services.FusionIntegration.Models
{
    using System;

    using Fusion.ApiClients.Org;

    /// <summary>
    /// Represents a Project from Fusion - see <see cref="ApiProjectV2"/>.
    /// Not to be confused with <see cref="FusionProjectMaster"/>.
    /// </summary>
    public class FusionProject
    {
        public FusionProject(FusionDecisionGateDates decisionGateDates, Guid? projectDirectorId)
        {
            DecisionGateDates = decisionGateDates;
            ProjectDirectorId = projectDirectorId;
        }

        public FusionDecisionGateDates DecisionGateDates { get; set; }

        public Guid? ProjectDirectorId { get; set; }
    }
}
