import {
    useEffect,
    useRef,
    useState,
    useMemo,
} from "react"
import Grid from "@mui/material/Grid"
import { useParams } from "react-router"
import { useQueryClient, useQuery } from "react-query"
import { SetTableYearsFromProfiles } from "../../Components/CaseTabTableHelper"
import { useProjectContext } from "../../../../Context/ProjectContext"
import { useCaseContext } from "../../../../Context/CaseContext"
import { useModalContext } from "../../../../Context/ModalContext"
import Header from "./Header"
import CessationCosts from "./Tables/CessationCosts"
import DevelopmentWellCosts from "./Tables/DevelopmentWellCosts"
import ExplorationWellCosts from "./Tables/ExplorationWellCosts"
import OffshoreFacillityCosts from "./Tables/OffshoreFacilityCosts"
import OpexCosts from "./Tables/OpexCosts"
import TotalStudyCosts from "./Tables/TotalStudyCosts"

const CaseCostTab = (): React.ReactElement | null => {
    const { project } = useProjectContext()

    const { activeTabCase } = useCaseContext()

    const {
        wellProject,
        exploration,
    } = useModalContext()
    const { caseId } = useParams()
    const queryClient = useQueryClient()
    const projectId = project?.id || null

    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

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

    const { data: apiData } = useQuery<Components.Schemas.CaseWithAssetsDto | undefined>(
        ["apiData", { projectId, caseId }],
        () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        {
            enabled: !!projectId && !!caseId,
            initialData: () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        },
    )

    const surfData = apiData?.surf as Components.Schemas.SurfWithProfilesDto
    const caseData = apiData?.case as Components.Schemas.CaseWithProfilesDto
    const topside = apiData?.topside as Components.Schemas.TopsideWithProfilesDto
    const substructure = apiData?.substructure as Components.Schemas.SubstructureWithProfilesDto
    const transport = apiData?.transport as Components.Schemas.TransportWithProfilesDto
    const surf = apiData?.surf as Components.Schemas.SurfWithProfilesDto

    useEffect(() => {
        (async () => {
            try {
                if (caseData && project && topside && surf && substructure && transport && exploration && wellProject) {
                    if (activeTabCase === 5) {
                        SetTableYearsFromProfiles([
                            caseData.totalFeasibilityAndConceptStudies,
                            caseData.totalFEEDStudies,
                            caseData.totalOtherStudiesCostProfile,
                            caseData.wellInterventionCostProfile,
                            caseData.offshoreFacilitiesOperationsCostProfile,
                            caseData.cessationWellsCost,
                            caseData.cessationOffshoreFacilitiesCost,
                            caseData.cessationOnshoreFacilitiesCostProfile,
                            caseData.totalFeasibilityAndConceptStudiesOverride,
                            caseData.totalFEEDStudiesOverride,
                            caseData.wellInterventionCostProfileOverride,
                            caseData.offshoreFacilitiesOperationsCostProfileOverride,
                            caseData.cessationWellsCostOverride,
                            caseData.cessationOffshoreFacilitiesCostOverride,
                            surf.costProfile,
                            surf.costProfileOverride,
                            topside.costProfile,
                            topside.costProfileOverride,
                            substructure.costProfile,
                            substructure.costProfileOverride,
                            transport.costProfileOverride,
                            transport.costProfile,
                            wellProject.oilProducerCostProfile,
                            wellProject.gasProducerCostProfile,
                            wellProject.waterInjectorCostProfile,
                            wellProject.gasInjectorCostProfile,
                            wellProject.oilProducerCostProfileOverride,
                            wellProject.gasProducerCostProfileOverride,
                            wellProject.waterInjectorCostProfileOverride,
                            wellProject.gasInjectorCostProfileOverride,
                            exploration.explorationWellCostProfile,
                            exploration.seismicAcquisitionAndProcessing,
                            exploration.countryOfficeCost,
                            exploration?.gAndGAdminCost,
                            exploration?.gAndGAdminCostOverride,
                        ], caseData.dG4Date ? new Date(caseData.dG4Date).getFullYear() : 2030, setStartYear, setEndYear, setTableYears)
                    }
                }
            } catch (error) {
                console.error("[CaseView] Error while generating cost profile", error)
            }
        })()
    }, [activeTabCase])

    if (activeTabCase !== 5) { return null }

    if (!caseData || !surfData || !apiData) {
        return <p>loading....</p>
    }

    return (
        <Grid container spacing={3}>
            <Header
                startYear={startYear}
                endYear={endYear}
                setStartYear={setStartYear}
                setEndYear={setEndYear}
                setTableYears={setTableYears}
                caseData={caseData}
                surfData={surfData}
            />
            <Grid item xs={12}>
                <TotalStudyCosts
                    tableYears={tableYears}
                    studyGridRef={studyGridRef}
                    alignedGridsRef={alignedGridsRef}
                    caseData={caseData}
                    apiData={apiData}
                />
            </Grid>
            <Grid item xs={12}>
                <OpexCosts
                    tableYears={tableYears}
                    opexGridRef={opexGridRef}
                    alignedGridsRef={alignedGridsRef}
                    caseData={caseData}
                    apiData={apiData}
                />
            </Grid>
            <Grid item xs={12}>
                <CessationCosts
                    tableYears={tableYears}
                    cessationGridRef={cessationGridRef}
                    alignedGridsRef={alignedGridsRef}
                    caseData={caseData}
                    apiData={apiData}
                />
            </Grid>
            <Grid item xs={12}>
                <OffshoreFacillityCosts
                    tableYears={tableYears}
                    capexGridRef={capexGridRef}
                    alignedGridsRef={alignedGridsRef}
                    caseData={caseData}
                    apiData={apiData}
                />
            </Grid>
            <Grid item xs={12}>
                <DevelopmentWellCosts
                    tableYears={tableYears}
                    developmentWellsGridRef={developmentWellsGridRef}
                    alignedGridsRef={alignedGridsRef}
                    caseData={caseData}
                    apiData={apiData}
                />
            </Grid>
            <Grid item xs={12}>
                <ExplorationWellCosts
                    tableYears={tableYears}
                    explorationWellsGridRef={explorationWellsGridRef}
                    alignedGridsRef={alignedGridsRef}
                    caseData={caseData}
                    apiData={apiData}
                />
            </Grid>
        </Grid>
    )
}

export default CaseCostTab
