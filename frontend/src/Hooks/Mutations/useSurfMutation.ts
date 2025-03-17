import { GetSurfService } from "@/Services/SurfService"
import { useBaseMutation } from "./useBaseMutation"

export const useSurfMutation = () => {
    const mutation = useBaseMutation({
        resourceName: "surf",
        getService: GetSurfService,
        updateMethod: "updateSurf",
        getResourceFromApiData: (apiData) => apiData?.surf,
        loggerName: "SURF_MUTATION",
    })

    const updateProductionFlowline = (surfId: string, newValue: number) => mutation.mutateAsync({
        resourceId: surfId,
        updatedValue: newValue,
        propertyKey: "productionFlowline",
    })

    const updateCessationCost = (surfId: string, newValue: number) => mutation.mutateAsync({
        resourceId: surfId,
        updatedValue: newValue,
        propertyKey: "cessationCost",
    })

    const updateTemplateCount = (surfId: string, newValue: number) => mutation.mutateAsync({
        resourceId: surfId,
        updatedValue: newValue,
        propertyKey: "templateCount",
    })

    const updateRiserCount = (surfId: string, newValue: number) => mutation.mutateAsync({
        resourceId: surfId,
        updatedValue: newValue,
        propertyKey: "riserCount",
    })

    const updateInfieldPipelineSystemLength = (surfId: string, newValue: number) => mutation.mutateAsync({
        resourceId: surfId,
        updatedValue: newValue,
        propertyKey: "infieldPipelineSystemLength",
    })

    const updateUmbilicalSystemLength = (surfId: string, newValue: number) => mutation.mutateAsync({
        resourceId: surfId,
        updatedValue: newValue,
        propertyKey: "umbilicalSystemLength",
    })

    const updateProducerCount = (surfId: string, newValue: number) => mutation.mutateAsync({
        resourceId: surfId,
        updatedValue: newValue,
        propertyKey: "producerCount",
    })

    const updateGasInjectorCount = (surfId: string, newValue: number) => mutation.mutateAsync({
        resourceId: surfId,
        updatedValue: newValue,
        propertyKey: "gasInjectorCount",
    })

    const updateWaterInjectorCount = (surfId: string, newValue: number) => mutation.mutateAsync({
        resourceId: surfId,
        updatedValue: newValue,
        propertyKey: "waterInjectorCount",
    })

    const updateMaturity = (surfId: string, newValue: number) => mutation.mutateAsync({
        resourceId: surfId,
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
