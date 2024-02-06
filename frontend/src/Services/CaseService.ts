import { __BaseService } from "./__BaseService"

import { Project } from "../models/Project"
import { config } from "./config"

import { GetToken, LoginAccessTokenKey } from "../Utils/common"

class __CaseService extends __BaseService {
    public async create(
        projectId: string,
        data: Components.Schemas.CaseDto,
    ): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.post(`projects/${projectId}/cases`, { body: data })
        return Project.fromJSON(res)
    }

    public async updateCase(
        projectId: string,
        caseId: string,
        body: Components.Schemas.CaseDto,
    ): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.put(`projects/${projectId}/cases/${caseId}`, { body })
        return Project.fromJSON(res)
    }

    public async duplicateCase(
        projectId: string,
        copyCaseId: string,
        data: Components.Schemas.CaseDto,
    ): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams(
            `projects/${projectId}/cases/copy`,
            { body: data },
            { params: { copyCaseId } },
        )
        return Project.fromJSON(res)
    }

    public async deleteCase(
        projectId: string,
        caseId: string,
    ): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.delete(`projects/${projectId}/cases/${caseId}`)
        return Project.fromJSON(res)
    }
}

export const CaseService = new __CaseService({
    ...config.CaseService,
    accessToken: window.sessionStorage.getItem("loginAccessToken")!,
})

export async function GetCaseService() {
    return new __CaseService({
        ...config.CaseService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
