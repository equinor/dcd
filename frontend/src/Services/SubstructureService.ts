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
}

export const GetSubstructureService = async () => new SubstructureService({
    ...config.BaseUrl,
    accessToken: await getToken(loginAccessTokenKey)!,
})
