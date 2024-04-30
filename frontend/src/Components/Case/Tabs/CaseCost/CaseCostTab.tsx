import {
    useEffect,
    useRef,
    useState,
    useMemo,
} from "react"
import Grid from "@mui/material/Grid"
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

    const {
        projectCase,
        activeTabCase,
        topside,
        surf,
        substructure,
        transport,
    } = useCaseContext()

    const {
        wellProject,
        exploration,
    } = useModalContext()

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
        opexGridRef,
        cessationGridRef,
        capexGridRef,
        developmentWellsGridRef,
        explorationWellsGridRef,
    ], [opexGridRef, cessationGridRef, capexGridRef, developmentWellsGridRef, explorationWellsGridRef])

    useEffect(() => {
        (async () => {
            try {
                if (projectCase && project && topside && surf && substructure && transport && exploration && wellProject) {
                    if (activeTabCase === 5) {
                        SetTableYearsFromProfiles([
                            projectCase.totalFeasibilityAndConceptStudies,
                            projectCase.totalFEEDStudies,
                            projectCase.wellInterventionCostProfile,
                            projectCase.offshoreFacilitiesOperationsCostProfile,
                            projectCase.cessationWellsCost,
                            projectCase.cessationOffshoreFacilitiesCost,
                            projectCase.cessationOnshoreFacilitiesCostProfile,
                            projectCase.totalFeasibilityAndConceptStudiesOverride,
                            projectCase.totalFEEDStudiesOverride,
                            projectCase.wellInterventionCostProfileOverride,
                            projectCase.offshoreFacilitiesOperationsCostProfileOverride,
                            projectCase.cessationWellsCostOverride,
                            projectCase.cessationOffshoreFacilitiesCostOverride,
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
                        ], projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030, setStartYear, setEndYear, setTableYears)
                    }
                }
            } catch (error) {
                console.error("[CaseView] Error while generating cost profile", error)
            }
        })()
    }, [activeTabCase])

    if (activeTabCase !== 5) { return null }

    return (
        <Grid container spacing={3}>
            <Header
                startYear={startYear}
                endYear={endYear}
                setStartYear={setStartYear}
                setEndYear={setEndYear}
                setTableYears={setTableYears}
            />
            <Grid item xs={12}>
                <TotalStudyCosts
                    tableYears={tableYears}
                    studyGridRef={studyGridRef}
                    alignedGridsRef={alignedGridsRef}
                />
            </Grid>
            <Grid item xs={12}>
                <OpexCosts
                    tableYears={tableYears}
                    opexGridRef={opexGridRef}
                    alignedGridsRef={alignedGridsRef}
                />
            </Grid>
            <Grid item xs={12}>
                <CessationCosts
                    tableYears={tableYears}
                    cessationGridRef={cessationGridRef}
                    alignedGridsRef={alignedGridsRef}
                />
            </Grid>
            <Grid item xs={12}>
                <OffshoreFacillityCosts
                    tableYears={tableYears}
                    capexGridRef={capexGridRef}
                    alignedGridsRef={alignedGridsRef}
                />
            </Grid>
            <Grid item xs={12}>
                <DevelopmentWellCosts
                    tableYears={tableYears}
                    developmentWellsGridRef={developmentWellsGridRef}
                    alignedGridsRef={alignedGridsRef}
                />
            </Grid>
            <Grid item xs={12}>
                <ExplorationWellCosts
                    tableYears={tableYears}
                    explorationWellsGridRef={explorationWellsGridRef}
                    alignedGridsRef={alignedGridsRef}
                />
            </Grid>
        </Grid>
    )
}

export default CaseCostTab
