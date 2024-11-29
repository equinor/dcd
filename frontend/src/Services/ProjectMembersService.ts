import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { getToken, loginAccessTokenKey } from "../Utils/common"

class __ProjectMembersService extends __BaseService {
    async deletePerson(projectId: string, userId: string) {
        const res: Components.Schemas.ProjectMemberDto = await this.delete<Components.Schemas.ProjectMemberDto>(`/${projectId}/members/${userId}`)
        return res
    }

    public async addPerson(projectId: string, body: Components.Schemas.CreateProjectMemberDto): Promise<Components.Schemas.ProjectMemberDto> {
        const res: Components.Schemas.ProjectMemberDto = await this.post<Components.Schemas.ProjectMemberDto>(
            `${projectId}/members`,
            { body },
        )
        return res
    }

    public async updatePerson(projectId: string, body: Components.Schemas.UpdateProjectMemberDto): Promise<Components.Schemas.ProjectMemberDto> {
        const res: Components.Schemas.ProjectMemberDto = await this.put<Components.Schemas.ProjectMemberDto>(
            `${projectId}/members`,
            { body },
        )
        return res
    }
}

export const GetProjectMembersService = async () => new __ProjectMembersService({
    ...config.AccessService,
    accessToken: await getToken(loginAccessTokenKey)!,
})
