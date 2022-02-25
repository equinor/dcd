const recentProjectKeyPrefix = "projectid:"

export const GetDrainageStrategy = (project: Components.Schemas.ProjectDto, drainageStrategyId?: string) => {
    return project.drainageStrategies?.find(o => o.id === drainageStrategyId);
};

export function ProjectPhaseNumberToText(phaseNumber: Components.Schemas.ProjectPhase) {
    return "DG" + (phaseNumber+1).toString()
}
function recentProjectKey(projectId: string) {
    return recentProjectKeyPrefix+projectId
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
