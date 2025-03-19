import { useBaseMutation } from "./useBaseMutation"

import { GetCaseService } from "@/Services/CaseService"

export const useCaseMutation = () => {
    const mutation = useBaseMutation({
        resourceName: "case",
        getService: GetCaseService,
        updateMethod: "updateCase",
        getResourceFromApiData: (apiData) => apiData?.case,
    })

    const updateFacilitiesAvailability = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "facilitiesAvailability",
    })

    const updateHost = (newValue: string) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "host",
    })

    const updateInitialYearsWithoutWellInterventionCost = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "initialYearsWithoutWellInterventionCost",
    })

    const updateFinalYearsWithoutWellInterventionCost = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "finalYearsWithoutWellInterventionCost",
    })

    const updateProductionStrategyOverview = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "productionStrategyOverview",
    })

    const updateArtificialLift = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "artificialLift",
    })

    const updateProducerCount = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "producerCount",
    })

    const updateWaterInjectorCount = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "waterInjectorCount",
    })

    const updateGasInjectorCount = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "gasInjectorCount",
    })

    const updateCapexFactorFeedStudies = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "capexFactorFeedStudies",
    })

    const updateCapexFactorFeasibilityStudies = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "capexFactorFeasibilityStudies",
    })

    const updateNpvOverride = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "npvOverride",
    })

    const updateBreakEvenOverride = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "breakEvenOverride",
    })

    const updateDescription = (newValue: string) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "description",
    })

    const updateMilestoneDate = (dateKey: string, newDate: Date | null) => mutation.mutateAsync({
        updatedValue: newDate ? newDate.toISOString() : null,
        propertyKey: dateKey,
    })

    const updateName = (newValue: string) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "name",
    })

    const updateArchived = (newValue: boolean, caseId: string) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "archived",
        localCaseId: caseId,
    })

    return {
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
        isLoading: mutation.isPending,
    }
}
