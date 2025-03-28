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

    const updateCo2RemovedFromGas = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "co2RemovedFromGas",
    })

    const updateCo2EmissionFromFuelGas = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "co2EmissionFromFuelGas",
    })

    const updateFlaredGasPerProducedVolume = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "flaredGasPerProducedVolume",
    })

    const updateCo2EmissionsFromFlaredGas = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "co2EmissionsFromFlaredGas",
    })

    const updateCo2Vented = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "co2Vented",
    })

    const updateAverageDevelopmentDrillingDays = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "averageDevelopmentDrillingDays",
    })

    const updateDailyEmissionFromDrillingRig = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "dailyEmissionFromDrillingRig",
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
