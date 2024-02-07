import { __BaseService } from "./__BaseService"

import { config } from "./config"

import { GetToken, LoginAccessTokenKey } from "../Utils/common"

class CaseWithAssetsService extends __BaseService {
    public async update(body: any): Promise<Components.Schemas.ProjectWithGeneratedProfilesDto> {
        const res: Components.Schemas.ProjectWithGeneratedProfilesDto = await this.put("", { body })
        return res
    }
}

export async function GetCaseWithAssetsService() {
    return new CaseWithAssetsService({
        ...config.CaseWithAssetsService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
