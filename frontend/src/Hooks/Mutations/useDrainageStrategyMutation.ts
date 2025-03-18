import { useQueryClient } from "@tanstack/react-query"
import { GetDrainageStrategyService } from "@/Services/DrainageStrategyService"
import { useBaseMutation, MutationParams } from "./useBaseMutation"

export const useDrainageStrategyMutation = () => {
    const queryClient = useQueryClient()

    // Custom mutation function that ensures we send a complete drainage strategy DTO
    const drainageStrategyMutationFn = async (
        service: ReturnType<typeof GetDrainageStrategyService>,
        projectId: string,
        caseId: string,
        params: MutationParams<any>,
    ) => {
        const apiData = await queryClient.getQueryData<any>(["caseApiData", projectId, caseId])

        if (!apiData?.drainageStrategy) {
            throw new Error("Drainage strategy not found in cache")
        }

        const currentStrategy = apiData.drainageStrategy
        const updatedStrategy = {
            ...currentStrategy,
            [params.propertyKey]: params.updatedValue,
        }

        // Make sure we're sending only the fields expected by the backend DTO
        const dto = {
            nglYield: updatedStrategy.nglYield,
            condensateYield: updatedStrategy.condensateYield,
            gasShrinkageFactor: updatedStrategy.gasShrinkageFactor,
            producerCount: updatedStrategy.producerCount || 0,
            gasInjectorCount: updatedStrategy.gasInjectorCount || 0,
            waterInjectorCount: updatedStrategy.waterInjectorCount || 0,
            artificialLift: updatedStrategy.artificialLift || 0,
            gasSolution: updatedStrategy.gasSolution,
        }

        return service.updateDrainageStrategy(
            projectId,
            caseId,
            dto,
        )
    }

    const mutation = useBaseMutation({
        resourceName: "drainageStrategy",
        getService: GetDrainageStrategyService,
        updateMethod: "updateDrainageStrategy",
        customMutationFn: drainageStrategyMutationFn,
        getResourceFromApiData: (apiData) => apiData?.drainageStrategy,
        loggerName: "DRAINAGE_STRATEGY_MUTATION",
    })

    const updateGasSolution = (drainageStrategyId: string, newValue: number) => mutation.mutateAsync({
        resourceId: drainageStrategyId,
        updatedValue: newValue,
        propertyKey: "gasSolution",
    })

    const updateNglYield = (drainageStrategyId: string, newValue: number) => mutation.mutateAsync({
        resourceId: drainageStrategyId,
        updatedValue: newValue,
        propertyKey: "nglYield",
    })

    const updateCondensateYield = (drainageStrategyId: string, newValue: number) => mutation.mutateAsync({
        resourceId: drainageStrategyId,
        updatedValue: newValue,
        propertyKey: "condensateYield",
    })

    const updateGasShrinkageFactor = (drainageStrategyId: string, newValue: number) => mutation.mutateAsync({
        resourceId: drainageStrategyId,
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
