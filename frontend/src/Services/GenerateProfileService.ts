import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { GetToken, LoginAccessTokenKey } from "../Utils/common"
import { GAndGAdminCost } from "../models/assets/exploration/GAndGAdminCost"
import { OpexCostProfile } from "../models/case/OpexCostProfile"
import { StudyCostProfile } from "../models/case/StudyCostProfile"
import { CaseCessationCostProfile } from "../models/case/CaseCessationCostProfile"
import { NetSalesGas } from "../models/assets/drainagestrategy/NetSalesGas"
import { ImportedElectricity } from "../models/assets/drainagestrategy/ImportedElectricity"
import { Co2Emissions } from "../models/assets/drainagestrategy/Co2Emissions"
import { FuelFlaringAndLosses } from "../models/assets/drainagestrategy/FuelFlaringAndLosses"

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
        const costProfile: Components.Schemas.StudyCostProfileDto = await this.post<Components.Schemas.StudyCostProfileDto>(`/${id}/calculateStudy`)
        return StudyCostProfile.fromJSON(costProfile)
    }

    async generateCessationCost(id: string) {
        // eslint-disable-next-line max-len
        const costProfile: Components.Schemas.CessationCostDto = await this.post<Components.Schemas.CessationCostDto>(`/${id}/generateCessation`)
        return CaseCessationCostProfile.fromJSON(costProfile)
    }

    async generateNetSaleProfile(id:string) {
        // eslint-disable-next-line max-len
        const profile: Components.Schemas.NetSalesGasDto = await this.post<Components.Schemas.NetSalesGasDto>(`/${id}/generateNetSaleGas`)
        return NetSalesGas.fromJson(profile)
    }

    async generateImportedElectricityProfile(id:string) {
        // eslint-disable-next-line max-len
        const profile: Components.Schemas.ImportedElectricityDto = await this.post<Components.Schemas.ImportedElectricityDto>(`/${id}/generateImportedElectricity`)
        return ImportedElectricity.fromJson(profile)
    }

    async generateCo2EmissionsProfile(id:string) {
        // eslint-disable-next-line max-len
        const profile: Components.Schemas.Co2EmissionsDto = await this.post<Components.Schemas.Co2EmissionsDto>(`/${id}/generateCo2Emissions`)
        return Co2Emissions.fromJson(profile)
    }

    async generateFuelFlaringLossesProfile(id:string) {
        // eslint-disable-next-line max-len
        const profile:Components.Schemas.FuelFlaringAndLossesDto = await this.post < Components.Schemas.FuelFlaringAndLossesDto>(`/${id}/generateFuelFlaringLosses`)
        return FuelFlaringAndLosses.fromJson(profile)
    }
}

export async function GetGenerateProfileService() {
    return new __GenerateProfileService({
        ...config.GenerateProfileService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
