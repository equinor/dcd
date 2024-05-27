import {
    FC,
    Dispatch,
    SetStateAction,
    createContext,
    useState,
    ReactNode,
    useContext,
    useMemo,
    useEffect,
} from "react"
import { ITimeSeries } from "../Models/ITimeSeries"
import { EditInstance } from "../Models/Interfaces"
import { useAppContext } from "../Context/AppContext"

interface CaseContextType {
    projectCase: Components.Schemas.CaseDto | undefined;
    setProjectCase: Dispatch<SetStateAction<Components.Schemas.CaseDto | undefined>>,
    projectCaseEdited: Components.Schemas.CaseDto | undefined; // todo: replace with caseEdits
    setProjectCaseEdited: Dispatch<SetStateAction<Components.Schemas.CaseDto | undefined>>, // todo: replace with caseEdits
    caseEdits: EditInstance[];
    setCaseEdits: Dispatch<SetStateAction<EditInstance[]>>,
    saveProjectCase: boolean,
    setSaveProjectCase: Dispatch<SetStateAction<boolean>>,
    activeTabCase: number;
    setActiveTabCase: Dispatch<SetStateAction<number>>,
    editIndexes: any[]
    setEditIndexes: Dispatch<SetStateAction<any[]>>

    topside: Components.Schemas.TopsideWithProfilesDto | undefined
    setTopside: Dispatch<SetStateAction<Components.Schemas.TopsideWithProfilesDto | undefined>>
    topsideCost: Components.Schemas.TopsideCostProfileDto | undefined
    setTopsideCost: Dispatch<SetStateAction<Components.Schemas.TopsideCostProfileDto | undefined>>
    surf: Components.Schemas.SurfWithProfilesDto | undefined
    setSurf: Dispatch<SetStateAction<Components.Schemas.SurfWithProfilesDto | undefined>>
    surfCost: Components.Schemas.SurfCostProfileDto | undefined
    setSurfCost: Dispatch<SetStateAction<Components.Schemas.SurfCostProfileDto | undefined>>
    surfCostOverride: Components.Schemas.SurfCostProfileOverrideDto | undefined
    setSurfCostOverride: Dispatch<SetStateAction<Components.Schemas.SurfCostProfileOverrideDto | undefined>>
    substructure: Components.Schemas.SubstructureWithProfilesDto | undefined
    setSubstructure: Dispatch<SetStateAction<Components.Schemas.SubstructureWithProfilesDto | undefined>>
    substructureCost: Components.Schemas.SubstructureCostProfileDto | undefined
    setSubstructureCost: Dispatch<SetStateAction<Components.Schemas.SubstructureCostProfileDto | undefined>>
    transport: Components.Schemas.TransportWithProfilesDto | undefined
    setTransport: Dispatch<SetStateAction<Components.Schemas.TransportWithProfilesDto | undefined>>
    transportCost: Components.Schemas.TransportCostProfileDto | undefined
    setTransportCost: Dispatch<SetStateAction<Components.Schemas.TransportCostProfileDto | undefined>>
    drainageStrategy: Components.Schemas.DrainageStrategyWithProfilesDto | undefined
    setDrainageStrategy: Dispatch<SetStateAction<Components.Schemas.DrainageStrategyWithProfilesDto | undefined>>

    wellProjectWells: Components.Schemas.WellProjectWellDto[] | undefined
    setWellProjectWells: Dispatch<SetStateAction<Components.Schemas.WellProjectWellDto[] | undefined>>
    explorationWells: Components.Schemas.ExplorationWellDto[] | undefined
    setExplorationWells: Dispatch<SetStateAction<Components.Schemas.ExplorationWellDto[] | undefined>>

    // CAPEX
    // Drilling cost
    totalDrillingCost: ITimeSeries | undefined,
    setTotalDrillingCost: Dispatch<SetStateAction<ITimeSeries | undefined>>,
    developmentOperationalWellCosts: Components.Schemas.DevelopmentOperationalWellCostsDto | undefined
    setDevelopmentOperationalWellCosts: Dispatch<SetStateAction<Components.Schemas.DevelopmentOperationalWellCostsDto | undefined>>
    cessationWellsCost: Components.Schemas.CessationWellsCostDto | undefined
    setCessationWellsCost: Dispatch<SetStateAction<Components.Schemas.CessationWellsCostDto | undefined>>

    cessationOffshoreFacilitiesCost: Components.Schemas.CessationOffshoreFacilitiesCostDto | undefined,
    setCessationOffshoreFacilitiesCost: Dispatch<SetStateAction<Components.Schemas.CessationOffshoreFacilitiesCostDto | undefined>>
    cessationOffshoreFacilitiesCostOverride: Components.Schemas.CessationOffshoreFacilitiesCostOverrideDto | undefined
    setCessationOffshoreFacilitiesCostOverride: Dispatch<SetStateAction<Components.Schemas.CessationOffshoreFacilitiesCostOverrideDto | undefined>>
    cessationOnshoreFacilitiesCostProfile: Components.Schemas.CessationOnshoreFacilitiesCostProfileDto | undefined,
    setCessationOnshoreFacilitiesCostProfile: Dispatch<SetStateAction<Components.Schemas.CessationOnshoreFacilitiesCostProfileDto | undefined>>
    // Study
    totalFeasibilityAndConceptStudies: Components.Schemas.TotalFeasibilityAndConceptStudiesDto | undefined
    setTotalFeasibilityAndConceptStudies: Dispatch<SetStateAction<Components.Schemas.TotalFeasibilityAndConceptStudiesDto | undefined>>
    totalFeasibilityAndConceptStudiesOverride: Components.Schemas.TotalFeasibilityAndConceptStudiesOverrideDto | undefined
    setTotalFeasibilityAndConceptStudiesOverride: Dispatch<SetStateAction<Components.Schemas.TotalFeasibilityAndConceptStudiesOverrideDto | undefined>>

    totalFEEDStudies: Components.Schemas.TotalFEEDStudiesDto | undefined
    setTotalFEEDStudies: Dispatch<SetStateAction<Components.Schemas.TotalFEEDStudiesDto | undefined>>
    totalFEEDStudiesOverride: Components.Schemas.TotalFEEDStudiesOverrideDto | undefined
    setTotalFEEDStudiesOverride: Dispatch<SetStateAction<Components.Schemas.TotalFEEDStudiesOverrideDto | undefined>>

    totalOtherStudies: Components.Schemas.TotalOtherStudiesDto | undefined,
    setTotalOtherStudies: Dispatch<SetStateAction<Components.Schemas.TotalOtherStudiesDto | undefined>>

    // OPEX
    historicCostCostProfile: Components.Schemas.HistoricCostCostProfileDto | undefined,
    setHistoricCostCostProfile: Dispatch<SetStateAction<Components.Schemas.HistoricCostCostProfileDto | undefined>>,
    wellInterventionCostProfile: Components.Schemas.WellInterventionCostProfileDto | undefined,
    setWellInterventionCostProfile: Dispatch<SetStateAction<Components.Schemas.WellInterventionCostProfileDto | undefined>>,
    offshoreFacilitiesOperationsCostProfile: Components.Schemas.OffshoreFacilitiesOperationsCostProfileDto | undefined,
    setOffshoreFacilitiesOperationsCostProfile: Dispatch<SetStateAction<Components.Schemas.OffshoreFacilitiesOperationsCostProfileDto | undefined>>,
    onshoreRelatedOPEXCostProfile: Components.Schemas.OnshoreRelatedOPEXCostProfileDto | undefined,
    setOnshoreRelatedOPEXCostProfile: Dispatch<SetStateAction<Components.Schemas.OnshoreRelatedOPEXCostProfileDto | undefined>>,
    additionalOPEXCostProfile: Components.Schemas.AdditionalOPEXCostProfileDto | undefined,
    setAdditionalOPEXCostProfile: Dispatch<SetStateAction<Components.Schemas.AdditionalOPEXCostProfileDto | undefined>>,

    // Exploration
    totalExplorationCost: ITimeSeries | undefined,
    setTotalExplorationCost: Dispatch<SetStateAction<ITimeSeries | undefined>>,
    gAndGAdminCost: Components.Schemas.GAndGAdminCostDto | undefined
    setGAndGAdminCost: Dispatch<SetStateAction<Components.Schemas.GAndGAdminCostDto | undefined>>
    seismicAcquisitionAndProcessing: Components.Schemas.SeismicAcquisitionAndProcessingDto | undefined
    setSeismicAcquisitionAndProcessing: Dispatch<SetStateAction<Components.Schemas.SeismicAcquisitionAndProcessingDto | undefined>>
    countryOfficeCost: Components.Schemas.CountryOfficeCostDto | undefined
    setCountryOfficeCost: Dispatch<SetStateAction<Components.Schemas.CountryOfficeCostDto | undefined>>
    explorationWellCostProfile: Components.Schemas.ExplorationWellCostProfileDto | undefined
    setExplorationWellCostProfile: Dispatch<SetStateAction<Components.Schemas.ExplorationWellCostProfileDto | undefined>>
    appraisalWellCostProfile: Components.Schemas.AppraisalWellCostProfileDto | undefined
    setAppraisalWellCostProfile: Dispatch<SetStateAction<Components.Schemas.AppraisalWellCostProfileDto | undefined>>
    sidetrackCostProfile: Components.Schemas.SidetrackCostProfileDto | undefined
    setSidetrackCostProfile: Dispatch<SetStateAction<Components.Schemas.SidetrackCostProfileDto | undefined>>

    offshoreFacilitiesCost: ITimeSeries | undefined,
    setOffshoreFacilitiesCost: Dispatch<SetStateAction<ITimeSeries | undefined>>,

    offshoreOpexPlussWellIntervention: ITimeSeries | undefined,
    setOffshoreOpexPlussWellIntervention: Dispatch<SetStateAction<ITimeSeries | undefined>>,
}

const CaseContext = createContext<CaseContextType | undefined>(undefined)

const getFilteredEdits = () => {
    const savedEdits = JSON.parse(localStorage.getItem("caseEdits") || "[]")
    const oneHourAgo = new Date().getTime() - (60 * 60 * 1000)

    // localStorage cleanup to remove edits older than 1 hour
    const recentEdits = savedEdits.filter((edit: EditInstance) => edit.timeStamp > oneHourAgo)
    localStorage.setItem("caseEdits", JSON.stringify(recentEdits))

    return recentEdits
}

const CaseContextProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const [projectCase, setProjectCase] = useState<Components.Schemas.CaseDto | undefined>()
    const [caseEdits, setCaseEdits] = useState<EditInstance[]>(getFilteredEdits())
    const [projectCaseEdited, setProjectCaseEdited] = useState<Components.Schemas.CaseDto | undefined>()
    const [saveProjectCase, setSaveProjectCase] = useState<boolean>(false)
    const [activeTabCase, setActiveTabCase] = useState<number>(0)
    const [editIndexes, setEditIndexes] = useState<any[]>([])

    const [topside, setTopside] = useState<Components.Schemas.TopsideWithProfilesDto | undefined>()
    const [topsideCost, setTopsideCost] = useState<Components.Schemas.TopsideCostProfileDto | undefined>()
    const [surf, setSurf] = useState<Components.Schemas.SurfWithProfilesDto>()
    const [surfCost, setSurfCost] = useState<Components.Schemas.SurfCostProfileDto | undefined>()
    const [surfCostOverride, setSurfCostOverride] = useState<Components.Schemas.SurfCostProfileOverrideDto>()
    const [substructure, setSubstructure] = useState<Components.Schemas.SubstructureWithProfilesDto>()
    const [substructureCost, setSubstructureCost] = useState<Components.Schemas.SubstructureCostProfileDto | undefined>()
    const [transport, setTransport] = useState<Components.Schemas.TransportWithProfilesDto>()
    const [transportCost, setTransportCost] = useState<Components.Schemas.TransportCostProfileDto | undefined>()
    const [drainageStrategy, setDrainageStrategy] = useState<Components.Schemas.DrainageStrategyWithProfilesDto>()

    const [wellProjectWells, setWellProjectWells] = useState<Components.Schemas.WellProjectWellDto[] | undefined>()
    const [explorationWells, setExplorationWells] = useState<Components.Schemas.ExplorationWellDto[] | undefined>()

    // CAPEX
    // Drilling cost
    const [totalDrillingCost, setTotalDrillingCost] = useState<ITimeSeries | undefined>()
    const [cessationWellsCost, setCessationWellsCost] = useState<Components.Schemas.CessationWellsCostDto | undefined>()
    const [developmentOperationalWellCosts, setDevelopmentOperationalWellCosts] = useState<Components.Schemas.DevelopmentOperationalWellCostsDto | undefined>()
    const [cessationOffshoreFacilitiesCost, setCessationOffshoreFacilitiesCost] = useState<Components.Schemas.CessationOffshoreFacilitiesCostDto | undefined>()
    const [cessationOffshoreFacilitiesCostOverride, setCessationOffshoreFacilitiesCostOverride] = useState<Components.Schemas.CessationOffshoreFacilitiesCostOverrideDto | undefined>()
    const [cessationOnshoreFacilitiesCostProfile, setCessationOnshoreFacilitiesCostProfile] = useState<Components.Schemas.CessationOnshoreFacilitiesCostProfileDto | undefined>()

    // Study
    const [totalFeasibilityAndConceptStudies, setTotalFeasibilityAndConceptStudies] = useState<Components.Schemas.TotalFeasibilityAndConceptStudiesDto | undefined>()
    const [totalFeasibilityAndConceptStudiesOverride, setTotalFeasibilityAndConceptStudiesOverride] = useState<Components.Schemas.TotalFeasibilityAndConceptStudiesOverrideDto | undefined>()

    const [totalFEEDStudies, setTotalFEEDStudies] = useState<Components.Schemas.TotalFEEDStudiesDto | undefined>()
    const [totalFEEDStudiesOverride, setTotalFEEDStudiesOverride] = useState<Components.Schemas.TotalFEEDStudiesOverrideDto | undefined>()
    const [totalOtherStudies, setTotalOtherStudies] = useState<Components.Schemas.TotalOtherStudiesDto | undefined>()

    // OPEX
    const [historicCostCostProfile, setHistoricCostCostProfile] = useState<Components.Schemas.HistoricCostCostProfileDto | undefined>()
    const [wellInterventionCostProfile, setWellInterventionCostProfile] = useState<Components.Schemas.WellInterventionCostProfileDto | undefined>()
    const [offshoreFacilitiesOperationsCostProfile, setOffshoreFacilitiesOperationsCostProfile] = useState<Components.Schemas.OffshoreFacilitiesOperationsCostProfileDto | undefined>()
    const [onshoreRelatedOPEXCostProfile, setOnshoreRelatedOPEXCostProfile] = useState<Components.Schemas.OnshoreRelatedOPEXCostProfileDto | undefined>()
    const [additionalOPEXCostProfile, setAdditionalOPEXCostProfile] = useState<Components.Schemas.AdditionalOPEXCostProfileDto | undefined>()

    // Exploration
    const [totalExplorationCost, setTotalExplorationCost] = useState<ITimeSeries | undefined>()
    const [explorationWellCostProfile, setExplorationWellCostProfile] = useState<Components.Schemas.ExplorationWellCostProfileDto>()
    const [appraisalWellCostProfile, setAppraisalWellCostProfile] = useState<Components.Schemas.AppraisalWellCostProfileDto>()
    const [sidetrackCostProfile, setSidetrackCostProfile] = useState<Components.Schemas.SidetrackCostProfileDto>()
    const [seismicAcquisitionAndProcessing, setSeismicAcquisitionAndProcessing] = useState<Components.Schemas.SeismicAcquisitionAndProcessingDto>()
    const [countryOfficeCost, setCountryOfficeCost] = useState<Components.Schemas.CountryOfficeCostDto>()
    const [gAndGAdminCost, setGAndGAdminCost] = useState<Components.Schemas.GAndGAdminCostDto>()

    const [offshoreFacilitiesCost, setOffshoreFacilitiesCost] = useState<ITimeSeries>()
    const [offshoreOpexPlussWellIntervention, setOffshoreOpexPlussWellIntervention] = useState<ITimeSeries | undefined>()

    const value = useMemo(() => ({
        projectCase,
        setProjectCase,
        caseEdits,
        setCaseEdits,
        projectCaseEdited,
        setProjectCaseEdited,
        saveProjectCase,
        setSaveProjectCase,
        activeTabCase,
        setActiveTabCase,
        editIndexes,
        setEditIndexes,

        topside,
        setTopside,
        topsideCost,
        setTopsideCost,
        surf,
        setSurf,
        surfCost,
        setSurfCost,
        surfCostOverride,
        setSurfCostOverride,
        substructure,
        setSubstructure,
        substructureCost,
        setSubstructureCost,
        transport,
        setTransport,
        transportCost,
        setTransportCost,
        drainageStrategy,
        setDrainageStrategy,

        wellProjectWells,
        setWellProjectWells,
        explorationWells,
        setExplorationWells,

        // CAPEX
        // Drilling cost
        totalDrillingCost,
        setTotalDrillingCost,
        cessationWellsCost,
        setCessationWellsCost,
        developmentOperationalWellCosts,
        setDevelopmentOperationalWellCosts,
        cessationOffshoreFacilitiesCost,
        setCessationOffshoreFacilitiesCost,
        cessationOffshoreFacilitiesCostOverride,
        setCessationOffshoreFacilitiesCostOverride,
        cessationOnshoreFacilitiesCostProfile,
        setCessationOnshoreFacilitiesCostProfile,

        // Study
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
        totalExplorationCost,
        setTotalExplorationCost,
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

        offshoreFacilitiesCost,
        setOffshoreFacilitiesCost,

        offshoreOpexPlussWellIntervention,
        setOffshoreOpexPlussWellIntervention,
    }), [
        projectCase,
        setProjectCase,
        projectCaseEdited,
        setProjectCaseEdited,
        saveProjectCase,
        setSaveProjectCase,
        activeTabCase,
        setActiveTabCase,
        editIndexes,
        setEditIndexes,

        topside,
        topsideCost,
        surf,
        surfCost,
        surfCostOverride,
        substructure,
        substructureCost,
        transport,
        transportCost,
        drainageStrategy,

        wellProjectWells,
        explorationWells,

        // CAPEX
        // Drilling cost
        totalDrillingCost,
        cessationWellsCost,
        developmentOperationalWellCosts,
        cessationOffshoreFacilitiesCost,
        cessationOffshoreFacilitiesCostOverride,
        cessationOnshoreFacilitiesCostProfile,

        // Study
        totalFeasibilityAndConceptStudies,
        totalFeasibilityAndConceptStudiesOverride,
        totalFEEDStudies,
        totalFEEDStudiesOverride,
        totalOtherStudies,

        // OPEX
        historicCostCostProfile,
        wellInterventionCostProfile,
        offshoreFacilitiesOperationsCostProfile,
        onshoreRelatedOPEXCostProfile,
        additionalOPEXCostProfile,

        // Exploration
        totalExplorationCost,
        explorationWellCostProfile,
        gAndGAdminCost,
        countryOfficeCost,
        appraisalWellCostProfile,
        sidetrackCostProfile,
        seismicAcquisitionAndProcessing,

        offshoreFacilitiesCost,
        offshoreOpexPlussWellIntervention,
    ])

    const { editMode } = useAppContext()

    useEffect(() => {
        localStorage.setItem("caseEdits", JSON.stringify(caseEdits))
    }, [caseEdits])

    useEffect(() => {
        if (editMode && projectCase && !projectCaseEdited) {
            setProjectCaseEdited(projectCase)
        }
    }, [editMode, projectCaseEdited])

    return (
        <CaseContext.Provider value={value}>
            {children}
        </CaseContext.Provider>
    )
}

export const useCaseContext = (): CaseContextType => {
    const context = useContext(CaseContext)
    if (context === undefined) {
        throw new Error("useCaseContext must be used within an CaseContextProvider")
    }
    return context
}

export { CaseContextProvider }
