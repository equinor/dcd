import { useMutation, useQueryClient } from "@tanstack/react-query"

import { GetTechnicalInputService } from "../Services/TechnicalInputService"
import { useAppStore } from "../Store/AppStore"

type UpdateTechnicalInputVariables = {
    projectId: string;
    fusionProjectId: string;
    body: Components.Schemas.UpdateWellsDto;
};

export const useTechnicalInputEdits = () => {
    const queryClient = useQueryClient()
    const { setSnackBarMessage, setIsSaving } = useAppStore()

    const technicalInputMutationFn = async ({ projectId, body }: UpdateTechnicalInputVariables) => {
        const res = await GetTechnicalInputService().updateWells(projectId, body)

        return res
    }

    const wellsMutation = useMutation({
        mutationFn: technicalInputMutationFn,
        onSuccess: (variables) => {
            const { fusionProjectId } = variables.commonProjectAndRevisionData

            queryClient.invalidateQueries(
                { queryKey: ["projectApiData", fusionProjectId] },
            )
            setIsSaving(false)
        },
        onError: (error) => {
            console.error("Error updating technical input", error)
            setSnackBarMessage(error.message)
            setIsSaving(false)
        },
    })

    const addWellsEdit = (
        projectId: string,
        fusionProjectId: string,
        technicalInputEdit: Components.Schemas.UpdateWellsDto,
    ) => {
        setIsSaving(true)
        wellsMutation.mutate({ projectId, fusionProjectId, body: technicalInputEdit })
    }

    return {
        addWellsEdit,
    }
}

export default useTechnicalInputEdits
