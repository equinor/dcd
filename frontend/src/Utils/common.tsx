import _ from "lodash"
import { ITimeSeries } from "../Models/ITimeSeries"
export const loginAccessTokenKey = "loginAccessToken"
export const FusionAccessTokenKey = "fusionAccessToken"

export const getDrainageStrategy = (
    project: Components.Schemas.ProjectDto,
    drainageStrategyId?: string,
) => project.drainageStrategies?.find((o) => o.id === drainageStrategyId)

export const projectPath = (projectId: string) => {
    return `/${projectId}`;
}

export const casePath = (projectId: string, caseId: string) => {
    return `${projectPath(projectId)}/case/${caseId}`
}

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
    let maxLength = Math.max(...arrays.map(arr => arr.length + offsets[arrays.indexOf(arr)]));
    let result = new Array(maxLength).fill(0);

    arrays.forEach((arr, idx) => {
        let offset = offsets[idx];
        for (let i = 0; i < arr.length; i++) {
            if (i + offset < maxLength) {
                result[i + offset] += arr[i];
            }
        }
    });

    return result;
}

export const MergeTimeseries = (t1: ITimeSeries | undefined, t2: ITimeSeries | undefined, t3: ITimeSeries | undefined): ITimeSeries => {
    const startYears = [t1, t2, t3].map(t => t?.startYear ?? 0);
    const minYear = Math.min(...startYears);
    const arrays = [t1, t2, t3].map(t => t?.values ?? []);
    const offsets = startYears.map(year => Math.abs(year - minYear));

    let values: number[] = MergeCostProfileData(arrays, offsets);

    const timeSeries = {
        id: t1?.id ?? t2?.id ?? t3?.id ?? "",
        startYear: minYear,
        values,
    }
    return timeSeries;
}

