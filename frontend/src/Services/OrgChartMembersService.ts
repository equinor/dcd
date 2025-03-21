import { __BaseService } from "./__BaseService"

class OrgChartMembersService extends __BaseService {
    public async getOrgChartPeople(projectId: string, contextId: string): Promise<Components.Schemas.ProjectMemberDto[]> {
        const res: Components.Schemas.ProjectMemberDto[] = await this.get<Components.Schemas.ProjectMemberDto[]>(`projects/${projectId}/members/context/${contextId}`)

        return res
    }
}

export const GetOrgChartMembersService = () => new OrgChartMembersService()
