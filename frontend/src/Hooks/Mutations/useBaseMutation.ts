import { useMutation, useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router"
import { useProjectContext } from "@/Store/ProjectContext"
import { useAppStore } from "@/Store/AppStore"

export interface MutationParams<T = any> {
  resourceId?: string;
  updatedValue: T;
  propertyKey: string;
  localCaseId?: string;
  [key: string]: any; // Allow additional custom parameters
}

export interface BaseMutationOptions<T = any, R = any> {
  resourceName: string;
  getService: () => any;
  updateMethod: string;
  customMutationFn?: (
    service: any,
    projectId: string,
    caseId: string,
    params: MutationParams<T>
  ) => Promise<R>;
  getResourceFromApiData: (apiData: any) => any;
  invalidateQueries?: string[][];
}

/**
 * Base mutation hook that supports both standard property updates and custom mutation functions
 */
export const useBaseMutation = <T = any, R = any>({
    resourceName,
    getService,
    updateMethod,
    customMutationFn,
    getResourceFromApiData,
    invalidateQueries = [],
}: BaseMutationOptions<T, R>) => {
    const queryClient = useQueryClient()
    const { caseId } = useParams()
    const { projectId } = useProjectContext()
    const { setIsSaving, setSnackBarMessage } = useAppStore()

    const mutation = useMutation({
        mutationFn: async (params: MutationParams<T>) => {
            if (!projectId || (!caseId && !params.localCaseId)) {
                throw new Error("Project ID and Case ID are required")
            }

            setIsSaving(true)

            try {
                const service = getService()
                const finalCaseId = params.localCaseId || caseId || ""

                // If a custom mutation function is provided, use it
                if (customMutationFn) {
                    return await customMutationFn(service, projectId, finalCaseId, params)
                }

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
                    [params.propertyKey as string]: params.updatedValue,
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
                        params.resourceId || updatedResource.id,
                        params.resourceId ? params.updatedValue : updatedResource,
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

                // Invalidate any additional queries specified in the options
                invalidateQueries.forEach((queryKey) => {
                    queryClient.invalidateQueries({ queryKey })
                })
            }
        },
        onError: (error: any) => {
            setSnackBarMessage(error.message || `Failed to update ${resourceName}`)
        },
    })

    return mutation
}
