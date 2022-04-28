export interface ITimeSeries {
    id?: string
    startYear?: number
    values?: number []
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined
}
