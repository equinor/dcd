import { __BaseService } from "./__BaseService"

import { config } from "./config"

import { getToken, loginAccessTokenKey } from "../Utils/common"

class CaseWithAssetsService extends __BaseService {
    public async update(projectId: string, caseId: string, body: any): Promise<Components.Schemas.ProjectWithGeneratedProfilesDto> {
        const res: Components.Schemas.ProjectWithGeneratedProfilesDto = await this.post(`projects/${projectId}/cases/${caseId}/case-with-assets`, { body })
        return res
    }
}

export const GetCaseWithAssetsService = async () => new CaseWithAssetsService({
    ...config.CaseWithAssetsService,
    accessToken: await getToken(loginAccessTokenKey)!,
})
