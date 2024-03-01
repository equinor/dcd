import {
    ChangeEventHandler, Dispatch, SetStateAction, useEffect, useState, useMemo
} from "react"
import styled from "styled-components"

import { Typography } from "@equinor/eds-core-react"
import CaseNumberInput from "../../Input/CaseNumberInput"
import CaseTabTable from "../Components/CaseTabTable"
import { ITimeSeries } from "../../../Models/ITimeSeries"
import { GetGenerateProfileService } from "../../../Services/CaseGeneratedProfileService"
import { MergeTimeseries, MergeTimeseriesList } from "../../../Utils/common"
import { ITimeSeriesCost } from "../../../Models/ITimeSeriesCost"
import InputContainer from "../../Input/Containers/InputContainer"
import InputSwitcher from "../../Input/InputSwitcher"
import { useAppContext } from "../../../Context/AppContext"
import { ITimeSeriesCostOverride } from "../../../Models/ITimeSeriesCostOverride"
import { AgGridReact } from "@ag-grid-community/react"
import { ColDef } from '@ag-grid-community/core';
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import CaseTabTableWithGrouping from "../Components/CaseTabTableWithGrouping"

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
interface ITimeSeriesData {
    profileName: string
    unit: string,
    set?: Dispatch<SetStateAction<ITimeSeriesCost | undefined>>,
    profile: ITimeSeries | undefined
}


const CaseSummaryTab = (): React.ReactElement | null => {
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
        cessationOffshoreFacilitiesCost, setCessationOffshoreFacilitiesCost,
        totalFeasibilityAndConceptStudies, setTotalFeasibilityAndConceptStudies,
        totalFEEDStudies, setTotalFEEDStudies,
        totalOtherStudies,
        historicCostCostProfile,
        additionalOPEXCostProfile,
        activeTab, setActiveTab,
        drillingCost, setDrillingCost,
        totalStudyCost, setTotalStudyCost,
        productionAndSalesVolume, setProductionAndSalesVolume,
        oilCondensateProduction, setOilCondensateProduction,
        nglProduction, setNGLProduction,
        netSalesGas, setNetSalesGas,
        cO2Emissions, setCO2Emissions,
        importedElectricity, setImportedElectricity,
        setStartYear,
        setEndYear,
        tableYears, setTableYears,
        totalExplorationCost, setTotalExplorationCost,
        explorationWellCostProfile, setExplorationWellCostProfile,
        gAndGAdminCost, setGAndGAdminCost,
        seismicAcquisitionAndProcessing, setSeismicAcquisitionAndProcessing,
        explorationSidetrackCost, setExplorationSidetrackCost,
        explorationAppraisalWellCost, setExplorationAppraisalWellCost,
        countryOfficeCost, setCountryOfficeCost,
    } = useAppContext();

    // const [columnDefs, setColumnDefs] = useState<ColDef[]>([
    //     { field: 'profileName', headerName: 'Profile Name', minWidth: 200 },
    //     // Initialize with a set of year columns based on expected range if possible
    // ]);
    const styles = useStyles()

    const [tableName, setTableName] = useState<string>("");
    const [totalRowName, setTotalRowName] = useState<string>("");

    const [rowData, setRowData] = useState<any[]>([]);

    const [cessationCost, setCessationCost] = useState<Components.Schemas.SurfCessationCostProfileDto>()

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
        if (caseItem)
            if (firstYear < Number.MAX_SAFE_INTEGER && lastYear > Number.MIN_SAFE_INTEGER && caseItem.dG4Date) {
                setStartYear(firstYear + new Date(caseItem.dG4Date).getFullYear())
                setEndYear(lastYear + new Date(caseItem.dG4Date).getFullYear())
                setTableYears([firstYear + new Date(caseItem.dG4Date).getFullYear(), lastYear + new Date(caseItem.dG4Date).getFullYear()])
            }
    }

    useEffect(() => {
        (async () => {
            try {
                if (caseItem && project && topside && surf && substructure && transport) //test if this work, if not break into smaller ifs
                    if (activeTab === 7 && caseItem.id) {
                        const studyWrapper = (await GetGenerateProfileService()).generateStudyCost(project.id, caseItem.id)
                        const opexWrapper = (await GetGenerateProfileService()).generateOpexCost(project.id, caseItem.id)
                        const cessationWrapper = (await GetGenerateProfileService()).generateCessationCost(project.id, caseItem.id)

                        const opex = (await opexWrapper).opexCostProfileDto
                        const cessation = (await cessationWrapper).cessationCostDto

                        let feasibility = (await studyWrapper).totalFeasibilityAndConceptStudiesDto
                        let feed = (await studyWrapper).totalFEEDStudiesDto
                        let totalOtherStudies = (await studyWrapper).totalOtherStudiesDto

                        if (caseItem.totalFeasibilityAndConceptStudiesOverride?.override === true) {
                            feasibility = caseItem.totalFeasibilityAndConceptStudiesOverride
                        }
                        if (caseItem.totalFEEDStudiesOverride?.override === true) {
                            feed = caseItem.totalFEEDStudiesOverride
                        }

                        const totalStudy = MergeTimeseriesList([feasibility, feed, totalOtherStudies])
                        setTotalStudyCost(totalStudy)
                        setOpexSum(opex)
                        setCessationCost(cessation)

                        // CAPEX
                        const topsideCostProfile = topside.costProfileOverride?.override
                            ? topside.costProfileOverride : topside.costProfile
                        setTopsideCost(topsideCostProfile)

                        const surfCostProfile = surf.costProfileOverride?.override
                            ? surf.costProfileOverride : surf.costProfile
                        setSurfCost(surfCostProfile)

                        const substructureCostProfile = substructure.costProfileOverride?.override
                            ? substructure.costProfileOverride : substructure.costProfile
                        setSubstructureCost(substructureCostProfile)

                        const transportCostProfile = transport.costProfileOverride?.override
                            ? transport.costProfileOverride : transport.costProfile
                        setTransportCost(transportCostProfile)

                        //ADD ALL
                        setTableYearsFromProfiles([
                            totalStudy, opex, cessation,
                            topsideCostProfile, surfCostProfile, substructureCostProfile, transportCostProfile, explorationWellCostProfile,
                            explorationAppraisalWellCost,
                            explorationSidetrackCost,
                            seismicAcquisitionAndProcessing,
                            countryOfficeCost,
                            gAndGAdminCost,
                        ])

                        //Exploration costs                
                        setTotalExplorationCost(MergeTimeseriesList([
                            explorationWellCostProfile,
                            explorationAppraisalWellCost,
                            explorationSidetrackCost,
                            seismicAcquisitionAndProcessing,
                            countryOfficeCost,
                            gAndGAdminCost]))

                    }
            } catch (error) {
                console.error("[CaseView] Error while generating cost profile", error)
            }
        })()
    }, [activeTab])

    const handleCaseNPVChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        newCase.npv = e.currentTarget.value.length > 0 ? Number(e.currentTarget.value) : 0
        setCase(newCase as Components.Schemas.CaseDto)
    }

    const handleCaseBreakEvenChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        newCase.breakEven = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        setCase(newCase as Components.Schemas.CaseDto);
    }

    interface ITimeSeriesData {
        group?: string
        profileName: string
        unit: string,
        set?: Dispatch<SetStateAction<ITimeSeriesCost | undefined>>,
        overrideProfileSet?: Dispatch<SetStateAction<ITimeSeriesCostOverride | undefined>>,
        profile: ITimeSeries | undefined
        overrideProfile?: ITimeSeries | undefined
        overridable?: boolean
    }

    const explorationTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Exploration cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalExplorationCost,
            group: "Exploration",
        },

    ]

    const capexTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Drilling",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: drillingCost,
            group: "CAPEX",
        },

    ]

    const studycostTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Feasibility & Conceptual studies",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalFeasibilityAndConceptStudies,
            group: "Study cost",
        },
        {
            profileName: "FEED studies (DG2-DG3",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalFEEDStudies,
            group: "Study cost",
        },
        {
            profileName: "Other studies",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalOtherStudies,
            group: "Study cost",
        },

    ]

    const opexTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Historic cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: historicCostCostProfile,
            group: "OPEX",
        },
        {
            profileName: "Offshore related OPEX, incl. well intervention",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: opexSum,
            group: "OPEX",
        },
        {
            profileName: "Onshore related OPEX",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: undefined,
            group: "OPEX",
        },
    ]

    const prodAndSalesTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Oil / condensate production",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: undefined,
        },
        {
            profileName: "NGL production",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: undefined,
        },
        {
            profileName: "Sales gas",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: undefined,
        },
        {
            profileName: "Gas import",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: undefined,
        },
        {
            profileName: "CO2 emissions",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: undefined,
        },
        {
            profileName: "Imported electricity",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: undefined,
        },
        {
            profileName: "Deferred oil profile (MSm3/yr)",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: undefined,
        },
        {
            profileName: "Deferreal gas (GSm3/yr)",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: undefined,
        },
    ]
    
    const allTimeSeriesData = [
        explorationTimeSeriesData,
        capexTimeSeriesData,
        studycostTimeSeriesData,
        opexTimeSeriesData,
        //prodAndSalesTimeSeriesData
    ];


    const containerStyle = useMemo(() => ({ width: '100%', height: '100%' }), []);
    const gridStyle = useMemo(() => ({ height: '100%', width: '100%' }), []);
    const defaultColDef = useMemo<ColDef>(() => {
        return {
            flex: 1,
            minWidth: 100,
        };
    }, []);


    // const [columnDefs, setColumnDefs] = useState<ColDef[]>(generateTableYearColDefsGrouping())
    // useEffect(() => {
    //     const rowData = profilesToRowDataGrouping();
    //     setRowData(rowData);
    //     const newColDefs = generateTableYearColDefsGrouping();
    //     setColumnDefs(newColDefs);
    // }, [allTimeSeriesData, tableYears]);

    if (activeTab !== 7 || !caseItem) { return null }
    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Summary</PageTitle>
            </TopWrapper>

            <InputContainer mobileColumns={1} desktopColumns={2} breakPoint={850}>
                <InputSwitcher value={`${caseItem.npv}`} label="NPV before tax">
                    <CaseNumberInput
                        onChange={handleCaseNPVChange}
                        defaultValue={caseItem.npv}
                        integer={false}
                        label="NPV before tax"
                        allowNegative
                    />
                </InputSwitcher>

                <InputSwitcher value={`${caseItem.breakEven}`} label="B/E before tax">
                    <CaseNumberInput
                        onChange={handleCaseBreakEvenChange}
                        defaultValue={caseItem.breakEven}
                        integer={false}
                        label="B/E before tax"
                    />
                </InputSwitcher>
            </InputContainer>
            <TableWrapper>

                <CaseTabTableWithGrouping
                    allTimeSeriesData={allTimeSeriesData}
                    //timeSeriesData={explorationTimeSeriesData}
                    dg4Year={caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="Summary"
                    includeFooter={false}
                />
            </TableWrapper>
            
            <TableWrapper>
                <CaseTabTable
                    timeSeriesData={prodAndSalesTimeSeriesData}
                    dg4Year={caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="Production & Sales Volume"
                    includeFooter
                />
            </TableWrapper>
        </>
    )
}

export default CaseSummaryTab