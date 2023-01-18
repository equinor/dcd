import { ITimeSeries } from "../../ITimeSeries"

export class Co2EmissionsOverride implements Components.Schemas.Co2EmissionsOverrideDto, ITimeSeries {
    id?: string // uuid
    startYear?: number // int32
    internalData?: string | null
    values?: number []
    override?: boolean

    constructor(data?: Components.Schemas.Co2EmissionsOverrideDto) {
        if (data !== null && data !== undefined) {
            this.id = data?.id
            this.startYear = data.startYear ?? 0
            this.values = data.values ?? []
            this.override = data.override
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.startYear = 0
            this.values = []
            this.override = false
        }
    }

    static fromJson(data?: Components.Schemas.Co2EmissionsOverrideDto): Co2EmissionsOverride | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new Co2EmissionsOverride(data)
    }
}
