import { GetCaseService } from "../Services/CaseService"
import { GetProjectService } from "../Services/ProjectService"
import { EMPTY_GUID } from "../Utils/constants"

export const deleteCase = async (
    caseId: string,
    project: Components.Schemas.ProjectDto,
    setProject: (project: Components.Schemas.ProjectDto) => void,
): Promise<boolean> => {
    try {
        const newProject = await (await GetCaseService()).deleteCase(project.id, caseId)
        setProject(newProject)
        return true
    } catch (error) {
        console.error("[ProjectView] Error while deleting case", error)
        return false
    }
}

export const duplicateCase = async (
    caseId: string,
    project: Components.Schemas.ProjectDto,
    setProject: (project: Components.Schemas.ProjectDto) => void,
): Promise<boolean> => {
    try {
        const newProject = await (await GetCaseService()).duplicateCase(project.id, caseId)
        setProject(newProject)
        return true
    } catch (error) {
        console.error("[ProjectView] error while submitting form data", error)
        return false
    }
}

export const setCaseAsReference = async (
    caseId: string | undefined,
    project: Components.Schemas.ProjectDto,
    setProject: (project: Components.Schemas.ProjectDto) => void,
) => {
    try {
        const projectDto = { ...project }
        if (projectDto.referenceCaseId === caseId) {
            projectDto.referenceCaseId = EMPTY_GUID
        } else {
            projectDto.referenceCaseId = caseId ?? ""
        }
        const newProject = await (await GetProjectService()).updateProject(project.id, projectDto)
        setProject(newProject)
    } catch (error) {
        console.error("[ProjectView] error while submitting form data", error)
    }
}
