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

            return await mutation.mutateAsync(payload)
        } catch (error) {
            return false
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
            return returnValue
        } catch (error) {
            return error
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
            return returnValue
        } catch (error) {
            return error
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
    }: SubmitToApiParams): Promise<any> => {
        if (productionOverrideResources.includes(resourceName)) {
            setIsCalculatingProductionOverrides(true)
        }

        if (totalStudyCostOverrideResources.includes(resourceName)) {
            setIsCalculatingTotalStudyCostOverrides(true)
        }

        if (resourceName !== "case" && !resourceId) {
            console.log("asset id is required for this service")
            throw Error()
        }

        let success = {}
        switch (resourceName) {
        case "case":
            success = await updateCase({
                projectId, caseId, resourceObject,
            })
            break
        case "topside":
            success = await updateTopside({
                projectId, caseId, resourceId, resourceObject,
            })
            break
        case "surf":
            success = await updateSurf({
                projectId, caseId, resourceId, resourceObject,
            })
            break
        case "substructure":
            success = await updateSubstructure({
                projectId, caseId, resourceId, resourceObject,
            })
            break
        case "transport":
            success = await updateTransport({
                projectId, caseId, resourceId, resourceObject,
            })
            break
        case "onshorePowerSupply":
            success = await updateOnshorePowerSupply({
                projectId, caseId, resourceId, resourceObject,
            })
            break
        case "drainageStrategy":
            success = await updateDrainageStrategy({
                projectId, caseId, resourceId, resourceObject,
            })
            break
        case "productionProfileOil":
            success = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createProductionProfileOil(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesVolumeDto,
                    )
                    : await (await GetDrainageStrategyService()).updateProductionProfileOil(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId,
                            resourceObject as Components.Schemas.UpdateTimeSeriesVolumeDto,
                    ),
            })
            break
        case "additionalProductionProfileOil":
            success = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createAdditionalProductionProfileOil(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesVolumeDto,
                    )
                    : await (await GetDrainageStrategyService()).updateAdditionalProductionProfileOil(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesVolumeDto,
                    ),
            })
            break
        case "productionProfileGas":
            success = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createProductionProfileGas(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesVolumeDto,
                    )
                    : await (await GetDrainageStrategyService()).updateProductionProfileGas(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesVolumeDto,
                    ),
            })
            break
        case "additionalProductionProfileGas":
            success = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createAdditionalProductionProfileGas(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesVolumeDto,
                    )
                    : await (await GetDrainageStrategyService()).updateAdditionalProductionProfileGas(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesVolumeDto,
                    ),
            })
            break
        case "productionProfileWater":
            success = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createProductionProfileWater(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesVolumeDto,
                    )
                    : await (await GetDrainageStrategyService()).updateProductionProfileWater(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesVolumeDto,
                    ),
            })
            break
        case "productionProfileWaterInjection":
            success = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createProductionProfileWaterInjection(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesVolumeDto,
                    )
                    : await (await GetDrainageStrategyService()).updateProductionProfileWaterInjection(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesVolumeDto,
                    ),
            })
            break
        case "productionProfileFuelFlaringAndLossesOverride":
            success = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createProductionProfileFuelFlaringAndLossesOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesVolumeOverrideDto,
                    )
                    : await (await GetDrainageStrategyService()).updateProductionProfileFuelFlaringAndLossesOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesVolumeOverrideDto,
                    ),
            })
            break
        case "productionProfileNetSalesGasOverride":
            success = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createProductionProfileNetSalesGasOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesVolumeOverrideDto,
                    )
                    : await (await GetDrainageStrategyService()).updateProductionProfileNetSalesGasOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesVolumeOverrideDto,
                    ),
            })
            break
        case "productionProfileImportedElectricityOverride":
            success = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createProductionProfileImportedElectricityOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesEnergyDto,
                    )
                    : await (await GetDrainageStrategyService()).updateProductionProfileImportedElectricityOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesEnergyOverrideDto,
                    ),
            })
            break
        case "deferredOilProduction":
            success = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createDeferredOilProduction(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesVolumeDto,
                    )
                    : await (await GetDrainageStrategyService()).updateDeferredOilProduction(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesVolumeDto,
                    ),
            })
            break
        case "deferredGasProduction":
            success = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createDeferredGasProduction(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesVolumeDto,
                    )
                    : await (await GetDrainageStrategyService()).updateDeferredGasProduction(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesVolumeDto,
                    ),
            })
            break
        case "totalFeasibilityAndConceptStudiesOverride":
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
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
            success = await createOrUpdateTimeSeriesProfile({
                projectId,
                caseId,
                resourceId,
                resourceProfileId,
                createOrUpdateFunction: !resourceProfileId
                    ? await (await GetDrainageStrategyService()).createProductionProfileCo2EmissionsOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTimeSeriesMassOverrideDto,
                    )
                    : await (await GetDrainageStrategyService()).updateProductionProfileCo2EmissionsOverride(
                        projectId,
                        caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTimeSeriesMassOverrideDto,
                    ),
            })
            break
        case "explorationWellDrillingSchedule":
            success = await createOrUpdateDrillingSchedule(
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
            success = await createOrUpdateDrillingSchedule(
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
        }
        return success
    }

    return { submitToApi, updateCase, mutation }
}
