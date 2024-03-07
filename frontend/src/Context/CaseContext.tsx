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
import { useAppContext } from "./AppContext"
import { ITimeSeries } from "../Models/ITimeSeries"

interface CaseContextType {
    projectCase: Components.Schemas.CaseDto | undefined;
    setProjectCase: Dispatch<SetStateAction<Components.Schemas.CaseDto | undefined>>,
    renameProjectCase: boolean,
    setRenameProjectCase: Dispatch<SetStateAction<boolean>>,
    projectCaseEdited: Components.Schemas.CaseDto | undefined;
    setProjectCaseEdited: Dispatch<SetStateAction<Components.Schemas.CaseDto | undefined>>,
    saveProjectCase: boolean,
    setSaveProjectCase: Dispatch<SetStateAction<boolean>>,
    projectCaseNew: Components.Schemas.CreateCaseDto | undefined;
    setProjectCaseNew: Dispatch<SetStateAction<Components.Schemas.CreateCaseDto | undefined>>,
    activeTabCase: number;
    setActiveTabCase: Dispatch<SetStateAction<number>>,

    topside: Components.Schemas.TopsideDto | undefined
    setTopside: Dispatch<SetStateAction<Components.Schemas.TopsideDto | undefined>>
    topsideCost: Components.Schemas.TopsideCostProfileDto | undefined
    setTopsideCost: Dispatch<SetStateAction<Components.Schemas.TopsideCostProfileDto | undefined>>
    surf: Components.Schemas.SurfDto | undefined
    setSurf: Dispatch<SetStateAction<Components.Schemas.SurfDto | undefined>>
    surfCost: Components.Schemas.SurfCostProfileDto | undefined
    setSurfCost: Dispatch<SetStateAction<Components.Schemas.SurfCostProfileDto | undefined>>
    substructure: Components.Schemas.SubstructureDto | undefined
    setSubstructure: Dispatch<SetStateAction<Components.Schemas.SubstructureDto | undefined>>
    substructureCost: Components.Schemas.SubstructureCostProfileDto | undefined
    setSubstructureCost: Dispatch<SetStateAction<Components.Schemas.SubstructureCostProfileDto | undefined>>
    transport: Components.Schemas.TransportDto | undefined
    setTransport: Dispatch<SetStateAction<Components.Schemas.TransportDto | undefined>>
    transportCost: Components.Schemas.TransportCostProfileDto | undefined
    setTransportCost: Dispatch<SetStateAction<Components.Schemas.TransportCostProfileDto | undefined>>

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
    explorationAppraisalWellCost: Components.Schemas.AppraisalWellCostProfileDto | undefined
    setExplorationAppraisalWellCost: Dispatch<SetStateAction<Components.Schemas.AppraisalWellCostProfileDto | undefined>>
    explorationSidetrackCost: Components.Schemas.SidetrackCostProfileDto | undefined
    setExplorationSidetrackCost: Dispatch<SetStateAction<Components.Schemas.SidetrackCostProfileDto | undefined>>
}

const CaseContext = createContext<CaseContextType | undefined>(undefined)

const CaseContextProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const { editMode, setEditMode } = useAppContext()
    const [projectCase, setProjectCase] = useState<Components.Schemas.CaseDto | undefined>()
    const [renameProjectCase, setRenameProjectCase] = useState<boolean>(false)
    const [projectCaseEdited, setProjectCaseEdited] = useState<Components.Schemas.CaseDto | undefined>()
    const [saveProjectCase, setSaveProjectCase] = useState<boolean>(false)
    const [projectCaseNew, setProjectCaseNew] = useState<Components.Schemas.CreateCaseDto | undefined>()
    const [activeTabCase, setActiveTabCase] = useState<number>(0)

    const [topside, setTopside] = useState<Components.Schemas.TopsideDto | undefined>()
    const [topsideCost, setTopsideCost] = useState<Components.Schemas.TopsideCostProfileDto | undefined>()
    const [surf, setSurf] = useState<Components.Schemas.SurfDto>()
    const [surfCost, setSurfCost] = useState<Components.Schemas.SurfCostProfileDto | undefined>()
    const [substructure, setSubstructure] = useState<Components.Schemas.SubstructureDto>()
    const [substructureCost, setSubstructureCost] = useState<Components.Schemas.SubstructureCostProfileDto | undefined>()
    const [transport, setTransport] = useState<Components.Schemas.TransportDto>()
    const [transportCost, setTransportCost] = useState<Components.Schemas.TransportCostProfileDto | undefined>()

    // Study
    const [totalFeasibilityAndConceptStudies, setTotalFeasibilityAndConceptStudies] = useState<Components.Schemas.TotalFeasibilityAndConceptStudiesDto | undefined>()
    const [totalFeasibilityAndConceptStudiesOverride, setTotalFeasibilityAndConceptStudiesOverride] = useState<Components.Schemas.TotalFeasibilityAndConceptStudiesOverrideDto | undefined>()

    const [totalFEEDStudies, setTotalFEEDStudies] = useState<Components.Schemas.TotalFEEDStudiesDto | undefined>()
    const [totalFEEDStudiesOverride, setTotalFEEDStudiesOverride] = useState<Components.Schemas.TotalFEEDStudiesOverrideDto | undefined>()
    const [totalOtherStudies, setTotalOtherStudies] = useState<Components.Schemas.TotalOtherStudiesDto | undefined>()

        // Exploration
        const [totalExplorationCost, setTotalExplorationCost] = useState<ITimeSeries | undefined>()
        const [explorationWellCostProfile, setExplorationWellCostProfile] = useState<Components.Schemas.ExplorationWellCostProfileDto>()
        const [explorationAppraisalWellCost, setExplorationAppraisalWellCost] = useState<Components.Schemas.AppraisalWellCostProfileDto>()
        const [explorationSidetrackCost, setExplorationSidetrackCost] = useState<Components.Schemas.SidetrackCostProfileDto>()
        const [seismicAcquisitionAndProcessing, setSeismicAcquisitionAndProcessing] = useState<Components.Schemas.SeismicAcquisitionAndProcessingDto>()
        const [countryOfficeCost, setCountryOfficeCost] = useState<Components.Schemas.CountryOfficeCostDto>()
        const [gAndGAdminCost, setGAndGAdminCost] = useState<Components.Schemas.GAndGAdminCostDto>()

    const value = useMemo(() => ({
        projectCase,
        setProjectCase,
        renameProjectCase,
        setRenameProjectCase,
        projectCaseEdited,
        setProjectCaseEdited,
        saveProjectCase,
        setSaveProjectCase,
        projectCaseNew,
        setProjectCaseNew,
        activeTabCase,
        setActiveTabCase,

        topside,
        setTopside,
        topsideCost,
        setTopsideCost,
        surf,
        setSurf,
        surfCost,
        setSurfCost,
        substructure,
        setSubstructure,
        substructureCost,
        setSubstructureCost,
        transport,
        setTransport,
        transportCost,
        setTransportCost,

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

        // Exploration
        totalExplorationCost,
        setTotalExplorationCost,
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
    }), [
        projectCase,
        setProjectCase,
        renameProjectCase,
        setRenameProjectCase,
        projectCaseEdited,
        setProjectCaseEdited,
        saveProjectCase,
        setSaveProjectCase,
        projectCaseNew,
        setProjectCaseNew,
        activeTabCase,
        setActiveTabCase,

        topside,
        topsideCost,
        surf,
        surfCost,
        substructure,
        substructureCost,
        transport,
        transportCost,

        // Study
        totalFeasibilityAndConceptStudies,
        totalFeasibilityAndConceptStudiesOverride,
        totalFEEDStudies,
        totalFEEDStudiesOverride,
        totalOtherStudies,

        // Exploration
        totalExplorationCost,
        explorationWellCostProfile,
        gAndGAdminCost,
        countryOfficeCost,
        explorationAppraisalWellCost,
        explorationSidetrackCost,
        seismicAcquisitionAndProcessing,
    ])

    useEffect(() => {
        if (editMode && projectCase && !projectCaseEdited) {
            setProjectCaseEdited(projectCase)
            setRenameProjectCase(editMode)
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
