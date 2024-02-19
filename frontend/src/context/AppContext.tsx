import React, {
    createContext,
    useState,
    ReactNode,
    useContext,
    useMemo,
} from "react";
import { ITimeSeries } from "../models/ITimeSeries";


// Assuming Components.Schemas.* are defined elsewhere
// Replace Components.Schemas.* with the correct type or interface for your project

interface AppContextType {
    project: Components.Schemas.ProjectDto | undefined;
    setProject: React.Dispatch<React.SetStateAction<Components.Schemas.ProjectDto | undefined>>;
    caseItem: Components.Schemas.CaseDto | undefined;
    setCase: React.Dispatch<React.SetStateAction<Components.Schemas.CaseDto | undefined>>;
    activeTab: number;
    setActiveTab: React.Dispatch<React.SetStateAction<number>>;
    createCaseModalIsOpen: boolean;
    setCreateCaseModalIsOpen: React.Dispatch<React.SetStateAction<boolean>>;

    // OPEX
    totalStudyCost: ITimeSeries | undefined;
    setTotalStudyCost: React.Dispatch<React.SetStateAction<ITimeSeries | undefined>>;
    opexSum: Components.Schemas.OpexCostProfileDto | undefined;
    setOpexSum: React.Dispatch<React.SetStateAction<Components.Schemas.OpexCostProfileDto | undefined>>;
    // cessationCost: Components.Schemas.SurfCessationCostProfileDto | undefined;
    // setCessationCost: React.Dispatch<React.SetStateAction<Components.Schemas.SurfCessationCostProfileDto | undefined>>;

    // CAPEX
    topside: Components.Schemas.TopsideDto | undefined;
    setTopside: React.Dispatch<React.SetStateAction<Components.Schemas.TopsideDto | undefined>>;
    topsideCost: Components.Schemas.TopsideCostProfileDto | undefined;
    setTopsideCost: React.Dispatch<React.SetStateAction<Components.Schemas.TopsideCostProfileDto | undefined>>;
    surf: Components.Schemas.SurfDto | undefined;
    setSurf: React.Dispatch<React.SetStateAction<Components.Schemas.SurfDto | undefined>>;
    surfCost: Components.Schemas.SurfCostProfileDto | undefined;
    setSurfCost: React.Dispatch<React.SetStateAction<Components.Schemas.SurfCostProfileDto | undefined>>;
    substructure: Components.Schemas.SubstructureDto | undefined;
    setSubstructure: React.Dispatch<React.SetStateAction<Components.Schemas.SubstructureDto | undefined>>;
    substructureCost: Components.Schemas.SubstructureCostProfileDto | undefined;
    setSubstructureCost: React.Dispatch<React.SetStateAction<Components.Schemas.SubstructureCostProfileDto | undefined>>;
    transport: Components.Schemas.TransportDto | undefined;
    setTransport: React.Dispatch<React.SetStateAction<Components.Schemas.TransportDto | undefined>>;
    transportCost: Components.Schemas.TransportCostProfileDto | undefined;
    setTransportCost: React.Dispatch<React.SetStateAction<Components.Schemas.TransportCostProfileDto | undefined>>;
    setStartYear: React.Dispatch<React.SetStateAction<number | undefined>>;
    setEndYear: React.Dispatch<React.SetStateAction<number | undefined>>;
    tableYears: [number, number];
    setTableYears: React.Dispatch<React.SetStateAction<[number, number]>>;
    drillingCost: Components.Schemas.drillingCostDto | undefined;
    setDrillingCost: React.Dispatch<React.SetStateAction<Components.Schemas.drillingCostDto | undefined>>;
    cessationOffshoreFacilitiesCost: Components.Schemas.CessationOffshoreFacilitiesCostDto | undefined;
    setCessationOffshoreFacilitiesCost: React.Dispatch<React.SetStateAction<Components.Schemas.CessationOffshoreFacilitiesCostDto | undefined>>;

    explorationCost: Components.Schemas.ExplorationWellCostProfileDto | undefined;
    setExplorationCost: React.Dispatch<React.SetStateAction<Components.Schemas.ExplorationWellCostProfileDto | undefined>>;

    feasibilityAndConceptStudiesCost: Components.Schemas.TotalFeasibilityAndConceptStudiesDto | undefined;
    setFeasibilityAndConceptStudiesCost: React.Dispatch<React.SetStateAction<Components.Schemas.TotalFeasibilityAndConceptStudiesDto | undefined>>;
    feedStudiesCost: Components.Schemas.TotalFEEDStudiesDto | undefined;
    setFEEDStudiesCost: React.Dispatch<React.SetStateAction<Components.Schemas.TotalFEEDStudiesDto | undefined>>;


    productionAndSalesVolume: Components.Schemas.ProductionAndSalesVolumesDto | undefined;
    setProductionAndSalesVolume: React.Dispatch<React.SetStateAction<Components.Schemas.ProductionAndSalesVolumesDto | undefined>>;
    oilCondensateProduction: Components.Schemas.ProductionProfileOilDto | undefined;
    setOilCondensateProduction: React.Dispatch<React.SetStateAction<Components.Schemas.ProductionProfileOilDto | undefined>>;
    nglProduction: Components.Schemas.ProductionProfileNGLDto | undefined;
    setNGLProduction: React.Dispatch<React.SetStateAction<Components.Schemas.ProductionProfileNGLDto | undefined>>;
    NetSalesGas: Components.Schemas.NetSalesGasDto | undefined;
    setNetSalesGas: React.Dispatch<React.SetStateAction<Components.Schemas.NetSalesGasDto | undefined>>;
    cO2Emissions: Components.Schemas.Co2EmissionsDto | undefined;
    setCO2Emissions: React.Dispatch<React.SetStateAction<Components.Schemas.Co2EmissionsDto | undefined>>;
    importedElectricity: Components.Schemas.ImportedElectricityDto | undefined;
    setImportedElectricity: React.Dispatch<React.SetStateAction<Components.Schemas.ImportedElectricityDto | undefined>>;
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

    const [topside, setTopside] = useState<Components.Schemas.TopsideDto | undefined>();
    const [topsideCost, setTopsideCost] = useState<Components.Schemas.TopsideCostProfileDto | undefined>();
    const [surf, setSurf] = useState<Components.Schemas.SurfDto | undefined>();
    const [surfCost, setSurfCost] = useState<Components.Schemas.SurfCostProfileDto | undefined>();
    const [substructure, setSubstructure] = useState<Components.Schemas.SubstructureDto | undefined>();
    const [substructureCost, setSubstructureCost] = useState<Components.Schemas.SubstructureCostProfileDto | undefined>();
    const [transport, setTransport] = useState<Components.Schemas.TransportDto | undefined>();
    const [transportCost, setTransportCost] = useState<Components.Schemas.TransportCostProfileDto | undefined>();

    const [startYear, setStartYear] = useState<number | undefined>();
    const [endYear, setEndYear] = useState<number | undefined>();
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030]);

    const [feasibilityAndConceptStudiesCost, setFeasibilityAndConceptStudiesCost] = useState<Components.Schemas.TotalFeasibilityAndConceptStudiesDto | undefined>();
    const [feedStudiesCost, setFEEDStudiesCost] = useState<Components.Schemas.TotalFEEDStudiesDto | undefined>();

    const [productionAndSalesVolume, setProductionAndSalesVolume] = useState<Components.Schemas.ProductionAndSalesVolumesDto | undefined>();
    const [oilCondensateProduction, setOilCondensateProduction] = useState<Components.Schemas.ProductionProfileOilDto | undefined>();
    const [nglProduction, setNGLProduction] = useState<Components.Schemas.ProductionProfileNGLDto | undefined>();
    const [NetSalesGas, setNetSalesGas] = useState<Components.Schemas.NetSalesGasDto | undefined>();
    const [cO2Emissions, setCO2Emissions] = useState<Components.Schemas.Co2EmissionsDto | undefined>();
    const [importedElectricity, setImportedElectricity] = useState<Components.Schemas.ImportedElectricityDto | undefined>();
    const [drillingCost, setDrillingCost] = useState<Components.Schemas.drillingCostDto>();
    const [explorationCost, setExplorationCost] = useState<Components.Schemas.ExplorationWellCostProfileDto | undefined>();
    const [cessationOffshoreFacilitiesCost, setCessationOffshoreFacilitiesCost] = useState<Components.Schemas.CessationOffshoreFacilitiesCostDto>();


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
        feasibilityAndConceptStudiesCost, setFeasibilityAndConceptStudiesCost,
        feedStudiesCost, setFEEDStudiesCost,
        productionAndSalesVolume, setProductionAndSalesVolume,
        oilCondensateProduction, setOilCondensateProduction,
        nglProduction, setNGLProduction,
        NetSalesGas, setNetSalesGas,
        cO2Emissions, setCO2Emissions,
        importedElectricity, setImportedElectricity,
        drillingCost, setDrillingCost,
        explorationCost, setExplorationCost,
    }), [
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
        feasibilityAndConceptStudiesCost,
        feedStudiesCost,
        productionAndSalesVolume,
        oilCondensateProduction,
        nglProduction,
        NetSalesGas,
        cO2Emissions,
        importedElectricity,
        drillingCost,
        explorationCost,
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
