import { DrainageStrategy } from "../models/DrainageStrategy"
import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"

export class __DrainageStrategyService extends __BaseService {
    async getDrainageStrategies() {
        const drainageStrategy = await this.get<Components.Schemas.DrainageStrategyDto[]>("")
        return drainageStrategy.map(DrainageStrategy.fromJSON)
    }

    async getDrainageStrategyByID(id: string) {
        const project = await this.get<Components.Schemas.DrainageStrategyDto>(`/${id}`)
        return DrainageStrategy.fromJSON(project)
    }

    createDrainageStrategy(sourceCaseId: string, body: Components.Schemas.DrainageStrategyDto) {
        return this.postWithParams("", { body }, { params: { sourceCaseId } })
    }
}

export function GetDrainageStrategyService() {
    return new __DrainageStrategyService({
        ...config.DrainageStrategyService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
