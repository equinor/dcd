import { Case } from "../models/Case"

export const LoginAccessTokenKey = "loginAccessToken"
export const FusionAccessTokenKey = "fusionAccessToken"

export const GetDrainageStrategy = (
    project: Components.Schemas.ProjectDto,
    drainageStrategyId?: string,
) => project.drainageStrategies?.find((o) => o.id === drainageStrategyId)

export function ProjectPath(projectId: string) {
    return `/project/${projectId}`
}

export function CasePath(projectId: string, caseId?: string) {
    return `${ProjectPath(projectId)}/case/${caseId}`
}

export function StoreToken(keyName: string, token: string) {
    window.sessionStorage.setItem(keyName, token)
}

export function GetToken(keyName: string) {
    return window.sessionStorage.getItem(keyName)
}

export const unwrapCase = (casee?: Case | undefined): Case => {
    if (casee === undefined || casee === null) {
        throw new Error("Attempted to Import Timeseries onto a Case which does not exist")
    }
    return casee
}

export const unwrapProjectId = (projectId?: string | undefined): string => {
    if (projectId === undefined || projectId === null) {
        throw new Error("Attempted to Import Timeseries onto a Project which does not exist")
    }
    return projectId
}

export const unwrapCaseId = (caseId?: string | undefined): string => {
    if (caseId === undefined || caseId === null) {
        throw new Error("Attempted to Import Timeseries onto a Case Id which does not exist")
    }
    return caseId
}
