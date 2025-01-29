import {
    useEffect,
    useRef,
    useState,
    useMemo,
} from "react"
import { default as Grid } from "@mui/material/Grid2"
import { useParams } from "react-router"
import { useQuery } from "@tanstack/react-query"

import { useProjectContext } from "@/Context/ProjectContext"
import { useCaseContext } from "@/Context/CaseContext"
import { caseQueryFn } from "@/Services/QueryFunctions"
import CaseCostHeader from "./CaseCostHeader"
import OpexCosts from "./Tables/OpexCosts"
import CessationCosts from "./Tables/CessationCosts"
import DevelopmentWellCosts from "./Tables/DevelopmentWellCosts"
import ExplorationWellCosts from "./Tables/ExplorationWellCosts"
import OffshoreFacillityCosts from "./Tables/OffshoreFacilityCosts"
import TotalStudyCosts from "./Tables/TotalStudyCosts"
import AggregatedTotals from "./Tables/AggregatedTotalsChart"
import { SetTableYearsFromProfiles } from "../../../Tables/CaseTables/CaseTabTableHelper"
import CaseCostSkeleton from "../../../LoadingSkeletons/CaseCostTabSkeleton"
import { getYearFromDateString } from "@/Utils/DateUtils"

const CaseCostTab = ({ addEdit }: { addEdit: any }) => {
    const { activeTabCase } = useCaseContext()
    const { caseId, revisionId } = useParams()
    const { projectId, isRevision } = useProjectContext()

    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])
    const [yearRangeSetFromProfiles, setYearRangeSetFromProfiles] = useState<boolean>(false)

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

    const { data: apiData } = useQuery({
        queryKey: ["caseApiData", isRevision ? revisionId : projectId, caseId],
        queryFn: () => caseQueryFn(isRevision ? revisionId ?? "" : projectId, caseId),
        enabled: !!projectId && !!caseId,
    })

    useEffect(() => {
        if (activeTabCase === 5 && apiData && !yearRangeSetFromProfiles) {
            const caseData = apiData?.case
            SetTableYearsFromProfiles(
                [
                    apiData.totalFeasibilityAndConceptStudies,
                    apiData.totalFEEDStudies,
                    apiData.totalOtherStudiesCostProfile,
                    apiData.wellInterventionCostProfile,
                    apiData.offshoreFacilitiesOperationsCostProfile,
                    apiData.cessationWellsCost,
                    apiData.cessationOffshoreFacilitiesCost,
                    apiData.cessationOnshoreFacilitiesCostProfile,
                    apiData.totalFeasibilityAndConceptStudiesOverride,
                    apiData.totalFEEDStudiesOverride,
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
                    apiData.onshoreRelatedOPEXCostProfile,
                    apiData.additionalOPEXCostProfile,
                    apiData.appraisalWellCostProfile,
                    apiData.sidetrackCostProfile,
                ],
                getYearFromDateString(caseData.dG4Date),
                setStartYear,
                setEndYear,
                setTableYears,
            )
            setYearRangeSetFromProfiles(true)
        }
    }, [activeTabCase, apiData])

    if (activeTabCase !== 5) { return null }

    if (!apiData) {
        return <CaseCostSkeleton />
    }

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
                addEdit={addEdit}
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
                    addEdit={addEdit}
                />
            </Grid>
            <Grid size={12}>
                <OpexCosts
                    tableYears={tableYears}
                    opexGridRef={opexGridRef}
                    alignedGridsRef={alignedGridsRef}
                    apiData={apiData}
                    addEdit={addEdit}
                />
            </Grid>
            <Grid size={12}>
                <CessationCosts
                    tableYears={tableYears}
                    cessationGridRef={cessationGridRef}
                    alignedGridsRef={alignedGridsRef}
                    apiData={apiData}
                    addEdit={addEdit}
                />
            </Grid>
            <Grid size={12}>
                <OffshoreFacillityCosts
                    tableYears={tableYears}
                    capexGridRef={capexGridRef}
                    alignedGridsRef={alignedGridsRef}
                    apiData={apiData}
                    addEdit={addEdit}
                />
            </Grid>
            <Grid size={12}>
                <DevelopmentWellCosts
                    tableYears={tableYears}
                    developmentWellsGridRef={developmentWellsGridRef}
                    alignedGridsRef={alignedGridsRef}
                    apiData={apiData}
                    addEdit={addEdit}
                />
            </Grid>
            <Grid size={12}>
                <ExplorationWellCosts
                    tableYears={tableYears}
                    explorationWellsGridRef={explorationWellsGridRef}
                    alignedGridsRef={alignedGridsRef}
                    apiData={apiData}
                    addEdit={addEdit}
                />
            </Grid>
        </Grid>
    )
}

export default CaseCostTab
