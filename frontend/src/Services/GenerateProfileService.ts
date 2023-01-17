import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { GetToken, LoginAccessTokenKey } from "../Utils/common"
import { GAndGAdminCost } from "../models/assets/exploration/GAndGAdminCost"
import { NetSalesGas } from "../models/assets/drainagestrategy/NetSalesGas"
import { ImportedElectricity } from "../models/assets/drainagestrategy/ImportedElectricity"
import { Co2Emissions } from "../models/assets/drainagestrategy/Co2Emissions"
import { FuelFlaringAndLosses } from "../models/assets/drainagestrategy/FuelFlaringAndLosses"
import { Co2Intensity } from "../models/assets/drainagestrategy/Co2Intensity"
import { Co2DrillingFlaringFuelTotals } from "../models/assets/drainagestrategy/Co2DrillingFlaringFuelTotals"

export class __GenerateProfileService extends __BaseService {
    async generateGAndGAdminCost(id: string) {
        // eslint-disable-next-line max-len
        const costProfile: Components.Schemas.GAndGAdminCostDto = await this.post<Components.Schemas.GAndGAdminCostDto>(`/${id}/generateGAndGAdminCost`)
        return GAndGAdminCost.fromJSON(costProfile)
    }

    async generateOpexCost(id: string) {
        // eslint-disable-next-line max-len
        const costProfiles: Components.Schemas.OpexCostProfileWrapperDto = await this.post<Components.Schemas.OpexCostProfileWrapperDto>(`/${id}/generateOpex`)
        return costProfiles
    }

    async generateStudyCost(id: string) {
        // eslint-disable-next-line max-len
        const costProfiles: Components.Schemas.StudyCostProfileWrapperDto = await this.post<Components.Schemas.StudyCostProfileWrapperDto>(`/${id}/generateStudy`)
        return costProfiles
    }

    async generateCessationCost(id: string) {
        // eslint-disable-next-line max-len
        const costProfiles: Components.Schemas.CessationCostWrapperDto = await this.post<Components.Schemas.CessationCostWrapperDto>(`/${id}/generateCessation`)
        return costProfiles
    }

    async generateNetSaleProfile(id: string) {
        // eslint-disable-next-line max-len
        const profile: Components.Schemas.NetSalesGasDto = await this.post<Components.Schemas.NetSalesGasDto>(`/${id}/generateNetSaleGas`)
        return NetSalesGas.fromJson(profile)
    }

    async generateImportedElectricityProfile(id: string) {
        // eslint-disable-next-line max-len
        const profile: Components.Schemas.ImportedElectricityDto = await this.post<Components.Schemas.ImportedElectricityDto>(`/${id}/generateImportedElectricity`)
        return ImportedElectricity.fromJson(profile)
    }

    async generateCo2EmissionsProfile(id: string) {
        // eslint-disable-next-line max-len
        const profile: Components.Schemas.Co2EmissionsDto = await this.post<Components.Schemas.Co2EmissionsDto>(`/${id}/generateCo2Emissions`)
        return Co2Emissions.fromJson(profile)
    }

    async generateFuelFlaringLossesProfile(id: string) {
        // eslint-disable-next-line max-len
        const profile: Components.Schemas.FuelFlaringAndLossesDto = await this.post<Components.Schemas.FuelFlaringAndLossesDto>(`/${id}/generateFuelFlaringLosses`)
        return FuelFlaringAndLosses.fromJson(profile)
    }

    async generateCo2IntensityProfile(id: string) {
        // eslint-disable-next-line max-len
        const profile: Components.Schemas.Co2IntensityDto = await this.post<Components.Schemas.Co2IntensityDto>(`/${id}/generateCo2Intensity`)
        return Co2Intensity.fromJson(profile)
    }

    async generateCo2IntensityTotal(caseId: string) {
        // eslint-disable-next-line max-len
        const res: number = await this.post<number>(`/${caseId}/generateCo2IntensityTotal`)
        return res
    }

    async generateCo2DrillingFlaringFuelTotals(caseId: string) {
        // eslint-disable-next-line max-len
        const res: Components.Schemas.Co2DrillingFlaringFuelTotalsDto = await this.post<Components.Schemas.Co2DrillingFlaringFuelTotalsDto>(`/${caseId}/generateCo2DrillingFlaringFuelTotals`)
        return Co2DrillingFlaringFuelTotals.fromJson(res)
    }
}

export async function GetGenerateProfileService() {
    return new __GenerateProfileService({
        ...config.GenerateProfileService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
