export interface ITimeSeries {
    id?: string
    startYear?: number | undefined
    values?: any [] | null
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined
}