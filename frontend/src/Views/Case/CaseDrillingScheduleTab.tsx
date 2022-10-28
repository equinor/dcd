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
    Button, NativeSelect, Progress, Typography,
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
const InputWrapper = styled.div`
    margin-right: 20px;
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
    activeTab: number
}

function CaseDrillingScheduleTab({
    project, setProject,
    caseItem, setCase,
    exploration, setExploration,
    wellProject, setWellProject,
    explorationWells, setExplorationWells,
    wellProjectWells, setWellProjectWells,
    wells, activeTab,
}: Props) {
    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

    const wellProjectWellsGridRef = useRef(null)
    const explorationWellsGridRef = useRef(null)

    // WellProjectWell
    const [oilProducerCount, setOilProducerCount] = useState<number>(0)
    const [gasProducerCount, setGasProducerCount] = useState<number>(0)
    const [waterInjectorCount, setWaterInjectorCount] = useState<number>(0)
    const [gasInjectorCount, setGasInjectorCount] = useState<number>(0)

    // ExplorationWell
    const [explorationWellCount, setExplorationWellCount] = useState<number>(0)
    const [appraisalWellCount, setAppraisalWellCount] = useState<number>(0)
    const [sidetrackCount, setSidetrackCount] = useState<number>(0)

    const [isSaving, setIsSaving] = useState<boolean>()

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
        if (activeTab === 3) {
            const explorationDrillingSchedule = explorationWells?.map((ew) => ew.drillingSchedule) ?? []
            const wellProjectDrillingSchedule = wellProjectWells?.map((ew) => ew.drillingSchedule) ?? []
            SetTableYearsFromProfiles(
                [...explorationDrillingSchedule, ...wellProjectDrillingSchedule],
                caseItem.DG4Date.getFullYear(),
                setStartYear,
                setEndYear,
                setTableYears,
            )
        }
    }, [activeTab])

    const sumWellsForWellCategory = (category: Components.Schemas.WellCategory): number => {
        if (wells && wells.length > 0) {
            if (category >= 4) {
                const filteredExplorationWells = explorationWells.filter((ew) => ew.explorationId === exploration.id)
                const filteredWells = wells.filter((w) => w.wellCategory === category)
                let sum = 0
                filteredWells.forEach((fw) => {
                    filteredExplorationWells.filter((few) => few.wellId === fw.id).forEach((ew) => {
                        if (ew.drillingSchedule
                            && ew.drillingSchedule.values
                            && ew.drillingSchedule.values.length > 0) {
                            sum += ew.drillingSchedule.values.reduce((a, b) => a + b, 0)
                        }
                    })
                })
                return sum
            }
            const filteredWellProjectWells = wellProjectWells.filter((wpw) => wpw.wellProjectId === wellProject.id)
            const filteredWells = wells.filter((w) => w.wellCategory === category)
            let sum = 0
            filteredWells.forEach((fw) => {
                filteredWellProjectWells.filter((fwpw) => fwpw.wellId === fw.id).forEach((ew) => {
                    if (ew.drillingSchedule && ew.drillingSchedule.values && ew.drillingSchedule.values.length > 0) {
                        sum += ew.drillingSchedule.values.reduce((a, b) => a + b, 0)
                    }
                })
            })
            return sum
        }
        return 0
    }

    useEffect(() => {
        if (activeTab === 3) {
            setOilProducerCount(sumWellsForWellCategory(0))
            setGasProducerCount(sumWellsForWellCategory(1))
            setWaterInjectorCount(sumWellsForWellCategory(2))
            setGasInjectorCount(sumWellsForWellCategory(3))
            setExplorationWellCount(sumWellsForWellCategory(4))
            setAppraisalWellCount(sumWellsForWellCategory(5))
            setSidetrackCount(sumWellsForWellCategory(6))
        }
    }, [wells, explorationWells, wellProjectWells, activeTab])

    const handleSave = async () => {
        setIsSaving(true)
        // Exploration wells
        const newExplorationWells = explorationWells
            .filter((ew) => ew.drillingSchedule && ew.drillingSchedule.id === EMPTY_GUID)
        const newExplorationWellsResult = await (await GetExplorationWellService())
            .createMultipleExplorationWells(newExplorationWells)

        const updateExplorationWells = explorationWells
            .filter((ew) => ew.drillingSchedule && ew.drillingSchedule.id !== EMPTY_GUID)
        const updateExplorationWellsResult = await (await GetExplorationWellService())
            .updateMultipleExplorationWells(updateExplorationWells)

        if (updateExplorationWellsResult && updateExplorationWellsResult.length > 0) {
            setExplorationWells(updateExplorationWellsResult)
        } else if (newExplorationWellsResult && newExplorationWellsResult.length > 0) {
            setExplorationWells(newExplorationWellsResult)
        }

        // WellProject wells
        const newWellProjectWells = wellProjectWells
            .filter((ew) => ew.drillingSchedule && ew.drillingSchedule.id === EMPTY_GUID)
        const newWellProjectWellsResult = await (await GetWellProjectWellService())
            .createMultipleWellProjectWell(newWellProjectWells)

        const updateWellProjectWells = wellProjectWells
            .filter((ew) => ew.drillingSchedule && ew.drillingSchedule.id !== EMPTY_GUID)
        const updateWellProjectWellsResult = await (await GetWellProjectWellService())
            .updateMultipleWellProjectWells(updateWellProjectWells)

        setIsSaving(false)

        if (updateWellProjectWellsResult && updateWellProjectWellsResult.length > 0) {
            setWellProjectWells(updateWellProjectWellsResult)
        } else if (newWellProjectWellsResult && newWellProjectWellsResult.length > 0) {
            setWellProjectWells(newWellProjectWellsResult)
        }
    }

    if (activeTab !== 3) { return null }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Drilling schedule</PageTitle>
                {!isSaving ? <Button onClick={handleSave}>Save</Button> : (
                    <Button>
                        <Progress.Dots />
                    </Button>
                )}
            </TopWrapper>
            <p>Create wells in technical input in order to see them in the list below.</p>
            <ColumnWrapper>
                <RowWrapper>
                    <InputWrapper>
                        <CaseNumberInput
                            onChange={() => { }}
                            value={explorationWellCount}
                            integer
                            disabled
                            label="Exploration wells"
                        />
                    </InputWrapper>
                    <InputWrapper>
                        <CaseNumberInput
                            onChange={() => { }}
                            value={appraisalWellCount}
                            integer
                            disabled
                            label="Appraisal wells"
                        />
                    </InputWrapper>
                    <InputWrapper>
                        <CaseNumberInput
                            onChange={() => { }}
                            value={oilProducerCount}
                            integer
                            disabled
                            label="Oil producer wells"
                        />
                    </InputWrapper>
                    <InputWrapper>
                        <CaseNumberInput
                            onChange={() => { }}
                            value={gasProducerCount}
                            integer
                            disabled
                            label="Gas producer wells"
                        />
                    </InputWrapper>
                    <InputWrapper>
                        <CaseNumberInput
                            onChange={() => { }}
                            value={waterInjectorCount}
                            integer
                            disabled
                            label="Water injector wells"
                        />
                    </InputWrapper>
                    <InputWrapper>
                        <CaseNumberInput
                            onChange={() => { }}
                            value={gasInjectorCount}
                            integer
                            disabled
                            label="Gas injector wells"
                        />
                    </InputWrapper>
                </RowWrapper>
            </ColumnWrapper>
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
