export const LoginAccessTokenKey = "loginAccessToken"
export const FusionAccessTokenKey = "fusionAccessToken"

export const GetDrainageStrategy = (
    project: Components.Schemas.ProjectDto,
    drainageStrategyId?: string,
) => project.drainageStrategies?.find((o) => o.id === drainageStrategyId)

export function ProjectPath(projectId: string) {
    return `/project/${projectId}`
}

export function CasePath(projectId: string, caseId: string) {
    return `${ProjectPath(projectId)}/case/${caseId}`
}

export function SurfPath(projectId: string, caseId: string, surfId: string) {
    return `${CasePath(projectId, caseId)}/surf/${surfId}`
}

export function StoreToken(keyName: string, token: string) {
    window.sessionStorage.setItem(keyName, token)
}

export function GetToken(keyName: string) {
    return window.sessionStorage.getItem(keyName)
}
