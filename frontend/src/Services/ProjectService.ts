import { Project } from "../models/Project"
import { config } from "./config"
import { __BaseService } from "./__BaseService"

export class __ProjectService extends __BaseService {
    async getProjects() {
        const projects = await this.get<Components.Schemas.ProjectDto[]>("")
        return projects.map(Project.fromJSON)
    }

    async getProjectByID(id: string) {
        const project = await this.get<Components.Schemas.ProjectDto>(`/${id}`)
        return Project.fromJSON(project)
    }

    createProject(project: Components.Schemas.ProjectDto) {
        return this.post("", { body: project })
    }
}

export const ProjectService = new __ProjectService({
    ...config.ProjectService,
    accessToken: window.sessionStorage.getItem("loginAccessToken")!,
})
