/* eslint-disable max-len */
import { __BaseService } from "./__BaseService"

import { Project } from "../models/Project"
import { config } from "./config"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { ExplorationWell } from "../models/ExplorationWell"

class _ExplorationWellService extends __BaseService {
    public async getExplorationWells() {
        const explorationWells: Components.Schemas.ExplorationWellDto[] = await this.get<Components.Schemas.ExplorationWellDto[]>("")
        return explorationWells.map(ExplorationWell.fromJSON)
    }

    public async getExplorationWellsByProjectId(projectId: string) {
        const explorationWells: Components.Schemas.ExplorationWellDto[] = await this.getWithParams("", { params: { projectId } })
        return explorationWells.map(ExplorationWell.fromJSON)
    }

    async getExplorationWellById(id: string) {
        const explorationWell: Components.Schemas.ExplorationWellDto = await this.get<Components.Schemas.ExplorationWellDto>(`/${id}`)
        return ExplorationWell.fromJSON(explorationWell)
    }

    public async createExplorationWell(data: Components.Schemas.ExplorationWellDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.post("", { body: data })
        return Project.fromJSON(res)
    }

    public async updateExplorationWell(body: Components.Schemas.ExplorationWellDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.put("", { body })
        return Project.fromJSON(res)
    }
}

export const ExplorationWellService = new _ExplorationWellService({
    ...config.ExplorationWellService,
    accessToken: window.sessionStorage.getItem("loginAccessToken")!,
})

export async function GetExplorationWellService() {
    return new _ExplorationWellService({
        ...config.ExplorationWellService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
