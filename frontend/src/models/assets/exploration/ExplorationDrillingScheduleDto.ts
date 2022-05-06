import { ITimeSeries } from "../../ITimeSeries"

export class ExplorationDrillingScheduleDto implements Components.Schemas.ExplorationDrillingScheduleDto, ITimeSeries {
    id?: string
    startYear?: number
    values?: number []

    constructor(data?: Components.Schemas.ExplorationDrillingScheduleDto) {
        if (data !== undefined && data !== null) {
            this.id = data.id
            this.startYear = data.startYear ?? 0
            this.values = data.values ?? []
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.startYear = 0
            this.values = []
        }
    }

    static fromJSON(data?: Components.Schemas.ExplorationDrillingScheduleDto):
    ExplorationDrillingScheduleDto | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new ExplorationDrillingScheduleDto(data)
    }
}
