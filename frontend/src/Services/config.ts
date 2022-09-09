export type ServiceConfig = {
    BASE_URL: string
    accessToken?: string
    headers?: Record<string, string>
}

const configuration = {
    CaseService: {
        BASE_URL: "",
    },
    CommonLibraryService: {
        BASE_URL: "",
    },
    FusionService: {
        BASE_URL: "https://pro-s-context-fprd.azurewebsites.net",
    },
    ProjectService: {
        BASE_URL: "",
    },
    DrainageStrategyService: {
        BASE_URL: "",
    },
    ExplorationService: {
        BASE_URL: "",
    },
    WellProjectService: {
        BASE_URL: "",
    },
    SurfService: {
        BASE_URL: "",
    },
    TopsideService: {
        BASE_URL: "",
    },
    SubstructureService: {
        BASE_URL: "",
    },
    TransportService: {
        BASE_URL: "",
    },
    STEAService: {
        BASE_URL: "",
    },
    UploadService: {
        BASE_URL: "",
    },
    WellService: {
        BASE_URL: "",
    },
    WellProjectWellService: {
        BASE_URL: "",
    },
    ExplorationWellService: {
        BASE_URL: "",
    },
}

export const buildConfig = (baseUrl: string) => {
    configuration.CaseService.BASE_URL = `${baseUrl}/cases`
    configuration.CommonLibraryService.BASE_URL = `${baseUrl}/common-library`
    configuration.ProjectService.BASE_URL = `${baseUrl}/projects`
    configuration.DrainageStrategyService.BASE_URL = `${baseUrl}/drainage-strategies`
    configuration.ExplorationService.BASE_URL = `${baseUrl}/explorations`
    configuration.WellProjectService.BASE_URL = `${baseUrl}/well-projects`
    configuration.SurfService.BASE_URL = `${baseUrl}/surfs`
    configuration.TopsideService.BASE_URL = `${baseUrl}/topsides`
    configuration.SubstructureService.BASE_URL = `${baseUrl}/substructures`
    configuration.TransportService.BASE_URL = `${baseUrl}/transports`
    configuration.STEAService.BASE_URL = `${baseUrl}/stea`
    configuration.UploadService.BASE_URL = `${baseUrl}/prosp`
    configuration.WellService.BASE_URL = `${baseUrl}/wells`
    configuration.WellProjectWellService.BASE_URL = `${baseUrl}/well-project-wells`
    configuration.ExplorationWellService.BASE_URL = `${baseUrl}/exploration-wells`
}

export const config = Object.freeze(configuration)
