import { useMutation, useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router"
import { useProjectContext } from "@/Store/ProjectContext"
import { useAppStore } from "@/Store/AppStore"
import { createLogger } from "@/Utils/logger"

interface MutationParams {
  resourceId?: string;
  updatedValue: any;
  propertyKey: string;
  localCaseId?: string;
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
            localCaseId: paramLocalCaseId,
        }: MutationParams) => {
            if (!projectId || (!caseId && !paramLocalCaseId)) {
                throw new Error("Project ID and Case ID are required")
            }

            setIsSaving(true)
            logger.info(`Updating ${resourceName}:`, { resourceId, updatedValue, propertyKey })

            try {
                const service = getService()
                const finalCaseId = paramLocalCaseId || caseId

                let apiData = await queryClient.getQueryData<any>(["caseApiData", projectId, finalCaseId])
                // If the data is not in the cache and we're dealing with a case resource, fetch it directly
                if (!apiData && resourceName === "case" && finalCaseId) {
                    try {
                        apiData = await service.getCaseWithAssets(projectId, finalCaseId)
                        // Store the fetched data in the cache for future use
                        queryClient.setQueryData(["caseApiData", projectId, finalCaseId], apiData)
                    } catch (error) {
                        console.error("Error fetching case data directly:", error)
                    }
                }

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
                        finalCaseId,
                        updatedResource,
                    )
                } else {
                    result = await service[updateMethod](
                        projectId,
                        finalCaseId,
                        resourceId || updatedResource.id,
                        resourceId ? updatedValue : updatedResource,
                    )
                }

                return result
            } finally {
                setIsSaving(false)
            }
        },
        onSuccess: (_, variables) => {
            const finalCaseId = variables.localCaseId || caseId
            if (projectId && finalCaseId) {
                // Invalidate the case data query
                queryClient.invalidateQueries({ queryKey: ["caseApiData", projectId, finalCaseId] })

                // Also invalidate the project data query to update the case list
                if (resourceName === "case" && variables.propertyKey === "archived") {
                    queryClient.invalidateQueries({ queryKey: ["projectApiData"] })
                }
            }
        },
        onError: (error: any) => {
            setSnackBarMessage(error.message || `Failed to update ${resourceName}`)
            logger.error(`Error updating ${resourceName}:`, error)
        },
    })

    return mutation
}
