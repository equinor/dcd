import { __BaseService } from "./__BaseService"

import { Project } from "../models/Project"
import { config } from "./config"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { WellCase } from "../models/WellCase"

class __WellCaseService extends __BaseService {
    public async getWellCases() {
        const wellCases: Components.Schemas.WellCaseDto[] = await this.get<Components.Schemas.WellCaseDto[]>("")
        return wellCases.map(WellCase.fromJSON)
    }

    public async getWellCasesByProjectId(projectId: string) {
        // eslint-disable-next-line max-len
        const wellCases: Components.Schemas.WellCaseDto[] = await this.getWithParams("", { params: { projectId } })
        return wellCases.map(WellCase.fromJSON)
    }

    async getWellCaseById(id: string) {
        const wellCase: Components.Schemas.WellCaseDto = await this.get<Components.Schemas.WellCaseDto>(`/${id}`)
        return WellCase.fromJSON(wellCase)
    }

    public async createWellCase(data: Components.Schemas.WellCaseDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.post("", { body: data })
        return Project.fromJSON(res)
    }

    public async updateWellCase(body: Components.Schemas.WellCaseDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.put("", { body })
        return Project.fromJSON(res)
    }
}

export const WellCaseService = new __WellCaseService({
    ...config.WellCaseService,
    accessToken: window.sessionStorage.getItem("loginAccessToken")!,
})

export function GetWellCaseService() {
    return new __WellCaseService({
        ...config.WellCaseService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
