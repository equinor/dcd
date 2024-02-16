import {
    ChangeEventHandler, Dispatch, SetStateAction, useEffect, useState,
} from "react"
import styled from "styled-components"

import { Typography } from "@equinor/eds-core-react"
import CaseNumberInput from "../../Components/Case/CaseNumberInput"
import CaseTabTable from "./CaseTabTable"
import { ITimeSeries } from "../../models/ITimeSeries"
import { GetGenerateProfileService } from "../../Services/CaseGeneratedProfileService"
import { MergeTimeseries } from "../../Utils/common"
import { ITimeSeriesCost } from "../../models/ITimeSeriesCost"
import { useAppContext } from "../../context/AppContext"
import { createGrid } from 'ag-grid-community';
import { AgGridReact } from "@ag-grid-community/react"
import 'ag-grid-community/styles/ag-grid.css'; // Core grid CSS, required
import 'ag-grid-community/styles/ag-theme-alpine.css'; // Theme CSS, choose a theme

const ColumnWrapper = styled.div`
    display: flex;
    flex-direction: column;
`
const RowWrapper = styled.div`
    display: flex;
    flex-direction: row;
    margin-bottom: 78px;
`
const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    margin-top: 20px;
    margin-bottom: 20px;
`
const PageTitle = styled(Typography)`
    flex-grow: 1;
`
const NumberInputField = styled.div`
    padding-right: 20px;
`
const TableWrapper = styled.div`
    margin-bottom: 50px;
`


const CaseSummaryTab = () => {
    const {
        project,
        caseItem, setCase,
        topside, setTopside,
        topsideCost, setTopsideCost,
        surf, setSurf,
        surfCost, setSurfCost,
        substructure, setSubstructure,
        substructureCost, setSubstructureCost,
        transport, setTransport,
        transportCost, setTransportCost,
        opexSum, setOpexSum,
        cessationCost, setCessationCost,
        feasibilityAndConceptStudies, setFeasibilityAndConceptStudies,
        feedStudies, setFEEDStudies,
        activeTab, setActiveTab,
        explorationCost, setExplorationCost,
        drillingCost, setDrillingCost,
        totalStudyCost, setTotalStudyCost,
        productionAndSalesVolume, setProductionAndSalesVolume,
        oilCondensateProduction, setOilCondensateProduction,
        nglProduction, setNGLProduction,
        salesGas, setSalesGas,
        cO2Emissions, setCO2Emissions,
        importedElectricity, setImportedElectricity,
        setStartYear,
        setEndYear,
        tableYears, setTableYears
    } = useAppContext();

    const [summaryRowData, setSummaryRowData] = useState([]);
    const [productionRowData, setProductionRowData] = useState([]);
    
    const getTimeSeriesLastYear = (timeSeries: ITimeSeries | undefined): number | undefined => {
        if (timeSeries && timeSeries.startYear && timeSeries.values) {
            return timeSeries.startYear + timeSeries.values.length - 1
        } return undefined
    }

    const setTableYearsFromProfiles = (profiles: (ITimeSeries | undefined)[]) => {
        let firstYear = Number.MAX_SAFE_INTEGER
        let lastYear = Number.MIN_SAFE_INTEGER
        profiles.forEach((p) => {
            if (p && p.startYear !== undefined && p.startYear < firstYear) {
                firstYear = p.startYear
            }
            const profileLastYear = getTimeSeriesLastYear(p)
            if (profileLastYear !== undefined && profileLastYear > lastYear) {
                lastYear = profileLastYear
            }
        })
        if (firstYear < Number.MAX_SAFE_INTEGER && lastYear > Number.MIN_SAFE_INTEGER && caseItem?.dG4Date) {
            setStartYear(firstYear + new Date(caseItem.dG4Date).getFullYear())
            setEndYear(lastYear + new Date(caseItem.dG4Date).getFullYear())
            setTableYears([firstYear + new Date(caseItem.dG4Date).getFullYear(), lastYear + new Date(caseItem.dG4Date).getFullYear()])
        }
    }

    useEffect(() => {
        const fetchData = async () => {
            if (activeTab !== 7 || !caseItem?.id || !project) {
                return;
            }
    
            try {
                const studyWrapper = await (await GetGenerateProfileService()).generateStudyCost(project.id, caseItem.id);
                const opexWrapper = await (await GetGenerateProfileService()).generateOpexCost(project.id, caseItem.id);
                const cessationWrapper = await (await GetGenerateProfileService()).generateCessationCost(project.id, caseItem.id);
    
                let opex = await opexWrapper.opexCostProfileDto;
                let cessation = await cessationWrapper.cessationCostDto;
    
                let feasibility = await studyWrapper.totalFeasibilityAndConceptStudiesDto;
                let feed = await studyWrapper.totalFEEDStudiesDto;
    
                if (caseItem.totalFeasibilityAndConceptStudiesOverride?.override === true) {
                    feasibility = caseItem.totalFeasibilityAndConceptStudiesOverride;
                }
                if (caseItem.totalFEEDStudiesOverride?.override === true) {
                    feed = caseItem.totalFEEDStudiesOverride;
                }
    
                const totalStudy = MergeTimeseries(feasibility, feed);
    
                // Assuming each profile can be directly mapped to a row in the grid
                const summaryRowData = [
                    { category: 'Total Study Cost', sum: totalStudy.sum },
                    { category: 'OPEX', sum: opex?.sum },
                    { category: 'Cessation', sum: cessation?.sum },
                    // Add other categories as needed
                ];

                const productionRowData = [
                    { category: 'Production & Sales volume', sum: totalStudy.sum },
                    // Add other categories as needed
                ];
    
                setRowData(rowData); // Update the grid's row data
    
            } catch (error) {
                console.error("[CaseView] Error while fetching data", error);
            }
        };
    
        fetchData();
    }, [activeTab, caseItem, project]);
    

    const handleCaseNPVChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        newCase.npv = e.currentTarget.value.length > 0 ? Number(e.currentTarget.value) : 0
        setCase(newCase as Components.Schemas.CaseDto);

    }

    const handleCaseBreakEvenChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        newCase.breakEven = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        setCase(newCase as Components.Schemas.CaseDto);
    }

    const columnDefs = [
        // Assuming 'groupingField' is the field you want to group by
        { headerName: 'Grouping Field', field: 'groupingField', rowGroup: true, hide: true },
        // Other column definitions as needed
        { headerName: 'Sum', field: 'sum' },
        // Add other fields/columns as necessary
    ];

    const autoGroupColumnDef = {
        headerName: 'Group',
        minWidth: 200,
        // Customize the group column as needed
    };
    

    interface ITimeSeriesData {
        profileName: string
        unit: string,
        set?: Dispatch<SetStateAction<ITimeSeriesCost | undefined>>,
        profile: ITimeSeries | undefined
    }

    const summaryColumnDefs = [
        {
            headerName: 'Summary',
            children: [
                {
                    headerName: 'Exploration Cost',
                    marryChildren: true,
                    children: [
                        // Add columns relevant to Exploration Cost here
                        // Example: { headerName: 'Exploration Detail', field: 'explorationDetail' }
                    ]
                },
                {
                    headerName: 'CAPEX',
                    marryChildren: true,
                    children: [
                        { headerName: 'Drilling', field: 'drilling' },
                        { headerName: 'Offshore facilities', field: 'offshoreFacilities' },
                        { headerName: 'Onshore facilities', field: 'onshoreFacilities' },
                        { headerName: 'Cessation - Offshore facilities', field: 'cessationOffshoreFacilities' },
                        { headerName: 'Cessation - Onshore facilities', field: 'cessationOnshoreFacilities' },
                    ]
                },
                {
                    headerName: 'Study Cost',
                    marryChildren: true,
                    children: [
                        { headerName: 'Feasibility & Conceptual studies', field: 'feasibilityConceptualStudies' },
                        { headerName: 'FEED studies (DG2, DG3)', field: 'feedStudies' },
                        { headerName: 'Other studies', field: 'otherStudies' },
                    ]
                },
                {
                    headerName: 'OPEX',
                    marryChildren: true,
                    children: [
                        { headerName: 'Historic cost', field: 'historicCost' },
                        { headerName: 'Offshore related OPEX incl. well intervention', field: 'offshoreRelatedOPEX' },
                        { headerName: 'Onshore related OPEX', field: 'onshoreRelatedOPEX' },
                    ]
                }
            ]
        }
    ];

    const productionSalesVolumeColumnDefs = [
        { headerName: 'Oil / condensate pr MSm3', field: 'oilCondensatePrMSm3' },
        { headerName: 'NGL production', field: 'nglProduction' },
        { headerName: 'Sales gas', field: 'salesGas' },
        { headerName: 'Gas import', field: 'gasImport' },
        { headerName: 'CO2 emissions', field: 'co2Emissions' },
        { headerName: 'Imported electricity', field: 'importedElectricity' },
        { headerName: 'Deferred oil profile MSm3', field: 'deferredOilProfileMSm3' },
        { headerName: 'Deferred gas (GSm3)', field: 'deferredGasGSm3' }
    ];



    const summaryTableData = [
        {
            profileName: "Exploration Cost",
            unit: "MUSD", // Assuming currency for simplification; replace with dynamic currency if needed
            profile: explorationCost,
            set: setExplorationCost,
        },
        {
            profileName: "CAPEX", 
            subcategories: [
                { profileName: "Drilling", unit: "MUSD", profile: drillingCost, set: setDrillingCost },
                { profileName: "Offshore facilities", unit: "MUSD", profile: topsideCost, set: setTopsideCost },
                { profileName: "Onshore facilities", unit: "MUSD", profile: undefined, set: undefined }, // Placeholder for onshore facilities cost and setter
                { profileName: "Cessation - Offshore facilities", unit: "MUSD", profile: cessationCost, set: setCessationCost },
                { profileName: "Cessation - Onshore facilities", unit: "MUSD", profile: undefined, set: undefined }, // Placeholder for cessation onshore facilities cost and setter
            ],
        },
        {
            profileName: "Study Cost",
            subcategories: [
                { profileName: "Feasibility & Conceptual studies", unit: "MUSD", profile: feasibilityAndConceptStudies, set: setFeasibilityAndConceptStudies },
                { profileName: "FEED studies (DG2, DG3)", unit: "MUSD", profile: feedStudies, set: setFEEDStudies },
                { profileName: "Other studies", unit: "MUSD", profile: undefined, set: undefined }, // Placeholder for other studies cost and setter
            ],
        },
        {
            profileName: "OPEX",
            subcategories: [
                { profileName: "Historic cost", unit: "MUSD", profile: undefined, set: undefined }, // Placeholder for historic cost and setter
                { profileName: "Offshore related OPEX incl. well intervention", unit: "MUSD", profile: opexSum, set: setOpexSum },
                { profileName: "Onshore related OPEX", unit: "MUSD", profile: undefined, set: undefined }, // Placeholder for onshore related OPEX cost and setter
            ],
        },
    ];


    const productionSalesVolumeData = [
        { profileName: "Oil / condensate pr MSm3", unit: "MSm3", profile: oilCondensateProduction, set: setOilCondensateProduction },
        { profileName: "NGL production", unit: "MSm3", profile: nglProduction, set: setNGLProduction },
        { profileName: "Sales gas", unit: "GSm3", profile: salesGas, set: setSalesGas },
        { profileName: "Gas import", unit: "GSm3", profile: undefined, set: undefined }, // Placeholder for gas import data and setter
        { profileName: "CO2 emissions", unit: "Mt", profile: cO2Emissions, set: setCO2Emissions },
        { profileName: "Imported electricity", unit: "GWh", profile: importedElectricity, set: setImportedElectricity },
        { profileName: "Deferred oil profile MSm3", unit: "MSm3", profile: undefined, set: undefined }, // Placeholder for deferred oil profile data and setter
        { profileName: "Deferred gas (GSm3)", unit: "GSm3", profile: undefined, set: undefined }, // Placeholder for deferred gas data and setter
    ];


     return (
<>
    <div className="ag-theme-alpine" style={{ height: 600, width: '100%', marginBottom: '20px' }}>
        <AgGridReact
            columnDefs={summaryColumnDefs}
            rowData={summaryRowData}
            autoGroupColumnDef={{ headerName: "Categories", minWidth: 200 }}
            domLayout='autoHeight'
            groupDisplayType='groupRows'
        />
    </div>
    <div className="ag-theme-alpine" style={{ height: 600, width: '100%' }}>
        <AgGridReact
            columnDefs={productionSalesVolumeColumnDefs}
            rowData={productionRowData}
            domLayout='autoHeight'
        />
    </div>
</>
    );
};

export default CaseSummaryTab;