import { ITimeSeries } from "../../ITimeSeries"

export class Co2EmissionsCostProfile implements Components.Schemas.Co2EmissionsDto, ITimeSeries {
    id?: string
    startYear?: number
    values?: number []
    sum?: number

    constructor(data?: Components.Schemas.Co2EmissionsDto) {
        if (data !== undefined && data !== null) {
            this.id = data.id
            this.startYear = data.startYear ?? 0
            this.values = data.values ?? []
            this.sum = data.sum
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.startYear = 0
            this.values = []
        }
    }

    static fromJson(data?: Components.Schemas.NetSalesGasDto): Co2EmissionsCostProfile | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new Co2EmissionsCostProfile(data)
    }
}
