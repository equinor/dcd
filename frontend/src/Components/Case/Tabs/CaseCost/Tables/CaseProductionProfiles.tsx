import { useEffect, useState } from "react"
import CaseTabTable from "../../../Components/CaseTabTable"
import { ITimeSeriesData } from "../../../../../Models/Interfaces"
import { useProjectContext } from "../../../../../Context/ProjectContext"

interface CaseProductionProfilesProps {
    apiData: Components.Schemas.CaseWithAssetsDto,
    tableYears: [number, number],
    alignedGridsRef: any,
}

const CaseProductionProfiles: React.FC<CaseProductionProfilesProps> = ({
    apiData, tableYears, alignedGridsRef,
}) => {
    const { project } = useProjectContext()
    const [CaseProductionProfilesData, setCaseProductionProfilesData] = useState<ITimeSeriesData[]>([])

    useEffect(() => {
        const drainageStrategyData = apiData.drainageStrategy
        const oilProductionData = apiData.productionProfileOil
        const gasProductionData = apiData.productionProfileGas
        const waterProductionData = apiData.productionProfileWater
        const waterInjectionData = apiData.productionProfileWaterInjection
        const fuelFlaringAndLossesData = apiData.fuelFlaringAndLosses
        const fuelFlaringAndLossesOverrideData = apiData.fuelFlaringAndLossesOverride
        const netSalesGasData = apiData.netSalesGas
        const netSalesGasOverrideData = apiData.netSalesGasOverride
        const importedElectricityData = apiData.importedElectricity
        const importedElectricityOverrideData = apiData.importedElectricityOverride
        const deferredOilData = apiData.deferredOilProduction
        const deferredGasData = apiData.deferredGasProduction

        const newTimeSeriesData: ITimeSeriesData[] = [
            {
                profileName: "Oil production",
                unit: `${project?.physicalUnit === 0 ? "MSm³/yr" : "mill bbls/yr"}`,
                profile: oilProductionData,
                resourceName: "productionProfileOil",
                resourceId: drainageStrategyData?.id,
                resourceProfileId: oilProductionData?.id,
                resourcePropertyKey: "productionProfileOil",
                editable: true,
                overridable: false,
            },
            {
                profileName: "Gas production",
                unit: `${project?.physicalUnit === 0 ? "GSm³/yr" : "Bscf/yr"}`,
                profile: gasProductionData,
                resourceName: "productionProfileGas",
                resourceId: drainageStrategyData?.id,
                resourceProfileId: gasProductionData?.id,
                resourcePropertyKey: "productionProfileGas",
                editable: true,
                overridable: false,
            },
            {
                profileName: "Water production",
                unit: `${project?.physicalUnit === 0 ? "MSm³/yr" : "mill bbls/yr"}`,
                profile: waterProductionData,
                resourceName: "productionProfileWater",
                resourceId: drainageStrategyData?.id,
                resourceProfileId: waterProductionData?.id,
                resourcePropertyKey: "productionProfileWater",
                editable: true,
                overridable: false,
            },
            {
                profileName: "Water injection",
                unit: `${project?.physicalUnit === 0 ? "MSm³/yr" : "mill bbls/yr"}`,
                profile: waterInjectionData,
                resourceName: "productionProfileWaterInjection",
                resourceId: drainageStrategyData?.id,
                resourceProfileId: waterInjectionData?.id,
                resourcePropertyKey: "productionProfileWaterInjection",
                editable: true,
                overridable: false,
            },
            {
                profileName: "Fuel, flaring and losses",
                unit: `${project?.physicalUnit === 0 ? "GSm³/yr" : "Bscf/yr"}`,
                profile: fuelFlaringAndLossesData,
                resourceName: "productionProfileFuelFlaringAndLossesOverride",
                resourceId: drainageStrategyData?.id,
                resourceProfileId: fuelFlaringAndLossesOverrideData?.id,
                resourcePropertyKey: "productionProfileFuelFlaringAndLossesOverride",
                overrideProfile: fuelFlaringAndLossesOverrideData,
                editable: true,
                overridable: true,
            },
            {
                profileName: "Net sales gas",
                unit: `${project?.physicalUnit === 0 ? "GSm³/yr" : "Bscf/yr"}`,
                profile: netSalesGasData,
                resourceName: "productionProfileNetSalesGasOverride",
                resourceId: drainageStrategyData?.id,
                resourceProfileId: netSalesGasOverrideData?.id,
                resourcePropertyKey: "productionProfileNetSalesGasOverride",
                overrideProfile: netSalesGasOverrideData,
                editable: true,
                overridable: true,
            },
            {
                profileName: "Imported electricity",
                unit: "GWh",
                profile: importedElectricityData,
                resourceName: "productionProfileImportedElectricityOverride",
                resourceId: drainageStrategyData?.id,
                resourceProfileId: importedElectricityOverrideData?.id,
                resourcePropertyKey: "productionProfileImportedElectricityOverride",
                overrideProfile: importedElectricityOverrideData,
                editable: true,
                overridable: true,
            },
            {
                profileName: "Deferred oil production",
                unit: `${project?.physicalUnit === 0 ? "MSm³/yr" : "mill bbls/yr"}`,
                profile: deferredOilData,
                resourceName: "deferredOilProduction",
                resourceId: drainageStrategyData?.id,
                resourceProfileId: deferredOilData?.id,
                resourcePropertyKey: "deferredOilProduction",
                editable: true,
                overridable: false,
                hideIfEmpty: true,
            },
            {
                profileName: "Deferred gas production",
                unit: `${project?.physicalUnit === 0 ? "GSm³/yr" : "Bscf/yr"}`,
                profile: deferredGasData,
                resourceName: "deferredGasProduction",
                resourceId: drainageStrategyData?.id,
                resourceProfileId: deferredGasData?.id,
                resourcePropertyKey: "deferredGasProduction",
                editable: true,
                overridable: false,
                hideIfEmpty: true,
            },
        ]

        setCaseProductionProfilesData(newTimeSeriesData)
    }, [apiData, project])

    return (
        <CaseTabTable
            timeSeriesData={CaseProductionProfilesData}
            dg4Year={apiData.case.dG4Date ? new Date(apiData.case.dG4Date).getFullYear() : 2030}
            tableYears={tableYears}
            tableName="Production profiles"
            includeFooter={false}
            gridRef={alignedGridsRef}
        />
    )
}

export default CaseProductionProfiles
