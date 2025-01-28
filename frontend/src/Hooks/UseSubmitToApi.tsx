import { useMutation, useQueryClient } from "@tanstack/react-query"

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

interface UpdateResourceParams {
    projectId: string;
    caseId: string;
    resourceObject: ResourceObject;
    resourceId?: string;
}

export const useSubmitToApi = () => {
    const queryClient = useQueryClient()
    const { setSnackBarMessage, setIsCalculatingProductionOverrides, setIsCalculatingTotalStudyCostOverrides } = useAppContext()

    const mutationFn = async ({ serviceMethod }: {
        projectId: string,
        caseId: string,
        resourceId?: string,
        resourceProfileId?: string,
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
        if (productionOverrideResources.includes(resourceName)) {
            setIsCalculatingProductionOverrides(true)
        }

        if (totalStudyCostOverrideResources.includes(resourceName)) {
            setIsCalculatingTotalStudyCostOverrides(true)
        }

        if (resourceName !== "case" && !resourceId) {
            console.log("asset id is required for this service")
            return { success: false, error: new Error("Asset ID is required for this service") }
        }

        let result
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
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createProductionProfileOil(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostDto,
                    )
                    : await (await GetDrainageStrategyService()).updateProductionProfileOil(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostDto,
                    ),
            })
            break
        case "additionalProductionProfileOil":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createAdditionalProductionProfileOil(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostDto,
                    )
                    : await (await GetDrainageStrategyService()).updateAdditionalProductionProfileOil(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostDto,
                    ),
            })
            break
        case "productionProfileGas":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createProductionProfileGas(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostDto,
                    )
                    : await (await GetDrainageStrategyService()).updateProductionProfileGas(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostDto,
                    ),
            })
            break
        case "additionalProductionProfileGas":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createAdditionalProductionProfileGas(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostDto,
                    )
                    : await (await GetDrainageStrategyService()).updateAdditionalProductionProfileGas(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostDto,
                    ),
            })
            break
        case "productionProfileWater":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createProductionProfileWater(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostDto,
                    )
                    : await (await GetDrainageStrategyService()).updateProductionProfileWater(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostDto,
                    ),
            })
            break
        case "productionProfileWaterInjection":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createProductionProfileWaterInjection(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostDto,
                    )
                    : await (await GetDrainageStrategyService()).updateProductionProfileWaterInjection(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostDto,
                    ),
            })
            break
        case "productionProfileFuelFlaringAndLossesOverride":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createProductionProfileFuelFlaringAndLossesOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostOverrideDto,
                    )
                    : await (await GetDrainageStrategyService()).updateProductionProfileFuelFlaringAndLossesOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
                    ),
            })
            break
        case "productionProfileNetSalesGasOverride":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createProductionProfileNetSalesGasOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostOverrideDto,
                    )
                    : await (await GetDrainageStrategyService()).updateProductionProfileNetSalesGasOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
                    ),
            })
            break
        case "productionProfileImportedElectricityOverride":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createProductionProfileImportedElectricityOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostDto,
                    )
                    : await (await GetDrainageStrategyService()).updateProductionProfileImportedElectricityOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
                    ),
            })
            break
        case "deferredOilProduction":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createDeferredOilProduction(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostDto,
                    )
                    : await (await GetDrainageStrategyService()).updateDeferredOilProduction(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostDto,
                    ),
            })
            break
        case "deferredGasProduction":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createDeferredGasProduction(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostDto,
                    )
                    : await (await GetDrainageStrategyService()).updateDeferredGasProduction(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostDto,
                    ),
            })
            break
        case "totalFeasibilityAndConceptStudiesOverride":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetCaseService()).createTotalFeasibilityAndConceptStudiesOverride(
                        projectId,
                        caseId,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostOverrideDto,
                    )
                    : await (await GetCaseService()).updateTotalFeasibilityAndConceptStudiesOverride(
                        projectId,
                        caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
                    ),
            })
            break
        case "totalFEEDStudiesOverride":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetCaseService()).createTotalFEEDStudiesOverride(
                        projectId,
                        caseId,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostOverrideDto,
                    )
                    : await (await GetCaseService()).updateTotalFEEDStudiesOverride(
                        projectId,
                        caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
                    ),
            })
            break
        case "totalOtherStudiesCostProfile":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetCaseService()).createTotalOtherStudiesCostProfile(
                        projectId,
                        caseId,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostDto,
                    )
                    : await (await GetCaseService()).updateTotalOtherStudiesCostProfile(
                        projectId,
                        caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
                    ),
            })
            break
        case "historicCostCostProfile":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetCaseService()).createHistoricCostCostProfile(
                        projectId,
                        caseId,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostDto,
                    )
                    : await (await GetCaseService()).updateHistoricCostCostProfile(
                        projectId,
                        caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostDto,
                    ),
            })
            break
        case "wellInterventionCostProfileOverride":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetCaseService()).createWellInterventionCostProfileOverride(
                        projectId,
                        caseId,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostOverrideDto,
                    )
                    : await (await GetCaseService()).updateWellInterventionCostProfileOverride(
                        projectId,
                        caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
                    ),
            })
            break
        case "offshoreFacilitiesOperationsCostProfileOverride":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetCaseService()).createOffshoreFacilitiesOperationsCostProfileOverride(
                        projectId,
                        caseId,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostOverrideDto,
                    )
                    : await (await GetCaseService()).updateOffshoreFacilitiesOperationsCostProfileOverride(
                        projectId,
                        caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
                    ),
            })
            break
        case "onshoreRelatedOPEXCostProfile":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetCaseService()).createOnshoreRelatedOPEXCostProfile(
                        projectId,
                        caseId,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostDto,
                    )
                    : await (await GetCaseService()).updateOnshoreRelatedOPEXCostProfile(
                        projectId,
                        caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostDto,
                    ),
            })
            break
        case "additionalOPEXCostProfile":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetCaseService()).createAdditionalOPEXCostProfile(
                        projectId,
                        caseId,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostDto,
                    )
                    : await (await GetCaseService()).updateAdditionalOPEXCostProfile(
                        projectId,
                        caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostDto,
                    ),
            })
            break
        case "cessationWellsCostOverride":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetCaseService()).createCessationWellsCostOverride(
                        projectId,
                        caseId,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostOverrideDto,
                    )
                    : await (await GetCaseService()).updateCessationWellsCostOverride(
                        projectId,
                        caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
                    ),
            })
            break
        case "cessationOffshoreFacilitiesCostOverride":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetCaseService()).createCessationOffshoreFacilitiesCostOverride(
                        projectId,
                        caseId,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostOverrideDto,
                    )
                    : await (await GetCaseService()).updateCessationOffshoreFacilitiesCostOverride(
                        projectId,
                        caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
                    ),
            })
            break
        case "cessationOnshoreFacilitiesCostProfile":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetCaseService()).createCessationOnshoreFacilitiesCostProfile(
                        projectId,
                        caseId,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostDto,
                    )
                    : await (await GetCaseService()).updateCessationOnshoreFacilitiesCostProfile(
                        projectId,
                        caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostDto,
                    ),
            })
            break
        case "surfCostOverride":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetSurfService()).createSurfCostProfileOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostOverrideDto,
                    )
                    : await (await GetSurfService()).updateSurfCostProfileOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
                    ),
            })
            break
        case "topsideCostOverride":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetTopsideService()).createTopsideCostProfileOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostOverrideDto,
                    )
                    : await (await GetTopsideService()).updateTopsideCostProfileOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
                    ),
            })
            break
        case "substructureCostOverride":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetSubstructureService()).createSubstructureCostProfileOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostOverrideDto,
                    )
                    : await (await GetSubstructureService()).updateSubstructureCostProfileOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
                    ),
            })
            break
        case "transportCostOverride":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetTransportService()).createTransportCostProfileOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostOverrideDto,
                    )
                    : await (await GetTransportService()).updateTransportCostProfileOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
                    ),
            })
            break
        case "onshorePowerSupplyCostOverride":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetOnshorePowerSupplyService()).createOnshorePowerSupplyCostProfileOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostOverrideDto,
                    )
                    : await (await GetOnshorePowerSupplyService()).updateOnshorePowerSupplyCostProfileOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
                    ),
            })
            break
        case "wellProjectOilProducerCostOverride":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetWellProjectService()).createOilProducerCostProfileOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostOverrideDto,
                    )
                    : await (await GetWellProjectService()).updateOilProducerCostProfileOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
                    ),
            })
            break
        case "wellProjectGasProducerCostOverride":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetWellProjectService()).createGasProducerCostProfileOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostOverrideDto,
                    )
                    : await (await GetWellProjectService()).updateGasProducerCostProfileOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
                    ),
            })
            break
        case "wellProjectWaterInjectorCostOverride":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetWellProjectService()).createWaterInjectorCostProfileOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostOverrideDto,
                    )
                    : await (await GetWellProjectService()).updateWaterInjectorCostProfileOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
                    ),
            })
            break
        case "wellProjectGasInjectorCostOverride":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetWellProjectService()).createGasInjectorCostProfileOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostOverrideDto,
                    )
                    : await (await GetWellProjectService()).updateGasInjectorCostProfileOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
                    ),
            })
            break
        case "gAndGAdminCost":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetExplorationService()).createGAndGAdminCostOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostOverrideDto,
                    )
                    : await (await GetExplorationService()).updateGAndGAdminCostOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
                    ),
            })
            break
        case "seismicAcquisitionAndProcessing":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetExplorationService()).createSeismicAcquisitionAndProcessing(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostDto,
                    )
                    : await (await GetExplorationService()).updateSeismicAcquisitionAndProcessing(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostDto,
                    ),
            })
            break
        case "countryOfficeCost":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetExplorationService()).createCountryOfficeCost(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostDto,
                    )
                    : await (await GetExplorationService()).updateCountryOfficeCost(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostDto,
                    ),
            })
            break
        case "co2EmissionsOverride":
            result = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createProductionProfileCo2EmissionsOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesCostOverrideDto,
                    )
                    : await (await GetDrainageStrategyService()).updateProductionProfileCo2EmissionsOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesCostOverrideDto,
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
        case "wellProjectWellDrillingSchedule":
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
        return result
    }

    return { submitToApi, updateCase, mutation }
}
