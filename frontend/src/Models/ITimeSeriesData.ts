import { Dispatch, SetStateAction } from "react"
import { ITimeSeries } from "./ITimeSeries"
import { ITimeSeriesCost } from "./ITimeSeriesCost"
import { ITimeSeriesCostOverride } from "./ITimeSeriesCostOverride"

export interface ITimeSeriesData {
        profileName: string
        unit: string,
        set?: Dispatch<SetStateAction<ITimeSeriesCost | undefined>>,
        overrideProfileSet?: Dispatch<SetStateAction<ITimeSeriesCostOverride | undefined>>,
        profile: ITimeSeries | undefined
        overrideProfile?: ITimeSeries | undefined
        overridable?: boolean
    }
