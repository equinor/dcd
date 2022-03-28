import { __BaseService } from "./__BaseService"

// import { Project } from "../models/Project"
import { config } from "./config"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"

class __STEAService extends __BaseService {
    public async excelToSTEA(projectId: string) {
        // this.client.defaults.headers.common = {
        //     Accept: "application/json",
        //     Authorization: `Bearer ${config.accessToken}`,
        //     ...this.config.headers,
        // }
        const res = await this.postWithParams("", undefined, { params: { projectId } })
        return this.post(res)
    }
}

export function GetSTEAService() {
    return new __STEAService({
        ...config.STEAService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
