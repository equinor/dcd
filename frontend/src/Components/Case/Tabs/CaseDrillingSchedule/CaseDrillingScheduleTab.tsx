import {
    useState,
    useEffect,
    useRef,
} from "react"

import { Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useQuery, useQueryClient } from "react-query"
import { useParams } from "react-router"
import SwitchableNumberInput from "../../../Input/SwitchableNumberInput"
import CaseDrillingScheduleTabTable from "./CaseDrillingScheduleAgGridTable"
import { SetTableYearsFromProfiles } from "../../Components/CaseTabTableHelper"
import { useProjectContext } from "../../../../Context/ProjectContext"
import { useCaseContext } from "../../../../Context/CaseContext"
import DateRangePicker from "../../../Input/TableDateRangePicker"

const CaseDrillingScheduleTab = () => {
    const { project } = useProjectContext()
    const { activeTabCase } = useCaseContext()
    const queryClient = useQueryClient()
    const { caseId } = useParams()
    const projectId = project?.id || null
    const wells = project?.wells

    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

    // WellProjectWell
    const [oilProducerCount, setOilProducerCount] = useState<number>(0)
    const [gasProducerCount, setGasProducerCount] = useState<number>(0)
    const [waterInjectorCount, setWaterInjectorCount] = useState<number>(0)
    const [gasInjectorCount, setGasInjectorCount] = useState<number>(0)

    // ExplorationWell
    const [explorationWellCount, setExplorationWellCount] = useState<number>(0)
    const [appraisalWellCount, setAppraisalWellCount] = useState<number>(0)

    const [, setSidetrackCount] = useState<number>(0)
    const wellProjectWellsGridRef = useRef(null)
    const explorationWellsGridRef = useRef(null)

    const { data: apiData } = useQuery<Components.Schemas.CaseWithAssetsDto | undefined>(
        ["apiData", { projectId, caseId }],
        () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        {
            enabled: !!projectId && !!caseId,
            initialData: () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        },
    )

    const wellProjectWellsData = apiData?.wellProjectWells
    const explorationWellsData = apiData?.explorationWells
    const explorationData = apiData?.exploration
    const wellProjectData = apiData?.wellProject
    const caseData = apiData?.case

    useEffect(() => {
        if (activeTabCase === 3 && caseData) {
            const explorationDrillingSchedule = explorationWellsData?.map((ew) => ew.drillingSchedule) ?? []
            const wellProjectDrillingSchedule = wellProjectWellsData?.map((ew) => ew.drillingSchedule) ?? []
            SetTableYearsFromProfiles(
                [...explorationDrillingSchedule, ...wellProjectDrillingSchedule],
                new Date(caseData.dG4Date).getFullYear(),
                setStartYear,
                setEndYear,
                setTableYears,
            )
        }
    }, [activeTabCase])

    const sumWellsForWellCategory = (category: Components.Schemas.WellCategory): number => {
        if (wells && wells.length > 0) {
            if (category >= 4) {
                const filteredExplorationWells = explorationWellsData?.filter((ew) => ew.explorationId === explorationData?.id)
                const filteredWells = wells.filter((w) => w.wellCategory === category)
                let sum = 0
                filteredWells.forEach((fw) => {
                    filteredExplorationWells?.filter((few) => few.wellId === fw.id).forEach((ew) => {
                        if (ew.drillingSchedule
                            && ew.drillingSchedule.values
                            && ew.drillingSchedule.values.length > 0) {
                            sum += ew.drillingSchedule.values.reduce((a, b) => a + b, 0)
                        }
                    })
                })
                return sum
            }
            const filteredWellProjectWells = wellProjectWellsData?.filter((wpw) => wpw.wellProjectId === wellProjectData?.id)
            const filteredWells = wells.filter((w) => w.wellCategory === category)
            let sum = 0
            filteredWells.forEach((fw) => {
                filteredWellProjectWells?.filter((fwpw) => fwpw.wellId === fw.id).forEach((ew) => {
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
        if (activeTabCase === 3) {
            setOilProducerCount(sumWellsForWellCategory(0))
            setGasProducerCount(sumWellsForWellCategory(1))
            setWaterInjectorCount(sumWellsForWellCategory(2))
            setGasInjectorCount(sumWellsForWellCategory(3))
            setExplorationWellCount(sumWellsForWellCategory(4))
            setAppraisalWellCount(sumWellsForWellCategory(5))
            setSidetrackCount(sumWellsForWellCategory(6))
        }
    }, [wells, explorationWellsData, wellProjectWellsData, activeTabCase])

    if (
        activeTabCase !== 3
        || !explorationWellsData
        || !caseData
        || !wellProjectWellsData
        || !explorationData
        || !wellProjectData
    ) { return null }

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    return (
        <Grid container spacing={2}>
            <Grid item xs={12}>
                <Typography>Create wells in technical input in order to see them in the list below.</Typography>
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    resourceName="case"
                    resourcePropertyKey="producerCount" // dummy just to display swithable number input
                    label="Exploration wells"
                    value={explorationWellCount}
                    integer
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    resourceName="case"
                    resourcePropertyKey="producerCount" // dummy just to display swithable number input
                    label="Appraisal wells"
                    value={appraisalWellCount}
                    integer
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    resourceName="case"
                    resourcePropertyKey="producerCount" // dummy just to display disabled number input
                    label="Oil producer wells"
                    value={oilProducerCount}
                    integer
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    resourceName="case"
                    resourcePropertyKey="producerCount" // dummy just to display disabled number input
                    label="Gas producer wells"
                    value={gasProducerCount}
                    integer
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    resourceName="case"
                    resourcePropertyKey="producerCount" // dummy just to display disabled number input
                    label="Water injector wells"
                    value={waterInjectorCount}
                    integer
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    resourceName="case"
                    resourcePropertyKey="producerCount" // dummy just to display disabled number input
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
                handleTableYearsClick={handleTableYearsClick}
            />
            <Grid item xs={12}>
                <CaseDrillingScheduleTabTable
                    assetWells={explorationWellsData}
                    dg4Year={caseData.dG4Date !== undefined ? new Date(caseData.dG4Date).getFullYear() : 2030}
                    tableName="Exploration wells"
                    tableYears={tableYears}
                    resourceId={explorationData.id}
                    wells={wells}
                    isExplorationTable
                    gridRef={explorationWellsGridRef}
                    alignedGridsRef={[wellProjectWellsGridRef]}
                />
            </Grid>
            <Grid item xs={12}>
                <CaseDrillingScheduleTabTable
                    assetWells={wellProjectWellsData}
                    dg4Year={caseData.dG4Date !== undefined ? new Date(caseData.dG4Date).getFullYear() : 2030}
                    tableName="Development wells"
                    tableYears={tableYears}
                    resourceId={wellProjectData.id}
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
