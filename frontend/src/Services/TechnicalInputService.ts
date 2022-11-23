import { __BaseService } from "./__BaseService"

import { Project } from "../models/Project"
import { config } from "./config"

import { GetToken, LoginAccessTokenKey } from "../Utils/common"
import { Case } from "../models/case/Case"

class __TechnicalInputService extends __BaseService {
    public async update(body: any): Promise<Components.Schemas.TechnicalInputDto> {
        const res: Components.Schemas.TechnicalInputDto = await this.put("", { body })
        return res
    }
}

export const TechnicalInputService = new __TechnicalInputService({
    ...config.TechnicalInputService,
    accessToken: window.sessionStorage.getItem("loginAccessToken")!,
})

export async function GetTechnicalInputService() {
    return new __TechnicalInputService({
        ...config.TechnicalInputService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
