import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { getToken, loginAccessTokenKey } from "../Utils/common"

class AccessService extends __BaseService {
    async deletePerson(projectId: string, personId: string) {
        const res: Components.Schemas.ProjectMemberDto = await this.delete<Components.Schemas.ProjectMemberDto>(`/${projectId}/members/${personId}`)
        return res
    }

    public async addPerson(projectId: string, body: Components.Schemas.ProjectMemberDto): Promise<Components.Schemas.ProjectMemberDto> {
        const res: Components.Schemas.ProjectMemberDto = await this.post<Components.Schemas.ProjectMemberDto>(
            `${projectId}/members`,
            { body },
        )
        return res
    }
}

export const GetAccessService = async () => new AccessService({
    ...config.AccessService,
    accessToken: await getToken(loginAccessTokenKey)!,
})
