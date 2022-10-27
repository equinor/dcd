import { __BaseService } from "./__BaseService"

import { Project } from "../models/Project"
import { config } from "./config"

import { GetToken, LoginAccessTokenKey } from "../Utils/common"
import { Case } from "../models/case/Case"

class __CaseService extends __BaseService {
    public async createCase(data: Components.Schemas.CaseDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.post("", { body: data })
        return Project.fromJSON(res)
    }

    public async create(data: Components.Schemas.CaseDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.post("/new", { body: data })
        return Project.fromJSON(res)
    }

    public async updateCase(body: Components.Schemas.CaseDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.put("", { body })
        return Project.fromJSON(res)
    }

    public async update(body: Components.Schemas.CaseDto): Promise<Case> {
        const res: Components.Schemas.ProjectDto = await this.put("/new", { body })
        return new Case(res)
    }

    public async duplicateCase(copyCaseId: string, data: Components.Schemas.CaseDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams(
            "/copy",
            { body: data },
            { params: { copyCaseId } },
        )
        return Project.fromJSON(res)
    }

    public async deleteCase(caseId: string): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.delete(`/${caseId}`)
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
