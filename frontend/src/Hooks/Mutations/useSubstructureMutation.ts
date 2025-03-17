import { GetSubstructureService } from "@/Services/SubstructureService"
import { useBaseMutation } from "./useBaseMutation"

export const useSubstructureMutation = () => {
    const mutation = useBaseMutation({
        resourceName: "substructure",
        getService: GetSubstructureService,
        updateMethod: "updateSubstructure",
        getResourceFromApiData: (apiData) => apiData?.substructure,
        loggerName: "SUBSTRUCTURE_MUTATION",
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
