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
    // OPEX
    totalStudyCost: ITimeSeries | undefined;
    setTotalStudyCost: React.Dispatch<React.SetStateAction<ITimeSeries | undefined>>;
    opexCost: Components.Schemas.OpexCostProfileDto | undefined;
    setOpexCost: React.Dispatch<React.SetStateAction<Components.Schemas.OpexCostProfileDto | undefined>>;
    cessationCost: Components.Schemas.SurfCessationCostProfileDto | undefined;
    setCessationCost: React.Dispatch<React.SetStateAction<Components.Schemas.SurfCessationCostProfileDto | undefined>>;
    
    // CAPEX
    topsideCost: Components.Schemas.TopsideCostProfileDto | undefined;
    setTopsideCost: React.Dispatch<React.SetStateAction<Components.Schemas.TopsideCostProfileDto | undefined>>;
    surfCost: Components.Schemas.SurfCostProfileDto | undefined;
    setSurfCost: React.Dispatch<React.SetStateAction<Components.Schemas.SurfCostProfileDto | undefined>>;
    substructureCost: Components.Schemas.SubstructureCostProfileDto | undefined;
    setSubstructureCost: React.Dispatch<React.SetStateAction<Components.Schemas.SubstructureCostProfileDto | undefined>>;
    transportCost: Components.Schemas.TransportCostProfileDto | undefined;
    setTransportCost: React.Dispatch<React.SetStateAction<Components.Schemas.TransportCostProfileDto | undefined>>;
    startYear: number | undefined;
    setStartYear: React.Dispatch<React.SetStateAction<number | undefined>>;
    endYear: number | undefined;
    setEndYear: React.Dispatch<React.SetStateAction<number | undefined>>;
    tableYears: [number, number];
    setTableYears: React.Dispatch<React.SetStateAction<[number, number]>>;

    feasibilityAndConceptStudies: ITimeSeries | undefined;
    setFeasibilityAndConceptStudies: React.Dispatch<React.SetStateAction<ITimeSeries | undefined>>;
    feedStudies: ITimeSeries | undefined;
    setFEEDStudies: React.Dispatch<React.SetStateAction<ITimeSeries | undefined>>;
    otherStudies: any;
    setOtherStudies: React.Dispatch<React.SetStateAction<any>>;
    opexSum: any[];
    setOpexSum: React.Dispatch<React.SetStateAction<any[]>>;
    historicCost: any;
    setHistoricCost: React.Dispatch<React.SetStateAction<any>>;
    offshoreRelatedOpexInclWellIntervention: any;
    setOffshoreRelatedOpexInclWellIntervention: React.Dispatch<React.SetStateAction<any>>;
    onshoreRelatedOpex: any;
    setOnshoreRelatedOpex: React.Dispatch<React.SetStateAction<any>>;
    productionAndSalesVolume: any[];
    setProductionAndSalesVolume: React.Dispatch<React.SetStateAction<any[]>>;
    oilCondensateProduction: any;
    setOilCondensateProduction: React.Dispatch<React.SetStateAction<any>>;
    nglProduction: any;
    setNGLProduction: React.Dispatch<React.SetStateAction<any>>;
    salesGas: any;
    setSalesGas: React.Dispatch<React.SetStateAction<any>>;
    gasImport: any;
    setGasImport: React.Dispatch<React.SetStateAction<any>>;
    cO2Emissions: any;
    setCO2Emissions: React.Dispatch<React.SetStateAction<any>>;
    importedElectricity: any;
    setImportedElectricity: React.Dispatch<React.SetStateAction<any>>;
    defferedOilProfileMSm3: any;
    setDefferedOilProfileMSm3: React.Dispatch<React.SetStateAction<any>>;
    deferralGas: any;
    setDeferralGas: React.Dispatch<React.SetStateAction<any>>;
}

const AppContext = createContext<AppContextType | undefined>(undefined);

const AppContextProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [project, setProject] = useState<Components.Schemas.ProjectDto | undefined>();
    // Initialize the state for each property
    const [totalStudyCost, setTotalStudyCost] = useState<ITimeSeries | undefined>();
    const [opexCost, setOpexCost] = useState<Components.Schemas.OpexCostProfileDto | undefined>();
    const [cessationCost, setCessationCost] = useState<Components.Schemas.SurfCessationCostProfileDto | undefined>();

    const [topsideCost, setTopsideCost] = useState<Components.Schemas.TopsideCostProfileDto | undefined>();
    const [surfCost, setSurfCost] = useState<Components.Schemas.SurfCostProfileDto | undefined>();
    const [substructureCost, setSubstructureCost] = useState<Components.Schemas.SubstructureCostProfileDto | undefined>();
    const [transportCost, setTransportCost] = useState<Components.Schemas.TransportCostProfileDto | undefined>();

    const [startYear, setStartYear] = useState<number>();
    const [endYear, setEndYear] = useState<number>();
    const [tableYears, setTableYears] = useState<[number, number]>([0, 0]);

    const [studyCostSum, setStudyCostSum] = useState<any[]>([]);
    const [feasibilityAndConceptStudies, setFeasibilityAndConceptStudies] = useState<any>();
    const [feedStudies, setFEEDStudies] = useState<any>();
    const [otherStudies, setOtherStudies] = useState<any>();
    const [opexSum, setOpexSum] = useState<any[]>([]);
    const [historicCost, setHistoricCost] = useState<any>();
    const [offshoreRelatedOpexInclWellIntervention, setOffshoreRelatedOpexInclWellIntervention] = useState<any>();
    const [onshoreRelatedOpex, setOnshoreRelatedOpex] = useState<any>();
    const [productionAndSalesVolume, setProductionAndSalesVolume] = useState<any[]>([]);
    const [oilCondensateProduction, setOilCondensateProduction] = useState<any>();
    const [nglProduction, setNGLProduction] = useState<any>();
    const [salesGas, setSalesGas] = useState<any>();
    const [gasImport, setGasImport] = useState<any>();
    const [cO2Emissions, setCO2Emissions] = useState<any>();
    const [importedElectricity, setImportedElectricity] = useState<any>();
    const [defferedOilProfileMSm3, setDefferedOilProfileMSm3] = useState<any>();
    const [deferralGas, setDeferralGas] = useState<any>();

    const value = useMemo(() => ({
        project, setProject,
        totalStudyCost, setTotalStudyCost,
        opexCost, setOpexCost,
        cessationCost, setCessationCost,
        topsideCost, setTopsideCost,
        surfCost, setSurfCost,
        substructureCost, setSubstructureCost,
        transportCost, setTransportCost,
        startYear, setStartYear,
        endYear, setEndYear,
        tableYears, setTableYears,
        studyCostSum, setStudyCostSum,
        feasibilityAndConceptStudies, setFeasibilityAndConceptStudies,
        feedStudies, setFEEDStudies,
        otherStudies, setOtherStudies,
        opexSum, setOpexSum,
        historicCost, setHistoricCost,
        offshoreRelatedOpexInclWellIntervention, setOffshoreRelatedOpexInclWellIntervention,
        onshoreRelatedOpex, setOnshoreRelatedOpex,
        productionAndSalesVolume, setProductionAndSalesVolume,
        oilCondensateProduction, setOilCondensateProduction,
        nglProduction, setNGLProduction,
        salesGas, setSalesGas,
        gasImport, setGasImport,
        cO2Emissions, setCO2Emissions,
        importedElectricity, setImportedElectricity,
        defferedOilProfileMSm3, setDefferedOilProfileMSm3,
        deferralGas, setDeferralGas,
    }), [
        project,
        totalStudyCost,
        opexCost,
        cessationCost,
        topsideCost,
        surfCost,
        substructureCost,
        transportCost,
        startYear,
        endYear,
        tableYears,
        studyCostSum,
        feasibilityAndConceptStudies,
        feedStudies,
        otherStudies,
        opexSum,
        historicCost,
        offshoreRelatedOpexInclWellIntervention,
        onshoreRelatedOpex,
        productionAndSalesVolume,
        oilCondensateProduction,
        nglProduction,
        salesGas,
        gasImport,
        cO2Emissions,
        importedElectricity,
        defferedOilProfileMSm3,
        deferralGas,
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
