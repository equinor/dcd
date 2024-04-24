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
        topsideCost, setTopsideCost,
        surf, setSurf,
        surfCost, setSurfCost,
        substructure, setSubstructure,
        substructureCost, setSubstructureCost,
        transport, setTransport,
        transportCost, setTransportCost,
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

        // Exploration
        explorationWellCostProfile,
        setExplorationWellCostProfile,
        gAndGAdminCost,
        setGAndGAdminCost,
        seismicAcquisitionAndProcessing,
        setSeismicAcquisitionAndProcessing,
        sidetrackCostProfile,
        setSidetrackCostProfile,
        appraisalWellCostProfile,
        setAppraisalWellCostProfile,
        countryOfficeCost,
        setCountryOfficeCost,
    } = useCaseContext()
    const {
        wellProject,
        setWellProject,
        exploration,
        setExploration,
    } = useModalContext()

    const { project } = useProjectContext()

    // OPEX
    const [offshoreFacilitiesOperationsCostProfileOverride, setOffshoreFacilitiesOperationsCostProfileOverride] = useState<Components.Schemas.OffshoreFacilitiesOperationsCostProfileOverrideDto>()
    const [wellInterventionCostProfileOverride, setWellInterventionCostProfileOverride] = useState<Components.Schemas.WellInterventionCostProfileOverrideDto>()

    // CAPEX
    const [topsideCostOverride, setTopsideCostOverride] = useState<Components.Schemas.TopsideCostProfileOverrideDto>()
    const [surfCostOverride, setSurfCostOverride] = useState<Components.Schemas.SurfCostProfileOverrideDto>()
    const [substructureCostOverride, setSubstructureCostOverride] = useState<Components.Schemas.SubstructureCostProfileOverrideDto>()
    const [transportCostOverride, setTransportCostOverride] = useState<Components.Schemas.TransportCostProfileOverrideDto>()

    // Development
    const [wellProjectOilProducerCost, setWellProjectOilProducerCost] = useState<Components.Schemas.OilProducerCostProfileDto>()
    const [wellProjectOilProducerCostOverride,
        setWellProjectOilProducerCostOverride] = useState<Components.Schemas.OilProducerCostProfileOverrideDto>()

    const [wellProjectGasProducerCost, setWellProjectGasProducerCost] = useState<Components.Schemas.GasProducerCostProfileDto>()
    const [wellProjectGasProducerCostOverride,
        setWellProjectGasProducerCostOverride] = useState<Components.Schemas.GasProducerCostProfileOverrideDto>()

    const [wellProjectWaterInjectorCost, setWellProjectWaterInjectorCost] = useState<Components.Schemas.WaterInjectorCostProfileDto>()
    const [wellProjectWaterInjectorCostOverride,
        setWellProjectWaterInjectorCostOverride] = useState<Components.Schemas.WaterInjectorCostProfileOverrideDto>()

    const [wellProjectGasInjectorCost, setWellProjectGasInjectorCost] = useState<Components.Schemas.GasInjectorCostProfileDto>()
    const [wellProjectGasInjectorCostOverride,
        setWellProjectGasInjectorCostOverride] = useState<Components.Schemas.GasInjectorCostProfileOverrideDto>()

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

                        setTopsideCost(topside.costProfile)
                        setTopsideCostOverride(topside.costProfileOverride)

                        setSurfCost(surf.costProfile)
                        setSurfCostOverride(surf.costProfileOverride)

                        setSubstructureCost(substructure.costProfile)
                        setSubstructureCostOverride(substructure.costProfileOverride)

                        setTransportCost(transport.costProfile)
                        setTransportCostOverride(transport.costProfileOverride)

                        setWellProjectOilProducerCost(wellProject.oilProducerCostProfile)
                        setWellProjectOilProducerCostOverride(wellProject.oilProducerCostProfileOverride)

                        setWellProjectGasProducerCost(wellProject.gasProducerCostProfile)
                        setWellProjectGasProducerCostOverride(wellProject.gasProducerCostProfileOverride)

                        setWellProjectWaterInjectorCost(wellProject.waterInjectorCostProfile)
                        setWellProjectWaterInjectorCostOverride(wellProject.waterInjectorCostProfileOverride)

                        setWellProjectGasInjectorCost(wellProject.gasInjectorCostProfile)
                        setWellProjectGasInjectorCostOverride(wellProject.gasInjectorCostProfileOverride)

                        setSeismicAcquisitionAndProcessing(exploration.seismicAcquisitionAndProcessing)
                        setExplorationWellCostProfile(exploration.explorationWellCostProfile)
                        setAppraisalWellCostProfile(exploration.appraisalWellCostProfile)
                        setSidetrackCostProfile(exploration.sidetrackCostProfile)
                        setCountryOfficeCost(exploration.countryOfficeCost)
                        setGAndGAdminCost(exploration.gAndGAdminCost)

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

    useEffect(() => {
        if (explorationWellsGridRef.current
            && explorationWellsGridRef.current.api
            && explorationWellsGridRef.current.api.refreshCells) {
            explorationWellsGridRef.current.api.refreshCells()

            console.log("Refreshing exploration wells grid")
        }
    }, [gAndGAdminCost])

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

    const capexTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Subsea production system",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: surfCost,
            overridable: true,
            overrideProfile: surfCostOverride,
            overrideProfileSet: setSurfCostOverride,
        },
        {
            profileName: "Topside",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: topsideCost,
            overridable: true,
            overrideProfile: topsideCostOverride,
            overrideProfileSet: setTopsideCostOverride,
        },
        {
            profileName: "Substructure",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: substructureCost,
            overridable: true,
            overrideProfile: substructureCostOverride,
            overrideProfileSet: setSubstructureCostOverride,
        },
        {
            profileName: "Transport system",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: transportCost,
            overridable: true,
            overrideProfile: transportCostOverride,
            overrideProfileSet: setTransportCostOverride,
        },
    ]

    const developmentTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Oil producer cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: wellProjectOilProducerCost,
            overridable: true,
            overrideProfile: wellProjectOilProducerCostOverride,
            overrideProfileSet: setWellProjectOilProducerCostOverride,
        },
        {
            profileName: "Gas producer cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: wellProjectGasProducerCost,
            overridable: true,
            overrideProfile: wellProjectGasProducerCostOverride,
            overrideProfileSet: setWellProjectGasProducerCostOverride,
        },
        {
            profileName: "Water injector cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: wellProjectWaterInjectorCost,
            overridable: true,
            overrideProfile: wellProjectWaterInjectorCostOverride,
            overrideProfileSet: setWellProjectWaterInjectorCostOverride,
        },
        {
            profileName: "Gas injector cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: wellProjectGasInjectorCost,
            overridable: true,
            overrideProfile: wellProjectGasInjectorCostOverride,
            overrideProfileSet: setWellProjectGasInjectorCostOverride,
        },
    ]

    const explorationTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "G&G and admin costs",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: gAndGAdminCost,
        },
        {
            profileName: "Seismic acquisition and processing",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: seismicAcquisitionAndProcessing,
            set: setSeismicAcquisitionAndProcessing,
        },
        {
            profileName: "Country office cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: countryOfficeCost,
            set: setCountryOfficeCost,
        },
        {
            profileName: "Exploration well cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: explorationWellCostProfile,
            set: setExplorationWellCostProfile,
        },
        {
            profileName: "Appraisal well cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: appraisalWellCostProfile,
            set: setAppraisalWellCostProfile,
        },
        {
            profileName: "Sidetrack well cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: sidetrackCostProfile,
            set: setSidetrackCostProfile,
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

    useEffect(() => {
        if (surf) {
            updateObject(surf, setSurf, "costProfileOverride", surfCostOverride)
        }
    }, [surfCostOverride])

    useEffect(() => {
        if (topside) {
            updateObject(topside, setTopside, "costProfileOverride", topsideCostOverride)
        }
    }, [topsideCostOverride])

    useEffect(() => {
        if (substructure) {
            updateObject(substructure, setSubstructure, "costProfileOverride", substructureCostOverride)
        }
    }, [substructureCostOverride])

    useEffect(() => {
        if (transport) {
            updateObject(transport, setTransport, "costProfileOverride", transportCostOverride)
        }
    }, [transportCostOverride])

    useEffect(() => {
        if (wellProject) {
            updateObject(wellProject, setWellProject, "oilProducerCostProfile", wellProjectOilProducerCost)
        }
    }, [wellProjectOilProducerCost])

    useEffect(() => {
        if (wellProject) {
            updateObject(wellProject, setWellProject, "oilProducerCostProfileOverride", wellProjectOilProducerCostOverride)
        }
    }, [wellProjectOilProducerCostOverride])

    useEffect(() => {
        if (wellProject) {
            updateObject(wellProject, setWellProject, "gasProducerCostProfile", wellProjectGasProducerCost)
        }
    }, [wellProjectGasProducerCost])

    useEffect(() => {
        if (wellProject) {
            updateObject(wellProject, setWellProject, "gasProducerCostProfileOverride", wellProjectGasProducerCostOverride)
        }
    }, [wellProjectGasProducerCostOverride])

    useEffect(() => {
        if (wellProject) {
            updateObject(wellProject, setWellProject, "waterInjectorCostProfile", wellProjectWaterInjectorCost)
        }
    }, [wellProjectWaterInjectorCost])

    useEffect(() => {
        if (wellProject) {
            updateObject(wellProject, setWellProject, "waterInjectorCostProfileOverride", wellProjectWaterInjectorCostOverride)
        }
    }, [wellProjectWaterInjectorCostOverride])

    useEffect(() => {
        if (wellProject) {
            updateObject(wellProject, setWellProject, "gasInjectorCostProfile", wellProjectGasInjectorCost)
        }
    }, [wellProjectGasInjectorCost])

    useEffect(() => {
        if (wellProject) {
            updateObject(wellProject, setWellProject, "gasInjectorCostProfileOverride", wellProjectGasInjectorCostOverride)
        }
    }, [wellProjectGasInjectorCostOverride])

    useEffect(() => {
        if (exploration) {
            updateObject(exploration, setExploration, "explorationWellCostProfile", explorationWellCostProfile)
        }
    }, [explorationWellCostProfile])

    useEffect(() => {
        if (exploration) {
            updateObject(exploration, setExploration, "appraisalWellCostProfile", appraisalWellCostProfile)
        }
    }, [appraisalWellCostProfile])

    useEffect(() => {
        if (exploration) {
            updateObject(exploration, setExploration, "sidetrackCostProfile", sidetrackCostProfile)
        }
    }, [sidetrackCostProfile])

    useEffect(() => {
        if (exploration) {
            updateObject(exploration, setExploration, "seismicAcquisitionAndProcessing", seismicAcquisitionAndProcessing)
        }
    }, [seismicAcquisitionAndProcessing])

    useEffect(() => {
        if (exploration) {
            updateObject(exploration, setExploration, "countryOfficeCost", countryOfficeCost)
        }
    }, [countryOfficeCost])

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
                    alignedGridsRef={[studyGridRef, cessationGridRef, capexGridRef,
                        developmentWellsGridRef, explorationWellsGridRef]}
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
                <CaseTabTable
                    timeSeriesData={capexTimeSeriesData}
                    dg4Year={projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="Offshore facilitiy costs"
                    gridRef={capexGridRef}
                    alignedGridsRef={[studyGridRef, opexGridRef, cessationGridRef,
                        developmentWellsGridRef, explorationWellsGridRef]}
                    includeFooter
                    totalRowName="Total offshore facility cost"
                />
            </Grid>
            <Grid item xs={12}>
                <CaseTabTable
                    timeSeriesData={developmentTimeSeriesData}
                    dg4Year={projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="Development well costs"
                    gridRef={developmentWellsGridRef}
                    alignedGridsRef={[studyGridRef, opexGridRef, cessationGridRef, capexGridRef,
                        explorationWellsGridRef]}
                    includeFooter
                    totalRowName="Total development well cost"
                />
            </Grid>
            <Grid item xs={12}>
                <CaseTabTable
                    timeSeriesData={explorationTimeSeriesData}
                    dg4Year={projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="Exploration well costs"
                    gridRef={explorationWellsGridRef}
                    alignedGridsRef={[studyGridRef, opexGridRef, cessationGridRef, capexGridRef,
                        developmentWellsGridRef]}
                    includeFooter
                    totalRowName="Total exploration cost"
                />
            </Grid>
        </Grid>
    )
}

export default CaseCostTab
