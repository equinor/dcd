import { __BaseService } from "./__BaseService"

import { Project } from "../models/Project"
import { config } from "./config"

import { GetToken, LoginAccessTokenKey } from "../Utils/common"
import { Case } from "../models/case/Case"

class __CaseWithAssetsService extends __BaseService {
    public async update(body: any): Promise<Components.Schemas.ProjectWithGeneratedProfilesDto> {
        const res: Components.Schemas.ProjectWithGeneratedProfilesDto = await this.put("", { body })
        return res
    }
}

export const CaseWithAssetsService = new __CaseWithAssetsService({
    ...config.CaseWithAssetsService,
    accessToken: window.sessionStorage.getItem("loginAccessToken")!,
})

export async function GetCaseWithAssetsService() {
    return new __CaseWithAssetsService({
        ...config.CaseWithAssetsService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
