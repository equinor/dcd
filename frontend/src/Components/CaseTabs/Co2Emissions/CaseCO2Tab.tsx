import { Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid2"
import {
    useState,
    useEffect,
    useRef,
} from "react"

import PROSPBar from "../Components/PROSPBar"

import CaseCO2DistributionTable from "./Co2EmissionsAgGridTable"
import Co2EmissionsTable from "./Co2EmissionsTable"

import { PieChart } from "@/Components/CaseTabs/Co2Emissions/PieChart"
import { TimeSeriesChart, setValueToCorrespondingYear } from "@/Components/Charts/TimeSeriesChart"
import DateRangePicker from "@/Components/Input/TableDateRangePicker"
import CaseCo2TabSkeleton from "@/Components/LoadingSkeletons/CaseCo2TabSkeleton"
import CaseBaseTable from "@/Components/Tables/CaseBaseTable"
import { useDataFetch, useCaseApiData, useDefaultYearRanges } from "@/Hooks"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { PhysUnit, ProfileTypes } from "@/Models/enums"
import { GetGenerateProfileService } from "@/Services/CaseGeneratedProfileService"
import { useCaseStore } from "@/Store/CaseStore"
import { useProjectContext } from "@/Store/ProjectContext"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { formatNumberForView } from "@/Utils/FormatingUtils"
import { calculateTableYears } from "@/Utils/TableUtils"

interface ICo2DistributionChartData {
    profile: string
    value: number | undefined
}

const CaseCO2Tab = (): React.ReactNode => {
    const { activeTabCase } = useCaseStore()
    const { projectId } = useProjectContext()
    const revisionAndProjectData = useDataFetch()
    const { apiData } = useCaseApiData()
    const { DEFAULT_CO2_EMISSIONS_YEARS } = useDefaultYearRanges()

    const caseData = apiData?.case
    const topsideData = apiData?.topside
    const drainageStrategyData = apiData?.drainageStrategy

    const co2EmissionsData = apiData?.co2Emissions
    const co2EmissionsOverrideData = apiData?.co2EmissionsOverride

    const co2IntensityData = apiData?.co2Intensity
    const co2IntensityOverrideData = apiData?.co2IntensityOverride

    const [co2DistributionChartData, setCo2DistributionChartData] = useState<ICo2DistributionChartData[]>([
        { profile: "Drilling", value: 0 },
        { profile: "Flaring", value: 0 },
        { profile: "Fuel", value: 0 },
    ])

    const [co2DrillingFlaringFuelTotals, setCo2DrillingFlaringFuelTotals] = useState<Components.Schemas.Co2DrillingFlaringFuelTotalsDto>()
    const [startYear, setStartYear] = useState<number>(DEFAULT_CO2_EMISSIONS_YEARS[0])
    const [endYear, setEndYear] = useState<number>(DEFAULT_CO2_EMISSIONS_YEARS[1])
    const [tableYears, setTableYears] = useState<[number, number]>(DEFAULT_CO2_EMISSIONS_YEARS)
    const [timeSeriesData, setTimeSeriesData] = useState<ITimeSeriesTableData[]>([])
    const co2GridRef = useRef<any>(null)
    const averageCo2IntensityData = apiData?.case.averageCo2Intensity

    const co2IntensityLine = {
        type: "line",
        xKey: "year",
        yKey: "co2Intensity",
        yName: "Year-by-year CO2 intensity (kg CO2/boe)",
    }

    // Special configuration for CO2 chart with dual axes
    const dualAxesData = [
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
            label: {
                formatter: (label: { value: number }): string => Math.floor(Number(label.value)).toString(),
            },
        },
        {
            type: "number",
            position: "left",
            keys: ["co2Emissions"],
            title: {
                text: "CO2 emissions",
            },
            nice: true,
            label: {
                formatter: (params: { value: number }): string => formatNumberForView(params.value),
            },
        },
        {
            type: "number",
            position: "right",
            keys: ["co2Intensity"],
            title: {
                text: "Year-by-year CO2 intensity",
            },
            nice: true,
            label: {
                formatter: (params: { value: number }): string => formatNumberForView(params.value),
            },
        },
    ]

    useEffect(() => {
        (async (): Promise<void> => {
            try {
                if (caseData && revisionAndProjectData && activeTabCase === 6 && caseData.caseId) {
                    const co2DFFTotal = await GetGenerateProfileService().generateCo2DrillingFlaringFuelTotals(revisionAndProjectData.projectId, caseData.caseId)

                    setCo2DrillingFlaringFuelTotals(co2DFFTotal)

                    const profiles = [
                        co2EmissionsData,
                        co2EmissionsOverrideData?.override ? co2EmissionsOverrideData : undefined,
                        co2IntensityData,
                        co2IntensityOverrideData?.override ? co2IntensityOverrideData : undefined,
                    ]
                    const dg4Year = getYearFromDateString(caseData.dg4Date)
                    const years = calculateTableYears(profiles, dg4Year, DEFAULT_CO2_EMISSIONS_YEARS)

                    const [firstYear, lastYear] = years

                    setStartYear(firstYear)
                    setEndYear(lastYear)
                    setTableYears([firstYear, lastYear])
                }
            } catch (error) {
                console.error("[CaseView] Error while generating cost profile", error)
            }
        })()
    }, [activeTabCase, caseData, co2EmissionsData, co2IntensityData, co2EmissionsOverrideData, revisionAndProjectData])

    useEffect(() => {
        if (drainageStrategyData) {
            const newTimeSeriesData: ITimeSeriesTableData[] = [
                {
                    profileName: "Annual CO2 emissions",
                    unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.Si ? "MTPA" : "MTPA"}`,
                    profile: co2EmissionsData,
                    overridable: true,
                    editable: true,
                    overrideProfile: co2EmissionsOverrideData,
                    resourceName: ProfileTypes.Co2EmissionsOverride,
                    resourceId: drainageStrategyData.id,
                    resourcePropertyKey: ProfileTypes.Co2EmissionsOverride,
                },
                {
                    profileName: "Year-by-year CO2 intensity",
                    unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.Si ? "kg CO2/boe" : "kg CO2/boe"}`,
                    profile: co2IntensityData,
                    overridable: true,
                    editable: true,
                    overrideProfile: co2IntensityOverrideData,
                    resourceName: ProfileTypes.Co2IntensityOverride,
                    resourceId: drainageStrategyData.id,
                    resourcePropertyKey: ProfileTypes.Co2IntensityOverride,
                },
            ]

            setTimeSeriesData(newTimeSeriesData)
        }
    }, [
        co2EmissionsData,
        co2EmissionsOverrideData,
        co2IntensityData,
        co2IntensityOverrideData,
        co2DrillingFlaringFuelTotals,
    ])

    const handleTableYearsClick = (): void => {
        setTableYears([startYear, endYear])
    }

    const formatValue = (num: number | null | undefined): number => {
        if (num === null || num === undefined) { return 0 }
        if (num === 0) { return 0 }

        return Number(num)
    }

    type Co2EmissionsChartData = {
        year: number
        co2Emissions: number
        co2Intensity: number
    }

    const co2EmissionsChartData = (): Co2EmissionsChartData[] => {
        const dataArray: Co2EmissionsChartData[] = []

        if (!caseData) { return [{ year: 0, co2Emissions: 0, co2Intensity: 0 }] }
        const useCo2EmissionOverride = co2EmissionsOverrideData && co2EmissionsOverrideData.override
        const useCo2IntensityOverride = co2IntensityOverrideData && co2IntensityOverrideData.override

        for (let i = tableYears[0]; i <= tableYears[1]; i += 1) {
            const emissionsValue = setValueToCorrespondingYear(
                useCo2EmissionOverride ? co2EmissionsOverrideData : co2EmissionsData,
                i,
                getYearFromDateString(caseData.dg4Date),
            )

            const intensityValue = setValueToCorrespondingYear(
                useCo2IntensityOverride ? co2IntensityOverrideData : co2IntensityData,
                i,
                getYearFromDateString(caseData.dg4Date),
            )

            dataArray.push({
                year: i,
                co2Emissions: emissionsValue || 0,
                co2Intensity: intensityValue || 0,
            })
        }

        return dataArray
    }

    const [drillingPortion, setDrillingPortion] = useState(0)
    const [flaringPortion, setFlaringPortion] = useState(0)
    const [fuelPortion, setFuelPortion] = useState(0)

    useEffect(() => {
        if (averageCo2IntensityData && co2DrillingFlaringFuelTotals) {
            const { co2Drilling = 0, co2Flaring = 0, co2Fuel = 0 } = co2DrillingFlaringFuelTotals

            const totalEmissions = co2Drilling + co2Flaring + co2Fuel

            if (totalEmissions > 0) {
                const drillingPercentage = co2Drilling / totalEmissions
                const flaringPercentage = co2Flaring / totalEmissions
                const fuelPercentage = co2Fuel / totalEmissions

                setDrillingPortion(drillingPercentage * averageCo2IntensityData)
                setFlaringPortion(flaringPercentage * averageCo2IntensityData)
                setFuelPortion(fuelPercentage * averageCo2IntensityData)
            } else {
                const defaultIntensity = averageCo2IntensityData || 1

                setDrillingPortion(defaultIntensity * 0.33)
                setFlaringPortion(defaultIntensity * 0.33)
                setFuelPortion(defaultIntensity * 0.34)
            }
        } else {
            setDrillingPortion(0.33)
            setFlaringPortion(0.33)
            setFuelPortion(0.34)
        }
    }, [averageCo2IntensityData, co2DrillingFlaringFuelTotals])

    useEffect(() => {
        const formattedDrilling = Math.max(0, formatValue(drillingPortion))
        const formattedFlaring = Math.max(0, formatValue(flaringPortion))
        const formattedFuel = Math.max(0, formatValue(fuelPortion))

        const hasNonZeroValues = formattedDrilling > 0 || formattedFlaring > 0 || formattedFuel > 0

        const chartData = hasNonZeroValues ? [
            { profile: "Drilling", value: formattedDrilling },
            { profile: "Flaring", value: formattedFlaring },
            { profile: "Fuel", value: formattedFuel },
        ] : [
            { profile: "Drilling", value: 1 },
            { profile: "Flaring", value: 1 },
            { profile: "Fuel", value: 1 },
        ]

        setCo2DistributionChartData(chartData)
    }, [drillingPortion, flaringPortion, fuelPortion])

    if (!topsideData) {
        return <CaseCo2TabSkeleton />
    }

    if (activeTabCase !== 6 || !caseData) { return null }

    return (
        <Grid container spacing={2} style={{ width: "100%" /* workaround to make AgChart behave */ }}>
            <Grid size={12}>
                <Typography>Facility data, Cost and CO2 emissions can be imported using the PROSP import feature in Technical input</Typography>
            </Grid>
            <Grid size={12}>
                <CaseCO2DistributionTable topside={topsideData} />
            </Grid>
            <Grid size={{ xs: 12, xl: 6 }}>
                <TimeSeriesChart
                    data={co2EmissionsChartData()}
                    chartTitle="Annual CO2 emissions"
                    barColors={["#E24973", "#FF92A8"]}
                    barProfiles={["co2Emissions"]}
                    barNames={["Annual CO2 emissions (million tonnes)"]}
                    lineChart={co2IntensityLine}
                    axesData={dualAxesData}
                />
            </Grid>
            <Grid size={{ xs: 12, xl: 6 }} container direction="column" spacing={1} justifyContent="center" alignItems="center">

                <Typography variant="h4">Average lifetime CO2 intensity</Typography>

                <Typography variant="h4">
                    {averageCo2IntensityData ? formatNumberForView(averageCo2IntensityData) : "0"}
                    {" "}
                    kg CO2/boe
                </Typography>

                <Grid size={12}>
                    <PieChart
                        data={co2DistributionChartData}
                        chartTitle="CO2 intensity distribution"
                        barColors={["#EB0037", "#A8CED1", "#243746"]}
                        unit="kg CO2/boe"
                        enableLegend
                    />
                </Grid>
            </Grid>
            <DateRangePicker
                startYear={startYear}
                endYear={endYear}
                setStartYear={setStartYear}
                setEndYear={setEndYear}
                handleTableYearsClick={handleTableYearsClick}
            />
            <Grid size={12}>
                <CaseBaseTable
                    timeSeriesData={timeSeriesData}
                    dg4Year={getYearFromDateString(caseData.dg4Date)}
                    tableYears={tableYears}
                    tableName="CO2 emissions"
                    includeFooter={false}
                    gridRef={co2GridRef}
                    decimalPrecision={4}
                />
            </Grid>
            <Grid size={12}>
                <Co2EmissionsTable />
            </Grid>
            <PROSPBar
                projectId={projectId}
                caseId={caseData.caseId}
                currentSharePointFileId={caseData.sharepointFileId || null}
            />
        </Grid>
    )
}

export default CaseCO2Tab
