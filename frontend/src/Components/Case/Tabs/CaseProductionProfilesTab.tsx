import {
    useState,
    useRef,
} from "react"
import { NativeSelect } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useQueryClient, useQuery } from "react-query"
import { useParams } from "react-router"
import SwitchableNumberInput from "../../Input/SwitchableNumberInput"
import CaseTabTable from "../Components/CaseTabTable"
import { ITimeSeries } from "../../../Models/ITimeSeries"
import { AgChartsTimeseries, setValueToCorrespondingYear } from "../../AgGrid/AgChartsTimeseries"
import InputSwitcher from "../../Input/Components/InputSwitcher"
import { useProjectContext } from "../../../Context/ProjectContext"
import { useCaseContext } from "../../../Context/CaseContext"
import DateRangePicker from "../../Input/TableDateRangePicker"
import SwitchableDropdownInput from "../../Input/SwitchableDropdownInput"
import CaseProductionProfilesTabSkeleton from "./LoadingSkeletons/CaseProductionProfilesTabSkeleton"
import { ITimeSeriesData } from "../../../Models/Interfaces"

const CaseProductionProfilesTab = () => {
    const queryClient = useQueryClient()
    const { caseId } = useParams()
    const { project } = useProjectContext()
    const { projectCase, activeTabCase } = useCaseContext()
    const projectId = project?.id || null

    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

    const profilesToHideWithoutValues = ["Deferred oil production", "Deferred gas production"]

    const productionStrategyOptions = {
        0: "Depletion",
        1: "Water injection",
        2: "Gas injection",
        3: "WAG",
        4: "Mixed",
    }

    const artificialLiftOptions = {
        0: "No lift",
        1: "Gas lift",
        2: "Electrical submerged pumps",
        3: "Subsea booster pumps",
    }

    const datePickerValue = (() => {
        if (project?.physicalUnit === 1) {
            return "Oil field"
        } if (project?.physicalUnit === 0) {
            return "SI"
        }
        return ""
    })()

    const gridRef = useRef<any>(null)

    const gasSolutionOptions = {
        0: "Export",
        1: "Injection",
    }

    const { data: apiData } = useQuery<Components.Schemas.CaseWithAssetsDto | undefined>(
        ["apiData", { projectId, caseId }],
        () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        {
            enabled: !!projectId && !!caseId,
            initialData: () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        },
    )

    const drainageStrategyData = apiData?.drainageStrategy
    const oilProductionData = apiData?.productionProfileOil
    const gasProductionData = apiData?.productionProfileGas
    const waterProductionData = apiData?.productionProfileWater
    const waterInjectionData = apiData?.productionProfileWaterInjection
    const fuelFlaringAndLossesData = apiData?.fuelFlaringAndLosses
    const fuelFlaringAndLossesOverrideData = apiData?.fuelFlaringAndLossesOverride
    const netSalesGasData = apiData?.netSalesGas
    const netSalesGasOverrideData = apiData?.netSalesGasOverride
    const importedElectricityData = apiData?.importedElectricity
    const importedElectricityOverrideData = apiData?.importedElectricityOverride
    const deferredOilData = apiData?.deferredOilProduction
    const deferredGasData = apiData?.deferredGasProduction
    const caseData = apiData?.case

    if (!projectCase || !drainageStrategyData) { return null }

    const timeSeriesData: ITimeSeriesData[] = [
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
        },
    ]

    console.log("Timeseriesdata: ", timeSeriesData)

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    const productionProfilesChartData = () => {
        const dataArray: object[] = []
        if (projectCase?.dG4Date === undefined) { return dataArray }
        for (let i = startYear; i <= endYear; i += 1) {
            dataArray.push({
                year: i,
                oilProduction: setValueToCorrespondingYear(oilProductionData, i, startYear, new Date(projectCase?.dG4Date).getFullYear()),
                gasProduction: setValueToCorrespondingYear(gasProductionData, i, startYear, new Date(projectCase?.dG4Date).getFullYear()),
                waterProduction: setValueToCorrespondingYear(waterProductionData, i, startYear, new Date(projectCase?.dG4Date).getFullYear()),
            })
        }
        return dataArray
    }

    const injectionProfilesChartData = () => {
        const dataArray: object[] = []
        if (projectCase?.dG4Date === undefined) { return dataArray }
        for (let i = startYear; i <= endYear; i += 1) {
            dataArray.push({
                year: i,
                waterInjection:
                    setValueToCorrespondingYear(waterInjectionData, i, startYear, new Date(projectCase?.dG4Date).getFullYear()),
            })
        }
        return dataArray
    }

    if (activeTabCase !== 1) { return null }

    if (!caseData || !drainageStrategyData || !projectId) {
        return (<CaseProductionProfilesTabSkeleton />)
    }

    return (
        <Grid container spacing={2}>
            <Grid item xs={12} md={6} lg={3}>
                <SwitchableNumberInput
                    resourceName="case"
                    resourcePropertyKey="facilitiesAvailability"
                    label="Facilities availability"
                    value={caseData.facilitiesAvailability}
                    integer={false}
                    unit="%"
                    min={0}
                    max={100}
                />
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
                <SwitchableDropdownInput
                    resourceName="drainageStrategy"
                    resourcePropertyKey="gasSolution"
                    resourceId={drainageStrategyData.id}
                    value={drainageStrategyData.gasSolution}
                    options={gasSolutionOptions}
                    label="Gas solution"
                />
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
                <InputSwitcher
                    value={productionStrategyOptions[caseData.productionStrategyOverview]}
                    label="Production strategy overview"
                >
                    <NativeSelect
                        id="productionStrategy"
                        label=""
                        disabled
                        value={caseData.productionStrategyOverview}
                    >
                        {Object.entries(productionStrategyOptions).map(([value, label]) => (
                            <option key={value} value={value}>{label}</option>
                        ))}
                    </NativeSelect>
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
                <InputSwitcher
                    value={artificialLiftOptions[caseData.artificialLift]}
                    label="Artificial lift"
                >
                    <NativeSelect
                        id="artificialLift"
                        label=""
                        disabled
                        value={caseData.artificialLift}
                    >
                        {Object.entries(artificialLiftOptions).map(([value, label]) => (
                            <option key={value} value={value}>{label}</option>
                        ))}
                    </NativeSelect>
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
                <SwitchableNumberInput
                    resourceName="case"
                    resourcePropertyKey="producerCount"
                    label="Oil producer wells"
                    value={caseData.producerCount}
                    integer
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
                <SwitchableNumberInput
                    resourceName="case"
                    resourcePropertyKey="waterInjectorCount"
                    label="Water injector wells"
                    value={caseData.waterInjectorCount}
                    integer
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
                <SwitchableNumberInput
                    resourceName="case"
                    resourcePropertyKey="gasInjectorCount"
                    label="Gas injector wells"
                    value={caseData.gasInjectorCount}
                    integer
                    disabled
                />
            </Grid>
            <DateRangePicker
                setStartYear={setStartYear}
                setEndYear={setEndYear}
                startYear={startYear}
                endYear={endYear}
                labelText="Units"
                labelValue={datePickerValue}
                handleTableYearsClick={handleTableYearsClick}
            />
            <Grid item xs={12}>
                <AgChartsTimeseries
                    data={productionProfilesChartData()}
                    chartTitle="Production profiles"
                    barColors={["#243746", "#EB0037", "#A8CED1"]}
                    barProfiles={["oilProduction", "gasProduction", "waterProduction"]}
                    barNames={[
                        "Oil production (MSm3)",
                        "Gas production (GSm3)",
                        "Water production (MSm3)",
                    ]}
                />
            </Grid>
            {
                (waterInjectionData?.values && waterInjectionData.values?.length > 0)
                && (
                    <Grid item xs={12}>
                        <AgChartsTimeseries
                            data={injectionProfilesChartData()}
                            chartTitle="Injection profiles"
                            barColors={["#A8CED1"]}
                            barProfiles={["waterInjection"]}
                            barNames={["Water injection"]}
                            unit="MSm3"
                        />
                    </Grid>
                )
            }
            <Grid item xs={12}>
                <CaseTabTable
                    timeSeriesData={timeSeriesData}
                    dg4Year={caseData.dG4Date ? new Date(caseData.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="Production profiles"
                    includeFooter={false}
                    gridRef={gridRef}
                    profilesToHideWithoutValues={profilesToHideWithoutValues}
                />
            </Grid>
        </Grid>
    )
}

export default CaseProductionProfilesTab
