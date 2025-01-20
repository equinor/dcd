import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

class SubstructureService extends __BaseService {
    public async updateSubstructure(
        projectId: string,
        caseId: string,
        substructureId: string,
        dto: Components.Schemas.UpdateSubstructureDto,
    ): Promise<Components.Schemas.SubstructureDto> {
        const res: Components.Schemas.SubstructureDto = await this.put(
            `projects/${projectId}/cases/${caseId}/substructures/${substructureId}`,
            { body: dto },
        )
        return res
    }

    public async createSubstructureCostProfileOverride(
        projectId: string,
        caseId: string,
        substructureId: string,
        dto: Components.Schemas.CreateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/substructures/${substructureId}/cost-profile-override/`,
            { body: dto },
        )
        return res
    }

    public async updateSubstructureCostProfileOverride(
        projectId: string,
        caseId: string,
        substructureId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/substructures/${substructureId}/cost-profile-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }
}

export const GetSubstructureService = async () => new SubstructureService({
    ...config.BaseUrl,
    accessToken: await getToken(loginAccessTokenKey)!,
})
