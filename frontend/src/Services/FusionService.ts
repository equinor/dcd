import { config } from "./config"
import { __BaseService } from "./__BaseService"
import { GetToken, FusionAccessTokenKey } from "../Utils/common"

export class __FusionService extends __BaseService {
    getFusionProjects() {
        return this.get("/contexts?$filter=type%20in%20(%27OrgChart%27)")
    }
}

export async function GetFusionService() {
    return new __FusionService({
        ...config.FusionService,
        accessToken: await GetToken(FusionAccessTokenKey)!,
    })
}
