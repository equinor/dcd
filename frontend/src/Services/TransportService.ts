import { __BaseService } from "./__BaseService"

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

export const GetTransportService = () => new TransportService()
