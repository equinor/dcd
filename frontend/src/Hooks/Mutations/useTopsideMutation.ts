import { GetTopsideService } from "@/Services/TopsideService"
import { useBaseMutation } from "./useBaseMutation"

export const useTopsideMutation = () => {
    const mutation = useBaseMutation({
        resourceName: "topside",
        getService: GetTopsideService,
        updateMethod: "updateTopside",
        getResourceFromApiData: (apiData) => apiData?.topside,
        loggerName: "TOPSIDE_MUTATION",
    })

    const updateFacilityOpex = (topsideId: string, newValue: number) => mutation.mutateAsync({
        resourceId: topsideId,
        updatedValue: newValue,
        propertyKey: "facilityOpex",
    })

    const updateDryWeight = (topsideId: string, newValue: number) => mutation.mutateAsync({
        resourceId: topsideId,
        updatedValue: newValue,
        propertyKey: "dryWeight",
    })

    const updatePeakElectricityImported = (topsideId: string, newValue: number) => mutation.mutateAsync({
        resourceId: topsideId,
        updatedValue: newValue,
        propertyKey: "peakElectricityImported",
    })

    const updateOilCapacity = (topsideId: string, newValue: number) => mutation.mutateAsync({
        resourceId: topsideId,
        updatedValue: newValue,
        propertyKey: "oilCapacity",
    })

    const updateGasCapacity = (topsideId: string, newValue: number) => mutation.mutateAsync({
        resourceId: topsideId,
        updatedValue: newValue,
        propertyKey: "gasCapacity",
    })

    const updateWaterInjectionCapacity = (topsideId: string, newValue: number) => mutation.mutateAsync({
        resourceId: topsideId,
        updatedValue: newValue,
        propertyKey: "waterInjectionCapacity",
    })

    const updateProducerCount = (topsideId: string, newValue: number) => mutation.mutateAsync({
        resourceId: topsideId,
        updatedValue: newValue,
        propertyKey: "producerCount",
    })

    const updateGasInjectorCount = (topsideId: string, newValue: number) => mutation.mutateAsync({
        resourceId: topsideId,
        updatedValue: newValue,
        propertyKey: "gasInjectorCount",
    })

    const updateWaterInjectorCount = (topsideId: string, newValue: number) => mutation.mutateAsync({
        resourceId: topsideId,
        updatedValue: newValue,
        propertyKey: "waterInjectorCount",
    })

    const updateFuelConsumption = (topsideId: string, newValue: number) => mutation.mutateAsync({
        resourceId: topsideId,
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
