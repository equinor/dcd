import Grid from "@mui/material/Grid2"
import {
    useState,
    useRef,
    useEffect,
} from "react"

import CaseProductionProfiles from "./CaseCost/Tables/CaseProductionProfiles"

import { TimeSeriesChart, setValueToCorrespondingYear } from "@/Components/Charts/TimeSeriesChart"
import SwitchableDropdownInput from "@/Components/Input/SwitchableDropdownInput"
import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"
import DateRangePicker from "@/Components/Input/TableDateRangePicker"
import CaseProductionProfilesTabSkeleton from "@/Components/LoadingSkeletons/CaseProductionProfilesTabSkeleton"
import { useCaseApiData, useDataFetch, useTableRanges } from "@/Hooks"
import { useDrainageStrategyMutation, useCaseMutation } from "@/Hooks/Mutations"
import { PhysUnit } from "@/Models/enums"
import { useAppStore } from "@/Store/AppStore"
import { useCaseStore } from "@/Store/CaseStore"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { formatNumberForView, roundToDecimals } from "@/Utils/FormatingUtils"
import { calculateTableYears } from "@/Utils/TableUtils"

const defaultAxesData = [
    {
        type: "category",
        position: "bottom",
        nice: true,
        gridLine: {
            style: [
                {
                    stroke: "rgba(0, 0, 0, 0.2)",
                    lineDash: [3, 2],
                },
                {
                    stroke: "rgba(0, 0, 0, 0.2)",
                    lineDash: [3, 2],
                },
            ],
        },
    },
    {
        type: "number",
        position: "left",
        nice: true,
        label: {
            formatter: (params: { value: number }): string => formatNumberForView(roundToDecimals(params.value, 4)),
        },
        gridLine: {
            style: [
                {
                    stroke: "rgba(0, 0, 0, 0.1)",
                    lineDash: [3, 2],
                },
                {
                    stroke: "rgba(0, 0, 0, 0.1)",
                    lineDash: [3, 2],
                },
            ],
        },
    },
]

// Helper to check if a profile has values
const hasProfileValues = (profile: any): boolean => profile && "values" in profile && Array.isArray(profile.values) && profile.values.length > 0

const CaseProductionProfilesTab = (): React.ReactNode => {
    const { editMode } = useAppStore()
    const { activeTabCase } = useCaseStore()
    const { tableRanges, updateProductionProfilesYears } = useTableRanges()

    const [startYear, setStartYear] = useState<number>(0)
    const [endYear, setEndYear] = useState<number>(0)
    const [tableYears, setTableYears] = useState<[number, number]>([0, 0])
    const revisionAndProjectData = useDataFetch()
    const {
        updateGasSolution,
        updateNglYield,
        updateCondensateYield,
        updateGasShrinkageFactor,
    } = useDrainageStrategyMutation()
    const {
        updateFacilitiesAvailability,
        updateProductionStrategyOverview,
        updateArtificialLift,
        updateProducerCount,
        updateWaterInjectorCount,
        updateGasInjectorCount,
    } = useCaseMutation()

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
        if (apiData && activeTabCase === 1 && tableRanges) {
            // Check if we have productionProfilesYears from the backend
            if (tableRanges.productionProfilesYears && tableRanges.productionProfilesYears.length >= 2) {
                const firstYear = Math.min(...tableRanges.productionProfilesYears)
                const lastYear = Math.max(...tableRanges.productionProfilesYears)

                setStartYear(firstYear)
                setEndYear(lastYear)
                setTableYears([firstYear, lastYear])

                return
            }

            // Fall back to calculating based on profiles if needed
            const profiles = [
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
            ]

            const dg4Year = getYearFromDateString(apiData.case.dg4Date)

            // Define initial years
            let yearRange: [number, number] = [dg4Year - 5, dg4Year + 25]

            // Calculate years based on profile data if available
            if (profiles.some((p) => hasProfileValues(p))) {
                yearRange = calculateTableYears(profiles, dg4Year, yearRange)
            }

            setStartYear(yearRange[0])
            setEndYear(yearRange[1])
            setTableYears(yearRange)
        }
    }, [apiData, activeTabCase, tableRanges])

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

    const handleTableYearsClick = async (): Promise<void> => {
        setTableYears([startYear, endYear])

        // Update the backend with the new range
        try {
            await updateProductionProfilesYears(startYear, endYear)
        } catch (error) {
            console.error("CaseProductionProfilesTab - Error updating production profiles years:", error)
        }
    }

    const getProductionProfilesChartData = (): object[] => {
        const dataArray: object[] = []

        if (caseData.dg4Date === undefined) { return dataArray }

        for (let i = tableYears[0]; i <= tableYears[1]; i += 1) {
            dataArray.push({
                year: i,
                oilProduction: setValueToCorrespondingYear(oilProductionData, i, getYearFromDateString(caseData.dg4Date)),
                additionalOilProduction: setValueToCorrespondingYear(additionalOilProductionData, i, getYearFromDateString(caseData.dg4Date)),
                gasProduction: setValueToCorrespondingYear(gasProductionData, i, getYearFromDateString(caseData.dg4Date)),
                additionalGasProduction: setValueToCorrespondingYear(additionalGasProductionData, i, getYearFromDateString(caseData.dg4Date)),
                waterProduction: setValueToCorrespondingYear(waterProductionData, i, getYearFromDateString(caseData.dg4Date)),
            })
        }

        return dataArray
    }

    const injectionProfilesChartData = (): object[] => {
        const dataArray: object[] = []

        if (caseData.dg4Date === undefined) { return dataArray }

        for (let i = tableYears[0]; i <= tableYears[1]; i += 1) {
            dataArray.push({
                year: i,
                waterInjection: setValueToCorrespondingYear(waterInjectionData, i, getYearFromDateString(caseData.dg4Date)),
            })
        }

        return dataArray
    }

    if (!drainageStrategyData || !caseData || !apiData || !revisionAndProjectData) {
        return (<CaseProductionProfilesTabSkeleton />)
    }

    const getPhysicalUnit = (): string => (revisionAndProjectData.commonProjectAndRevisionData.physicalUnit === PhysUnit.Si ? "SI" : "Oil field")

    return (
        <Grid container spacing={2} style={{ width: "100%" /* workaround to make AgChart behave */ }}>
            <Grid container size={12} justifyContent="flex-start">
                <Grid container size={{ xs: 12, md: 10, lg: 8 }} spacing={2}>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            label="Facilities availability"
                            value={caseData.facilitiesAvailability}
                            integer={false}
                            unit="%"
                            min={0}
                            max={100}
                            id={`case-facilities-availability-${caseData.caseId}`}
                            onSubmit={updateFacilitiesAvailability}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableDropdownInput
                            value={drainageStrategyData.gasSolution}
                            options={gasSolutionOptions}
                            label="Gas solution"
                            id={`drainage-strategy-gas-solution-${drainageStrategyData.id}`}
                            onSubmit={updateGasSolution}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableDropdownInput
                            value={caseData.productionStrategyOverview}
                            options={productionStrategyOptions}
                            label="Production strategy overview"
                            disabled
                            id={`case-production-strategy-overview-${caseData.caseId}`}
                            onSubmit={updateProductionStrategyOverview}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableDropdownInput
                            value={caseData.artificialLift}
                            options={artificialLiftOptions}
                            label="Artificial lift"
                            id={`case-artificial-lift-${caseData.caseId}`}
                            disabled
                            onSubmit={updateArtificialLift}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            label="Oil producer wells"
                            value={caseData.producerCount}
                            integer
                            disabled
                            id={`case-producer-count-${caseData.caseId}`}
                            onSubmit={updateProducerCount}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            label="Water injector wells"
                            value={caseData.waterInjectorCount}
                            integer
                            disabled
                            id={`case-water-injector-count-${caseData.caseId}`}
                            onSubmit={updateWaterInjectorCount}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            label="Gas injector wells"
                            value={caseData.gasInjectorCount}
                            integer
                            disabled
                            id={`case-gas-injector-count-${caseData.caseId}`}
                            onSubmit={updateGasInjectorCount}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            label="NGL yield"
                            value={drainageStrategyData.nglYield}
                            id={`drainage-strategy-ngl-yield-${drainageStrategyData.id}`}
                            integer
                            unit={getPhysicalUnit() === "SI" ? "tonnes/MSm³" : "tonnes/mmscf"}
                            onSubmit={updateNglYield}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            label="Condensate yield"
                            value={drainageStrategyData.condensateYield}
                            id={`drainage-strategy-condensate-yield-${drainageStrategyData.id}`}
                            integer
                            unit={getPhysicalUnit() === "SI" ? "Sm³/MSm³" : "bbls/mmscf"}
                            onSubmit={updateCondensateYield}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        {editMode ? (
                            <SwitchableNumberInput
                                label="Gas shrinkage factor"
                                value={drainageStrategyData.gasShrinkageFactor}
                                id={`drainage-strategy-gas-shrinkage-factor-${drainageStrategyData.id}`}
                                integer
                                min={0}
                                max={100}
                                unit="%"
                                onSubmit={updateGasShrinkageFactor}
                            />
                        ) : (drainageStrategyData.nglYield > 0 || drainageStrategyData.condensateYield > 0) && (
                            <SwitchableNumberInput
                                label="Gas shrinkage factor"
                                value={drainageStrategyData.gasShrinkageFactor}
                                id={`drainage-strategy-gas-shrinkage-factor-${drainageStrategyData.id}`}
                                integer
                                min={0}
                                max={100}
                                unit="%"
                                onSubmit={updateGasShrinkageFactor}
                            />
                        )}
                    </Grid>
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
                <TimeSeriesChart
                    data={getProductionProfilesChartData()}
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
                        <TimeSeriesChart
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
