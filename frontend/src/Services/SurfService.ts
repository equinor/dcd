import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"

export class __SurfService extends __BaseService {
    createSurf(sourceCaseId: string, body: Components.Schemas.SurfDto) {
        return this.postWithParams("", { body }, { params: { sourceCaseId } })
    }
}

export function GetSurfService() {
    return new __SurfService({
        ...config.SurfService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
