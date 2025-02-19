import { __BaseService } from "./__BaseService"

class DrainageStrategyService extends __BaseService {
    public async updateDrainageStrategy(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.UpdateDrainageStrategyDto,
    ): Promise<Components.Schemas.DrainageStrategyDto> {
        const res: Components.Schemas.DrainageStrategyDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategy`,
            { body: dto },
        )
        return res
    }
}

export const GetDrainageStrategyService = () => new DrainageStrategyService()
