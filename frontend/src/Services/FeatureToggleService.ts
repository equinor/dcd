import { __BaseService } from "./__BaseService"

import { getToken, loginAccessTokenKey } from "../Utils/common"

export class __FeatureToggleService extends __BaseService {
    async getFeatureToggles() {
        const toggles: Components.Schemas.FeatureToggleDto = await this.get<Components.Schemas.FeatureToggleDto>("feature-toggles")
        return toggles
    }
}

export const GetFeatureToggleService = async () => new __FeatureToggleService({
    accessToken: await getToken(loginAccessTokenKey)!,
})
