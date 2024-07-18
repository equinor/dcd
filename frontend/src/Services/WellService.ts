import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

class WellService extends __BaseService {
    public async createWell(
        projectId: string,
        data: Components.Schemas.CreateWellDto,
    ): Promise<Components.Schemas.WellDto> {
        const res: Components.Schemas.WellDto = await this.post(
            `projects/${projectId}/wells`,
            { body: data },
        )
        return res
    }

    public async updateWell(
        projectId: string,
        wellId: string,
        body: Components.Schemas.UpdateWellDto,
    ): Promise<Components.Schemas.WellDto> {
        const res: Components.Schemas.WellDto = await this.put(
            `projects/${projectId}/wells/${wellId}`,
            { body },
        )
        return res
    }

    public async checkWellIsInUse(
        projectId: string,
        wellId: string,
    ): Promise<Components.Schemas.CaseDto[]> {
        const res = await this.get(`projects/${projectId}/wells/${wellId}/affected-cases`)
        return res
    }

    public async deleteWell(
        projectId: string,
        wellId: string,
    ): Promise<void> {
        await this.delete(`projects/${projectId}/wells/${wellId}`)
    }
}

export const GetWellService = async () => new WellService({
    ...config.WellService,
    accessToken: await getToken(loginAccessTokenKey)!,
})
