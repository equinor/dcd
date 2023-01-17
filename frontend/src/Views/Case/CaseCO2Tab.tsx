/* eslint-disable max-len */
import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
    useState,
    useEffect,
} from "react"
import styled from "styled-components"

import {
    Button, NativeSelect, Typography,
} from "@equinor/eds-core-react"
import { Project } from "../../models/Project"
import { Case } from "../../models/case/Case"
import CaseNumberInput from "../../Components/Case/CaseNumberInput"
import CaseTabTable from "./CaseTabTable"
import { ITimeSeries } from "../../models/ITimeSeries"
import { SetTableYearsFromProfiles } from "./CaseTabTableHelper"
import { Co2Emissions } from "../../models/assets/drainagestrategy/Co2Emissions"
import { GetGenerateProfileService } from "../../Services/GenerateProfileService"
import { Topside } from "../../models/assets/topside/Topside"
import CaseCO2DistributionTable from "./CaseCO2DistributionTable"
import { AgChartsTimeseries, setValueToCorrespondingYear } from "../../Components/AgGrid/AgChartsTimeseries"
import { AgChartsPie } from "../../Components/AgGrid/AgChartsPie"
import { WrapperColumn } from "../Asset/StyledAssetComponents"
import { Co2Intensity } from "../../models/assets/drainagestrategy/Co2Intensity"
import { Co2DrillingFlaringFuelTotals } from "../../models/assets/drainagestrategy/Co2DrillingFlaringFuelTotals"
import { Co2EmissionsOverride } from "../../models/assets/drainagestrategy/Co2EmissionsOverride"
import { DrainageStrategy } from "../../models/assets/drainagestrategy/DrainageStrategy"

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
    setProject: Dispatch<SetStateAction<Project | undefined>>,
    caseItem: Case,
    setCase: Dispatch<SetStateAction<Case | undefined>>,
    topside: Topside,
    setTopside: Dispatch<SetStateAction<Topside | undefined>>,
    drainageStrategy: DrainageStrategy,
    setDrainageStrategy: Dispatch<SetStateAction<DrainageStrategy | undefined>>,
    activeTab: number
}

function CaseCO2Tab({
    project, setProject,
    caseItem, setCase,
    topside, setTopside,
    activeTab, drainageStrategy, setDrainageStrategy,
}: Props) {
    const [co2Emissions, setCo2Emissions] = useState<Co2Emissions>()
    const [co2Intensity, setCo2Intensity] = useState<Co2Intensity>()
    const [co2IntensityTotal, setCo2IntensityTotal] = useState<number>(0)
    const [co2DrillingFlaringFuelTotals, setCo2DrillingFlaringFuelTotals] = useState<Co2DrillingFlaringFuelTotals>()

    const [co2EmissionsOverride, setCo2EmissionsOverride] = useState<Co2EmissionsOverride>()

    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

    useEffect(() => {
        (async () => {
            try {
                if (activeTab === 6) {
                    const co2E = (await GetGenerateProfileService()).generateCo2EmissionsProfile(caseItem.id)
                    const co2I = (await GetGenerateProfileService()).generateCo2IntensityProfile(caseItem.id)
                    const co2ITotal = await (await GetGenerateProfileService()).generateCo2IntensityTotal(caseItem.id)
                    const co2DFFTotal = await (await GetGenerateProfileService()).generateCo2DrillingFlaringFuelTotals(caseItem.id)

                    setCo2Emissions(await co2E)
                    setCo2Intensity(await co2I)
                    setCo2IntensityTotal(Number(co2ITotal))
                    setCo2DrillingFlaringFuelTotals(co2DFFTotal)

                    SetTableYearsFromProfiles(
                        [await co2E, await co2I],
                        caseItem.DG4Date.getFullYear(),
                        setStartYear,
                        setEndYear,
                        setTableYears,
                    )
                }
            } catch (error) {
                console.error("[CaseView] Error while generating cost profile", error)
            }
        })()
    }, [activeTab])

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
        const newTopside: Topside = { ...topside }
        newTopside.fuelConsumption = e.currentTarget.value.length > 0
            ? Number(e.currentTarget.value) : undefined
        setTopside(newTopside)
    }

    const handleTopsideFlaredGasChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside: Topside = { ...topside }
        newTopside.flaredGas = e.currentTarget.value.length > 0
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
        for (let i = startYear; i <= endYear; i += 1) {
            dataArray.push({
                year: i,
                co2Emissions:
                    setValueToCorrespondingYear(co2Emissions, i, startYear, caseItem.DG4Date.getFullYear()),
                co2Intensity:
                    setValueToCorrespondingYear(co2Intensity, i, startYear, caseItem.DG4Date.getFullYear()),
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
        const newDrainageStrategy: DrainageStrategy = { ...drainageStrategy }
        if (newDrainageStrategy.co2EmissionsOverride && !co2EmissionsOverride) { return }
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
                caseItem={caseItem}
                project={project}
                setCase={setCase}
                setProject={setProject}
                timeSeriesData={timeSeriesData}
                dg4Year={caseItem.DG4Date.getFullYear()}
                tableYears={tableYears}
                tableName="CO2 emissions"
                includeFooter={false}
            />
        </>
    )
}

export default CaseCO2Tab
