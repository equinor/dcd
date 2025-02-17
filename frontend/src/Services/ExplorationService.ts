import { __BaseService } from "./__BaseService"

class ExplorationService extends __BaseService {
    public async createExplorationWellDrillingSchedule(
        projectId: string,
        caseId: string,
        explorationId: string,
        wellId: string,
        dto: Components.Schemas.CreateTimeSeriesScheduleDto,
    ): Promise<Components.Schemas.TimeSeriesScheduleDto> {
        const res: Components.Schemas.TimeSeriesScheduleDto = await this.post(
            `projects/${projectId}/cases/${caseId}/explorations/${explorationId}/wells/${wellId}/drilling-schedule/`,
            { body: dto },
        )
        return res
    }

    public async updateExplorationWellDrillingSchedule(
        projectId: string,
        caseId: string,
        explorationId: string,
        wellId: string,
        drillingScheuleId: string,
        dto: Components.Schemas.UpdateTimeSeriesScheduleDto,
    ): Promise<Components.Schemas.TimeSeriesScheduleDto> {
        const res: Components.Schemas.TimeSeriesScheduleDto = await this.put(
            `projects/${projectId}/cases/${caseId}/explorations/${explorationId}/wells/${wellId}/drilling-schedule/${drillingScheuleId}`,
            { body: dto },
        )
        return res
    }
}

export const GetExplorationService = () => new ExplorationService()
