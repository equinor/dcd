export interface ITimeSeriesCost {
    id: string
    startYear: number
    name?: string
    values?: number[] | null
    sum?: number | undefined
    epaVersion: string
    currency: Components.Schemas.Currency
}
