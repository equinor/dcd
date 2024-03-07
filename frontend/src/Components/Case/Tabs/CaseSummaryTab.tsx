import {
    ChangeEventHandler, Dispatch, SetStateAction, useEffect, useState,
} from "react"
import styled from "styled-components"

import { Typography } from "@equinor/eds-core-react"
import { ColDef } from "@ag-grid-community/core"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
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
import CaseTabTableWithGrouping from "../Components/CaseTabTableWithGrouping"
import { SetTableYearsFromProfiles } from "../Components/CaseTabTableHelper"

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
        offshoreFacilitiesOperationsCostProfile, setOffshoreFacilitiesOperationsCostProfile,
        wellInterventionCostProfile, setWellInterventionCostProfile,
        totalFEEDStudiesOverride, setTotalFEEDStudiesOverride,
        totalFeasibilityAndConceptStudiesOverride, setTotalFeasibilityAndConceptStudiesOverride,
    } = useAppContext()

    const [cessationCost, setCessationCost] = useState<Components.Schemas.SurfCessationCostProfileDto>()
    const [offshoreOpexPlussWellIntervention, setOffshoreOpexPlussWellIntervention] = useState<ITimeSeries | undefined>()
    const [wellInterventionCostProfileOverride, setWellInterventionCostProfileOverride] = useState<Components.Schemas.WellInterventionCostProfileOverrideDto>()
    const [offshoreFacilitiesOperationsCostProfileOverride] = useState<Components.Schemas.OffshoreFacilitiesOperationsCostProfileOverrideDto>()

    useEffect(() => {
        (async () => {
            try {
                if (caseItem && project && topside && surf && substructure && transport) {
                    if (activeTab === 7 && caseItem.id) {
                        const studyWrapper = (await GetGenerateProfileService()).generateStudyCost(project.id, caseItem.id)
                        const opexWrapper = (await GetGenerateProfileService()).generateOpexCost(project.id, caseItem.id)
                        const cessationWrapper = (await GetGenerateProfileService()).generateCessationCost(project.id, caseItem.id)

                        const opex = (await opexWrapper).opexCostProfileDto
                        const cessation = (await cessationWrapper).cessationCostDto

                        let feasibility = (await studyWrapper).totalFeasibilityAndConceptStudiesDto
                        let feed = (await studyWrapper).totalFEEDStudiesDto
                        const totalOtherStudiesLocal = (await studyWrapper).totalOtherStudiesDto

                        if (caseItem.totalFeasibilityAndConceptStudiesOverride?.override === true) {
                            feasibility = caseItem.totalFeasibilityAndConceptStudiesOverride
                        }
                        if (caseItem.totalFEEDStudiesOverride?.override === true) {
                            feed = caseItem.totalFEEDStudiesOverride
                        }

                        const totalStudy = MergeTimeseriesList([feasibility, feed, totalOtherStudiesLocal])
                        setTotalStudyCost(totalStudy)
                        setOpexSum(opex)
                        setCessationCost(cessation)

                        setOffshoreOpexPlussWellIntervention(MergeTimeseriesList(
                            [caseItem.wellInterventionCostProfileOverride, caseItem.offshoreFacilitiesOperationsCostProfileOverride]))

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

                        SetTableYearsFromProfiles([caseItem.totalFeasibilityAndConceptStudies, caseItem.totalFEEDStudies,
                        caseItem.wellInterventionCostProfile, caseItem.offshoreFacilitiesOperationsCostProfile,
                        caseItem.cessationWellsCost, caseItem.cessationOffshoreFacilitiesCost,
                        caseItem.totalFeasibilityAndConceptStudiesOverride, caseItem.totalFEEDStudiesOverride,
                        caseItem.wellInterventionCostProfileOverride, caseItem.offshoreFacilitiesOperationsCostProfileOverride,
                        caseItem.cessationWellsCostOverride, caseItem.cessationOffshoreFacilitiesCostOverride,
                            surfCostProfile, topsideCostProfile, substructureCostProfile, transportCostProfile,
                            explorationWellCostProfile,
                            seismicAcquisitionAndProcessing,
                        ], caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030, setStartYear, setEndYear, setTableYears)

                        // Exploration costs
                        setTotalExplorationCost(MergeTimeseriesList([
                            explorationWellCostProfile,
                            explorationAppraisalWellCost,
                            explorationSidetrackCost,
                            seismicAcquisitionAndProcessing,
                            countryOfficeCost,
                            gAndGAdminCost]))
                    }
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
        setCase(newCase as Components.Schemas.CaseDto)
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
            profile: totalFeasibilityAndConceptStudiesOverride?.override ? totalFeasibilityAndConceptStudiesOverride : totalFeasibilityAndConceptStudies,
            group: "Study cost",
        },
        {
            profileName: "FEED studies (DG2-DG3",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalFEEDStudiesOverride?.override ? totalFEEDStudiesOverride : totalFEEDStudies,
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
            profile: offshoreOpexPlussWellIntervention,
            group: "OPEX",
        },
        {
            profileName: "Onshore related OPEX",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: undefined,
            group: "OPEX",
        },
        {
            profileName: "Additional OPEX",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: additionalOPEXCostProfile,
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
    ]

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
                        min={0}
                        max={1000000}
                    />
                </InputSwitcher>

                <InputSwitcher value={`${caseItem.breakEven}`} label="B/E before tax">
                    <CaseNumberInput
                        onChange={handleCaseBreakEvenChange}
                        defaultValue={caseItem.breakEven}
                        integer={false}
                        label="B/E before tax"
                        min={0}
                        max={1000000}
                    />
                </InputSwitcher>
            </InputContainer>
            <TableWrapper>

                <CaseTabTableWithGrouping
                    allTimeSeriesData={allTimeSeriesData}
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
