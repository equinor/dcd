import { config } from "./config"
import { __BaseService } from "./__BaseService"
import { getToken, loginAccessTokenKey } from "@/Utils/common"

class __OrgChartMembersService extends __BaseService {
    public async getOrgChartPeople(projectId: string, contextId: string): Promise<Components.Schemas.ProjectMemberDto[]> {
        const res: Components.Schemas.ProjectMemberDto[] = await this.get<Components.Schemas.ProjectMemberDto[]>(`projects/${projectId}/members/context/${contextId}`)
        return res
    }
}

export const GetOrgChartMembersService = async () => new __OrgChartMembersService({
    ...config.BaseUrl,
    accessToken: await getToken(loginAccessTokenKey)!,
})
