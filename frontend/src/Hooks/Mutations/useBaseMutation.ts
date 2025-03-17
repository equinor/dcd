import { useMutation, useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router"
import { useProjectContext } from "@/Store/ProjectContext"
import { useAppStore } from "@/Store/AppStore"
import { createLogger } from "@/Utils/logger"

interface MutationParams {
  resourceId?: string;
  updatedValue: any;
  propertyKey: string;
}

interface BaseMutationOptions {
  resourceName: string;
  getService: () => any;
  updateMethod: string;
  getResourceFromApiData: (apiData: any) => any;
  loggerName: string;
}

export const useBaseMutation = ({
    resourceName,
    getService,
    updateMethod,
    getResourceFromApiData,
    loggerName,
}: BaseMutationOptions) => {
    const queryClient = useQueryClient()
    const { caseId } = useParams()
    const { projectId } = useProjectContext()
    const { setIsSaving, setSnackBarMessage } = useAppStore()

    const logger = createLogger({
        name: loggerName,
        enabled: false,
    })

    const mutation = useMutation({
        mutationFn: async ({
            resourceId,
            updatedValue,
            propertyKey,
        }: MutationParams) => {
            if (!projectId || !caseId) {
                throw new Error("Project ID and Case ID are required")
            }

            setIsSaving(true)
            logger.info(`Updating ${resourceName}:`, { resourceId, updatedValue, propertyKey })

            try {
                const service = getService()
                const apiData = await queryClient.getQueryData<any>(["caseApiData", projectId, caseId])
                const resource = getResourceFromApiData(apiData)

                if (!resource) {
                    throw new Error(`${resourceName} not found`)
                }

                const updatedResource = {
                    ...resource,
                    [propertyKey]: updatedValue,
                }

                let result

                if (resourceName === "case") {
                    result = await service[updateMethod](
                        projectId,
                        caseId,
                        updatedResource,
                    )
                } else {
                    result = await service[updateMethod](
                        projectId,
                        caseId,
                        resourceId || updatedResource.id,
                        resourceId ? updatedValue : updatedResource,
                    )
                }

                return result
            } finally {
                setIsSaving(false)
            }
        },
        onSuccess: () => {
            if (projectId && caseId) {
                queryClient.invalidateQueries({ queryKey: ["caseApiData", projectId, caseId] })
            }
        },
        onError: (error: any) => {
            setSnackBarMessage(error.message || `Failed to update ${resourceName}`)
            logger.error(`Error updating ${resourceName}:`, error)
        },
    })

    return mutation
}
