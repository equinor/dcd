const recentProjectKeyPrefix = "projectid:"

export const LoginAccessTokenKey = "loginAccessToken"
export const FusionAccessTokenKey = "fusionAccessToken"

export const GetDrainageStrategy = (
    project: Components.Schemas.ProjectDto,
    drainageStrategyId?: string,
) => project.drainageStrategies?.find((o) => o.id === drainageStrategyId)

export function ProjectPhaseNumberToText(phaseNumber: Components.Schemas.ProjectPhase) {
    return `DG${(phaseNumber + 1).toString()}`
}
function recentProjectKey(projectId: string) {
    return recentProjectKeyPrefix + projectId
}
export function StoreRecentProject(projectId: string) {
    const timeStamp = new Date().getTime()
    const key = recentProjectKey(projectId)
    localStorage.setItem(key, timeStamp.toString())
}
export function RetrieveLastVisitForProject(projectId: string) {
    const timeStamp = localStorage.getItem(recentProjectKey(projectId))
    return timeStamp
}

export function ProjectPath(projectId: string) {
    return `/project/${projectId}`
}

export function CasePath(projectId: string, caseId: string) {
    return `${ProjectPath(projectId)}/case/${caseId}`
}

export function StoreToken(keyName: string, token: string) {
    window.sessionStorage.setItem(keyName, token)
}

export function GetToken(keyName: string) {
    return window.sessionStorage.getItem(keyName)
}
