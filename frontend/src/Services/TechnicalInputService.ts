import { __BaseService } from "./__BaseService"

class __TechnicalInputService extends __BaseService {
    public async updateWells(projectId: string, body: Components.Schemas.UpdateWellsDto): Promise<Components.Schemas.ProjectDataDto> {
        const res: Components.Schemas.ProjectDataDto = await this.put(`projects/${projectId}/wells`, { body })
        return res
    }

    public async updateDevelopmentOperationalWellCosts(
        projectId: string,
        developmentOperationalWellCostsId: string,
        body: any,
    ): Promise<Components.Schemas.UpdateDevelopmentOperationalWellCostsDto> {
        const res: Components.Schemas.UpdateDevelopmentOperationalWellCostsDto = await this.put(
            `projects/${projectId}/development-operational-well-costs/${developmentOperationalWellCostsId}`,
            { body },
        )
        return res
    }

    public async updateExplorationOperationalWellCosts(
        projectId: string,
        explorationOperationalWellCostsId: string,
        body: any,
    ): Promise<Components.Schemas.UpdateExplorationOperationalWellCostsDto> {
        const res: Components.Schemas.UpdateExplorationOperationalWellCostsDto = await this.put(
            `projects/${projectId}/exploration-operational-well-costs/${explorationOperationalWellCostsId}`,
            { body },
        )
        return res
    }
}

export const GetTechnicalInputService = () => new __TechnicalInputService()
