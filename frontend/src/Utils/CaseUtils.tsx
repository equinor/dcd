import { GetCaseService } from "../Services/CaseService"
import { GetProjectService } from "../Services/ProjectService"

export const deleteCase = async (
    caseId: string,
    projectId: string,
    addProjectEdit: (projectId: string, project: Components.Schemas.UpdateProjectDto) => void,
): Promise<boolean> => {
    try {
        const newProject = await GetCaseService().deleteCase(projectId, caseId)
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
        const newProject = await GetCaseService().duplicateCase(projectId, caseId)
        addProjectEdit(projectId, newProject.commonProjectAndRevisionData)
        return true
    } catch (error) {
        console.error("[ProjectView] error while submitting form data", error)
        return false
    }
}

export const setCaseAsReference = async (
    caseId: string | undefined,
    project: Components.Schemas.ProjectDataDto,
    addProjectEdit: (projectId: string, project: Components.Schemas.UpdateProjectDto) => void,
) => {
    try {
        const projectDto: Components.Schemas.UpdateProjectDto = { ...project.commonProjectAndRevisionData }

        projectDto.referenceCaseId = projectDto.referenceCaseId === caseId ? null : (caseId || null)

        const newProject = await GetProjectService().updateProject(project.projectId, projectDto)
        addProjectEdit(project.projectId, newProject.commonProjectAndRevisionData)
    } catch (error) {
        console.error("[ProjectView] error while submitting form data", error)
    }
}
