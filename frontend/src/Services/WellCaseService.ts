import { __BaseService } from "./__BaseService"

import { Project } from "../models/Project"
import { config } from "./config"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { Well } from "../models/Well"

class __WellCaseService extends __BaseService {
    public async getWellCases() {
        const wells: Components.Schemas.WellDto[] = await this.get<Components.Schemas.WellDto[]>("")
        return wells.map(Well.fromJSON)
    }

    public async getWellCasesByProjectId(projectId: string) {
        // eslint-disable-next-line max-len
        const wells: Components.Schemas.WellDto[] = await this.getWithParams("", { params: { projectId } })
        return wells.map(Well.fromJSON)
    }

    async getWellCaseById(id: string) {
        const well: Components.Schemas.WellDto = await this.get<Components.Schemas.WellDto>(`/${id}`)
        return Well.fromJSON(well)
    }

    public async createWellCase(data: Components.Schemas.WellDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.post("", { body: data })
        return Project.fromJSON(res)
    }

    public async updateWellCase(body: Components.Schemas.WellDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.put("", { body })
        return Project.fromJSON(res)
    }
}

export const WellService = new __WellCaseService({
    ...config.WellService,
    accessToken: window.sessionStorage.getItem("loginAccessToken")!,
})

export function GetWellService() {
    return new __WellCaseService({
        ...config.WellCaseService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
