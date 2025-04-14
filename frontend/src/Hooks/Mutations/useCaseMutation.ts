import { useMutation, useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router-dom"

import { GetCaseService } from "@/Services/CaseService"
import { useAppStore } from "@/Store/AppStore"
import { useProjectContext } from "@/Store/ProjectContext"

export const useCaseMutation = (): {
    updateWholeCase: (caseObject: Components.Schemas.UpdateCaseDto) => Promise<void>
    updateFacilitiesAvailability: (newValue: number) => Promise<void>
    updateHost: (newValue: string) => Promise<void>
    updateInitialYearsWithoutWellInterventionCost: (newValue: number) => Promise<void>
    updateFinalYearsWithoutWellInterventionCost: (newValue: number) => Promise<void>
    updateProductionStrategyOverview: (newValue: number) => Promise<void>
    updateArtificialLift: (newValue: number) => Promise<void>
    updateProducerCount: (newValue: number) => Promise<void>
    updateWaterInjectorCount: (newValue: number) => Promise<void>
    updateGasInjectorCount: (newValue: number) => Promise<void>
    updateCapexFactorFeedStudies: (newValue: number) => Promise<void>
    updateCapexFactorFeasibilityStudies: (newValue: number) => Promise<void>
    updateNpvOverride: (newValue: number) => Promise<void>
    updateBreakEvenOverride: (newValue: number) => Promise<void>
    updateDescription: (newValue: string) => Promise<void>
    updateMilestoneDate: (dateKey: string, newDate: Date | null) => Promise<void>
    updateName: (newValue: string) => Promise<void>
    updateArchived: (newValue: boolean, caseId: string) => Promise<void>
    updateCo2RemovedFromGas: (newValue: number) => Promise<void>
    updateCo2EmissionFromFuelGas: (newValue: number) => Promise<void>
    updateFlaredGasPerProducedVolume: (newValue: number) => Promise<void>
    updateCo2EmissionsFromFlaredGas: (newValue: number) => Promise<void>
    updateCo2Vented: (newValue: number) => Promise<void>
    updateAverageDevelopmentDrillingDays: (newValue: number) => Promise<void>
    updateDailyEmissionFromDrillingRig: (newValue: number) => Promise<void>
    isLoading: boolean
} => {
    const queryClient = useQueryClient()
    const { caseId } = useParams()
    const { projectId } = useProjectContext()
    const { setIsSaving, setSnackBarMessage } = useAppStore()

    const mutation = useMutation({
        mutationFn: async (params: any) => {
            if (!projectId || !caseId) {
                throw new Error("Project ID and Case ID are required")
            }

            setIsSaving(true)

            try {
                const service = GetCaseService()

                let apiData = queryClient.getQueryData<Components.Schemas.CaseWithAssetsDto>(["caseApiData", projectId, caseId])

                if (!apiData && caseId) {
                    try {
                        apiData = await service.getCaseWithAssets(projectId, caseId)
                        queryClient.setQueryData(["caseApiData", projectId, caseId], apiData)
                    } catch (error) {
                        console.error("Error fetching case data directly:", error)
                    }
                }

                const resource = apiData?.case

                if (!resource) {
                    throw new Error("case not found")
                }

                let updatedResource

                if (params.isFullUpdate) {
                    updatedResource = params.updatedValue
                } else {
                    updatedResource = {
                        ...resource,
                        [params.propertyKey]: params.updatedValue,
                    }
                }

                return await service.updateCase(
                    projectId,
                    caseId,
                    updatedResource,
                )
            } finally {
                setIsSaving(false)
            }
        },
        onSuccess: (_, variables) => {
            const finalCaseId = variables.localCaseId || caseId

            if (projectId && finalCaseId) {
                queryClient.invalidateQueries({ queryKey: ["caseApiData", projectId, finalCaseId] })
                queryClient.invalidateQueries({ queryKey: ["projectApiData"] })
            }
        },
        onError: (error: unknown) => {
            if (error instanceof Error) {
                setSnackBarMessage(error.message || "Failed to update case")
            }
        },
    })

    const updateWholeCase = (caseObject: Components.Schemas.UpdateCaseDto): Promise<void> => mutation.mutateAsync({
        updatedValue: caseObject,
        propertyKey: "_fullUpdate",
        isFullUpdate: true,
    })

    const updateFacilitiesAvailability = (newValue: number): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "facilitiesAvailability",
    })

    const updateHost = (newValue: string): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "host",
    })

    const updateInitialYearsWithoutWellInterventionCost = (newValue: number): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "initialYearsWithoutWellInterventionCost",
    })

    const updateFinalYearsWithoutWellInterventionCost = (newValue: number): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "finalYearsWithoutWellInterventionCost",
    })

    const updateProductionStrategyOverview = (newValue: number): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "productionStrategyOverview",
    })

    const updateArtificialLift = (newValue: number): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "artificialLift",
    })

    const updateProducerCount = (newValue: number): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "producerCount",
    })

    const updateWaterInjectorCount = (newValue: number): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "waterInjectorCount",
    })

    const updateGasInjectorCount = (newValue: number): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "gasInjectorCount",
    })

    const updateCapexFactorFeedStudies = (newValue: number): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "capexFactorFeedStudies",
    })

    const updateCapexFactorFeasibilityStudies = (newValue: number): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "capexFactorFeasibilityStudies",
    })

    const updateNpvOverride = (newValue: number): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "npvOverride",
    })

    const updateBreakEvenOverride = (newValue: number): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "breakEvenOverride",
    })

    const updateDescription = (newValue: string): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "description",
    })

    const updateMilestoneDate = (dateKey: string, newDate: Date | null): Promise<void> => mutation.mutateAsync({
        updatedValue: newDate ? newDate.toISOString() : null,
        propertyKey: dateKey,
    })

    const updateName = (newValue: string): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "name",
    })

    const updateArchived = (newValue: boolean): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "archived",
        localCaseId: caseId,
    })

    const updateCo2RemovedFromGas = (newValue: number): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "co2RemovedFromGas",
    })

    const updateCo2EmissionFromFuelGas = (newValue: number): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "co2EmissionFromFuelGas",
    })

    const updateFlaredGasPerProducedVolume = (newValue: number): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "flaredGasPerProducedVolume",
    })

    const updateCo2EmissionsFromFlaredGas = (newValue: number): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "co2EmissionsFromFlaredGas",
    })

    const updateCo2Vented = (newValue: number): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "co2Vented",
    })

    const updateAverageDevelopmentDrillingDays = (newValue: number): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "averageDevelopmentDrillingDays",
    })

    const updateDailyEmissionFromDrillingRig = (newValue: number): Promise<void> => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "dailyEmissionFromDrillingRig",
    })

    return {
        updateWholeCase,
        updateFacilitiesAvailability,
        updateHost,
        updateInitialYearsWithoutWellInterventionCost,
        updateFinalYearsWithoutWellInterventionCost,
        updateProductionStrategyOverview,
        updateArtificialLift,
        updateProducerCount,
        updateWaterInjectorCount,
        updateGasInjectorCount,
        updateCapexFactorFeedStudies,
        updateCapexFactorFeasibilityStudies,
        updateNpvOverride,
        updateBreakEvenOverride,
        updateDescription,
        updateMilestoneDate,
        updateName,
        updateArchived,
        updateCo2RemovedFromGas,
        updateCo2EmissionFromFuelGas,
        updateFlaredGasPerProducedVolume,
        updateCo2EmissionsFromFlaredGas,
        updateCo2Vented,
        updateAverageDevelopmentDrillingDays,
        updateDailyEmissionFromDrillingRig,
        isLoading: mutation.isPending,
    }
}
