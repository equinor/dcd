import { useEffect, useMemo, useState } from "react"

import CaseBaseTable from "@/Components/Tables/CaseBaseTable"
import { useDataFetch } from "@/Hooks"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { ProfileTypes } from "@/Models/enums"
import { useAppStore } from "@/Store/AppStore"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { getUnitByProfileName } from "@/Utils/FormatingUtils"

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

        const physUnit = revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit
        const currency = revisionAndProjectData?.commonProjectAndRevisionData.currency

        interface CreateProfileDataParams {
            profileName: string;
            profile: any;
            resourceName: ProfileTypes;
            overrideProfile?: any;
            editable?: boolean;
            overridable?: boolean;
            hideIfEmpty?: boolean;
        }

        const createProfileData = ({
            profileName,
            profile,
            resourceName,
            overrideProfile,
            editable = true,
            overridable,
            hideIfEmpty,
        }: CreateProfileDataParams): ITimeSeriesTableData => ({
            profileName,
            unit: getUnitByProfileName(profileName, physUnit, currency),
            profile,
            resourceName,
            resourceId: drainageStrategyData?.id,
            resourcePropertyKey: resourceName,
            editable,
            overridable: overridable ?? !!overrideProfile,
            ...(overrideProfile && { overrideProfile }),
            ...(hideIfEmpty && { hideIfEmpty }),
        })

        const newTimeSeriesData: ITimeSeriesTableData[] = [
            createProfileData({
                profileName: "Oil production",
                profile: oilProductionData,
                resourceName: ProfileTypes.ProductionProfileOil,
            }),
            createProfileData({
                profileName: "Additional Oil production",
                profile: additionalOilProductionData,
                resourceName: ProfileTypes.AdditionalProductionProfileOil,
                hideIfEmpty: true,
            }),
            createProfileData({
                profileName: "Rich gas production",
                profile: gasProductionData,
                resourceName: ProfileTypes.ProductionProfileGas,
            }),
            createProfileData({
                profileName: "Additional rich gas production",
                profile: additionalGasProductionData,
                resourceName: ProfileTypes.AdditionalProductionProfileGas,
                hideIfEmpty: true,
            }),
            createProfileData({
                profileName: "Water production",
                profile: waterProductionData,
                resourceName: ProfileTypes.ProductionProfileWater,
            }),
            createProfileData({
                profileName: "Water injection",
                profile: waterInjectionData,
                resourceName: ProfileTypes.ProductionProfileWaterInjection,
            }),
            createProfileData({
                profileName: "NGL Production",
                profile: productionProfileNglData,
                resourceName: ProfileTypes.ProductionProfileNglOverride,
                overrideProfile: productionProfileNglOverrideData,
            }),
            createProfileData({
                profileName: "Condensate production",
                profile: condensateProductionData,
                resourceName: ProfileTypes.CondensateProductionOverride,
                overrideProfile: condensateProductionOverrideData,
            }),
            createProfileData({
                profileName: "Fuel, flaring and losses",
                profile: fuelFlaringAndLossesData,
                resourceName: ProfileTypes.FuelFlaringAndLossesOverride,
                overrideProfile: fuelFlaringAndLossesOverrideData,
            }),
            createProfileData({
                profileName: "Net sales gas",
                profile: netSalesGasData,
                resourceName: ProfileTypes.NetSalesGasOverride,
                overrideProfile: netSalesGasOverrideData,
            }),
            createProfileData({
                profileName: "Imported electricity",
                profile: importedElectricityData,
                resourceName: ProfileTypes.ImportedElectricityOverride,
                overrideProfile: importedElectricityOverrideData,
            }),
            createProfileData({
                profileName: "Deferred oil production",
                profile: deferredOilData,
                resourceName: ProfileTypes.DeferredOilProduction,
                hideIfEmpty: true,
            }),
            createProfileData({
                profileName: "Deferred gas production",
                profile: deferredGasData,
                resourceName: ProfileTypes.DeferredGasProduction,
                hideIfEmpty: true,
            }),
            createProfileData({
                profileName: "Total exported volumes",
                profile: totalExportedVolumesData,
                resourceName: ProfileTypes.TotalExportedVolumesOverride,
                overrideProfile: totalExportedVolumesOverrideData,
            }),
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
