import { useQueryClient } from "@tanstack/react-query"

import { useBaseMutation, MutationParams } from "./useBaseMutation"

import { GetSubstructureService } from "@/Services/SubstructureService"

export const useSubstructureMutation = () => {
    const queryClient = useQueryClient()

    const substructureMutationFn = async (
        service: ReturnType<typeof GetSubstructureService>,
        projectIdParam: string,
        caseIdParam: string,
        params: MutationParams<any>,
    ) => {
        const apiData = await queryClient.getQueryData<any>(["caseApiData", projectIdParam, caseIdParam])

        if (!apiData?.substructure) {
            throw new Error("Substructure data not found in cache")
        }

        const currentSubstructure = apiData.substructure
        const updatedSubstructure = {
            ...currentSubstructure,
            [params.propertyKey]: params.updatedValue,
        }

        const dto = {
            dryWeight: updatedSubstructure.dryWeight,
            costYear: updatedSubstructure.costYear,
            source: updatedSubstructure.source,
            concept: updatedSubstructure.concept,
            maturity: updatedSubstructure.maturity,
            approvedBy: updatedSubstructure.approvedBy || "",
        }

        return service.updateSubstructure(
            projectIdParam,
            caseIdParam,
            dto,
        )
    }

    const mutation = useBaseMutation({
        resourceName: "substructure",
        getService: GetSubstructureService,
        updateMethod: "updateSubstructure",
        customMutationFn: substructureMutationFn,
        getResourceFromApiData: (apiData) => apiData?.substructure,
    })

    const updateConcept = (substructureId: string, newValue: number) => mutation.mutateAsync({
        resourceId: substructureId,
        updatedValue: newValue,
        propertyKey: "concept",
    })

    const updateDryWeight = (substructureId: string, newValue: number) => mutation.mutateAsync({
        resourceId: substructureId,
        updatedValue: newValue,
        propertyKey: "dryWeight",
    })

    return {
        updateConcept,
        updateDryWeight,
        isLoading: mutation.isPending,
    }
}
