import { useProjectContext } from "@/Context/ProjectContext"

const useEditDisabled = () => {
    const { isRevision, accessRights } = useProjectContext()

    const isEditDisabled = isRevision || !accessRights?.canEdit

    const getEditDisabledText = () => {
        if (isRevision) {
            return "Project revisions are not editable"
        }
        if (!accessRights?.canEdit) {
            return "You do not have edit access to this project"
        }
        return ""
    }

    return { isEditDisabled, getEditDisabledText }
}

export default useEditDisabled
