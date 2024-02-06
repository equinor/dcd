import { Project } from "../models/Project"
import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { GetToken, LoginAccessTokenKey } from "../Utils/common"

export class __ProjectService extends __BaseService {
    async getProjectByID(id: string) {
        const project: Components.Schemas.ProjectDto = await this.get<Components.Schemas.ProjectDto>(`/${id}`)
        return Project.fromJSON(project)
    }

    public async createProject(project: Components.Schemas.ProjectDto) {
        return this.post("", { body: project })
    }

    public async createProjectFromContextId(contextId: string): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams(
            "/createFromFusion",
            {},
            { params: { contextId } },
        )
        return Project.fromJSON(res)
    }

    public async updateProject(body: Components.Schemas.ProjectDto): Promise<Project> {
        const res = await this.put("", { body })
        return Project.fromJSON(res)
    }

    public async setReferenceCase(body:Components.Schemas.ProjectDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.put(
            "/ReferenceCase",
            { body },
        )
        return Project.fromJSON(res)
    }
}

export async function GetProjectService() {
    return new __ProjectService({
        ...config.ProjectService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
