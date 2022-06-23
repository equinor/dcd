namespace Api.Services.Models
{
    /// <summary>
    /// Strongly typed Project Phase definitions.
    /// Both from CommonLib and ServiceNow we are receiving string-representations of the phases, which actually differ
    /// between the two systems, even though they represent the same thing.
    /// This typing will ensure consistent handling of ProjectPhases within the Pitstop application.
    /// </summary>
    public enum ProjectPhase
    {
        /// <summary>
        /// DGA - Screening Business Opportunities.
        /// </summary>
        ScreeningBusinessOpportunities = 0,

        /// <summary>
        /// DGB - Bid preparations.
        /// </summary>
        BidPreparations = 1,

        /// <summary>
        /// DGB - Concession.
        /// </summary>
        ConcessionNegotiations = 2,

        /// <summary>
        /// Pre DG0 - Business identification.
        /// </summary>
        BusinessIdentification = 3,

        /// <summary>
        /// DG0-DG1 - Business planning.
        /// </summary>
        BusinessPlanning = 4,

        /// <summary>
        /// DG1-DG2 - Concept planning.
        /// </summary>
        ConceptPlanning = 5,

        /// <summary>
        /// DG2-DG3 - Definition.
        /// </summary>
        Definition = 6,

        /// <summary>
        /// DG3-DG4 - Execution.
        /// </summary>
        Execution = 7,

        /// <summary>
        /// Post DG4 - Operation.
        /// </summary>
        Operation = 8,
    }
}
