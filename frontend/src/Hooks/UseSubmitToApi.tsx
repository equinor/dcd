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
import { useAppContext } from "@/Context/AppContext"
import { ResourceObject } from "@/Models/Interfaces"
import {
    productionOverrideResources,
    totalStudyCostOverrideResources,
} from "@/Utils/constants"
import { GetDrillingCampaignsService } from "@/Services/DrillingCampaignsService"

interface UpdateResourceParams {
    projectId: string;
    caseId: string;
    resourceObject: ResourceObject;
    resourceId?: string;
}

const submitApiLogger = createLogger({
    name: "SUBMIT_API",
    enabled: false, // Set to true to enable debug logging. Don't leave this on for production
})

export const useSubmitToApi = () => {
    const queryClient = useQueryClient()
    const { setSnackBarMessage, setIsCalculatingProductionOverrides, setIsCalculatingTotalStudyCostOverrides } = useAppContext()

    const mutationFn = async ({ serviceMethod }: {
        projectId: string,
        caseId: string,
        resourceId?: string,
        resourceProfileId?: string,
        wellId?: string,
        campaignId?: string,
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
            console.error("Failed to update data:", error)
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

    interface CreateOrUpdateTimeSeriesProfileParams {
        projectId: string;
        caseId: string;
        resourceId: string | undefined;
        resourceProfileId: string | undefined;
        createOrUpdateFunction: any;
    }
    const createOrUpdateTimeSeriesProfile = async ({
        projectId,
        caseId,
        resourceId,
        resourceProfileId,
        createOrUpdateFunction,
    }: CreateOrUpdateTimeSeriesProfileParams) => {
        try {
            const result = await mutation.mutateAsync({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                serviceMethod: createOrUpdateFunction,
            })
            const returnValue = { ...result, resourceProfileId: result.id }
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
            const returnValue = { ...result, resourceProfileId: result.id }
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
        resourceProfileId?: string,
        wellId?: string,
        drillingScheduleId?: string,
    }

    const submitToApi = async ({
        projectId,
        caseId,
        resourceName,
        resourceId,
        resourceObject,
        resourceProfileId,
        wellId,
        drillingScheduleId,
    }: SubmitToApiParams): Promise<{ success: boolean; data?: any; error?: any }> => {
        submitApiLogger.warn("Submitting to API:", {
            resourceName,
            resourceId,
            resourceObject,
        })

        if (productionOverrideResources.includes(resourceName)) {
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

        let result
        try {
            switch (resourceName) {
                case "case":
                    result = await updateCase({
                        projectId, caseId, resourceObject,
                    })
                    break
                case "topside":
                    result = await updateTopside({
                        projectId, caseId, resourceId, resourceObject,
                    })
                    break
                case "surf":
                    result = await updateSurf({
                        projectId, caseId, resourceId, resourceObject,
                    })
                    break
                case "substructure":
                    result = await updateSubstructure({
                        projectId, caseId, resourceId, resourceObject,
                    })
                    break
                case "transport":
                    result = await updateTransport({
                        projectId, caseId, resourceId, resourceObject,
                    })
                    break
                case "onshorePowerSupply":
                    result = await updateOnshorePowerSupply({
                        projectId, caseId, resourceId, resourceObject,
                    })
                    break
                case "campaign":
                    result = await updateCampaign({
                        projectId, caseId, resourceId, resourceObject,
                    })
                    break
                case "drainageStrategy":
                    result = await updateDrainageStrategy({
                        projectId, caseId, resourceId, resourceObject,
                    })
                    break
                case "productionProfileOil":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "ProductionProfileOil" } as Components.Schemas.SaveTimeSeriesDto,
                        ),
                    })
                    break
                case "additionalProductionProfileOil":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "AdditionalProductionProfileOil" } as Components.Schemas.SaveTimeSeriesDto,
                        ),
                    })
                    break
                case "productionProfileGas":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "ProductionProfileGas" } as Components.Schemas.SaveTimeSeriesDto,
                        ),
                    })
                    break
                case "additionalProductionProfileGas":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "AdditionalProductionProfileGas" } as Components.Schemas.SaveTimeSeriesDto,
                        ),
                    })
                    break
                case "productionProfileWater":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "ProductionProfileWater" } as Components.Schemas.SaveTimeSeriesDto,
                        ),
                    })
                    break
                case "productionProfileWaterInjection":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "ProductionProfileWaterInjection" } as Components.Schemas.SaveTimeSeriesDto,
                        ),
                    })
                    break
                case "productionProfileFuelFlaringAndLossesOverride":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "FuelFlaringAndLossesOverride" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "productionProfileNetSalesGasOverride":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "NetSalesGasOverride" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "productionProfileImportedElectricityOverride":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "ImportedElectricityOverride" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "deferredOilProduction":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "DeferredOilProduction" } as Components.Schemas.SaveTimeSeriesDto,
                        ),
                    })
                    break
                case "deferredGasProduction":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "DeferredGasProduction" } as Components.Schemas.SaveTimeSeriesDto,
                        ),
                    })
                    break
                case "totalFeasibilityAndConceptStudiesOverride":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "TotalFeasibilityAndConceptStudiesOverride" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "totalFEEDStudiesOverride":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "TotalFEEDStudiesOverride" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "totalOtherStudiesCostProfile":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "TotalOtherStudiesCostProfile" } as Components.Schemas.SaveTimeSeriesDto,
                        ),
                    })
                    break
                case "historicCostCostProfile":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "HistoricCostCostProfile" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "wellInterventionCostProfileOverride":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "WellInterventionCostProfileOverride" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "offshoreFacilitiesOperationsCostProfileOverride":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: !await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "OffshoreFacilitiesOperationsCostProfileOverride" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "onshoreRelatedOPEXCostProfile":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "OnshoreRelatedOPEXCostProfile" } as Components.Schemas.SaveTimeSeriesDto,
                        ),
                    })
                    break
                case "additionalOPEXCostProfile":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "AdditionalOPEXCostProfile" } as Components.Schemas.SaveTimeSeriesDto,
                        ),
                    })
                    break
                case "cessationWellsCostOverride":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "CessationWellsCostOverride" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "cessationOffshoreFacilitiesCostOverride":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "CessationOffshoreFacilitiesCostOverride" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "cessationOnshoreFacilitiesCostProfile":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "CessationOnshoreFacilitiesCostProfile" } as Components.Schemas.SaveTimeSeriesDto,
                        ),
                    })
                    break
                case "surfCostOverride":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "SurfCostProfileOverride" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "topsideCostOverride":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "TopsideCostProfileOverride" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "substructureCostOverride":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "SubstructureCostProfileOverride" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "transportCostOverride":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "TransportCostProfileOverride" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "onshorePowerSupplyCostOverride":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "OnshorePowerSupplyCostProfileOverride" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "wellProjectOilProducerCostOverride":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "OilProducerCostProfileOverride" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "wellProjectGasProducerCostOverride":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "GasProducerCostProfileOverride" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "wellProjectWaterInjectorCostOverride":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "WaterInjectorCostProfileOverride" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "wellProjectGasInjectorCostOverride":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "GasInjectorCostProfileOverride" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "gAndGAdminCost":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "GAndGAdminCostOverride" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "seismicAcquisitionAndProcessing":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "SeismicAcquisitionAndProcessing" } as Components.Schemas.SaveTimeSeriesDto,
                        ),
                    })
                    break
                case "countryOfficeCost":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "CountryOfficeCost" } as Components.Schemas.SaveTimeSeriesDto,
                        ),
                    })
                    break
                case "co2EmissionsOverride":
                    result = await createOrUpdateTimeSeriesProfile({
                        projectId,
                        caseId,
                        resourceId,
                        resourceProfileId,
                        createOrUpdateFunction: await (await GetCaseService()).saveOverrideProfile(
                            projectId,
                            caseId,
                            { ...resourceObject, profileType: "Co2EmissionsOverride" } as Components.Schemas.SaveTimeSeriesOverrideDto,
                        ),
                    })
                    break
                case "explorationWellDrillingSchedule":
                    result = await createOrUpdateDrillingSchedule(
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
                    break
                case "developmentWellDrillingSchedule":
                    result = await createOrUpdateDrillingSchedule(
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

                    break
                default:
                    console.log("Service not found")
                    return { success: false, error: new Error("Service not found") }
            }
            submitApiLogger.warn("API submission successful:", result)
            return result
        } catch (error) {
            submitApiLogger.error("Service not found or error occurred", error)
            return { success: false, error: new Error("Service not found") }
        }
    }

    return { submitToApi, updateCase, mutation }
}
