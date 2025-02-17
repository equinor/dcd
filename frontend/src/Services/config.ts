export type ServiceConfig = {
    BASE_URL: string
    accessToken?: string
    headers?: Record<string, string>
}

const configuration = {
    BaseUrl: {
        BASE_URL: "",
    },
}

export const buildConfig = (baseUrl: string) => {
    configuration.BaseUrl.BASE_URL = `${baseUrl}/`
}

export const config = Object.freeze(configuration)
