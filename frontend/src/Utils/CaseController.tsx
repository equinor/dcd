import { GetCaseService } from "../Services/CaseService"
import { GetProjectService } from "../Services/ProjectService"
import { EMPTY_GUID } from "../Utils/constants"

export const deleteCase = async (
    caseId: string,
    projectId: string,
    addProjectEdit: (projectId: string, project: Components.Schemas.UpdateProjectDto) => void,
): Promise<boolean> => {
    try {
        const newProject = await (await GetCaseService()).deleteCase(projectId, caseId)
        addProjectEdit(projectId, newProject.commonProjectAndRevisionData)
        return true
    } catch (error) {
        console.error("[ProjectView] Error while deleting case", error)
        return false
    }
}

export const duplicateCase = async (
    caseId: string,
    projectId: string,
    addProjectEdit: (projectId: string, project: Components.Schemas.UpdateProjectDto) => void,
): Promise<boolean> => {
    try {
        const newProject = await (await GetCaseService()).duplicateCase(projectId, caseId)
        addProjectEdit(projectId, newProject.commonProjectAndRevisionData)
        return true
    } catch (error) {
        console.error("[ProjectView] error while submitting form data", error)
        return false
    }
}

export const setCaseAsReference = async (
    caseId: string | undefined,
    project: Components.Schemas.ProjectDataDto | Components.Schemas.RevisionDataDto,
    addProjectEdit: (projectId: string, project: Components.Schemas.UpdateProjectDto) => void,
) => {
    try {
        const projectDto: Components.Schemas.UpdateProjectDto = { ...project.commonProjectAndRevisionData }
        if (projectDto.referenceCaseId === caseId) {
            projectDto.referenceCaseId = EMPTY_GUID
        } else {
            projectDto.referenceCaseId = caseId ?? ""
        }
        const newProject = await (await GetProjectService()).updateProject(project.projectId, projectDto)
        addProjectEdit(project.projectId, newProject.commonProjectAndRevisionData)
    } catch (error) {
        console.error("[ProjectView] error while submitting form data", error)
    }
}
