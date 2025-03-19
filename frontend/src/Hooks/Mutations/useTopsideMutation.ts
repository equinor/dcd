import { useQueryClient } from "@tanstack/react-query"

import { useBaseMutation, MutationParams } from "./useBaseMutation"

import { GetTopsideService } from "@/Services/TopsideService"

export const useTopsideMutation = () => {
    const queryClient = useQueryClient()

    const topsideMutationFn = async (
        service: ReturnType<typeof GetTopsideService>,
        projectIdParam: string,
        caseIdParam: string,
        params: MutationParams<any>,
    ) => {
        const apiData = await queryClient.getQueryData<any>(["caseApiData", projectIdParam, caseIdParam])

        if (!apiData?.topside) {
            throw new Error("Topside data not found in cache")
        }

        const currentTopside = apiData.topside
        const updatedTopside = {
            ...currentTopside,
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
            approvedBy: updatedTopside.approvedBy || "",
        }

        return service.updateTopside(
            projectIdParam,
            caseIdParam,
            dto,
        )
    }

    const mutation = useBaseMutation({
        resourceName: "topside",
        getService: GetTopsideService,
        updateMethod: "updateTopside",
        customMutationFn: topsideMutationFn,
        getResourceFromApiData: (apiData) => apiData?.topside,
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
