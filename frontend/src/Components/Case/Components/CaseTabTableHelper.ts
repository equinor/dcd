import { Dispatch, SetStateAction } from "react"
import { ITimeSeries } from "../../../Models/ITimeSeries"

export const GetTimeSeriesLastYear = (
    timeSeries:
        | {
              id?: string;
              startYear?: number;
              name?: string;
              values?: number[] | null;
              sum?: number | undefined;
          }
        | undefined,
): number | undefined => {
    if (
        timeSeries
        && timeSeries.startYear !== undefined
        && timeSeries.values
        && timeSeries.values.length > 0
    ) {
        return timeSeries.startYear + timeSeries.values.length - 1
    }
    return undefined
}

export const SetTableYearsFromProfiles = (
    profiles: (
        | {
            id?: string;
            startYear?: number;
            name?: string;
            values?: number[] | null;
            sum?: number | undefined;
        }
        | undefined
    )[],
    dG4Year: number,
    setStartYear: Dispatch<SetStateAction<number>>,
    setEndYear: Dispatch<SetStateAction<number>>,
    setTableYears: Dispatch<SetStateAction<[number, number]>>,
) => {
    let firstYear: number | undefined
    let lastYear: number | undefined

    profiles.forEach((profile) => {
        if (profile?.startYear !== undefined) {
            const { startYear } = profile
            const profileStartYear: number = startYear + dG4Year
            if (firstYear === undefined) {
                firstYear = profileStartYear
            } else if (profileStartYear < firstYear) {
                firstYear = profileStartYear
            }
        }

        const profileLastYear = GetTimeSeriesLastYear(profile)
        if (profileLastYear !== undefined) {
            const adjustedProfileLastYear = profileLastYear + dG4Year

            if (lastYear === undefined) {
                lastYear = adjustedProfileLastYear
            } else if (adjustedProfileLastYear > lastYear) {
                lastYear = adjustedProfileLastYear
            }
        }
    })

    if (firstYear !== undefined) {
        setStartYear(firstYear)
    }
    if (lastYear !== undefined) {
        setEndYear(lastYear)
    }
    if (firstYear !== undefined && lastYear !== undefined) {
        setTableYears([firstYear, lastYear])
    }
}

export const SetSummaryTableYearsFromProfiles = (
    profiles: (ITimeSeries | undefined)[],
    dG4Year: number,
    setTableYears: Dispatch<SetStateAction<[number, number]>>,
) => {
    let firstYear: number | undefined
    let lastYear: number | undefined

    profiles.forEach((profile) => {
        if (profile?.startYear !== undefined) {
            const { startYear } = profile
            const profileStartYear: number = startYear + dG4Year
            if (firstYear === undefined) {
                firstYear = profileStartYear
            } else if (profileStartYear < firstYear) {
                firstYear = profileStartYear
            }
        }

        const profileLastYear = GetTimeSeriesLastYear(profile)
        if (profileLastYear !== undefined) {
            const adjustedProfileLastYear = profileLastYear + dG4Year

            if (lastYear === undefined) {
                lastYear = adjustedProfileLastYear
            } else if (adjustedProfileLastYear > lastYear) {
                lastYear = adjustedProfileLastYear
            }
        }
    })

    if (firstYear !== undefined && lastYear !== undefined) {
        setTableYears([firstYear, lastYear])
    }
}
