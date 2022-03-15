import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"

export class __TransportService extends __BaseService {
    createTransport(sourceCaseId: string, body: Components.Schemas.TransportDto) {
        return this.postWithParams("", { body }, { params: { sourceCaseId } })
    }
}

export function GetTransportService() {
    return new __TransportService({
        ...config.TransportService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
