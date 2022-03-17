export class ExplorationDrillingScheduleDto implements Components.Schemas.ExplorationDrillingScheduleDto {
    startYear?: number | undefined
    values?: number [] | null

    constructor(data?: Components.Schemas.ExplorationDrillingScheduleDto) {
        this.startYear = data?.startYear
        this.values = data?.values ?? []
    }

    static fromJSON(data?: Components.Schemas.ExplorationDrillingScheduleDto): ExplorationDrillingScheduleDto {
        return new ExplorationDrillingScheduleDto(data)
    }
}
