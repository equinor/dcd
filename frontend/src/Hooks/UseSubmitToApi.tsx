import { useMutation, useQueryClient } from "@tanstack/react-query"
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
import { ResourceObject } from "@/Models/Interfaces"
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
        getService: () => any,
        updateMethodName: string,
        {
            projectId,
            caseId,
            resourceObject,
            resourceId,
        }: UpdateResourceParams,
    ) => {
        const service = getService()

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
                                ? await GetExplorationService().createExplorationWellDrillingSchedule(
                                    projectId,
                                    caseId,
                                    resourceId!,
                                    wellId!,
                                    resourceObject as Components.Schemas.CreateTimeSeriesScheduleDto,
                                )
                                : await GetExplorationService().updateExplorationWellDrillingSchedule(
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
                                ? await GetWellProjectService().createWellProjectWellDrillingSchedule(
                                    projectId,
                                    caseId,
                                    resourceId!,
                                    wellId!,
                                    resourceObject as Components.Schemas.CreateTimeSeriesScheduleDto,
                                )
                                : await GetWellProjectService().updateWellProjectWellDrillingSchedule(
                                    projectId,
                                    caseId,
                                    resourceId!,
                                    wellId!,
                                    drillingScheduleId!,
                                    resourceObject as Components.Schemas.UpdateTimeSeriesScheduleDto,
                                ),
                    )
                }

                const ordinaryTimeSeriesTypes = [
                    ProfileTypes.AdditionalProductionProfileGas,
                    ProfileTypes.AdditionalOPEXCostProfile,
                    ProfileTypes.AdditionalProductionProfileOil,
                    ProfileTypes.CessationOnshoreFacilitiesCostProfile,
                    ProfileTypes.CountryOfficeCost,
                    ProfileTypes.HistoricCostCostProfile,
                    ProfileTypes.DeferredGasProduction,
                    ProfileTypes.DeferredOilProduction,
                    ProfileTypes.OnshoreRelatedOPEXCostProfile,
                    ProfileTypes.ProductionProfileGas,
                    ProfileTypes.ProductionProfileOil,
                    ProfileTypes.ProductionProfileWater,
                    ProfileTypes.ProductionProfileWaterInjection,
                    ProfileTypes.ProjectSpecificDrillingCostProfile,
                    ProfileTypes.SeismicAcquisitionAndProcessing,
                    ProfileTypes.TotalOtherStudiesCostProfile,
                ]

                if (ordinaryTimeSeriesTypes.find((x) => x === resourceName)) {
                    return await saveTimeSeriesProfile({
                        projectId,
                        caseId,
                        saveFunction: await GetCaseService().saveProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: resourceName } as Components.Schemas.SaveTimeSeriesDto,
                        ),
                    })
                }

                const overrideTimeSeriesTypes = [
                    ProfileTypes.CessationOffshoreFacilitiesCostOverride,
                    ProfileTypes.CessationWellsCostOverride,
                    ProfileTypes.Co2EmissionsOverride,
                    ProfileTypes.GAndGAdminCostOverride,
                    ProfileTypes.OffshoreFacilitiesOperationsCostProfileOverride,
                    ProfileTypes.OnshorePowerSupplyCostProfileOverride,
                    ProfileTypes.FuelFlaringAndLossesOverride,
                    ProfileTypes.ImportedElectricityOverride,
                    ProfileTypes.NetSalesGasOverride,
                    ProfileTypes.SubstructureCostProfileOverride,
                    ProfileTypes.SurfCostProfileOverride,
                    ProfileTypes.TopsideCostProfileOverride,
                    ProfileTypes.TotalFeasibilityAndConceptStudiesOverride,
                    ProfileTypes.TotalFEEDStudiesOverride,
                    ProfileTypes.TransportCostProfileOverride,
                    ProfileTypes.WellInterventionCostProfileOverride,
                    ProfileTypes.GasInjectorCostProfileOverride,
                    ProfileTypes.GasProducerCostProfileOverride,
                    ProfileTypes.OilProducerCostProfileOverride,
                    ProfileTypes.WaterInjectorCostProfileOverride,
                ]

                if (overrideTimeSeriesTypes.find((x) => x === resourceName)) {
                    return await saveTimeSeriesProfile({
                        projectId,
                        caseId,
                        saveFunction: await GetCaseService().saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: resourceName } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
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
