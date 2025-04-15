import Grid from "@mui/material/Grid2"
import {
    useEffect,
    useRef,
    useState,
    useMemo,
} from "react"

import PROSPBar from "../Components/PROSPBar"

import AggregatedTotals from "./AggregatedTotalsChart"
import CaseCostHeader from "./CaseCostHeader"
import CessationCosts from "./Tables/CessationCosts"
import DevelopmentWellCosts from "./Tables/DevelopmentWellCosts"
import ExplorationWellCosts from "./Tables/ExplorationWellCosts"
import OffshoreFacillityCosts from "./Tables/OffshoreFacilityCosts"
import OpexCosts from "./Tables/OpexCosts"
import TotalStudyCosts from "./Tables/TotalStudyCosts"

import CaseCostSkeleton from "@/Components//LoadingSkeletons/CaseCostTabSkeleton"
import { useCaseApiData, useTableRanges } from "@/Hooks"
import { useCaseStore } from "@/Store/CaseStore"
import { useProjectContext } from "@/Store/ProjectContext"
// import { getYearFromDateString } from "@/Utils/DateUtils"
// import { calculateTableYears } from "@/Utils/TableUtils"

const CaseCostTab = (): React.ReactNode => {
    const { activeTabCase } = useCaseStore()
    const { projectId } = useProjectContext()
    const { apiData } = useCaseApiData()
    const { tableRanges, updateCaseCostYears } = useTableRanges()

    const [startYear, setStartYear] = useState<number>(0)
    const [endYear, setEndYear] = useState<number>(0)

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

    useEffect(() => {
        isMounted.current = true

        return () => {
            isMounted.current = false
        }
    }, [])

    useEffect(() => {
        if (activeTabCase === 5 && apiData && tableRanges) {
            const caseData = apiData.case

            // Initialize from backend table ranges or calculate if needed
            if (currentCaseId !== caseData.caseId) {
                setCurrentCaseId(caseData.caseId)

                if (tableRanges.caseCostYears && tableRanges.caseCostYears.length >= 2) {
                    setStartYear(tableRanges.caseCostYears[0])
                    setEndYear(tableRanges.caseCostYears[1])
                }

                // // If we don't have valid ranges yet, calculate them
                // const profiles = [
                //     apiData.totalFeasibilityAndConceptStudies,
                //     apiData.totalFeedStudies,
                //     apiData.totalOtherStudiesCostProfile,
                //     apiData.wellInterventionCostProfile,
                //     apiData.offshoreFacilitiesOperationsCostProfile,
                //     apiData.cessationWellsCost,
                //     apiData.cessationOffshoreFacilitiesCost,
                //     apiData.cessationOnshoreFacilitiesCostProfile,
                //     apiData.totalFeasibilityAndConceptStudiesOverride,
                //     apiData.totalFeedStudiesOverride,
                //     apiData.wellInterventionCostProfileOverride,
                //     apiData.offshoreFacilitiesOperationsCostProfileOverride,
                //     apiData.cessationWellsCostOverride,
                //     apiData.cessationOffshoreFacilitiesCostOverride,
                //     apiData.surfCostProfile,
                //     apiData.surfCostProfileOverride?.values?.length ? apiData.surfCostProfileOverride : undefined,
                //     apiData.topsideCostProfile,
                //     apiData.topsideCostProfileOverride?.values?.length ? apiData.topsideCostProfileOverride : undefined,
                //     apiData.substructureCostProfile,
                //     apiData.substructureCostProfileOverride?.values?.length ? apiData.substructureCostProfileOverride : undefined,
                //     apiData.transportCostProfile,
                //     apiData.transportCostProfileOverride?.values?.length ? apiData.transportCostProfileOverride : undefined,
                //     apiData.onshorePowerSupplyCostProfile,
                //     apiData.onshorePowerSupplyCostProfileOverride?.values?.length ? apiData.onshorePowerSupplyCostProfileOverride : undefined,
                //     apiData.oilProducerCostProfile,
                //     apiData.gasProducerCostProfile,
                //     apiData.waterInjectorCostProfile,
                //     apiData.gasInjectorCostProfile,
                //     apiData.oilProducerCostProfileOverride,
                //     apiData.gasProducerCostProfileOverride,
                //     apiData.waterInjectorCostProfileOverride,
                //     apiData.gasInjectorCostProfileOverride,
                //     apiData.explorationWellCostProfile,
                //     apiData.seismicAcquisitionAndProcessing,
                //     apiData.countryOfficeCost,
                //     apiData.gAndGAdminCost,
                //     apiData.gAndGAdminCostOverride,
                //     apiData.historicCostCostProfile,
                //     apiData.onshoreRelatedOpexCostProfile,
                //     apiData.additionalOpexCostProfile,
                //     apiData.appraisalWellCostProfile,
                //     apiData.sidetrackCostProfile,
                // ]

                // const dg4Year = getYearFromDateString(caseData.dg4Date)
                // const defaultYears: [number, number] = [tableRanges.caseCostYears[0], tableRanges.caseCostYears[tableRanges.caseCostYears.length - 1]]
                // const years = calculateTableYears(profiles, dg4Year, defaultYears)

                // if (isMounted.current) {
                //     const [firstYear, lastYear] = years

                //     console.log("tableyears calculated from data:", [firstYear, lastYear])
                // }
            }
        }
    }, [activeTabCase, apiData, tableRanges])

    const handleTableYearsClick = async (pickedStartYear: number, pickedEndYear: number): Promise<void> => {
        try {
            await updateCaseCostYears(pickedStartYear, pickedEndYear)
            setStartYear(pickedStartYear)
            setEndYear(pickedEndYear)
        } catch (error) {
            console.error("CaseCostTab - Error updating case cost years:", error)
        }
    }

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
                caseData={apiData.case}
                surfData={apiData.surf}
                handleTableYearsClick={handleTableYearsClick}
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
                    tableYears={[startYear, endYear]}
                />
            </Grid>

            <Grid size={12}>
                <TotalStudyCosts
                    tableYears={[startYear, endYear]}
                    studyGridRef={studyGridRef}
                    alignedGridsRef={alignedGridsRef}
                    apiData={apiData}
                />
            </Grid>
            <Grid size={12}>
                <OpexCosts
                    tableYears={[startYear, endYear]}
                    opexGridRef={opexGridRef}
                    alignedGridsRef={alignedGridsRef}
                    apiData={apiData}
                />
            </Grid>
            <Grid size={12}>
                <CessationCosts
                    tableYears={[startYear, endYear]}
                    cessationGridRef={cessationGridRef}
                    alignedGridsRef={alignedGridsRef}
                    apiData={apiData}
                />
            </Grid>
            <Grid size={12}>
                <OffshoreFacillityCosts
                    tableYears={[startYear, endYear]}
                    capexGridRef={capexGridRef}
                    alignedGridsRef={alignedGridsRef}
                    apiData={apiData}

                />
            </Grid>
            <Grid size={12}>
                <DevelopmentWellCosts
                    tableYears={[startYear, endYear]}
                    developmentWellsGridRef={developmentWellsGridRef}
                    alignedGridsRef={alignedGridsRef}
                    apiData={apiData}

                />
            </Grid>
            <Grid size={12}>
                <ExplorationWellCosts
                    tableYears={[startYear, endYear]}
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
