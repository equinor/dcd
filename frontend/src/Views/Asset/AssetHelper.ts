import { Dispatch, SetStateAction } from "react"
import { ITimeSeries } from "../../models/ITimeSeries"

interface IAsset {
    id?: string | undefined
    name?: string | undefined
    projectId?: string | undefined
    costProfile?: ITimeSeries | undefined
    drillingSchedule?: ITimeSeries | undefined
    gAndGAdminCost?: ITimeSeries | undefined
    co2Emissions?: ITimeSeries | undefined
    netSalesGas?: ITimeSeries | undefined
    fuelFlaringAndLosses?: ITimeSeries | undefined
    productionProfileGas?: ITimeSeries | undefined
    productionProfileOil?: ITimeSeries | undefined
    productionProfileWater?: ITimeSeries | undefined
    productionProfileWaterInjection?: ITimeSeries | undefined
    substructureCessationCostProfileDto?: ITimeSeries | undefined
    surfCessationCostProfileDto?: ITimeSeries | undefined
    topsideCessationCostProfileDto?: ITimeSeries | undefined
    transportCessationCostProfileDto?: ITimeSeries | undefined
    dryweight?: number | undefined
    maturity?: Components.Schemas.Maturity | undefined}

const SetYears = (
    years: [number, number],
    timeSeries: ITimeSeries,
    dG4Year: number,
    setFirstYear: Dispatch<SetStateAction<number | undefined>>,
    setLastYear: Dispatch<SetStateAction<number | undefined>>,
): [number, number] => {
    const newYears = years
    if (timeSeries.startYear !== undefined && timeSeries.values !== undefined) {
        if (timeSeries.startYear + dG4Year < years[0]) {
            setFirstYear(timeSeries.startYear + dG4Year)
            newYears[0] = timeSeries.startYear + dG4Year
        }

        if (timeSeries.startYear + dG4Year + timeSeries.values.length > years[1]) {
            setLastYear(timeSeries.startYear + dG4Year + timeSeries.values.length)
            newYears[1] = timeSeries.startYear + dG4Year + timeSeries.values.length
        }
    }

    return newYears
}

export const TimeSeriesYears = (
    asset: IAsset,
    dG4Year: number,
    setFirstYear: Dispatch<SetStateAction<number | undefined>>,
    setLastYear: Dispatch<SetStateAction<number | undefined>>,
) => {
    let years: [number, number]
    years = [Number.MAX_SAFE_INTEGER, Number.MIN_SAFE_INTEGER]
    if (asset.co2Emissions !== undefined) {
        years = SetYears(years, asset.co2Emissions, dG4Year, setFirstYear, setLastYear)
    }
    if (asset.costProfile !== undefined) {
        years = SetYears(years, asset.costProfile, dG4Year, setFirstYear, setLastYear)
    }
    if (asset.drillingSchedule !== undefined) {
        years = SetYears(years, asset.drillingSchedule, dG4Year, setFirstYear, setLastYear)
    }
    if (asset.fuelFlaringAndLosses !== undefined) {
        years = SetYears(years, asset.fuelFlaringAndLosses, dG4Year, setFirstYear, setLastYear)
    }
    if (asset.netSalesGas !== undefined) {
        years = SetYears(years, asset.netSalesGas, dG4Year, setFirstYear, setLastYear)
    }
    if (asset.productionProfileGas !== undefined) {
        years = SetYears(years, asset.productionProfileGas, dG4Year, setFirstYear, setLastYear)
    }
    if (asset.productionProfileOil !== undefined) {
        years = SetYears(years, asset.productionProfileOil, dG4Year, setFirstYear, setLastYear)
    }
    if (asset.productionProfileWater !== undefined) {
        years = SetYears(years, asset.productionProfileWater, dG4Year, setFirstYear, setLastYear)
    }
    if (asset.productionProfileWaterInjection !== undefined) {
        years = SetYears(years, asset.productionProfileWaterInjection, dG4Year, setFirstYear, setLastYear)
    }
    if (asset.substructureCessationCostProfileDto !== undefined) {
        years = SetYears(years, asset.substructureCessationCostProfileDto, dG4Year, setFirstYear, setLastYear)
    }
    if (asset.surfCessationCostProfileDto !== undefined) {
        years = SetYears(years, asset.surfCessationCostProfileDto, dG4Year, setFirstYear, setLastYear)
    }
    if (asset.topsideCessationCostProfileDto !== undefined) {
        years = SetYears(years, asset.topsideCessationCostProfileDto, dG4Year, setFirstYear, setLastYear)
    }
    if (asset.transportCessationCostProfileDto !== undefined) {
        years = SetYears(years, asset.transportCessationCostProfileDto, dG4Year, setFirstYear, setLastYear)
    }
    return years
}

export const GetArtificialLiftName = (value: number | undefined): string => {
    switch (value) {
    case 0:
        return "No lift"
    case 1:
        return "Gas lift"
    case 2:
        return "Electrical submerged pumps"
    case 3:
        return "Subsea booster pumps"
    default:
        return ""
    }
}

const setYears = (
    years: [number, number],
    timeSeries: ITimeSeries,
    dG4Year: number,
    setFirstYear: Dispatch<SetStateAction<number | undefined>>,
    setLastYear: Dispatch<SetStateAction<number | undefined>>,
): [number, number] => {
    const newYears = years
    if (timeSeries.startYear !== undefined && timeSeries.values !== undefined) {
        if (timeSeries.startYear + dG4Year < years[0]) {
            setFirstYear(timeSeries.startYear + dG4Year)
            newYears[0] = timeSeries.startYear + dG4Year
        }

        if (timeSeries.startYear + dG4Year + timeSeries.values.length > years[1]) {
            setLastYear(timeSeries.startYear + dG4Year + timeSeries.values.length)
            newYears[1] = timeSeries.startYear + dG4Year + timeSeries.values.length
        }
    }

    return newYears
}

export const initializeFirstAndLastYear = (
    dG4: number,
    timeSeries: (ITimeSeries | undefined)[],
    setFirstYear: Dispatch<SetStateAction<number | undefined>>,
    setLastYear: Dispatch<SetStateAction<number | undefined>>,
) => {
    let years: [number, number] = [Number.MAX_SAFE_INTEGER, Number.MIN_SAFE_INTEGER]
    timeSeries.forEach((ts) => {
        if (ts) {
            years = setYears(years, ts, dG4, setFirstYear, setLastYear)
        }
    })
}
