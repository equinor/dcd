import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { GetToken, LoginAccessTokenKey } from "../Utils/common"

export class CaseGeneratedProfileService extends __BaseService {
    async generateOpexCost(projectId: string, caseId: string): Promise<Components.Schemas.OpexCostProfileWrapperDto> {
        const costProfiles = await this.get<Components.Schemas.OpexCostProfileWrapperDto>(`projects/${projectId}/cases/${caseId}/opex`)
        return costProfiles
    }

    async generateStudyCost(projectId: string, caseId: string): Promise<Components.Schemas.StudyCostProfileWrapperDto> {
        const costProfiles = await this.get<Components.Schemas.StudyCostProfileWrapperDto>(`projects/${projectId}/cases/${caseId}/study`)
        return costProfiles
    }

    async generateCessationCost(projectId: string, caseId: string): Promise<Components.Schemas.CessationCostWrapperDto> {
        const costProfiles = await this.get<Components.Schemas.CessationCostWrapperDto>(`projects/${projectId}/cases/${caseId}/cessation`)
        return costProfiles
    }

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

export const GetGenerateProfileService = async () => {
    return new CaseGeneratedProfileService({
        ...config.GenerateProfileService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
