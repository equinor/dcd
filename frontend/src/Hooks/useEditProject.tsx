import { useMutation, useQueryClient } from "@tanstack/react-query"

import { GetProjectService } from "../Services/ProjectService"
import { useAppStore } from "../Store/AppStore"

export const useProjectEdits = () => {
    const queryClient = useQueryClient()
    const { setSnackBarMessage, setIsSaving } = useAppStore()

    type UpdateProjectVariables = {
        projectId: string;
        body: Components.Schemas.UpdateProjectDto;
    };

    const projectMutationFn = async ({ projectId, body }: UpdateProjectVariables) => {
        const res = await GetProjectService().updateProject(projectId, body)

        return res
    }

    const projectMutation = useMutation({
        mutationFn: projectMutationFn,
        onSuccess: (variables) => {
            const { fusionProjectId } = variables.commonProjectAndRevisionData

            queryClient.invalidateQueries(
                { queryKey: ["projectApiData", fusionProjectId] },
            )
            setIsSaving(false)
        },
        onError: (error) => {
            console.error("Error updating project", error)
            setSnackBarMessage(error.message)
        },
    })

    const addProjectEdit = (
        projectId: string,
        projectEdit: Components.Schemas.UpdateProjectDto,
    ) => {
        setIsSaving(true)
        projectMutation.mutate({ projectId, body: projectEdit })
    }

    return {
        addProjectEdit,
    }
}

export default useProjectEdits
