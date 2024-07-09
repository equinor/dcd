import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { getToken, loginAccessTokenKey } from "../Utils/common"

export class CaseGeneratedProfileService extends __BaseService {
    async generateCo2IntensityProfile(projectId: string, caseId: string): Promise<Components.Schemas.Co2IntensityDto> {
        const profile = await this.get<Components.Schemas.Co2IntensityDto>(`projects/${projectId}/cases/${caseId}/co2Intensity`)
        return profile
    }

    async generateCo2IntensityTotal(projectId: string, caseId: string): Promise<number> {
        const res: number = await this.get<number>(`projects/${projectId}/cases/${caseId}/co2IntensityTotal`)
        return res
    }

    async generateCo2DrillingFlaringFuelTotals(projectId: string, caseId: string): Promise<Components.Schemas.Co2DrillingFlaringFuelTotalsDto> {
        const res = await this.get<Components.Schemas.Co2DrillingFlaringFuelTotalsDto>(`projects/${projectId}/cases/${caseId}/co2DrillingFlaringFuelTotals`)
        return res
    }
}

export const GetGenerateProfileService = async () => new CaseGeneratedProfileService({
    ...config.GenerateProfileService,
    accessToken: await getToken(loginAccessTokenKey)!,
})
