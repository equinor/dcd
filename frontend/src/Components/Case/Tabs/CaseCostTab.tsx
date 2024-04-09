import {
    ChangeEventHandler,
    Dispatch,
    SetStateAction,
    useEffect,
    useRef,
    useState,
} from "react"
import { NativeSelect, Typography } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import CaseNumberInput from "../../Input/CaseNumberInput"
import CaseTabTable from "../Components/CaseTabTable"
import { SetTableYearsFromProfiles } from "../Components/CaseTabTableHelper"
import { ITimeSeries } from "../../../Models/ITimeSeries"
import { ITimeSeriesCostOverride } from "../../../Models/ITimeSeriesCostOverride"
import { ITimeSeriesCost } from "../../../Models/ITimeSeriesCost"
import InputSwitcher from "../../Input/InputSwitcher"
import { useProjectContext } from "../../../Context/ProjectContext"
import { useCaseContext } from "../../../Context/CaseContext"
import { useModalContext } from "../../../Context/ModalContext"
import RangeButton from "../../Buttons/RangeButton"

const CaseCostTab = (): React.ReactElement | null => {
    const {
        projectCase, setProjectCase, setProjectCaseEdited, activeTabCase,
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

        // CAPEX
        cessationWellsCost,
        setCessationWellsCost,
        cessationOffshoreFacilitiesCost,
        setCessationOffshoreFacilitiesCost,
        cessationOnshoreFacilitiesCost,
        setCessationOnshoreFacilitiesCost,

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
        explorationSidetrackCost,
        setExplorationSidetrackCost,
        explorationAppraisalWellCost,
        setExplorationAppraisalWellCost,
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

    const [cessationWellsCostOverride, setCessationWellsCostOverride] = useState<Components.Schemas.CessationWellsCostOverrideDto>()
    const [cessationOffshoreFacilitiesCostOverride, setCessationOffshoreFacilitiesCostOverride] = useState<Components.Schemas.CessationOffshoreFacilitiesCostOverrideDto>()

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
                        const totalFeasibility = projectCase?.totalFeasibilityAndConceptStudies
                        const totalFEED = projectCase?.totalFEEDStudies
                        const totalOtherStudiesLocal = projectCase.totalOtherStudies

                        setTotalFeasibilityAndConceptStudies(totalFeasibility)
                        setTotalFeasibilityAndConceptStudiesOverride(projectCase?.totalFeasibilityAndConceptStudiesOverride)
                        setTotalFEEDStudies(totalFEED)
                        setTotalFEEDStudiesOverride(projectCase?.totalFEEDStudiesOverride)

                        setTotalOtherStudies(totalOtherStudiesLocal)

                        const wellIntervention = wellInterventionCostProfile
                        const offshoreFacilitiesOperations = projectCase?.offshoreFacilitiesOperationsCostProfile
                        const historicCostCostProfileLocal = projectCase?.historicCostCostProfile
                        const additionalOPEXCostProfileLocal = projectCase?.additionalOPEXCostProfile

                        setWellInterventionCostProfile(wellIntervention)
                        setWellInterventionCostProfileOverride(projectCase?.wellInterventionCostProfileOverride)
                        setOffshoreFacilitiesOperationsCostProfile(offshoreFacilitiesOperations)
                        setOffshoreFacilitiesOperationsCostProfileOverride(projectCase?.offshoreFacilitiesOperationsCostProfileOverride)
                        setHistoricCostCostProfile(historicCostCostProfileLocal)
                        setAdditionalOPEXCostProfile(additionalOPEXCostProfileLocal)

                        // Development
                        const {
                            cessationWellsCost,
                            cessationOffshoreFacilities,
                            cessationOnshoreFacilities,
                        } = projectCase

                        const cessationWells = projectCase?.cessationWellsCost
                        const cessationOffshoreFacilities = projectCase?.cessationOffshoreFacilitiesCost
                        const cessationOnshoreFacilities = projectCase?.cessationOnshoreFacilitiesCost

                        setCessationWellsCost(cessationWellsCost)
                        setCessationWellsCostOverride(projectCase?.cessationWellsCostOverride)
                        setCessationOffshoreFacilitiesCost(cessationOffshoreFacilities)
                        setCessationOffshoreFacilitiesCostOverride(projectCase?.cessationOffshoreFacilitiesCostOverride)
                        setCessationOnshoreFacilitiesCost(cessationOnshoreFacilities)

                        // CAPEX
                        const topsideCostProfile = topside.costProfile
                        setTopsideCost(topsideCostProfile)
                        const topsideCostProfileOverride = topside.costProfileOverride
                        setTopsideCostOverride(topsideCostProfileOverride)

                        const surfCostProfile = surf.costProfile
                        setSurfCost(surfCostProfile)
                        const surfCostProfileOverride = surf.costProfileOverride
                        setSurfCostOverride(surfCostProfileOverride)

                        const substructureCostProfile = substructure.costProfile
                        setSubstructureCost(substructureCostProfile)
                        const substructureCostProfileOverride = substructure.costProfileOverride
                        setSubstructureCostOverride(substructureCostProfileOverride)

                        const transportCostProfile = transport.costProfile
                        setTransportCost(transportCostProfile)
                        const transportCostProfileOverride = transport.costProfileOverride
                        setTransportCostOverride(transportCostProfileOverride)

                        // Development
                        const {
                            oilProducerCostProfile,
                            gasProducerCostProfile,
                            waterInjectorCostProfile,
                            gasInjectorCostProfile,
                            oilProducerCostProfileOverride,
                            gasProducerCostProfileOverride,
                            waterInjectorCostProfileOverride,
                            gasInjectorCostProfileOverride,
                        } = wellProject

                        setWellProjectOilProducerCost(oilProducerCostProfile)
                        setWellProjectOilProducerCostOverride(oilProducerCostProfileOverride)
                        setWellProjectGasProducerCost(gasProducerCostProfile)
                        setWellProjectGasProducerCostOverride(gasProducerCostProfileOverride)
                        setWellProjectWaterInjectorCost(waterInjectorCostProfile)
                        setWellProjectWaterInjectorCostOverride(waterInjectorCostProfileOverride)
                        setWellProjectGasInjectorCost(gasInjectorCostProfile)
                        setWellProjectGasInjectorCostOverride(gasInjectorCostProfileOverride)

                        // Exploration

                        setSeismicAcquisitionAndProcessing(exploration.seismicAcquisitionAndProcessing)
                        setExplorationWellCostProfile(exploration.explorationWellCostProfile)
                        setExplorationAppraisalWellCost(exploration.appraisalWellCostProfile)
                        setExplorationSidetrackCost(exploration.sidetrackCostProfile)
                        const countryOffice = exploration.countryOfficeCost
                        setCountryOfficeCost(countryOffice)

                        setGAndGAdminCost(exploration.gAndGAdminCost)

                        SetTableYearsFromProfiles([
                            projectCase?.totalFeasibilityAndConceptStudies,
                            projectCase?.totalFEEDStudies,
                            projectCase?.wellInterventionCostProfile,
                            projectCase?.offshoreFacilitiesOperationsCostProfile,
                            projectCase?.cessationWellsCost,
                            projectCase?.cessationOffshoreFacilitiesCost,
                            projectCase?.cessationOnshoreFacilitiesCost,
                            projectCase?.totalFeasibilityAndConceptStudiesOverride,
                            projectCase?.totalFEEDStudiesOverride,
                            projectCase?.wellInterventionCostProfileOverride,
                            projectCase?.offshoreFacilitiesOperationsCostProfileOverride,
                            projectCase?.cessationWellsCostOverride,
                            projectCase?.cessationOffshoreFacilitiesCostOverride,
                            surfCostProfile,
                            topsideCostProfile,
                            substructureCostProfile,
                            transportCostProfile,
                            surfCostOverride,
                            topsideCostOverride,
                            substructureCostOverride,
                            transportCostOverride,
                            oilProducerCostProfile,
                            gasProducerCostProfile,
                            waterInjectorCostProfile,
                            gasInjectorCostProfile,
                            oilProducerCostProfileOverride,
                            gasProducerCostProfileOverride,
                            waterInjectorCostProfileOverride,
                            gasInjectorCostProfileOverride,
                            explorationWellCostProfile,
                            seismicAcquisitionAndProcessing,
                            countryOffice,
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
        }
    }, [totalFeasibilityAndConceptStudies, totalFEEDStudies, totalOtherStudies])

    useEffect(() => {
        if (opexGridRef.current
            && opexGridRef.current.api
            && opexGridRef.current.api.refreshCells) {
            opexGridRef.current.api.refreshCells()
        }
    }, [
        offshoreFacilitiesOperationsCostProfile,
        wellInterventionCostProfile,
        historicCostCostProfile,
        onshoreRelatedOPEXCostProfile,
        additionalOPEXCostProfile,
    ])

    useEffect(() => {
        if (cessationGridRef.current
            && cessationGridRef.current.api
            && cessationGridRef.current.api.refreshCells) {
            cessationGridRef.current.api.refreshCells()
        }
    }, [cessationWellsCost, cessationOffshoreFacilitiesCost, cessationOnshoreFacilitiesCost])

    useEffect(() => {
        if (explorationWellsGridRef.current
            && explorationWellsGridRef.current.api
            && explorationWellsGridRef.current.api.refreshCells) {
            explorationWellsGridRef.current.api.refreshCells()
        }
    }, [gAndGAdminCost])

    const updatedAndSetSurf = (surfItem: Components.Schemas.SurfDto) => {
        const newSurf: Components.Schemas.SurfDto = { ...surfItem }
        if (surfCost) {
            newSurf.costProfile = surfCost
        }
        setSurf(newSurf)
    }

    const handleCaseFeasibilityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...projectCase }
        const newCapexFactorFeasibilityStudies = e.currentTarget.value.length > 0
            ? Math.min(Math.max(Number(e.currentTarget.value), 0), 100) : undefined
        if (newCapexFactorFeasibilityStudies !== undefined) {
            newCase.capexFactorFeasibilityStudies = newCapexFactorFeasibilityStudies / 100
        } else { newCase.capexFactorFeasibilityStudies = 0 }
        console.log("handling case feasibility change", newCase.capexFactorFeasibilityStudies)
        setProjectCaseEdited(newCase as Components.Schemas.CaseDto)
    }

    const handleCaseFEEDChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...projectCase }
        const newCapexFactorFEEDStudies = e.currentTarget.value.length > 0
            ? Math.min(Math.max(Number(e.currentTarget.value), 0), 100) : undefined
        if (newCapexFactorFEEDStudies !== undefined) {
            newCase.capexFactorFEEDStudies = newCapexFactorFEEDStudies / 100
        } else { newCase.capexFactorFEEDStudies = 0 }
        console.log("handling case FEED change", newCase.capexFactorFEEDStudies)
        setProjectCaseEdited(newCase as Components.Schemas.CaseDto)
    }

    const handleSurfMaturityChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3].indexOf(Number(e.currentTarget.value)) !== -1) {
            const newMaturity: Components.Schemas.Maturity = Number(e.currentTarget.value) as Components.Schemas.Maturity
            const newSurf = { ...surf }
            newSurf.maturity = newMaturity
            updatedAndSetSurf(newSurf as Components.Schemas.SurfDto)
        }
    }

    const handleStartYearChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newStartYear = Number(e.currentTarget.value)
        if (newStartYear < 2010) {
            setStartYear(2010)
            return
        }
        setStartYear(newStartYear)
    }

    const handleEndYearChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newEndYear = Number(e.currentTarget.value)
        if (newEndYear > 2100) {
            setEndYear(2100)
            return
        }
        setEndYear(newEndYear)
    }

    interface ITimeSeriesData {
        profileName: string
        unit: string,
        set?: Dispatch<SetStateAction<ITimeSeriesCost | undefined>>,
        overrideProfileSet?: Dispatch<SetStateAction<ITimeSeriesCostOverride | undefined>>,
        profile: ITimeSeries | undefined
        overrideProfile?: ITimeSeries | undefined
        overridable?: boolean
    }

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

    const cessationTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Cessation - Development wells",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationWellsCost,
            overridable: true,
            overrideProfile: cessationWellsCostOverride,
            overrideProfileSet: setCessationWellsCostOverride,
        },
        {
            profileName: "Cessation - Offshore facilities",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationOffshoreFacilitiesCost,
            overridable: true,
            overrideProfile: cessationOffshoreFacilitiesCostOverride,
            overrideProfileSet: setCessationOffshoreFacilitiesCostOverride,
        },
        {
            profileName: "CAPEX - Cessation - Onshore facilities",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationOnshoreFacilitiesCost,
            set: setCessationOnshoreFacilitiesCost,
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
            profile: explorationAppraisalWellCost,
            set: setExplorationAppraisalWellCost,
        },
        {
            profileName: "Sidetrack well cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: explorationSidetrackCost,
            set: setExplorationSidetrackCost,
        },
    ]

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    function updateObject<T>(object: T | undefined, setObject: Dispatch<SetStateAction<T | undefined>>, key: keyof T, value: any): void {
        if (!object || !value) {
            console.error("Object or value is undefined")
            return
        }
        if (object[key] === value) {
            console.error("Object key is already set to value")
            return
        }
        const newObject: T = { ...object }
        newObject[key] = value
        setObject(newObject)
    }

    useEffect(() => {
        if (projectCase) {
            updateObject(projectCase, setProjectCase, "totalFeasibilityAndConceptStudiesOverride", totalFeasibilityAndConceptStudiesOverride)
        }
    }, [totalFeasibilityAndConceptStudiesOverride])

    useEffect(() => {
        if (projectCase) {
            updateObject(projectCase, setProjectCase, "totalFEEDStudiesOverride", totalFEEDStudiesOverride)
        }
    }, [totalFEEDStudiesOverride])

    useEffect(() => {
        if (projectCase) {
            updateObject(projectCase, setProjectCase, "totalOtherStudies", totalOtherStudies)
        }
    }, [totalOtherStudies])

    useEffect(() => {
        if (projectCase) {
            updateObject(projectCase, setProjectCase, "wellInterventionCostProfileOverride", wellInterventionCostProfileOverride)
        }
    }, [wellInterventionCostProfileOverride])

    useEffect(() => {
        if (projectCase) {
            updateObject(projectCase, setProjectCase, "offshoreFacilitiesOperationsCostProfileOverride", offshoreFacilitiesOperationsCostProfileOverride)
        }
    }, [offshoreFacilitiesOperationsCostProfileOverride])

    useEffect(() => {
        if (projectCase) {
            updateObject(projectCase, setProjectCase, "historicCostCostProfile", historicCostCostProfile)
        }
    }, [historicCostCostProfile])

    useEffect(() => {
        if (projectCase) {
            updateObject(projectCase, setProjectCase, "additionalOPEXCostProfile", additionalOPEXCostProfile)
        }
    }, [additionalOPEXCostProfile])

    useEffect(() => {
        if (projectCase) {
            updateObject(projectCase, setProjectCase, "cessationWellsCostOverride", cessationWellsCostOverride)
        }
    }, [cessationWellsCostOverride])

    useEffect(() => {
        if (projectCase) {
            updateObject(projectCase, setProjectCase, "cessationOffshoreFacilitiesCostOverride", cessationOffshoreFacilitiesCostOverride)
        }
    }, [cessationOffshoreFacilitiesCostOverride])

    useEffect(() => {
        if (projectCase) {
            updateObject(projectCase, setProjectCase, "cessationOnshoreFacilitiesCost", cessationOnshoreFacilitiesCost)
        }
    }, [cessationOnshoreFacilitiesCost])

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
            updateObject(exploration, setExploration, "appraisalWellCostProfile", explorationAppraisalWellCost)
        }
    }, [explorationAppraisalWellCost])

    useEffect(() => {
        if (exploration) {
            updateObject(exploration, setExploration, "sidetrackCostProfile", explorationSidetrackCost)
        }
    }, [explorationSidetrackCost])

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

    const maturityOptions: { [key: string]: string } = {
        0: "A",
        1: "B",
        2: "C",
        3: "D",
    }
    return (
        <Grid container spacing={3}>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={`${projectCase?.capexFactorFeasibilityStudies !== undefined ? ((projectCase.capexFactorFeasibilityStudies ?? 0) * 100).toFixed(2) : ""}%`}
                    label="CAPEX factor feasibility studies"
                >
                    <CaseNumberInput
                        onChange={handleCaseFeasibilityChange}
                        defaultValue={projectCase?.capexFactorFeasibilityStudies !== undefined ? (projectCase.capexFactorFeasibilityStudies ?? 0) * 100 : undefined}
                        integer={false}
                        unit="%"
                        min={0}
                        max={100}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher
                    value={`${projectCase?.capexFactorFEEDStudies !== undefined ? ((projectCase.capexFactorFEEDStudies ?? 0) * 100).toFixed(2) : ""}%`}
                    label="CAPEX factor FEED studies"
                >
                    <CaseNumberInput
                        onChange={handleCaseFEEDChange}
                        defaultValue={projectCase?.capexFactorFEEDStudies !== undefined ? (projectCase.capexFactorFEEDStudies ?? 0) * 100 : undefined}
                        integer={false}
                        unit="%"
                        min={0}
                        max={100}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={4}>
                <InputSwitcher value={maturityOptions[surf?.maturity ?? "defaultKey"]} label="Maturity">
                    <NativeSelect
                        id="maturity"
                        label=""
                        onChange={handleSurfMaturityChange}
                        value={surf?.maturity ?? ""}
                    >
                        {Object.keys(maturityOptions).map((key) => (
                            <option key={key} value={key}>{maturityOptions[key]}</option>
                        ))}
                    </NativeSelect>
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} container spacing={1} justifyContent="flex-end" alignItems="baseline" marginTop={6}>
                <Grid item>
                    <NativeSelect
                        id="currency"
                        label="Currency"
                        onChange={() => { }}
                        value={project?.currency}
                        disabled
                    >
                        <option key="1" value={1}>MNOK</option>
                        <option key="2" value={2}>MUSD</option>
                    </NativeSelect>
                </Grid>
                <Grid item>
                    <Typography variant="caption">Start year</Typography>
                    <CaseNumberInput
                        onChange={handleStartYearChange}
                        defaultValue={startYear}
                        integer
                        min={2010}
                        max={2100}
                    />
                </Grid>
                <Grid item>
                    <Typography variant="caption">End year</Typography>
                    <CaseNumberInput
                        onChange={handleEndYearChange}
                        defaultValue={endYear}
                        integer
                        min={2010}
                        max={2100}
                    />
                </Grid>
                <Grid item>
                    <RangeButton onClick={handleTableYearsClick} />
                </Grid>
            </Grid>
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
                <CaseTabTable
                    timeSeriesData={cessationTimeSeriesData}
                    dg4Year={projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="Cessation costs"
                    gridRef={cessationGridRef}
                    alignedGridsRef={[studyGridRef, opexGridRef, capexGridRef,
                        developmentWellsGridRef, explorationWellsGridRef]}
                    includeFooter
                    totalRowName="Total cessation cost"
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
