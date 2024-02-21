import {
    ChangeEventHandler, Dispatch, SetStateAction, useEffect, useRef, useState,
} from "react"
import styled from "styled-components"

import {
    Button, NativeSelect, Typography,
} from "@equinor/eds-core-react"
import CaseNumberInput from "./CaseNumberInput"
import CaseTabTable from "./CaseTabTable"
import { SetTableYearsFromProfiles } from "./CaseTabTableHelper"
import { ITimeSeries } from "../../Models/ITimeSeries"
import { ITimeSeriesCostOverride } from "../../Models/ITimeSeriesCostOverride"
import { ITimeSeriesCost } from "../../Models/ITimeSeriesCost"
import FilterContainer from "../Input/FilterContainer"
import InputContainer from "../Input/InputContainer"

const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    margin-top: 20px;
    margin-bottom: 20px;
`
const PageTitle = styled(Typography)`
    flex-grow: 1;
`

const TableWrapper = styled.div`
    margin-bottom: 50px;
`

interface Props {
    project: Components.Schemas.ProjectDto,
    caseItem: Components.Schemas.CaseDto,
    setCase: Dispatch<SetStateAction<Components.Schemas.CaseDto | undefined>>,
    topside: Components.Schemas.TopsideDto,
    setTopside: Dispatch<SetStateAction<Components.Schemas.TopsideDto | undefined>>,
    surf: Components.Schemas.SurfDto,
    setSurf: Dispatch<SetStateAction<Components.Schemas.SurfDto | undefined>>,
    substructure: Components.Schemas.SubstructureDto,
    setSubstructure: Dispatch<SetStateAction<Components.Schemas.SubstructureDto | undefined>>,
    transport: Components.Schemas.TransportDto,
    setTransport: Dispatch<SetStateAction<Components.Schemas.TransportDto | undefined>>,
    exploration: Components.Schemas.ExplorationDto,
    setExploration: Dispatch<SetStateAction<Components.Schemas.ExplorationDto | undefined>>,
    wellProject: Components.Schemas.WellProjectDto,
    setWellProject: Dispatch<SetStateAction<Components.Schemas.WellProjectDto | undefined>>,
    activeTab: number

    totalFeasibilityAndConceptStudies: Components.Schemas.TotalFeasibilityAndConceptStudiesDto | undefined,
    setTotalFeasibilityAndConceptStudies: Dispatch<SetStateAction<Components.Schemas.TotalFeasibilityAndConceptStudiesDto | undefined>>,
    totalFEEDStudies: Components.Schemas.TotalFEEDStudiesDto | undefined,
    setTotalFEEDStudies: Dispatch<SetStateAction<Components.Schemas.TotalFEEDStudiesDto | undefined>>,

    offshoreFacilitiesOperationsCostProfile: Components.Schemas.OffshoreFacilitiesOperationsCostProfileDto | undefined,
    setOffshoreFacilitiesOperationsCostProfile: Dispatch<SetStateAction<Components.Schemas.OffshoreFacilitiesOperationsCostProfileDto | undefined>>,

    wellInterventionCostProfile: Components.Schemas.WellInterventionCostProfileDto | undefined,
    setWellInterventionCostProfile: Dispatch<SetStateAction<Components.Schemas.WellInterventionCostProfileDto | undefined>>,

    cessationWellsCost: Components.Schemas.TotalFEEDStudiesDto | undefined,
    setCessationWellsCost: Dispatch<SetStateAction<Components.Schemas.CessationWellsCostDto | undefined>>,

    cessationOffshoreFacilitiesCost: Components.Schemas.CessationOffshoreFacilitiesCostDto | undefined,
    setCessationOffshoreFacilitiesCost: Dispatch<SetStateAction<Components.Schemas.CessationOffshoreFacilitiesCostDto | undefined>>,

    gAndGAdminCost: Components.Schemas.GAndGAdminCostDto | undefined,
    setGAndGAdminCost: Dispatch<SetStateAction<Components.Schemas.GAndGAdminCostDto | undefined>>,
}

const CaseCostTab = ({
    project,
    caseItem, setCase,
    exploration, setExploration,
    wellProject, setWellProject,
    topside, setTopside,
    surf, setSurf,
    substructure, setSubstructure,
    transport, setTransport,
    activeTab,
    totalFeasibilityAndConceptStudies, setTotalFeasibilityAndConceptStudies,
    totalFEEDStudies, setTotalFEEDStudies,
    offshoreFacilitiesOperationsCostProfile, setOffshoreFacilitiesOperationsCostProfile,
    wellInterventionCostProfile, setWellInterventionCostProfile,
    cessationWellsCost, setCessationWellsCost,
    cessationOffshoreFacilitiesCost, setCessationOffshoreFacilitiesCost,
    gAndGAdminCost, setGAndGAdminCost,
}: Props) => {
    // OPEX
    const [totalFeasibilityAndConceptStudiesOverride,
        setTotalFeasibilityAndConceptStudiesOverride] = useState<Components.Schemas.TotalFeasibilityAndConceptStudiesOverrideDto>()

    const [totalFEEDStudiesOverride, setTotalFEEDStudiesOverride] = useState<Components.Schemas.TotalFEEDStudiesOverrideDto>()

    const [offshoreFacilitiesOperationsCostProfileOverride,
        setOffshoreFacilitiesOperationsCostProfileOverride] = useState<Components.Schemas.OffshoreFacilitiesOperationsCostProfileOverrideDto>()

    const [wellInterventionCostProfileOverride, setWellInterventionCostProfileOverride] = useState<Components.Schemas.WellInterventionCostProfileOverrideDto>()

    const [cessationWellsCostOverride, setCessationWellsCostOverride] = useState<Components.Schemas.CessationWellsCostOverrideDto>()
    const [cessationOffshoreFacilitiesCostOverride,
        setCessationOffshoreFacilitiesCostOverride] = useState<Components.Schemas.CessationOffshoreFacilitiesCostOverrideDto>()

    // CAPEX
    const [topsideCost, setTopsideCost] = useState<Components.Schemas.TopsideCostProfileDto>()
    const [topsideCostOverride, setTopsideCostOverride] = useState<Components.Schemas.TopsideCostProfileOverrideDto>()
    const [surfCost, setSurfCost] = useState<Components.Schemas.SurfCostProfileDto>()
    const [surfCostOverride, setSurfCostOverride] = useState<Components.Schemas.SurfCostProfileOverrideDto>()
    const [substructureCost, setSubstructureCost] = useState<Components.Schemas.SubstructureCostProfileDto>()
    const [substructureCostOverride, setSubstructureCostOverride] = useState<Components.Schemas.SubstructureCostProfileOverrideDto>()
    const [transportCost, setTransportCost] = useState<Components.Schemas.TransportCostProfileDto>()
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
    const [explorationWellCost, setExplorationWellCost] = useState<Components.Schemas.ExplorationWellCostProfileDto>()
    const [explorationAppraisalWellCost, setExplorationAppraisalWellCost] = useState<Components.Schemas.AppraisalWellCostProfileDto>()
    const [explorationSidetrackCost, setExplorationSidetrackCost] = useState<Components.Schemas.SidetrackCostProfileDto>()
    const [seismicAcqAndProcCost, setSeismicAcqAndProcCost] = useState<Components.Schemas.SeismicAcquisitionAndProcessingDto>()
    const [countryOfficeCost, setCountryOfficeCost] = useState<Components.Schemas.CountryOfficeCostDto>()

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
                if (activeTab === 5) {
                    const totalFeasibility = caseItem.totalFeasibilityAndConceptStudies
                    const totalFEED = caseItem.totalFEEDStudies

                    setTotalFeasibilityAndConceptStudies(totalFeasibility)
                    setTotalFeasibilityAndConceptStudiesOverride(caseItem.totalFeasibilityAndConceptStudiesOverride)
                    setTotalFEEDStudies(totalFEED)
                    setTotalFEEDStudiesOverride(caseItem.totalFEEDStudiesOverride)

                    const wellIntervention = wellInterventionCostProfile
                    const offshoreFacilitiesOperations = caseItem.offshoreFacilitiesOperationsCostProfile

                    setWellInterventionCostProfile(wellIntervention)
                    setWellInterventionCostProfileOverride(caseItem.wellInterventionCostProfileOverride)
                    setOffshoreFacilitiesOperationsCostProfile(offshoreFacilitiesOperations)
                    setOffshoreFacilitiesOperationsCostProfileOverride(caseItem.offshoreFacilitiesOperationsCostProfileOverride)

                    const cessationWells = caseItem.cessationWellsCost
                    const cessationOffshoreFacilities = caseItem.cessationOffshoreFacilitiesCost

                    setCessationWellsCost(cessationWells)
                    setCessationWellsCostOverride(caseItem.cessationWellsCostOverride)
                    setCessationOffshoreFacilitiesCost(cessationOffshoreFacilities)
                    setCessationOffshoreFacilitiesCostOverride(caseItem.cessationOffshoreFacilitiesCostOverride)

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
                        oilProducerCostProfile, gasProducerCostProfile,
                        waterInjectorCostProfile, gasInjectorCostProfile,
                        oilProducerCostProfileOverride, gasProducerCostProfileOverride,
                        waterInjectorCostProfileOverride, gasInjectorCostProfileOverride,
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
                    const {
                        explorationWellCostProfile, appraisalWellCostProfile, sidetrackCostProfile,
                        seismicAcquisitionAndProcessing,
                    } = exploration
                    setExplorationWellCost(explorationWellCostProfile)
                    setExplorationAppraisalWellCost(appraisalWellCostProfile)
                    setExplorationSidetrackCost(sidetrackCostProfile)
                    setSeismicAcqAndProcCost(seismicAcquisitionAndProcessing)
                    const countryOffice = exploration.countryOfficeCost
                    setCountryOfficeCost(countryOffice)

                    setGAndGAdminCost(exploration.gAndGAdminCost)

                    SetTableYearsFromProfiles([caseItem.totalFeasibilityAndConceptStudies, caseItem.totalFEEDStudies,
                    caseItem.wellInterventionCostProfile, caseItem.offshoreFacilitiesOperationsCostProfile,
                    caseItem.cessationWellsCost, caseItem.cessationOffshoreFacilitiesCost,
                    caseItem.totalFeasibilityAndConceptStudiesOverride, caseItem.totalFEEDStudiesOverride,
                    caseItem.wellInterventionCostProfileOverride, caseItem.offshoreFacilitiesOperationsCostProfileOverride,
                    caseItem.cessationWellsCostOverride, caseItem.cessationOffshoreFacilitiesCostOverride,
                        surfCostProfile, topsideCostProfile, substructureCostProfile, transportCostProfile,
                        surfCostOverride, topsideCostOverride, substructureCostOverride, transportCostOverride,
                        oilProducerCostProfile, gasProducerCostProfile,
                        waterInjectorCostProfile, gasInjectorCostProfile,
                        oilProducerCostProfileOverride, gasProducerCostProfileOverride,
                        waterInjectorCostProfileOverride, gasInjectorCostProfileOverride,
                        explorationWellCostProfile, appraisalWellCostProfile, sidetrackCostProfile,
                        seismicAcquisitionAndProcessing, countryOffice, exploration.gAndGAdminCost,
                    ], caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030, setStartYear, setEndYear, setTableYears)
                    console.log(project);
                    console.log(caseItem);
                    console.log(topside);
                    console.log(surf);
                    console.log(substructure);
                    console.log(transport);
                    console.log(activeTab);
                }
            } catch (error) {
                console.error("[CaseView] Error while generating cost profile", error)
            }
        })()
    }, [activeTab])

    useEffect(() => {
        const {
            explorationWellCostProfile, appraisalWellCostProfile, sidetrackCostProfile,
            seismicAcquisitionAndProcessing,
        } = exploration
        setExplorationWellCost(explorationWellCostProfile)
        setExplorationAppraisalWellCost(appraisalWellCostProfile)
        setExplorationSidetrackCost(sidetrackCostProfile)
        setSeismicAcqAndProcCost(seismicAcquisitionAndProcessing)
        const countryOffice = exploration.countryOfficeCost
        setCountryOfficeCost(countryOffice)
    }, [exploration])

    useEffect(() => {
        if (studyGridRef.current && studyGridRef.current.api && studyGridRef.current.api.refreshCells) {
            studyGridRef.current.api.refreshCells()
        }
    }, [totalFeasibilityAndConceptStudies, totalFEEDStudies])

    useEffect(() => {
        if (opexGridRef.current && opexGridRef.current.api && opexGridRef.current.api.refreshCells) {
            opexGridRef.current.api.refreshCells()
        }
    }, [offshoreFacilitiesOperationsCostProfile, wellInterventionCostProfile])

    useEffect(() => {
        if (cessationGridRef.current && cessationGridRef.current.api && cessationGridRef.current.api.refreshCells) {
            cessationGridRef.current.api.refreshCells()
        }
    }, [cessationWellsCost, cessationOffshoreFacilitiesCost])

    useEffect(() => {
        if (explorationWellsGridRef.current && explorationWellsGridRef.current.api && explorationWellsGridRef.current.api.refreshCells) {
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
        const newCase = { ...caseItem }
        const newCapexFactorFeasibilityStudies = e.currentTarget.value.length > 0
            ? Math.min(Math.max(Number(e.currentTarget.value), 0), 100) : undefined
        if (newCapexFactorFeasibilityStudies !== undefined) {
            newCase.capexFactorFeasibilityStudies = newCapexFactorFeasibilityStudies / 100
        } else { newCase.capexFactorFeasibilityStudies = 0 }
        setCase(newCase)
    }

    const handleCaseFEEDChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newCapexFactorFEEDStudies = e.currentTarget.value.length > 0
            ? Math.min(Math.max(Number(e.currentTarget.value), 0), 100) : undefined
        if (newCapexFactorFEEDStudies !== undefined) {
            newCase.capexFactorFEEDStudies = newCapexFactorFEEDStudies / 100
        } else { newCase.capexFactorFEEDStudies = 0 }
        setCase(newCase)
    }

    const handleSurfMaturityChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3].indexOf(Number(e.currentTarget.value)) !== -1) {
            const newMaturity: Components.Schemas.Maturity = Number(e.currentTarget.value) as Components.Schemas.Maturity
            const newSurf = { ...surf }
            newSurf.maturity = newMaturity
            updatedAndSetSurf(newSurf)
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
    ]

    const opexTimeSeriesData: ITimeSeriesData[] = [
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
            profile: seismicAcqAndProcCost,
            set: setSeismicAcqAndProcCost,
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
            profile: explorationWellCost,
            set: setExplorationWellCost,
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

    useEffect(() => {
        const newCase: Components.Schemas.CaseDto = { ...caseItem }
        if (!totalFeasibilityAndConceptStudiesOverride) { return }
        newCase.totalFeasibilityAndConceptStudiesOverride = totalFeasibilityAndConceptStudiesOverride
        setCase(newCase)
    }, [totalFeasibilityAndConceptStudiesOverride])

    useEffect(() => {
        const newCase: Components.Schemas.CaseDto = { ...caseItem }
        if (!totalFEEDStudiesOverride) { return }
        newCase.totalFEEDStudiesOverride = totalFEEDStudiesOverride
        setCase(newCase)
    }, [totalFEEDStudiesOverride])

    useEffect(() => {
        const newCase: Components.Schemas.CaseDto = { ...caseItem }
        if (!wellInterventionCostProfileOverride) { return }
        newCase.wellInterventionCostProfileOverride = wellInterventionCostProfileOverride
        setCase(newCase)
    }, [wellInterventionCostProfileOverride])

    useEffect(() => {
        const newCase: Components.Schemas.CaseDto = { ...caseItem }
        if (!offshoreFacilitiesOperationsCostProfileOverride) { return }
        newCase.offshoreFacilitiesOperationsCostProfileOverride = offshoreFacilitiesOperationsCostProfileOverride
        setCase(newCase)
    }, [offshoreFacilitiesOperationsCostProfileOverride])

    useEffect(() => {
        const newCase: Components.Schemas.CaseDto = { ...caseItem }
        if (!cessationWellsCostOverride) { return }
        newCase.cessationWellsCostOverride = cessationWellsCostOverride
        setCase(newCase)
    }, [cessationWellsCostOverride])

    useEffect(() => {
        const newCase: Components.Schemas.CaseDto = { ...caseItem }
        if (!cessationOffshoreFacilitiesCostOverride) { return }
        newCase.cessationOffshoreFacilitiesCostOverride = cessationOffshoreFacilitiesCostOverride
        setCase(newCase)
    }, [cessationOffshoreFacilitiesCostOverride])

    useEffect(() => {
        const newSurf: Components.Schemas.SurfDto = { ...surf }
        if (!surfCost) { return }
        newSurf.costProfile = surfCost
        setSurf(newSurf)
    }, [surfCost])

    useEffect(() => {
        const newTopside: Components.Schemas.TopsideDto = { ...topside }
        if (!topsideCost) { return }
        newTopside.costProfile = topsideCost
        setTopside(newTopside)
    }, [topsideCost])

    useEffect(() => {
        const newSubstructure: Components.Schemas.SubstructureDto = { ...substructure }
        if (!substructureCost) { return }
        newSubstructure.costProfile = substructureCost
        setSubstructure(newSubstructure)
    }, [substructureCost])

    useEffect(() => {
        const newTransport: Components.Schemas.TransportDto = { ...transport }
        if (!transportCost) { return }
        newTransport.costProfile = transportCost
        setTransport(newTransport)
    }, [transportCost])

    useEffect(() => {
        const newSurf: Components.Schemas.SurfDto = { ...surf }
        if (!surfCostOverride) { return }
        newSurf.costProfileOverride = surfCostOverride
        setSurf(newSurf)
    }, [surfCostOverride])

    useEffect(() => {
        const newTopside: Components.Schemas.TopsideDto = { ...topside }
        if (!topsideCostOverride) { return }
        newTopside.costProfileOverride = topsideCostOverride
        setTopside(newTopside)
    }, [topsideCostOverride])

    useEffect(() => {
        const newSubstructure: Components.Schemas.SubstructureDto = { ...substructure }
        if (!substructureCostOverride) { return }
        newSubstructure.costProfileOverride = substructureCostOverride
        setSubstructure(newSubstructure)
    }, [substructureCostOverride])

    useEffect(() => {
        const newTransport: Components.Schemas.TransportDto = { ...transport }
        if (!transportCostOverride) { return }
        newTransport.costProfileOverride = transportCostOverride
        setTransport(newTransport)
    }, [transportCostOverride])

    useEffect(() => {
        const newWellProject: Components.Schemas.WellProjectDto = { ...wellProject }
        if (!wellProjectOilProducerCost) { return }
        newWellProject.oilProducerCostProfile = wellProjectOilProducerCost
        setWellProject(newWellProject)
    }, [wellProjectOilProducerCost])

    useEffect(() => {
        const newWellProject: Components.Schemas.WellProjectDto = { ...wellProject }
        if (!wellProjectOilProducerCostOverride) { return }
        newWellProject.oilProducerCostProfileOverride = wellProjectOilProducerCostOverride
        setWellProject(newWellProject)
    }, [wellProjectOilProducerCostOverride])

    useEffect(() => {
        const newWellProject: Components.Schemas.WellProjectDto = { ...wellProject }
        if (!wellProjectGasProducerCost) { return }
        newWellProject.gasProducerCostProfile = wellProjectGasProducerCost
        setWellProject(newWellProject)
    }, [wellProjectGasProducerCost])

    useEffect(() => {
        const newWellProject: Components.Schemas.WellProjectDto = { ...wellProject }
        if (!wellProjectGasProducerCostOverride) { return }
        newWellProject.gasProducerCostProfileOverride = wellProjectGasProducerCostOverride
        setWellProject(newWellProject)
    }, [wellProjectGasProducerCostOverride])

    useEffect(() => {
        const newWellProject: Components.Schemas.WellProjectDto = { ...wellProject }
        if (!wellProjectWaterInjectorCost) { return }
        newWellProject.waterInjectorCostProfile = wellProjectWaterInjectorCost
        setWellProject(newWellProject)
    }, [wellProjectWaterInjectorCost])

    useEffect(() => {
        const newWellProject: Components.Schemas.WellProjectDto = { ...wellProject }
        if (!wellProjectWaterInjectorCostOverride) { return }
        newWellProject.waterInjectorCostProfileOverride = wellProjectWaterInjectorCostOverride
        setWellProject(newWellProject)
    }, [wellProjectWaterInjectorCostOverride])

    useEffect(() => {
        const newWellProject: Components.Schemas.WellProjectDto = { ...wellProject }
        if (!wellProjectGasInjectorCost) { return }
        newWellProject.gasInjectorCostProfile = wellProjectGasInjectorCost
        setWellProject(newWellProject)
    }, [wellProjectGasInjectorCost])

    useEffect(() => {
        const newWellProject: Components.Schemas.WellProjectDto = { ...wellProject }
        if (!wellProjectGasInjectorCostOverride) { return }
        newWellProject.gasInjectorCostProfileOverride = wellProjectGasInjectorCostOverride
        setWellProject(newWellProject)
    }, [wellProjectGasInjectorCostOverride])

    useEffect(() => {
        const newExploration: Components.Schemas.ExplorationDto = { ...exploration }
        if (!explorationWellCost) { return }
        newExploration.explorationWellCostProfile = explorationWellCost
        setExploration(newExploration)
    }, [explorationWellCost])

    useEffect(() => {
        const newExploration: Components.Schemas.ExplorationDto = { ...exploration }
        if (!explorationAppraisalWellCost) { return }
        newExploration.appraisalWellCostProfile = explorationAppraisalWellCost
        setExploration(newExploration)
    }, [explorationAppraisalWellCost])

    useEffect(() => {
        const newExploration: Components.Schemas.ExplorationDto = { ...exploration }
        if (!explorationSidetrackCost) { return }
        newExploration.sidetrackCostProfile = explorationSidetrackCost
        setExploration(newExploration)
    }, [explorationSidetrackCost])

    useEffect(() => {
        const newExploration: Components.Schemas.ExplorationDto = { ...exploration }
        if (!seismicAcqAndProcCost) { return }
        newExploration.seismicAcquisitionAndProcessing = seismicAcqAndProcCost
        setExploration(newExploration)
    }, [seismicAcqAndProcCost])

    useEffect(() => {
        const newExploration: Components.Schemas.ExplorationDto = { ...exploration }
        if (!countryOfficeCost) { return }
        newExploration.countryOfficeCost = countryOfficeCost
        setExploration(newExploration)
    }, [countryOfficeCost])

    if (activeTab !== 5) { return null }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Cost</PageTitle>
            </TopWrapper>
            <InputContainer mobileColumns={1} desktopColumns={3} breakPoint={850}>
                <CaseNumberInput
                    onChange={handleCaseFeasibilityChange}
                    defaultValue={caseItem.capexFactorFeasibilityStudies
                        !== undefined ? caseItem.capexFactorFeasibilityStudies * 100 : undefined}
                    integer={false}
                    label="CAPEX factor feasibility studies"
                    unit="%"
                />
                <CaseNumberInput
                    onChange={handleCaseFEEDChange}
                    defaultValue={caseItem.capexFactorFEEDStudies
                        !== undefined ? caseItem.capexFactorFEEDStudies * 100 : undefined}
                    integer={false}
                    label="CAPEX factor FEED studies"
                    unit="%"
                />
                <NativeSelect
                    id="maturity"
                    label="Maturity"
                    onChange={handleSurfMaturityChange}
                    value={surf.maturity}
                >
                    <option key="0" value={0}>A</option>
                    <option key="1" value={1}>B</option>
                    <option key="2" value={2}>C</option>
                    <option key="3" value={3}>D</option>
                </NativeSelect>

            </InputContainer>
            <FilterContainer>
                <NativeSelect
                    id="currency"
                    label="Currency"
                    onChange={() => { }}
                    value={project.currency}
                    disabled
                >
                    <option key="1" value={1}>MNOK</option>
                    <option key="2" value={2}>MUSD</option>
                </NativeSelect>
                <CaseNumberInput
                    onChange={handleStartYearChange}
                    defaultValue={startYear}
                    integer
                    label="Start year"
                />

                <CaseNumberInput
                    onChange={handleEndYearChange}
                    defaultValue={endYear}
                    integer
                    label="End year"
                />
                <Button onClick={handleTableYearsClick}>
                    Apply
                </Button>
            </FilterContainer>
            <TableWrapper>
                <CaseTabTable
                    timeSeriesData={studyTimeSeriesData}
                    dg4Year={caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="Study costs"
                    gridRef={studyGridRef}
                    alignedGridsRef={[opexGridRef, cessationGridRef, capexGridRef,
                        developmentWellsGridRef, explorationWellsGridRef]}
                    includeFooter
                    totalRowName="Study cost"
                />
            </TableWrapper>
            <TableWrapper>
                <CaseTabTable
                    timeSeriesData={opexTimeSeriesData}
                    dg4Year={caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="OPEX"
                    gridRef={opexGridRef}
                    alignedGridsRef={[studyGridRef, cessationGridRef, capexGridRef,
                        developmentWellsGridRef, explorationWellsGridRef]}
                    includeFooter
                    totalRowName="Total OPEX cost"
                />
            </TableWrapper>
            <TableWrapper>
                <CaseTabTable
                    timeSeriesData={cessationTimeSeriesData}
                    dg4Year={caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="Cessation costs"
                    gridRef={cessationGridRef}
                    alignedGridsRef={[studyGridRef, opexGridRef, capexGridRef,
                        developmentWellsGridRef, explorationWellsGridRef]}
                    includeFooter
                    totalRowName="Total cessation cost"
                />
            </TableWrapper>
            <TableWrapper>
                <CaseTabTable
                    timeSeriesData={capexTimeSeriesData}
                    dg4Year={caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="Offshore facilitiy costs"
                    gridRef={capexGridRef}
                    alignedGridsRef={[studyGridRef, opexGridRef, cessationGridRef,
                        developmentWellsGridRef, explorationWellsGridRef]}
                    includeFooter
                    totalRowName="Total offshore facility cost"
                />
            </TableWrapper>
            <TableWrapper>
                <CaseTabTable
                    timeSeriesData={developmentTimeSeriesData}
                    dg4Year={caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="Development well costs"
                    gridRef={developmentWellsGridRef}
                    alignedGridsRef={[studyGridRef, opexGridRef, cessationGridRef, capexGridRef,
                        explorationWellsGridRef]}
                    includeFooter
                    totalRowName="Total development well cost"
                />
            </TableWrapper>
            <CaseTabTable
                timeSeriesData={explorationTimeSeriesData}
                dg4Year={caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030}
                tableYears={tableYears}
                tableName="Exploration well costs"
                gridRef={explorationWellsGridRef}
                alignedGridsRef={[studyGridRef, opexGridRef, cessationGridRef, capexGridRef,
                    developmentWellsGridRef]}
                includeFooter
                totalRowName="Total exploration cost"
            />
        </>
    )
}

export default CaseCostTab
