import { ITimeSeries } from "../../ITimeSeries"

export class ImportedElectricityOverride implements Components.Schemas.ImportedElectricityOverrideDto, ITimeSeries {
    id?: string // uuid
    startYear?: number // int32
    internalData?: string | null
    values?: number []
    override?: boolean

    constructor(data?: Components.Schemas.ImportedElectricityOverrideDto) {
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

    static fromJson(data?: Components.Schemas.ImportedElectricityOverrideDto): ImportedElectricityOverride | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new ImportedElectricityOverride(data)
    }
}
