import { Dispatch, SetStateAction } from "react"
import { ProfileTypes } from "./enums"

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

export interface ITimeSeriesData {
    profileName: string
    unit: string,
    set?: Dispatch<SetStateAction<ITimeSeries | undefined>>,
    overrideProfileSet?: Dispatch<SetStateAction<ITimeSeriesOverride | undefined>>,
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
    resourceName: ProfileTypes
    editable: boolean
    total?: string
    hideIfEmpty?: boolean
}

export interface ITimeSeriesTableDataWithSet extends ITimeSeriesTableData {
    set?: Dispatch<SetStateAction<ITimeSeries | undefined>>,
    overrideProfileSet?: Dispatch<SetStateAction<ITimeSeriesOverride | undefined>>,
    group?: string
}

export interface ITimeSeriesTableDataOverrideWithSet extends ITimeSeriesTableData {
    set?: Dispatch<SetStateAction<ITimeSeries | undefined>>,
    overrideProfileSet?: Dispatch<SetStateAction<ITimeSeriesOverride | undefined>>,
    override: boolean
    group?: string
}
