import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"

export class __ExplorationService extends __BaseService {
    createExploration(sourceCaseId: string, body: Components.Schemas.ExplorationDto) {
        return this.postWithParams("", { body }, { params: { sourceCaseId } })
    }
}

export function GetExplorationService() {
    return new __ExplorationService({
        ...config.ExplorationService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
