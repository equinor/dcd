import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"

export class __WellProjectService extends __BaseService {
    createWellProject(sourceCaseId: string, body: Components.Schemas.WellProjectDto) {
        return this.postWithParams("", { body }, { params: { sourceCaseId } })
    }
}

export function GetWellProjectService() {
    return new __WellProjectService({
        ...config.WellProjectService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
