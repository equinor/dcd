import { useDataFetch } from "@/Hooks/useDataFetch"

const useEditDisabled = () => {
    const revisionAndProjectData = useDataFetch()

    const isRevision = revisionAndProjectData?.dataType === "revision"
    const isEditDisabled = !revisionAndProjectData?.userActions.canEditProjectData

    const getEditDisabledText = () => {
        if (isRevision) {
            return "Project revisions are not editable"
        }
        if (!isEditDisabled) {
            return "You do not have access to edit this project"
        }
        return ""
    }

    return { isEditDisabled, getEditDisabledText }
}

export default useEditDisabled
