import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
    useState,
    useEffect,
    useRef,
} from "react"
import styled from "styled-components"

import {
    Button, NativeSelect, Typography,
} from "@equinor/eds-core-react"
import { Project } from "../../models/Project"
import CaseNumberInput from "../../Components/Case/CaseNumberInput"
import CaseTabTable from "./CaseTabTable"
import { ITimeSeries } from "../../models/ITimeSeries"
import { SetTableYearsFromProfiles } from "./CaseTabTableHelper"
import { GetGenerateProfileService } from "../../Services/CaseGeneratedProfileService"
import CaseCO2DistributionTable from "./CaseCO2DistributionTable"
import { AgChartsTimeseries, setValueToCorrespondingYear } from "../../Components/AgGrid/AgChartsTimeseries"
import { AgChartsPie } from "../../Components/AgGrid/AgChartsPie"
import { WrapperColumn } from "../Asset/StyledAssetComponents"

const ColumnWrapper = styled.div`
    display: flex;
    flex-direction: column;
`
const GraphWrapper = styled.div`
    display: flex;
    flex-direction: column;
    text-align: center;
    margin-bottom: 40px;
`
const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    margin-top: 20px;
    margin-bottom: 20px;
`
const RowWrapper = styled.div`
    display: flex;
    flex-direction: row;
    margin-bottom: 50px;
    margin-top: 20px;
`
const PageTitle = styled(Typography)`
    flex-grow: 1;
`
const NativeSelectField = styled(NativeSelect)`
    width: 200px;
    padding-right: 20px;
`
const TableYearWrapper = styled.div`
    align-items: flex-end;
    display: flex;
    flex-direction: row;
    align-content: right;
    margin-left: auto;
    margin-bottom: 20px;
`
const YearInputWrapper = styled.div`
    width: 80px;
    padding-right: 10px;
`
const YearDashWrapper = styled.div`
    padding-right: 5px;
`
const NumberInputField = styled.div`
    padding-right: 20px;
    padding-left: 50px;
`

interface Props {
    project: Project,
    caseItem: Components.Schemas.CaseDto,
    topside: Components.Schemas.TopsideDto,
    setTopside: Dispatch<SetStateAction<Components.Schemas.TopsideDto | undefined>>,
    drainageStrategy: Components.Schemas.DrainageStrategyDto,
    setDrainageStrategy: Dispatch<SetStateAction<Components.Schemas.DrainageStrategyDto | undefined>>,
    activeTab: number

    co2Emissions: Components.Schemas.Co2EmissionsDto | undefined,
    setCo2Emissions: Dispatch<SetStateAction<Components.Schemas.Co2EmissionsDto | undefined>>,
}

function CaseCO2Tab({
    project,
    caseItem,
    topside, setTopside,
    activeTab, drainageStrategy, setDrainageStrategy,
    co2Emissions, setCo2Emissions,
}: Props) {
    const [co2Intensity, setCo2Intensity] = useState<Components.Schemas.Co2IntensityDto>()
    const [co2IntensityTotal, setCo2IntensityTotal] = useState<number>(0)
    const [co2DrillingFlaringFuelTotals, setCo2DrillingFlaringFuelTotals] = useState<Components.Schemas.Co2DrillingFlaringFuelTotalsDto>()

    const [co2EmissionsOverride, setCo2EmissionsOverride] = useState<Components.Schemas.Co2EmissionsOverrideDto>()

    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

    const co2GridRef = useRef<any>(null)

    useEffect(() => {
        (async () => {
            try {
                if (activeTab === 6 && caseItem.id) {
                    const co2I = (await GetGenerateProfileService()).generateCo2IntensityProfile(project.id, caseItem.id)
                    const co2ITotal = await (await GetGenerateProfileService()).generateCo2IntensityTotal(project.id, caseItem.id)
                    const co2DFFTotal = await (await GetGenerateProfileService()).generateCo2DrillingFlaringFuelTotals(project.id, caseItem.id)

                    setCo2Emissions(drainageStrategy.co2Emissions)
                    setCo2Intensity(await co2I)
                    setCo2IntensityTotal(Number(co2ITotal))
                    setCo2DrillingFlaringFuelTotals(co2DFFTotal)

                    SetTableYearsFromProfiles(
                        [drainageStrategy.co2Emissions, await co2I, drainageStrategy.co2EmissionsOverride?.override ? drainageStrategy.co2EmissionsOverride : undefined],
                        caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030,
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
    }, [activeTab])

    useEffect(() => {
        if (co2GridRef.current && co2GridRef.current.api && co2GridRef.current.api.refreshCells) {
            co2GridRef.current.api.refreshCells()
        }
    }, [co2Emissions])

    const handleStartYearChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newStartYear = Number(e.currentTarget.value)
        if (newStartYear < 2010) {
            setStartYear(2010)
            return
        }
        setStartYear(newStartYear)
    }

    const handleEndYearChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newEndYear = Number(e.currentTarget.value)
        if (newEndYear > 2100) {
            setEndYear(2100)
            return
        }
        setEndYear(newEndYear)
    }

    const handleTopsideFuelConsumptionChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside = { ...topside }
        newTopside.fuelConsumption = e.currentTarget.value.length > 0
            ? Number(e.currentTarget.value) : undefined
        setTopside(newTopside)
    }

    interface ITimeSeriesData {
        profileName: string
        unit: string,
        set?: Dispatch<SetStateAction<ITimeSeries | undefined>>,
        overrideProfileSet?: Dispatch<SetStateAction<ITimeSeries | undefined>>,
        profile: ITimeSeries | undefined
        total?: string
        overrideProfile?: ITimeSeries | undefined
        overridable?: boolean
    }

    const timeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Annual CO2 emissions",
            unit: `${project?.physUnit === 0 ? "MTPA" : "MTPA"}`,
            profile: co2Emissions,
            overridable: true,
            overrideProfile: co2EmissionsOverride,
            overrideProfileSet: setCo2EmissionsOverride,
        },
        {
            profileName: "Year-by-year CO2 intensity",
            unit: `${project?.physUnit === 0 ? "kg CO2/boe" : "kg CO2/boe"}`,
            profile: co2Intensity,
            total: co2IntensityTotal?.toString(),
        },
    ]

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    const co2EmissionsChartData = () => {
        const dataArray = []
        const useOverride = co2EmissionsOverride !== undefined && co2EmissionsOverride.override
        for (let i = startYear; i <= endYear; i += 1) {
            dataArray.push({
                year: i,
                co2Emissions:
                    setValueToCorrespondingYear(useOverride ? co2EmissionsOverride : co2Emissions, i, startYear, caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030),
                co2Intensity:
                    setValueToCorrespondingYear(co2Intensity, i, startYear, caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030),
            })
        }
        return dataArray
    }

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
        const newDrainageStrategy: Components.Schemas.DrainageStrategyDto = { ...drainageStrategy }
        if (newDrainageStrategy.co2EmissionsOverride && !co2EmissionsOverride) {
            return
        }

        newDrainageStrategy.co2EmissionsOverride = co2EmissionsOverride
        setDrainageStrategy(newDrainageStrategy)
    }, [co2EmissionsOverride])

    if (activeTab !== 6) { return null }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">CO2 Emissions</PageTitle>
            </TopWrapper>
            <p>Facility data, Cost and CO2 emissions can be imported using the PROSP import feature in Technical input</p>
            <ColumnWrapper>
                <RowWrapper>
                    <CaseCO2DistributionTable topside={topside} />
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleTopsideFuelConsumptionChange}
                            defaultValue={topside?.fuelConsumption}
                            integer={false}
                            label="Fuel consumption"
                            unit="million Sm³ gas/sd"
                        />
                    </NumberInputField>
                </RowWrapper>
            </ColumnWrapper>
            <RowWrapper>
                <AgChartsTimeseries
                    data={co2EmissionsChartData()}
                    chartTitle="Annual CO2 emissions"
                    barColors={["#E24973", "#FF92A8"]}
                    barProfiles={["co2Emissions"]}
                    barNames={["Annual CO2 emissions (million tonnes)"]}
                    width="70%"
                    height="600"
                    lineChart={co2IntensityLine}
                    axesData={chartAxes}
                />
                <WrapperColumn>
                    <GraphWrapper>
                        <Typography
                            style={{
                                display: "flex", flexDirection: "column", textAlign: "center", fontWeight: "500", fontSize: "18px", marginBottom: "30px",
                            }}
                        >
                            Average lifetime CO2 intensity

                        </Typography>
                        <Typography
                            style={{
                                display: "flex", flexDirection: "column", textAlign: "center", fontWeight: "500", fontSize: "31px",
                            }}
                        >
                            {Math.round(co2IntensityTotal * 10) / 10}

                        </Typography>
                        <Typography
                            style={{
                                display: "flex", flexDirection: "column", textAlign: "center", fontWeight: "400", fontSize: "18px", color: "#B4B4B4",
                            }}
                        >
                            kg CO2/boe

                        </Typography>
                    </GraphWrapper>
                    <AgChartsPie
                        data={co2DistributionChartData}
                        chartTitle="CO2 distribution"
                        barColors={["#243746", "#EB0037", "#A8CED1"]}
                        height={400}
                        width="100%"
                        totalCo2Emission={co2EmissionsTotalString()}
                        unit="million tonnes"
                    />
                </WrapperColumn>
            </RowWrapper>
            <ColumnWrapper>
                <TableYearWrapper>
                    <NativeSelectField
                        id="unit"
                        label="Units"
                        onChange={() => { }}
                        value={project.physUnit}
                        disabled
                    >
                        <option key={0} value={0}>SI</option>
                        <option key={1} value={1}>Oil field</option>
                    </NativeSelectField>
                    <YearInputWrapper>
                        <CaseNumberInput
                            onChange={handleStartYearChange}
                            defaultValue={startYear}
                            integer
                            label="Start year"
                        />
                    </YearInputWrapper>
                    <YearDashWrapper>
                        <Typography variant="h2">-</Typography>
                    </YearDashWrapper>
                    <YearInputWrapper>
                        <CaseNumberInput
                            onChange={handleEndYearChange}
                            defaultValue={endYear}
                            integer
                            label="End year"
                        />
                    </YearInputWrapper>
                    <Button
                        onClick={handleTableYearsClick}
                    >
                        Apply
                    </Button>
                </TableYearWrapper>
            </ColumnWrapper>
            <CaseTabTable
                timeSeriesData={timeSeriesData}
                dg4Year={caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030}
                tableYears={tableYears}
                tableName="CO2 emissions"
                includeFooter={false}
                gridRef={co2GridRef}
            />
        </>
    )
}

export default CaseCO2Tab
