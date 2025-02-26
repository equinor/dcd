import { useParams } from "react-router"
import { useAppStore } from "../Store/AppStore"
import { EditInstance } from "../Models/Interfaces"
import { useSubmitToApi } from "./UseSubmitToApi"
import { createLogger } from "../Utils/logger"

const editLogger = createLogger({
    name: "EDIT_CASE",
    enabled: false,
})

const useEditCase = () => {
    const { setIsSaving } = useAppStore()
    const { submitToApi } = useSubmitToApi()
    const { caseId: caseIdFromParams } = useParams()

    const handleApiSubmission = async (editInstance: EditInstance) => {
        const {
            projectId,
            caseId,
            resourceName,
            resourceId,
            resourceObject,
        } = editInstance

        if (!caseId) {
            throw new Error("Case ID is required")
        }

        setIsSaving(true)

        editLogger.info("projectId:", projectId)
        editLogger.info("caseId:", caseId)
        editLogger.info("resourceName:", resourceName)
        editLogger.info("resourceId:", resourceId)
        editLogger.info("resourceObject:", resourceObject)

        try {
            const result = await submitToApi({
                projectId,
                caseId,
                resourceName,
                resourceId,
                resourceObject,
            })

            if (result.success) {
                const editWithProfileId = { ...editInstance }
                return editWithProfileId
            }
            return null
        } finally {
            setIsSaving(false)
        }
    }

    const addEdit = async (editInstance: EditInstance) => {
        if (!editInstance.caseId && !caseIdFromParams) {
            throw new Error("Case ID is required")
        }

        const effectiveCaseId = editInstance.caseId || caseIdFromParams

        const finalEditInstance: EditInstance = {
            ...editInstance,
            caseId: effectiveCaseId,
        }

        editLogger.log("Adding edit:", finalEditInstance, {})
        return handleApiSubmission(finalEditInstance)
    }

    return { addEdit }
}

export default useEditCase
