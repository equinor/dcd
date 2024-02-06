import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { GetToken, LoginAccessTokenKey } from "../Utils/common"
import { Co2Intensity } from "../models/assets/drainagestrategy/Co2Intensity"
import { Co2DrillingFlaringFuelTotals } from "../models/assets/drainagestrategy/Co2DrillingFlaringFuelTotals"

export class CaseGeneratedProfileService extends __BaseService {
    async generateOpexCost(projectId: string, caseId: string) {
        const costProfiles: Components.Schemas.OpexCostProfileWrapperDto = await this.get<Components.Schemas.OpexCostProfileWrapperDto>(`projects/${projectId}/cases/${caseId}/opex`)
        return costProfiles
    }

    async generateStudyCost(projectId: string, caseId: string) {
        const costProfiles: Components.Schemas.StudyCostProfileWrapperDto = await this.get<Components.Schemas.StudyCostProfileWrapperDto>(`projects/${projectId}/cases/${caseId}/study`)
        return costProfiles
    }

    async generateCessationCost(projectId: string, caseId: string) {
        const costProfiles: Components.Schemas.CessationCostWrapperDto = await this.get<Components.Schemas.CessationCostWrapperDto>(`projects/${projectId}/cases/${caseId}/cessation`)
        return costProfiles
    }

    async generateCo2IntensityProfile(projectId: string, caseId: string) {
        const profile: Components.Schemas.Co2IntensityDto = await this.get<Components.Schemas.Co2IntensityDto>(`projects/${projectId}/cases/${caseId}/co2Intensity`)
        return Co2Intensity.fromJson(profile)
    }

    async generateCo2IntensityTotal(projectId: string, caseId: string) {
        const res: number = await this.get<number>(`projects/${projectId}/cases/${caseId}/co2IntensityTotal`)
        return res
    }

    async generateCo2DrillingFlaringFuelTotals(projectId: string, caseId: string) {
        const res: Components.Schemas.Co2DrillingFlaringFuelTotalsDto = await this.get<Components.Schemas.Co2DrillingFlaringFuelTotalsDto>(`projects/${projectId}/cases/${caseId}/co2DrillingFlaringFuelTotals`)
        return Co2DrillingFlaringFuelTotals.fromJson(res)
    }
}

export async function GetGenerateProfileService() {
    return new CaseGeneratedProfileService({
        ...config.GenerateProfileService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
