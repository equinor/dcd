import { Dispatch, SetStateAction } from "react"
import { ITimeSeries } from "../../models/ITimeSeries"

export const GetTimeSeriesLastYear = (timeSeries: ITimeSeries | undefined): number | undefined => {
    if (timeSeries && timeSeries.startYear !== undefined && timeSeries.values && timeSeries.values.length > 0) {
        return timeSeries.startYear + timeSeries.values.length - 1
    } return undefined
}

export const SetTableYearsFromProfiles = (
    profiles: (ITimeSeries | undefined)[],
    dG4Year: number,
    setStartYear: Dispatch<SetStateAction<number>>,
    setEndYear: Dispatch<SetStateAction<number>>,
    setTableYears: Dispatch<SetStateAction<[number, number]>>,
) => {
    let firstYear = Number.MAX_SAFE_INTEGER
    let lastYear = Number.MIN_SAFE_INTEGER
    profiles.forEach((p) => {
        if (p && p.startYear !== undefined && p.startYear < firstYear) {
            firstYear = p.startYear
        }
        const profileLastYear = GetTimeSeriesLastYear(p)
        if (profileLastYear !== undefined && profileLastYear > lastYear) {
            lastYear = profileLastYear
        }
    })
    if (firstYear < Number.MAX_SAFE_INTEGER && lastYear > Number.MIN_SAFE_INTEGER) {
        setStartYear(firstYear + dG4Year)
        setEndYear(lastYear + dG4Year)
        setTableYears([firstYear + dG4Year, lastYear + dG4Year])
    }
}
