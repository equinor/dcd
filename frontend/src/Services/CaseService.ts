import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

class CaseService extends __BaseService {
    public async create(
        projectId: string,
        data: Components.Schemas.CreateCaseDto,
    ): Promise<Components.Schemas.ProjectDto> {
        const res: Components.Schemas.ProjectDto = await this.post(
            `projects/${projectId}/cases`,
            { body: data },
        )
        return res
    }

    public async updateCase(
        projectId: string,
        caseId: string,
        body: Components.Schemas.CaseDto,
    ): Promise<Components.Schemas.ProjectDto> {
        const res: Components.Schemas.ProjectDto = await this.put(
            `projects/${projectId}/cases/${caseId}`,
            { body },
        )
        return res
    }

    public async getCase(
        projectId: string,
        caseId: string,
    ): Promise<Components.Schemas.ProjectDto> {
        const res = await this.get(`projects/${projectId}/cases/${caseId}`)
        return res
    }

    public async duplicateCase(
        projectId: string,
        copyCaseId: string,
    ): Promise<Components.Schemas.ProjectDto> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams(
            `projects/${projectId}/cases/copy`,
            { body: {} },
            { params: { copyCaseId } },
        )
        return res
    }

    public async deleteCase(
        projectId: string,
        caseId: string,
    ): Promise<Components.Schemas.ProjectDto> {
        const res: Components.Schemas.ProjectDto = await this.delete(
            `projects/${projectId}/cases/${caseId}`,
        )
        return res
    }
}

export const GetCaseService = async () => new CaseService({
    ...config.CaseService,
    accessToken: await getToken(loginAccessTokenKey)!,
})
