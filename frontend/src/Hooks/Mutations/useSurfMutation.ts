import { useQueryClient } from "@tanstack/react-query"
import { GetSurfService } from "@/Services/SurfService"
import { useBaseMutation, MutationParams } from "./useBaseMutation"

export const useSurfMutation = () => {
    const queryClient = useQueryClient()

    const surfMutationFn = async (
        service: ReturnType<typeof GetSurfService>,
        projectIdParam: string,
        caseIdParam: string,
        params: MutationParams<any>,
    ) => {
        const apiData = await queryClient.getQueryData<any>(["caseApiData", projectIdParam, caseIdParam])

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
            approvedBy: updatedSurf.approvedBy || "",
            maturity: updatedSurf.maturity,
        }

        return service.updateSurf(
            projectIdParam,
            caseIdParam,
            dto,
        )
    }

    const mutation = useBaseMutation({
        resourceName: "surf",
        getService: GetSurfService,
        updateMethod: "updateSurf",
        customMutationFn: surfMutationFn,
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
