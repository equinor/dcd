export type ServiceConfig = {
    BASE_URL: string
    accessToken?: string
    headers?: Record<string, string>
}

export const config = Object.freeze({
    ProjectService: {
        BASE_URL: `${process.env.REACT_APP_API_BASE_URL}/projects`,
    },
    FusionService: {
        BASE_URL: `https://pro-s-context-fprd.azurewebsites.net`
    }
})
