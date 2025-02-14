import { useMutation, useQueryClient } from "@tanstack/react-query"
import { useState } from "react"

import { GetOnshorePowerSupplyService } from "@/Services/OnshorePowerSupplyService"
import { GetDrillingCampaignsService } from "@/Services/DrillingCampaignsService"
import { GetDrainageStrategyService } from "@/Services/DrainageStrategyService"
import { GetSubstructureService } from "@/Services/SubstructureService"
import { GetExplorationService } from "@/Services/ExplorationService"
import { GetWellProjectService } from "@/Services/WellProjectService"
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
import { ordinaryTimeSeriesTypes, overrideTimeSeriesTypes } from "@/Utils/TimeseriesTypes"

interface UpdateResourceParams {
    projectId: string;
    caseId: string;
    resourceObject: ResourceObject;
    resourceId?: string;
}

interface SubmissionHistoryEntry {
    before: {
        projectId: string;
        caseId: string;
        resourceName: string;
        resourceId?: string;
        resourceObject: ResourceObject;
        wellId?: string;
        drillingScheduleId?: string;
    };
    after: {
        data: any;
    };
}

const submitApiLogger = createLogger({
    name: "SUBMIT_API",
    enabled: true, // Enable logging temporarily to debug
})

export const useSubmitToApi = () => {
    const queryClient = useQueryClient()
    const { setSnackBarMessage, setIsCalculatingProductionOverrides, setIsCalculatingTotalStudyCostOverrides } = useAppStore()
    const [submissionHistory, setSubmissionHistory] = useState<SubmissionHistoryEntry[]>([])

    const mutationFn = async ({ serviceMethod }: {
        projectId: string,
        caseId: string,
        resourceId?: string,
        wellId?: string,
        drillingScheduleId?: string,
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
        getService: () => Promise<any>,
        updateMethodName: string,
        {
            projectId,
            caseId,
            resourceObject,
            resourceId,
        }: UpdateResourceParams,

    ) => {
        const service = await getService()

        const serviceMethod = resourceId
            ? service[updateMethodName](projectId, caseId, resourceId, resourceObject)
            : service[updateMethodName](projectId, caseId, resourceObject) // for case

        try {
            const payload: any = { projectId, caseId, serviceMethod }
            if (resourceId) { payload.resourceId = resourceId }

            const result = await mutation.mutateAsync(payload)
            return { success: true, data: result }
        } catch (error) {
            return { success: false, error }
        }
    }

    const updateCampaign = (params: UpdateResourceParams) => updateResource(GetDrillingCampaignsService, "updateCampaign", { ...params })

    const updateTopside = (params: UpdateResourceParams) => updateResource(GetTopsideService, "updateTopside", { ...params })

    const updateSurf = (params: UpdateResourceParams) => updateResource(GetSurfService, "updateSurf", { ...params })

    const updateSubstructure = (params: UpdateResourceParams) => updateResource(GetSubstructureService, "updateSubstructure", { ...params })

    const updateTransport = (params: UpdateResourceParams) => updateResource(GetTransportService, "updateTransport", { ...params })

    const updateOnshorePowerSupply = (params: UpdateResourceParams) => updateResource(GetOnshorePowerSupplyService, "updateOnshorePowerSupply", { ...params })

    const updateDrainageStrategy = (params: UpdateResourceParams) => updateResource(GetDrainageStrategyService, "updateDrainageStrategy", { ...params })

    const updateCase = (params: UpdateResourceParams) => updateResource(GetCaseService, "updateCase", { ...params })

    interface SaveTimeSeriesProfileParams {
        projectId: string;
        caseId: string;
        saveFunction: any;
    }
    const saveTimeSeriesProfile = async ({
        projectId,
        caseId,
        saveFunction,
    }: SaveTimeSeriesProfileParams) => {
        try {
            const result = await mutation.mutateAsync({
                projectId,
                caseId,
                serviceMethod: saveFunction,
            })
            const returnValue = { ...result }
            return { success: true, data: returnValue }
        } catch (error) {
            return { success: false, error }
        }
    }

    const createOrUpdateDrillingSchedule = async (
        projectId: string,
        caseId: string,
        assetId: string,
        wellId: string,
        drillingScheduleId: string,
        createOrUpdateFunction: any,

    ) => {
        try {
            const result = await mutation.mutateAsync({
                projectId,
                caseId,
                resourceId: assetId,
                wellId,
                drillingScheduleId,
                serviceMethod: createOrUpdateFunction,
            })
            const returnValue = { ...result }
            return { success: true, data: returnValue }
        } catch (error) {
            return { success: false, error }
        }
    }

    type SubmitToApiParams = {
        projectId: string,
        caseId: string,
        resourceName: string,
        resourceId?: string,
        resourceObject: ResourceObject,
        wellId?: string,
        drillingScheduleId?: string,
    }

    const submitToApi = async ({
        projectId,
        caseId,
        resourceName,
        resourceId,
        resourceObject,
        wellId,
        drillingScheduleId,
    }: SubmitToApiParams): Promise<{ success: boolean; data?: any; error?: any }> => {
        submitApiLogger.warn("Submitting to API:", {
            resourceName,
            resourceId,
            resourceObject,
        })

        // Create the before state
        const beforeState = {
            projectId,
            caseId,
            resourceName,
            resourceId,
            resourceObject,
            wellId,
            drillingScheduleId,
        }

        if (productionOverrideResources.find((x) => x.toString() === resourceName)) {
            setIsCalculatingProductionOverrides(true)
            submitApiLogger.warn("Setting production overrides calculation flag")
        }

        if (totalStudyCostOverrideResources.includes(resourceName)) {
            setIsCalculatingTotalStudyCostOverrides(true)
            submitApiLogger.warn("Setting total study cost overrides calculation flag")
        }

        if (resourceName !== "case" && !resourceId) {
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

                case "explorationWellDrillingSchedule":
                    return createOrUpdateDrillingSchedule(
                        projectId,
                        caseId,
                            resourceId!,
                            wellId!,
                            drillingScheduleId!,
                            !drillingScheduleId
                                ? await (await GetExplorationService()).createExplorationWellDrillingSchedule(
                                    projectId,
                                    caseId,
                                    resourceId!,
                                    wellId!,
                                    resourceObject as Components.Schemas.CreateTimeSeriesScheduleDto,
                                )
                                : await (await GetExplorationService()).updateExplorationWellDrillingSchedule(
                                    projectId,
                                    caseId,
                                    resourceId!,
                                    wellId!,
                                    drillingScheduleId!,
                                    resourceObject as Components.Schemas.UpdateTimeSeriesScheduleDto,
                                ),
                    )

                case "developmentWellDrillingSchedule":
                    return createOrUpdateDrillingSchedule(
                        projectId,
                        caseId,
                            resourceId!,
                            wellId!,
                            drillingScheduleId!,
                            !drillingScheduleId
                                ? await (await GetWellProjectService()).createWellProjectWellDrillingSchedule(
                                    projectId,
                                    caseId,
                                    resourceId!,
                                    wellId!,
                                    resourceObject as Components.Schemas.CreateTimeSeriesScheduleDto,
                                )
                                : await (await GetWellProjectService()).updateWellProjectWellDrillingSchedule(
                                    projectId,
                                    caseId,
                                    resourceId!,
                                    wellId!,
                                    drillingScheduleId!,
                                    resourceObject as Components.Schemas.UpdateTimeSeriesScheduleDto,
                                ),
                    )

                case "campaign":
                    return updateCampaign({
                        projectId, caseId, resourceId, resourceObject,
                    })

                default:
                    console.log("Resource name not found", resourceName)
                }

                if (ordinaryTimeSeriesTypes.find((x) => x === resourceName)) {
                    return saveTimeSeriesProfile({
                        projectId,
                        caseId,
                        saveFunction: await (await GetCaseService()).saveProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: resourceName } as Components.Schemas.SaveTimeSeriesDto,
                        ),
                    })
                }

                if (overrideTimeSeriesTypes.find((x) => x === resourceName)) {
                    return saveTimeSeriesProfile({
                        projectId,
                        caseId,
                        saveFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: resourceName } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                }

                return { success: false, error: new Error("Service not found") }
            })()

            if (result.success) {
                submitApiLogger.info("Submission successful, updating history. Current history length:", submissionHistory.length)
                // Add to history only if submission was successful
                setSubmissionHistory((prev) => {
                    const newHistory = [...prev, {
                        before: beforeState,
                        after: { data: result.data },
                    }]
                    submitApiLogger.info("New history length:", newHistory.length)
                    return newHistory
                })
            }

            return result
        } catch (error) {
            submitApiLogger.error("Service not found or error occurred", error)
            return { success: false, error: new Error("Service not found") }
        }
    }

    const getSubmissionHistory = () => {
        submitApiLogger.info("Getting submission history, current length:", submissionHistory.length)
        return submissionHistory
    }

    const clearSubmissionHistory = () => {
        submitApiLogger.info("Clearing submission history")
        setSubmissionHistory([])
    }

    return {
        submitToApi, updateCase, mutation, getSubmissionHistory, clearSubmissionHistory,
    }
}
