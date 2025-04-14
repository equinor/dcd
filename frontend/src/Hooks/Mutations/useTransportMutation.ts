import { useMutation, useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router"

import { GetTransportService } from "@/Services/TransportService"
import { useAppStore } from "@/Store/AppStore"
import { useProjectContext } from "@/Store/ProjectContext"

export interface Params {
    updatedValue: string | number | boolean;
    propertyKey: keyof Components.Schemas.UpdateTransportDto;
}

export const useTransportMutation = () => {
    const queryClient = useQueryClient()
    const { caseId } = useParams()
    const { projectId } = useProjectContext()
    const { setIsSaving, setSnackBarMessage } = useAppStore()

    const mutation = useMutation({
        mutationFn: async (params: Params) => {
            if (!projectId || !caseId) {
                throw new Error("Project ID and Case ID are required")
            }

            setIsSaving(true)

            try {
                const apiData = queryClient.getQueryData<Components.Schemas.CaseWithAssetsDto>(["caseApiData", projectId, caseId])

                if (!apiData?.transport) {
                    throw new Error("Transport data not found in cache")
                }

                const updatedTransport = {
                    ...apiData.transport,
                    [params.propertyKey]: params.updatedValue,
                }

                const dto = {
                    gasExportPipelineLength: updatedTransport.gasExportPipelineLength,
                    oilExportPipelineLength: updatedTransport.oilExportPipelineLength,
                    costYear: updatedTransport.costYear,
                    source: updatedTransport.source,
                    maturity: updatedTransport.maturity,
                }

                return GetTransportService().updateTransport(projectId, caseId, dto)
            } finally {
                setIsSaving(false)
            }
        },
        onSuccess: () => {
            if (projectId && caseId) {
                queryClient.invalidateQueries({ queryKey: ["caseApiData", projectId, caseId] })
            }
        },
        onError: (error: Error) => {
            setSnackBarMessage(error.message || "Failed to update Transport")
        },
    })

    const updateOilExportPipelineLength = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "oilExportPipelineLength",
    })

    const updateGasExportPipelineLength = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "gasExportPipelineLength",
    })

    return {
        updateOilExportPipelineLength,
        updateGasExportPipelineLength,
        isLoading: mutation.isPending,
    }
}
