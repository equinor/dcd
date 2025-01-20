import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

class SurfService extends __BaseService {
    public async updateSurf(
        projectId: string,
        caseId: string,
        surfId: string,
        dto: Components.Schemas.UpdateSurfDto,
    ): Promise<Components.Schemas.SurfDto> {
        const res: Components.Schemas.SurfDto = await this.put(
            `projects/${projectId}/cases/${caseId}/surfs/${surfId}`,
            { body: dto },
        )
        return res
    }

    public async createSurfCostProfileOverride(
        projectId: string,
        caseId: string,
        surfId: string,
        dto: Components.Schemas.CreateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/surfs/${surfId}/cost-profile-override/`,
            { body: dto },
        )
        return res
    }

    public async updateSurfCostProfileOverride(
        projectId: string,
        caseId: string,
        surfId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/surfs/${surfId}/cost-profile-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }
}

export const GetSurfService = async () => new SurfService({
    ...config.BaseUrl,
    accessToken: await getToken(loginAccessTokenKey)!,
})
