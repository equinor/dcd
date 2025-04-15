import Grid from "@mui/material/Grid2"
import {
    useState, useEffect,
} from "react"

import CaseTableWithGrouping from "@/Components/CaseTabs/CaseSummaryTab/CaseTableWithGrouping"
import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"
import CaseSummarySkeleton from "@/Components/LoadingSkeletons/CaseSummarySkeleton"
import { useDataFetch, useCaseApiData } from "@/Hooks"
import { useCaseMutation } from "@/Hooks/Mutations"
import {
    ITimeSeries,
    ITimeSeriesDataWithGroup,
    ITimeSeriesData,
} from "@/Models/ITimeSeries"
import { useCaseStore } from "@/Store/CaseStore"
import { getYearFromDateString } from "@/Utils/DateUtils"
import { formatCurrencyUnit } from "@/Utils/FormatingUtils"
import { mergeTimeseriesList, setSummaryTableYearsFromProfiles } from "@/Utils/TableUtils"

const CaseSummaryTab = (): React.ReactNode => {
    const { activeTabCase } = useCaseStore()
    const revisionAndProjectData = useDataFetch()
    const { apiData } = useCaseApiData()
    const { updateNpvOverride, updateBreakEvenOverride } = useCaseMutation()

    const DEFAULT_SUMMARY_YEARS = [2023, 2033] as [number, number]

    const [tableYears, setTableYears] = useState<[number, number]>(DEFAULT_SUMMARY_YEARS)
    const [allTimeSeriesData, setAllTimeSeriesData] = useState<ITimeSeriesData[][]>([])
    const [, setYearRangeSetFromProfiles] = useState<boolean>(false)

    const handleOffshoreFacilitiesCost = (): ITimeSeries | undefined => mergeTimeseriesList([
        (apiData?.surfCostProfileOverride?.override === true
            ? apiData?.surfCostProfileOverride
            : apiData?.surfCostProfile),
        (apiData?.substructureCostProfileOverride?.override === true
            ? apiData?.substructureCostProfileOverride
            : apiData?.substructureCostProfile),
        (apiData?.transportCostProfileOverride?.override === true
            ? apiData?.transportCostProfileOverride
            : apiData?.transportCostProfile),
        (apiData?.topsideCostProfileOverride?.override === true
            ? apiData?.topsideCostProfileOverride
            : apiData?.topsideCostProfile),
    ])

    const handleOffshoreOpexPlussWellIntervention = (): ITimeSeries | undefined => mergeTimeseriesList([
        (apiData?.wellInterventionCostProfileOverride?.override === true
            ? apiData?.wellInterventionCostProfileOverride
            : apiData?.wellInterventionCostProfile),
        (apiData?.offshoreFacilitiesOperationsCostProfileOverride?.override === true
            ? apiData?.offshoreFacilitiesOperationsCostProfileOverride
            : apiData?.offshoreFacilitiesOperationsCostProfile),
    ])

    const handleTotalExplorationCost = (): ITimeSeries | undefined => mergeTimeseriesList([
        apiData?.explorationWellCostProfile,
        apiData?.appraisalWellCostProfile,
        apiData?.sidetrackCostProfile,
        apiData?.seismicAcquisitionAndProcessing,
        apiData?.countryOfficeCost,
        apiData?.gAndGAdminCost,
        apiData?.projectSpecificDrillingCostProfile,
        (apiData?.explorationRigMobDemobOverride?.override === true
            ? apiData?.explorationRigMobDemobOverride
            : apiData?.explorationRigMobDemob),
        (apiData?.explorationRigUpgradingCostProfileOverride?.override === true
            ? apiData?.explorationRigUpgradingCostProfileOverride
            : apiData?.explorationRigUpgradingCostProfile),
    ])

    const handleDrilling = (): ITimeSeries | undefined => {
        if (revisionAndProjectData) {
            const oilProducerCostProfile = apiData?.oilProducerCostProfileOverride?.override
                ? apiData.oilProducerCostProfileOverride
                : apiData?.oilProducerCostProfile

            const gasProducerCostProfile = apiData?.gasProducerCostProfileOverride?.override
                ? apiData.gasProducerCostProfileOverride
                : apiData?.gasProducerCostProfile

            const waterInjectorCostProfile = apiData?.waterInjectorCostProfileOverride?.override
                ? apiData.waterInjectorCostProfileOverride
                : apiData?.waterInjectorCostProfile

            const gasInjectorCostProfile = apiData?.gasInjectorCostProfileOverride?.override
                ? apiData.gasInjectorCostProfileOverride
                : apiData?.gasInjectorCostProfile

            const developmentRigUpgrading = apiData?.developmentRigUpgradingCostProfileOverride?.override
                ? apiData.developmentRigUpgradingCostProfileOverride
                : apiData?.developmentRigUpgradingCostProfile

            const developmentRigMobDemob = apiData?.developmentRigMobDemobOverride?.override
                ? apiData.developmentRigMobDemobOverride
                : apiData?.developmentRigMobDemob

            const drillingCostSeriesList: (ITimeSeries | undefined)[] = [
                oilProducerCostProfile,
                gasProducerCostProfile,
                waterInjectorCostProfile,
                gasInjectorCostProfile,
                developmentRigUpgrading,
                developmentRigMobDemob,
            ]

            return mergeTimeseriesList(drillingCostSeriesList)
        }

        return undefined
    }

    useEffect(() => {
        if (activeTabCase === 7 && apiData) {
            const caseData = apiData?.case

            setSummaryTableYearsFromProfiles(
                [
                    handleTotalExplorationCost(),
                    handleDrilling(),
                    handleOffshoreFacilitiesCost(),
                    apiData.cessationOffshoreFacilitiesCostOverride,
                    apiData.cessationOffshoreFacilitiesCost,
                    apiData.cessationOnshoreFacilitiesCostProfile,
                    apiData.totalFeasibilityAndConceptStudiesOverride,
                    apiData.totalFeasibilityAndConceptStudies,
                    apiData.totalFeedStudiesOverride,
                    apiData.totalFeedStudies,
                    apiData.totalOtherStudiesCostProfile,
                    apiData.historicCostCostProfile,
                    handleOffshoreOpexPlussWellIntervention(),
                    apiData.onshoreRelatedOpexCostProfile,
                    apiData.additionalOpexCostProfile,
                    apiData.onshorePowerSupplyCostProfile,
                ],
                getYearFromDateString(caseData.dg4Date),
                setTableYears,
                DEFAULT_SUMMARY_YEARS,
            )
            setYearRangeSetFromProfiles(true)
        }
    }, [activeTabCase, apiData, revisionAndProjectData])

    useEffect(() => {
        if (apiData || revisionAndProjectData) {
            const totalExplorationCostData = handleTotalExplorationCost()
            const totalDrillingCostData = handleDrilling()
            const offshoreFacilitiesCostData = handleOffshoreFacilitiesCost()
            const projectSpecificDrillingCostData = apiData?.projectSpecificDrillingCostProfile
            const explorationRigUpgradingCostData = apiData?.explorationRigUpgradingCostProfile
            const explorationRigUpgradingCostDataOverride = apiData?.explorationRigUpgradingCostProfileOverride
            const explorationRigMobDemobCostData = apiData?.explorationRigMobDemobOverride
            const explorationRigMobDemobCostDataOverride = apiData?.explorationRigMobDemobOverride
            const cessationOffshoreFacilitiesCostOverrideData = apiData?.cessationOffshoreFacilitiesCostOverride
            const cessationOffshoreFacilitiesCostData = apiData?.cessationOffshoreFacilitiesCost
            const cessationOnshoreFacilitiesCostProfileData = apiData?.cessationOnshoreFacilitiesCostProfile
            const totalFeasibilityAndConceptStudiesOverrideData = apiData?.totalFeasibilityAndConceptStudiesOverride
            const totalFeasibilityAndConceptStudiesData = apiData?.totalFeasibilityAndConceptStudies
            const totalFeedStudiesOverrideData = apiData?.totalFeedStudiesOverride
            const totalFeedStudiesData = apiData?.totalFeedStudies
            const totalOtherStudiesCostProfileData = apiData?.totalOtherStudiesCostProfile
            const historicCostCostProfileData = apiData?.historicCostCostProfile
            const offshoreOpexPlussWellInterventionData = handleOffshoreOpexPlussWellIntervention()
            const onshoreRelatedOPEXCostProfileData = apiData?.onshoreRelatedOpexCostProfile
            const additionalOPEXCostProfileData = apiData?.additionalOpexCostProfile
            const onshorePowerSupplyCostProfileData = apiData?.onshorePowerSupplyCostProfile

            const newExplorationTimeSeriesData: ITimeSeriesDataWithGroup[] = [
                {
                    profileName: "Exploration cost",
                    unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                    profile: totalExplorationCostData,
                    group: "Exploration",
                },
                {
                    profileName: "project specific drilling cost",
                    unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                    profile: projectSpecificDrillingCostData,
                    group: "Exploration",
                },
                {
                    profileName: "rig upgrade",
                    unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                    profile: explorationRigUpgradingCostDataOverride?.override ? explorationRigUpgradingCostDataOverride : explorationRigUpgradingCostData,
                    group: "Exploration",
                },
                {
                    profileName: "rig mob/demob",
                    unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                    profile: explorationRigMobDemobCostDataOverride?.override ? explorationRigMobDemobCostDataOverride : explorationRigMobDemobCostData,
                    group: "Exploration",
                },
            ]

            const newCapexTimeSeriesData: ITimeSeriesDataWithGroup[] = [
                {
                    profileName: "Drilling",
                    unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                    profile: totalDrillingCostData,
                    group: "CAPEX",
                },
                {
                    profileName: "Offshore facilities",
                    unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                    profile: offshoreFacilitiesCostData,
                    group: "CAPEX",
                },
                {
                    profileName: "Cessation - offshore facilities",
                    unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                    profile: cessationOffshoreFacilitiesCostOverrideData?.override ? cessationOffshoreFacilitiesCostOverrideData : cessationOffshoreFacilitiesCostData,
                    group: "CAPEX",
                },
                {
                    profileName: "Cessation - onshore facilities",
                    unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                    profile: cessationOnshoreFacilitiesCostProfileData,
                    group: "CAPEX",
                },
                {
                    profileName: "Onshore (Power from shore)",
                    unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                    profile: onshorePowerSupplyCostProfileData,
                    group: "CAPEX",
                },
            ]

            const newStudycostTimeSeriesData: ITimeSeriesDataWithGroup[] = [
                {
                    profileName: "Feasibility & Conceptual studies",
                    unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                    profile: totalFeasibilityAndConceptStudiesOverrideData?.override ? totalFeasibilityAndConceptStudiesOverrideData : totalFeasibilityAndConceptStudiesData,
                    group: "Study cost",
                },
                {
                    profileName: "FEED studies (DG2-DG3)",
                    unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                    profile: totalFeedStudiesOverrideData?.override ? totalFeedStudiesOverrideData : totalFeedStudiesData,
                    group: "Study cost",
                },
                {
                    profileName: "Other studies",
                    unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                    profile: totalOtherStudiesCostProfileData,
                    group: "Study cost",
                },
            ]

            const newOpexTimeSeriesData: ITimeSeriesDataWithGroup[] = [
                {
                    profileName: "Historic cost",
                    unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                    profile: historicCostCostProfileData,
                    group: "OPEX",
                },
                {
                    profileName: "Offshore related OPEX, incl. well intervention",
                    unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                    profile: offshoreOpexPlussWellInterventionData,
                    group: "OPEX",
                },
                {
                    profileName: "Onshore related OPEX",
                    unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                    profile: onshoreRelatedOPEXCostProfileData,
                    group: "OPEX",
                },
                {
                    profileName: "Additional OPEX",
                    unit: formatCurrencyUnit(revisionAndProjectData?.commonProjectAndRevisionData.currency),
                    profile: additionalOPEXCostProfileData,
                    group: "OPEX",
                },
            ]

            setAllTimeSeriesData([
                newExplorationTimeSeriesData,
                newCapexTimeSeriesData,
                newStudycostTimeSeriesData,
                newOpexTimeSeriesData,
            ])
        }
    }, [tableYears])

    const caseData = apiData?.case

    if (!caseData) { return <CaseSummarySkeleton /> }

    if (activeTabCase !== 7) { return null }

    return (
        <Grid container spacing={2}>
            <Grid container size={12} justifyContent="flex-start">
                <Grid container size={{ xs: 12, md: 8, lg: 6 }} spacing={2}>
                    <Grid size={{ xs: 12, md: 6 }}>
                        <SwitchableNumberInput
                            label="NPV before tax (MUSD)"
                            value={caseData.npv ? Number(caseData.npv.toFixed(2)) : undefined}
                            id={`case-npv-${caseData.caseId}`}
                            integer={false}
                            disabled
                            onSubmit={(): void => {}}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, md: 6 }}>
                        <SwitchableNumberInput
                            label="STEA NPV after tax(MUSD)"
                            value={caseData.npvOverride ? Number(caseData.npvOverride.toFixed(2)) : undefined}
                            id={`case-npv-override-${caseData.caseId}`}
                            integer={false}
                            min={0}
                            max={1_000_000}
                            onSubmit={(newValue: number): Promise<void> => updateNpvOverride(newValue)}
                        />
                    </Grid>
                </Grid>
            </Grid>

            <Grid container size={12} justifyContent="flex-start">
                <Grid container size={{ xs: 12, md: 8, lg: 6 }} spacing={2}>
                    <Grid size={{ xs: 12, md: 6 }}>
                        <SwitchableNumberInput
                            label="B/E before tax (USD/bbl)"
                            value={caseData.breakEven ? Number(caseData.breakEven.toFixed(2)) : undefined}
                            id={`case-break-even-${caseData.caseId}`}
                            integer={false}
                            disabled
                            onSubmit={(): void => {}}
                        />
                    </Grid>
                    <Grid size={{ xs: 12, md: 6 }}>
                        <SwitchableNumberInput
                            label="STEA B/E after tax(MUSD)"
                            value={caseData.breakEvenOverride ? Number(caseData.breakEvenOverride.toFixed(2)) : undefined}
                            id={`case-break-even-override-${caseData.caseId}`}
                            integer={false}
                            allowNegative
                            min={0}
                            max={1_000_000}
                            onSubmit={(newValue: number): Promise<void> => updateBreakEvenOverride(newValue)}
                        />
                    </Grid>
                </Grid>
            </Grid>

            <Grid size={12}>
                <CaseTableWithGrouping
                    allTimeSeriesData={allTimeSeriesData}
                    dg4Year={getYearFromDateString(caseData.dg4Date)}
                    tableYears={tableYears}
                    includeFooter={false}
                    decimalPrecision={4}
                />
            </Grid>
        </Grid>
    )
}

export default CaseSummaryTab
