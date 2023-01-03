import { ITimeSeries } from "../../ITimeSeries"

export class Co2Intensity implements Components.Schemas.Co2IntensityDto, ITimeSeries {
    id?: string
    startYear?: number
    name?: string
    values?: number []
    sum?: number

    constructor(data?: Components.Schemas.Co2IntensityDto) {
        if (data !== undefined && data !== null) {
            this.id = data.id
            this.startYear = data.startYear ?? 0
            this.name = "CO2 intensity"
            this.values = data.values ?? []
            this.sum = data.sum
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.startYear = 0
            this.name = "CO2 intensity"
            this.values = []
        }
    }

    static fromJson(data?: Components.Schemas.Co2IntensityDto): Co2Intensity | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new Co2Intensity(data)
    }
}
