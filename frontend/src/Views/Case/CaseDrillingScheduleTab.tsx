/* eslint-disable max-len */
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
import { Case } from "../../models/case/Case"
import CaseNumberInput from "../../Components/Case/CaseNumberInput"
import { Exploration } from "../../models/assets/exploration/Exploration"
import { WellProject } from "../../models/assets/wellproject/WellProject"
import { Well } from "../../models/Well"
import { ExplorationWell } from "../../models/ExplorationWell"
import { WellProjectWell } from "../../models/WellProjectWell"
import { GetWellProjectWellService } from "../../Services/WellProjectWellService"
import { GetExplorationWellService } from "../../Services/ExplorationWellService"
import { EMPTY_GUID } from "../../Utils/constants"
import CaseDrillingScheduleTabTable from "./CaseDrillingScheduleTabTable"
import { SetTableYearsFromProfiles } from "./CaseTabTableHelper"

const ColumnWrapper = styled.div`
    display: flex;
    flex-direction: column;
`
const RowWrapper = styled.div`
    display: flex;
    flex-direction: row;
    margin-bottom: 78px;
`
const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    margin-top: 20px;
    margin-bottom: 20px;
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
const TableWrapper = styled.div`
    margin-bottom: 50px;
`

interface Props {
    project: Project,
    setProject: Dispatch<SetStateAction<Project | undefined>>,
    caseItem: Case,
    setCase: Dispatch<SetStateAction<Case | undefined>>,
    exploration: Exploration,
    setExploration: Dispatch<SetStateAction<Exploration | undefined>>,
    wellProject: WellProject,
    setWellProject: Dispatch<SetStateAction<WellProject | undefined>>,
    explorationWells: ExplorationWell[],
    setExplorationWells: Dispatch<SetStateAction<ExplorationWell[] | undefined>>,
    wellProjectWells: WellProjectWell[],
    setWellProjectWells: Dispatch<SetStateAction<WellProjectWell[] | undefined>>,
    wells: Well[] | undefined
}

function CaseDrillingScheduleTab({
    project, setProject,
    caseItem, setCase,
    exploration, setExploration,
    wellProject, setWellProject,
    explorationWells, setExplorationWells,
    wellProjectWells, setWellProjectWells,
    wells,
}: Props) {
    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

    const wellProjectWellsGridRef = useRef(null)
    const explorationWellsGridRef = useRef(null)

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
    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    useEffect(() => {
        const explorationDrillingSchedule = explorationWells?.map((ew) => ew.drillingSchedule) ?? []
        const wellProjectDrillingSchedule = wellProjectWells?.map((ew) => ew.drillingSchedule) ?? []
        SetTableYearsFromProfiles(
            [...explorationDrillingSchedule, ...wellProjectDrillingSchedule],
            caseItem.DG4Date.getFullYear(),
            setStartYear,
            setEndYear,
            setTableYears,
        )
    }, [])

    const handleSave = async () => {
        // Exploration wells
        const newExplorationWells = explorationWells.filter((ew) => ew.drillingSchedule && ew.drillingSchedule.id === EMPTY_GUID)
        const newExplorationWellsResult = await (await GetExplorationWellService()).createMultipleExplorationWells(newExplorationWells)

        const updateExplorationWells = explorationWells.filter((ew) => ew.drillingSchedule && ew.drillingSchedule.id !== EMPTY_GUID)
        const updateExplorationWellsResult = await (await GetExplorationWellService()).updateMultipleExplorationWells(updateExplorationWells)

        if (updateExplorationWellsResult && updateExplorationWellsResult.length > 0) {
            setExplorationWells(updateExplorationWellsResult)
        } else if (newExplorationWellsResult && newExplorationWellsResult.length > 0) {
            setExplorationWells(newExplorationWellsResult)
        }

        // WellProject wells
        const newWellProjectWells = wellProjectWells.filter((ew) => ew.drillingSchedule && ew.drillingSchedule.id === EMPTY_GUID)
        const newWellProjectWellsResult = await (await GetWellProjectWellService()).createMultipleWellProjectWell(newWellProjectWells)

        const updateWellProjectWells = wellProjectWells.filter((ew) => ew.drillingSchedule && ew.drillingSchedule.id !== EMPTY_GUID)
        const updateWellProjectWellsResult = await (await GetWellProjectWellService()).updateMultipleWellProjectWells(updateWellProjectWells)

        if (updateWellProjectWellsResult && updateWellProjectWellsResult.length > 0) {
            setWellProjectWells(updateWellProjectWellsResult)
        } else if (newWellProjectWellsResult && newWellProjectWellsResult.length > 0) {
            setWellProjectWells(newWellProjectWellsResult)
        }
    }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Drilling schedule</PageTitle>
                <Button onClick={handleSave}>Save</Button>
            </TopWrapper>
            <p>Create wells in technical input in order to see them in the list below.</p>
            <ColumnWrapper>
                <TableYearWrapper>
                    <NativeSelectField
                        id="currency"
                        label="Currency"
                        onChange={() => { }}
                        value={project.currency}
                        disabled
                    >
                        <option key="1" value={1}>MNOK</option>
                        <option key="2" value={2}>MUSD</option>
                    </NativeSelectField>
                    <YearInputWrapper>
                        <CaseNumberInput
                            onChange={handleStartYearChange}
                            value={startYear}
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
                            value={endYear}
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
            <TableWrapper>
                <CaseDrillingScheduleTabTable
                    assetWells={explorationWells}
                    caseItem={caseItem}
                    dg4Year={caseItem.DG4Date.getFullYear()}
                    project={project}
                    setAssetWells={setExplorationWells}
                    setCase={setCase}
                    setProject={setProject}
                    tableName="Exploration wells"
                    tableYears={tableYears}
                    assetId={exploration.id!}
                    wells={wells}
                    isExplorationTable
                    gridRef={explorationWellsGridRef}
                    alignedGridsRef={[wellProjectWellsGridRef]}
                />
            </TableWrapper>
            <TableWrapper>
                <CaseDrillingScheduleTabTable
                    assetWells={wellProjectWells}
                    caseItem={caseItem}
                    dg4Year={caseItem.DG4Date.getFullYear()}
                    project={project}
                    setAssetWells={setWellProjectWells}
                    setCase={setCase}
                    setProject={setProject}
                    tableName="Development wells"
                    tableYears={tableYears}
                    assetId={wellProject.id!}
                    wells={wells}
                    isExplorationTable={false}
                    gridRef={wellProjectWellsGridRef}
                    alignedGridsRef={[explorationWellsGridRef]}
                />
            </TableWrapper>
        </>
    )
}

export default CaseDrillingScheduleTab
