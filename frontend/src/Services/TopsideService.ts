import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

class TopsideService extends __BaseService {
    public async updateTopside(
        projectId: string,
        caseId: string,
        topsideId: string,
        dto: Components.Schemas.APIUpdateTopsideDto,
    ): Promise<Components.Schemas.TopsideDto> {
        const res: Components.Schemas.TopsideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/topsides/${topsideId}`,
            { body: dto },
        )
        return res
    }

    public async createTopsideCostProfileOverride(
        projectId: string,
        caseId: string,
        topsideId: string,
        dto: Components.Schemas.CreateTopsideCostProfileOverrideDto,
    ): Promise<Components.Schemas.TopsideCostProfileOverrideDto> {
        const res: Components.Schemas.TopsideCostProfileOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/topsides/${topsideId}/cost-profile-override/`,
            { body: dto },
        )
        return res
    }

    public async updateTopsideCostProfileOverride(
        projectId: string,
        caseId: string,
        topsideId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTopsideCostProfileOverrideDto,
    ): Promise<Components.Schemas.TopsideCostProfileOverrideDto> {
        const res: Components.Schemas.TopsideCostProfileOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/topsides/${topsideId}/cost-profile-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }
}

export const GetTopsideService = async () => new TopsideService({
    ...config.BaseUrl,
    accessToken: await getToken(loginAccessTokenKey)!,
})
