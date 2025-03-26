import { Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid2"
import {
    useState,
    useEffect,
    useRef,
} from "react"

import CaseCO2DistributionTable from "./Co2EmissionsAgGridTable"
import Co2EmissionsTable from "./Co2EmissionsTable"

import { PieChart } from "@/Components/CaseTabs/Co2Emissions/PieChart"
import { TimeSeriesChart, setValueToCorrespondingYear } from "@/Components/Charts/TimeSeriesChart"
import DateRangePicker from "@/Components/Input/TableDateRangePicker"
import CaseCo2TabSkeleton from "@/Components/LoadingSkeletons/CaseCo2TabSkeleton"
import CaseBaseTable from "@/Components/Tables/CaseBaseTable"
import { useDataFetch, useCaseApiData } from "@/Hooks"
import { useTopsideMutation } from "@/Hooks/Mutations"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { PhysUnit, ProfileTypes } from "@/Models/enums"
import { GetGenerateProfileService } from "@/Services/CaseGeneratedProfileService"
import { useCaseStore } from "@/Store/CaseStore"
import { DEFAULT_CO2_EMISSIONS_YEARS } from "@/Utils/Config/constants"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { calculateTableYears } from "@/Utils/TableUtils"

interface ICo2DistributionChartData {
    profile: string
    value: number | undefined
}

const CaseCO2Tab = () => {
    const { activeTabCase } = useCaseStore()
    const revisionAndProjectData = useDataFetch()
    const { apiData } = useCaseApiData()
    const { updateFuelConsumption } = useTopsideMutation()

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

    const chartAxes = [
        {
            type: "category",
            position: "bottom",
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
                formatter: (label: any) => Math.floor(Number(label.value)),
            },
        },
        {
            type: "number",
            position: "left",
            keys: ["co2Emissions"],
            title: {
                text: "CO2 emissions",
            },
            label: {
                formatter: (params: any) => `${params.value}`, // emission values
            },
        },
        {
            type: "number",
            position: "right",
            keys: ["co2Intensity"],
            title: {
                text: "Year-by-year CO2 intensity",
            },
            label: {
                formatter: (params: any) => `${params.value}`, // intensity values
            },
        },
    ]

    useEffect(() => {
        (async () => {
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
                    const years = calculateTableYears(profiles, dg4Year)

                    if (years) {
                        const [firstYear, lastYear] = years

                        setStartYear(firstYear)
                        setEndYear(lastYear)
                        setTableYears([firstYear, lastYear])
                    } else {
                        setStartYear(DEFAULT_CO2_EMISSIONS_YEARS[0])
                        setEndYear(DEFAULT_CO2_EMISSIONS_YEARS[1])
                        setTableYears(DEFAULT_CO2_EMISSIONS_YEARS)
                    }
                }
            } catch (error) {
                console.error("[CaseView] Error while generating cost profile", error)
            }
        })()
    }, [activeTabCase, caseData, co2EmissionsData, co2IntensityData, co2EmissionsOverrideData, revisionAndProjectData])

    useEffect(() => {
        const newTimeSeriesData: ITimeSeriesTableData[] = [
            {
                profileName: "Annual CO2 emissions",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === PhysUnit.Si ? "MTPA" : "MTPA"}`,
                profile: co2EmissionsData,
                overridable: true,
                editable: true,
                overrideProfile: co2EmissionsOverrideData,
                resourceName: ProfileTypes.Co2EmissionsOverride,
                resourceId: drainageStrategyData?.id!,
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
                resourceId: drainageStrategyData?.id!,
                resourcePropertyKey: ProfileTypes.Co2IntensityOverride,
            },
        ]

        setTimeSeriesData(newTimeSeriesData)
    }, [
        co2EmissionsData,
        co2EmissionsOverrideData,
        co2IntensityData,
        co2IntensityOverrideData,
        co2DrillingFlaringFuelTotals,
    ])

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    const formatValue = (num: number | null | undefined) => (num === 0 ? 0 : Number((num ?? 0).toFixed(4)))

    const co2EmissionsChartData = () => {
        const dataArray = []

        if (!caseData) { return [{}] }
        const useCo2EmissionOverride = co2EmissionsOverrideData && co2EmissionsOverrideData.override
        const useCo2IntensityOverride = co2IntensityOverrideData && co2IntensityOverrideData.override

        for (let i = tableYears[0]; i <= tableYears[1]; i += 1) {
            dataArray.push({
                year: i,
                co2Emissions: formatValue(
                    setValueToCorrespondingYear(
                        useCo2EmissionOverride ? co2EmissionsOverrideData : co2EmissionsData,
                        i,
                        getYearFromDateString(caseData.dg4Date),
                    ),
                ),
                co2Intensity: formatValue(
                    setValueToCorrespondingYear(
                        useCo2IntensityOverride ? co2IntensityOverrideData : co2IntensityData,
                        i,
                        getYearFromDateString(caseData.dg4Date),
                    ),
                ),
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
                setDrillingPortion(0)
                setFlaringPortion(0)
                setFuelPortion(0)
            }
        } else {
            setDrillingPortion(0)
            setFlaringPortion(0)
            setFuelPortion(0)
        }
    }, [averageCo2IntensityData, co2DrillingFlaringFuelTotals])

    useEffect(() => {
        setCo2DistributionChartData([
            { profile: "Drilling", value: formatValue(drillingPortion) },
            { profile: "Flaring", value: formatValue(flaringPortion) },
            { profile: "Fuel", value: formatValue(fuelPortion) },
        ])
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
                    axesData={chartAxes}
                />
            </Grid>
            <Grid size={{ xs: 12, xl: 6 }} container direction="column" spacing={1} justifyContent="center" alignItems="center">
                <Grid size={12}>
                    <Typography variant="h4">Average lifetime CO2 intensity</Typography>
                </Grid>
                <Grid size={12}>
                    <Typography variant="h4">
                        {averageCo2IntensityData?.toFixed(4)}
                        {" "}
                        kg CO2/boe
                    </Typography>
                </Grid>

                <Grid size={12}>
                    <PieChart
                        data={co2DistributionChartData}
                        chartTitle="CO2 intensity distribution"
                        barColors={["#EB0037", "#A8CED1", "#243746"]}
                        unit="kg CO2/boe"
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
        </Grid>
    )
}

export default CaseCO2Tab
