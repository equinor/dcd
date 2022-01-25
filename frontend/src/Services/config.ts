export type ServiceConfig = {
    BASE_URL: string
    accessToken?: string
    headers?: Record<string, string>
}

export const config = Object.freeze({
    ProjectService: {
        BASE_URL: `${process.env.REACT_APP_API_BASE_URL}/project`,
    }
})
