import {
    useState,
    useEffect,
    useRef,
} from "react"
import { Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid2"
import { useQuery } from "@tanstack/react-query"
import { useParams } from "react-router"

import CaseProductionProfilesTabSkeleton from "@/Components/LoadingSkeletons/CaseProductionProfilesTabSkeleton"
import { SetTableYearsFromProfiles } from "@/Components/Tables/CaseTables/CaseTabTableHelper"
import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"
import DateRangePicker from "@/Components/Input/TableDateRangePicker"
import { useProjectContext } from "@/Context/ProjectContext"
import { useCaseContext } from "@/Context/CaseContext"
import { caseQueryFn } from "@/Services/QueryFunctions"
import { useDataFetch } from "@/Hooks/useDataFetch"
import { getYearFromDateString } from "@/Utils/DateUtils"
import CaseDrillingScheduleTable from "./CaseDrillingScheduleTable"

const CaseDrillingScheduleTab = ({ addEdit }: { addEdit: any }) => {
    const { activeTabCase } = useCaseContext()
    const { caseId, revisionId } = useParams()
    const { projectId, isRevision } = useProjectContext()
    const revisionAndProjectData = useDataFetch()

    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])
    const [yearRangeSetFromProfiles, setYearRangeSetFromProfiles] = useState<boolean>(false)

    // DevelopmentWell
    const [oilProducerCount, setOilProducerCount] = useState<number>(0)
    const [gasProducerCount, setGasProducerCount] = useState<number>(0)
    const [waterInjectorCount, setWaterInjectorCount] = useState<number>(0)
    const [gasInjectorCount, setGasInjectorCount] = useState<number>(0)

    // ExplorationWell
    const [explorationWellCount, setExplorationWellCount] = useState<number>(0)
    const [appraisalWellCount, setAppraisalWellCount] = useState<number>(0)

    const [, setSidetrackCount] = useState<number>(0)
    const developmentWellsGridRef = useRef(null)
    const explorationWellsGridRef = useRef(null)

    const { data: apiData } = useQuery({
        queryKey: ["caseApiData", isRevision ? revisionId : projectId, caseId],
        queryFn: () => caseQueryFn(isRevision ? revisionId ?? "" : projectId, caseId),
        enabled: !!projectId && !!caseId,
    })

    const wells = revisionAndProjectData?.commonProjectAndRevisionData.wells

    useEffect(() => {
        if (activeTabCase === 3 && apiData && !yearRangeSetFromProfiles) {
            const explorationDrillingSchedule = apiData.explorationWells?.map((ew) => ew.drillingSchedule) ?? []
            const developmentDrillingSchedule = apiData.developmentWells?.map((ew) => ew.drillingSchedule) ?? []
            SetTableYearsFromProfiles(
                [...explorationDrillingSchedule, ...developmentDrillingSchedule],
                getYearFromDateString(apiData.case.dG4Date),
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
                const filteredWells = wells.filter((w) => w.wellCategory === category)
                let sum = 0
                filteredWells.forEach((fw) => {
                    apiData.explorationWells?.filter((few) => few.wellId === fw.id).forEach((ew) => {
                        if (ew.drillingSchedule
                            && ew.drillingSchedule.values
                            && ew.drillingSchedule.values.length > 0) {
                            sum += ew.drillingSchedule.values.reduce((a, b) => a + b, 0)
                        }
                    })
                })
                return sum
            }
            const filteredWells = wells.filter((w) => w.wellCategory === category)
            let sum = 0
            filteredWells.forEach((fw) => {
                apiData.developmentWells?.filter((fwpw) => fwpw.wellId === fw.id).forEach((ew) => {
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
    const developmentWellsData = apiData.developmentWells
    const explorationWellsData = apiData.explorationWells

    if (
        activeTabCase !== 3
        || !explorationWellsData
        || !caseData
        || !developmentWellsData
        || !explorationData
        || !wellProjectData
    ) { return (<CaseProductionProfilesTabSkeleton />) }

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    return (
        <Grid container spacing={2}>
            <Grid size={12}>
                <Typography>Create wells in technical input in order to see them in the list below.</Typography>
            </Grid>
            <Grid container size={12} justifyContent="flex-start">
                <Grid container size={{ xs: 12, md: 10, lg: 8 }} spacing={2}>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            addEdit={addEdit}
                            resourceName="case"
                            resourcePropertyKey="producerCount"
                            label="Exploration wells"
                            previousResourceObject={caseData}
                            value={explorationWellCount}
                            integer
                            disabled
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            addEdit={addEdit}
                            resourceName="case"
                            resourcePropertyKey="producerCount"
                            label="Oil producer wells"
                            previousResourceObject={caseData}
                            value={oilProducerCount}
                            integer
                            disabled
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            addEdit={addEdit}
                            resourceName="case"
                            resourcePropertyKey="producerCount"
                            label="Water injector wells"
                            previousResourceObject={caseData}
                            value={waterInjectorCount}
                            integer
                            disabled
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            addEdit={addEdit}
                            resourceName="case"
                            resourcePropertyKey="producerCount"
                            label="Appraisal wells"
                            previousResourceObject={caseData}
                            value={appraisalWellCount}
                            integer
                            disabled
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            addEdit={addEdit}
                            resourceName="case"
                            resourcePropertyKey="producerCount"
                            label="Gas producer wells"
                            previousResourceObject={caseData}
                            value={gasProducerCount}
                            integer
                            disabled
                        />
                    </Grid>
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <SwitchableNumberInput
                            addEdit={addEdit}
                            resourceName="case"
                            resourcePropertyKey="producerCount"
                            label="Gas injector wells"
                            previousResourceObject={caseData}
                            value={gasInjectorCount}
                            integer
                            disabled
                        />
                    </Grid>
                </Grid>
            </Grid>

            <DateRangePicker
                setStartYear={setStartYear}
                setEndYear={setEndYear}
                startYear={startYear}
                endYear={endYear}
                handleTableYearsClick={handleTableYearsClick}
            />
            <Grid size={12}>
                <CaseDrillingScheduleTable
                    addEdit={addEdit}
                    assetWells={explorationWellsData}
                    dg4Year={getYearFromDateString(caseData.dG4Date)}
                    tableName="Exploration wells"
                    tableYears={tableYears}
                    resourceId={explorationData.id}
                    wells={wells}
                    isExplorationTable
                    gridRef={explorationWellsGridRef}
                    alignedGridsRef={[developmentWellsGridRef]}
                />
            </Grid>
            <Grid size={12}>
                <CaseDrillingScheduleTable
                    addEdit={addEdit}
                    assetWells={developmentWellsData}
                    dg4Year={getYearFromDateString(caseData.dG4Date)}
                    tableName="Development wells"
                    tableYears={tableYears}
                    resourceId={wellProjectData.id}
                    wells={wells}
                    isExplorationTable={false}
                    gridRef={developmentWellsGridRef}
                    alignedGridsRef={[explorationWellsGridRef]}
                />
            </Grid>
        </Grid>
    )
}

export default CaseDrillingScheduleTab
