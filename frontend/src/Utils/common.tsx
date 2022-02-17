export const GetDrainageStrategy = (project: Components.Schemas.ProjectDto, drainageStrategyId?: string) => {
    return project.drainageStrategies?.find(o => o.id === drainageStrategyId);
};

const enum ProjectPhase {
    Null,
    BidPreparations,
    BusinessIdentification,
    BusinessPlanning,
    ConceptPlanning,
    ConcessionNegotiations,
    Definition,
    Execution,
    Operation,
    ScreeningBusinessOpportunities
}

export const ConvertProjectPhaseEnumToString = (phase: number) => {
    switch (phase) {
        case ProjectPhase.Null:
            return "Unknown";
        case ProjectPhase.BidPreparations:
            return "Bid preparations";
        case ProjectPhase.BusinessIdentification:
            return "Business identification";
        case ProjectPhase.BusinessPlanning:
            return "Business planning";
        case ProjectPhase.ConceptPlanning:
            return "Concept planning";
        case ProjectPhase.ConcessionNegotiations:
            return "Concessions / Negotiations";
        case ProjectPhase.Definition:
            return "Defintion";
        case ProjectPhase.Execution:
            return "Execution";
        case ProjectPhase.Operation:
            return "Operation";
        case ProjectPhase.ScreeningBusinessOpportunities:
            return "Screening business opportunities";
        default:
            return "Unknown";
    }
};

const enum ProjectCategory {
    Null,
    Brownfield,
    Cessation,
    DrillingUpgrade,
    Onshore,
    Pipeline,
    PlatformFpso,
    Subsea,
    Solar,
    Co2Storage,
    Efuel,
    Nuclear,
    Co2Capture,
    Fpso,
    Hydrogen,
    Hse,
    OffshoreWind,
    Platform,
    PowerFromShore,
    TieIn,
    RenewableOther,
    Ccs
}

export const ConvertProjectCategoryEnumToString = (category: number) => {
    switch (category) {
        case ProjectCategory.Null:
            return "Unknown";
        case ProjectCategory.Brownfield:
            return "Brownfield";
        case ProjectCategory.Cessation:
            return "Cessation";
        case ProjectCategory.DrillingUpgrade:
            return "Drilling upgrade";
        case ProjectCategory.Onshore:
            return "Onshore";
        case ProjectCategory.Pipeline:
            return "Pipeline";
        case ProjectCategory.PlatformFpso:
            return "Platform FPSO";
        case ProjectCategory.Subsea:
            return "Subsea";
        case ProjectCategory.Solar:
            return "Solar";
        case ProjectCategory.Co2Storage:
            return "CO2 storage";
        case ProjectCategory.Efuel:
            return "Efuel";
        case ProjectCategory.Nuclear:
            return "Nuclear";
        case ProjectCategory.Co2Capture:
            return "CO2 Capture";
        case ProjectCategory.Fpso:
            return "FPSO";
        case ProjectCategory.Hydrogen:
            return "Hydrogen";
        case ProjectCategory.Hse:
            return "Hse";
        case ProjectCategory.OffshoreWind:
            return "Offshore wind";
        case ProjectCategory.Platform:
            return "Platform";
        case ProjectCategory.PowerFromShore:
            return "Power from shore";
        case ProjectCategory.TieIn:
            return "Tie-in";
        case ProjectCategory.RenewableOther:
            return "Renewable other";
        case ProjectCategory.Ccs:
            return "CCS";
        default:
            return "Unknown";
    }
};
