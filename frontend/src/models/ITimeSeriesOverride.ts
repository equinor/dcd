export interface ITimeSeriesOverride {
    id: string
    startYear: number
    name?: string
    values?: number[] | null
    sum?: number | undefined
    override: boolean
}
