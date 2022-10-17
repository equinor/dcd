import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { ExplorationOperationalWellCosts } from "../models/ExplorationOperationalWellCosts"

export class __ExplorationOperationalWellCostsService extends __BaseService {
    public async create(body: Components.Schemas.ExplorationOperationalWellCostsDto):
        Promise<Components.Schemas.ExplorationOperationalWellCostsDto> {
        const res: Components.Schemas.ExplorationOperationalWellCostsDto = await this.post("", { body })
        return ExplorationOperationalWellCosts.fromJSON(res)
    }

    public async update(body: Components.Schemas.ExplorationOperationalWellCostsDto):
        Promise<Components.Schemas.ExplorationOperationalWellCostsDto> {
        const res: Components.Schemas.ExplorationOperationalWellCostsDto = await this.put("", { body })
        return ExplorationOperationalWellCosts.fromJSON(res)
    }
}

export async function GetExplorationOperationalWellCostsService() {
    return new __ExplorationOperationalWellCostsService({
        ...config.ExplorationOperationalWellCostsService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
