import Grid from "@mui/material/Grid2"
import {
    useEffect,
    useRef,
    useState,
    useMemo,
} from "react"

import AggregatedTotals from "./AggregatedTotalsChart"
import CaseCostHeader from "./CaseCostHeader"
import CessationCosts from "./Tables/CessationCosts"
import DevelopmentWellCosts from "./Tables/DevelopmentWellCosts"
import ExplorationWellCosts from "./Tables/ExplorationWellCosts"
import OffshoreFacillityCosts from "./Tables/OffshoreFacilityCosts"
import OpexCosts from "./Tables/OpexCosts"
import TotalStudyCosts from "./Tables/TotalStudyCosts"

import CaseCostSkeleton from "@/Components//LoadingSkeletons/CaseCostTabSkeleton"
import { useCaseApiData } from "@/Hooks"
import { useCaseStore } from "@/Store/CaseStore"
import { DEFAULT_CASE_COST_YEARS } from "@/Utils/Config/constants"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { calculateTableYears } from "@/Utils/TableUtils"
import PROSPBar from "../Components/PROSPBar"
import { useProjectContext } from "@/Store/ProjectContext"

const CaseCostTab = () => {
    const { activeTabCase } = useCaseStore()
    const { projectId } = useProjectContext()

    const [startYear, setStartYear] = useState<number>(DEFAULT_CASE_COST_YEARS[0])
    const [endYear, setEndYear] = useState<number>(DEFAULT_CASE_COST_YEARS[1])
    const [tableYears, setTableYears] = useState<[number, number]>(DEFAULT_CASE_COST_YEARS)

    const isMounted = useRef(false)
    const [currentCaseId, setCurrentCaseId] = useState<string | undefined>()

    const studyGridRef = useRef<any>(null)
    const opexGridRef = useRef<any>(null)
    const cessationGridRef = useRef<any>(null)
    const capexGridRef = useRef<any>(null)
    const developmentWellsGridRef = useRef<any>(null)
    const explorationWellsGridRef = useRef<any>(null)

    const alignedGridsRef = useMemo(() => [
        studyGridRef,
        opexGridRef,
        cessationGridRef,
        capexGridRef,
        developmentWellsGridRef,
        explorationWellsGridRef,
    ], [studyGridRef, opexGridRef, cessationGridRef, capexGridRef, developmentWellsGridRef, explorationWellsGridRef])

    const barColors = {
        studyColor: "#004F55",
        opexColor: "#007079",
        cessationColor: "#97CACE",
        offshoreFacilityColor: "#79C591",
        developmentWellColor: "#B39EEB",
        explorationWellColor: "#FF7D7D",
        totalIncomeColor: "#9F9F9F",
    }

    const { apiData } = useCaseApiData()

    useEffect(() => {
        isMounted.current = true

        return () => {
            isMounted.current = false
        }
    }, [])

    useEffect(() => {
        if (activeTabCase === 5 && apiData) {
            const caseData = apiData.case

            // Check if we're switching to a different case
            if (currentCaseId !== caseData.caseId) {
                setCurrentCaseId(caseData.caseId)
                const defaultYears: [number, number] = DEFAULT_CASE_COST_YEARS

                setStartYear(defaultYears[0])
                setEndYear(defaultYears[1])
                setTableYears(defaultYears)
            }
            const profiles = [
                apiData.totalFeasibilityAndConceptStudies,
                apiData.totalFeedStudies,
                apiData.totalOtherStudiesCostProfile,
                apiData.wellInterventionCostProfile,
                apiData.offshoreFacilitiesOperationsCostProfile,
                apiData.cessationWellsCost,
                apiData.cessationOffshoreFacilitiesCost,
                apiData.cessationOnshoreFacilitiesCostProfile,
                apiData.totalFeasibilityAndConceptStudiesOverride,
                apiData.totalFeedStudiesOverride,
                apiData.wellInterventionCostProfileOverride,
                apiData.offshoreFacilitiesOperationsCostProfileOverride,
                apiData.cessationWellsCostOverride,
                apiData.cessationOffshoreFacilitiesCostOverride,
                apiData.surfCostProfile,
                apiData.surfCostProfileOverride?.values?.length ? apiData.surfCostProfileOverride : undefined,
                apiData.topsideCostProfile,
                apiData.topsideCostProfileOverride?.values?.length ? apiData.topsideCostProfileOverride : undefined,
                apiData.substructureCostProfile,
                apiData.substructureCostProfileOverride?.values?.length ? apiData.substructureCostProfileOverride : undefined,
                apiData.transportCostProfile,
                apiData.transportCostProfileOverride?.values?.length ? apiData.transportCostProfileOverride : undefined,
                apiData.onshorePowerSupplyCostProfile,
                apiData.onshorePowerSupplyCostProfileOverride?.values?.length ? apiData.onshorePowerSupplyCostProfileOverride : undefined,
                apiData.oilProducerCostProfile,
                apiData.gasProducerCostProfile,
                apiData.waterInjectorCostProfile,
                apiData.gasInjectorCostProfile,
                apiData.oilProducerCostProfileOverride,
                apiData.gasProducerCostProfileOverride,
                apiData.waterInjectorCostProfileOverride,
                apiData.gasInjectorCostProfileOverride,
                apiData.explorationWellCostProfile,
                apiData.seismicAcquisitionAndProcessing,
                apiData.countryOfficeCost,
                apiData.gAndGAdminCost,
                apiData.gAndGAdminCostOverride,
                apiData.historicCostCostProfile,
                apiData.onshoreRelatedOpexCostProfile,
                apiData.additionalOpexCostProfile,
                apiData.appraisalWellCostProfile,
                apiData.sidetrackCostProfile,
            ]

            console.log("Number of profiles provided to calculateTableYears:", profiles.length)

            const dg4Year = getYearFromDateString(caseData.dg4Date)

            console.log("dg4Year:", dg4Year)

            const years = calculateTableYears(profiles, dg4Year)

            console.log("calculateTableYears returned:", years)

            if (years && isMounted.current) {
                const [firstYear, lastYear] = years

                setStartYear(firstYear)
                setEndYear(lastYear)
                setTableYears([firstYear, lastYear])
            } else if (isMounted.current) {
                const defaultYears = DEFAULT_CASE_COST_YEARS

                setStartYear(defaultYears[0])
                setEndYear(defaultYears[1])
                setTableYears(defaultYears)
            }
        }
    }, [activeTabCase, apiData])

    if (activeTabCase !== 5) { return null }

    if (!apiData) {
        return <CaseCostSkeleton />
    }
    
    const caseData = apiData.case

    return (
        <Grid container spacing={2}>
            <CaseCostHeader
                startYear={startYear}
                endYear={endYear}
                setStartYear={setStartYear}
                setEndYear={setEndYear}
                setTableYears={setTableYears}
                caseData={apiData.case}
                surfData={apiData.surf}
            />
            
            <Grid size={12}>
                <AggregatedTotals
                    apiData={apiData}
                    barColors={[
                        barColors.studyColor,
                        barColors.opexColor,
                        barColors.cessationColor,
                        barColors.offshoreFacilityColor,
                        barColors.developmentWellColor,
                        barColors.explorationWellColor,
                        barColors.totalIncomeColor]}
                    enableLegend
                    tableYears={tableYears}
                />
            </Grid>

            <Grid size={12}>
                <TotalStudyCosts
                    tableYears={tableYears}
                    studyGridRef={studyGridRef}
                    alignedGridsRef={alignedGridsRef}
                    apiData={apiData}
                />
            </Grid>
            <Grid size={12}>
                <OpexCosts
                    tableYears={tableYears}
                    opexGridRef={opexGridRef}
                    alignedGridsRef={alignedGridsRef}
                    apiData={apiData}
                />
            </Grid>
            <Grid size={12}>
                <CessationCosts
                    tableYears={tableYears}
                    cessationGridRef={cessationGridRef}
                    alignedGridsRef={alignedGridsRef}
                    apiData={apiData}
                />
            </Grid>
            <Grid size={12}>
                <OffshoreFacillityCosts
                    tableYears={tableYears}
                    capexGridRef={capexGridRef}
                    alignedGridsRef={alignedGridsRef}
                    apiData={apiData}

                />
            </Grid>
            <Grid size={12}>
                <DevelopmentWellCosts
                    tableYears={tableYears}
                    developmentWellsGridRef={developmentWellsGridRef}
                    alignedGridsRef={alignedGridsRef}
                    apiData={apiData}

                />
            </Grid>
            <Grid size={12}>
                <ExplorationWellCosts
                    tableYears={tableYears}
                    explorationWellsGridRef={explorationWellsGridRef}
                    alignedGridsRef={alignedGridsRef}
                    apiData={apiData}

                />
            </Grid>
            <PROSPBar 
                projectId={projectId} 
                caseId={caseData.caseId} 
                currentSharePointFileId={caseData.sharepointFileId || null} 
            />
        </Grid>
    )
}

export default CaseCostTab
