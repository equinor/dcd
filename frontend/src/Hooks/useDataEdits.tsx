import {
    useEffect, useState, useRef, useCallback,
} from "react"
import { v4 as uuidv4 } from "uuid"
import { useMutation, useQueryClient } from "react-query"
import { useLocation, useNavigate, useParams } from "react-router"
import _ from "lodash"
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
import {
    productionOverrideResources,
    totalStudyCostOverrideResources,
} from "../Utils/constants"

interface AddEditParams {
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
    newResourceObject: ResourceObject;
    previousResourceObject: ResourceObject;
    tabName?: string;
    tableName?: string;
    inputFieldId?: string;
}

const useDataEdits = (): {
    addEdit: (params: AddEditParams) => void;
    undoEdit: () => void;
    redoEdit: () => void;
} => {
    const {
        setSnackBarMessage,
        setIsSaving,
        setIsCalculatingProductionOverrides,
        setIsCalculatingTotalStudyCostOverrides,
        apiQueue,
        setApiQueue,
    } = useAppContext()
    const {
        caseEdits,
        setCaseEdits,
        editIndexes,
        setEditIndexes,
        caseEditsBelongingToCurrentCase,
    } = useCaseContext()

    const { caseId: caseIdFromParams } = useParams()
    const location = useLocation()
    const navigate = useNavigate()

    const updateEditIndex = (newEditId: string) => {
        if (!caseIdFromParams) {
            console.log("you are not in a project case")
            return
        }

        const editEntry: EditEntry = { caseId: caseIdFromParams, currentEditId: newEditId }
        const storedEditIndexes = localStorage.getItem("editIndexes")
        const editIndexesArray = storedEditIndexes ? JSON.parse(storedEditIndexes) : []
        const currentCasesEditIndex = editIndexesArray.findIndex((entry: { caseId: string }) => entry.caseId === caseIdFromParams)

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
            onSuccess: (
                results: any,
                variables,
            ) => {
                const { projectId, caseId } = variables
                queryClient.fetchQuery(["apiData", { projectId, caseId }])
            },
            onError: (error: any) => {
                console.error("Failed to update data:", error)
                setSnackBarMessage(error.message)
            },
            onSettled: () => {
                setIsSaving(false)
                setIsCalculatingProductionOverrides(false)
                setIsCalculatingTotalStudyCostOverrides(false)
            },
        },
    )

    const updateTopside = async (
        projectId: string,
        caseId: string,
        topsideId: string,
        resourcePropertyKey: ResourcePropertyKey,
        resourceObject: ResourceObject,

    ) => {
        const service = await GetTopsideService()
        const serviceMethod = service.updateTopside(
            projectId,
            caseId,
            topsideId,
            resourceObject as Components.Schemas.TopsideDto,
        )

        try {
            return await mutation.mutateAsync({
                projectId,
                caseId,
                resourceId: topsideId,
                serviceMethod,
            })
        } catch (error) {
            return error
        }
    }

    const updateSurf = async (
        projectId: string,
        caseId: string,
        surfId: string,
        resourcePropertyKey: ResourcePropertyKey,
        resourceObject: ResourceObject,
    ) => {
        const service = await GetSurfService()
        const serviceMethod = service.updateSurf(
            projectId,
            caseId,
            surfId,
            resourceObject as Components.Schemas.SurfDto,
        )

        try {
            return await mutation.mutateAsync({
                projectId,
                caseId,
                resourceId: surfId,
                serviceMethod,
            })
        } catch (error) {
            return false
        }
    }

    const updateSubstructure = async (
        projectId: string,
        caseId: string,
        substructureId: string,
        resourcePropertyKey: ResourcePropertyKey,
        resourceObject: ResourceObject,
    ) => {
        const service = await GetSubstructureService()
        const serviceMethod = service.updateSubstructure(
            projectId,
            caseId,
            substructureId,
            resourceObject as Components.Schemas.SubstructureDto,
        )

        try {
            return await mutation.mutateAsync({
                projectId,
                caseId,
                resourceId: substructureId,
                serviceMethod,
            })
        } catch (error) {
            return false
        }
    }

    const updateTransport = async (
        projectId: string,
        caseId: string,
        transportId: string,
        resourcePropertyKey: ResourcePropertyKey,
        resourceObject: ResourceObject,
    ) => {
        const service = await GetTransportService()
        const serviceMethod = service.updateTransport(
            projectId,
            caseId,
            transportId,
            resourceObject as Components.Schemas.TransportDto,
        )

        try {
            return await mutation.mutateAsync({
                projectId,
                caseId,
                resourceId: transportId,
                serviceMethod,
            })
        } catch (error) {
            return false
        }
    }

    const updateDrainageStrategy = async (
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        resourcePropertyKey: ResourcePropertyKey,
        resourceObject: ResourceObject,

    ) => {
        const service = await GetDrainageStrategyService()
        const serviceMethod = service.updateDrainageStrategy(
            projectId,
            caseId,
            drainageStrategyId,
            resourceObject as Components.Schemas.DrainageStrategyDto,
        )

        try {
            return await mutation.mutateAsync({
                projectId,
                caseId,
                resourceId: drainageStrategyId,
                serviceMethod,
            })
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
            const result = await mutation.mutateAsync({
                projectId,
                caseId,
                resourceId: assetId,
                resourceProfileId: profileId,
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

    const updateCase = async (
        projectId: string,
        caseId: string,
        resourcePropertyKey: ResourcePropertyKey,
        resourceObject: ResourceObject,
    ) => {
        const caseService = await GetCaseService()
        const serviceMethod = caseService.updateCase(
            projectId,
            caseId,
            resourceObject as Components.Schemas.CaseDto,
        )

        try {
            return await mutation.mutateAsync({
                projectId,
                caseId,
                serviceMethod,
            })
        } catch (error) {
            return false
        }
    }

    type SubmitToApiParams = {
        projectId: string,
        caseId: string,
        resourceName: string,
        resourcePropertyKey: ResourcePropertyKey,
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
        resourcePropertyKey,
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
                success = await updateCase(
                    projectId,
                    caseId,
                    resourcePropertyKey,
                    resourceObject,
                )
                break
            case "topside":
                success = await updateTopside(
                    projectId,
                    caseId,
                    resourceId!,
                    resourcePropertyKey,
                    resourceObject,
                )
                break
            case "surf":
                success = await updateSurf(
                    projectId,
                    caseId,
                    resourceId!,
                    resourcePropertyKey,
                    resourceObject,
                )
                break
            case "substructure":
                success = await updateSubstructure(
                    projectId,
                    caseId,
                    resourceId!,
                    resourcePropertyKey,
                    resourceObject,
                )
                break
            case "transport":
                success = await updateTransport(
                    projectId,
                    caseId,
                    resourceId!,
                    resourcePropertyKey,
                    resourceObject,
                )
                break
            case "drainageStrategy":
                success = await updateDrainageStrategy(
                    projectId,
                    caseId,
                    resourceId!,
                    resourcePropertyKey,
                    resourceObject,
                )
                break
            case "productionProfileOil":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).createProductionProfileOil(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateProductionProfileOilDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).updateProductionProfileOil(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateProductionProfileOilDto,
                        ),
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
                        await (await GetDrainageStrategyService()).createProductionProfileGas(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateProductionProfileGasDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).updateProductionProfileGas(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateProductionProfileGasDto,
                        ),
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
                        await (await GetDrainageStrategyService()).createProductionProfileWater(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateProductionProfileWaterDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).updateProductionProfileWater(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateProductionProfileWaterDto,
                        ),
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
                        await (await GetDrainageStrategyService()).createProductionProfileWaterInjection(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateProductionProfileWaterInjectionDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).updateProductionProfileWaterInjection(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateProductionProfileWaterInjectionDto,
                        ),
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
                        await (await GetDrainageStrategyService()).createProductionProfileFuelFlaringAndLossesOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateFuelFlaringAndLossesOverrideDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).updateProductionProfileFuelFlaringAndLossesOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateFuelFlaringAndLossesOverrideDto,
                        ),
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
                        await (await GetDrainageStrategyService()).createProductionProfileNetSalesGasOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateNetSalesGasOverrideDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).updateProductionProfileNetSalesGasOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateNetSalesGasOverrideDto,
                        ),
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
                        await (await GetDrainageStrategyService()).createProductionProfileImportedElectricityOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateImportedElectricityOverrideDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).updateProductionProfileImportedElectricityOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateImportedElectricityOverrideDto,
                        ),
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
                        await (await GetDrainageStrategyService()).createDeferredOilProduction(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateDeferredOilProductionDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).updateDeferredOilProduction(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateDeferredOilProductionDto,
                        ),
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
                        await (await GetDrainageStrategyService()).createDeferredGasProduction(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateDeferredGasProductionDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).updateDeferredGasProduction(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateDeferredGasProductionDto,
                        ),
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
                        await (await GetCaseService()).createTotalFeasibilityAndConceptStudiesOverride(
                            projectId,
                            caseId,
                            resourceObject as Components.Schemas.CreateTotalFeasibilityAndConceptStudiesOverrideDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).updateTotalFeasibilityAndConceptStudiesOverride(
                            projectId,
                            caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTotalFeasibilityAndConceptStudiesOverrideDto,
                        ),
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
                        await (await GetCaseService()).createTotalFEEDStudiesOverride(
                            projectId,
                            caseId,
                            resourceObject as Components.Schemas.CreateTotalFEEDStudiesOverrideDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).updateTotalFEEDStudiesOverride(
                            projectId,
                            caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTotalFEEDStudiesOverrideDto,
                        ),
                    )
                }
                break
            case "totalOtherStudiesCostProfile":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).createTotalOtherStudiesCostProfile(
                            projectId,
                            caseId,
                            resourceObject as Components.Schemas.CreateTotalOtherStudiesCostProfileDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).updateTotalOtherStudiesCostProfile(
                            projectId,
                            caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTotalOtherStudiesCostProfileDto,
                        ),
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
                        await (await GetCaseService()).createHistoricCostCostProfile(
                            projectId,
                            caseId,
                            resourceObject as Components.Schemas.CreateHistoricCostCostProfileDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).updateHistoricCostCostProfile(
                            projectId,
                            caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateHistoricCostCostProfileDto,
                        ),
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
                        await (await GetCaseService()).createWellInterventionCostProfileOverride(
                            projectId,
                            caseId,
                            resourceObject as Components.Schemas.CreateWellInterventionCostProfileOverrideDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).updateWellInterventionCostProfileOverride(
                            projectId,
                            caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateWellInterventionCostProfileOverrideDto,
                        ),
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
                        await (await GetCaseService()).createOffshoreFacilitiesOperationsCostProfileOverride(
                            projectId,
                            caseId,
                            resourceObject as Components.Schemas.CreateOffshoreFacilitiesOperationsCostProfileOverrideDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).updateOffshoreFacilitiesOperationsCostProfileOverride(
                            projectId,
                            caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto,
                        ),
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
                        await (await GetCaseService()).createOnshoreRelatedOPEXCostProfile(
                            projectId,
                            caseId,
                            resourceObject as Components.Schemas.CreateOnshoreRelatedOPEXCostProfileDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).updateOnshoreRelatedOPEXCostProfile(
                            projectId,
                            caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateOnshoreRelatedOPEXCostProfileDto,
                        ),
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
                        await (await GetCaseService()).createAdditionalOPEXCostProfile(
                            projectId,
                            caseId,
                            resourceObject as Components.Schemas.CreateAdditionalOPEXCostProfileDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).updateAdditionalOPEXCostProfile(
                            projectId,
                            caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateAdditionalOPEXCostProfileDto,
                        ),
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
                        await (await GetCaseService()).createCessationWellsCostOverride(
                            projectId,
                            caseId,
                            resourceObject as Components.Schemas.CreateCessationWellsCostOverrideDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).updateCessationWellsCostOverride(
                            projectId,
                            caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateCessationWellsCostOverrideDto,
                        ),
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
                        await (await GetCaseService()).createCessationOffshoreFacilitiesCostOverride(
                            projectId,
                            caseId,
                            resourceObject as Components.Schemas.CreateCessationOffshoreFacilitiesCostOverrideDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).updateCessationOffshoreFacilitiesCostOverride(
                            projectId,
                            caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateCessationOffshoreFacilitiesCostOverrideDto,
                        ),
                    )
                }
                break
            case "cessationOnshoreFacilitiesCostProfile":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).createCessationOnshoreFacilitiesCostProfile(
                            projectId,
                            caseId,
                            resourceObject as Components.Schemas.CreateCessationOnshoreFacilitiesCostProfileDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetCaseService()).updateCessationOnshoreFacilitiesCostProfile(
                            projectId,
                            caseId,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateCessationOnshoreFacilitiesCostProfileDto,
                        ),
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
                        await (await GetSurfService()).createSurfCostProfileOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateSurfCostProfileOverrideDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetSurfService()).updateSurfCostProfileOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateSurfCostProfileOverrideDto,
                        ),
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
                        await (await GetTopsideService()).createTopsideCostProfileOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTopsideCostProfileOverrideDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetTopsideService()).updateTopsideCostProfileOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTopsideCostProfileOverrideDto,
                        ),
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
                        await (await GetSubstructureService()).createSubstructureCostProfileOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateSubstructureCostProfileOverrideDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetSubstructureService()).updateSubstructureCostProfileOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateSubstructureCostProfileOverrideDto,
                        ),
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
                        await (await GetTransportService()).createTransportCostProfileOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateTransportCostProfileOverrideDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetTransportService()).updateTransportCostProfileOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateTransportCostProfileOverrideDto,
                        ),
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
                        await (await GetWellProjectService()).createOilProducerCostProfileOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateOilProducerCostProfileOverrideDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetWellProjectService()).updateOilProducerCostProfileOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateOilProducerCostProfileOverrideDto,
                        ),
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
                        await (await GetWellProjectService()).createGasProducerCostProfileOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateGasProducerCostProfileOverrideDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetWellProjectService()).updateGasProducerCostProfileOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateGasProducerCostProfileOverrideDto,
                        ),
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
                        await (await GetWellProjectService()).createWaterInjectorCostProfileOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateWaterInjectorCostProfileOverrideDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetWellProjectService()).updateWaterInjectorCostProfileOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateWaterInjectorCostProfileOverrideDto,
                        ),
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
                        await (await GetWellProjectService()).createGasInjectorCostProfileOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateGasInjectorCostProfileOverrideDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetWellProjectService()).updateGasInjectorCostProfileOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateGasInjectorCostProfileOverrideDto,
                        ),
                    )
                }
                break
            case "gAndGAdminCost":
                if (!resourceProfileId) {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetExplorationService()).createGAndGAdminCostOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateGAndGAdminCostOverrideDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetExplorationService()).updateGAndGAdminCostOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateGAndGAdminCostOverrideDto,
                        ),
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
                        await (await GetExplorationService()).createSeismicAcquisitionAndProcessing(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateSeismicAcquisitionAndProcessingDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetExplorationService()).updateSeismicAcquisitionAndProcessing(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateSeismicAcquisitionAndProcessingDto,
                        ),
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
                        await (await GetExplorationService()).createCountryOfficeCost(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateCountryOfficeCostDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetExplorationService()).updateCountryOfficeCost(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateCountryOfficeCostDto,
                        ),
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
                        await (await GetDrainageStrategyService()).createProductionProfileCo2EmissionsOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceObject as Components.Schemas.CreateCo2EmissionsOverrideDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateTimeSeriesProfile(
                        projectId,
                        caseId,
                        resourceId!,
                        resourceProfileId!,
                        await (await GetDrainageStrategyService()).updateProductionProfileCo2EmissionsOverride(
                            projectId,
                            caseId,
                            resourceId!,
                            resourceProfileId!,
                            resourceObject as Components.Schemas.UpdateCo2EmissionsOverrideDto,
                        ),
                    )
                }
                break
            case "explorationWellDrillingSchedule":
                if (!drillingScheduleId) {
                    success = await createOrUpdateDrillingSchedule(
                        projectId,
                        caseId,
                        resourceId!,
                        wellId!,
                        drillingScheduleId!,
                        await (await GetExplorationService()).createExplorationWellDrillingSchedule(
                            projectId,
                            caseId,
                            resourceId!,
                            wellId!,
                            resourceObject as Components.Schemas.CreateDrillingScheduleDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateDrillingSchedule(
                        projectId,
                        caseId,
                        resourceId!,
                        wellId!,
                        drillingScheduleId!,
                        await (await GetExplorationService()).updateExplorationWellDrillingSchedule(
                            projectId,
                            caseId,
                            resourceId!,
                            wellId!,
                            drillingScheduleId!,
                            resourceObject as Components.Schemas.UpdateDrillingScheduleDto,
                        ),
                    )
                }
                break
            case "wellProjectWellDrillingSchedule":
                if (!drillingScheduleId) {
                    success = await createOrUpdateDrillingSchedule(
                        projectId,
                        caseId,
                        resourceId!,
                        wellId!,
                        drillingScheduleId!,
                        await (await GetWellProjectService()).createWellProjectWellDrillingSchedule(
                            projectId,
                            caseId,
                            resourceId!,
                            wellId!,
                            resourceObject as Components.Schemas.CreateDrillingScheduleDto,
                        ),
                    )
                } else {
                    success = await createOrUpdateDrillingSchedule(
                        projectId,
                        caseId,
                        resourceId!,
                        wellId!,
                        drillingScheduleId!,
                        await (await GetWellProjectService()).updateWellProjectWellDrillingSchedule(
                            projectId,
                            caseId,
                            resourceId!,
                            wellId!,
                            drillingScheduleId!,
                            resourceObject as Components.Schemas.UpdateDrillingScheduleDto,
                        ),
                    )
                }
                break
            default:
                console.log("Service not found")
        }

        return success
    }

    const addToHistoryTracker = async (editInstanceObject: EditInstance, caseId: string) => {
        const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex((edit) => edit.uuid === getCurrentEditId(editIndexes, caseIdFromParams))
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

    const submitQueueItem = async (editInstance: EditInstance) => {
        console.log("submitting edit", editInstance)
        const success = await submitToApi({
            projectId: editInstance.projectId,
            caseId: editInstance.caseId!,
            resourceName: editInstance.resourceName,
            resourcePropertyKey: editInstance.resourcePropertyKey,
            resourceId: editInstance.resourceId,
            resourceProfileId: editInstance.resourceProfileId,
            wellId: editInstance.wellId,
            drillingScheduleId: editInstance.drillingScheduleId,
            resourceObject: editInstance.newResourceObject as ResourceObject,
        })
        return success
    }
    const timerRef = useRef<NodeJS.Timeout | null>(null)
    const lastUpdateRef = useRef(Date.now())

    const addEditToHistoryTracker = async (editInstance: EditInstance) => {
        console.log("Processing edit", editInstance)
        const submitted = await submitQueueItem(editInstance)

        if (submitted && editInstance.caseId) {
            if (!editInstance.resourceProfileId) {
                console.log("Edit saved successfully")
                addToHistoryTracker(editInstance, editInstance.caseId)
            } else {
                const editWithProfileId = structuredClone(editInstance)
                editWithProfileId.resourceProfileId = submitted.resourceProfileId
                editWithProfileId.drillingScheduleId = submitted.resourceProfileId
                addToHistoryTracker(editWithProfileId, editInstance.caseId)
                console.log("Edit with resourceProfileId saved successfully")
            }
        } else {
            console.log("Error saving edit")
        }
    }

    const processQueue = useCallback(() => {
        console.log("Processes queue method running")
        setApiQueue((prevQueue) => {
            console.log("Queue length", prevQueue.length)

            if (prevQueue.length > 0) {
                setIsSaving(true)
                const editInstance = prevQueue[0]
                addEditToHistoryTracker(editInstance).then(() => {
                    setIsSaving(false)
                    setApiQueue(prevQueue.slice(1))
                })
            }
            return prevQueue
        })
    }, [])

    useEffect(() => {
        if (apiQueue.length > 0) {
            console.log("Queue: ", apiQueue)
        }
    }, [apiQueue])

    const addEdit = async ({
        inputLabel,
        projectId,
        resourceName,
        resourcePropertyKey,
        resourceId,
        resourceProfileId,
        wellId,
        drillingScheduleId,
        caseId,
        newDisplayValue,
        previousDisplayValue,
        newResourceObject,
        previousResourceObject,
        tabName,
        tableName,
        inputFieldId,
    }: AddEditParams) => {
        if (resourceName !== "case" && !resourceId) {
            console.log("asset id is required for this service")
            return
        }

        const editInstanceObject: EditInstance = {
            uuid: uuidv4(),
            timeStamp: new Date().getTime(),
            inputLabel,
            projectId,
            resourceName,
            resourcePropertyKey,
            resourceId,
            resourceProfileId,
            wellId,
            drillingScheduleId,
            caseId,
            newDisplayValue,
            previousDisplayValue,
            newResourceObject,
            previousResourceObject,
            tabName,
            tableName,
            inputFieldId,
        }

        setApiQueue((prevQueue) => [...prevQueue, editInstanceObject])
    }

    /*
            setApiQueue((prevQueue) => {
                const existingEditIndex = prevQueue.findIndex(
                    (edit) => edit.caseId === caseId
                        && edit.resourceName === resourceName,
                )
                console.log("prevQueue", prevQueue)
                console.log("existingEditIndex", existingEditIndex)
                if (existingEditIndex !== -1) {
                    const existingResourceObject = prevQueue[existingEditIndex].newResourceObject

                    const combinedResourceObject: ResourceObject = structuredClone(existingResourceObject)
                    combinedResourceObject[resourcePropertyKey as keyof ResourceObject] = newResourceObject[resourcePropertyKey as keyof ResourceObject]
                    console.log("combinedResourceObject", combinedResourceObject)
                    const newQueue = [...prevQueue]
                    newQueue[existingEditIndex] = {
                        ...newQueue[existingEditIndex],
                        newResourceObject: combinedResourceObject,
                        newDisplayValue,
                        previousDisplayValue,
                        timeStamp: new Date().getTime(),
                    }
                    return newQueue
                }
                return [...prevQueue, editInstanceObject]
            }) */
    const { caseId } = useParams()

    const highlightElement = (element: HTMLElement | null, duration = 3000) => {
        if (element) {
            element.classList.add("highlighted")
            setTimeout(() => {
                element.classList.remove("highlighted")
            }, duration)
        }
    }

    const delay = (ms: number) => new Promise((resolve) => { setTimeout(resolve, ms) })

    const undoEdit = async () => {
        const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex(
            (edit) => edit.uuid === getCurrentEditId(editIndexes, caseIdFromParams),
        )
        if (currentEditIndex === -1) {
            return
        }

        const editThatWillBeUndone = caseEditsBelongingToCurrentCase[currentEditIndex]
        const updatedEditIndex = currentEditIndex + 1
        const updatedEdit = caseEditsBelongingToCurrentCase[updatedEditIndex]

        updateEditIndex(updatedEdit ? updatedEdit.uuid : "")

        if (editThatWillBeUndone) {
            const projectUrl = location.pathname.split("/case")[0]
            navigate(`${projectUrl}/case/${caseId}/${editThatWillBeUndone.tabName ?? ""}`)

            const scrollToElement = (elementId: string) => new Promise<void>((resolve, reject) => {
                const tabElement = document.getElementById(elementId) as HTMLElement | null
                if (!tabElement) {
                    reject(new Error(`Element with id ${elementId} not found`))
                    return
                }
                tabElement.scrollIntoView({ behavior: "smooth", block: "center" })
                resolve()
            })

            const rowWhereCellWillBeUndone = editThatWillBeUndone.tableName ?? editThatWillBeUndone.inputFieldId ?? editThatWillBeUndone.inputLabel

            if (!rowWhereCellWillBeUndone) {
                console.error("rowWhereCellWillBeUndone is undefined")
                return
            }

            setTimeout(async () => {
                try {
                    await scrollToElement(rowWhereCellWillBeUndone)

                    const tabElement = document.getElementById(rowWhereCellWillBeUndone) as HTMLElement | null
                    if (tabElement) {
                        // Attempt to highlight cell, doesnt work since querySelector can't find any element with data-key="${editThatWillBeUndone.resourcePropertyKey}
                        if (editThatWillBeUndone.tableName) {
                            const tableCell = tabElement.querySelector(`[data-key="${editThatWillBeUndone.resourcePropertyKey}"]`) as HTMLElement | null
                            highlightElement(tableCell ?? tabElement)
                        } else {
                            highlightElement(tabElement)
                        }
                    }

                    await delay(500)
                    await submitToApi({
                        projectId: editThatWillBeUndone.projectId,
                        caseId: editThatWillBeUndone.caseId!,
                        resourceProfileId: editThatWillBeUndone.resourceProfileId,
                        resourceName: editThatWillBeUndone.resourceName,
                        resourcePropertyKey: editThatWillBeUndone.resourcePropertyKey,
                        resourceId: editThatWillBeUndone.resourceId,
                        resourceObject: editThatWillBeUndone.previousResourceObject as ResourceObject,
                        wellId: editThatWillBeUndone.wellId,
                        drillingScheduleId: editThatWillBeUndone.drillingScheduleId,
                    })
                } catch (error) {
                    console.error(error)
                }
            }, 500)
        }
    }

    const redoEdit = async () => {
        const currentEditIndex = caseEditsBelongingToCurrentCase.findIndex(
            (edit) => edit.uuid === getCurrentEditId(editIndexes, caseIdFromParams),
        )
        if (currentEditIndex <= 0) {
            const lastEdit = caseEditsBelongingToCurrentCase[caseEditsBelongingToCurrentCase.length - 1]
            if (lastEdit) {
                updateEditIndex(lastEdit.uuid)
                const projectUrl = location.pathname.split("/case")[0]
                navigate(`${projectUrl}/case/${caseId}/${lastEdit.tabName ?? ""}`)

                const scrollToElement = (elementId: string) => new Promise<void>((resolve, reject) => {
                    const tabElement = document.getElementById(elementId) as HTMLElement | null
                    if (!tabElement) {
                        reject(new Error(`Element with id ${elementId} not found`))
                        return
                    }
                    tabElement.scrollIntoView({ behavior: "auto", block: "center" })
                    resolve()
                })

                const rowWhereCellWillBeUndone = lastEdit.tableName ?? lastEdit.inputFieldId ?? lastEdit.inputLabel
                if (!rowWhereCellWillBeUndone) {
                    console.error("rowWhereCellWillBeUndone is undefined")
                    return
                }

                setTimeout(async () => {
                    try {
                        await scrollToElement(rowWhereCellWillBeUndone)

                        const tabElement = document.getElementById(rowWhereCellWillBeUndone) as HTMLElement | null
                        if (tabElement) {
                            if (lastEdit.tableName) {
                                const tableCell = tabElement.querySelector(`[data-key="${lastEdit.resourcePropertyKey}"]`) as HTMLElement | null
                                highlightElement(tableCell ?? tabElement)
                            } else {
                                highlightElement(tabElement)
                            }
                        }

                        await delay(500)
                        await submitToApi({
                            projectId: lastEdit.projectId,
                            caseId: lastEdit.caseId!,
                            resourceProfileId: lastEdit.resourceProfileId,
                            resourceName: lastEdit.resourceName,
                            resourcePropertyKey: lastEdit.resourcePropertyKey,
                            resourceId: lastEdit.resourceId,
                            resourceObject: lastEdit.newResourceObject as ResourceObject,
                            wellId: lastEdit.wellId,
                            drillingScheduleId: lastEdit.drillingScheduleId,
                        })
                    } catch (error) {
                        console.error(error)
                    }
                }, 500)
            }
        } else {
            const updatedEdit = caseEditsBelongingToCurrentCase[currentEditIndex - 1]
            updateEditIndex(updatedEdit.uuid)
            if (updatedEdit) {
                const projectUrl = location.pathname.split("/case")[0]
                navigate(`${projectUrl}/case/${caseId}/${updatedEdit.tabName ?? ""}`)

                const scrollToElement = (elementId: string) => new Promise<void>((resolve, reject) => {
                    const tabElement = document.getElementById(elementId) as HTMLElement | null
                    if (!tabElement) {
                        reject(new Error(`Element with id ${elementId} not found`))
                        return
                    }
                    tabElement.scrollIntoView({ behavior: "smooth", block: "center" })
                    resolve()
                })

                const rowWhereCellWillBeUndone = updatedEdit.tableName ?? updatedEdit.inputFieldId ?? updatedEdit.inputLabel
                if (!rowWhereCellWillBeUndone) {
                    return
                }

                setTimeout(async () => {
                    try {
                        await scrollToElement(rowWhereCellWillBeUndone)

                        const tabElement = document.getElementById(rowWhereCellWillBeUndone) as HTMLElement | null
                        if (tabElement) {
                            if (updatedEdit.tableName) {
                                const tableCell = tabElement.querySelector(`[data-key="${updatedEdit.resourcePropertyKey}"]`) as HTMLElement | null
                                highlightElement(tableCell ?? tabElement)
                            } else {
                                highlightElement(tabElement)
                            }
                        }

                        await delay(500)
                        await submitToApi({
                            projectId: updatedEdit.projectId,
                            caseId: updatedEdit.caseId!,
                            resourceProfileId: updatedEdit.resourceProfileId,
                            resourceName: updatedEdit.resourceName,
                            resourcePropertyKey: updatedEdit.resourcePropertyKey,
                            resourceId: updatedEdit.resourceId,
                            resourceObject: updatedEdit.newResourceObject as ResourceObject,
                            wellId: updatedEdit.wellId,
                            drillingScheduleId: updatedEdit.drillingScheduleId,
                        })
                    } catch (error) {
                        console.error(error)
                    }
                }, 500)
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
