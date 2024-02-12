import { __BaseService } from "./__BaseService"

import { config } from "./config"

import { GetToken, LoginAccessTokenKey } from "../Utils/common"

class __TechnicalInputService extends __BaseService {
    public async update(projectId: string, body: any): Promise<Components.Schemas.TechnicalInputDto> {
        const res: Components.Schemas.TechnicalInputDto = await this.put(`projects/${projectId}/technical-input`, { body })
        return res
    }
}

export const TechnicalInputService = new __TechnicalInputService({
    ...config.TechnicalInputService,
    accessToken: window.sessionStorage.getItem("loginAccessToken")!,
})

export const GetTechnicalInputService = async () => {
    return new __TechnicalInputService({
        ...config.TechnicalInputService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
