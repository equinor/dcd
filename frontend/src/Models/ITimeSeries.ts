import { Dispatch, SetStateAction } from "react"
import { ProfileNames } from "./Interfaces"

export interface ITimeSeries {
    id: string
    startYear: number
    name?: string
    values?: number[] | null
    sum?: number | undefined
}

export interface ITimeSeriesOverride extends ITimeSeries {
    override: boolean
}

export interface ITimeSeriesCost extends ITimeSeries {
    currency: Components.Schemas.Currency
}

export interface ITimeSeriesCostOverride extends ITimeSeriesCost {
    override: boolean
}

export interface ITimeSeriesData {
    profileName: string
    unit: string,
    set?: Dispatch<SetStateAction<ITimeSeriesCost | undefined>>,
    overrideProfileSet?: Dispatch<SetStateAction<ITimeSeriesCostOverride | undefined>>,
    profile: ITimeSeries | undefined
    overrideProfile?: ITimeSeries | undefined
    overridable?: boolean
}

export interface ITimeSeriesDataWithGroup extends ITimeSeriesData {
    group?: string
}

export interface ITimeSeriesTableData {
    profileName: string
    unit: string,
    profile: ITimeSeries | undefined
    overrideProfile?: ITimeSeriesOverride | undefined
    overridable: boolean
    resourceId: string
    resourcePropertyKey: string
    resourceName: ProfileNames
    resourceProfileId?: string
    editable: boolean
    total?: string
    hideIfEmpty?: boolean
}

export interface ITimeSeriesTableDataWithSet extends ITimeSeriesTableData {
    set?: Dispatch<SetStateAction<ITimeSeriesCost | undefined>>,
    overrideProfileSet?: Dispatch<SetStateAction<ITimeSeriesCostOverride | undefined>>,
    group?: string
}

export interface ITimeSeriesTableDataOverrideWithSet extends ITimeSeriesTableData {
    set?: Dispatch<SetStateAction<ITimeSeriesCost | undefined>>,
    overrideProfileSet?: Dispatch<SetStateAction<ITimeSeriesCostOverride | undefined>>,
    override: boolean
    group?: string
}
