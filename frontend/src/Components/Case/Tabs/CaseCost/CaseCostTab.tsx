import {
    useEffect,
    useRef,
    useState,
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
import Opex from "./Tables/Opex"
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
                    alignedGridsRef={[
                        opexGridRef,
                        cessationGridRef,
                        capexGridRef,
                        developmentWellsGridRef,
                        explorationWellsGridRef,
                    ]}
                />
            </Grid>
            <Grid item xs={12}>
                <Opex
                    tableYears={tableYears}
                    opexGridRef={opexGridRef}
                    alignedGridsRef={[
                        studyGridRef,
                        cessationGridRef,
                        capexGridRef,
                        developmentWellsGridRef,
                        explorationWellsGridRef,
                    ]}
                />
            </Grid>
            <Grid item xs={12}>
                <CessationCosts
                    tableYears={tableYears}
                    cessationGridRef={cessationGridRef}
                    alignedGridsRef={[
                        studyGridRef,
                        opexGridRef,
                        capexGridRef,
                        developmentWellsGridRef,
                        explorationWellsGridRef,
                    ]}
                />
            </Grid>
            <Grid item xs={12}>
                <OffshoreFacillityCosts
                    tableYears={tableYears}
                    capexGridRef={capexGridRef}
                    alignedGridsRef={[
                        studyGridRef,
                        opexGridRef,
                        cessationGridRef,
                        developmentWellsGridRef,
                        explorationWellsGridRef,
                    ]}
                />
            </Grid>
            <Grid item xs={12}>
                <DevelopmentWellCosts
                    tableYears={tableYears}
                    developmentWellsGridRef={developmentWellsGridRef}
                    alignedGridsRef={[
                        studyGridRef,
                        opexGridRef,
                        cessationGridRef,
                        capexGridRef,
                        explorationWellsGridRef,
                    ]}
                />
            </Grid>
            <Grid item xs={12}>
                <ExplorationWellCosts
                    tableYears={tableYears}
                    explorationWellsGridRef={explorationWellsGridRef}
                    alignedGridsRef={[
                        studyGridRef,
                        opexGridRef,
                        cessationGridRef,
                        capexGridRef,
                        developmentWellsGridRef,
                    ]}
                />
            </Grid>
        </Grid>
    )
}

export default CaseCostTab
