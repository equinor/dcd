import { useMutation, useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router"
import { GetDrillingCampaignsService } from "@/Services/DrillingCampaignsService"
import { useProjectContext } from "@/Store/ProjectContext"
import { useAppStore } from "@/Store/AppStore"
import { createLogger } from "@/Utils/logger"

export const useCampaignMutation = () => {
    const queryClient = useQueryClient()
    const { caseId } = useParams()
    const { projectId } = useProjectContext()
    const { setIsSaving, setSnackBarMessage } = useAppStore()

    const logger = createLogger({
        name: "CAMPAIGN_MUTATION",
        enabled: false,
    })

    const createMutation = <T extends Record<string, any>>(
        operationName: string,
        serviceMethodName: string,
    ) => useMutation({
            mutationFn: async (params: { campaignId: string } & T) => {
                if (!projectId || !caseId) {
                    throw new Error("Project ID and Case ID are required")
                }

                setIsSaving(true)
                logger.info(`Updating campaign ${operationName}:`, params)

                try {
                    const service = GetDrillingCampaignsService()
                    const { campaignId, ...rest } = params

                    if (serviceMethodName === "updateCampaignWells") {
                        await service.updateCampaignWells(
                            projectId,
                            caseId,
                            campaignId,
                            rest.wells,
                        )
                    } else if (serviceMethodName === "updateRigUpgradingCost") {
                        await service.updateRigUpgradingCost(
                            projectId,
                            caseId,
                            campaignId,
                            rest.rigUpgradingCost,
                        )
                    } else if (serviceMethodName === "updateRigMobDemobCost") {
                        await service.updateRigMobDemobCost(
                            projectId,
                            caseId,
                            campaignId,
                            rest.rigMobDemobCost,
                        )
                    }
                } finally {
                    setIsSaving(false)
                }
            },
            onSuccess: () => {
                // Invalidate the case data query to trigger a refetch
                if (projectId && caseId) {
                    queryClient.invalidateQueries({ queryKey: ["caseApiData", projectId, caseId] })
                }
            },
            onError: (error: any) => {
                setSnackBarMessage(error.message || `Failed to update campaign ${operationName}`)
                logger.error(`Error updating campaign ${operationName}:`, error)
            },
        })

    const rigUpgradingCostMutation = createMutation<{ rigUpgradingCost: number | { startYear: number, values: number[] } }>(
        "rig upgrading cost",
        "updateRigUpgradingCost",
    )

    const rigMobDemobCostMutation = createMutation<{ rigMobDemobCost: number | { startYear: number, values: number[] } }>(
        "rig mob/demob cost",
        "updateRigMobDemobCost",
    )

    const campaignWellsMutation = createMutation<{ wells: Components.Schemas.SaveCampaignWellDto[] }>(
        "wells",
        "updateCampaignWells",
    )

    const updateRigUpgradingCost = (campaignId: string, rigUpgradingCost: number | { startYear: number, values: number[] }) => rigUpgradingCostMutation.mutateAsync({ campaignId, rigUpgradingCost })

    const updateRigMobDemobCost = (campaignId: string, rigMobDemobCost: number | { startYear: number, values: number[] }) => rigMobDemobCostMutation.mutateAsync({ campaignId, rigMobDemobCost })

    const updateCampaignWells = (campaignId: string, wells: Components.Schemas.SaveCampaignWellDto[]) => campaignWellsMutation.mutateAsync({ campaignId, wells })

    return {
        updateRigUpgradingCost,
        updateRigMobDemobCost,
        updateCampaignWells,
        isLoading:
            rigUpgradingCostMutation.isPending
            || rigMobDemobCostMutation.isPending
            || campaignWellsMutation.isPending,
    }
}
