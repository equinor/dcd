import { __BaseService } from "./__BaseService"

import { config } from "./config"

import { getToken, loginAccessTokenKey } from "../Utils/common"

class __TechnicalInputService extends __BaseService {
    public async update(projectId: string, body: any): Promise<Components.Schemas.TechnicalInputDto> {
        const res: Components.Schemas.TechnicalInputDto = await this.put(`projects/${projectId}/technical-input`, { body })
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
