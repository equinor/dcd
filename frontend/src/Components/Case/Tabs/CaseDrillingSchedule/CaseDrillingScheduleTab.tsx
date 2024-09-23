import {
    useState,
    useEffect,
    useRef,
} from "react"
import { Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useQuery } from "@tanstack/react-query"
import { useParams } from "react-router"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import SwitchableNumberInput from "../../../Input/SwitchableNumberInput"
import CaseDrillingScheduleTabTable from "./CaseDrillingScheduleAgGridTable"
import { SetTableYearsFromProfiles } from "../../Components/CaseTabTableHelper"
import { useCaseContext } from "../../../../Context/CaseContext"
import DateRangePicker from "../../../Input/TableDateRangePicker"
import CaseProductionProfilesTabSkeleton from "../../../LoadingSkeletons/CaseProductionProfilesTabSkeleton"
import { caseQueryFn, projectQueryFn } from "../../../../Services/QueryFunctions"

const CaseDrillingScheduleTab = ({ addEdit }: { addEdit: any }) => {
    const { activeTabCase } = useCaseContext()
    const { caseId } = useParams()
    const { currentContext } = useModuleCurrentContext()
    const projectId = currentContext?.externalId

    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])
    const [yearRangeSetFromProfiles, setYearRangeSetFromProfiles] = useState<boolean>(false)

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

    const { data: apiData } = useQuery({
        queryKey: ["caseApiData", projectId, caseId],
        queryFn: () => caseQueryFn(projectId, caseId),
        enabled: !!projectId && !!caseId,
    })

    const { data: projectData } = useQuery({
        queryKey: ["projectApiData", projectId],
        queryFn: () => projectQueryFn(projectId),
        enabled: !!projectId,
    })

    const wells = projectData?.wells

    useEffect(() => {
        if (activeTabCase === 3 && apiData && !yearRangeSetFromProfiles) {
            const explorationDrillingSchedule = apiData.explorationWells?.map((ew) => ew.drillingSchedule) ?? []
            const wellProjectDrillingSchedule = apiData.wellProjectWells?.map((ew) => ew.drillingSchedule) ?? []
            SetTableYearsFromProfiles(
                [...explorationDrillingSchedule, ...wellProjectDrillingSchedule],
                new Date(apiData.case.dG4Date).getFullYear(),
                setStartYear,
                setEndYear,
                setTableYears,
            )
            setYearRangeSetFromProfiles(true)
        }
    }, [activeTabCase, apiData])

    const sumWellsForWellCategory = (category: Components.Schemas.WellCategory): number => {
        if (!apiData) { return 0 }

        if (wells && wells.length > 0) {
            if (category >= 4) {
                const filteredExplorationWells = apiData.explorationWells?.filter((ew) => ew.explorationId === apiData.exploration?.id)
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
            const filteredWellProjectWells = apiData.wellProjectWells?.filter((wpw) => wpw.wellProjectId === apiData.wellProject?.id)
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
    }, [apiData, wells, activeTabCase])

    if (!apiData) { return (<CaseProductionProfilesTabSkeleton />) }

    const caseData = apiData.case
    const explorationData = apiData.exploration
    const wellProjectData = apiData.wellProject
    const wellProjectWellsData = apiData.wellProjectWells
    const explorationWellsData = apiData.explorationWells

    if (
        activeTabCase !== 3
        || !explorationWellsData
        || !caseData
        || !wellProjectWellsData
        || !explorationData
        || !wellProjectData
    ) { return (<CaseProductionProfilesTabSkeleton />) }

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
                    addEdit={addEdit}
                    resourceName="case"
                    resourcePropertyKey="producerCount" // dummy just to display swithable number input
                    label="Exploration wells"
                    previousResourceObject={caseData}
                    value={explorationWellCount}
                    integer
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="case"
                    resourcePropertyKey="producerCount" // dummy just to display swithable number input
                    label="Appraisal wells"
                    previousResourceObject={caseData}
                    value={appraisalWellCount}
                    integer
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="case"
                    resourcePropertyKey="producerCount" // dummy just to display disabled number input
                    label="Oil producer wells"
                    previousResourceObject={caseData}
                    value={oilProducerCount}
                    integer
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="case"
                    resourcePropertyKey="producerCount" // dummy just to display disabled number input
                    label="Gas producer wells"
                    previousResourceObject={caseData}
                    value={gasProducerCount}
                    integer
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="case"
                    resourcePropertyKey="producerCount" // dummy just to display disabled number input
                    label="Water injector wells"
                    previousResourceObject={caseData}
                    value={waterInjectorCount}
                    integer
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={4}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="case"
                    resourcePropertyKey="producerCount" // dummy just to display disabled number input
                    label="Gas injector wells"
                    previousResourceObject={caseData}
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
                    addEdit={addEdit}
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
                    addEdit={addEdit}
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
