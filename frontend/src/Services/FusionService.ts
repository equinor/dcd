import { config } from "./config"
import { __BaseService } from "./__BaseService"

export class __FusionService extends __BaseService {
    getFusionProjects() {
        return this.get("/contexts?$filter=type%20in%20(%27OrgChart%27)")
    }
}

export const fusionService = new __FusionService({
    ...config.FusionService,
    accessToken: window.sessionStorage.getItem("fusionAccessToken")!,
})
