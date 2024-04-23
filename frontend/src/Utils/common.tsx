import _ from "lodash"
import { ITimeSeries } from "../Models/ITimeSeries"

export const loginAccessTokenKey = "loginAccessToken"
export const FusionAccessTokenKey = "fusionAccessToken"

export const getDrainageStrategy = (
    project: Components.Schemas.ProjectDto,
    drainageStrategyId?: string,
) => project.drainageStrategies?.find((o) => o.id === drainageStrategyId)

export const projectPath = (projectId: string) => `/${projectId}`

export const casePath = (projectId: string, caseId: string) => `${projectPath(projectId)}/case/${caseId}`

export const storeToken = (keyName: string, token: string) => {
    window.sessionStorage.setItem(keyName, token)
}

export const storeAppId = (appId: string) => {
    window.sessionStorage.setItem("appId", appId)
}

export const storeAppScope = (appScope: string) => {
    window.sessionStorage.setItem("appScope", appScope)
}

export const getToken = (keyName: string) => {
    const scopes = [[window.sessionStorage.getItem("appScope") || ""][0]]
    return window.Fusion.modules.auth.acquireAccessToken({ scopes })
}

export const unwrapCase = (_case?: Components.Schemas.CaseDto | undefined): Components.Schemas.CaseDto => {
    if (_case === undefined || _case === null) {
        throw new Error("Attempted to Create a case from which has not been created")
    }
    return _case
}

export const unwrapProjectId = (projectId?: string | undefined | null): string => {
    if (projectId === undefined || projectId === null) {
        throw new Error("Attempted to use a Project ID which does not exist")
    }
    return projectId
}

export const unwrapCaseId = (caseId?: string | undefined): string => {
    if (caseId === undefined || caseId === null) {
        throw new Error("Attempted to use a Case ID which does not exist")
    }
    return caseId
}

export const getProjectCategoryName = (key?: Components.Schemas.ProjectCategory): string => {
    if (key === undefined) {
        return ""
    }
    return {
        0: "Unknown",
        1: "Brownfield",
        2: "Cessation",
        3: "Drilling upgrade",
        4: "Onshore",
        5: "Pipeline",
        6: "Platform FPSO",
        7: "Subsea",
        8: "Solar",
        9: "CO2 storage",
        10: "Efuel",
        11: "Nuclear",
        12: "CO2 Capture",
        13: "FPSO",
        14: "Hydrogen",
        15: "Hse",
        16: "Offshore wind",
        17: "Platform",
        18: "Power from shore",
        19: "Tie-in",
        20: "Renewable other",
        21: "CCS",
    }[key]
}

export const getProjectPhaseName = (key?: Components.Schemas.ProjectPhase): string => {
    if (key === undefined) {
        return ""
    }
    return {
        0: "Unknown",
        1: "Bid preparations",
        2: "Business identification",
        3: "Business planning",
        4: "Concept planning",
        5: "Concessions / Negotiations",
        6: "Defintion",
        7: "Execution",
        8: "Operation",
        9: "Screening business opportunities",
    }[key]
}

export const toMonthDate = (date?: Date | null): string | undefined => {
    if (Number.isNaN(date?.getTime())) {
        return undefined
    }

    return date?.toISOString().substring(0, 7)
}

export const isDefaultDate = (date?: Date | null): boolean => {
    if (date && (toMonthDate(date) === "0001-01" || date.toLocaleDateString("en-CA") === "1-01-01")) {
        return true
    }
    return false
}

export const isDefaultDateString = (dateString?: string | null): boolean => {
    const date = new Date(dateString ?? "")
    if (date && (toMonthDate(date) === "0001-01" || date.toLocaleDateString("en-CA") === "1-01-01")) {
        return true
    }
    return false
}

export const dateFromString = (dateString?: string | null): Date => new Date(dateString ?? "")

export const defaultDate = () => new Date("0001-01-01")

export const isInteger = (value: string) => /^-?\d+$/.test(value)

export const productionStrategyOverviewToString = (value?: Components.Schemas.ProductionStrategyOverview): string => {
    if (value === undefined) { return "" }
    return {
        0: "Depletion",
        1: "Water injection",
        2: "Gas injection",
        3: "WAG",
        4: "Mixed",
    }[value]
}

export const isExplorationWell = (well: Components.Schemas.WellDto | undefined) => [4, 5, 6].indexOf(well?.wellCategory ?? -1) > -1

const MergeCostProfileData = (arrays: number[][], offsets: number[]): number[] => {
    const maxLength = Math.max(...arrays.map((arr, idx) => arr.length + offsets[idx]))
    const result = new Array(maxLength).fill(0)

    arrays.forEach((arr, idx) => {
        const offset = offsets[idx]
        for (let i = 0; i < arr.length; i += 1) {
            if (i + offset < maxLength) {
                result[i + offset] += arr[i]
            }
        }
    })

    return result
}

const removeLeadingZeroes = (array: number[]): number[] => {
    let index = 0
    while (index < array.length && array[index] === 0) {
        index += 1
    }
    return array.slice(index)
}

export const MergeTimeseries = (t1: ITimeSeries | undefined, t2: ITimeSeries | undefined): ITimeSeries => {
    // Check if either time series is undefined and return the other one directly
    if (!t1) return t2 || { id: "", startYear: 0, values: [] }
    if (!t2) return t1

    const arrays = [t1.values ?? [], t2.values ?? []]

    // Merge the time series data
    const mergedValues = MergeCostProfileData(arrays, [t1.startYear ?? 0, t2.startYear ?? 0])

    // Find the first year that holds any value > 0
    const minYear = Math.min(t1.startYear ?? 3000, t2.startYear ?? 3000)

    // Remove leading zeros from the merged array
    const cleanedValues = removeLeadingZeroes(mergedValues)

    const timeSeries: ITimeSeries = {
        id: t1.id || t2.id || "",
        startYear: minYear,
        values: cleanedValues,
    }

    return timeSeries
}

export const MergeTimeseriesList = (timeSeriesList: (ITimeSeries | undefined)[]): ITimeSeries => {
    let mergedTimeSeries: ITimeSeries = { id: "", startYear: 3000, values: [] }

    // Iterate through the list and merge consecutively
    timeSeriesList.forEach((currentSeries, index) => {
        if (index === 0) {
            mergedTimeSeries = currentSeries ?? mergedTimeSeries
        } else {
            mergedTimeSeries = MergeTimeseries(mergedTimeSeries, currentSeries)
        }
    })

    return mergedTimeSeries
}

export function formatDate(isoDateString: string): string {
    if (isoDateString === "0001-01-01T00:00:00+00:00" || isoDateString === "0001-01-01T00:00:00.000Z") {
        return "_"
    }
    const date = new Date(isoDateString)
    const options: Intl.DateTimeFormatOptions = {
        month: "long",
        year: "numeric",
    }
    return new Intl.DateTimeFormat("no-NO", options).format(date)
}

export const isWithinRange = (number: number, max: number, min: number) => number >= max && number <= min

export const preventNonDigitInput = (e: React.KeyboardEvent<HTMLInputElement>): void => {
    if (!/\d/.test(e.key)) e.preventDefault()
}
