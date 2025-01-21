export interface TableWell {
    id: string,
    name: string,
    wellCategory: Components.Schemas.WellCategory,
    drillingDays: number,
    wellCost: number,
    well: Components.Schemas.WellOverviewDto
    wells: Components.Schemas.WellOverviewDto[]
}
