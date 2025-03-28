import { useParams } from "react-router"

import { useBaseMutation, MutationParams } from "./useBaseMutation"

import { CampaignProfileType } from "@/Components/CaseTabs/CaseDrillingSchedule/Components/CampaignProfileTypes"
import { ITimeSeries } from "@/Models/ITimeSeries"
import { GetDrillingCampaignsService } from "@/Services/DrillingCampaignsService"
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

    // Custom mutation function for campaign operations
    const campaignMutationFn = async (
        service: ReturnType<typeof GetDrillingCampaignsService>,
        projectIdParam: string,
        caseIdParam: string,
        params: MutationParams<CampaignMutationParams>,
    ) => {
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
                projectIdParam,
                caseIdParam,
                campaignId,
                rigUpgradingCost,
            )
        }

        if (rigMobDemobCost !== undefined) {
            return service.updateRigMobDemobCost(
                projectIdParam,
                caseIdParam,
                campaignId,
                rigMobDemobCost,
            )
        }

        // Handle table profile updates
        if (rigUpgradingProfile) {
            return updateCampaignProfile(
                service,
                projectIdParam,
                caseIdParam,
                campaignId,
                0 as Components.Schemas.CampaignCostType, // Rig upgrading profile
                rigUpgradingProfile,
            )
        }

        if (rigMobDemobProfile) {
            return updateCampaignProfile(
                service,
                projectIdParam,
                caseIdParam,
                campaignId,
                1 as Components.Schemas.CampaignCostType, // Rig mob/demob profile
                rigMobDemobProfile,
            )
        }

        // Handle campaign well updates
        if (wells) {
            return service.updateCampaignWells(
                projectIdParam,
                caseIdParam,
                campaignId,
                wells,
            )
        }

        throw new Error("Invalid campaign mutation parameters")
    }

    const mutation = useBaseMutation({
        resourceName: "campaign",
        getService: GetDrillingCampaignsService,
        updateMethod: "updateCampaign", // Not directly used with custom mutation function
        customMutationFn: campaignMutationFn,
        getResourceFromApiData: () => null, // Not used with custom mutation function
        invalidateQueries: caseId && projectId ? [["caseApiData", projectId, caseId]] : [],
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
