import { useMutation, useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router"

import { GetSubstructureService } from "@/Services/SubstructureService"
import { useAppStore } from "@/Store/AppStore"
import { useProjectContext } from "@/Store/ProjectContext"

export interface Params {
    updatedValue: string | number | boolean;
    propertyKey: keyof Components.Schemas.UpdateSubstructureDto;
}

export const useSubstructureMutation = () => {
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
                const service = GetSubstructureService()
                const apiData = queryClient.getQueryData<Components.Schemas.CaseWithAssetsDto>(["caseApiData", projectId, caseId])

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
                    approvedBy: updatedSubstructure.approvedBy,
                }

                return service.updateSubstructure(projectId, caseId, dto)
            } finally {
                setIsSaving(false)
            }
        },
        onSuccess: () => {
            if (projectId && caseId) {
                queryClient.invalidateQueries({ queryKey: ["caseApiData", projectId, caseId] })
            }
        },
        onError: (error: any) => {
            setSnackBarMessage(error.message || "Failed to update Substructure")
        },
    })

    const updateConcept = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "concept",
    })

    const updateDryWeight = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "dryWeight",
    })

    return {
        updateConcept,
        updateDryWeight,
        isLoading: mutation.isPending,
    }
}
