import { Project } from "../models/Project"
import { RecentProject } from "../models/RecentProject"

const recentProjectsKey = "recentProjects"

export const LoginAccessTokenKey = "loginAccessToken"
export const FusionAccessTokenKey = "fusionAccessToken"

export const GetDrainageStrategy = (
    project: Components.Schemas.ProjectDto,
    drainageStrategyId?: string,
) => project.drainageStrategies?.find((o) => o.id === drainageStrategyId)

export function RetrieveRecentProjects() {
    const recentProjectJSON = localStorage.getItem(recentProjectsKey)
    const recentProjects: RecentProject[] = JSON.parse(recentProjectJSON ?? "[]")
    return recentProjects
}
export function StoreRecentProject(project: Project) {
    const recentProject = new RecentProject(project)
    let currentRecentProjects = RetrieveRecentProjects()
    // find possible duplicate, remove it
    const projectAlreadyNotedIndex = currentRecentProjects.findIndex(
        (recordedProject) => recordedProject.id === recentProject.id,
    )
    if (projectAlreadyNotedIndex >= 0) {
        currentRecentProjects = currentRecentProjects
            .slice(0, projectAlreadyNotedIndex)
            .concat(
                currentRecentProjects.slice(projectAlreadyNotedIndex + 1),
            )
    }
    currentRecentProjects.unshift(recentProject)
    const recentProjects = currentRecentProjects.slice(0, 4)
    localStorage.setItem(recentProjectsKey, JSON.stringify(recentProjects))
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
