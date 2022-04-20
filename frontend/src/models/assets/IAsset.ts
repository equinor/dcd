import { ITimeSeries } from "../ITimeSeries"

export interface IAsset {
    id?: string | undefined
    name?: string | undefined
    projectId?: string | undefined
    costProfile?: ITimeSeries | undefined
    drillingSchedule?: ITimeSeries | undefined
    co2Emissions?: ITimeSeries | undefined
    netSalesGas?: ITimeSeries | undefined
    fuelFlaringAndLosses?: ITimeSeries | undefined
    productionProfileGas?: ITimeSeries | undefined
    productionProfileOil?: ITimeSeries | undefined
    productionProfileWater?: ITimeSeries | undefined
    productionProfileWaterInjection?: ITimeSeries | undefined
    dryweight?: number | undefined
    maturity?: Components.Schemas.Maturity | undefined
}