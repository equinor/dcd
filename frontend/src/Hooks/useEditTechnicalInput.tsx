import { useMutation, useQueryClient } from "@tanstack/react-query"
import { GetTechnicalInputService } from "../Services/TechnicalInputService"
import { useAppContext } from "../Context/AppContext"

type UpdateTechnicalInputVariables = {
    projectId: string;
    body: Components.Schemas.UpdateTechnicalInputDto;
};

type UpdateExplorationWellCostVariables = {
    projectId: string;
    explorationOperationalWellCostsId: string;
    body: Components.Schemas.ExplorationOperationalWellCostsDto;
};

type UpdateDevelopmentWellCostVariables = {
    projectId: string;
    developmentOperationalWellCostsId: string;
    body: Components.Schemas.DevelopmentOperationalWellCostsDto;
};

export const useTechnicalInputEdits = () => {
    const queryClient = useQueryClient()
    const { setSnackBarMessage, setIsSaving } = useAppContext()

    const technicalInputMutationFn = async ({ projectId, body }: UpdateTechnicalInputVariables) => {
        const technicalInputService = await GetTechnicalInputService()
        const res = await technicalInputService.updateWells(projectId, body)
        return res
    }

    const ExplorationWellCostMutationFn = async ({ projectId, explorationOperationalWellCostsId, body }: UpdateExplorationWellCostVariables) => {
        const technicalInputService = await GetTechnicalInputService()
        const res = await technicalInputService.updateExplorationOperationalWellCosts(projectId, explorationOperationalWellCostsId, body)
        return res
    }

    const DevelopmentWellCostMutationFn = async ({ projectId, developmentOperationalWellCostsId, body }: UpdateDevelopmentWellCostVariables) => {
        const technicalInputService = await GetTechnicalInputService()
        const res = await technicalInputService.updateDevelopmentOperationalWellCosts(projectId, developmentOperationalWellCostsId, body)
        return res
    }

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

    const ExplorationWellCostMutation = useMutation({
        mutationFn: ExplorationWellCostMutationFn,
        onSuccess: (variables) => {
            queryClient.invalidateQueries(
                { queryKey: ["projectApiData", variables.projectId] },
            )
            setIsSaving(false)
        },
        onError: (error) => {
            console.error("Error updating technical input", error)
            setSnackBarMessage(error.message)
        },
    })

    const DevelopmentWellCostMutation = useMutation({
        mutationFn: DevelopmentWellCostMutationFn,
        onSuccess: (variables) => {
            queryClient.invalidateQueries(
                { queryKey: ["projectApiData", variables.projectId] },
            )
            setIsSaving(false)
        },
    })

    const addTechnicalInputEdit = (
        projectId: string,
        technicalInputEdit: Components.Schemas.UpdateTechnicalInputDto,
    ) => {
        setIsSaving(true)
        technicalInputMutation.mutate({ projectId, body: technicalInputEdit })
    }

    const addExplorationWellCostEdit = (
        projectId: string,
        explorationOperationalWellCostsId: string,
        explorationWellsCostEdit: Components.Schemas.ExplorationOperationalWellCostsDto,
    ) => {
        setIsSaving(true)
        ExplorationWellCostMutation.mutate({ projectId, explorationOperationalWellCostsId, body: explorationWellsCostEdit })
    }

    const addDevelopmentWellCostEdit = (
        projectId: string,
        developmentOperationalWellCostsId: string,
        developmentWellsCostEdit: Components.Schemas.DevelopmentOperationalWellCostsDto,
    ) => {
        setIsSaving(true)
        DevelopmentWellCostMutation.mutate({ projectId, developmentOperationalWellCostsId, body: developmentWellsCostEdit })
    }

    return {
        addTechnicalInputEdit,
        addExplorationWellCostEdit,
        addDevelopmentWellCostEdit,
    }
}

export default useTechnicalInputEdits
