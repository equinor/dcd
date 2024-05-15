import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { getToken, loginAccessTokenKey } from "../Utils/common"

export class __ProjectService extends __BaseService {
    async getProject(id: string) {
        const project: Components.Schemas.ProjectDto = await this.get<Components.Schemas.ProjectDto>(`/${id}`)
        return project
    }

    public async createProject(contextId: string): Promise<Components.Schemas.ProjectDto> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams(
            "",
            {},
            { params: { contextId } },
        )
        return res
    }

    public async updateProject(projectId: string, body: Components.Schemas.UpdateProjectDto): Promise<Components.Schemas.ProjectDto> {
        const res = await this.put(`${projectId}`, { body })
        return res
    }

    public async compareCases(projectId: string) {
        const res: Components.Schemas.CompareCasesDto[] = await this.get<Components.Schemas.CompareCasesDto[]>(`/${projectId}/case-comparison`)
        return res
    }
}

export const GetProjectService = async () => new __ProjectService({
    ...config.ProjectService,
    accessToken: await getToken(loginAccessTokenKey)!,
})
