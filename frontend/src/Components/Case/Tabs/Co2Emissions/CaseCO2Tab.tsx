import {
    useState,
    useEffect,
    useRef,
} from "react"
import { Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useParams } from "react-router"
import { useQueryClient, useQuery } from "react-query"
import SwitchableNumberInput from "../../../Input/SwitchableNumberInput"
import CaseTabTable from "../../Components/CaseTabTable"
import { SetTableYearsFromProfiles } from "../../Components/CaseTabTableHelper"
import { GetGenerateProfileService } from "../../../../Services/CaseGeneratedProfileService"
import CaseCO2DistributionTable from "./Co2EmissionsAgGridTable"
import { AgChartsTimeseries, setValueToCorrespondingYear } from "../../../AgGrid/AgChartsTimeseries"
import { AgChartsPie } from "../../../AgGrid/AgChartsPie"
import { useProjectContext } from "../../../../Context/ProjectContext"
import { useCaseContext } from "../../../../Context/CaseContext"
import DateRangePicker from "../../../Input/TableDateRangePicker"
import { ITimeSeriesData } from "../../../../Models/Interfaces"

const CaseCO2Tab = () => {
    const { project } = useProjectContext()
    const { caseId } = useParams()
    const queryClient = useQueryClient()
    const projectId = project?.id || null
    const { activeTabCase } = useCaseContext()

    const { data: apiData } = useQuery<Components.Schemas.CaseWithAssetsDto | undefined>(
        ["apiData", { projectId, caseId }],
        () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        {
            enabled: !!projectId && !!caseId,
            initialData: () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        },
    )

    const caseData = apiData?.case
    const topsideData = apiData?.topside as Components.Schemas.TopsideWithProfilesDto
    const drainageStrategyData = apiData?.drainageStrategy as Components.Schemas.DrainageStrategyWithProfilesDto
    const co2EmissionsOverrideData = apiData?.co2EmissionsOverride
    const co2EmissionsData = apiData?.co2Emissions as Components.Schemas.Co2EmissionsDto

    // todo: get co2Intensity, co2IntensityTotal and co2DrillingFlaringFuelTotals stored in backend
    const [co2Intensity, setCo2Intensity] = useState<Components.Schemas.Co2IntensityDto>()
    const [co2IntensityTotal, setCo2IntensityTotal] = useState<number>(0)
    const [co2DrillingFlaringFuelTotals, setCo2DrillingFlaringFuelTotals] = useState<Components.Schemas.Co2DrillingFlaringFuelTotalsDto>()
    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])
    const [timeSeriesData, setTimeSeriesData] = useState<ITimeSeriesData[]>([])

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
                if (caseData && project && activeTabCase === 6 && caseData.id) {
                    const co2I = (await GetGenerateProfileService()).generateCo2IntensityProfile(project.id, caseData.id)
                    const co2ITotal = await (await GetGenerateProfileService()).generateCo2IntensityTotal(project.id, caseData.id)
                    const co2DFFTotal = await (await GetGenerateProfileService()).generateCo2DrillingFlaringFuelTotals(project.id, caseData.id)

                    setCo2Intensity(await co2I)
                    setCo2IntensityTotal(Number(co2ITotal))
                    setCo2DrillingFlaringFuelTotals(co2DFFTotal)

                    SetTableYearsFromProfiles(
                        [co2EmissionsData, await co2I, co2EmissionsOverrideData?.override ? co2EmissionsOverrideData : undefined],
                        caseData.dG4Date ? new Date(caseData.dG4Date).getFullYear() : 2030,
                        setStartYear,
                        setEndYear,
                        setTableYears,
                    )
                }
            } catch (error) {
                console.error("[CaseView] Error while generating cost profile", error)
            }
        })()
    }, [activeTabCase])

    useEffect(() => {
        const newTimeSeriesData: ITimeSeriesData[] = [
            {
                profileName: "Annual CO2 emissions",
                unit: `${project?.physicalUnit === 0 ? "MTPA" : "MTPA"}`,
                profile: co2EmissionsData,
                overridable: true,
                editable: true,
                overrideProfile: co2EmissionsOverrideData,
                resourceName: "co2EmissionsOverride",
                resourceId: drainageStrategyData.id,
                resourceProfileId: co2EmissionsOverrideData?.id,
                resourcePropertyKey: "co2EmissionsOverride",
            },
            {
                profileName: "Year-by-year CO2 intensity",
                unit: `${project?.physicalUnit === 0 ? "kg CO2/boe" : "kg CO2/boe"}`,
                profile: co2Intensity,
                total: co2IntensityTotal?.toString(),
                overridable: false,
                editable: false,
                resourceName: "co2Intensity",
                resourceId: drainageStrategyData.id,
                resourceProfileId: co2Intensity?.id,
                resourcePropertyKey: "co2Intensity",
            },
        ]
        setTimeSeriesData(newTimeSeriesData)
    }, [apiData])

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

    const datePickerValue = (() => {
        if (project?.physicalUnit === 0) {
            return "SI"
        } if (project?.physicalUnit === 1) {
            return "Oil field"
        }
        return ""
    })()

    const co2EmissionsTotalString = () => {
        if (co2EmissionsData) {
            return (Math.round(co2EmissionsData.sum! * 10) / 10).toString()
        }
        return "0"
    }

    const co2DistributionChartData = [
        { profile: "Drilling", value: co2DrillingFlaringFuelTotals?.co2Drilling },
        { profile: "Flaring", value: co2DrillingFlaringFuelTotals?.co2Flaring },
        { profile: "Fuel", value: co2DrillingFlaringFuelTotals?.co2Fuel },
    ]

    if (!topsideData) {
        return <p>loading....</p>
    }

    if (activeTabCase !== 6 || !caseData) { return null }

    return (
        <Grid container spacing={2}>
            <Grid item xs={12}>
                <Typography>Facility data, Cost and CO2 emissions can be imported using the PROSP import feature in Technical input</Typography>
            </Grid>
            <Grid item xs={12}>
                <SwitchableNumberInput
                    resourceName="topside"
                    resourcePropertyKey="fuelConsumption"
                    resourceId={topsideData.id}
                    label="Fuel consumption"
                    value={topsideData.fuelConsumption}
                    integer={false}
                    unit="million Sm³ gas/sd"
                />
            </Grid>
            <Grid item xs={12}>
                <CaseCO2DistributionTable topside={topsideData} />
            </Grid>
            <Grid item xs={12} lg={8}>
                <AgChartsTimeseries
                    data={co2EmissionsChartData()}
                    chartTitle="Annual CO2 emissions"
                    barColors={["#E24973", "#FF92A8"]}
                    barProfiles={["co2Emissions"]}
                    barNames={["Annual CO2 emissions (million tonnes)"]}
                    width="100%"
                    height="600px"
                    lineChart={co2IntensityLine}
                    axesData={chartAxes}
                />
            </Grid>
            <Grid item xs={12} lg={4} container direction="column" spacing={1} justifyContent="center" alignItems="center">
                <Grid item>
                    <Typography variant="h4">Average lifetime CO2 intensity</Typography>
                </Grid>
                <Grid item>
                    <Typography variant="h1_bold">{Math.round(co2IntensityTotal * 10) / 10}</Typography>
                </Grid>
                <Grid item>
                    <Typography color="disabled">kg CO2/boe</Typography>
                </Grid>
                <Grid item>
                    <AgChartsPie
                        data={co2DistributionChartData}
                        chartTitle="CO2 distribution"
                        barColors={["#243746", "#EB0037", "#A8CED1"]}
                        height={400}
                        width="400px"
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
