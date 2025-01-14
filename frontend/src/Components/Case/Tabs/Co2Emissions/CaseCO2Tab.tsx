import {
    useState,
    useEffect,
    useRef,
} from "react"
import { Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useParams } from "react-router"
import { useQuery } from "@tanstack/react-query"

import { AgChartsTimeseries, setValueToCorrespondingYear } from "@/Components/AgGrid/AgChartsTimeseries"
import { SetTableYearsFromProfiles } from "@/Components/Case/Components/CaseTabTableHelper"
import CaseCo2TabSkeleton from "@/Components/LoadingSkeletons/CaseCo2TabSkeleton"
import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"
import DateRangePicker from "@/Components/Input/TableDateRangePicker"
import CaseTabTable from "@/Components/Case/Components/CaseTabTable"
import { AgChartsPie } from "@/Components/AgGrid/AgChartsPie"
import { GetGenerateProfileService } from "@/Services/CaseGeneratedProfileService"
import { caseQueryFn } from "@/Services/QueryFunctions"
import { useProjectContext } from "@/Context/ProjectContext"
import { useCaseContext } from "@/Context/CaseContext"
import { ITimeSeriesTableData } from "@/Models/ITimeSeries"
import { useDataFetch } from "@/Hooks/useDataFetch"
import CaseCO2DistributionTable from "./Co2EmissionsAgGridTable"
import { roundToFourDecimalsAndJoin } from "@/Utils/common"

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

    const caseData = apiData?.case
    const topsideData = apiData?.topside
    const drainageStrategyData = apiData?.drainageStrategy
    const co2EmissionsOverrideData = apiData?.co2EmissionsOverride
    const co2EmissionsData = apiData?.co2Emissions

    // todo: get co2Intensity, co2IntensityTotal and co2DrillingFlaringFuelTotals stored in backend
    const [co2DistributionChartData, setCo2DistributionChartData] = useState<ICo2DistributionChartData[]>([
        { profile: "Drilling", value: 0 },
        { profile: "Flaring", value: 0 },
        { profile: "Fuel", value: 0 },
    ])
    const [co2Intensity, setCo2Intensity] = useState<Components.Schemas.Co2IntensityDto>()
    const [co2IntensityTotal, setCo2IntensityTotal] = useState<number>(0)
    const [co2DrillingFlaringFuelTotals, setCo2DrillingFlaringFuelTotals] = useState<Components.Schemas.Co2DrillingFlaringFuelTotalsDto>()
    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])
    const [timeSeriesData, setTimeSeriesData] = useState<ITimeSeriesTableData[]>([])
    const [yearRangeSetFromProfiles, setYearRangeSetFromProfiles] = useState<boolean>(false)

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
                    const co2I = (await GetGenerateProfileService()).generateCo2IntensityProfile(revisionAndProjectData.projectId, caseData.caseId)
                    const co2ITotal = await (await GetGenerateProfileService()).generateCo2IntensityTotal(revisionAndProjectData.projectId, caseData.caseId)
                    const co2DFFTotal = await (await GetGenerateProfileService()).generateCo2DrillingFlaringFuelTotals(revisionAndProjectData.projectId, caseData.caseId)

                    setCo2Intensity(await co2I)
                    console.log("co2Intensity", co2Intensity)
                    setCo2IntensityTotal(Number((await co2I).sum))
                    console.log("Co2IntensityTotal", co2IntensityTotal)
                    setCo2DrillingFlaringFuelTotals(co2DFFTotal)

                    if (!yearRangeSetFromProfiles) {
                        SetTableYearsFromProfiles(
                            [co2EmissionsData, await co2I, co2EmissionsOverrideData?.override ? co2EmissionsOverrideData : undefined],
                            caseData.dG4Date ? new Date(caseData.dG4Date).getFullYear() : 2030,
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
                profile: co2Intensity,
                // total: co2IntensityTotal?.toString(),
                overridable: false,
                editable: false,
                resourceName: "co2Intensity",
                resourceId: drainageStrategyData?.id!,
                resourceProfileId: co2Intensity?.id,
                resourcePropertyKey: "co2Intensity",
            },
        ]
        setTimeSeriesData(newTimeSeriesData)
    }, [
        co2EmissionsData,
        co2EmissionsOverrideData,
        co2Intensity,
        co2IntensityTotal,
        co2DrillingFlaringFuelTotals,
    ])

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    const co2EmissionsChartData = () => {
        const dataArray = []
        const useOverride = co2EmissionsOverrideData && co2EmissionsOverrideData.override
        for (let i = startYear; i <= endYear; i += 1) {
            dataArray.push({
                year: i,
                co2Emissions:
                    setValueToCorrespondingYear(
                        useOverride ? co2EmissionsOverrideData : co2EmissionsData,
                        i,
                        startYear,
                        caseData!.dG4Date ? new Date(caseData!.dG4Date).getFullYear() : 2030,
                    ),
                co2Intensity:
                    setValueToCorrespondingYear(
                        co2Intensity,
                        i,
                        startYear,
                        caseData!.dG4Date ? new Date(caseData!.dG4Date).getFullYear() : 2030,
                    ),
            })
        }
        return dataArray
    }

    const co2EmissionsTotalString = () => {
        if (co2EmissionsData) {
            return (Math.round(co2EmissionsData.sum! * 10) / 10).toString()
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
            <Grid item xs={12}>
                <Typography>Facility data, Cost and CO2 emissions can be imported using the PROSP import feature in Technical input</Typography>
            </Grid>
            <Grid item xs={12}>
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
            </Grid>
            <Grid item xs={12}>
                <CaseCO2DistributionTable topside={topsideData} />
            </Grid>
            <Grid item lg={12} xl={6}>
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
            <Grid item lg={12} xl={6} container direction="column" spacing={1} justifyContent="center" alignItems="center">
                <Grid item>
                    <Typography variant="h4">Average lifetime CO2 intensity</Typography>
                </Grid>
                <Grid item>
                    <Typography variant="h1_bold">{Math.round(Number(co2Intensity?.sum) * 10000) / 10000}</Typography>
                </Grid>
                <Grid item>
                    <Typography color="disabled">kg CO2/boe</Typography>
                </Grid>
                <Grid item>
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
            <Grid item xs={12}>
                <CaseTabTable
                    addEdit={addEdit}
                    timeSeriesData={timeSeriesData}
                    dg4Year={caseData.dG4Date ? new Date(caseData.dG4Date).getFullYear() : 2030}
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
