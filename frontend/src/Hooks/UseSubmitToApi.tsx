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
import { GetOrgChartMembersService } from "@/Services/OrgChartMembersService"

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
                    ? await (await GetCaseService()).createProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "ProductionProfileOil" } as Components.Schemas.CreateTimeSeriesDto,
                    )
                    : await (await GetCaseService()).updateProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "ProductionProfileOil" } as Components.Schemas.UpdateTimeSeriesDto,
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
                    ? await (await GetCaseService()).createProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "AdditionalProductionProfileOil" } as Components.Schemas.CreateTimeSeriesDto,
                    )
                    : await (await GetCaseService()).updateProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "AdditionalProductionProfileOil" } as Components.Schemas.UpdateTimeSeriesDto,
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
                    ? await (await GetCaseService()).createProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "ProductionProfileGas" } as Components.Schemas.CreateTimeSeriesDto,
                    )
                    : await (await GetCaseService()).updateProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "ProductionProfileGas" } as Components.Schemas.UpdateTimeSeriesDto,
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
                    ? await (await GetCaseService()).createProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "AdditionalProductionProfileGas" } as Components.Schemas.CreateTimeSeriesDto,
                    )
                    : await (await GetCaseService()).updateProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "AdditionalProductionProfileGas" } as Components.Schemas.UpdateTimeSeriesDto,
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
                    ? await (await GetCaseService()).createProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "ProductionProfileWater" } as Components.Schemas.CreateTimeSeriesDto,
                    )
                    : await (await GetCaseService()).updateProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "ProductionProfileWater" } as Components.Schemas.UpdateTimeSeriesDto,
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
                    ? await (await GetCaseService()).createProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "ProductionProfileWaterInjection" } as Components.Schemas.CreateTimeSeriesDto,
                    )
                    : await (await GetCaseService()).updateProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "ProductionProfileWaterInjection" } as Components.Schemas.UpdateTimeSeriesDto,
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "NetSalesGasOverride" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "NetSalesGasOverride" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "NetSalesGasOverride" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "NetSalesGasOverride" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "ImportedElectricityOverride" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "ImportedElectricityOverride" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
                    ? await (await GetCaseService()).createProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "DeferredOilProduction" } as Components.Schemas.CreateTimeSeriesDto,
                    )
                    : await (await GetCaseService()).updateProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "DeferredOilProduction" } as Components.Schemas.UpdateTimeSeriesDto,
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
                    ? await (await GetCaseService()).createProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "DeferredGasProduction" } as Components.Schemas.CreateTimeSeriesDto,
                    )
                    : await (await GetCaseService()).updateProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "DeferredGasProduction" } as Components.Schemas.UpdateTimeSeriesDto,
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "TotalFeasibilityAndConceptStudiesOverride" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "TotalFeasibilityAndConceptStudiesOverride" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "TotalFEEDStudiesOverride" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "TotalFEEDStudiesOverride" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
                    ? await (await GetCaseService()).createProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "TotalOtherStudiesCostProfile" } as Components.Schemas.CreateTimeSeriesDto,
                    )
                    : await (await GetCaseService()).updateProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "TotalOtherStudiesCostProfile" } as Components.Schemas.UpdateTimeSeriesDto,
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "HistoricCostCostProfile" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "HistoricCostCostProfile" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "WellInterventionCostProfileOverride" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "WellInterventionCostProfileOverride" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "OffshoreFacilitiesOperationsCostProfileOverride" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "OffshoreFacilitiesOperationsCostProfileOverride" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
                    ? await (await GetCaseService()).createProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "OnshoreRelatedOPEXCostProfile" } as Components.Schemas.CreateTimeSeriesDto,
                    )
                    : await (await GetCaseService()).updateProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "OnshoreRelatedOPEXCostProfile" } as Components.Schemas.UpdateTimeSeriesDto,
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
                    ? await (await GetCaseService()).createProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "AdditionalOPEXCostProfile" } as Components.Schemas.CreateTimeSeriesDto,
                    )
                    : await (await GetCaseService()).updateProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "AdditionalOPEXCostProfile" } as Components.Schemas.UpdateTimeSeriesDto,
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "CessationWellsCostOverride" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "CessationWellsCostOverride" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "CessationOffshoreFacilitiesCostOverride" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "CessationOffshoreFacilitiesCostOverride" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
                    ? await (await GetCaseService()).createProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "CessationOnshoreFacilitiesCostProfile" } as Components.Schemas.CreateTimeSeriesDto,
                    )
                    : await (await GetCaseService()).updateProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "CessationOnshoreFacilitiesCostProfile" } as Components.Schemas.UpdateTimeSeriesDto,
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "SurfCostProfileOverride" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "SurfCostProfileOverride" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "TopsideCostProfileOverride" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "TopsideCostProfileOverride" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "SubstructureCostProfileOverride" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "SubstructureCostProfileOverride" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "TransportCostProfileOverride" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "TransportCostProfileOverride" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "OnshorePowerSupplyCostProfileOverride" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "OnshorePowerSupplyCostProfileOverride" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "OilProducerCostProfileOverride" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "OilProducerCostProfileOverride" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "GasProducerCostProfileOverride" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "GasProducerCostProfileOverride" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "WaterInjectorCostProfileOverride" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "WaterInjectorCostProfileOverride" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "GasInjectorCostProfileOverride" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "GasInjectorCostProfileOverride" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "GAndGAdminCostOverride" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "GAndGAdminCostOverride" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
                    ? await (await GetCaseService()).createProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "SeismicAcquisitionAndProcessing" } as Components.Schemas.CreateTimeSeriesDto
                    )
                    : await (await GetCaseService()).updateProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "SeismicAcquisitionAndProcessing" } as Components.Schemas.UpdateTimeSeriesDto
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
                    ? await (await GetCaseService()).createProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "CountryOfficeCost" } as Components.Schemas.CreateTimeSeriesDto
                    )
                    : await (await GetCaseService()).updateProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "CountryOfficeCost" } as Components.Schemas.UpdateTimeSeriesDto
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
                    ? await (await GetCaseService()).createOverrideProfile(
                        projectId,
                        caseId,
                        {...resourceObject, profileType: "Co2EmissionsOverride" } as Components.Schemas.CreateTimeSeriesOverrideDto,
                    )
                    : await (await GetCaseService()).updateOverrideProfile(
                        projectId,
                        caseId,
                        resourceProfileId!,
                        {...resourceObject, profileType: "Co2EmissionsOverride" } as Components.Schemas.UpdateTimeSeriesOverrideDto,
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
