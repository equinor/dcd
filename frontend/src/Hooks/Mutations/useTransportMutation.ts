import { useMutation, useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router"
import { useProjectContext } from "@/Store/ProjectContext"
import { useAppStore } from "@/Store/AppStore"
import { GetTransportService } from "@/Services/TransportService"
import { createLogger } from "@/Utils/logger"

const logger = createLogger({
    name: "TRANSPORT_MUTATION",
    enabled: false,
})

export const useTransportMutation = () => {
    const queryClient = useQueryClient()
    const { caseId } = useParams()
    const { projectId } = useProjectContext()
    const { setIsSaving, setSnackBarMessage } = useAppStore()

    const mutation = useMutation({
        mutationFn: async ({
            transportId,
            updatedValue,
            propertyKey,
        }: {
            transportId: string,
            updatedValue: any,
            propertyKey: string,
        }) => {
            if (!projectId || !caseId) {
                throw new Error("Project ID and Case ID are required")
            }

            setIsSaving(true)
            logger.info("Updating transport:", { transportId, updatedValue, propertyKey })

            try {
                const service = GetTransportService()
                const transport = await queryClient.getQueryData<any>(["caseApiData", projectId, caseId])?.transport

                if (!transport) {
                    throw new Error("Transport not found")
                }

                // Create a copy of the transport and update the specified property
                const updatedTransport = {
                    ...transport,
                    [propertyKey]: updatedValue,
                }

                // Send the updated transport to the API
                const result = await service.updateTransport(
                    projectId,
                    caseId,
                    updatedTransport,
                )

                return result
            } finally {
                setIsSaving(false)
            }
        },
        onSuccess: () => {
            // Invalidate the case data query to trigger a refetch
            if (projectId && caseId) {
                queryClient.invalidateQueries({ queryKey: ["caseApiData", projectId, caseId] })
            }
        },
        onError: (error: any) => {
            setSnackBarMessage(error.message || "Failed to update transport")
            logger.error("Error updating transport:", error)
        },
    })

    const updateOilExportPipelineLength = (transportId: string, newValue: number) => mutation.mutateAsync({
        transportId,
        updatedValue: newValue,
        propertyKey: "oilExportPipelineLength",
    })

    const updateGasExportPipelineLength = (transportId: string, newValue: number) => mutation.mutateAsync({
        transportId,
        updatedValue: newValue,
        propertyKey: "gasExportPipelineLength",
    })

    return {
        updateOilExportPipelineLength,
        updateGasExportPipelineLength,
        isLoading: mutation.isPending,
    }
}
