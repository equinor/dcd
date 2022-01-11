export type ServiceConfig = {
    BASE_URL: string
    headers?: Record<string, string>
}

export const servicesConfig = Object.freeze({
    ProjectsService: {
        BASE_URL: "http://localhost:5000/project",
    }
})
