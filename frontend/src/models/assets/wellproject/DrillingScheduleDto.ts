export class DrillingScheduleDto implements Components.Schemas.DrillingScheduleDto {
    id?: string
    startYear?: number | undefined
    values?: number [] | null

    constructor(data?: Components.Schemas.DrillingScheduleDto) {
        if (data !== undefined && data !== null) {
            this.id = data?.id
            this.startYear = data?.startYear
            this.values = data?.values ?? []
        }
    }

    static fromJSON(data?: Components.Schemas.DrillingScheduleDto): DrillingScheduleDto | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new DrillingScheduleDto(data!)
    }
}
