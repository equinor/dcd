import {
    useState,
    useRef,
    useEffect,
} from "react"
import { NativeSelect } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useQueryClient } from "@tanstack/react-query"
import { useParams } from "react-router"
import SwitchableNumberInput from "../../Input/SwitchableNumberInput"
import { AgChartsTimeseries, setValueToCorrespondingYear } from "../../AgGrid/AgChartsTimeseries"
import InputSwitcher from "../../Input/Components/InputSwitcher"
import { useProjectContext } from "../../../Context/ProjectContext"
import { useCaseContext } from "../../../Context/CaseContext"
import DateRangePicker from "../../Input/TableDateRangePicker"
import SwitchableDropdownInput from "../../Input/SwitchableDropdownInput"
import CaseProductionProfilesTabSkeleton from "../../LoadingSkeletons/CaseProductionProfilesTabSkeleton"
import CaseProductionProfiles from "./CaseCost/Tables/CaseProductionProfiles"
import { SetTableYearsFromProfiles } from "../Components/CaseTabTableHelper"

const CaseProductionProfilesTab = ({ addEdit }: { addEdit: any }) => {
    const queryClient = useQueryClient()
    const { caseId } = useParams()
    const { project } = useProjectContext()
    const { activeTabCase } = useCaseContext()
    const projectId = project?.id || null

    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])
    const [yearRangeSetFromProfiles, setYearRangeSetFromProfiles] = useState<boolean>(false)
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

    const gridRef = useRef<any>(null)

    const gasSolutionOptions = {
        0: "Export",
        1: "Injection",
    }

    const apiData = queryClient.getQueryData(
        ["apiData", { projectId, caseId }],
    ) as Components.Schemas.CaseWithAssetsDto

    const drainageStrategyData = queryClient.getQueryData(
        ["drainageStrategy", { projectId, caseId }],
    ) as Components.Schemas.DrainageStrategyDto

    const oilProductionData = queryClient.getQueryData(
        ["productionProfileOil", { projectId, caseId }],
    ) as any

    const additionalOilProductionData = queryClient.getQueryData(
        ["additionalProductionProfileOil", { projectId, caseId }],
    ) as any

    const gasProductionData = queryClient.getQueryData(
        ["productionProfileGas", { projectId, caseId }],
    ) as any

    const additionalGasProductionData = queryClient.getQueryData(
        ["additionalProductionProfileGas", { projectId, caseId }],
    ) as any

    const waterProductionData = queryClient.getQueryData(
        ["productionProfileWater", { projectId, caseId }],
    ) as any

    const waterInjectionData = queryClient.getQueryData(
        ["productionProfileWaterInjection", { projectId, caseId }],
    ) as any

    const fuelFlaringAndLossesData = queryClient.getQueryData(
        ["fuelFlaringAndLosses", { projectId, caseId }],
    ) as Components.Schemas.FuelFlaringAndLossesDto

    const fuelFlaringAndLossesOverrideData = queryClient.getQueryData(
        ["fuelFlaringAndLossesOverride", { projectId, caseId }],
    ) as Components.Schemas.FuelFlaringAndLossesDto

    const netSalesGasData = queryClient.getQueryData(
        ["netSalesGas", { projectId, caseId }],
    ) as Components.Schemas.NetSalesGasDto

    const netSalesGasOverrideData = queryClient.getQueryData(
        ["netSalesGasOverride", { projectId, caseId }],
    ) as Components.Schemas.NetSalesGasDto // does not exist

    const importedElectricityData = queryClient.getQueryData(
        ["importedElectricity", { projectId, caseId }],
    ) as Components.Schemas.ImportedElectricityDto

    const importedElectricityOverrideData = queryClient.getQueryData(
        ["importedElectricityOverride", { projectId, caseId }],
    ) as Components.Schemas.ImportedElectricityDto // does not exist

    const deferredOilData = queryClient.getQueryData(
        ["deferredOilProduction", { projectId, caseId }],
    ) as Components.Schemas.DeferredOilProductionDto

    const deferredGasData = queryClient.getQueryData(
        ["deferredGasProduction", { projectId, caseId }],
    ) as Components.Schemas.DeferredGasProductionDto

    const caseData = queryClient.getQueryData(
        ["case", { projectId, caseId }],
    ) as Components.Schemas.CaseWithProfilesDto

    useEffect(() => {
        if (apiData && activeTabCase === 1 && !yearRangeSetFromProfiles) {
            SetTableYearsFromProfiles(
                [
                    drainageStrategyData,
                    oilProductionData,
                    gasProductionData,
                    waterProductionData,
                    waterInjectionData,
                    fuelFlaringAndLossesData,
                    fuelFlaringAndLossesOverrideData,
                    netSalesGasData,
                    netSalesGasOverrideData,
                    importedElectricityData,
                    importedElectricityOverrideData,
                    deferredOilData,
                    deferredGasData,
                ],
                caseData?.dG4Date ? new Date(caseData.dG4Date).getFullYear() : endYear,
                setStartYear,
                setEndYear,
                setTableYears,
            )
            setYearRangeSetFromProfiles(true)
        }
    }, [apiData, activeTabCase])

    if (activeTabCase !== 1) { return null }

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    const productionProfilesChartData = () => {
        const dataArray: object[] = []
        if (caseData.dG4Date === undefined) { return dataArray }
        for (let i = startYear; i <= endYear; i += 1) {
            dataArray.push({
                year: i,
                oilProduction: setValueToCorrespondingYear(oilProductionData, i, startYear, new Date(caseData.dG4Date).getFullYear()),
                additionalOilProduction: setValueToCorrespondingYear(additionalOilProductionData, i, startYear, new Date(caseData.dG4Date).getFullYear()),
                gasProduction: setValueToCorrespondingYear(gasProductionData, i, startYear, new Date(caseData.dG4Date).getFullYear()),
                additionalGasProduction: setValueToCorrespondingYear(additionalGasProductionData, i, startYear, new Date(caseData.dG4Date).getFullYear()),
                waterProduction: setValueToCorrespondingYear(waterProductionData, i, startYear, new Date(caseData.dG4Date).getFullYear()),
            })
        }
        return dataArray
    }

    const injectionProfilesChartData = () => {
        const dataArray: object[] = []
        if (caseData.dG4Date === undefined) { return dataArray }
        for (let i = startYear; i <= endYear; i += 1) {
            dataArray.push({
                year: i,
                waterInjection:
                    setValueToCorrespondingYear(waterInjectionData, i, startYear, new Date(caseData.dG4Date).getFullYear()),
            })
        }
        return dataArray
    }

    if (
        !apiData
        || !drainageStrategyData
        || !oilProductionData
        || !additionalOilProductionData
        || !gasProductionData
        || !additionalGasProductionData
        || !waterProductionData
        || !waterInjectionData
        || !fuelFlaringAndLossesData
        || !netSalesGasData
        || !importedElectricityData
        || !deferredOilData
        || !deferredGasData
        || !caseData
    ) {
        console.log("loading")
        return (<CaseProductionProfilesTabSkeleton />)
    }
    return (
        <Grid container spacing={2} style={{ width: "100%" /* workaround to make AgChart behave */ }}>
            <Grid item xs={12} md={6} lg={3}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="case"
                    resourcePropertyKey="facilitiesAvailability"
                    label="Facilities availability"
                    value={caseData.facilitiesAvailability}
                    previousResourceObject={caseData}
                    integer={false}
                    unit="%"
                    min={0}
                    max={100}
                    resourceId={caseData.id}
                />
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
                <SwitchableDropdownInput
                    addEdit={addEdit}
                    resourceName="drainageStrategy"
                    resourcePropertyKey="gasSolution"
                    resourceId={drainageStrategyData.id}
                    value={drainageStrategyData.gasSolution}
                    previousResourceObject={drainageStrategyData}
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
                    addEdit={addEdit}
                    resourceName="case"
                    resourcePropertyKey="producerCount"
                    label="Oil producer wells"
                    value={caseData.producerCount}
                    previousResourceObject={caseData}
                    integer
                    disabled
                    resourceId={caseData.id}

                />
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="case"
                    resourcePropertyKey="waterInjectorCount"
                    label="Water injector wells"
                    value={caseData.waterInjectorCount}
                    previousResourceObject={caseData}
                    integer
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="case"
                    resourcePropertyKey="gasInjectorCount"
                    label="Gas injector wells"
                    value={caseData.gasInjectorCount}
                    previousResourceObject={caseData}
                    integer
                    disabled
                />
            </Grid>
            <DateRangePicker
                setStartYear={setStartYear}
                setEndYear={setEndYear}
                startYear={startYear}
                endYear={endYear}
                handleTableYearsClick={handleTableYearsClick}
            />
            <Grid item xs={12}>
                <AgChartsTimeseries
                    data={productionProfilesChartData()}
                    chartTitle="Production profiles"
                    barColors={["#243746", "#EB0037", "#A8CED1"]}
                    barProfiles={["oilProduction", "additionalOilProduction", "gasProduction", "additionalGasProduction", "waterProduction"]}
                    barNames={[
                        "Oil production (MSm3)",
                        "Additional Oil production (MSm3)",
                        "Gas production (GSm3)",
                        "Additional Gas production (MSm3)",
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
            <Grid item xs={12} style={{ width: "calc(100%+  16px)" }}>
                <CaseProductionProfiles
                    apiData={apiData}
                    tableYears={tableYears}
                    alignedGridsRef={gridRef}
                    addEdit={addEdit}
                />
            </Grid>
        </Grid>
    )
}

export default CaseProductionProfilesTab
