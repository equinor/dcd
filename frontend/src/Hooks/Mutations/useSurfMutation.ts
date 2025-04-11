import { useMutation, useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router"

import { GetSurfService } from "@/Services/SurfService"
import { useAppStore } from "@/Store/AppStore"
import { useProjectContext } from "@/Store/ProjectContext"

export interface Params {
    updatedValue: string | number | boolean;
    propertyKey: keyof Components.Schemas.UpdateSurfDto;
}

export const useSurfMutation = () => {
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

                if (!apiData?.surf) {
                    throw new Error("Surf data not found in cache")
                }

                const currentSurf = apiData.surf
                const updatedSurf = {
                    ...currentSurf,
                    [params.propertyKey]: params.updatedValue,
                }

                const dto = {
                    cessationCost: updatedSurf.cessationCost,
                    infieldPipelineSystemLength: updatedSurf.infieldPipelineSystemLength,
                    umbilicalSystemLength: updatedSurf.umbilicalSystemLength,
                    artificialLift: updatedSurf.artificialLift,
                    riserCount: updatedSurf.riserCount,
                    templateCount: updatedSurf.templateCount,
                    producerCount: updatedSurf.producerCount,
                    gasInjectorCount: updatedSurf.gasInjectorCount,
                    waterInjectorCount: updatedSurf.waterInjectorCount,
                    productionFlowline: updatedSurf.productionFlowline,
                    costYear: updatedSurf.costYear,
                    source: updatedSurf.source,
                    approvedBy: updatedSurf.approvedBy,
                    maturity: updatedSurf.maturity,
                }

                return GetSurfService().updateSurf(projectId, caseId, dto)
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
            setSnackBarMessage(error.message || "Failed to update surf")
        },
    })

    const updateProductionFlowline = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "productionFlowline",
    })

    const updateCessationCost = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "cessationCost",
    })

    const updateTemplateCount = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "templateCount",
    })

    const updateRiserCount = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "riserCount",
    })

    const updateInfieldPipelineSystemLength = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "infieldPipelineSystemLength",
    })

    const updateUmbilicalSystemLength = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "umbilicalSystemLength",
    })

    const updateProducerCount = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "producerCount",
    })

    const updateGasInjectorCount = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "gasInjectorCount",
    })

    const updateWaterInjectorCount = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "waterInjectorCount",
    })

    const updateMaturity = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "maturity",
    })

    return {
        updateProductionFlowline,
        updateCessationCost,
        updateTemplateCount,
        updateRiserCount,
        updateInfieldPipelineSystemLength,
        updateUmbilicalSystemLength,
        updateProducerCount,
        updateGasInjectorCount,
        updateWaterInjectorCount,
        updateMaturity,
        isLoading: mutation.isPending,
    }
}
