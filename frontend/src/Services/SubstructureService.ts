import { __BaseService } from "./__BaseService"

class SubstructureService extends __BaseService {
    public async updateSubstructure(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.UpdateSubstructureDto,
    ): Promise<Components.Schemas.SubstructureDto> {
        const res: Components.Schemas.SubstructureDto = await this.put(
            `projects/${projectId}/cases/${caseId}/substructure`,
            { body: dto },
        )
        return res
    }
}

export const GetSubstructureService = () => new SubstructureService()
