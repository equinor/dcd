import { useMutation, useQueryClient } from "@tanstack/react-query"

import { GetOnshorePowerSupplyService } from "@/Services/OnshorePowerSupplyService"
import { GetDrillingCampaignsService } from "@/Services/DrillingCampaignsService"
import { GetDrainageStrategyService } from "@/Services/DrainageStrategyService"
import { GetSubstructureService } from "@/Services/SubstructureService"
import { GetTransportService } from "@/Services/TransportService"
import { GetTopsideService } from "@/Services/TopsideService"
import { GetSurfService } from "@/Services/SurfService"
import { GetCaseService } from "@/Services/CaseService"
import { useAppStore } from "@/Store/AppStore"
import { createLogger } from "@/Utils/logger"
import { ResourceObject } from "@/Models/Interfaces"
import {
    productionOverrideResources,
    totalStudyCostOverrideResources,
} from "@/Utils/constants"

interface UpdateResourceParams {
    projectId: string;
    caseId: string;
    resourceId?: string;
    resourceObject: ResourceObject;
}

const submitApiLogger = createLogger({
    name: "SUBMIT_API",
    enabled: false, // Enable logging temporarily to debug
})

export const useSubmitToApi = () => {
    const queryClient = useQueryClient()
    const { setSnackBarMessage, setIsCalculatingProductionOverrides, setIsCalculatingTotalStudyCostOverrides } = useAppStore()

    const mutationFn = async ({ serviceMethod }: {
        projectId: string,
        caseId: string,
        resourceId?: string,
        serviceMethod: object,
    }) => serviceMethod

    const mutation = useMutation({
        mutationFn,
        onSuccess: (
            results: any,
            variables,
        ) => {
            const { projectId, caseId } = variables
            queryClient.invalidateQueries(
                { queryKey: ["caseApiData", projectId, caseId] },
            )
        },
        onError: (error: any) => {
            setSnackBarMessage(error.message)
        },
        onSettled: () => {
            setIsCalculatingProductionOverrides(false)
            setIsCalculatingTotalStudyCostOverrides(false)
        },
    })

    const updateResource = async (
        getService: () => any,
        updateMethodName: string,
        params: UpdateResourceParams,
    ) => {
        const service = getService()

        const serviceMethod = params.resourceId
            ? service[updateMethodName](params.projectId, params.caseId, params.resourceId, params.resourceObject)
            : service[updateMethodName](params.projectId, params.caseId, params.resourceObject)

        try {
            const payload: any = {
                projectId: params.projectId,
                caseId: params.caseId,
                serviceMethod,
            }
            if (params.resourceId) {
                payload.resourceId = params.resourceId
            }

            const result = await mutation.mutateAsync(payload)
            return { success: true, data: result }
        } catch (error) {
            return { success: false, error }
        }
    }

    const updateCampaign = (params: UpdateResourceParams) => updateResource(GetDrillingCampaignsService, "updateCampaign", params)

    const updateCampaignWells = (params: UpdateResourceParams) => updateResource(GetDrillingCampaignsService, "updateCampaignWells", params)

    const updateRigUpgradingCost = (params: UpdateResourceParams) => updateResource(GetDrillingCampaignsService, "updateRigUpgradingCost", params)

    const updateRigMobDemobCost = (params: UpdateResourceParams) => updateResource(GetDrillingCampaignsService, "updateRigMobDemobCost", params)

    const updateTopside = (params: UpdateResourceParams) => updateResource(GetTopsideService, "updateTopside", { ...params })

    const updateSurf = (params: UpdateResourceParams) => updateResource(GetSurfService, "updateSurf", { ...params })

    const updateSubstructure = (params: UpdateResourceParams) => updateResource(GetSubstructureService, "updateSubstructure", { ...params })

    const updateTransport = (params: UpdateResourceParams) => updateResource(GetTransportService, "updateTransport", { ...params })

    const updateOnshorePowerSupply = (params: UpdateResourceParams) => updateResource(GetOnshorePowerSupplyService, "updateOnshorePowerSupply", { ...params })

    const updateDrainageStrategy = (params: UpdateResourceParams) => updateResource(GetDrainageStrategyService, "updateDrainageStrategy", { ...params })

    const updateCaseProfiles = (params: UpdateResourceParams) => updateResource(GetCaseService, "saveProfiles", { ...params })

    const updateCase = (params: UpdateResourceParams) => updateResource(GetCaseService, "updateCase", { ...params })

    type SubmitToApiParams = {
        projectId: string,
        caseId: string,
        resourceName: string,
        resourceId?: string,
        resourceObject: ResourceObject,
    }

    const submitToApi = async ({
        projectId,
        caseId,
        resourceName,
        resourceId,
        resourceObject,
    }: SubmitToApiParams): Promise<{ success: boolean; data?: any; error?: any }> => {
        submitApiLogger.log("Submitting to API:", {
            resourceName,
            resourceId,
            resourceObject,
        })

        if (productionOverrideResources.find((x) => x.toString() === resourceName)) {
            setIsCalculatingProductionOverrides(true)
            submitApiLogger.log("Setting production overrides calculation flag")
        }

        if (totalStudyCostOverrideResources.includes(resourceName)) {
            setIsCalculatingTotalStudyCostOverrides(true)
            submitApiLogger.log("Setting total study cost overrides calculation flag")
        }

        if (!["case", "caseProfiles"].includes(resourceName) && !resourceId) {
            submitApiLogger.error("Asset ID is required for this service", null)
            return { success: false, error: new Error("Asset ID is required for this service") }
        }

        try {
            const result = await (async () => {
                switch (resourceName) {
                case "case":
                    return updateCase({
                        projectId, caseId, resourceObject,
                    })

                case "caseProfiles":
                    return updateCaseProfiles({
                        projectId, caseId, resourceObject,
                    })

                case "topside":
                    return updateTopside({
                        projectId, caseId, resourceObject,
                    })

                case "surf":
                    return updateSurf({
                        projectId, caseId, resourceObject,
                    })

                case "substructure":
                    return updateSubstructure({
                        projectId, caseId, resourceObject,
                    })

                case "transport":
                    return updateTransport({
                        projectId, caseId, resourceObject,
                    })

                case "onshorePowerSupply":
                    return updateOnshorePowerSupply({
                        projectId, caseId, resourceObject,
                    })

                case "drainageStrategy":
                    return updateDrainageStrategy({
                        projectId, caseId, resourceObject,
                    })

                case "rigUpgrading":
                    return updateCampaign({
                        projectId,
                        caseId,
                        resourceId: resourceId!,
                        resourceObject: { ...resourceObject, campaignCostType: 0 as Components.Schemas.CampaignCostType },
                    })

                case "rigMobDemob":
                    return updateCampaign({
                        projectId,
                        caseId,
                        resourceId: resourceId!,
                        resourceObject: { ...resourceObject, campaignCostType: 1 as Components.Schemas.CampaignCostType },
                    })
                case "rigUpgradingCost":
                    return updateRigUpgradingCost({
                        projectId,
                        caseId,
                        resourceId: resourceId!,
                        resourceObject: (resourceObject as any).rigUpgradingCost,
                    })
                case "rigMobDemobCost":
                    return updateRigMobDemobCost({
                        projectId,
                        caseId,
                        resourceId: resourceId!,
                        resourceObject: (resourceObject as any).rigMobDemobCost,
                    })
                case "campaignWells":
                    return updateCampaignWells({
                        projectId,
                        caseId,
                        resourceId: resourceId!,
                        resourceObject,
                    })

                default:
                    console.log("Resource name not found", resourceName)
                }

                return { success: false, error: new Error("Service not found") }
            })()

            if (result.success) {
                submitApiLogger.info("Submission successful")
            }

            return result
        } catch (error) {
            submitApiLogger.error("Service not found or error occurred", error)
            return { success: false, error: new Error("Service not found") }
        }
    }

    return {
        submitToApi, updateCase, mutation,
    }
}
