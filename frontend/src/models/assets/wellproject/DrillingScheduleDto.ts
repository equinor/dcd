export class DrillingScheduleDto implements Components.Schemas.DrillingScheduleDto {
    startYear: number | undefined
    values: number [] | null

    constructor(data?: Components.Schemas.DrillingScheduleDto) {
        this.startYear = data?.startYear
        this.values = data?.values ?? []
    }

    static fromJSON(data?: Components.Schemas.DrillingScheduleDto): DrillingScheduleDto {
        return new DrillingScheduleDto(data!)
    }
}
