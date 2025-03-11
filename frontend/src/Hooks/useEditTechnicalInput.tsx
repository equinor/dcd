import { useMutation, useQueryClient } from "@tanstack/react-query"
import { GetTechnicalInputService } from "../Services/TechnicalInputService"
import { useAppStore } from "../Store/AppStore"

type UpdateTechnicalInputVariables = {
    projectId: string;
    fusionProjectId: string;
    body: Components.Schemas.UpdateWellsDto;
};

type UpdateExplorationWellCostVariables = {
    projectId: string;
    fusionProjectId: string;
    explorationOperationalWellCostsId: string;
    body: Components.Schemas.UpdateExplorationOperationalWellCostsDto;
};

type UpdateDevelopmentWellCostVariables = {
    projectId: string;
    fusionProjectId: string;
    developmentOperationalWellCostsId: string;
    body: Components.Schemas.UpdateDevelopmentOperationalWellCostsDto;
};

export const useTechnicalInputEdits = () => {
    const queryClient = useQueryClient()
    const { setSnackBarMessage, setIsSaving } = useAppStore()

    const technicalInputMutationFn = async ({ projectId, body }: UpdateTechnicalInputVariables) => {
        const res = await GetTechnicalInputService().updateWells(projectId, body)
        return res
    }

    const ExplorationWellCostMutationFn = async ({ projectId, explorationOperationalWellCostsId, body }: UpdateExplorationWellCostVariables) => {
        const res = await GetTechnicalInputService().updateExplorationOperationalWellCosts(projectId, explorationOperationalWellCostsId, body)
        return res
    }

    const DevelopmentWellCostMutationFn = async ({ projectId, developmentOperationalWellCostsId, body }: UpdateDevelopmentWellCostVariables) => {
        const res = await GetTechnicalInputService().updateDevelopmentOperationalWellCosts(projectId, developmentOperationalWellCostsId, body)
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

    const ExplorationWellCostMutation = useMutation({
        mutationFn: ExplorationWellCostMutationFn,
        onSuccess: (_, variables) => {
            queryClient.invalidateQueries(
                { queryKey: ["projectApiData", variables.fusionProjectId] },
            )
            setIsSaving(false)
        },
        onError: (error) => {
            console.error("Error updating technical input", error)
            setSnackBarMessage(error.message)
            setIsSaving(false)
        },
    })

    const DevelopmentWellCostMutation = useMutation({
        mutationFn: DevelopmentWellCostMutationFn,
        onSuccess: (_, variables) => {
            queryClient.invalidateQueries(
                { queryKey: ["projectApiData", variables.fusionProjectId] },
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

    const addExplorationWellCostEdit = (
        projectId: string,
        fusionProjectId: string,
        explorationOperationalWellCostsId: string,
        explorationWellsCostEdit: Components.Schemas.UpdateExplorationOperationalWellCostsDto,
    ) => {
        setIsSaving(true)
        ExplorationWellCostMutation.mutate({
            projectId,
            fusionProjectId,
            explorationOperationalWellCostsId,
            body: explorationWellsCostEdit,
        })
    }

    const addDevelopmentWellCostEdit = (
        projectId: string,
        fusionProjectId: string,
        developmentOperationalWellCostsId: string,
        developmentWellsCostEdit: Components.Schemas.UpdateDevelopmentOperationalWellCostsDto,
    ) => {
        setIsSaving(true)
        DevelopmentWellCostMutation.mutate({
            projectId,
            fusionProjectId,
            developmentOperationalWellCostsId,
            body: developmentWellsCostEdit,
        })
    }

    return {
        addWellsEdit,
        addExplorationWellCostEdit,
        addDevelopmentWellCostEdit,
    }
}

export default useTechnicalInputEdits
