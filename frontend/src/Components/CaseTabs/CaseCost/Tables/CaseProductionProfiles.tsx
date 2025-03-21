import { useEffect, useMemo, useState } from "react"

import CaseBaseTable from "@/Components/Tables/CaseBaseTable"
import { useDataFetch } from "@/Hooks"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { PhysUnit, ProfileTypes } from "@/Models/enums"
import { useAppStore } from "@/Store/AppStore"
import { getYearFromDateString } from "@/Utils/DateUtils"

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
        ProfileTypes.ProductionProfileNglOverride,
        ProfileTypes.CondensateProductionOverride,
        ProfileTypes.FuelFlaringAndLossesOverride,
        ProfileTypes.NetSalesGasOverride,
        ProfileTypes.TotalExportedVolumesOverride,
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
        const productionProfileNglData = apiData.productionProfileNgl
        const productionProfileNglOverrideData = apiData.productionProfileNglOverride
        const condensateProductionData = apiData.condensateProduction
        const condensateProductionOverrideData = apiData.condensateProductionOverride
        const fuelFlaringAndLossesData = apiData.fuelFlaringAndLosses
        const fuelFlaringAndLossesOverrideData = apiData.fuelFlaringAndLossesOverride
        const netSalesGasData = apiData.netSalesGas
        const netSalesGasOverrideData = apiData.netSalesGasOverride
        const totalExportedVolumesData = apiData.totalExportedVolumes
        const totalExportedVolumesOverrideData = apiData.totalExportedVolumesOverride
        const importedElectricityData = apiData.importedElectricity
        const importedElectricityOverrideData = apiData.importedElectricityOverride
        const deferredOilData = apiData.deferredOilProduction
        const deferredGasData = apiData.deferredGasProduction

        const newTimeSeriesData: ITimeSeriesTableData[] = [
            {
                profileName: "Oil production",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.Si ? "MSm³/yr" : "mill bbls/yr"}`,
                profile: oilProductionData,
                resourceName: ProfileTypes.ProductionProfileOil,
                resourceId: drainageStrategyData?.id,
                resourcePropertyKey: ProfileTypes.ProductionProfileOil,
                editable: true,
                overridable: false,
            },
            {
                profileName: "Additional Oil production",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.Si ? "MSm³/yr" : "mill bbls/yr"}`,
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
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.Si ? "GSm³/yr" : "Bscf/yr"}`,
                profile: gasProductionData,
                resourceName: ProfileTypes.ProductionProfileGas,
                resourceId: drainageStrategyData?.id,
                resourcePropertyKey: ProfileTypes.ProductionProfileGas,
                editable: true,
                overridable: false,
            },
            {
                profileName: "Additional rich gas production",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.Si ? "GSm³/yr" : "Bscf/yr"}`,
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
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.Si ? "MSm³/yr" : "mill bbls/yr"}`,
                profile: waterProductionData,
                resourceName: ProfileTypes.ProductionProfileWater,
                resourceId: drainageStrategyData?.id,
                resourcePropertyKey: ProfileTypes.ProductionProfileWater,
                editable: true,
                overridable: false,
            },
            {
                profileName: "Water injection",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.Si ? "MSm³/yr" : "mill bbls/yr"}`,
                profile: waterInjectionData,
                resourceName: ProfileTypes.ProductionProfileWaterInjection,
                resourceId: drainageStrategyData?.id,
                resourcePropertyKey: ProfileTypes.ProductionProfileWaterInjection,
                editable: true,
                overridable: false,
            },
            {
                profileName: "NGL Production",
                unit: "MTPA",
                profile: productionProfileNglData,
                resourceName: ProfileTypes.ProductionProfileNglOverride,
                resourceId: drainageStrategyData?.id,
                resourcePropertyKey: ProfileTypes.ProductionProfileNglOverride,
                overrideProfile: productionProfileNglOverrideData,
                editable: true,
                overridable: true,
            },
            {
                profileName: "Condensate production",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.Si ? "MSm³/yr" : "mill bbls/yr"}`,
                profile: condensateProductionData,
                resourceName: ProfileTypes.CondensateProductionOverride,
                resourceId: drainageStrategyData?.id,
                resourcePropertyKey: ProfileTypes.CondensateProductionOverride,
                overrideProfile: condensateProductionOverrideData,
                editable: true,
                overridable: true,
            },
            {
                profileName: "Fuel, flaring and losses",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.Si ? "GSm³/yr" : "Bscf/yr"}`,
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
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.Si ? "GSm³/yr" : "Bscf/yr"}`,
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
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.Si ? "MSm³/yr" : "mill bbls/yr"}`,
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
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.Si ? "GSm³/yr" : "Bscf/yr"}`,
                profile: deferredGasData,
                resourceName: ProfileTypes.DeferredGasProduction,
                resourceId: drainageStrategyData?.id,
                resourcePropertyKey: ProfileTypes.DeferredGasProduction,
                editable: true,
                overridable: false,
                hideIfEmpty: true,
            },
            {
                profileName: "Total exported volumes",
                unit: "MBoE/yr",
                profile: totalExportedVolumesData,
                resourceName: ProfileTypes.TotalExportedVolumesOverride,
                resourceId: drainageStrategyData?.id,
                resourcePropertyKey: ProfileTypes.TotalExportedVolumesOverride,
                overrideProfile: totalExportedVolumesOverrideData,
                editable: true,
                overridable: true,
            },
        ]

        setCaseProductionProfilesData(newTimeSeriesData)
    }, [apiData, revisionAndProjectData, tableYears])

    return (
        <CaseBaseTable
            timeSeriesData={CaseProductionProfilesData}
            dg4Year={getYearFromDateString(apiData.case.dg4Date)}
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
