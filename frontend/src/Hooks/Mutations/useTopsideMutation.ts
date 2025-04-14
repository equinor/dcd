import { useMutation, useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router"

import { GetTopsideService } from "@/Services/TopsideService"
import { useAppStore } from "@/Store/AppStore"
import { useProjectContext } from "@/Store/ProjectContext"

export interface Params {
    updatedValue: string | number | boolean;
    propertyKey: keyof Components.Schemas.UpdateTopsideDto;
}

export const useTopsideMutation = () => {
    const queryClient = useQueryClient()
    const { caseId } = useParams()
    const { projectId } = useProjectContext()
    const { setIsSaving, setSnackBarMessage } = useAppStore()

    const mutation = useMutation({
        mutationFn: async (params: Params) => {
            if (!projectId || !caseId) {
                throw new Error("Project ID and Case ID are required")
            }

            setIsSaving(true)

            try {
                const apiData = queryClient.getQueryData<Components.Schemas.CaseWithAssetsDto>(["caseApiData", projectId, caseId])

                if (!apiData?.topside) {
                    throw new Error("Topside data not found in cache")
                }

                const updatedTopside = {
                    ...apiData.topside,
                    [params.propertyKey]: params.updatedValue,
                }

                const dto = {
                    dryWeight: updatedTopside.dryWeight,
                    oilCapacity: updatedTopside.oilCapacity,
                    gasCapacity: updatedTopside.gasCapacity,
                    waterInjectionCapacity: updatedTopside.waterInjectionCapacity,
                    artificialLift: updatedTopside.artificialLift,
                    fuelConsumption: updatedTopside.fuelConsumption,
                    flaredGas: updatedTopside.flaredGas,
                    producerCount: updatedTopside.producerCount,
                    gasInjectorCount: updatedTopside.gasInjectorCount,
                    waterInjectorCount: updatedTopside.waterInjectorCount,
                    co2ShareOilProfile: updatedTopside.co2ShareOilProfile,
                    co2ShareGasProfile: updatedTopside.co2ShareGasProfile,
                    co2ShareWaterInjectionProfile: updatedTopside.co2ShareWaterInjectionProfile,
                    co2OnMaxOilProfile: updatedTopside.co2OnMaxOilProfile,
                    co2OnMaxGasProfile: updatedTopside.co2OnMaxGasProfile,
                    co2OnMaxWaterInjectionProfile: updatedTopside.co2OnMaxWaterInjectionProfile,
                    costYear: updatedTopside.costYear,
                    facilityOpex: updatedTopside.facilityOpex,
                    peakElectricityImported: updatedTopside.peakElectricityImported,
                    source: updatedTopside.source,
                    maturity: updatedTopside.maturity,
                    approvedBy: updatedTopside.approvedBy,
                }

                return GetTopsideService().updateTopside(projectId, caseId, dto)
            } finally {
                setIsSaving(false)
            }
        },
        onSuccess: () => {
            if (projectId && caseId) {
                queryClient.invalidateQueries({ queryKey: ["caseApiData", projectId, caseId] })
            }
        },
        onError: (error: Error) => {
            setSnackBarMessage(error.message || "Failed to update Topside")
        },
    })

    const updateFacilityOpex = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "facilityOpex",
    })

    const updateDryWeight = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "dryWeight",
    })

    const updatePeakElectricityImported = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "peakElectricityImported",
    })

    const updateOilCapacity = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "oilCapacity",
    })

    const updateGasCapacity = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "gasCapacity",
    })

    const updateWaterInjectionCapacity = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "waterInjectionCapacity",
    })

    const updateProducerCount = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "producerCount",
    })

    const updateGasInjectorCount = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "gasInjectorCount",
    })

    const updateWaterInjectorCount = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "waterInjectorCount",
    })

    const updateFuelConsumption = (newValue: number) => mutation.mutateAsync({
        updatedValue: newValue,
        propertyKey: "fuelConsumption",
    })

    return {
        updateFacilityOpex,
        updateDryWeight,
        updatePeakElectricityImported,
        updateOilCapacity,
        updateGasCapacity,
        updateWaterInjectionCapacity,
        updateProducerCount,
        updateGasInjectorCount,
        updateWaterInjectorCount,
        updateFuelConsumption,
        isLoading: mutation.isPending,
    }
}
