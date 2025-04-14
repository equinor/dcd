import { useMutation, useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router"

import { CampaignProfileType } from "@/Components/CaseTabs/CaseDrillingSchedule/Components/CampaignProfileTypes"
import { ITimeSeries } from "@/Models/ITimeSeries"
import { CampaignCostType } from "@/Models/enums"
import { GetDrillingCampaignsService } from "@/Services/DrillingCampaignsService"
import { useAppStore } from "@/Store/AppStore"
import { useProjectContext } from "@/Store/ProjectContext"

export interface CampaignWellUpdate {
    wellId: string
    startYear: number
    values: number[]
}

export interface CampaignMutationParams {
    campaignId: string;
    rigUpgradingCost?: number;
    rigMobDemobCost?: number;
    [CampaignProfileType.RigUpgrading]?: ITimeSeries;
    [CampaignProfileType.RigMobDemob]?: ITimeSeries;
    wells?: CampaignWellUpdate[];
}

export const useCampaignMutation = () => {
    const { caseId } = useParams()
    const { projectId } = useProjectContext()

    /**
     * Helper function to update a campaign profile (table edit)
     */
    const updateCampaignProfile = (
        service: ReturnType<typeof GetDrillingCampaignsService>,
        projectIdParam: string,
        caseIdParam: string,
        campaignId: string,
        campaignCostType: Components.Schemas.CampaignCostType,
        profile: ITimeSeries,
    ) => {
        const campaignUpdate: Components.Schemas.UpdateCampaignDto = {
            campaignCostType,
            startYear: profile.startYear,
            values: profile.values || [],
        }

        return service.updateCampaign(
            projectIdParam,
            caseIdParam,
            campaignId,
            campaignUpdate,
        )
    }

    const queryClient = useQueryClient()
    const { setIsSaving, setSnackBarMessage } = useAppStore()

    const mutation = useMutation({
        mutationFn: async (params: any) => {
            if (!projectId || !caseId) {
                throw new Error("Project ID and Case ID are required")
            }

            setIsSaving(true)

            try {
                const service = GetDrillingCampaignsService()

                const {
                    campaignId,
                    rigUpgradingCost,
                    rigMobDemobCost,
                    [CampaignProfileType.RigUpgrading]: rigUpgradingProfile,
                    [CampaignProfileType.RigMobDemob]: rigMobDemobProfile,
                    wells,
                } = params.updatedValue

                // Handle standalone cost input updates
                if (rigUpgradingCost !== undefined) {
                    return service.updateRigUpgradingCost(
                        projectId,
                        caseId,
                        campaignId,
                        rigUpgradingCost,
                    )
                }

                if (rigMobDemobCost !== undefined) {
                    return service.updateRigMobDemobCost(
                        projectId,
                        caseId,
                        campaignId,
                        rigMobDemobCost,
                    )
                }

                // Handle table profile updates
                if (rigUpgradingProfile) {
                    return updateCampaignProfile(
                        service,
                        projectId,
                        caseId,
                        campaignId,
                        CampaignCostType.RigUpgrading,
                        rigUpgradingProfile,
                    )
                }

                if (rigMobDemobProfile) {
                    return updateCampaignProfile(
                        service,
                        projectId,
                        caseId,
                        campaignId,
                        CampaignCostType.RigMobDemob,
                        rigMobDemobProfile,
                    )
                }

                // Handle campaign well updates
                if (wells) {
                    return service.updateCampaignWells(
                        projectId,
                        caseId,
                        campaignId,
                        wells,
                    )
                }

                throw new Error("Invalid campaign mutation parameters")
            } finally {
                setIsSaving(false)
            }
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["caseApiData", projectId, caseId] })
        },
        onError: (error: unknown) => {
            if (error instanceof Error) {
                setSnackBarMessage(error.message || "Failed to update campaign")
            }
        },
    })

    // Cost updates - from standalone input fields
    const updateRigUpgradingCost = (campaignId: string, cost: number) => mutation.mutateAsync({
        updatedValue: {
            campaignId,
            rigUpgradingCost: cost,
        },
        propertyKey: "rigUpgradingCost",
    })

    const updateRigMobDemobCost = (campaignId: string, cost: number) => mutation.mutateAsync({
        updatedValue: {
            campaignId,
            rigMobDemobCost: cost,
        },
        propertyKey: "rigMobDemobCost",
    })

    // Profile updates - from table edits
    const updateRigUpgradingProfile = (campaignId: string, profile: ITimeSeries) => mutation.mutateAsync({
        updatedValue: {
            campaignId,
            [CampaignProfileType.RigUpgrading]: profile,
        },
        propertyKey: CampaignProfileType.RigUpgrading,
    })

    const updateRigMobDemobProfile = (campaignId: string, profile: ITimeSeries) => mutation.mutateAsync({
        updatedValue: {
            campaignId,
            [CampaignProfileType.RigMobDemob]: profile,
        },
        propertyKey: CampaignProfileType.RigMobDemob,
    })

    // Well updates
    const updateCampaignWells = (params: { campaignId: string, wells: CampaignWellUpdate[] }) => mutation.mutateAsync({
        updatedValue: {
            campaignId: params.campaignId,
            wells: params.wells,
        },
        propertyKey: "wells",
    })

    return {
        // Cost updates (from standalone input fields)
        updateRigUpgradingCost,
        updateRigMobDemobCost,

        // Profile updates (from tables)
        updateRigUpgradingProfile,
        updateRigMobDemobProfile,

        // Well updates
        updateCampaignWells,

        isLoading: mutation.isPending,
    }
}
