import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { getToken, loginAccessTokenKey } from "../Utils/common"

export class CaseGeneratedProfileService extends __BaseService {
    async generateCo2DrillingFlaringFuelTotals(projectId: string, caseId: string): Promise<Components.Schemas.Co2DrillingFlaringFuelTotalsDto> {
        const res = await this.get<Components.Schemas.Co2DrillingFlaringFuelTotalsDto>(`projects/${projectId}/cases/${caseId}/co2DrillingFlaringFuelTotals`)
        return res
    }
}

export const GetGenerateProfileService = async () => new CaseGeneratedProfileService({
    ...config.BaseUrl,
    accessToken: await getToken(loginAccessTokenKey)!,
})
