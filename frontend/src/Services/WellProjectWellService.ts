/* eslint-disable max-len */
import { __BaseService } from "./__BaseService"

import { Project } from "../models/Project"
import { config } from "./config"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { WellProjectWell } from "../models/WellProjectWell"

class __WellProjectWellService extends __BaseService {
    public async getWellProjectWells() {
        const wellProjectWells: Components.Schemas.WellProjectWellDto[] = await this.get<Components.Schemas.WellProjectWellDto[]>("")
        return wellProjectWells.map(WellProjectWell.fromJSON)
    }

    public async getWellProjectWellsByProjectId(projectId: string) {
        // eslint-disable-next-line max-len
        const wellProjectWells: Components.Schemas.WellProjectWellDto[] = await this.getWithParams("", { params: { projectId } })
        return wellProjectWells.map(WellProjectWell.fromJSON)
    }

    async getWellProjectWellById(id: string) {
        const wellProjectWell: Components.Schemas.WellProjectWellDto = await this.get<Components.Schemas.WellProjectWellDto>(`/${id}`)
        return WellProjectWell.fromJSON(wellProjectWell)
    }

    public async createWellProjectWell(data: Components.Schemas.WellProjectWellDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.post("", { body: data })
        return Project.fromJSON(res)
    }

    public async updateWellProjectWell(body: Components.Schemas.WellProjectWellDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.put("", { body })
        return Project.fromJSON(res)
    }

    public async updateMultipleWellProjectWells(body: Components.Schemas.WellProjectWellDto[]): Promise<any> {
        const res: Components.Schemas.WellProjectWellDto[] = await this.put("/multiple", { body })
        return res
    }

    public async createMultipleWellProjectWell(data: Components.Schemas.WellProjectWellDto[]): Promise<any> {
        const res: Components.Schemas.WellProjectWellDto[] = await this.post("/multiple", { body: data })
        return res
    }
}

export const WellProjectWellService = new __WellProjectWellService({
    ...config.WellProjectWellService,
    accessToken: window.sessionStorage.getItem("loginAccessToken")!,
})

export async function GetWellProjectWellService() {
    return new __WellProjectWellService({
        ...config.WellProjectWellService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
