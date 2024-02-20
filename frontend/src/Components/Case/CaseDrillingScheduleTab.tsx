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
import CaseNumberInput from "./CaseNumberInput"
import CaseDrillingScheduleTabTable from "./CaseDrillingScheduleTabTable"
import { SetTableYearsFromProfiles } from "./CaseTabTableHelper"
import InputContainer from "../Input/InputContainer"
import FilterContainer from "../Input/FilterContainer"

const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    margin-top: 20px;
    margin-bottom: 20px;
`
const PageTitle = styled(Typography)`
    flex-grow: 1;
`

const YearInputWrapper = styled.div`
    width: 80px;
    padding-right: 10px;
`

const TableWrapper = styled.div`
    margin-bottom: 50px;
`

interface Props {
    project: Components.Schemas.ProjectDto,
    caseItem: Components.Schemas.CaseDto,
    exploration: Components.Schemas.ExplorationDto,
    wellProject: Components.Schemas.WellProjectDto,
    explorationWells: Components.Schemas.ExplorationWellDto[],
    setExplorationWells: Dispatch<SetStateAction<Components.Schemas.ExplorationWellDto[] | undefined>>,
    wellProjectWells: Components.Schemas.WellProjectWellDto[],
    setWellProjectWells: Dispatch<SetStateAction<Components.Schemas.WellProjectWellDto[] | undefined>>,
    wells: Components.Schemas.WellDto[] | undefined
    activeTab: number
}

const CaseDrillingScheduleTab = ({
    project,
    caseItem,
    exploration,
    wellProject,
    explorationWells, setExplorationWells,
    wellProjectWells, setWellProjectWells,
    wells, activeTab,
}: Props) => {
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
    const [, setSidetrackCount] = useState<number>(0)

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
        if (activeTab === 3 && caseItem.dG4Date !== undefined) {
            const explorationDrillingSchedule = explorationWells?.map((ew) => ew.drillingSchedule) ?? []
            const wellProjectDrillingSchedule = wellProjectWells?.map((ew) => ew.drillingSchedule) ?? []
            SetTableYearsFromProfiles(
                [...explorationDrillingSchedule, ...wellProjectDrillingSchedule],
                new Date(caseItem.dG4Date).getFullYear(),
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

    if (activeTab !== 3) { return null }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Drilling schedule</PageTitle>
            </TopWrapper>
            <p>Create wells in technical input in order to see them in the list below.</p>

            <InputContainer mobileColumns={1} desktopColumns={2} breakPoint={850}>
                <CaseNumberInput
                    onChange={() => { }}
                    defaultValue={explorationWellCount}
                    integer
                    disabled
                    label="Exploration wells"
                />
                <CaseNumberInput
                    onChange={() => { }}
                    defaultValue={appraisalWellCount}
                    integer
                    disabled
                    label="Appraisal wells"
                />
                <CaseNumberInput
                    onChange={() => { }}
                    defaultValue={oilProducerCount}
                    integer
                    disabled
                    label="Oil producer wells"
                />
                <CaseNumberInput
                    onChange={() => { }}
                    defaultValue={gasProducerCount}
                    integer
                    disabled
                    label="Gas producer wells"
                />
                <CaseNumberInput
                    onChange={() => { }}
                    defaultValue={waterInjectorCount}
                    integer
                    disabled
                    label="Water injector wells"
                />
                <CaseNumberInput
                    onChange={() => { }}
                    defaultValue={gasInjectorCount}
                    integer
                    disabled
                    label="Gas injector wells"
                />
            </InputContainer>
            <FilterContainer>
                <NativeSelect
                    id="currency"
                    label="Currency"
                    onChange={() => { }}
                    value={project.currency}
                    disabled
                >
                    <option key="1" value={1}>MNOK</option>
                    <option key="2" value={2}>MUSD</option>
                </NativeSelect>
                <CaseNumberInput
                    onChange={handleStartYearChange}
                    defaultValue={startYear}
                    integer
                    label="Start year"
                />

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
            </FilterContainer>
            <TableWrapper>
                <CaseDrillingScheduleTabTable
                    assetWells={explorationWells}
                    dg4Year={caseItem.dG4Date !== undefined ? new Date(caseItem.dG4Date).getFullYear() : 2030}
                    setAssetWells={setExplorationWells}
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
                    dg4Year={caseItem.dG4Date !== undefined ? new Date(caseItem.dG4Date).getFullYear() : 2030}
                    setAssetWells={setWellProjectWells}
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
