import { useEffect, useMemo, useState } from "react"

import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { useAppStore } from "@/Store/AppStore"
import CaseTabTable from "@/Components/Tables/CaseTables/CaseTabTable"
import { useDataFetch } from "@/Hooks"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { PhysUnit, ProfileTypes } from "@/Models/enums"

interface CaseProductionProfilesProps {
    apiData: Components.Schemas.CaseWithAssetsDto,
    tableYears: [number, number],
    alignedGridsRef: any,
}

const CaseProductionProfiles: React.FC<CaseProductionProfilesProps> = ({
    apiData,
    tableYears,
    alignedGridsRef,
}) => {
    const { isCalculatingProductionOverrides } = useAppStore()
    const revisionAndProjectData = useDataFetch()
    const [CaseProductionProfilesData, setCaseProductionProfilesData] = useState<ITimeSeriesTableData[]>([])
    const calculatedFields = useMemo(() => [
        ProfileTypes.FuelFlaringAndLossesOverride,
        ProfileTypes.NetSalesGasOverride,
        ProfileTypes.ImportedElectricityOverride,
    ], [])

    useEffect(() => {
        const drainageStrategyData = apiData.drainageStrategy
        const oilProductionData = apiData.productionProfileOil
        const additionalOilProductionData = apiData.additionalProductionProfileOil
        const gasProductionData = apiData.productionProfileGas
        const additionalGasProductionData = apiData.additionalProductionProfileGas
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

        const newTimeSeriesData: ITimeSeriesTableData[] = [
            {
                profileName: "Oil production",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.SI ? "MSm³/yr" : "mill bbls/yr"}`,
                profile: oilProductionData,
                resourceName: ProfileTypes.ProductionProfileOil,
                resourceId: drainageStrategyData?.id,
                resourcePropertyKey: ProfileTypes.ProductionProfileOil,
                editable: true,
                overridable: false,
            },
            {
                profileName: "Additional Oil production",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.SI ? "MSm³/yr" : "mill bbls/yr"}`,
                profile: additionalOilProductionData,
                resourceName: ProfileTypes.AdditionalProductionProfileOil,
                resourceId: drainageStrategyData?.id,
                resourcePropertyKey: ProfileTypes.AdditionalProductionProfileOil,
                editable: true,
                overridable: false,
                hideIfEmpty: true,
            },
            {
                profileName: "Rich gas production",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.SI ? "GSm³/yr" : "Bscf/yr"}`,
                profile: gasProductionData,
                resourceName: ProfileTypes.ProductionProfileGas,
                resourceId: drainageStrategyData?.id,
                resourcePropertyKey: ProfileTypes.ProductionProfileGas,
                editable: true,
                overridable: false,
            },
            {
                profileName: "Additional rich gas production",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.SI ? "GSm³/yr" : "Bscf/yr"}`,
                profile: additionalGasProductionData,
                resourceName: ProfileTypes.AdditionalProductionProfileGas,
                resourceId: drainageStrategyData?.id,
                resourcePropertyKey: ProfileTypes.AdditionalProductionProfileGas,
                editable: true,
                overridable: false,
                hideIfEmpty: true,
            },
            {
                profileName: "Water production",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.SI ? "MSm³/yr" : "mill bbls/yr"}`,
                profile: waterProductionData,
                resourceName: ProfileTypes.ProductionProfileWater,
                resourceId: drainageStrategyData?.id,
                resourcePropertyKey: ProfileTypes.ProductionProfileWater,
                editable: true,
                overridable: false,
            },
            {
                profileName: "Water injection",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.SI ? "MSm³/yr" : "mill bbls/yr"}`,
                profile: waterInjectionData,
                resourceName: ProfileTypes.ProductionProfileWaterInjection,
                resourceId: drainageStrategyData?.id,
                resourcePropertyKey: ProfileTypes.ProductionProfileWaterInjection,
                editable: true,
                overridable: false,
            },
            {
                profileName: "Fuel, flaring and losses",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.SI ? "GSm³/yr" : "Bscf/yr"}`,
                profile: fuelFlaringAndLossesData,
                resourceName: ProfileTypes.FuelFlaringAndLossesOverride,
                resourceId: drainageStrategyData?.id,
                resourcePropertyKey: ProfileTypes.FuelFlaringAndLossesOverride,
                overrideProfile: fuelFlaringAndLossesOverrideData,
                editable: true,
                overridable: true,
            },
            {
                profileName: "Net sales gas",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.SI ? "GSm³/yr" : "Bscf/yr"}`,
                profile: netSalesGasData,
                resourceName: ProfileTypes.NetSalesGasOverride,
                resourceId: drainageStrategyData?.id,
                resourcePropertyKey: ProfileTypes.NetSalesGasOverride,
                overrideProfile: netSalesGasOverrideData,
                editable: true,
                overridable: true,
            },
            {
                profileName: "Imported electricity",
                unit: "GWh",
                profile: importedElectricityData,
                resourceName: ProfileTypes.ImportedElectricityOverride,
                resourceId: drainageStrategyData?.id,
                resourcePropertyKey: ProfileTypes.ImportedElectricityOverride,
                overrideProfile: importedElectricityOverrideData,
                editable: true,
                overridable: true,
            },
            {
                profileName: "Deferred oil production",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.SI ? "MSm³/yr" : "mill bbls/yr"}`,
                profile: deferredOilData,
                resourceName: ProfileTypes.DeferredOilProduction,
                resourceId: drainageStrategyData?.id,
                resourcePropertyKey: ProfileTypes.DeferredOilProduction,
                editable: true,
                overridable: false,
                hideIfEmpty: true,
            },
            {
                profileName: "Deferred gas production",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.SI ? "GSm³/yr" : "Bscf/yr"}`,
                profile: deferredGasData,
                resourceName: "deferredGasProduction",
                resourceId: drainageStrategyData?.id,
                resourcePropertyKey: "deferredGasProduction",
                editable: true,
                overridable: false,
                hideIfEmpty: true,
            },
        ]

        setCaseProductionProfilesData(newTimeSeriesData)
    }, [apiData, revisionAndProjectData, tableYears])

    return (
        <CaseTabTable
            timeSeriesData={CaseProductionProfilesData}
            dg4Year={getYearFromDateString(apiData.case.dG4Date)}
            tableYears={tableYears}
            tableName="Production profiles"
            includeFooter={false}
            gridRef={alignedGridsRef}
            calculatedFields={calculatedFields}
            ongoingCalculation={isCalculatingProductionOverrides}
        />
    )
}

export default CaseProductionProfiles
