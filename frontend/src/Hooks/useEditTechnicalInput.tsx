import { useMutation, useQueryClient } from "@tanstack/react-query"
import { GetTechnicalInputService } from "../Services/TechnicalInputService"
import { useAppContext } from "../Context/AppContext"

type UpdateTechnicalInputVariables = {
    projectId: string;
    body: Components.Schemas.UpdateWellsDto;
};

type UpdateExplorationWellCostVariables = {
    projectId: string;
    explorationOperationalWellCostsId: string;
    body: Components.Schemas.UpdateExplorationOperationalWellCostsDto;
};

type UpdateDevelopmentWellCostVariables = {
    projectId: string;
    developmentOperationalWellCostsId: string;
    body: Components.Schemas.UpdateDevelopmentOperationalWellCostsDto;
};

export const useTechnicalInputEdits = () => {
    const queryClient = useQueryClient()
    const { setSnackBarMessage } = useAppContext()

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

    const wellsMutation = useMutation({
        mutationFn: technicalInputMutationFn,
        onSuccess: (variables) => {
            const { fusionProjectId } = variables.commonProjectAndRevisionData
            queryClient.invalidateQueries(
                { queryKey: ["projectApiData", fusionProjectId] },
            )
        },
        onError: (error) => {
            console.error("Error updating technical input", error)
            setSnackBarMessage(error.message)
        },
    })

    const ExplorationWellCostMutation = useMutation({
        mutationFn: ExplorationWellCostMutationFn,
        onSuccess: (_, variables) => {
            queryClient.invalidateQueries(
                { queryKey: ["projectApiData", variables.projectId] },
            )
        },
        onError: (error) => {
            console.error("Error updating technical input", error)
            setSnackBarMessage(error.message)
        },
    })

    const DevelopmentWellCostMutation = useMutation({
        mutationFn: DevelopmentWellCostMutationFn,
        onSuccess: (_, variables) => {
            queryClient.invalidateQueries(
                { queryKey: ["projectApiData", variables.projectId] },
            )
        },
    })

    const addWellsEdit = (
        projectId: string,
        technicalInputEdit: Components.Schemas.UpdateWellsDto,
    ) => {
        wellsMutation.mutate({ projectId, body: technicalInputEdit })
    }

    const addExplorationWellCostEdit = (
        projectId: string,
        explorationOperationalWellCostsId: string,
        explorationWellsCostEdit: Components.Schemas.UpdateExplorationOperationalWellCostsDto,
    ) => {
        ExplorationWellCostMutation.mutate({ projectId, explorationOperationalWellCostsId, body: explorationWellsCostEdit })
    }

    const addDevelopmentWellCostEdit = (
        projectId: string,
        developmentOperationalWellCostsId: string,
        developmentWellsCostEdit: Components.Schemas.UpdateDevelopmentOperationalWellCostsDto,
    ) => {
        DevelopmentWellCostMutation.mutate({ projectId, developmentOperationalWellCostsId, body: developmentWellsCostEdit })
    }

    return {
        addWellsEdit,
        addExplorationWellCostEdit,
        addDevelopmentWellCostEdit,
    }
}

export default useTechnicalInputEdits
