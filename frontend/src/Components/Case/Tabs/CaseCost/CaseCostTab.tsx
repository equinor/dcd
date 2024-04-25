import {
    useEffect,
    useRef,
    useState,
} from "react"
import Grid from "@mui/material/Grid"
import CaseTabTable from "../../Components/CaseTabTable"
import { SetTableYearsFromProfiles } from "../../Components/CaseTabTableHelper"
import { ITimeSeriesData } from "../../../../Models/ITimeSeriesData"
import { useProjectContext } from "../../../../Context/ProjectContext"
import { useCaseContext } from "../../../../Context/CaseContext"
import { useModalContext } from "../../../../Context/ModalContext"
import Header from "./Header"
import CessationCosts from "./Tables/CessationCosts"
import { updateObject } from "../../../../Utils/common"
import DevelopmentWellCosts from "./Tables/DevelopmentWellCosts"
import ExplorationWellCosts from "./Tables/ExplorationWellCosts"
import OffshoreFacillityCosts from "./Tables/OffshoreFacilityCosts"

const CaseCostTab = (): React.ReactElement | null => {
    const {
        projectCase,
        projectCaseEdited, setProjectCaseEdited,
        activeTabCase,
        totalFeasibilityAndConceptStudies,
        setTotalFeasibilityAndConceptStudies,
        totalFeasibilityAndConceptStudiesOverride,
        setTotalFeasibilityAndConceptStudiesOverride,
        totalFEEDStudies,
        setTotalFEEDStudies,
        totalFEEDStudiesOverride,
        setTotalFEEDStudiesOverride,
        totalOtherStudies,
        setTotalOtherStudies,
        topside, setTopside,
        topsideCost,
        surf, setSurf,
        surfCost,
        substructure, setSubstructure,
        substructureCost,
        transport, setTransport,
        transportCost,
        // OPEX
        historicCostCostProfile,
        setHistoricCostCostProfile,
        wellInterventionCostProfile,
        setWellInterventionCostProfile,
        offshoreFacilitiesOperationsCostProfile,
        setOffshoreFacilitiesOperationsCostProfile,
        onshoreRelatedOPEXCostProfile,
        setOnshoreRelatedOPEXCostProfile,
        additionalOPEXCostProfile,
        setAdditionalOPEXCostProfile,

    } = useCaseContext()
    const {
        wellProject,
        exploration,
    } = useModalContext()

    const { project } = useProjectContext()

    // OPEX
    const [offshoreFacilitiesOperationsCostProfileOverride, setOffshoreFacilitiesOperationsCostProfileOverride] = useState<Components.Schemas.OffshoreFacilitiesOperationsCostProfileOverrideDto>()
    const [wellInterventionCostProfileOverride, setWellInterventionCostProfileOverride] = useState<Components.Schemas.WellInterventionCostProfileOverrideDto>()

    // Exploration

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
                        setTotalFeasibilityAndConceptStudies(projectCase.totalFeasibilityAndConceptStudies)
                        setTotalFeasibilityAndConceptStudiesOverride(projectCase.totalFeasibilityAndConceptStudiesOverride)

                        setTotalFEEDStudies(projectCase.totalFEEDStudies)
                        setTotalFEEDStudiesOverride(projectCase.totalFEEDStudiesOverride)

                        setWellInterventionCostProfile(projectCase.wellInterventionCostProfile)
                        setWellInterventionCostProfileOverride(projectCase.wellInterventionCostProfileOverride)

                        setOffshoreFacilitiesOperationsCostProfile(projectCase.offshoreFacilitiesOperationsCostProfile)
                        setOffshoreFacilitiesOperationsCostProfileOverride(projectCase.offshoreFacilitiesOperationsCostProfileOverride)

                        setHistoricCostCostProfile(projectCase.historicCostCostProfile)
                        setOnshoreRelatedOPEXCostProfile(projectCase.onshoreRelatedOPEXCostProfile)
                        setAdditionalOPEXCostProfile(projectCase.additionalOPEXCostProfile)
                        setTotalOtherStudies(projectCase.totalOtherStudies)

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

    useEffect(() => {
        if (studyGridRef.current
            && studyGridRef.current.api
            && studyGridRef.current.api.refreshCells) {
            studyGridRef.current.api.refreshCells()

            console.log("Refreshing study grid")
        }
    }, [totalFeasibilityAndConceptStudies, totalFEEDStudies, totalOtherStudies])

    useEffect(() => {
        if (opexGridRef.current
            && opexGridRef.current.api
            && opexGridRef.current.api.refreshCells) {
            opexGridRef.current.api.refreshCells()

            console.log("Refreshing opex grid")
        }
    }, [
        offshoreFacilitiesOperationsCostProfile,
        wellInterventionCostProfile,
        historicCostCostProfile,
        onshoreRelatedOPEXCostProfile,
        additionalOPEXCostProfile,
    ])

    const studyTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Feasibility & conceptual stud.",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalFeasibilityAndConceptStudies,
            overridable: true,
            overrideProfile: totalFeasibilityAndConceptStudiesOverride,
            overrideProfileSet: setTotalFeasibilityAndConceptStudiesOverride,
        },
        {
            profileName: "FEED studies (DG2-DG3)",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalFEEDStudies,
            overridable: true,
            overrideProfile: totalFEEDStudiesOverride,
            overrideProfileSet: setTotalFEEDStudiesOverride,
        },
        {
            profileName: "Other studies",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalOtherStudies,
            set: setTotalOtherStudies,
        },
    ]

    const opexTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Historic Cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: historicCostCostProfile,
            set: setHistoricCostCostProfile,
        },
        {
            profileName: "Well intervention",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: wellInterventionCostProfile,
            overridable: true,
            overrideProfile: wellInterventionCostProfileOverride,
            overrideProfileSet: setWellInterventionCostProfileOverride,
        },
        {
            profileName: "Offshore facilities operations",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: offshoreFacilitiesOperationsCostProfile,
            overridable: true,
            overrideProfile: offshoreFacilitiesOperationsCostProfileOverride,
            overrideProfileSet: setOffshoreFacilitiesOperationsCostProfileOverride,
        },
        {
            profileName: "Onshore related OPEX (input req.)",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: onshoreRelatedOPEXCostProfile,
            set: setOnshoreRelatedOPEXCostProfile,
        },
        {
            profileName: "Additional OPEX (input req.)",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: additionalOPEXCostProfile,
            set: setAdditionalOPEXCostProfile,
        },
    ]

    useEffect(() => {
        if (projectCaseEdited) {
            updateObject(projectCaseEdited, setProjectCaseEdited, "totalFeasibilityAndConceptStudiesOverride", totalFeasibilityAndConceptStudiesOverride)
        }
    }, [totalFeasibilityAndConceptStudiesOverride])

    useEffect(() => {
        if (projectCaseEdited) {
            updateObject(projectCaseEdited, setProjectCaseEdited, "totalFEEDStudiesOverride", totalFEEDStudiesOverride)
        }
    }, [totalFEEDStudiesOverride])

    useEffect(() => {
        if (projectCaseEdited) {
            updateObject(projectCaseEdited, setProjectCaseEdited, "totalOtherStudies", totalOtherStudies)
        }
    }, [totalOtherStudies])

    useEffect(() => {
        if (projectCaseEdited) {
            updateObject(projectCaseEdited, setProjectCaseEdited, "wellInterventionCostProfileOverride", wellInterventionCostProfileOverride)
        }
    }, [wellInterventionCostProfileOverride])

    useEffect(() => {
        if (projectCaseEdited) {
            updateObject(projectCaseEdited, setProjectCaseEdited, "offshoreFacilitiesOperationsCostProfileOverride", offshoreFacilitiesOperationsCostProfileOverride)
        }
    }, [offshoreFacilitiesOperationsCostProfileOverride])

    useEffect(() => {
        if (projectCaseEdited) {
            updateObject(projectCaseEdited, setProjectCaseEdited, "onshoreRelatedOPEXCostProfile", onshoreRelatedOPEXCostProfile)
        }
    }, [onshoreRelatedOPEXCostProfile])

    useEffect(() => {
        if (projectCaseEdited) {
            updateObject(projectCaseEdited, setProjectCaseEdited, "historicCostCostProfile", historicCostCostProfile)
        }
    }, [historicCostCostProfile])

    useEffect(() => {
        if (projectCaseEdited) {
            updateObject(projectCaseEdited, setProjectCaseEdited, "additionalOPEXCostProfile", additionalOPEXCostProfile)
        }
    }, [additionalOPEXCostProfile])

    useEffect(() => {
        if (surf) {
            updateObject(surf, setSurf, "costProfile", surfCost)
        }
    }, [surfCost])

    useEffect(() => {
        if (topside) {
            updateObject(topside, setTopside, "costProfile", topsideCost)
        }
    }, [topsideCost])

    useEffect(() => {
        if (substructure) {
            updateObject(substructure, setSubstructure, "costProfile", substructureCost)
        }
    }, [substructureCost])

    useEffect(() => {
        if (transport) {
            updateObject(transport, setTransport, "costProfile", transportCost)
        }
    }, [transportCost])

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
                <CaseTabTable
                    timeSeriesData={studyTimeSeriesData}
                    dg4Year={projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="Total study costs"
                    gridRef={studyGridRef}
                    alignedGridsRef={[opexGridRef, cessationGridRef, capexGridRef,
                        developmentWellsGridRef, explorationWellsGridRef]}
                    includeFooter
                    totalRowName="Total study costs"
                />
            </Grid>
            <Grid item xs={12}>
                <CaseTabTable
                    timeSeriesData={opexTimeSeriesData}
                    dg4Year={projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="OPEX"
                    gridRef={opexGridRef}
                    alignedGridsRef={[
                        studyGridRef,
                        cessationGridRef,
                        capexGridRef,
                        developmentWellsGridRef,
                        explorationWellsGridRef,
                    ]}
                    includeFooter
                    totalRowName="Total OPEX cost"
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
