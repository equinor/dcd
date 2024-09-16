import { v4 as uuidv4 } from "uuid"
import { useMutation, useQueryClient } from "react-query"
import { useLocation, useNavigate, useParams } from "react-router"
import _ from "lodash"
import { useCaseContext } from "../Context/CaseContext"
import {
    EditInstance,
    EditEntry,
    ResourceName,
    ResourcePropertyKey,
    ResourceObject,
} from "../Models/Interfaces"
import { getCurrentEditId } from "../Utils/common"
import { GetCaseService } from "../Services/CaseService"
import { GetTopsideService } from "../Services/TopsideService"
import { GetSurfService } from "../Services/SurfService"
import { GetSubstructureService } from "../Services/SubstructureService"
import { GetTransportService } from "../Services/TransportService"
import { GetDrainageStrategyService } from "../Services/DrainageStrategyService"
import { useAppContext } from "../Context/AppContext"
import { GetWellProjectService } from "../Services/WellProjectService"
import { GetExplorationService } from "../Services/ExplorationService"
import {
    productionOverrideResources,
    totalStudyCostOverrideResources,
} from "../Utils/constants"
import { GetProjectService } from "../Services/ProjectService"

interface AddEditParams {
    // inputLabel: string;
    projectId: string;
    newDisplayValue?: string | number | undefined;
    // previousDisplayValue?: string | number | undefined;
    newResourceObject: ResourceObject;
    // previousResourceObject: ResourceObject;
    // tabName?: string;
    // tableName?: string;
    // inputFieldId?: string;
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
        setIsCalculatingProductionOverrides,
        setIsCalculatingTotalStudyCostOverrides,
        apiQueue,
        setApiQueue,
    } = useAppContext()
    const {
        caseEdits,
        setCaseEdits,
        editIndexes,
        setEditIndexes,
        caseEditsBelongingToCurrentCase,
    } = useCaseContext()

    const { caseId: caseIdFromParams } = useParams()
    const location = useLocation()
    const navigate = useNavigate()

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
        newDisplayValue,
        // previousDisplayValue,
        newResourceObject,
        // previousResourceObject,
    }: AddEditParams) => {
        // if (_.isEqual(newResourceObject, previousResourceObject)) {
        //     console.log("No changes made")
        //     return
        // }

        // const insertedEditInstanceObject: EditInstance = {
        //     uuid: uuidv4(),
        //     timeStamp: new Date().getTime(),
        //     inputLabel,
        //     projectId,
        //     newDisplayValue,
        //     previousDisplayValue,
        //     newResourceObject,
        //     previousResourceObject,
        //     tabName,
        //     tableName,
        //     inputFieldId,
        // }

        // if (newDisplayValue === previousDisplayValue && !newResourceObject) {
        //     console.log("No changes detected")
        //     return
        // }

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
