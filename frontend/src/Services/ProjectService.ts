import { Project } from "../models/Project"
import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"

export class __ProjectService extends __BaseService {
    async getProjects() {
        const projects: Components.Schemas.ProjectDto[] = await this.get<Components.Schemas.ProjectDto[]>("")
        return projects.map(Project.fromJSON)
    }

    async getProjectByID(id: string) {
        const project: Components.Schemas.ProjectDto = await this.get<Components.Schemas.ProjectDto>(`/${id}`)
        return Project.fromJSON(project)
    }

    createProject(project: Components.Schemas.ProjectDto) {
        return this.post("", { body: project })
    }
}

export function GetProjectService() {
    return new __ProjectService({
        ...config.ProjectService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
