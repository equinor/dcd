import { __BaseService } from "./__BaseService"
import { getToken, loginAccessTokenKey } from "../Utils/common"

class WellProjectService extends __BaseService {
    public async createWellProjectWellDrillingSchedule(
        projectId: string,
        caseId: string,
        wellProjectId: string,
        wellId: string,
        dto: Components.Schemas.CreateTimeSeriesScheduleDto,
    ): Promise<Components.Schemas.TimeSeriesScheduleDto> {
        const res: Components.Schemas.TimeSeriesScheduleDto = await this.post(
            `projects/${projectId}/cases/${caseId}/well-projects/${wellProjectId}/wells/${wellId}/drilling-schedule/`,
            { body: dto },
        )
        return res
    }

    public async updateWellProjectWellDrillingSchedule(
        projectId: string,
        caseId: string,
        wellProjectId: string,
        wellId: string,
        drillingScheuleId: string,
        dto: Components.Schemas.UpdateTimeSeriesScheduleDto,
    ): Promise<Components.Schemas.TimeSeriesScheduleDto> {
        const res: Components.Schemas.TimeSeriesScheduleDto = await this.put(
            `projects/${projectId}/cases/${caseId}/well-projects/${wellProjectId}/wells/${wellId}/drilling-schedule/${drillingScheuleId}`,
            { body: dto },
        )
        return res
    }
}

export const GetWellProjectService = async () => new WellProjectService({
    accessToken: await getToken(loginAccessTokenKey)!,
})
