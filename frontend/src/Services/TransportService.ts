import { __BaseService } from "./__BaseService"
import { getToken, loginAccessTokenKey } from "../Utils/common"

class TransportService extends __BaseService {
    public async updateTransport(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.UpdateTransportDto,
    ): Promise<Components.Schemas.TransportDto> {
        const res: Components.Schemas.TransportDto = await this.put(
            `projects/${projectId}/cases/${caseId}/transport`,
            { body: dto },
        )
        return res
    }
}

export const GetTransportService = async () => new TransportService({
    accessToken: await getToken(loginAccessTokenKey)!,
})
