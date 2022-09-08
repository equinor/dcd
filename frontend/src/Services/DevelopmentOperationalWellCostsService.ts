/* eslint-disable max-len */
import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { DevelopmentOperationalWellCosts } from "../models/DevelopmentOperationalWellCosts"

export class __DevelopmentOperationalWellCostsService extends __BaseService {
    public async create(body: Components.Schemas.DevelopmentOperationalWellCostsDto):
        Promise<Components.Schemas.DevelopmentOperationalWellCostsDto> {
        const res: Components.Schemas.DevelopmentOperationalWellCostsDto = await this.post("", { body })
        return DevelopmentOperationalWellCosts.fromJSON(res)
    }

    public async update(body: Components.Schemas.DevelopmentOperationalWellCostsDto): Promise<Components.Schemas.DevelopmentOperationalWellCostsDto> {
        const res: Components.Schemas.DevelopmentOperationalWellCostsDto = await this.put("", { body })
        return DevelopmentOperationalWellCosts.fromJSON(res)
    }
}

export async function GetDevelopmentOperationalWellCostsService() {
    return new __DevelopmentOperationalWellCostsService({
        ...config.DevelopmentOperationalWellCostsService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
