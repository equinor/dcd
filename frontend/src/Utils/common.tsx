import Cookies from 'universal-cookie'

const recentProjectCookieKeyPrefix = "projectid:"

export function GetDrainageStrategy(project: Components.Schemas.ProjectDto, drainageStrategyId?: string) {
  return project.drainageStrategies?.find(o => o.id === drainageStrategyId);
};
export function ProjectPhaseNumberToText(phaseNumber: Components.Schemas.ProjectPhase) {
    return "DG" + (phaseNumber+1).toString()
}
function recentProjectCookieKey(projectId: string) {
    return recentProjectCookieKeyPrefix+projectId
}
export function StoreRecentProject(projectId: string, cookies: Cookies) {
    const timeStamp = new Date().getTime()
    const cookieKey = recentProjectCookieKey(projectId)
    cookies.set(cookieKey, timeStamp.toString(), { path: '/' })
}
export function IsRecentProjectCookieKey(key: string) {
    return key.startsWith(recentProjectCookieKeyPrefix)
}
export function ExtractProjectIdFromCookieKey(key: string) {
    return key.substring(recentProjectCookieKeyPrefix.length)
}

export function ProjectPath(projectId: string) {
    return `/project/${projectId}`
}

export function CasePath(projectId: string, caseId: string) {
    return `${ProjectPath(projectId)}/case/${caseId}`
}
