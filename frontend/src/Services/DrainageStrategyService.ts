import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"

export class __DrainageStrategyService extends __BaseService {
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
