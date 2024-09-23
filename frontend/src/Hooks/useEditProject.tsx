import { useMutation, useQueryClient } from "@tanstack/react-query"
import { GetProjectService } from "../Services/ProjectService"
import { useAppContext } from "../Context/AppContext"

export const useProjectEdits = () => {
    const queryClient = useQueryClient()
    const { setSnackBarMessage } = useAppContext()

    type UpdateProjectVariables = {
        projectId: string;
        body: Components.Schemas.UpdateProjectDto;
    };

    const mutationFn = async ({ projectId, body }: UpdateProjectVariables) => {
        const projectService = await GetProjectService()
        const res = await projectService.updateProject(projectId, body)
        return res
    }

    const mutation = useMutation({
        mutationFn,
        onSuccess: (variables) => {
            const { id } = variables
            queryClient.invalidateQueries(
                { queryKey: ["projectApiData", id] },
            )
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
        mutation.mutate({ projectId, body: projectEdit })
    }

    return {
        addProjectEdit,
    }
}

export default useProjectEdits
