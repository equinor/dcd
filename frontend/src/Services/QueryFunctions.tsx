import { GetCaseService } from "./CaseService"
import { GetFeatureToggleService } from "./FeatureToggleService"
import { GetProjectMembersService } from "./ProjectMembersService"
import { GetProjectService } from "./ProjectService"

export const caseQueryFn = async (projectId: string, caseId: string | undefined) => {
    if (projectId === "" || !caseId) {
        console.error("projectId or caseId is undefined")
        return null
    }
    return GetCaseService().getCaseWithAssets(projectId, caseId)
}

export const projectQueryFn = async (projectId: string | undefined) => {
    if (!projectId) {
        console.error("projectId is undefined")
        return null
    }

    return GetProjectService().getProject(projectId!)
}

export const peopleQueryFn = async (projectId: string | undefined) => {
    if (!projectId) {
        console.error("projectId is undefined")
        return null
    }
    return GetProjectMembersService().getPeople(projectId)
}

export const revisionQueryFn = async (revisionId: string | undefined) => {
    if (!revisionId) {
        console.error("revisionId is undefined")
        return null
    }
    return GetProjectService().getRevision(revisionId)
}

export const compareCasesQueryFn = async (projectId: string | undefined) => {
    if (!projectId) {
        console.error("projectId is undefined")
        return null
    }
    return GetProjectService().compareCases(projectId!)
}

export const featureToggleQueryFn = async () => GetFeatureToggleService().getFeatureToggles()
