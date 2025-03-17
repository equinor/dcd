import { useMutation, useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router"
import { GetDrillingCampaignsService } from "@/Services/DrillingCampaignsService"
import { useProjectContext } from "@/Store/ProjectContext"
import { useAppStore } from "@/Store/AppStore"
import { createLogger } from "@/Utils/logger"

export const useWellMutation = () => {
    const queryClient = useQueryClient()
    const { caseId } = useParams()
    const { projectId } = useProjectContext()
    const { setIsSaving, setSnackBarMessage } = useAppStore()

    const logger = createLogger({
        name: "WELL_MUTATION",
        enabled: false,
    })

    const campaignWellsMutation = useMutation({
        mutationFn: async ({ campaignId, wells }: { campaignId: string, wells: any[] }) => {
            if (!projectId || !caseId) {
                throw new Error("Project ID and Case ID are required")
            }

            setIsSaving(true)
            logger.info("Updating campaign wells:", { campaignId, wells })

            try {
                const service = GetDrillingCampaignsService()
                await service.updateCampaignWells(
                    projectId,
                    caseId,
                    campaignId,
                    wells,
                )
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
            setSnackBarMessage(error.message || "Failed to update campaign wells")
            logger.error("Error updating campaign wells:", error)
        },
    })

    const updateCampaignWells = (params: { campaignId: string, wells: any[] }) => campaignWellsMutation.mutateAsync(params)

    return {
        updateCampaignWells,
        isLoading: campaignWellsMutation.isPending,
    }
}
