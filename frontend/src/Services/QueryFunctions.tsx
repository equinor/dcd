import { GetCaseService } from "./CaseService"

export const caseQueryFn = async (projectId: string | undefined, caseId: string | undefined) => {
    if (!projectId || !caseId) {
        console.error("projectId or caseId is undefined")
        return null
    }
    const caseService = await GetCaseService()
    return caseService.getCaseWithAssets(projectId, caseId)
}
