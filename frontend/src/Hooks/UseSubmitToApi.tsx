import { useMutation, useQueryClient } from "@tanstack/react-query"
import { useState } from "react"
import { createLogger } from "../Utils/logger"

import { GetOnshorePowerSupplyService } from "@/Services/OnshorePowerSupplyService"
import { GetDrainageStrategyService } from "@/Services/DrainageStrategyService"
import { GetSubstructureService } from "@/Services/SubstructureService"
import { GetExplorationService } from "@/Services/ExplorationService"
import { GetWellProjectService } from "@/Services/WellProjectService"
import { GetTransportService } from "@/Services/TransportService"
import { GetTopsideService } from "@/Services/TopsideService"
import { GetSurfService } from "@/Services/SurfService"
import { GetCaseService } from "@/Services/CaseService"
import { useAppStore } from "@/Store/AppStore"
import { ResourceName, ResourceObject } from "@/Models/Interfaces"
import {
    productionOverrideResources,
    totalStudyCostOverrideResources,
} from "@/Utils/constants"
import { ProfileTypes } from "@/Models/enums"

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

        if (productionOverrideResources.find(x => x.toString() === resourceName)) {
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
                    return await updateCase({
                        projectId, caseId, resourceObject,
                    })

                case "topside":
                    return await updateTopside({
                        projectId, caseId, resourceObject,
                    })

                case "surf":
                    return await updateSurf({
                        projectId, caseId, resourceObject,
                    })

                case "substructure":
                    return await updateSubstructure({
                        projectId, caseId, resourceObject,
                    })

                case "transport":
                    return await updateTransport({
                        projectId, caseId, resourceObject,
                    })

                case "onshorePowerSupply":
                    return await updateOnshorePowerSupply({
                        projectId, caseId, resourceObject,
                    })

                case "drainageStrategy":
                    return await updateDrainageStrategy({
                        projectId, caseId, resourceObject,
                    })

                case "explorationWellDrillingSchedule":
                    return await createOrUpdateDrillingSchedule(
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
                    return await createOrUpdateDrillingSchedule(
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
                }

                const profileNameMap = new Map<string, string>([
                    ["additionalOPEXCostProfile", ProfileTypes.AdditionalOPEXCostProfile],
                    ["additionalProductionProfileGas", ProfileTypes.AdditionalProductionProfileGas],
                    ["additionalProductionProfileOil", ProfileTypes.AdditionalProductionProfileOil],
                    ["cessationOnshoreFacilitiesCostProfile", ProfileTypes.CessationOnshoreFacilitiesCostProfile],
                    ["countryOfficeCost", ProfileTypes.CountryOfficeCost],
                    ["deferredGasProduction", ProfileTypes.DeferredGasProduction],
                    ["deferredOilProduction", ProfileTypes.DeferredOilProduction],
                    ["onshoreRelatedOPEXCostProfile", ProfileTypes.OnshoreRelatedOPEXCostProfile],
                    ["productionProfileGas", ProfileTypes.ProductionProfileGas],
                    ["productionProfileOil", ProfileTypes.ProductionProfileOil],
                    ["productionProfileWater", ProfileTypes.ProductionProfileWater],
                    ["productionProfileWaterInjection", ProfileTypes.ProductionProfileWaterInjection],
                    ["projectSpecificDrillingCostProfile", ProfileTypes.ProjectSpecificDrillingCostProfile],
                    ["seismicAcquisitionAndProcessing", ProfileTypes.SeismicAcquisitionAndProcessing],
                    ["totalOtherStudiesCostProfile", ProfileTypes.TotalOtherStudiesCostProfile],

                    [ProfileTypes.AdditionalOPEXCostProfile, ProfileTypes.AdditionalOPEXCostProfile],
                    [ProfileTypes.AdditionalProductionProfileGas, ProfileTypes.AdditionalProductionProfileGas],
                    [ProfileTypes.AdditionalProductionProfileOil, ProfileTypes.AdditionalProductionProfileOil],
                    [ProfileTypes.CessationOnshoreFacilitiesCostProfile, ProfileTypes.CessationOnshoreFacilitiesCostProfile],
                    [ProfileTypes.CountryOfficeCost, ProfileTypes.CountryOfficeCost],
                    [ProfileTypes.DeferredGasProduction, ProfileTypes.DeferredGasProduction],
                    [ProfileTypes.DeferredOilProduction, ProfileTypes.DeferredOilProduction],
                    [ProfileTypes.OnshoreRelatedOPEXCostProfile, ProfileTypes.OnshoreRelatedOPEXCostProfile],
                    [ProfileTypes.ProductionProfileGas, ProfileTypes.ProductionProfileGas],
                    [ProfileTypes.ProductionProfileOil, ProfileTypes.ProductionProfileOil],
                    [ProfileTypes.ProductionProfileWater, ProfileTypes.ProductionProfileWater],
                    [ProfileTypes.ProductionProfileWaterInjection, ProfileTypes.ProductionProfileWaterInjection],
                    [ProfileTypes.ProjectSpecificDrillingCostProfile, ProfileTypes.ProjectSpecificDrillingCostProfile],
                    [ProfileTypes.SeismicAcquisitionAndProcessing, ProfileTypes.SeismicAcquisitionAndProcessing],
                    [ProfileTypes.TotalOtherStudiesCostProfile, ProfileTypes.TotalOtherStudiesCostProfile],
                ])

                if (profileNameMap.has(resourceName)) {
                    return await saveTimeSeriesProfile({
                        projectId,
                        caseId,
                        saveFunction: await (await GetCaseService()).saveProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: profileNameMap.get(resourceName) } as Components.Schemas.SaveTimeSeriesDto,
                        ),
                    })
                }

                const overrideProfileNameMap = new Map<string, string>([
                    ["cessationOffshoreFacilitiesCostOverride", ProfileTypes.CessationOffshoreFacilitiesCostOverride],
                    ["cessationWellsCostOverride", ProfileTypes.CessationWellsCostOverride],
                    ["co2EmissionsOverride", ProfileTypes.Co2EmissionsOverride],
                    ["gAndGAdminCost", ProfileTypes.GAndGAdminCostOverride],
                    ["historicCostCostProfile", ProfileTypes.HistoricCostCostProfile],
                    ["offshoreFacilitiesOperationsCostProfileOverride", ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride],
                    ["onshorePowerSupplyCostOverride", ProfileTypes.OnshorePowerSupplyCostProfileOverride],
                    ["productionProfileFuelFlaringAndLossesOverride", ProfileTypes.FuelFlaringAndLossesOverride],
                    ["productionProfileImportedElectricityOverride", ProfileTypes.ImportedElectricityOverride],
                    ["productionProfileNetSalesGasOverride", ProfileTypes.NetSalesGasOverride],
                    ["substructureCostOverride", ProfileTypes.SubstructureCostProfileOverride],
                    ["surfCostOverride", ProfileTypes.SurfCostProfileOverride],
                    ["topsideCostOverride", ProfileTypes.TopsideCostProfileOverride],
                    ["totalFeasibilityAndConceptStudiesOverride", ProfileTypes.TotalFeasibilityAndConceptStudiesOverride],
                    ["totalFEEDStudiesOverride", ProfileTypes.TotalFEEDStudiesOverride],
                    ["transportCostOverride", ProfileTypes.TransportCostProfileOverride],
                    ["wellInterventionCostProfileOverride", ProfileTypes.WellInterventionCostProfileOverride],
                    ["wellProjectGasInjectorCostOverride", ProfileTypes.GasInjectorCostProfileOverride],
                    ["wellProjectGasProducerCostOverride", ProfileTypes.GasProducerCostProfileOverride],
                    ["wellProjectOilProducerCostOverride", ProfileTypes.OilProducerCostProfileOverride],
                    ["wellProjectWaterInjectorCostOverride", ProfileTypes.WaterInjectorCostProfileOverride],

                    [ProfileTypes.CessationOffshoreFacilitiesCostOverride, ProfileTypes.CessationOffshoreFacilitiesCostOverride],
                    [ProfileTypes.CessationWellsCostOverride, ProfileTypes.CessationWellsCostOverride],
                    [ProfileTypes.Co2EmissionsOverride, ProfileTypes.Co2EmissionsOverride],
                    [ProfileTypes.GAndGAdminCostOverride, ProfileTypes.GAndGAdminCostOverride],
                    [ProfileTypes.HistoricCostCostProfile, ProfileTypes.HistoricCostCostProfile],
                    [ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride, ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride],
                    [ProfileTypes.OnshorePowerSupplyCostProfileOverride, ProfileTypes.OnshorePowerSupplyCostProfileOverride],
                    [ProfileTypes.FuelFlaringAndLossesOverride, ProfileTypes.FuelFlaringAndLossesOverride],
                    [ProfileTypes.ImportedElectricityOverride, ProfileTypes.ImportedElectricityOverride],
                    [ProfileTypes.NetSalesGasOverride, ProfileTypes.NetSalesGasOverride],
                    [ProfileTypes.SubstructureCostProfileOverride, ProfileTypes.SubstructureCostProfileOverride],
                    [ProfileTypes.SurfCostProfileOverride, ProfileTypes.SurfCostProfileOverride],
                    [ProfileTypes.TopsideCostProfileOverride, ProfileTypes.TopsideCostProfileOverride],
                    [ProfileTypes.TotalFeasibilityAndConceptStudiesOverride, ProfileTypes.TotalFeasibilityAndConceptStudiesOverride],
                    [ProfileTypes.TotalFEEDStudiesOverride, ProfileTypes.TotalFEEDStudiesOverride],
                    [ProfileTypes.TransportCostProfileOverride, ProfileTypes.TransportCostProfileOverride],
                    [ProfileTypes.WellInterventionCostProfileOverride, ProfileTypes.WellInterventionCostProfileOverride],
                    [ProfileTypes.GasInjectorCostProfileOverride, ProfileTypes.GasInjectorCostProfileOverride],
                    [ProfileTypes.GasProducerCostProfileOverride, ProfileTypes.GasProducerCostProfileOverride],
                    [ProfileTypes.OilProducerCostProfileOverride, ProfileTypes.OilProducerCostProfileOverride],
                    [ProfileTypes.WaterInjectorCostProfileOverride, ProfileTypes.WaterInjectorCostProfileOverride],
                ])

                if (overrideProfileNameMap.has(resourceName)) {
                    return await saveTimeSeriesProfile({
                        projectId,
                        caseId,
                        saveFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: overrideProfileNameMap.get(resourceName) } as Components.Schemas.SaveTimeSeriesOverrideDto,
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
