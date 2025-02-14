using api.Models.Enums;

namespace api.Features.BackgroundServices.ProjectMaster.Services.EnumConverters;

public static class ProjectPhaseEnumConverter
{
    public static ProjectPhase ConvertPhase(string? phase)
    {
        return phase switch
        {
            "Bid preparations" => ProjectPhase.BidPreparations,
            "Business identification" => ProjectPhase.BusinessIdentification,
            "Business planning" => ProjectPhase.BusinessPlanning,
            "Concept planning" => ProjectPhase.ConceptPlanning,
            "Concession / Negotiations" => ProjectPhase.ConcessionNegotiations,
            "Definition" => ProjectPhase.Definition,
            "Execution" => ProjectPhase.Execution,
            "Operation" => ProjectPhase.Operation,
            "Screening business opportunities" => ProjectPhase.ScreeningBusinessOpportunities,
            _ => ProjectPhase.Null
        };
    }
}
