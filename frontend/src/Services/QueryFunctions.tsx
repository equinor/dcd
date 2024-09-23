import { GetCaseService } from "./CaseService"
import { GetProjectService } from "./ProjectService"

export const caseQueryFn = async (projectId: string | undefined, caseId: string | undefined) => {
    if (!projectId || !caseId) {
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
