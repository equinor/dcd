import { useDataFetch } from "@/Hooks"
import { useProjectContext } from "@/Store/ProjectContext"
import { useAppStore } from "@/Store/AppStore"

/**
 * Custom hook to determine if the current user can edit data.
 */
const useCanUserEdit = () => {
    const { isRevision } = useProjectContext()
    const revisionAndProjectData = useDataFetch()
    const { editMode } = useAppStore()

    /**
     * Indicates if editing is disabled due to missing user access.
     */
    const isEditDisabled = !revisionAndProjectData?.userActions.canEditProjectData

    /**
     * @returns {string} Returns a string explaining why editing is disabled.
     */
    const getEditDisabledText = () => {
        if (isRevision) {
            return "Project revisions are not editable"
        }
        if (isEditDisabled) {
            return "You do not have access to edit this project"
        }
        return ""
    }
    /**
     * @returns {boolean} Returns a boolean indicating if the user can edit the project data.
     */
    const canEdit = (): boolean => editMode && !isEditDisabled

    return { isEditDisabled, getEditDisabledText, canEdit }
}

export default useCanUserEdit
