import _ from "lodash"
import { ITimeSeries } from "../models/ITimeSeries"

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

const zip = (t1: number[], t2: number[]) => t1.map((t1Value, index) => t1Value + (t2[index] ?? 0))

const mergeCostProfileData = (t1: number[], t2: number[], offset: number): number[] => {
    let doubleList: number[] = []
    if (offset > t1.length) {
        doubleList = doubleList.concat(t1)
        const zeros = offset - t1.length

        const zeroList = new Array(zeros).fill(0)

        doubleList = doubleList.concat(zeroList)
        doubleList = doubleList.concat(t2)
        return doubleList
    }
    doubleList = doubleList.concat(t1.slice(0, offset))

    if (t1.length - offset === t2.length) {
        doubleList = doubleList.concat(zip(_.takeRight(t1, (t1.length - offset)), (t2)))
    } else if (t1.length - offset > t2.length) {
        doubleList = doubleList.concat(zip(_.takeRight(t1, (t1.length - offset)), t2))
    } else {
        doubleList = doubleList.concat(zip(_.takeRight(t1, (t1.length - offset)), t2))
        const remaining = t2.length - (t1.length - offset)
        doubleList = doubleList.concat(_.takeRight(t2, remaining))
    }
    return doubleList
}

export const mergeTimeseries = (t1: ITimeSeries | undefined, t2: ITimeSeries | undefined): ITimeSeries => {
    const t1Year = t1?.startYear ?? 0
    const t2Year = t2?.startYear ?? 0
    const t1Values = t1?.values
    const t2Values = t2?.values

    if (!t1Values || t1Values.length === 0) {
        if (!t2Values || t2Values.length === 0) {
            return {
                id: "",
                startYear: 0,
                values: [],
            }
        }
        return t2
    }
    if (!t2Values || t2Values.length === 0) {
        return t1
    }

    const offset = t1Year < t2Year ? t2Year - t1Year : t1Year - t2Year

    let values: number[] = []
    if (t1Year < t2Year) {
        values = mergeCostProfileData(t1Values, t2Values, offset)
    } else {
        values = mergeCostProfileData(t2Values, t1Values, offset)
    }

    const timeSeries = {
        id: t1?.id ?? t2?.id ?? "",
        startYear: Math.min(t1Year, t2Year),
        values,
    }
    return timeSeries
}
