import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
    useState,
    useEffect,
    useRef,
} from "react"

import {
    Button, NativeSelect, Typography,
} from "@equinor/eds-core-react"
import CaseNumberInput from "../../../Input/CaseNumberInput"
import CaseDrillingScheduleTabTable from "./CaseDrillingScheduleAgGridTable"
import { SetTableYearsFromProfiles } from "../../Components/CaseTabTableHelper"
import InputSwitcher from "../../../Input/InputSwitcher"
import Grid from "@mui/material/Grid"
import { useProjectContext } from "../../../../Context/ProjectContext"
import { useCaseContext } from "../../../../Context/CaseContext"

interface Props {
    explorationWells: Components.Schemas.ExplorationWellDto[],
    setExplorationWells: Dispatch<SetStateAction<Components.Schemas.ExplorationWellDto[] | undefined>>,
    wellProjectWells: Components.Schemas.WellProjectWellDto[],
    setWellProjectWells: Dispatch<SetStateAction<Components.Schemas.WellProjectWellDto[] | undefined>>,
    wells: Components.Schemas.WellDto[] | undefined
    exploration: Components.Schemas.ExplorationDto,
    wellProject: Components.Schemas.WellProjectDto,
}

const CaseDrillingScheduleTab = ({
    explorationWells,
    setExplorationWells,
    wellProjectWells,
    setWellProjectWells,
    wells,
    exploration,
    wellProject,
}: Props) => {
    const { project } = useProjectContext()
    const { projectCase, projectCaseEdited, setProjectCaseEdited, activeTabCase } = useCaseContext()
    if (!projectCase) return (<></>)
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
        if (activeTabCase === 3 && projectCase.dG4Date !== undefined) {
            const explorationDrillingSchedule = explorationWells?.map((ew) => ew.drillingSchedule) ?? []
            const wellProjectDrillingSchedule = wellProjectWells?.map((ew) => ew.drillingSchedule) ?? []
            SetTableYearsFromProfiles(
                [...explorationDrillingSchedule, ...wellProjectDrillingSchedule],
                new Date(projectCase.dG4Date).getFullYear(),
                setStartYear,
                setEndYear,
                setTableYears,
            )
        }
    }, [activeTabCase])

    useEffect(() => {
        if (activeTabCase === 3) {
            setOilProducerCount(sumWellsForWellCategory(0))
            setGasProducerCount(sumWellsForWellCategory(1))
            setWaterInjectorCount(sumWellsForWellCategory(2))
            setGasInjectorCount(sumWellsForWellCategory(3))
            setExplorationWellCount(sumWellsForWellCategory(4))
            setAppraisalWellCount(sumWellsForWellCategory(5))
            setSidetrackCount(sumWellsForWellCategory(6))
        }
    }, [wells, explorationWells, wellProjectWells, activeTabCase])

    if (activeTabCase !== 3) { return null }

    return (
        <Grid container spacing={2}>
            <Grid item xs={12}>
                <Typography>Create wells in technical input in order to see them in the list below.</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={explorationWellCount.toString()}
                    label="Exploration wells"
                >
                    <CaseNumberInput
                        onChange={() => { }}
                        defaultValue={explorationWellCount}
                        integer
                        disabled
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={appraisalWellCount.toString()}
                    label="Appraisal wells"
                >
                    <CaseNumberInput
                        onChange={() => { }}
                        defaultValue={appraisalWellCount}
                        integer
                        disabled
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={oilProducerCount.toString()}
                    label="Oil producer wells"
                >
                    <CaseNumberInput
                        onChange={() => { }}
                        defaultValue={oilProducerCount}
                        integer
                        disabled
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={gasProducerCount.toString()}
                    label="Gas producer wells"
                >
                    <CaseNumberInput
                        onChange={() => { }}
                        defaultValue={gasProducerCount}
                        integer
                        disabled
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={waterInjectorCount.toString()}
                    label="Water injector wells"
                >
                    <CaseNumberInput
                        onChange={() => { }}
                        defaultValue={waterInjectorCount}
                        integer
                        disabled
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={gasInjectorCount.toString()}
                    label="Gas injector wells"
                >
                    <CaseNumberInput
                        onChange={() => { }}
                        defaultValue={gasInjectorCount}
                        integer
                        disabled
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} container spacing={1} justifyContent="flex-end" alignItems="flex-end">
                <Grid item>
                    <NativeSelect
                        id="currency"
                        label="Currency"
                        onChange={() => { }}
                        value={project?.currency}
                        disabled
                    >
                        <option key="1" value={1}>MNOK</option>
                        <option key="2" value={2}>MUSD</option>
                    </NativeSelect>
                </Grid>
                <Grid item>
                    <CaseNumberInput
                        onChange={handleStartYearChange}
                        defaultValue={startYear}
                        integer
                        label="Start year"
                    />
                </Grid>
                <Grid item>
                    <CaseNumberInput
                        onChange={handleEndYearChange}
                        defaultValue={endYear}
                        integer
                        label="End year"
                        min={2010}
                        max={2100}
                    />
                </Grid>
                <Grid item>
                    <Button onClick={handleTableYearsClick}>
                        Apply
                    </Button>
                </Grid>
            </Grid>
            <Grid item xs={12}>
                <CaseDrillingScheduleTabTable
                    assetWells={explorationWells}
                    dg4Year={projectCase.dG4Date !== undefined ? new Date(projectCase.dG4Date).getFullYear() : 2030}
                    setAssetWells={setExplorationWells}
                    tableName="Exploration wells"
                    tableYears={tableYears}
                    assetId={exploration.id}
                    wells={wells}
                    isExplorationTable
                    gridRef={explorationWellsGridRef}
                    alignedGridsRef={[wellProjectWellsGridRef]}
                />
            </Grid>
            <Grid item xs={12}>
                <CaseDrillingScheduleTabTable
                    assetWells={wellProjectWells}
                    dg4Year={projectCase.dG4Date !== undefined ? new Date(projectCase.dG4Date).getFullYear() : 2030}
                    setAssetWells={setWellProjectWells}
                    tableName="Development wells"
                    tableYears={tableYears}
                    assetId={wellProject.id}
                    wells={wells}
                    isExplorationTable={false}
                    gridRef={wellProjectWellsGridRef}
                    alignedGridsRef={[explorationWellsGridRef]}
                />
            </Grid>
        </Grid>
    )
}

export default CaseDrillingScheduleTab
