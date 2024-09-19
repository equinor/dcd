import { useMutation, useQueryClient } from "react-query"
import _ from "lodash"
import {
    ResourceObject,
} from "../Models/Interfaces"
import { useAppContext } from "../Context/AppContext"
import { GetProjectService } from "../Services/ProjectService"

interface AddEditParams {
    projectId: string;
    newDisplayValue?: string | number | undefined;
    newResourceObject: ResourceObject;
    previousResourceObject: ResourceObject;
}

type SubmitToApiParams = {
    projectId: string,
    resourceObject: ResourceObject,
}

const useProjectDataEdits = (): {
    addProjectEdit: (params: AddEditParams) => void;
} => {
    const {
        setSnackBarMessage,
    } = useAppContext()

    const queryClient = useQueryClient()

    const mutation = useMutation(
        async ({ serviceMethod }: {
            projectId: string,
            serviceMethod: object,
        }) => serviceMethod,
        {
            onSuccess: (
                results: any,
                variables,
            ) => {
                const { projectId } = variables
                queryClient.fetchQuery(["apiData", { projectId }])
                // queryClient.invalidateQueries({
                //     queryKey: ["apiData", { projectId }],
                // })
            },
            onError: (error: any) => {
                console.error("Failed to update data:", error)
                setSnackBarMessage(error.message)
            },
        },
    )

    const updateProject = async (
        projectId: string,
        resourceObject: ResourceObject,

    ) => {
        const service = await GetProjectService()
        const serviceMethod = service.updateProject(
            projectId,
            resourceObject as Components.Schemas.ProjectWithAssetsDto,
        )

        try {
            console.log("updating project")
            return await mutation.mutateAsync({
                projectId,
                serviceMethod,
            })
        } catch (error) {
            return error
        }
    }

    const submitToApi = async ({
        projectId,
        resourceObject,
    }: SubmitToApiParams): Promise<any> => {
        let success = {}
        success = await updateProject(
            projectId,
            resourceObject,
        )

        return success
    }

    const addProjectEdit = async ({
        projectId,
        newResourceObject,
        previousResourceObject,
    }: AddEditParams) => {
        if (_.isEqual(newResourceObject, previousResourceObject)) {
            console.log("No changes made")
            return
        }

        const success = await submitToApi(
            {
                projectId,
                resourceObject: newResourceObject as ResourceObject,
            },
        )

        console.log(success)
    }

    return {
        addProjectEdit,
    }
}

export default useProjectDataEdits
