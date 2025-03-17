import { GetDrainageStrategyService } from "@/Services/DrainageStrategyService"
import { useBaseMutation } from "./useBaseMutation"

export const useDrainageStrategyMutation = () => {
    const mutation = useBaseMutation({
        resourceName: "drainageStrategy",
        getService: GetDrainageStrategyService,
        updateMethod: "updateDrainageStrategy",
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
