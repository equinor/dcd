export type ServiceConfig = {
    BASE_URL: string
    accessToken?: string
    headers?: Record<string, string>
}

export const config = Object.freeze({
    CaseService: {
        BASE_URL: `${process.env.REACT_APP_API_BASE_URL}/cases`,
    },
    CommonLibraryService: {
        BASE_URL: `${process.env.REACT_APP_API_BASE_URL}/common-library`,
    },
    FusionService: {
        BASE_URL: "https://pro-s-context-fprd.azurewebsites.net",
    },
    ProjectService: {
        BASE_URL: `${process.env.REACT_APP_API_BASE_URL}/projects`,
    },
    DrainageStrategyService: {
        BASE_URL: `${process.env.REACT_APP_API_BASE_URL}/drainage-strategies`,
    },
    ExplorationService: {
        BASE_URL: `${process.env.REACT_APP_API_BASE_URL}/explorations`,
    },
    WellProjectService: {
        BASE_URL: `${process.env.REACT_APP_API_BASE_URL}/well-projects`,
    },
    SurfService: {
        BASE_URL: `${process.env.REACT_APP_API_BASE_URL}/surfs`,
    },
    TopsideService: {
        BASE_URL: `${process.env.REACT_APP_API_BASE_URL}/topsides`,
    },
    SubstructureService: {
        BASE_URL: `${process.env.REACT_APP_API_BASE_URL}/substructures`,
    },
    TransportService: {
        BASE_URL: `${process.env.REACT_APP_API_BASE_URL}/transports`,
    },
})
