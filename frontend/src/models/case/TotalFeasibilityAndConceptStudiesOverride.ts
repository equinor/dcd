import { ITimeSeries } from "../ITimeSeries"

export class TotalFeasibilityAndConceptStudiesOverride
    implements Components.Schemas.TotalFeasibilityAndConceptStudiesOverrideDto, ITimeSeries {
    id?: string
    startYear?: number
    values?: number[]
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined
    override?: boolean

    constructor(data?: Components.Schemas.TotalFeasibilityAndConceptStudiesOverrideDto) {
        if (data !== undefined && data !== null) {
            this.id = data.id
            this.startYear = data.startYear ?? 0
            this.values = data.values?.map((v) => Math.round((v + Number.EPSILON) * 10) / 10) ?? []
            this.epaVersion = data.epaVersion ?? ""
            this.currency = data.currency
            this.sum = data.sum
            this.override = data.override
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.startYear = 0
            this.values = []
        }
    }

    static fromJSON(data?: Components.Schemas.TotalFeasibilityAndConceptStudiesOverrideDto):
        TotalFeasibilityAndConceptStudiesOverride | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new TotalFeasibilityAndConceptStudiesOverride(data)
    }
}
