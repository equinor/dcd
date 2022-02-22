export type ServiceConfig = {
    BASE_URL: string
    accessToken?: string
    headers?: Record<string, string>
}

export const config = Object.freeze({
    CaseService: {
        BASE_URL: `${process.env.REACT_APP_API_BASE_URL}/case`,
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
})
