import { useDataFetch } from "./useDataFetch"

const useEditDisabled = () => {
    const revisionAndProjectData = useDataFetch()

    const isEditDisabled = !revisionAndProjectData?.actions.canCreateRevision

    const getEditDisabledText = () => {
        if (revisionAndProjectData?.dataType === "revision") {
            return "Project revisions are not editable"
        }
        if (!revisionAndProjectData?.actions.canEditProjectData) {
            return "You do not have access to edit this project"
        }
        return ""
    }

    return { isEditDisabled, getEditDisabledText }
}

export default useEditDisabled
