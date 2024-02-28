import React, {
    createContext,
    useState,
    ReactNode,
    useContext,
    useMemo,
    Dispatch,
    SetStateAction
} from "react";
import { ITimeSeries } from "../Models/ITimeSeries";


// Assuming Components.Schemas.* are defined elsewhere
// Replace Components.Schemas.* with the correct type or interface for your project

interface AppContextType {
    modalEditMode: boolean;
    setModalEditMode: Dispatch<SetStateAction<boolean>>;
    modalShouldNavigate: boolean;
    setModalShouldNavigate: Dispatch<SetStateAction<boolean>>;
    modalCaseId: string | undefined;
    setModalCaseId: Dispatch<SetStateAction<string | undefined>>;
    addNewCase: () => void;
    editCase: (caseId: string) => void;
    editMode: boolean;
    setEditMode: Dispatch<SetStateAction<boolean>>;

    project: Components.Schemas.ProjectDto | undefined;
    setProject: Dispatch<SetStateAction<Components.Schemas.ProjectDto | undefined>>;
    caseItem: Components.Schemas.CaseDto | undefined;
    setCase: Dispatch<SetStateAction<Components.Schemas.CaseDto | undefined>>;
    activeTab: number;
    setActiveTab: Dispatch<SetStateAction<number>>;
    createCaseModalIsOpen: boolean;
    setCreateCaseModalIsOpen: Dispatch<SetStateAction<boolean>>;

    // OPEX
    totalStudyCost: ITimeSeries | undefined;
    setTotalStudyCost: Dispatch<SetStateAction<ITimeSeries | undefined>>;
    opexSum: Components.Schemas.OpexCostProfileDto | undefined;
    setOpexSum: Dispatch<SetStateAction<Components.Schemas.OpexCostProfileDto | undefined>>;
    // cessationCost: Components.Schemas.SurfCessationCostProfileDto | undefined;
    // setCessationCost: Dispatch<SetStateAction<Components.Schemas.SurfCessationCostProfileDto | undefined>>;

    // CAPEX
    topside: Components.Schemas.TopsideDto | undefined;
    setTopside: Dispatch<SetStateAction<Components.Schemas.TopsideDto | undefined>>;
    topsideCost: Components.Schemas.TopsideCostProfileDto | undefined;
    setTopsideCost: Dispatch<SetStateAction<Components.Schemas.TopsideCostProfileDto | undefined>>;
    surf: Components.Schemas.SurfDto | undefined;
    setSurf: Dispatch<SetStateAction<Components.Schemas.SurfDto | undefined>>;
    surfCost: Components.Schemas.SurfCostProfileDto | undefined;
    setSurfCost: Dispatch<SetStateAction<Components.Schemas.SurfCostProfileDto | undefined>>;
    substructure: Components.Schemas.SubstructureDto| undefined;
    setSubstructure: Dispatch<SetStateAction<Components.Schemas.SubstructureDto | undefined>>;
    substructureCost: Components.Schemas.SubstructureCostProfileDto | undefined;
    setSubstructureCost: Dispatch<SetStateAction<Components.Schemas.SubstructureCostProfileDto | undefined>>;
    transport: Components.Schemas.TransportDto| undefined;
    setTransport: Dispatch<SetStateAction<Components.Schemas.TransportDto | undefined>>;
    transportCost: Components.Schemas.TransportCostProfileDto | undefined;
    setTransportCost: Dispatch<SetStateAction<Components.Schemas.TransportCostProfileDto | undefined>>;
    setStartYear: Dispatch<SetStateAction<number>>;
    setEndYear: Dispatch<SetStateAction<number>>;
    tableYears: [number, number];
    setTableYears: Dispatch<SetStateAction<[number, number]>>;
    drillingCost: Components.Schemas.drillingCostDto | undefined;
    setDrillingCost: Dispatch<SetStateAction<Components.Schemas.drillingCostDto | undefined>>;
    cessationOffshoreFacilitiesCost: Components.Schemas.CessationOffshoreFacilitiesCostDto | undefined;
    setCessationOffshoreFacilitiesCost: Dispatch<SetStateAction<Components.Schemas.CessationOffshoreFacilitiesCostDto | undefined>>;

    exploration: Components.Schemas.ExplorationDto | undefined,
    setExploration: Dispatch<SetStateAction<Components.Schemas.ExplorationDto | undefined>>,
    totalExplorationCost: ITimeSeries | undefined,
    setTotalExplorationCost: Dispatch<SetStateAction<ITimeSeries | undefined>>,
    gAndGAdminCost: Components.Schemas.GAndGAdminCostDto| undefined;
    setGAndGAdminCost: Dispatch<SetStateAction<Components.Schemas.GAndGAdminCostDto | undefined>>;
    seismicAcqAndProcCost: Components.Schemas.SeismicAcquisitionAndProcessingDto| undefined;
    setSeismicAcqAndProcCost: Dispatch<SetStateAction<Components.Schemas.SeismicAcquisitionAndProcessingDto | undefined>>;
    countryOfficeCost: Components.Schemas.CountryOfficeCostDto| undefined;
    setCountryOfficeCost: Dispatch<SetStateAction<Components.Schemas.CountryOfficeCostDto | undefined>>;
    explorationWellCostProfile: Components.Schemas.ExplorationWellCostProfileDto| undefined;
    setExplorationWellCostProfile: Dispatch<SetStateAction<Components.Schemas.ExplorationWellCostProfileDto | undefined>>;
    explorationAppraisalWellCost: Components.Schemas.AppraisalWellCostProfileDto| undefined;
    setExplorationAppraisalWellCost: Dispatch<SetStateAction<Components.Schemas.AppraisalWellCostProfileDto | undefined>>;
    explorationSidetrackCost: Components.Schemas.SidetrackCostProfileDto| undefined;
    setExplorationSidetrackCost: Dispatch<SetStateAction<Components.Schemas.SidetrackCostProfileDto | undefined>>;


    totalFeasibilityAndConceptStudies: Components.Schemas.TotalFeasibilityAndConceptStudiesDto | undefined;
    setTotalFeasibilityAndConceptStudies: Dispatch<SetStateAction<Components.Schemas.TotalFeasibilityAndConceptStudiesDto | undefined>>;
    totalFEEDStudies: Components.Schemas.TotalFEEDStudiesDto | undefined;
    setTotalFEEDStudies: Dispatch<SetStateAction<Components.Schemas.TotalFEEDStudiesDto | undefined>>;
    totalOtherStudies: Components.Schemas.TotalOtherStudiesDto | undefined,
    setTotalOtherStudies: Dispatch<SetStateAction<Components.Schemas.TotalOtherStudiesDto | undefined>>;
    historicCostCostProfile: Components.Schemas.HistoricCostCostProfileDto | undefined,
    setHistoricCostCostProfile: Dispatch<SetStateAction<Components.Schemas.HistoricCostCostProfileDto | undefined>>;
    additionalOPEXCostProfile: Components.Schemas.AdditionalOPEXCostProfileDto | undefined,
    setAdditionalOPEXCostProfile: Dispatch<SetStateAction<Components.Schemas.AdditionalOPEXCostProfileDto | undefined>>;
    
    productionAndSalesVolume: Components.Schemas.ProductionAndSalesVolumesDto | undefined;
    setProductionAndSalesVolume: Dispatch<SetStateAction<Components.Schemas.ProductionAndSalesVolumesDto | undefined>>;
    oilCondensateProduction: Components.Schemas.ProductionProfileOilDto | undefined;
    setOilCondensateProduction: Dispatch<SetStateAction<Components.Schemas.ProductionProfileOilDto | undefined>>;
    nglProduction: Components.Schemas.ProductionProfileNGLDto | undefined;
    setNGLProduction: Dispatch<SetStateAction<Components.Schemas.ProductionProfileNGLDto | undefined>>;
    NetSalesGas: Components.Schemas.NetSalesGasDto | undefined;
    setNetSalesGas: Dispatch<SetStateAction<Components.Schemas.NetSalesGasDto | undefined>>;
    cO2Emissions: Components.Schemas.Co2EmissionsDto | undefined;
    setCO2Emissions: Dispatch<SetStateAction<Components.Schemas.Co2EmissionsDto | undefined>>;
    importedElectricity: Components.Schemas.ImportedElectricityDto | undefined;
    setImportedElectricity: Dispatch<SetStateAction<Components.Schemas.ImportedElectricityDto | undefined>>;
}

export const AppContext = createContext<AppContextType | undefined>(undefined);

const AppContextProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [project, setProject] = useState<Components.Schemas.ProjectDto | undefined>();
    // Initialize the state for each property
    const [totalStudyCost, setTotalStudyCost] = useState<ITimeSeries | undefined>();
    const [opexSum, setOpexSum] = useState<Components.Schemas.OpexCostProfileDto | undefined>();
    const [activeTab, setActiveTab] = useState<number>(0);
    const [caseItem, setCase] = useState<Components.Schemas.CaseDto | undefined>();
    const [createCaseModalIsOpen, setCreateCaseModalIsOpen] = useState<boolean>(false)
    const [editModalIsOpen, setEditModalIsOpen] = useState<boolean>(false)
    const [modalEditMode, setModalEditMode] = useState<boolean>(false)
    const [modalShouldNavigate, setModalShouldNavigate] = useState<boolean>(false)
    const [modalCaseId, setModalCaseId] = useState<string | undefined>(undefined)
    const [editMode, setEditMode] = useState<boolean>(false)

    const editCase = (caseId: string) => {
        setModalShouldNavigate(true)
        setModalEditMode(true)
        setModalCaseId(caseId)
        setCreateCaseModalIsOpen(true)
    }

    const addNewCase = () => {
        setModalCaseId(undefined)
        setModalShouldNavigate(false)
        setModalEditMode(false)
        setCreateCaseModalIsOpen(true)
    }

    const [topside, setTopside] = useState<Components.Schemas.TopsideDto | undefined>();
    const [topsideCost, setTopsideCost] = useState<Components.Schemas.TopsideCostProfileDto | undefined>();
    const [surf, setSurf] = useState<Components.Schemas.SurfDto>();
    const [surfCost, setSurfCost] = useState<Components.Schemas.SurfCostProfileDto | undefined>();
    const [substructure, setSubstructure] = useState<Components.Schemas.SubstructureDto>();
    const [substructureCost, setSubstructureCost] = useState<Components.Schemas.SubstructureCostProfileDto | undefined>();
    const [transport, setTransport] = useState<Components.Schemas.TransportDto>();
    const [transportCost, setTransportCost] = useState<Components.Schemas.TransportCostProfileDto | undefined>();

    const [startYear, setStartYear] = useState<number>(2020);
    const [endYear, setEndYear] = useState<number>(2030);
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030]);

    const [totalFeasibilityAndConceptStudies, setTotalFeasibilityAndConceptStudies] = useState<Components.Schemas.TotalFeasibilityAndConceptStudiesDto | undefined>();
    const [totalFEEDStudies, setTotalFEEDStudies] = useState<Components.Schemas.TotalFEEDStudiesDto | undefined>();
    const [totalOtherStudies, setTotalOtherStudies] = useState<Components.Schemas.TotalOtherStudiesDto | undefined>();
    const [historicCostCostProfile, setHistoricCostCostProfile] = useState<Components.Schemas.HistoricCostCostProfileDto | undefined>();
    const [additionalOPEXCostProfile, setAdditionalOPEXCostProfile] = useState<Components.Schemas.AdditionalOPEXCostProfileDto | undefined>();

    const [productionAndSalesVolume, setProductionAndSalesVolume] = useState<Components.Schemas.ProductionAndSalesVolumesDto | undefined>();
    const [oilCondensateProduction, setOilCondensateProduction] = useState<Components.Schemas.ProductionProfileOilDto | undefined>();
    const [nglProduction, setNGLProduction] = useState<Components.Schemas.ProductionProfileNGLDto | undefined>();
    const [NetSalesGas, setNetSalesGas] = useState<Components.Schemas.NetSalesGasDto | undefined>();
    const [cO2Emissions, setCO2Emissions] = useState<Components.Schemas.Co2EmissionsDto | undefined>();
    const [importedElectricity, setImportedElectricity] = useState<Components.Schemas.ImportedElectricityDto | undefined>();
    const [drillingCost, setDrillingCost] = useState<Components.Schemas.drillingCostDto>();
    const [cessationOffshoreFacilitiesCost, setCessationOffshoreFacilitiesCost] = useState<Components.Schemas.CessationOffshoreFacilitiesCostDto>();
    const [exploration, setExploration] = useState<Components.Schemas.ExplorationDto | undefined>();
    const [totalExplorationCost, setTotalExplorationCost] = useState<ITimeSeries | undefined>()
    const [explorationWellCostProfile, setExplorationWellCostProfile] = useState<Components.Schemas.ExplorationWellCostProfileDto>();
    const [gAndGAdminCost, setGAndGAdminCost] = useState<Components.Schemas.GAndGAdminCostDto>();
    const [countryOfficeCost, setCountryOfficeCost] = useState<Components.Schemas.CountryOfficeCostDto>()
    const [explorationAppraisalWellCost, setExplorationAppraisalWellCost] = useState<Components.Schemas.AppraisalWellCostProfileDto>()
    const [explorationSidetrackCost, setExplorationSidetrackCost] = useState<Components.Schemas.SidetrackCostProfileDto>()
    const [seismicAcqAndProcCost, setSeismicAcqAndProcCost] = useState<Components.Schemas.SeismicAcquisitionAndProcessingDto>()

    const value = useMemo(() => ({
        project, setProject,
        activeTab, setActiveTab,
        createCaseModalIsOpen, setCreateCaseModalIsOpen,
        caseItem, setCase,
        totalStudyCost, setTotalStudyCost,
        opexSum, setOpexSum,
        cessationOffshoreFacilitiesCost, setCessationOffshoreFacilitiesCost,
        topside, setTopside,
        topsideCost, setTopsideCost,
        surf, setSurf,
        surfCost, setSurfCost,
        substructure, setSubstructure,
        substructureCost, setSubstructureCost,
        transport, setTransport,
        transportCost, setTransportCost,
        startYear, setStartYear,
        endYear, setEndYear,
        tableYears, setTableYears,
        totalFeasibilityAndConceptStudies, setTotalFeasibilityAndConceptStudies,
        totalFEEDStudies, setTotalFEEDStudies,
        totalOtherStudies, setTotalOtherStudies,
        historicCostCostProfile, setHistoricCostCostProfile,
        additionalOPEXCostProfile, setAdditionalOPEXCostProfile,
        productionAndSalesVolume, setProductionAndSalesVolume,
        oilCondensateProduction, setOilCondensateProduction,
        nglProduction, setNGLProduction,
        NetSalesGas, setNetSalesGas,
        cO2Emissions, setCO2Emissions,
        importedElectricity, setImportedElectricity,
        drillingCost, setDrillingCost,
        exploration, setExploration,
        totalExplorationCost, setTotalExplorationCost,
        explorationWellCostProfile, setExplorationWellCostProfile,
        gAndGAdminCost, setGAndGAdminCost,
        seismicAcqAndProcCost, setSeismicAcqAndProcCost,
        explorationSidetrackCost, setExplorationSidetrackCost,
        explorationAppraisalWellCost, setExplorationAppraisalWellCost,
        countryOfficeCost, setCountryOfficeCost,
        editModalIsOpen,
        setEditModalIsOpen,
        modalEditMode,
        setModalEditMode,
        modalShouldNavigate,
        setModalShouldNavigate,
        modalCaseId,
        setModalCaseId,
        editMode,
        setEditMode,
        editCase,
        addNewCase,
    }), [
        project,
        setProject,
        createCaseModalIsOpen,
        setCreateCaseModalIsOpen,
        editModalIsOpen,
        setEditModalIsOpen,
        modalEditMode,
        setModalEditMode,
        modalShouldNavigate,
        setModalShouldNavigate,
        modalCaseId,
        setModalCaseId,
        editMode,
        setEditMode,
        editCase,
        addNewCase,
        project, setProject,
        createCaseModalIsOpen, setCreateCaseModalIsOpen,
        activeTab,
        caseItem,
        totalStudyCost,
        opexSum,
        cessationOffshoreFacilitiesCost,
        topside,
        topsideCost,
        surf,
        surfCost,
        substructure,
        substructureCost,
        transport,
        transportCost,
        tableYears,
        totalFeasibilityAndConceptStudies,
        totalFEEDStudies,
        totalOtherStudies,
        additionalOPEXCostProfile,
        historicCostCostProfile,
        productionAndSalesVolume,
        oilCondensateProduction,
        nglProduction,
        NetSalesGas,
        cO2Emissions,
        importedElectricity,
        drillingCost,
        exploration,
        totalExplorationCost,
        explorationWellCostProfile,
        gAndGAdminCost,
        countryOfficeCost,
        explorationAppraisalWellCost,
        explorationSidetrackCost,
        seismicAcqAndProcCost
    ]);

    return (
        <AppContext.Provider value={value}>
            {children}
        </AppContext.Provider>
    );
};

export const useAppContext = (): AppContextType => {
    const context = useContext(AppContext);
    if (context === undefined) {
        throw new Error("useAppContext must be used within an AppContextProvider");
    }
    return context;
};

export { AppContextProvider };
