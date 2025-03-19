import { useQueryClient } from "@tanstack/react-query"
import { GetTransportService } from "@/Services/TransportService"
import { useBaseMutation, MutationParams } from "./useBaseMutation"

export const useTransportMutation = () => {
    const queryClient = useQueryClient()

    const transportMutationFn = async (
        service: ReturnType<typeof GetTransportService>,
        projectIdParam: string,
        caseIdParam: string,
        params: MutationParams<any>,
    ) => {
        const apiData = await queryClient.getQueryData<any>(["caseApiData", projectIdParam, caseIdParam])

        if (!apiData?.transport) {
            throw new Error("Transport data not found in cache")
        }

        const currentTransport = apiData.transport
        const updatedTransport = {
            ...currentTransport,
            [params.propertyKey]: params.updatedValue,
        }

        const dto = {
            gasExportPipelineLength: updatedTransport.gasExportPipelineLength,
            oilExportPipelineLength: updatedTransport.oilExportPipelineLength,
            costYear: updatedTransport.costYear,
            source: updatedTransport.source,
            maturity: updatedTransport.maturity,
        }

        return service.updateTransport(
            projectIdParam,
            caseIdParam,
            dto,
        )
    }

    const mutation = useBaseMutation({
        resourceName: "transport",
        getService: GetTransportService,
        updateMethod: "updateTransport",
        customMutationFn: transportMutationFn,
        getResourceFromApiData: (apiData) => apiData?.transport,
    })

    const updateOilExportPipelineLength = (transportId: string, newValue: number) => mutation.mutateAsync({
        resourceId: transportId,
        updatedValue: newValue,
        propertyKey: "oilExportPipelineLength",
    })

    const updateGasExportPipelineLength = (transportId: string, newValue: number) => mutation.mutateAsync({
        resourceId: transportId,
        updatedValue: newValue,
        propertyKey: "gasExportPipelineLength",
    })

    return {
        updateOilExportPipelineLength,
        updateGasExportPipelineLength,
        isLoading: mutation.isPending,
    }
}
