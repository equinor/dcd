export class ExplorationDrillingScheduleDto implements Components.Schemas.ExplorationDrillingScheduleDto {
    id?: string
    startYear?: number | undefined
    values?: number []

    constructor(data?: Components.Schemas.ExplorationDrillingScheduleDto) {
        if (data !== undefined && data !== null) {
            this.id = data?.id
            this.startYear = data?.startYear
            this.values = data?.values ?? []
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
