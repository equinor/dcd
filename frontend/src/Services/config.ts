export type ServiceConfig = {
    accessToken?: string
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
