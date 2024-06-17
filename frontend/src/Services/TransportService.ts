import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

class TransportService extends __BaseService {
    public async updateTransport(
        projectId: string,
        caseId: string,
        transportId: string,
        dto: Components.Schemas.APIUpdateTransportDto,
    ): Promise<Components.Schemas.TransportDto> {
        const res: Components.Schemas.TransportDto = await this.put(
            `projects/${projectId}/cases/${caseId}/transports/${transportId}`,
            { body: dto },
        )
        return res
    }

    public async createTransportCostProfileOverride(
        projectId: string,
        caseId: string,
        transportId: string,
        dto: Components.Schemas.CreateTransportCostProfileOverrideDto,
    ): Promise<Components.Schemas.TransportCostProfileOverrideDto> {
        const res: Components.Schemas.TransportCostProfileOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/transports/${transportId}/cost-profile-override/`,
            { body: dto },
        )
        return res
    }

    public async updateTransportCostProfileOverride(
        projectId: string,
        caseId: string,
        transportId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTransportCostProfileOverrideDto,
    ): Promise<Components.Schemas.TransportCostProfileOverrideDto> {
        const res: Components.Schemas.TransportCostProfileOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/transports/${transportId}/cost-profile-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }
}

export const GetTransportService = async () => new TransportService({
    ...config.BaseUrl,
    accessToken: await getToken(loginAccessTokenKey)!,
})
