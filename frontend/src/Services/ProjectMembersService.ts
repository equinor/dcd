import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { getToken, loginAccessTokenKey } from "../Utils/common"
import { FusionPersonV1, User } from "@/Models/AccessManagement"

class __ProjectMembersService extends __BaseService {
    async deletePerson(projectId: string, userId: string) {
        const res: Components.Schemas.ProjectMemberDto = await this.delete<Components.Schemas.ProjectMemberDto>(`/${projectId}/members/${userId}`)
        return res
    }

    public async addPerson(projectId: string, body: User): Promise<Components.Schemas.ProjectMemberDto> {
        const res: Components.Schemas.ProjectMemberDto = await this.post<Components.Schemas.ProjectMemberDto>(
            `${projectId}/members`,
            { body },
        )
        return res
    }

    // hvordan finne rett context??? for i urlen er det bare project
    public async getOrgChartPeople(contextId: string): Promise<FusionPersonV1[]> {
        const res: FusionPersonV1[] = await this.getWithParams(
            "",
            { params: { contextId } },
        )
        return res
    }
}

export const GetProjectMembersService = async () => new __ProjectMembersService({
    ...config.AccessService,
    accessToken: await getToken(loginAccessTokenKey)!,
})
