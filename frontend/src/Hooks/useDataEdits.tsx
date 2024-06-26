/* eslint-disable indent */
import { v4 as uuidv4 } from "uuid"
import { useMutation, useQueryClient } from "react-query"
import { useCaseContext } from "../Context/CaseContext"
import {
    EditInstance,
    EditEntry,
    ResourceName,
    ResourcePropertyKey,
    ResourceObject,
} from "../Models/Interfaces"
import { getCurrentEditId } from "../Utils/common"
import { GetCaseService } from "../Services/CaseService"
import { GetTopsideService } from "../Services/TopsideService"
import { GetSurfService } from "../Services/SurfService"
import { GetSubstructureService } from "../Services/SubstructureService"
import { GetTransportService } from "../Services/TransportService"
import { GetDrainageStrategyService } from "../Services/DrainageStrategyService"
import { useAppContext } from "../Context/AppContext"
import { GetWellProjectService } from "../Services/WellProjectService"
import { GetExplorationService } from "../Services/ExplorationService"
import { EMPTY_GUID } from "../Utils/constants"

interface AddEditParams {
    newValue: string | number | undefined;
    previousValue: string | number | undefined;
    inputLabel: string;
    projectId: string;
    resourceName: ResourceName;
    resourcePropertyKey: ResourcePropertyKey;
    resourceId?: string;
    resourceProfileId?: string;
    wellId?: string;
    drillingScheduleId?: string;
    caseId?: string;
    newDisplayValue?: string | number | undefined;
    previousDisplayValue?: string | number | undefined;
    newResourceObject?: ResourceObject;
}

const useDataEdits = (): {
    addEdit: (params: AddEditParams) => void;
    undoEdit: () => void;
    redoEdit: () => void;
} => {
    const { setSnackBarMessage } = useAppContext()
    const {
        caseEdits,
        setCaseEdits,
        projectCase,
        editIndexes,
        setEditIndexes,
        caseEditsBelongingToCurrentCase,
    } = useCaseContext()

    const updateEditIndex = (newEditId: string) => {
        if (!projectCase) {
            console.log("you are not in a project case")
            return
        }

        const editEntry: EditEntry = { caseId: projectCase.id, currentEditId: newEditId }
        const storedEditIndexes = localStorage.getItem("editIndexes")
        const editIndexesArray = storedEditIndexes ? JSON.parse(storedEditIndexes) : []
        const currentCasesEditIndex = editIndexesArray.findIndex((entry: { caseId: string }) => entry.caseId === projectCase.id)

        if (currentCasesEditIndex !== -1) {
            editIndexesArray[currentCasesEditIndex].currentEditId = newEditId
        } else {
            editIndexesArray.push(editEntry)
        }

        localStorage.setItem("editIndexes", JSON.stringify(editIndexesArray))
        setEditIndexes(editIndexesArray)
    }

    const queryClient = useQueryClient()

    const mutation = useMutation(
        async ({ serviceMethod }: {
            projectId: string,
            caseId: string,
            resourceId?: string,
            resourceProfileId?: string,
            wellId?: string,
            drillingScheduleId?: string,
            serviceMethod: object,
        }) => serviceMethod,
        {
            // TODO: Consider adding optimistic updates
            onSuccess: (
                results: any,
                variables,
            ) => {
                const { projectId, caseId } = variables
                /* this should work but doesnt :(
                const assetId = caseId === results.id ? "" : results.id
                queryClient.setQueryData([{ caseId, projectId , assetId }], results)
                */

                // this makes the app refetch all data. We should only refetch the data that was updated in the future.
                queryClient.invalidateQueries(["apiData", { projectId, caseId }])
            },
            onError: (error: any) => {
                console.error("Failed to update data:", error)
                setSnackBarMessage(error.message)
            },
        },
    )

    const updateTopside = async (
        projectId: string,
        caseId: string,
        topsideId: string,
        resourcePropertyKey: ResourcePropertyKey,
        value: string | number | undefined,
        resourceObject?: object,

    ) => {
        const service = await GetTopsideService()
        const existingDataInClient: object | undefined = queryClient.getQueryData([{
            projectId, caseId, resourceId: topsideId, resourceProfileId: EMPTY_GUID,
        }])
        const updatedData = resourceObject || { ...existingDataInClient, [resourcePropertyKey]: value }
        const serviceMethod = service.updateTopside(projectId, caseId, topsideId, updatedData)

        try {
            await mutation.mutateAsync({
                projectId,
                caseId,
                resourceId: topsideId,
                serviceMethod,
            })
            return true
        } catch (error) {
            return false
        }
    }

    const updateSurf = async (
        projectId: string,
        caseId: string,
        surfId: string,
        resourcePropertyKey: ResourcePropertyKey,
        value: string | number | undefined,
        resourceObject?: object,
    ) => {
        const service = await GetSurfService()
        const existingDataInClient: object | undefined = queryClient.getQueryData([{
            projectId, caseId, resourceId: surfId, resourceProfileId: EMPTY_GUID,
        }])
        const updatedData = resourceObject || { ...existingDataInClient, [resourcePropertyKey]: value }
        const serviceMethod = service.updateSurf(projectId, caseId, surfId, updatedData)

        try {
            await mutation.mutateAsync({
                projectId,
                caseId,
                resourceId: surfId,
                serviceMethod,
            })
            return true
        } catch (error) {
            return false
        }
    }

    const updateSubstructure = async (
        projectId: string,
        caseId: string,
        substructureId: string,
        resourcePropertyKey: ResourcePropertyKey,
        value: string | number | undefined,
        resourceObject?: object,
    ) => {
        const service = await GetSubstructureService()
        const existingDataInClient: object | undefined = queryClient.getQueryData([{
            projectId, caseId, resourceId: substructureId, resourceProfileId: EMPTY_GUID,
        }])
        const updatedData = resourceObject || { ...existingDataInClient, [resourcePropertyKey]: value }
        const serviceMethod = service.updateSubstructure(projectId, caseId, substructureId, updatedData)

        try {
            await mutation.mutateAsync({
                projectId,
                caseId,
                resourceId: substructureId,
                serviceMethod,
            })
            return true
        } catch (error) {
            return false
        }
    }

    const updateTransport = async (
        projectId: string,
        caseId: string,
        transportId: string,
        resourcePropertyKey: ResourcePropertyKey,
        value: string | number | undefined,
        resourceObject?: object,
    ) => {
        const service = await GetTransportService()
        const existingDataInClient: object | undefined = queryClient.getQueryData([{
            projectId, caseId, resourceId: transportId, resourceProfileId: EMPTY_GUID,
        }])
        const updatedData = resourceObject || { ...existingDataInClient, [resourcePropertyKey]: value }
        const serviceMethod = service.updateTransport(projectId, caseId, transportId, updatedData)

        try {
            await mutation.mutateAsync({
                projectId,
                caseId,
                resourceId: transportId,
                serviceMethod,
            })
            return true
        } catch (error) {
            return false
        }
    }

    const updateDrainageStrategy = async (
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        resourcePropertyKey: ResourcePropertyKey,
        value: any,
        resourceObject?: object,

    ) => {
        const service = await GetDrainageStrategyService()
        const existingDataInClient: object | undefined = queryClient.getQueryData([{ projectId, caseId, resourceId: drainageStrategyId }])
        const updatedData = resourceObject || { ...existingDataInClient, [resourcePropertyKey]: value }
        const serviceMethod = service.updateDrainageStrategy(projectId, caseId, drainageStrategyId, updatedData)

        try {
            await mutation.mutateAsync({
                projectId,
                caseId,
                resourceId: drainageStrategyId,
                serviceMethod,
            })
            return true
        } catch (error) {
            return false
        }
    }

    const createOrUpdateTimeSeriesProfile = async (
        projectId: string,
        caseId: string,
        assetId: string,
        profileId: string,
        createOrUpdateFunction: any,

    ) => {
        try {
            await mutation.mutateAsync({
                projectId,
                caseId,
                resourceId: assetId,
                resourceProfileId: profileId,
                serviceMethod: createOrUpdateFunction,
            })
            return true
        } catch (error) {
            return false
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
            await mutation.mutateAsync({
                projectId,
                caseId,
                resourceId: assetId,
                wellId,
                drillingScheduleId,
                serviceMethod: createOrUpdateFunction,
            })
            return true
        } catch (error) {
            return false
        }
    }

    const updateCase = async (
        projectId: string,
        caseId: string,
        resourcePropertyKey: ResourcePropertyKey,
        value: any,
        resourceObject?: ResourceObject,
    ) => {
        const caseService = await GetCaseService()
        const existingDataInClient: object | undefined = queryClient.getQueryData([{
            projectId, caseId, resourceId: EMPTY_GUID, resourceProfileId: EMPTY_GUID,
        }])
        const updatedData = resourceObject || { ...existingDataInClient, [resourcePropertyKey]: value }
        const serviceMethod = caseService.updateCase(projectId, caseId, updatedData as Components.Schemas.CaseDto)

        try {
            await mutation.mutateAsync({
                projectId,
                caseId,
                serviceMethod,
            })
            return true
        } catch (error) {
            return false
        }
    }

    type SubmitToApiParams = {
        projectId: string,
        caseId: string,
        resourceName: string,
        resourcePropertyKey: ResourcePropertyKey,
        value: string,
        resourceId?: string,
        resourceObject?: ResourceObject,
        resourceProfileId?: string,
        wellId?: string,
        drillingScheduleId?: string,
    }

    const submitToApi = async ({
        projectId,
        caseId,
        resourceName,
        resourcePropertyKey,
        value,
        resourceId,
        resourceObject,
        resourceProfileId,
        wellId,
        drillingScheduleId,
    }: SubmitToApiParams): Promise<boolean> => {
        const existingDataInClient: object | undefined = queryClient.getQueryData([{
            projectId, caseId, resourceId, resourceProfileId: EMPTY_GUID,
        }])
        const updatedData = resourceObject || { ...existingDataInClient }

        if (resourceName !== "case" && !resourceId) {
            console.log("asset id is required for this service")
            return false
        }
        let success = false
        switch (resourceName) {
            case "case":
                success = await updateCase(projectId, caseId, resourcePropertyKey, value, resourceObject)
                break
            case "topside":
                success = await updateTopside(projectId, caseId, resourceId!, resourcePropertyKey, value, resourceObject)
                break
            case "surf":
                success = await updateSurf(projectId, caseId, resourceId!, resourcePropertyKey, value, resourceObject)
                break
            case "substructure":
                success = await updateSubstructure(projectId, caseId, resourceId!, resourcePropertyKey, value, resourceObject)
                break
            case "transport":
                success = await updateTransport(projectId, caseId, resourceId!, resourcePropertyKey, value, resourceObject)
                break
            case "drainageStrategy":
                success = await updateDrainageStrategy(projectId, caseId, resourceId!, resourcePropertyKey, value, resourceObject)
                break
            case "productionProfileOil":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).createProductionProfileOil(projectId, caseId, resourceId!, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).updateProductionProfileOil(projectId, caseId, resourceId!, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "productionProfileGas":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).createProductionProfileGas(projectId, caseId, resourceId!, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).updateProductionProfileGas(projectId, caseId, resourceId!, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "productionProfileWater":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).createProductionProfileWater(projectId, caseId, resourceId!, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).updateProductionProfileWater(projectId, caseId, resourceId!, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "productionProfileWaterInjection":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).createProductionProfileWaterInjection(projectId, caseId, resourceId!, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).updateProductionProfileWaterInjection(projectId, caseId, resourceId!, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "productionProfileFuelFlaringAndLossesOverride":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).createProductionProfileFuelFlaringAndLossesOverride(projectId, caseId, resourceId!, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).updateProductionProfileFuelFlaringAndLossesOverride(projectId, caseId, resourceId!, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "productionProfileNetSalesGasOverride":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).createProductionProfileNetSalesGasOverride(projectId, caseId, resourceId!, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).updateProductionProfileNetSalesGasOverride(projectId, caseId, resourceId!, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "productionProfileImportedElectricityOverride":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).createProductionProfileImportedElectricityOverride(projectId, caseId, resourceId!, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).updateProductionProfileImportedElectricityOverride(projectId, caseId, resourceId!, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "deferredOilProduction":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).createDeferredOilProduction(projectId, caseId, resourceId!, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).updateDeferredOilProduction(projectId, caseId, resourceId!, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "deferredGasProduction":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).createDeferredGasProduction(projectId, caseId, resourceId!, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).updateDeferredGasProduction(projectId, caseId, resourceId!, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "totalFeasibilityAndConceptStudiesOverride":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).createTotalFeasibilityAndConceptStudiesOverride(projectId, caseId, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).updateTotalFeasibilityAndConceptStudiesOverride(projectId, caseId, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "totalFEEDStudiesOverride":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).createTotalFEEDStudiesOverride(projectId, caseId, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).updateTotalFEEDStudiesOverride(projectId, caseId, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "historicCostCostProfile":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).createHistoricCostCostProfile(projectId, caseId, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).updateHistoricCostCostProfile(projectId, caseId, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "wellInterventionCostProfileOverride":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).createWellInterventionCostProfileOverride(projectId, caseId, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).updateWellInterventionCostProfileOverride(projectId, caseId, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "offshoreFacilitiesOperationsCostProfileOverride":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).createOffshoreFacilitiesOperationsCostProfileOverride(projectId, caseId, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).updateOffshoreFacilitiesOperationsCostProfileOverride(projectId, caseId, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "onshoreRelatedOPEXCostProfile":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).createOnshoreRelatedOPEXCostProfile(projectId, caseId, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).updateOnshoreRelatedOPEXCostProfile(projectId, caseId, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "additionalOPEXCostProfile":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).createAdditionalOPEXCostProfile(projectId, caseId, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).updateAdditionalOPEXCostProfile(projectId, caseId, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "cessationWellsCostOverride":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).createCessationWellsCostOverride(projectId, caseId, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).updateCessationWellsCostOverride(projectId, caseId, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "cessationOffshoreFacilitiesCostOverride":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).createCessationOffshoreFacilitiesCostOverride(projectId, caseId, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).updateCessationOffshoreFacilitiesCostOverride(projectId, caseId, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "surfCostOverride":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetSurfService()).createSurfCostProfileOverride(projectId, caseId, resourceId!, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetSurfService()).updateSurfCostProfileOverride(projectId, caseId, resourceId!, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "topsideCostOverride":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetTopsideService()).createTopsideCostProfileOverride(projectId, caseId, resourceId!, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetTopsideService()).updateTopsideCostProfileOverride(projectId, caseId, resourceId!, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "substructureCostOverride":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetSubstructureService()).createSubstructureCostProfileOverride(projectId, caseId, resourceId!, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetSubstructureService()).updateSubstructureCostProfileOverride(projectId, caseId, resourceId!, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "transportCostOverride":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetTransportService()).createTransportCostProfileOverride(projectId, caseId, resourceId!, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetTransportService()).updateTransportCostProfileOverride(projectId, caseId, resourceId!, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "wellProjectOilProducerCostOverride":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetWellProjectService()).createOilProducerCostProfileOverride(projectId, caseId, resourceId!, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetWellProjectService()).updateOilProducerCostProfileOverride(projectId, caseId, resourceId!, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "wellProjectGasProducerCostOverride":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetWellProjectService()).createGasProducerCostProfileOverride(projectId, caseId, resourceId!, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetWellProjectService()).updateGasProducerCostProfileOverride(projectId, caseId, resourceId!, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "wellProjectWaterInjectorCostOverride":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetWellProjectService()).createWaterInjectorCostProfileOverride(projectId, caseId, resourceId!, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetWellProjectService()).updateWaterInjectorCostProfileOverride(projectId, caseId, resourceId!, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "wellProjectGasInjectorCostOverride":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetWellProjectService()).createGasInjectorCostProfileOverride(projectId, caseId, resourceId!, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetWellProjectService()).updateGasInjectorCostProfileOverride(projectId, caseId, resourceId!, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "seismicAcquisitionAndProcessing":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetExplorationService()).createSeismicAcquisitionAndProcessing(projectId, caseId, resourceId!, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetExplorationService()).updateSeismicAcquisitionAndProcessing(projectId, caseId, resourceId!, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "countryOfficeCost":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetExplorationService()).createCountryOfficeCost(projectId, caseId, resourceId!, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetExplorationService()).updateCountryOfficeCost(projectId, caseId, resourceId!, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "co2EmissionsOverride":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).createProductionProfileCo2EmissionsOverride(projectId, caseId, resourceId!, updatedData!),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).updateProductionProfileCo2EmissionsOverride(projectId, caseId, resourceId!, resourceProfileId!, updatedData!),
                    )
                }
                break
            case "explorationWellDrillingSchedule":
                if (!wellId && !drillingScheduleId) {
                    success = await createOrUpdateDrillingSchedule(
                        projectId,
                        caseId,
                        resourceId!,
                        wellId!,
                        drillingScheduleId!,
                        await (await GetExplorationService()).createExplorationWellDrillingSchedule(projectId, caseId, resourceId!, wellId!, updatedData!),
                    )
                } else if (!wellId && drillingScheduleId) {
                    success = await createOrUpdateDrillingSchedule(
                        projectId,
                        caseId,
                        resourceId!,
                        wellId!,
                        drillingScheduleId!,
                        await (await GetExplorationService()).updateExplorationWellDrillingSchedule(projectId, caseId, resourceId!, wellId!, drillingScheduleId!, updatedData!),
                    )
                }
                break
            case "wellProjectWellDrillingSchedule":
                if (!wellId && !drillingScheduleId) {
                    success = await createOrUpdateDrillingSchedule(
                        projectId,
                        caseId,
                        resourceId!,
                        wellId!,
                        drillingScheduleId!,
                        await (await GetWellProjectService()).createWellProjectWellDrillingSchedule(projectId, caseId, resourceId!, wellId!, updatedData!),
                    )
                } else if (!wellId && drillingScheduleId) {
                    success = await createOrUpdateDrillingSchedule(
                        projectId,
                        caseId,
                        resourceId!,
                        wellId!,
                        drillingScheduleId!,
                        await (await GetWellProjectService()).updateWellProjectWellDrillingSchedule(projectId, caseId, resourceId!, wellId!, drillingScheduleId!, updatedData!),
                    )
                }
                break
            default:
                console.log("Service not found")
        }

        return success
    }

    const addToHistoryTracker = async (editInstanceObject: EditInstance, caseId: string) => {
        const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex((edit) => edit.uuid === getCurrentEditId(editIndexes, projectCase))
        const caseEditsNotBelongingToCurrentCase = caseEdits.filter((edit) => edit.caseId !== caseId)

        let edits = caseEditsBelongingToCurrentCase

        if (currentEditIndex > 0) {
            edits = caseEditsBelongingToCurrentCase.slice(currentEditIndex)
        }

        if (currentEditIndex === -1) {
            edits = []
        }

        edits = [editInstanceObject, ...edits, ...caseEditsNotBelongingToCurrentCase]
        setCaseEdits(edits)
        updateEditIndex(editInstanceObject.uuid)
    }

    const addEdit = async ({
        newValue,
        previousValue,
        inputLabel,
        projectId,
        resourceName,
        resourcePropertyKey,
        resourceId,
        resourceProfileId,
        caseId,
        newDisplayValue,
        previousDisplayValue,
        newResourceObject,
    }: AddEditParams) => {
        if (resourceName !== "case" && !resourceId) {
            console.log("asset id is required for this service")
            return
        }

        if (newValue === previousValue && !newResourceObject) {
            console.log("No changes detected")
            return
        }

        const editInstanceObject: EditInstance = {
            uuid: uuidv4(),
            timeStamp: new Date().getTime(),
            newValue,
            previousValue,
            inputLabel,
            projectId,
            resourceName,
            resourcePropertyKey,
            resourceId,
            resourceProfileId,
            caseId,
            newDisplayValue,
            previousDisplayValue,
            newResourceObject,
        }

        const success = await submitToApi(
            {
                projectId,
                caseId: caseId!,
                resourceName,
                resourcePropertyKey,
                value: newValue as string,
                resourceId,
                resourceProfileId,
                resourceObject: newResourceObject as ResourceObject | undefined,
            },
        )

        if (success && caseId) {
            addToHistoryTracker(editInstanceObject, caseId)
        }
    }

    const undoEdit = () => {
        const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex((edit) => edit.uuid === getCurrentEditId(editIndexes, projectCase))
        const editThatWillBeUndone = caseEditsBelongingToCurrentCase[currentEditIndex]
        const updatedEditIndex = currentEditIndex + 1
        const updatedEdit = caseEditsBelongingToCurrentCase[updatedEditIndex]

        if (currentEditIndex === -1) {
            return
        }
        if (!updatedEdit) {
            updateEditIndex("")
        } else {
            updateEditIndex(updatedEdit.uuid)
        }

        if (editThatWillBeUndone) {
            submitToApi(
                {
                    projectId: editThatWillBeUndone.projectId,
                    caseId: editThatWillBeUndone.caseId!,
                    resourceProfileId: editThatWillBeUndone.resourceProfileId,
                    resourceName: editThatWillBeUndone.resourceName,
                    resourcePropertyKey: editThatWillBeUndone.resourcePropertyKey,
                    value: editThatWillBeUndone.previousValue as string,
                    resourceId: editThatWillBeUndone.resourceId,
                    resourceObject: editThatWillBeUndone.resourceProfileId
                        ? updatedEdit?.newResourceObject as ResourceObject : editThatWillBeUndone.newResourceObject as ResourceObject,
                },
            )
        }
    }

    const redoEdit = () => {
        const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex((edit) => edit.uuid === getCurrentEditId(editIndexes, projectCase))

        if (currentEditIndex <= 0) {
            // If the current edit is the first one or not found, redo the last edit.
            const lastEdit = caseEditsBelongingToCurrentCase[caseEditsBelongingToCurrentCase.length - 1]

            if (lastEdit) {
                updateEditIndex(lastEdit.uuid)
                submitToApi(
                    {
                        projectId: lastEdit.projectId,
                        caseId: lastEdit.caseId!,
                        resourceProfileId: lastEdit.resourceProfileId,
                        resourceName: lastEdit.resourceName,
                        resourcePropertyKey: lastEdit.resourcePropertyKey,
                        value: lastEdit.newValue as string,
                        resourceId: lastEdit.resourceId,
                        resourceObject: lastEdit.newResourceObject as ResourceObject,
                    },
                )
            }
        } else {
            // Otherwise, redo the previous edit.
            const updatedEdit = caseEditsBelongingToCurrentCase[currentEditIndex - 1]
            updateEditIndex(updatedEdit.uuid)

            if (updatedEdit) {
                submitToApi(
                    {
                        projectId: updatedEdit.projectId,
                        caseId: updatedEdit.caseId!,
                        resourceProfileId: updatedEdit.resourceProfileId,
                        resourceName: updatedEdit.resourceName,
                        resourcePropertyKey: updatedEdit.resourcePropertyKey,
                        value: updatedEdit.newValue as string,
                        resourceId: updatedEdit.resourceId,
                        resourceObject: updatedEdit.newResourceObject as ResourceObject,
                    },
                )
            }
        }
    }

    return {
        addEdit,
        undoEdit,
        redoEdit,
    }
}

export default useDataEdits
