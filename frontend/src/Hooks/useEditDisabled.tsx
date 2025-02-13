import { useDataFetch } from "@/Hooks"
import { useProjectContext } from "@/Store/ProjectContext"

const useEditDisabled = () => {
    const { isRevision } = useProjectContext()
    const revisionAndProjectData = useDataFetch()

    const isEditDisabled = !revisionAndProjectData?.userActions.canEditProjectData

    const getEditDisabledText = () => {
        if (isRevision) {
            return "Project revisions are not editable"
        }
        if (isEditDisabled) {
            return "You do not have access to edit this project"
        }
        return ""
    }

    return { isEditDisabled, getEditDisabledText }
}

export default useEditDisabled
