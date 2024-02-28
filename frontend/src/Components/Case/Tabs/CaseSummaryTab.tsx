import {
    ChangeEventHandler, Dispatch, SetStateAction, useEffect, useState,
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
        NetSalesGas, setNetSalesGas,
        cO2Emissions, setCO2Emissions,
        importedElectricity, setImportedElectricity,
        setStartYear,
        setEndYear,
        tableYears, setTableYears,
        totalExplorationCost, setTotalExplorationCost,
        explorationWellCostProfile, setExplorationWellCostProfile,
        gAndGAdminCost, setGAndGAdminCost,
        seismicAcqAndProcCost, setSeismicAcqAndProcCost,
        explorationSidetrackCost, setExplorationSidetrackCost,
        explorationAppraisalWellCost, setExplorationAppraisalWellCost,
        countryOfficeCost, setCountryOfficeCost,
    } = useAppContext();

    const [columnDefs, setColumnDefs] = useState<ColDef[]>([
        { field: 'profileName', headerName: 'Profile Name', pinned: 'left', minWidth: 200 },
        // Initialize with a set of year columns based on expected range if possible
    ]);

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
                            seismicAcqAndProcCost,
                            countryOfficeCost,
                            gAndGAdminCost,
                        ])

                        //Exploration costs                
                        setTotalExplorationCost(MergeTimeseriesList([
                            explorationWellCostProfile,
                            explorationAppraisalWellCost,
                            explorationSidetrackCost,|
                            seismicAcqAndProcCost,
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
        },

    ]

    const capexTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Drilling",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: drillingCost,
        },

    ]

    const studycostTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Feasibility & Conceptual studies",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalFeasibilityAndConceptStudies,
        },
        {
            profileName: "FEED studies (DG2-DG3",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalFEEDStudies,
        },
        {
            profileName: "Other studies",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalOtherStudies,
        },

    ]

    const opexTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Historic cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: historicCostCostProfile,
        },
        {
            profileName: "Offshore related OPEX, incl. well intervention",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: opexSum,
        },
        {
            profileName: "Onshore related OPEX",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationCost,
        },
    ]

    const prodAndSalesTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Oil / condensate production",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalStudyCost,
        },
        {
            profileName: "NGL production",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: opexSum,
        },
        {
            profileName: "Sales gas",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationCost,
        },
        {
            profileName: "Gas import",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationCost,
        },
        {
            profileName: "CO2 emissions",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationCost,
        },
        {
            profileName: "Imported electricity",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationCost,
        },
        {
            profileName: "Deferred oil profile (MSm3/yr)",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationCost,
        },
        {
            profileName: "Deferreal gas (GSm3/yr)",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationCost,
        },
    ]
    const allTimeSeriesData = [
        explorationTimeSeriesData,
        capexTimeSeriesData,
        studycostTimeSeriesData,
        opexTimeSeriesData,
        prodAndSalesTimeSeriesData
    ];

    function transformToRowData(
        categories: ITimeSeriesData[][],
        rowDataTableYears = tableYears
    ): any[] {
        const rowData: any[] = [];

        categories.forEach(categoryData => {
            categoryData.forEach(dataItem => {
                const { profileName, unit, profile, overrideProfile } = dataItem;

                // Initialize base row structure
                const baseRow: any = { profileName, unit };

                // Function to add profile values to row
                const addProfileValues = (profile: ITimeSeries | undefined, prefix: string = '') => {
                    if (!profile || !profile.values) return;

                    profile.values.forEach((value, index) => {
                        const year = profile.startYear + index;
                        if (year >= tableYears[0] && year <= tableYears[1]) {
                            const yearKey = `${prefix}${year}`;
                            baseRow[yearKey] = value;
                        }
                    });
                };

                // Add original profile values
                addProfileValues(profile);

                // Optionally, add override profile values
                if (overrideProfile) {
                    addProfileValues(overrideProfile, 'override_');
                }

                rowData.push(baseRow);
            });
        });

        return rowData;
    }

    const generateColumnDefsForYears = (allTimeSeriesData: ITimeSeriesData[][]): any[] => {
        let minYear = Infinity;
        let maxYear = -Infinity;

        allTimeSeriesData.forEach(category => {
            category.forEach(item => {
                // Check the startYear and length of values for both profile and overrideProfile
                [item.profile, item.overrideProfile].forEach(profile => {
                    if (profile && profile.startYear && profile.values) {
                        const endYear = profile.startYear + profile.values.length - 1;
                        minYear = Math.min(minYear, profile.startYear);
                        maxYear = Math.max(maxYear, endYear);
                    }
                });
            });
        });

        // Generate columnDefs for each year in the range
        const columnDefs: ColDef[] = [
            { field: 'profileName', headerName: 'Profile Name', pinned: 'left', minWidth: 200 }, // Ensure adequate minimum width
        ];

        for (let year = minYear; year <= maxYear; year++) {
            columnDefs.push({
                field: year.toString(),
                headerName: year.toString(),
                minWidth: 120,
                flex: 1,
            });
        }

        return columnDefs;
    };



    useEffect(() => {
        setRowData(transformToRowData(allTimeSeriesData, tableYears))
        setColumnDefs(generateColumnDefsForYears(allTimeSeriesData))
    }, [allTimeSeriesData, tableYears])

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
                <AgGridReact
                    className="ag-theme-alpine-fusion"
                    rowData={rowData}
                    columnDefs={columnDefs}
                    defaultColDef={{ flex: 1, minWidth: 120, sortable: true, filter: true }} // Adjust minWidth as needed
                    autoGroupColumnDef={{ headerName: "Category", minWidth: 200 }}
                    domLayout="autoHeight"
                    animateRows={true}
                    groupDisplayType={'groupRows'}
                />
            </TableWrapper>
            <TableWrapper>

                <CaseTabTable
                    timeSeriesData={explorationTimeSeriesData}
                    dg4Year={caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="Exploration"
                    includeFooter={false}
                />
            </TableWrapper>
            <TableWrapper>
                <CaseTabTable
                    timeSeriesData={capexTimeSeriesData}
                    dg4Year={caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="CAPEX"
                    includeFooter
                />
            </TableWrapper>
            <TableWrapper>
                <CaseTabTable
                    timeSeriesData={studycostTimeSeriesData}
                    dg4Year={caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="Study cost"
                    includeFooter
                />
            </TableWrapper>
            <TableWrapper>
                <CaseTabTable
                    timeSeriesData={opexTimeSeriesData}
                    dg4Year={caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="OPEX"
                    includeFooter
                />
            </TableWrapper>
            <TableWrapper>
                <CaseTabTable
                    timeSeriesData={prodAndSalesTimeSeriesData}
                    dg4Year={caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="Production & sales volume"
                    includeFooter
                />
            </TableWrapper>
        </>
    )
}

export default CaseSummaryTab