import { Dispatch, SetStateAction } from "react"
import { ITimeSeries } from "../../models/ITimeSeries"

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
): [number, number] => {
    const newYears = years
    if (timeSeries.startYear !== undefined && timeSeries.values !== undefined) {
        if (Number(timeSeries.startYear) + dG4Year < years[0]) {
            newYears[0] = Number(timeSeries.startYear) + Number(dG4Year)
        }

        if (Number(timeSeries.startYear) + dG4Year + timeSeries.values.length > years[1]) {
            newYears[1] = Number(timeSeries.startYear) + Number(dG4Year) + Number(timeSeries.values.length)
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
            years = setYears(years, ts, dG4)
        }
    })

    setFirstYear(years[0])
    setLastYear(years[1])
}
