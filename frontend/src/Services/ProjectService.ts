import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { GetToken, LoginAccessTokenKey } from "../Utils/common"

export class __ProjectService extends __BaseService {
    async getProjectByID(id: string) {
        const project: Components.Schemas.ProjectDto = await this.get<Components.Schemas.ProjectDto>(`/${id}`)
        return project
    }

    public async createProject(project: Components.Schemas.ProjectDto) {
        return this.post("", { body: project })
    }

    public async createProjectFromContextId(contextId: string): Promise<Components.Schemas.ProjectDto> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams(
            "/createFromFusion",
            {},
            { params: { contextId } },
        )
        return res
    }

    public async updateProject(body: Components.Schemas.ProjectDto): Promise<Components.Schemas.ProjectDto> {
        const res = await this.put("", { body })
        return res
    }

    public async compareCases(projectId: string) {
        const res: Components.Schemas.CompareCasesDto[] = await this.get<Components.Schemas.CompareCasesDto[]>(`/${projectId}/case-comparison`)
        return res
    }
}

export const GetProjectService = async () => {
    return new __ProjectService({
        ...config.ProjectService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
