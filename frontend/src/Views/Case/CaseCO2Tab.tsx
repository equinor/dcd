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
    Button, NativeSelect, Typography, Progress,
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
import { GetTopsideService } from "../../Services/TopsideService"
import CaseCO2DistributionTable from "./CaseCO2DistributionTable"

const ColumnWrapper = styled.div`
    display: flex;
    flex-direction: column;
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
    activeTab: number
}

function CaseCO2Tab({
    project, setProject,
    caseItem, setCase,
    topside, setTopside,
    activeTab,
}: Props) {
    const [co2Emissions, setCo2Emissions] = useState<Co2Emissions>()

    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

    useEffect(() => {
        (async () => {
            try {
                if (activeTab === 6) {
                    const co2 = (await GetGenerateProfileService()).generateCo2EmissionsProfile(caseItem.id)
                    setCo2Emissions(await co2)

                    SetTableYearsFromProfiles(
                        [await co2],
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
        newTopside.fuelConsumption = Number(e.currentTarget.value)
        setTopside(newTopside)
    }

    const handleTopsideFlaredGasChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newTopside: Topside = { ...topside }
        newTopside.flaredGas = Number(e.currentTarget.value)
        setTopside(newTopside)
    }

    interface ITimeSeriesData {
        profileName: string
        unit: string,
        set?: Dispatch<SetStateAction<ITimeSeries | undefined>>,
        profile: ITimeSeries | undefined
    }

    const timeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Annual CO2 emissions",
            unit: `${project?.physUnit === 0 ? "MTPA" : "MTPA"}`,
            profile: co2Emissions,
        },
    ]

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

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
            />
        </>
    )
}

export default CaseCO2Tab
