import {
    ChangeEventHandler, Dispatch, SetStateAction, useEffect, useState,
} from "react"
import styled from "styled-components"

import { Typography } from "@equinor/eds-core-react"
import CaseNumberInput from "./CaseNumberInput"
import CaseTabTable from "./CaseTabTable"
import { GetGenerateProfileService } from "../../Services/CaseGeneratedProfileService"
import { MergeTimeseries } from "../../Utils/common"
import { ITimeSeries } from "../../Models/ITimeSeries"
import { useAppContext } from "../../Context/AppContext"
import { ITimeSeriesCost } from "../../Models/ITimeSeriesCost"
import InputContainer from "../Input/InputContainer"
import { AgGridReact } from 'ag-grid-react';
import { ColDef, ColGroupDef } from "ag-grid-community"

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
        feasibilityAndConceptStudiesCost, setFeasibilityAndConceptStudiesCost,
        feedStudiesCost, setFEEDStudiesCost,
        activeTab, setActiveTab,
        explorationCost, setExplorationCost,
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
        tableYears, setTableYears
    } = useAppContext();


    
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

                        if (caseItem.totalFeasibilityAndConceptStudiesOverride?.override === true) {
                            feasibility = caseItem.totalFeasibilityAndConceptStudiesOverride
                        }
                        if (caseItem.totalFEEDStudiesOverride?.override === true) {
                            feed = caseItem.totalFEEDStudiesOverride
                        }

                        const totalStudy = MergeTimeseries(feasibility, feed)
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

                        setTableYearsFromProfiles([
                            totalStudy, opex, cessation,
                            topsideCostProfile, surfCostProfile, substructureCostProfile, transportCostProfile,
                        ])
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
        profile: ITimeSeries | undefined
    }

    const explorationTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Exploration cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: explorationCost,
        },

    ]

    const capexTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Drilling",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: drillingCost,
        },
        // {
        //     profileName: "Offshore facliities",
        //     unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
        //     profile: cessation,
        // },
        // {
        //     profileName: "Cessation",
        //     unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
        //     profile: cessation,
        // },

    ]

    const studycostTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Feasibility & Conceptual studies",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: feasibilityAndConceptStudiesCost,
        },
        {
            profileName: "FEED studies (DG2-DG3",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: feedStudiesCost,
        },
        // {
        //     profileName: "Other studies",
        //     unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
        //     profile: otherStudiesCost,
        // },

    ]

    const opexTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Study cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalStudyCost,
        },
        {
            profileName: "Offshore facliities operations + well intervention",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: opexSum,
        },
        {
            profileName: "Cessation wells + Cessation offshore facilities",
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

    if (activeTab !== 7 || !caseItem) { return null }
    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Summary</PageTitle>
            </TopWrapper>

            <InputContainer mobileColumns={1} desktopColumns={2} breakPoint={850}>
                <CaseNumberInput
                    onChange={handleCaseNPVChange}
                    defaultValue={caseItem.npv}
                    integer={false}
                    label="NPV before tax"
                    allowNegative
                />
                <CaseNumberInput
                    onChange={handleCaseBreakEvenChange}
                    defaultValue={caseItem.breakEven}
                    integer={false}
                    label="B/E before tax"
                />
            </InputContainer>
            <TableWrapper>

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