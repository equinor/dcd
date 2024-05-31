export type ServiceConfig = {
    BASE_URL: string
    accessToken?: string
    headers?: Record<string, string>
}

const configuration = {
    BaseUrl: {
        BASE_URL: "",
    },
    CaseService: {
        BASE_URL: "",
    },
    CaseWithAssetsService: {
        BASE_URL: "",
    },
    TechnicalInputService: {
        BASE_URL: "",
    },
    ProjectService: {
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
    GenerateProfileService: {
        BASE_URL: "",
    },
    CompareCasesService: {
        BASE_URL: "",
    },
    ImageService: {
        BASE_URL: "",
    },
}

export const buildConfig = (baseUrl: string) => {
    configuration.BaseUrl.BASE_URL = `${baseUrl}/`
    configuration.CaseService.BASE_URL = `${baseUrl}/`
    configuration.CaseWithAssetsService.BASE_URL = `${baseUrl}/`
    configuration.TechnicalInputService.BASE_URL = `${baseUrl}/`
    configuration.ProjectService.BASE_URL = `${baseUrl}/projects`
    configuration.STEAService.BASE_URL = `${baseUrl}/stea`
    configuration.UploadService.BASE_URL = `${baseUrl}/prosp`
    configuration.WellService.BASE_URL = `${baseUrl}/`
    configuration.GenerateProfileService.BASE_URL = `${baseUrl}/`
    configuration.CompareCasesService.BASE_URL = `${baseUrl}/compare-cases`
    configuration.ImageService.BASE_URL = `${baseUrl}/`
}

export const config = Object.freeze(configuration)
