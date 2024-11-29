import { config } from "./config"
import { FusionPersonV1 } from "@/Models/AccessManagement"
import { __BaseService } from "./__BaseService"
import { getToken, loginAccessTokenKey } from "@/Utils/common"

class __OrgChartMembersService extends __BaseService {
    public async getOrgChartPeople(contextId: string): Promise<FusionPersonV1[]> {
        const res: FusionPersonV1[] = await this.get<FusionPersonV1[]>(`/context/${contextId}`)
        return res
    }
}

export const GetOrgChartMembersService = async () => new __OrgChartMembersService({
    ...config.OrgChartService,
    accessToken: await getToken(loginAccessTokenKey)!,
})
