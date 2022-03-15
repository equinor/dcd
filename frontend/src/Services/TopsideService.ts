import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"

export class __TopsideService extends __BaseService {
    createTopside(sourceCaseId: string, body: Components.Schemas.TopsideDto) {
        return this.postWithParams("", { body }, { params: { sourceCaseId } })
    }
}

export function GetTopsideService() {
    return new __TopsideService({
        ...config.TopsideService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
