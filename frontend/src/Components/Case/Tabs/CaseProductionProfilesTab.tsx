import {
    useState,
    useRef,
    useEffect,
} from "react"
import { NativeSelect } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid2"

import CaseProductionProfilesTabSkeleton from "@/Components/LoadingSkeletons/CaseProductionProfilesTabSkeleton"
import { AgChartsTimeseries, setValueToCorrespondingYear } from "@/Components/AgGrid/AgChartsTimeseries"
import SwitchableDropdownInput from "@/Components/Input/SwitchableDropdownInput"
import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"
import InputSwitcher from "@/Components/Input/Components/InputSwitcher"
import DateRangePicker from "@/Components/Input/TableDateRangePicker"
import { useCaseStore } from "@/Store/CaseStore"
import { defaultAxesData } from "@/Utils/common"
import CaseProductionProfiles from "./CaseCost/Tables/CaseProductionProfiles"
import { SetTableYearsFromProfiles } from "@/Components/Tables/CaseTables/CaseTabTableHelper"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { useCaseApiData } from "@/Hooks"

const CaseProductionProfilesTab = () => {
    const { activeTabCase } = useCaseStore()
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

    const { apiData } = useCaseApiData()

    useEffect(() => {
        if (apiData && activeTabCase === 1 && !yearRangeSetFromProfiles) {
            SetTableYearsFromProfiles(
                [
                    apiData.drainageStrategy,
                    apiData.productionProfileOil,
                    apiData.additionalProductionProfileOil,
                    apiData.productionProfileGas,
                    apiData.additionalProductionProfileGas,
                    apiData.productionProfileWater,
                    apiData.productionProfileWaterInjection,
                    apiData.productionProfileNgl,
                    apiData.productionProfileNglOverride,
                    apiData.condensateProduction,
                    apiData.condensateProductionOverride,
                    apiData.fuelFlaringAndLosses,
                    apiData.fuelFlaringAndLossesOverride,
                    apiData.netSalesGas,
                    apiData.netSalesGasOverride,
                    apiData.totalExportedVolumes,
                    apiData.totalExportedVolumesOverride,
                    apiData.importedElectricity,
                    apiData.importedElectricityOverride,
                    apiData.deferredOilProduction,
                    apiData.deferredGasProduction,
                ],
                getYearFromDateString(apiData.case.dG4Date),
                setStartYear,
                setEndYear,
                setTableYears,
            )
            setYearRangeSetFromProfiles(true)
        }
    }, [apiData, activeTabCase, tableYears])

    if (activeTabCase !== 1) { return null }

    if (!apiData) {
        return <CaseProductionProfilesTabSkeleton />
    }

    const caseData = apiData.case
    const drainageStrategyData = apiData.drainageStrategy
    const oilProductionData = apiData.productionProfileOil
    const additionalOilProductionData = apiData.additionalProductionProfileOil
    const gasProductionData = apiData.productionProfileGas
    const additionalGasProductionData = apiData.additionalProductionProfileGas
    const waterProductionData = apiData.productionProfileWater
    const waterInjectionData = apiData.productionProfileWaterInjection

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    const productionProfilesChartData = () => {
        const dataArray: object[] = []
        if (caseData.dG4Date === undefined) { return dataArray }
        for (let i = tableYears[0]; i <= tableYears[1]; i += 1) {
            dataArray.push({
                year: i,
                oilProduction: setValueToCorrespondingYear(oilProductionData, i, startYear, getYearFromDateString(caseData.dG4Date)),
                additionalOilProduction: setValueToCorrespondingYear(additionalOilProductionData, i, startYear, getYearFromDateString(caseData.dG4Date)),
                gasProduction: setValueToCorrespondingYear(gasProductionData, i, startYear, getYearFromDateString(caseData.dG4Date)),
                additionalGasProduction: setValueToCorrespondingYear(additionalGasProductionData, i, startYear, getYearFromDateString(caseData.dG4Date)),
                waterProduction: setValueToCorrespondingYear(waterProductionData, i, startYear, getYearFromDateString(caseData.dG4Date)),
            })
        }
        return dataArray
    }

    const injectionProfilesChartData = () => {
        const dataArray: object[] = []
        if (caseData.dG4Date === undefined) { return dataArray }
        for (let i = tableYears[0]; i <= tableYears[1]; i += 1) {
            dataArray.push({
                year: i,
                waterInjection:
                    setValueToCorrespondingYear(waterInjectionData, i, startYear, getYearFromDateString(caseData.dG4Date)),
            })
        }
        return dataArray
    }

    if (!drainageStrategyData || !caseData || !apiData) {
        return (<CaseProductionProfilesTabSkeleton />)
    }
    return (
        <Grid container spacing={2} style={{ width: "100%" /* workaround to make AgChart behave */ }}>
            <Grid container size={12} justifyContent="flex-start">
                <Grid container size={{ xs: 12, md: 10, lg: 8 }} spacing={2}>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            resourceName="case"
                            resourcePropertyKey="facilitiesAvailability"
                            label="Facilities availability"
                            value={caseData.facilitiesAvailability}
                            previousResourceObject={caseData}
                            integer={false}
                            unit="%"
                            min={0}
                            max={100}
                            resourceId={caseData.caseId}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableDropdownInput
                            resourceName="drainageStrategy"
                            resourcePropertyKey="gasSolution"
                            resourceId={drainageStrategyData.id}
                            value={drainageStrategyData.gasSolution}
                            previousResourceObject={drainageStrategyData}
                            options={gasSolutionOptions}
                            label="Gas solution"
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <InputSwitcher
                            value={productionStrategyOptions[caseData.productionStrategyOverview]}
                            label="Production strategy overview"
                        >
                            <NativeSelect
                                id="productionStrategy"
                                label=""
                                disabled
                                value={caseData.productionStrategyOverview}
                                onChange={() => { }}
                            >
                                {Object.entries(productionStrategyOptions).map(([value, label]) => (
                                    <option key={value} value={value}>{label}</option>
                                ))}
                            </NativeSelect>
                        </InputSwitcher>
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <InputSwitcher
                            value={artificialLiftOptions[caseData.artificialLift]}
                            label="Artificial lift"
                        >
                            <NativeSelect
                                id="artificialLift"
                                label=""
                                disabled
                                value={caseData.artificialLift}
                                onChange={() => { }}
                            >
                                {Object.entries(artificialLiftOptions).map(([value, label]) => (
                                    <option key={value} value={value}>{label}</option>
                                ))}
                            </NativeSelect>
                        </InputSwitcher>
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            resourceName="case"
                            resourcePropertyKey="producerCount"
                            label="Oil producer wells"
                            value={caseData.producerCount}
                            previousResourceObject={caseData}
                            integer
                            disabled
                            resourceId={caseData.caseId}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            resourceName="case"
                            resourcePropertyKey="waterInjectorCount"
                            label="Water injector wells"
                            value={caseData.waterInjectorCount}
                            previousResourceObject={caseData}
                            integer
                            disabled
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            resourceName="case"
                            resourcePropertyKey="gasInjectorCount"
                            label="Gas injector wells"
                            value={caseData.gasInjectorCount}
                            previousResourceObject={caseData}
                            integer
                            disabled
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            resourceName="drainageStrategy"
                            resourcePropertyKey="nglYield"
                            label="NGL yield"
                            value={drainageStrategyData.nglYield}
                            previousResourceObject={drainageStrategyData}
                            resourceId={drainageStrategyData.id}
                            integer
                            unit="tonnes/MSm³"
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            resourceName="drainageStrategy"
                            resourcePropertyKey="condensateYield"
                            label="Condensate yield"
                            value={drainageStrategyData.condensateYield}
                            previousResourceObject={drainageStrategyData}
                            resourceId={drainageStrategyData.id}
                            integer
                            unit="Sm³/MSm³"
                        />
                    </Grid>
                    {(drainageStrategyData.nglYield > 0 || drainageStrategyData.condensateYield > 0) && (
                        <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                            <SwitchableNumberInput
                                resourceName="drainageStrategy"
                                resourcePropertyKey="gasShrinkageFactor"
                                label="Gas shrinkage factor"
                                value={drainageStrategyData.gasShrinkageFactor}
                                previousResourceObject={drainageStrategyData}
                                resourceId={drainageStrategyData.id}
                                integer
                            />
                        </Grid>
                    )}
                </Grid>
            </Grid>

            <DateRangePicker
                setStartYear={setStartYear}
                setEndYear={setEndYear}
                startYear={startYear}
                endYear={endYear}
                handleTableYearsClick={handleTableYearsClick}
            />
            <Grid size={12}>
                <AgChartsTimeseries
                    data={productionProfilesChartData()}
                    chartTitle="Production profiles"
                    axesData={defaultAxesData}
                    barColors={["#243746", "#EB0037", "#A8CED1"]}
                    barProfiles={["oilProduction", "additionalOilProduction", "gasProduction", "additionalGasProduction", "waterProduction"]}
                    barNames={[
                        "Oil production (MSm3)",
                        "Additional Oil production (MSm3)",
                        "Rich gas production (GSm3)",
                        "Additional rich gas production (GSm3)",
                        "Water production (MSm3)",
                    ]}
                />
            </Grid>
            {
                (waterInjectionData?.values && waterInjectionData.values?.length > 0)
                && (
                    <Grid size={12}>
                        <AgChartsTimeseries
                            axesData={defaultAxesData}
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
            <Grid size={12} style={{ width: "calc(100%+  16px)" }}>
                <CaseProductionProfiles
                    apiData={apiData}
                    tableYears={tableYears}
                    alignedGridsRef={gridRef}
                />
            </Grid>
        </Grid>
    )
}

export default CaseProductionProfilesTab
