export type ServiceConfig = {
    BASE_URL: string
    accessToken?: string
    headers?: Record<string, string>
}

export const config = Object.freeze({
    ProjectService: {
        BASE_URL: 'http://localhost:5000/project',
    }
})
