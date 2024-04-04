import {
    ChangeEventHandler, Dispatch, SetStateAction, useEffect, useState,
} from "react"
import Grid from "@mui/material/Grid"
import CaseNumberInput from "../../Input/CaseNumberInput"
import CaseTabTable from "../Components/CaseTabTable"
import { ITimeSeries } from "../../../Models/ITimeSeries"
import { GetGenerateProfileService } from "../../../Services/CaseGeneratedProfileService"
import { MergeTimeseriesList } from "../../../Utils/common"
import { ITimeSeriesCost } from "../../../Models/ITimeSeriesCost"
import InputSwitcher from "../../Input/InputSwitcher"
import { useProjectContext } from "../../../Context/ProjectContext"
import { useCaseContext } from "../../../Context/CaseContext"
import CaseTabTableWithGrouping from "../Components/CaseTabTableWithGrouping"
import { ITimeSeriesCostOverride } from "../../../Models/ITimeSeriesCostOverride"
import { useModalContext } from "../../../Context/ModalContext"
import { SetTableYearsFromProfiles } from "../Components/CaseTabTableHelper"

interface Props {
    topside: Components.Schemas.TopsideDto,
    surf: Components.Schemas.SurfDto,
    substructure: Components.Schemas.SubstructureDto,
    transport: Components.Schemas.TransportDto,
}

const CaseSummaryTab = (): React.ReactElement | null => {
    const {
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
        topside, setTopside,
        topsideCost, setTopsideCost,
        surf, setSurf,
        surfCost, setSurfCost,
        substructure, setSubstructure,
        substructureCost, setSubstructureCost,
        transport, setTransport,
        transportCost, setTransportCost,

        // OPEX
        historicCostCostProfile,
        // wellInterventionCostProfile,
        offshoreFacilitiesOperationsCostProfile,
        additionalOPEXCostProfile,

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
    } = useCaseContext()

    const {
        exploration,
        setExploration,
    } = useModalContext()

    const { project } = useProjectContext()
    const {
        projectCase, projectCaseEdited, setProjectCaseEdited, activeTabCase,
    } = useCaseContext()

    // OPEX
    const [totalStudyCost, setTotalStudyCost] = useState<ITimeSeries>()
    const [opexCost, setOpexCost] = useState<Components.Schemas.OpexCostProfileDto>()
    const [cessationCost, setCessationCost] = useState<Components.Schemas.SurfCessationCostProfileDto>()
    const [historicCost] = useState<Components.Schemas.HistoricCostCostProfileDto>()

    // CAPEX
    const [totalDrillingCost, setTotalDrillingCost] = useState<ITimeSeries>()
    const [offshoreFacilitiesCost, setOffshoreFacilitiesCost] = useState<ITimeSeries>()

    const [, setStartYear] = useState<number>(2020)
    const [, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

    const [offshoreOpexPlussWellIntervention, setOffshoreOpexPlussWellIntervention] = useState<ITimeSeries | undefined>()

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
            profile: totalDrillingCost,
            group: "CAPEX",
        },
        {
            profileName: "Offshore facilities",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: offshoreFacilitiesCost,
            group: "CAPEX",
        },
        {
            profileName: "Onshore facilities",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: undefined,
            group: "CAPEX",
        },
        {
            profileName: "Cessation - offshore facilities",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: undefined,
            group: "CAPEX",
        },
        {
            profileName: "Cessation - onshore facilities",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: undefined,
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

    const getTimeSeriesLastYear = (timeSeries: ITimeSeries | undefined): number | undefined => {
        if (timeSeries && timeSeries.startYear && timeSeries.values) {
            return timeSeries.startYear + timeSeries.values.length - 1
        } return undefined
    }

    const handleCaseNPVChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...projectCase }
        newCase.npv = e.currentTarget.value.length > 0 ? Number(e.currentTarget.value) : 0
        newCase ?? setProjectCaseEdited(newCase)
    }

    const handleCaseBreakEvenChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...projectCase }
        newCase.breakEven = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        newCase ?? setProjectCaseEdited(newCase)
    }

    useEffect(() => {
        (async () => {
            try {
                if (projectCase && project && topside && surf && substructure && transport) {
                    if (project && activeTabCase === 7 && projectCase?.id) {
                        const studyWrapper = (await GetGenerateProfileService()).generateStudyCost(project.id, projectCase?.id)
                        const opexWrapper = (await GetGenerateProfileService()).generateOpexCost(project.id, projectCase?.id)
                        const cessationWrapper = (await GetGenerateProfileService()).generateCessationCost(project.id, projectCase?.id)

                        const opex = (await opexWrapper).opexCostProfileDto
                        const cessation = (await cessationWrapper).cessationCostDto

                        let feasibility = (await studyWrapper).totalFeasibilityAndConceptStudiesDto
                        let feed = (await studyWrapper).totalFEEDStudiesDto
                        const totalOtherStudiesLocal = (await studyWrapper).totalOtherStudiesDto

                        if (projectCase?.totalFeasibilityAndConceptStudiesOverride?.override === true) {
                            feasibility = projectCase?.totalFeasibilityAndConceptStudiesOverride
                        }
                        if (projectCase?.totalFEEDStudiesOverride?.override === true) {
                            feed = projectCase?.totalFEEDStudiesOverride
                        }

                        const totalStudy = MergeTimeseriesList([feasibility, feed, totalOtherStudiesLocal])
                        setTotalStudyCost(totalStudy)

                        setOpexCost(opex)
                        setCessationCost(cessation)

                        setOffshoreOpexPlussWellIntervention(MergeTimeseriesList(
                            [projectCase.wellInterventionCostProfileOverride, projectCase.offshoreFacilitiesOperationsCostProfileOverride],
                        ))

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

                        // Exploration costs
                        setTotalExplorationCost(MergeTimeseriesList([
                            explorationWellCostProfile,
                            explorationAppraisalWellCost,
                            explorationSidetrackCost,
                            seismicAcquisitionAndProcessing,
                            countryOfficeCost,
                            // gAndGAdminCost
                        ]))

                        SetTableYearsFromProfiles([
                            totalExplorationCost, totalOtherStudies, totalFeasibilityAndConceptStudies, totalFEEDStudies, historicCostCostProfile, offshoreOpexPlussWellIntervention,
                            additionalOPEXCostProfile,
                            // totalStudy, opex, cessation,
                            // topsideCostProfile, surfCostProfile, substructureCostProfile, transportCostProfile,
                        ], projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030, setStartYear, setEndYear, setTableYears)
                    }
                }
            } catch (error) {
                console.error("[CaseView] Error while generating cost profile", error)
            }
        })()
    }, [activeTabCase])

    if (activeTabCase !== 7) { return null }

    return (
        <Grid container spacing={3}>
            <Grid item xs={12} md={6}>
                <InputSwitcher value={`${projectCase?.npv}`} label="NPV before tax">
                    <CaseNumberInput
                        onChange={handleCaseNPVChange}
                        defaultValue={projectCase?.npv}
                        integer={false}
                        allowNegative
                        min={0}
                        max={1000000}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={6}>
                <InputSwitcher value={`${projectCase?.breakEven}`} label="B/E before tax">
                    <CaseNumberInput
                        onChange={handleCaseBreakEvenChange}
                        defaultValue={projectCase?.breakEven}
                        integer={false}
                        min={0}
                        max={1000000}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12}>
                <CaseTabTableWithGrouping
                    allTimeSeriesData={allTimeSeriesData}
                    dg4Year={projectCase?.dG4Date ? new Date(projectCase.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    includeFooter={false}
                />
            </Grid>
            <Grid item xs={12}>
                <CaseTabTable
                    timeSeriesData={prodAndSalesTimeSeriesData}
                    dg4Year={projectCase?.dG4Date ? new Date(projectCase.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="Production & Sales Volume"
                    includeFooter
                />
            </Grid>
        </Grid>
    )
}

export default CaseSummaryTab
