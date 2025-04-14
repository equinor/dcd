import { useMutation, useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router"

import { GetDrainageStrategyService } from "@/Services/DrainageStrategyService"
import { useAppStore } from "@/Store/AppStore"
import { useProjectContext } from "@/Store/ProjectContext"

export interface Params {
    updatedValue: string | number | boolean;
    propertyKey: keyof Components.Schemas.UpdateDrainageStrategyDto;
}

export const useDrainageStrategyMutation = () => {
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

                if (!apiData?.drainageStrategy) {
                    throw new Error("Drainage strategy not found in cache")
                }

                const currentStrategy = apiData.drainageStrategy
                const updatedStrategy = {
                    ...currentStrategy,
                    [params.propertyKey]: params.updatedValue,
                }

                const dto = {
                    nglYield: updatedStrategy.nglYield,
                    condensateYield: updatedStrategy.condensateYield,
                    gasShrinkageFactor: updatedStrategy.gasShrinkageFactor,
                    producerCount: updatedStrategy.producerCount || 0,
                    gasInjectorCount: updatedStrategy.gasInjectorCount || 0,
                    waterInjectorCount: updatedStrategy.waterInjectorCount || 0,
                    artificialLift: updatedStrategy.artificialLift,
                    gasSolution: updatedStrategy.gasSolution,
                }

                return GetDrainageStrategyService().updateDrainageStrategy(projectId, caseId, dto)
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
            setSnackBarMessage(error.message || "Failed to update DrainageStrategy")
        },
    })

    const updateGasSolution = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "gasSolution",
    })

    const updateNglYield = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "nglYield",
    })

    const updateCondensateYield = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "condensateYield",
    })

    const updateGasShrinkageFactor = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "gasShrinkageFactor",
    })

    return {
        updateGasSolution,
        updateNglYield,
        updateCondensateYield,
        updateGasShrinkageFactor,
        isLoading: mutation.isPending,
    }
}
