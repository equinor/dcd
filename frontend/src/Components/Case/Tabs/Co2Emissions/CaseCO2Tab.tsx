import {
    useState,
    useEffect,
    useRef,
} from "react"
import { Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid2"
import { useParams } from "react-router"
import { useQuery } from "@tanstack/react-query"

import { AgChartsTimeseries, setValueToCorrespondingYear } from "@/Components/AgGrid/AgChartsTimeseries"
import { SetTableYearsFromProfiles } from "@/Components/Tables/CaseTables/CaseTabTableHelper"
import CaseCo2TabSkeleton from "@/Components/LoadingSkeletons/CaseCo2TabSkeleton"
import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"
import DateRangePicker from "@/Components/Input/TableDateRangePicker"
import CaseTabTable from "@/Components/Tables/CaseTables/CaseTabTable"
import { AgChartsPie } from "@/Components/AgGrid/AgChartsPie"
import { GetGenerateProfileService } from "@/Services/CaseGeneratedProfileService"
import { caseQueryFn } from "@/Services/QueryFunctions"
import { useProjectContext } from "@/Context/ProjectContext"
import { useCaseContext } from "@/Context/CaseContext"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { useDataFetch } from "@/Hooks/useDataFetch"
import CaseCO2DistributionTable from "./Co2EmissionsAgGridTable"
import { getYearFromDateString } from "@/Utils/DateUtils"

interface ICo2DistributionChartData {
    profile: string
    value: number | undefined
}

const CaseCO2Tab = ({ addEdit }: { addEdit: any }) => {
    const { caseId } = useParams()
    const { activeTabCase } = useCaseContext()
    const { projectId, isRevision } = useProjectContext()
    const { revisionId } = useParams()
    const revisionAndProjectData = useDataFetch()

    const { data: apiData } = useQuery({
        queryKey: ["caseApiData", isRevision ? revisionId : projectId, caseId],
        queryFn: () => caseQueryFn(isRevision ? revisionId ?? "" : projectId, caseId),
        enabled: !!projectId && !!caseId,
    })

    const sumValues = (input: Components.Schemas.TimeSeriesCostDto | undefined) => {
        if (!input || input?.values) {
            return 0
        }

        return input.values.reduce((sum, current) => sum + current, 0)
    }

    const caseData = apiData?.case
    const topsideData = apiData?.topside
    const drainageStrategyData = apiData?.drainageStrategy
    const co2EmissionsOverrideData = apiData?.co2EmissionsOverride
    const co2EmissionsData = apiData?.co2Emissions
    const co2IntensityData = apiData?.co2Intensity

    // todo: get co2Intensity, co2IntensityTotal and co2DrillingFlaringFuelTotals stored in backend
    const [co2DistributionChartData, setCo2DistributionChartData] = useState<ICo2DistributionChartData[]>([
        { profile: "Drilling", value: 0 },
        { profile: "Flaring", value: 0 },
        { profile: "Fuel", value: 0 },
    ])

    const [co2DrillingFlaringFuelTotals, setCo2DrillingFlaringFuelTotals] = useState<Components.Schemas.Co2DrillingFlaringFuelTotalsDto>()
    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])
    const [timeSeriesData, setTimeSeriesData] = useState<ITimeSeriesTableData[]>([])
    const [yearRangeSetFromProfiles, setYearRangeSetFromProfiles] = useState<boolean>(false)
    const totalOilProduction = (sumValues(apiData?.productionProfileOil) ?? 0)
        + (sumValues(apiData?.additionalProductionProfileOil) ?? 0)
    const co2GridRef = useRef<any>(null)

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
                    const co2DFFTotal = await (await GetGenerateProfileService()).generateCo2DrillingFlaringFuelTotals(revisionAndProjectData.projectId, caseData.caseId)

                    setCo2DrillingFlaringFuelTotals(co2DFFTotal)

                    if (!yearRangeSetFromProfiles) {
                        SetTableYearsFromProfiles(
                            [co2EmissionsData, await co2IntensityData, co2EmissionsOverrideData?.override ? co2EmissionsOverrideData : undefined],
                            getYearFromDateString(caseData.dG4Date),
                            setStartYear,
                            setEndYear,
                            setTableYears,
                        )
                        setYearRangeSetFromProfiles(true)
                    }
                }
            } catch (error) {
                console.error("[CaseView] Error while generating cost profile", error)
            }
        })()
    }, [activeTabCase, caseData])

    useEffect(() => {
        const newTimeSeriesData: ITimeSeriesTableData[] = [
            {
                profileName: "Annual CO2 emissions",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === 0 ? "MTPA" : "MTPA"}`,
                profile: co2EmissionsData,
                overridable: true,
                editable: true,
                overrideProfile: co2EmissionsOverrideData,
                resourceName: "co2EmissionsOverride",
                resourceId: drainageStrategyData?.id!,
                resourceProfileId: co2EmissionsOverrideData?.id,
                resourcePropertyKey: "co2EmissionsOverride",
            },
            {
                profileName: "Year-by-year CO2 intensity",
                unit: `${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === 0 ? "kg CO2/boe" : "kg CO2/boe"}`,
                profile: co2IntensityData,
                overridable: false,
                editable: false,
                resourceName: "co2Intensity",
                resourceId: drainageStrategyData?.id!,
                resourceProfileId: co2IntensityData?.id,
                resourcePropertyKey: "co2Intensity",
            },
        ]
        setTimeSeriesData(newTimeSeriesData)
    }, [
        co2EmissionsData,
        co2EmissionsOverrideData,
        co2IntensityData,
        co2DrillingFlaringFuelTotals,
    ])

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    const co2EmissionsChartData = () => {
        const dataArray = []
        if (!caseData) { return [{}] }
        const useOverride = co2EmissionsOverrideData && co2EmissionsOverrideData.override
        for (let i = startYear; i <= endYear; i += 1) {
            dataArray.push({
                year: i,
                co2Emissions:
                    setValueToCorrespondingYear(
                        useOverride ? co2EmissionsOverrideData : co2EmissionsData,
                        i,
                        startYear,
                        getYearFromDateString(caseData.dG4Date),
                    ),
                co2Intensity:
                    setValueToCorrespondingYear(
                        co2IntensityData,
                        i,
                        startYear,
                        getYearFromDateString(caseData.dG4Date),
                    ),
            })
        }
        return dataArray
    }

    const co2EmissionsTotalString = () => {
        if (co2EmissionsData) {
            return (Math.round(sumValues(co2EmissionsData) * 10) / 10).toString()
        }
        return "0"
    }

    useEffect(() => {
        setCo2DistributionChartData([
            { profile: "Drilling", value: co2DrillingFlaringFuelTotals?.co2Drilling ?? 0 },
            { profile: "Flaring", value: co2DrillingFlaringFuelTotals?.co2Flaring ?? 0 },
            { profile: "Fuel", value: co2DrillingFlaringFuelTotals?.co2Fuel ?? 0 },
        ])
    }, [co2DrillingFlaringFuelTotals?.co2Flaring])

    if (!topsideData) {
        return <CaseCo2TabSkeleton />
    }

    if (activeTabCase !== 6 || !caseData) { return null }

    return (
        <Grid container spacing={2} style={{ width: "100%" /* workaround to make AgChart behave */ }}>
            <Grid size={12}>
                <Typography>Facility data, Cost and CO2 emissions can be imported using the PROSP import feature in Technical input</Typography>
            </Grid>
            <div>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="topside"
                    resourcePropertyKey="fuelConsumption"
                    resourceId={topsideData.id}
                    label="Fuel consumption"
                    value={topsideData.fuelConsumption}
                    previousResourceObject={topsideData}
                    integer={false}
                    unit="million Sm³ gas/sd"
                />
            </div>
            <Grid size={12}>
                <CaseCO2DistributionTable topside={topsideData} />
            </Grid>
            <Grid size={{ xs: 12, xl: 6 }}>
                <AgChartsTimeseries
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
                    <Typography variant="h1_bold">
                        {sumValues(co2IntensityData) && co2IntensityData?.values && co2IntensityData.values.length > 0
                            ? ((sumValues(co2EmissionsData) * 1000) / (totalOilProduction * 6.29)).toFixed(4)
                            : "0.0000"}
                    </Typography>
                </Grid>
                <Grid size={12}>
                    <Typography color="disabled">kg CO2/boe</Typography>
                </Grid>
                <Grid size={12}>
                    <AgChartsPie
                        data={co2DistributionChartData}
                        chartTitle="CO2 distribution"
                        barColors={["#243746", "#EB0037", "#A8CED1"]}
                        totalCo2Emission={co2EmissionsTotalString()}
                        unit="million tonnes"
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
                <CaseTabTable
                    addEdit={addEdit}
                    timeSeriesData={timeSeriesData}
                    dg4Year={getYearFromDateString(caseData.dG4Date)}
                    tableYears={tableYears}
                    tableName="CO2 emissions"
                    includeFooter={false}
                    gridRef={co2GridRef}
                />
            </Grid>
        </Grid>
    )
}

export default CaseCO2Tab
