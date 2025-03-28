import { __BaseService } from "./__BaseService"

class TechnicalInputService extends __BaseService {
    public async updateWells(projectId: string, body: Components.Schemas.UpdateWellsDto): Promise<Components.Schemas.ProjectDataDto> {
        const res: Components.Schemas.ProjectDataDto = await this.put(`projects/${projectId}/wells`, { body })

        return res
    }
}

export const GetTechnicalInputService = () => new TechnicalInputService()
