import { useMutation, useQueryClient } from "@tanstack/react-query"
import { GetProjectService } from "../Services/ProjectService"
import { GetTechnicalInputService } from "../Services/TechnicalInputService"
import { useAppContext } from "../Context/AppContext"

export const useProjectEdits = () => {
    const queryClient = useQueryClient()
    const { setSnackBarMessage, setIsSaving } = useAppContext()

    type UpdateProjectVariables = {
        projectId: string;
        body: Components.Schemas.UpdateProjectDto;
    };

    type UpdateTechnicalInputVariables = {
        projectId: string;
        body: Components.Schemas.UpdateTechnicalInputDto;
    };

    const projectMutationFn = async ({ projectId, body }: UpdateProjectVariables) => {
        const projectService = await GetProjectService()
        const res = await projectService.updateProject(projectId, body)
        return res
    }

    const technicalInputMutationFn = async ({ projectId, body }: UpdateTechnicalInputVariables) => {
        const technicalInputService = await GetTechnicalInputService()
        const res = await technicalInputService.update(projectId, body)
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

    const technicalInputMutation = useMutation({
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
        },
    })

    const addProjectEdit = (
        projectId: string,
        projectEdit: Components.Schemas.UpdateProjectDto,
    ) => {
        setIsSaving(true)
        projectMutation.mutate({ projectId, body: projectEdit })
    }

    const addTechnicalInputEdit = (
        projectId: string,
        technicalInputEdit: Components.Schemas.UpdateTechnicalInputDto,
    ) => {
        setIsSaving(true)
        technicalInputMutation.mutate({ projectId, body: technicalInputEdit })
    }

    return {
        addProjectEdit,
        addTechnicalInputEdit,
    }
}

export default useProjectEdits
