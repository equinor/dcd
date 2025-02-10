import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

class TransportService extends __BaseService {
    public async updateTransport(
        projectId: string,
        caseId: string,
        transportId: string,
        dto: Components.Schemas.UpdateTransportDto,
    ): Promise<Components.Schemas.TransportDto> {
        const res: Components.Schemas.TransportDto = await this.put(
            `projects/${projectId}/cases/${caseId}/transports/${transportId}`,
            { body: dto },
        )
        return res
    }
}

export const GetTransportService = async () => new TransportService({
    ...config.BaseUrl,
    accessToken: await getToken(loginAccessTokenKey)!,
})
