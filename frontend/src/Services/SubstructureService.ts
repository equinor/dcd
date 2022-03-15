import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"

export class __SubstructureService extends __BaseService {
    createSubstructure(sourceCaseId: string, body: Components.Schemas.SubstructureDto) {
        return this.postWithParams("", { body }, { params: { sourceCaseId } })
    }
}

export function GetSubstructureService() {
    return new __SubstructureService({
        ...config.SubstructureService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
