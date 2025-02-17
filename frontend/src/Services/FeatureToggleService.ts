import { __BaseService } from "./__BaseService"

export class __FeatureToggleService extends __BaseService {
    async getFeatureToggles() {
        const toggles: Components.Schemas.FeatureToggleDto = await this.get<Components.Schemas.FeatureToggleDto>("feature-toggles")
        return toggles
    }
}

export const GetFeatureToggleService = () => new __FeatureToggleService()
