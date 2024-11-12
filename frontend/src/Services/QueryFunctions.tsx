import { GetCaseService } from "./CaseService"
import { GetFeatureToggleService } from "./FeatureToggleService"
import { GetProjectService } from "./ProjectService"

export const caseQueryFn = async (projectId: string, caseId: string | undefined) => {
    if (projectId === "" || !caseId) {
        console.error("projectId or caseId is undefined")
        return null
    }
    const caseService = await GetCaseService()
    return caseService.getCaseWithAssets(projectId, caseId)
}

export const projectQueryFn = async (projectId: string | undefined) => {
    if (!projectId) {
        console.error("projectId is undefined")
        return null
    }
    const projectService = await GetProjectService()
    return projectService.getProject(projectId!)
}

export const revisionQueryFn = async (projectId: string | undefined, revisionId: string | undefined) => {
    if (!revisionId || !projectId) {
        console.error("projectId or revisionId is undefined")
        return null
    }
    const projectService = await GetProjectService()
    return projectService.getRevision(projectId, revisionId)
}

export const compareCasesQueryFn = async (projectId: string | undefined) => {
    if (!projectId) {
        console.error("projectId is undefined")
        return null
    }
    const projectService = await GetProjectService()
    return projectService.compareCases(projectId!)
}

export const featureToggleQueryFn = async () => {
    const featureToggleService = await GetFeatureToggleService()

    return featureToggleService.getFeatureToggles()
}
