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
import CaseDrillingScheduleTabTable from "./CaseDrillingScheduleAgGridTable"
import { SetTableYearsFromProfiles } from "../../Components/CaseTabTableHelper"
import { useProjectContext } from "../../../../Context/ProjectContext"
import { useCaseContext } from "../../../../Context/CaseContext"
import DateRangePicker from "../../../Input/TableDateRangePicker"

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
    const { projectCase, activeTabCase } = useCaseContext()
    if (!projectCase) { return null }
    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

    const datePickerValue = (() => {
        if (project?.currency === 1) {
            return "MNOK"
        } if (project?.currency === 2) {
            return "MUSD"
        }
        return ""
    })()

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
                <SwitchableNumberInput
                    label="Exploration wells"
                    value={explorationWellCount}
                    integer
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    label="Appraisal wells"
                    value={appraisalWellCount}
                    integer
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    label="Oil producer wells"
                    value={oilProducerCount}
                    integer
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    label="Gas producer wells"
                    value={gasProducerCount}
                    integer
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    label="Water injector wells"
                    value={waterInjectorCount}
                    integer
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    label="Gas injector wells"
                    value={gasInjectorCount}
                    integer
                    disabled
                />
            </Grid>
            <DateRangePicker
                setStartYear={setStartYear}
                setEndYear={setEndYear}
                startYear={startYear}
                endYear={endYear}
                labelText="Currency"
                labelValue={datePickerValue}
                handleTableYearsClick={handleTableYearsClick}
            />
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
