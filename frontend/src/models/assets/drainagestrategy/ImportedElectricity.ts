import { ITimeSeries } from "../../ITimeSeries"

export class ImportedElectricity implements Components.Schemas.ImportedElectricityDto, ITimeSeries {
    id?: string // uuid
    startYear?: number // int32
    internalData?: string | null
    values?: number []

    constructor(data?: Components.Schemas.ImportedElectricityDto) {
        if (data !== null && data !== undefined) {
            this.id = data?.id
            this.startYear = data.startYear ?? 0
            this.values = data.values ?? []
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.startYear = 0
            this.values = []
        }
    }

    static fromJson(data?: Components.Schemas.ImportedElectricityDto): ImportedElectricity | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new ImportedElectricity(data)
    }
}
