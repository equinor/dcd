import {
    Dispatch,
    SetStateAction,
    useState,
    useEffect,
    useRef,
} from "react"
import { Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import SwitchableNumberInput from "../../../Input/SwitchableNumberInput"
import CaseTabTable from "../../Components/CaseTabTable"
import { ITimeSeries } from "../../../../Models/ITimeSeries"
import { SetTableYearsFromProfiles } from "../../Components/CaseTabTableHelper"
import { GetGenerateProfileService } from "../../../../Services/CaseGeneratedProfileService"
import CaseCO2DistributionTable from "./Co2EmissionsAgGridTable"
import { AgChartsTimeseries, setValueToCorrespondingYear } from "../../../AgGrid/AgChartsTimeseries"
import { AgChartsPie } from "../../../AgGrid/AgChartsPie"
import { ITimeSeriesOverride } from "../../../../Models/ITimeSeriesOverride"
import { useProjectContext } from "../../../../Context/ProjectContext"
import { useCaseContext } from "../../../../Context/CaseContext"
import { setNonNegativeNumberState } from "../../../../Utils/common"
import DateRangePicker from "../../../Input/TableDateRangePicker"
import { GetTopsideService } from "../../../../Services/TopsideService"
import useQuery from "../../../../Hooks/useQuery"

interface Props {
    topside: Components.Schemas.TopsideWithProfilesDto,
    setTopside: Dispatch<SetStateAction<Components.Schemas.TopsideWithProfilesDto | undefined>>,
    drainageStrategy: Components.Schemas.DrainageStrategyWithProfilesDto,
    setDrainageStrategy: Dispatch<SetStateAction<Components.Schemas.DrainageStrategyWithProfilesDto | undefined>>,
    co2Emissions: Components.Schemas.Co2EmissionsDto | undefined,
    setCo2Emissions: Dispatch<SetStateAction<Components.Schemas.Co2EmissionsDto | undefined>>,
}

const CaseCO2Tab = ({
    topside, setTopside,
    drainageStrategy, setDrainageStrategy,
    co2Emissions, setCo2Emissions,
}: Props) => {
    const { project } = useProjectContext()
    const {
        projectCase, activeTabCase,
    } = useCaseContext()
    if (!projectCase) { return null }

    const [co2Intensity, setCo2Intensity] = useState<Components.Schemas.Co2IntensityDto>()
    const [co2IntensityTotal, setCo2IntensityTotal] = useState<number>(0)
    const [co2DrillingFlaringFuelTotals, setCo2DrillingFlaringFuelTotals] = useState<Components.Schemas.Co2DrillingFlaringFuelTotalsDto>()
    const [co2EmissionsOverride, setCo2EmissionsOverride] = useState<Components.Schemas.Co2EmissionsOverrideDto>()
    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

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

    const datePickerValue = (() => {
        if (project?.physicalUnit === 0) {
            return "SI"
        } if (project?.physicalUnit === 1) {
            return "Oil field"
        }
        return ""
    })()

    useEffect(() => {
        (async () => {
            try {
                if (project && activeTabCase === 6 && projectCase.id) {
                    const co2I = (await GetGenerateProfileService()).generateCo2IntensityProfile(project.id, projectCase.id)
                    const co2ITotal = await (await GetGenerateProfileService()).generateCo2IntensityTotal(project.id, projectCase.id)
                    const co2DFFTotal = await (await GetGenerateProfileService()).generateCo2DrillingFlaringFuelTotals(project.id, projectCase.id)

                    setCo2Emissions(drainageStrategy.co2Emissions)
                    setCo2Intensity(await co2I)
                    setCo2IntensityTotal(Number(co2ITotal))
                    setCo2DrillingFlaringFuelTotals(co2DFFTotal)

                    SetTableYearsFromProfiles(
                        [drainageStrategy.co2Emissions, await co2I, drainageStrategy.co2EmissionsOverride?.override ? drainageStrategy.co2EmissionsOverride : undefined],
                        projectCase.dG4Date ? new Date(projectCase.dG4Date).getFullYear() : 2030,
                        setStartYear,
                        setEndYear,
                        setTableYears,
                    )
                    setCo2EmissionsOverride(drainageStrategy.co2EmissionsOverride)
                }
            } catch (error) {
                console.error("[CaseView] Error while generating cost profile", error)
            }
        })()
    }, [activeTabCase])

    interface ITimeSeriesData {
        profileName: string
        unit: string,
        set?: Dispatch<SetStateAction<ITimeSeries | undefined>>,
        overrideProfileSet?: Dispatch<SetStateAction<ITimeSeriesOverride | undefined>>,
        profile: ITimeSeries | undefined
        total?: string
        overrideProfile?: ITimeSeries | undefined
        overridable?: boolean
    }

    const timeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Annual CO2 emissions",
            unit: `${project?.physicalUnit === 0 ? "MTPA" : "MTPA"}`,
            profile: co2Emissions,
            overridable: true,
            overrideProfile: co2EmissionsOverride,
            overrideProfileSet: setCo2EmissionsOverride,
        },
        {
            profileName: "Year-by-year CO2 intensity",
            unit: `${project?.physicalUnit === 0 ? "kg CO2/boe" : "kg CO2/boe"}`,
            profile: co2Intensity,
            total: co2IntensityTotal?.toString(),
        },
    ]

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    const co2EmissionsChartData = () => {
        const dataArray = []
        const useOverride = co2EmissionsOverride && co2EmissionsOverride.override
        for (let i = startYear; i <= endYear; i += 1) {
            dataArray.push({
                year: i,
                co2Emissions:
                    setValueToCorrespondingYear(
                        useOverride ? co2EmissionsOverride : co2Emissions,
                        i,
                        startYear,
                        projectCase.dG4Date ? new Date(projectCase.dG4Date).getFullYear() : 2030,
                    ),
                co2Intensity:
                    setValueToCorrespondingYear(
                        co2Intensity,
                        i,
                        startYear,
                        projectCase.dG4Date ? new Date(projectCase.dG4Date).getFullYear() : 2030,
                    ),
            })
        }
        return dataArray
    }

    const co2EmissionsTotalString = () => {
        if (co2Emissions) {
            return (Math.round(co2Emissions.sum! * 10) / 10).toString()
        }
        return "0"
    }

    const co2DistributionChartData = [
        { profile: "Drilling", value: co2DrillingFlaringFuelTotals?.co2Drilling },
        { profile: "Flaring", value: co2DrillingFlaringFuelTotals?.co2Flaring },
        { profile: "Fuel", value: co2DrillingFlaringFuelTotals?.co2Fuel },
    ]

    useEffect(() => {
        const newDrainageStrategy: Components.Schemas.DrainageStrategyWithProfilesDto = { ...drainageStrategy }
        if (!co2EmissionsOverride) {
            return
        }

        newDrainageStrategy.co2EmissionsOverride = co2EmissionsOverride
        setDrainageStrategy(newDrainageStrategy)
    }, [co2EmissionsOverride])

    useEffect(() => {
        if (co2GridRef.current && co2GridRef.current.api && co2GridRef.current.api.refreshCells) {
            co2GridRef.current.api.refreshCells()
        }
    }, [co2Emissions])

    if (activeTabCase !== 6) { return null }

    const { updateData: updateTopside } = useQuery({
        queryKey: ["topsideData", project!.id, projectCase.id],
        mutationFn: async (updatedData: Components.Schemas.APIUpdateTopsideDto) => {
            const topsideService = await GetTopsideService()
            return topsideService.updateTopside(project!.id, projectCase.id, topside.id, updatedData)
        },
    })

    return (
        <Grid container spacing={2}>
            <Grid item xs={12}>
                <Typography>Facility data, Cost and CO2 emissions can be imported using the PROSP import feature in Technical input</Typography>
            </Grid>
            <Grid item xs={12}>
                <SwitchableNumberInput
                    objectKey={topside?.fuelConsumption}
                    label="Fuel consumption"
                    onSubmit={(value: number) => {
                        setNonNegativeNumberState(value, "fuelConsumption", topside, setTopside)
                        updateTopside("fuelConsumption", value)
                    }}
                    value={topside?.fuelConsumption}
                    integer={false}
                    unit="million Sm³ gas/sd"
                />
            </Grid>
            <Grid item xs={12}>
                <CaseCO2DistributionTable topside={topside} />
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
                labelText="Units"
                labelValue={datePickerValue}
                handleTableYearsClick={handleTableYearsClick}
            />
            <Grid item xs={12}>
                <CaseTabTable
                    timeSeriesData={timeSeriesData}
                    dg4Year={projectCase.dG4Date ? new Date(projectCase.dG4Date).getFullYear() : 2030}
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
