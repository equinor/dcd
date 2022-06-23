namespace Api.Services.FusionIntegration.Models
{
    using System;

    using Fusion.ApiClients.Org;

    /// <summary>
    /// Decision gate dates for a given project.
    /// Essentially a mirror of <see cref="ApiProjectDecisionGatesV2"/>. We need a local "copy" of the dates, as we
    /// might do our own mutations on these dates.
    /// </summary>
    public class FusionDecisionGateDates
    {
        public FusionDecisionGateDates(ApiProjectDecisionGatesV2? decisionGateDates)
        {
            DG1 = decisionGateDates?.DG1;
            DG2 = decisionGateDates?.DG2;
            DG3 = decisionGateDates?.DG3;
            DG4 = decisionGateDates?.DG4;
        }

        private FusionDecisionGateDates()
        {
        }

        public DateTime? DG1 { get; set; }

        public DateTime? DG2 { get; set; }

        public DateTime? DG3 { get; set; }

        public DateTime? DG4 { get; set; }

        public static FusionDecisionGateDates MockDates()
        {
            return new()
            {
                DG1 = new DateTime(2100, 1, 1),
                DG2 = new DateTime(2101, 4, 1),
                DG3 = new DateTime(2102, 8, 1),
                DG4 = new DateTime(2103, 12, 1),
            };
        }
    }
}
