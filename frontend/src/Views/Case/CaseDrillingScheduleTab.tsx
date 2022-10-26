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
import CaseTabTable from "./CaseTabTable"
import { ITimeSeries } from "../../models/ITimeSeries"
import { Exploration } from "../../models/assets/exploration/Exploration"
import { WellProject } from "../../models/assets/wellproject/WellProject"
import { Well } from "../../models/Well"
import { ExplorationWell } from "../../models/ExplorationWell"
import { WellProjectWell } from "../../models/WellProjectWell"
import { GetWellProjectWellService } from "../../Services/WellProjectWellService"
import { GetExplorationWellService } from "../../Services/ExplorationWellService"
import { EMPTY_GUID } from "../../Utils/constants"
import { IsExplorationWell } from "../../Utils/common"

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
    setExplorationWells: Dispatch<SetStateAction<ExplorationWell[]>>,
    wellProjectWells: WellProjectWell[],
    setWellProjectWells: Dispatch<SetStateAction<WellProjectWell[]>>,
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
    // Development
    // eslint-disable-next-line max-len
    // const [wellProjectWells, setWellProjectWells] = useState<WellProjectWell[]>(wellProject.wellProjectWells ?? [])
    // // Exploration
    // // eslint-disable-next-line max-len
    // const [explorationWells, setExplorationWells] = useState<ExplorationWell[]>(exploration.explorationWells ?? [])

    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

    const developmentWellsGridRef = useRef(null)
    const [developmentWellsGridData, setDevelopmentWellsGridData] = useState<any[]>([])

    const explorationWellsGridRef = useRef(null)
    const [explorationWellsGridData, setExplorationWellsGridData] = useState<any[]>([])
    const [tableExplorationWells, setTableExplorationWells] = useState<ExplorationWell[]>(explorationWells)

    const getTimeSeriesLastYear = (timeSeries: ITimeSeries | undefined): number | undefined => {
        if (timeSeries && timeSeries.startYear !== undefined && timeSeries.values && timeSeries.values.length > 0) {
            return timeSeries.startYear + timeSeries.values.length - 1
        } return undefined
    }

    const setTableYearsFromProfiles = (profiles: (ITimeSeries | undefined)[]) => {
        let firstYear = Number.MAX_SAFE_INTEGER
        let lastYear = Number.MIN_SAFE_INTEGER
        profiles.forEach((p) => {
            if (p && p.startYear !== undefined && p.startYear < firstYear) {
                firstYear = p.startYear
            }
            const profileLastYear = getTimeSeriesLastYear(p)
            if (profileLastYear !== undefined && profileLastYear > lastYear) {
                lastYear = profileLastYear
            }
        })
        if (firstYear < Number.MAX_SAFE_INTEGER && lastYear > Number.MIN_SAFE_INTEGER) {
            setStartYear(firstYear + caseItem.DG4Date.getFullYear())
            setEndYear(lastYear + caseItem.DG4Date.getFullYear())
            setTableYears([firstYear + caseItem.DG4Date.getFullYear(), lastYear + caseItem.DG4Date.getFullYear()])
        }
    }

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
        setTableYearsFromProfiles([...explorationDrillingSchedule, ...wellProjectDrillingSchedule])
    }, [])

    interface ITimeSeriesData {
        profileName: string
        unit: string,
        set?: Dispatch<SetStateAction<ITimeSeries | undefined>>,
        profile: ITimeSeries | undefined
    }

    // const buildDrillingScheduleTableData = (assetWells: ExplorationWell[] | WellProjectWell[]) => {
    //     console.log("assetWells: ", assetWells)
    //     console.log("wells: ", wells)
    //     const data: any[] = []
    //     assetWells.forEach((aw) => {
    //         const well = wells?.find((w) => w.id === aw.wellId)
    //         const name = well?.name
    //         const { drillingSchedule } = aw
    //         data.push({
    //             profileName: name, profile: drillingSchedule, set: () => {}, unit: "Yee",
    //         })
    //     })
    //     return data
    // }

    // 1. funksjon som lager tomme ExplorationWells fra wells av kategori exploration
    const createMissingExplorationWellsFromWells = (expWell: ExplorationWell[]) => {
        const newExplorationWells: ExplorationWell[] = [...tableExplorationWells]
        wells?.filter((w) => IsExplorationWell(w)).forEach((w) => {
            const explorationWell = expWell.find((ew) => ew.wellId === w.id)
            if (!explorationWell) {
                const newExplorationWell = new ExplorationWell()
                newExplorationWell.explorationId = exploration.id
                newExplorationWell.wellId = w.id
                newExplorationWells.push(newExplorationWell)
            }
        })
        setTableExplorationWells(newExplorationWells)

        return newExplorationWells
    }

    // 2. funskjon som bygger tabledata basert på exploration wells

    const buildExplorationDrillingScheduleTableData = (expWell: ExplorationWell[]) => {
        console.log("assetWells: ", expWell)
        console.log("wells: ", wells)
        const data: any[] = []

        const expWells = createMissingExplorationWellsFromWells(expWell)
        console.log("Hello")

        expWells.forEach((aw) => {
            const well = wells?.find((w) => w.id === aw.wellId)
            const name = well?.name
            const { drillingSchedule } = aw
            data.push({
                profileName: name, profile: drillingSchedule, set: () => { }, unit: "Yee",
            })
        })
        console.log(expWells)
        return data
    }

    useEffect(() => {
        if (wells) {
            const explorationWellsTableData = buildExplorationDrillingScheduleTableData(explorationWells)
            console.log(explorationWellsTableData)
            setExplorationWellsGridData(explorationWellsTableData)
        }
    }, [wells, explorationWells])

    // const developmentTimeSeriesData: ITimeSeriesData[] = [
    //     {
    //         profileName: "Development cost", unit: "MNOK", profile: wellProjectCost, set: setWellProjectCost,
    //     },
    // ]

    // useEffect(() => {
    //     const newWellProject: WellProject = { ...wellProject }
    //     newWellProject.costProfile = wellProjectCost
    //     setWellProject(newWellProject)
    // }, [wellProjectCost])

    // useEffect(() => {
    //     const newExploration: Exploration = { ...exploration }
    //     newExploration.costProfile = explorationCost
    //     newExploration.seismicAcquisitionAndProcessing = seismicAcqAndProcCost
    //     newExploration.countryOfficeCost = countryOfficeCost
    //     setExploration(newExploration)
    // }, [explorationCost, seismicAcqAndProcCost, countryOfficeCost])

    const handleSave = async () => {
        // Exploration wells
        // Table exploration wells => explorationwelldto[] => explorationwells => table exploration well
        const newExplorationWells = explorationWells.filter((ew) => ew.drillingSchedule && ew.drillingSchedule.id === EMPTY_GUID)
        const newExplorationWellsResult = await (await GetExplorationWellService()).createMultipleExplorationWells(newExplorationWells)

        const updateExplorationWells = explorationWells.filter((ew) => ew.drillingSchedule && ew.drillingSchedule.id !== EMPTY_GUID)
        const updateExplorationWellsResult = await (await GetExplorationWellService()).updateMultipleExplorationWells(updateExplorationWells)

        setExploration(updateExplorationWellsResult)

        // WellProject wells
        const newWellProjectWells = wellProjectWells.filter((ew) => ew.drillingSchedule && ew.drillingSchedule.id === EMPTY_GUID)
        const newWellProjectWellsResult = await (await GetWellProjectWellService()).createMultipleWellProjectWell(newWellProjectWells)

        const updateWellProjectWells = wellProjectWells.filter((ew) => ew.drillingSchedule && ew.drillingSchedule.id !== EMPTY_GUID)
        const updateWellProjectWellsResult = await (await GetWellProjectWellService()).updateMultipleWellProjectWells(updateWellProjectWells)

        setWellProject(updateWellProjectWellsResult)
    }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Cost</PageTitle>
                <Button onClick={handleSave}>Save</Button>
            </TopWrapper>
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
                <CaseTabTable
                    caseItem={caseItem}
                    project={project}
                    setCase={setCase}
                    setProject={setProject}
                    timeSeriesData={explorationWellsGridData}
                    dg4Year={caseItem.DG4Date.getFullYear()}
                    tableYears={tableYears}
                    tableName="Exploration wells"
                    gridRef={developmentWellsGridRef}
                    alignedGridsRef={[explorationWellsGridRef]}
                />
            </TableWrapper>
            {/* <CaseTabTable
                caseItem={caseItem}
                project={project}
                setCase={setCase}
                setProject={setProject}
                timeSeriesData={explorationTimeSeriesData}
                dg4Year={caseItem.DG4Date.getFullYear()}
                tableYears={tableYears}
                tableName="Exploration well costs"
                gridRef={explorationWellsGridRef}
                alignedGridsRef={[developmentWellsGridRef]}
            /> */}
        </>
    )
}

export default CaseDrillingScheduleTab
