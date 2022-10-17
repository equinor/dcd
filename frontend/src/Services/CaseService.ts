import { __BaseService } from "./__BaseService"

import { Project } from "../models/Project"
import { config } from "./config"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { GAndGAdminCost } from "../models/assets/exploration/GAndGAdminCost"
import { OpexCostProfile } from "../models/case/OpexCostProfile"
import { StudyCostProfile } from "../models/case/StudyCostProfile"
import { Case } from "../models/case/Case"
import { CaseCessationCostProfile } from "../models/case/CaseCessationCostProfile"

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

    async generateGAndGAdminCost(id: string) {
        // eslint-disable-next-line max-len
        const costProfile: Components.Schemas.GAndGAdminCostDto = await this.post<Components.Schemas.GAndGAdminCostDto>(`/${id}/generateGAndGAdminCost`)
        return GAndGAdminCost.fromJSON(costProfile)
    }

    async generateOpexCost(id: string) {
        // eslint-disable-next-line max-len
        const costProfile: Components.Schemas.OpexCostProfileDto = await this.post<Components.Schemas.OpexCostProfileDto>(`/${id}/calculateOpex`)
        return OpexCostProfile.fromJSON(costProfile)
    }

    async generateStudyCost(id: string) {
        // eslint-disable-next-line max-len
        const costProfile: Components.Schemas.StudyCostProfileDto = await this.post<Components.Schemas.StudyCostProfileDto>(`/${id}/calculateStudy`)
        return StudyCostProfile.fromJSON(costProfile)
    }

    async generateCessationCost(id: string) {
        // eslint-disable-next-line max-len
        const costProfile: Components.Schemas.CessationCostDto = await this.post<Components.Schemas.CessationCostDto>(`/${id}/generateCessation`)
        return CaseCessationCostProfile.fromJSON(costProfile)
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
