import { __BaseService } from "./__BaseService"

import { config } from "./config"

import { getToken, loginAccessTokenKey } from "../Utils/common"

class __TechnicalInputService extends __BaseService {
    // denne har endret type til å bare ta update create delete wells
    public async updateWells(projectId: string, body: Components.Schemas.UpdateTechnicalInputDto): Promise<Components.Schemas.ProjectDataDto> {
        const res: Components.Schemas.ProjectDataDto = await this.put(`projects/${projectId}/wells`, { body })
        return res
    }

    public async updateDevelopmentOperationalWellCosts(
        projectId: string,
        developmentOperationalWellCostsId: string,
        body: any,
    ): Promise<Components.Schemas.DevelopmentOperationalWellCostsDto> {
        const res: Components.Schemas.DevelopmentOperationalWellCostsDto = await this.put(
            `projects/${projectId}/development-operational-well-costs/${developmentOperationalWellCostsId}`,
            { body },
        )
        return res
    }

    // denne har endret returtype
    public async updateExplorationOperationalWellCosts(
        projectId: string,
        explorationOperationalWellCostsId: string,
        body: any,
    ): Promise<Components.Schemas.ExplorationOperationalWellCostsDto> {
        const res: Components.Schemas.ExplorationOperationalWellCostsDto = await this.put(
            `projects/${projectId}/exploration-operational-well-costs/${explorationOperationalWellCostsId}`,
            { body },
        )
        return res
    }
}

export const TechnicalInputService = new __TechnicalInputService({
    ...config.TechnicalInputService,
    accessToken: window.sessionStorage.getItem("loginAccessToken")!,
})

export const GetTechnicalInputService = async () => new __TechnicalInputService({
    ...config.TechnicalInputService,
    accessToken: await getToken(loginAccessTokenKey)!,
})
