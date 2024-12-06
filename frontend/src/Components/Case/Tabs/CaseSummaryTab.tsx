import {
    useState, useEffect,
} from "react"
import Grid from "@mui/material/Grid"
import { useQuery } from "@tanstack/react-query"
import { useParams } from "react-router"
import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"
import {
    ITimeSeries,
    ITimeSeriesDataWithGroup,
    ITimeSeriesData,
} from "@/Models/ITimeSeries"
import { useCaseContext } from "@/Context/CaseContext"
import CaseTabTableWithGrouping from "../Components/CaseTabTableWithGrouping"
import { mergeTimeseriesList } from "@/Utils/common"
import { SetSummaryTableYearsFromProfiles } from "../Components/CaseTabTableHelper"
import CaseSummarySkeleton from "@/Components/LoadingSkeletons/CaseSummarySkeleton"
import { caseQueryFn, projectQueryFn } from "@/Services/QueryFunctions"
import { useProjectContext } from "@/Context/ProjectContext"

const CaseSummaryTab = ({ addEdit }: { addEdit: any }) => {
    const { activeTabCase } = useCaseContext()
    const { caseId } = useParams()
    const { projectId, isRevision } = useProjectContext()
    const { revisionId } = useParams()
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])
    const [allTimeSeriesData, setAllTimeSeriesData] = useState<ITimeSeriesData[][]>([])

    const { data: projectData } = useQuery({
        queryKey: ["projectApiData", projectId],
        queryFn: () => projectQueryFn(projectId),
        enabled: !!projectId,
    })

    const { data: apiData } = useQuery({
        queryKey: ["caseApiData", isRevision ? revisionId : projectId, caseId],
        queryFn: () => caseQueryFn(isRevision ? revisionId ?? "" : projectId, caseId),
        enabled: !!projectId && !!caseId,
    })

    const handleOffshoreFacilitiesCost = () => mergeTimeseriesList([
        (apiData?.surfCostProfileOverride?.override === true
            ? apiData?.surfCostProfileOverride
            : apiData?.surfCostProfile),
        (apiData?.substructureCostProfileOverride?.override === true
            ? apiData?.substructureCostProfileOverride
            : apiData?.substructureCostProfile),
        (apiData?.transportCostProfileOverride?.override === true
            ? apiData?.transportCostProfileOverride
            : apiData?.transportCostProfile),
    ])

    const handleOffshoreOpexPlussWellIntervention = () => mergeTimeseriesList([
        (apiData?.wellInterventionCostProfileOverride?.override === true
            ? apiData?.wellInterventionCostProfileOverride
            : apiData?.wellInterventionCostProfile),
        (apiData?.offshoreFacilitiesOperationsCostProfileOverride?.override === true
            ? apiData?.offshoreFacilitiesOperationsCostProfileOverride
            : apiData?.offshoreFacilitiesOperationsCostProfile),
    ])

    const handleTotalExplorationCost = () => mergeTimeseriesList([
        apiData?.explorationWellCostProfile,
        apiData?.appraisalWellCostProfile,
        apiData?.sidetrackCostProfile,
        apiData?.seismicAcquisitionAndProcessing,
        apiData?.countryOfficeCost,
        apiData?.gAndGAdminCost,
    ])

    const handleDrilling = () => {
        if (projectData) {
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

            const startYears = [
                oilProducerCostProfile,
                gasProducerCostProfile,
                waterInjectorCostProfile,
                gasInjectorCostProfile,
            ].map((series) => series?.startYear).filter((startYear) => startYear !== undefined) as number[]

            const minStartYear = startYears.length > 0 ? Math.min(...startYears) : 2020

            let drillingCostSeriesList: (ITimeSeries | undefined)[] = [
                oilProducerCostProfile,
                gasProducerCostProfile,
                waterInjectorCostProfile,
                gasInjectorCostProfile,
            ]

            const rigUpgradingCost = projectData.commonProjectAndRevisionData.developmentOperationalWellCosts?.rigUpgrading
            const rigMobDemobCost = projectData.commonProjectAndRevisionData.developmentOperationalWellCosts?.rigMobDemob
            const sumOfRigAndMobDemob = rigUpgradingCost + rigMobDemobCost

            if (sumOfRigAndMobDemob > 0) {
                interface ITimeSeriesWithCostProfile extends ITimeSeries {
                    developmentRigUpgradingAndMobDemobCostProfile?: number[] | null;
                }

                const timeSeriesWithCostProfile: ITimeSeriesWithCostProfile = {
                    id: "developmentRigUpgradingAndMobDemob",
                    startYear: minStartYear,
                    name: "Development Rig Upgrading and Mob/Demob Costs",
                    values: [sumOfRigAndMobDemob],
                    sum: sumOfRigAndMobDemob,
                }

                if (
                    drillingCostSeriesList.every((series) => !series || !series.values || series.values.length === 0)
                    && timeSeriesWithCostProfile?.values && timeSeriesWithCostProfile.values.length > 0
                ) {
                    drillingCostSeriesList = [timeSeriesWithCostProfile]
                }
                if (!drillingCostSeriesList.includes(timeSeriesWithCostProfile)) {
                    drillingCostSeriesList.push(timeSeriesWithCostProfile)
                }
            }
            return mergeTimeseriesList(drillingCostSeriesList)
        }
        return undefined
    }

    useEffect(() => {
        if (activeTabCase === 7 && apiData) {
            const tableYearsData = [
                handleTotalExplorationCost(),
                handleDrilling(),
                handleOffshoreFacilitiesCost(),
                apiData.cessationOffshoreFacilitiesCostOverride,
                apiData.cessationOffshoreFacilitiesCost,
                apiData.cessationOnshoreFacilitiesCostProfile,
                apiData.totalFeasibilityAndConceptStudiesOverride,
                apiData.totalFeasibilityAndConceptStudies,
                apiData.totalFEEDStudiesOverride,
                apiData.totalFEEDStudies,
                apiData.totalOtherStudiesCostProfile,
                apiData.historicCostCostProfile,
                handleOffshoreOpexPlussWellIntervention(),
                apiData.onshoreRelatedOPEXCostProfile,
                apiData.additionalOPEXCostProfile,
            ]

            const yearsFromDate = apiData.case.dG4Date ? new Date(apiData.case.dG4Date).getFullYear() : 2030

            SetSummaryTableYearsFromProfiles(tableYearsData, yearsFromDate, setTableYears)
        }
    }, [activeTabCase, apiData, projectData])

    useEffect(() => {
        if (apiData || projectData) {
            const totalExplorationCostData = handleTotalExplorationCost()
            const totalDrillingCostData = handleDrilling()
            const offshoreFacilitiesCostData = handleOffshoreFacilitiesCost()
            const cessationOffshoreFacilitiesCostOverrideData = apiData?.cessationOffshoreFacilitiesCostOverride
            const cessationOffshoreFacilitiesCostData = apiData?.cessationOffshoreFacilitiesCost
            const cessationOnshoreFacilitiesCostProfileData = apiData?.cessationOnshoreFacilitiesCostProfile
            const totalFeasibilityAndConceptStudiesOverrideData = apiData?.totalFeasibilityAndConceptStudiesOverride
            const totalFeasibilityAndConceptStudiesData = apiData?.totalFeasibilityAndConceptStudies
            const totalFEEDStudiesOverrideData = apiData?.totalFEEDStudiesOverride
            const totalFEEDStudiesData = apiData?.totalFEEDStudies
            const totalOtherStudiesCostProfileData = apiData?.totalOtherStudiesCostProfile
            const historicCostCostProfileData = apiData?.historicCostCostProfile
            const offshoreOpexPlussWellInterventionData = handleOffshoreOpexPlussWellIntervention()
            const onshoreRelatedOPEXCostProfileData = apiData?.onshoreRelatedOPEXCostProfile
            const additionalOPEXCostProfileData = apiData?.additionalOPEXCostProfile

            const newExplorationTimeSeriesData: ITimeSeriesDataWithGroup[] = [
                {
                    profileName: "Exploration cost",
                    unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                    profile: totalExplorationCostData,
                    group: "Exploration",
                },
            ]

            const newCapexTimeSeriesData: ITimeSeriesDataWithGroup[] = [
                {
                    profileName: "Drilling",
                    unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                    profile: totalDrillingCostData,
                    group: "CAPEX",
                },
                {
                    profileName: "Offshore facilities",
                    unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                    profile: offshoreFacilitiesCostData,
                    group: "CAPEX",
                },
                {
                    profileName: "Cessation - offshore facilities",
                    unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                    profile: cessationOffshoreFacilitiesCostOverrideData?.override ? cessationOffshoreFacilitiesCostOverrideData : cessationOffshoreFacilitiesCostData,
                    group: "CAPEX",
                },
                {
                    profileName: "Cessation - onshore facilities",
                    unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                    profile: cessationOnshoreFacilitiesCostProfileData,
                    group: "CAPEX",
                },
            ]

            const newStudycostTimeSeriesData: ITimeSeriesDataWithGroup[] = [
                {
                    profileName: "Feasibility & Conceptual studies",
                    unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                    profile: totalFeasibilityAndConceptStudiesOverrideData?.override ? totalFeasibilityAndConceptStudiesOverrideData : totalFeasibilityAndConceptStudiesData,
                    group: "Study cost",
                },
                {
                    profileName: "FEED studies (DG2-DG3)",
                    unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                    profile: totalFEEDStudiesOverrideData?.override ? totalFEEDStudiesOverrideData : totalFEEDStudiesData,
                    group: "Study cost",
                },
                {
                    profileName: "Other studies",
                    unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                    profile: totalOtherStudiesCostProfileData,
                    group: "Study cost",
                },
            ]

            const newOpexTimeSeriesData: ITimeSeriesDataWithGroup[] = [
                {
                    profileName: "Historic cost",
                    unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                    profile: historicCostCostProfileData,
                    group: "OPEX",
                },
                {
                    profileName: "Offshore related OPEX, incl. well intervention",
                    unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                    profile: offshoreOpexPlussWellInterventionData,
                    group: "OPEX",
                },
                {
                    profileName: "Onshore related OPEX",
                    unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
                    profile: onshoreRelatedOPEXCostProfileData,
                    group: "OPEX",
                },
                {
                    profileName: "Additional OPEX",
                    unit: `${projectData?.commonProjectAndRevisionData.currency === 1 ? "MNOK" : "MUSD"}`,
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
    }, [apiData, projectData])

    const caseData = apiData?.case

    if (!caseData) { return <CaseSummarySkeleton /> }

    if (activeTabCase !== 7) { return null }

    return (
        <Grid container spacing={2}>
            <Grid item xs={12} md={6}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="case"
                    resourcePropertyKey="npv"
                    label="NPV before tax (MUSD)"
                    value={caseData.npv ? Number(caseData.npv.toFixed(2)) : undefined}
                    previousResourceObject={caseData}
                    integer={false}
                    allowNegative
                    disabled
                />
            </Grid>

            <Grid item xs={12} md={6}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="case"
                    resourcePropertyKey="npvOverride"
                    label="STEA NPV after tax(MUSD)"
                    value={caseData.npvOverride ? Number(caseData.npvOverride.toFixed(2)) : undefined}
                    previousResourceObject={caseData}
                    integer={false}
                    allowNegative
                    min={0}
                    max={1000000}
                    resourceId={caseData.id}
                />
            </Grid>
            <Grid item xs={12} md={6}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="case"
                    resourcePropertyKey="breakEven"
                    previousResourceObject={caseData}
                    label="B/E before tax (USD/bbl)"
                    value={caseData.breakEven ? Number(caseData.breakEven.toFixed(2)) : undefined}
                    integer={false}
                    min={0}
                    max={1000000}
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={6}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="case"
                    resourcePropertyKey="breakEvenOverride"
                    label="STEA B/E after tax(MUSD)"
                    value={caseData.breakEvenOverride ? Number(caseData.breakEvenOverride.toFixed(2)) : undefined}
                    previousResourceObject={caseData}
                    integer={false}
                    allowNegative
                    min={0}
                    max={1000000}
                    resourceId={caseData.id}
                />
            </Grid>
            <Grid item xs={12}>
                <CaseTabTableWithGrouping
                    allTimeSeriesData={allTimeSeriesData}
                    dg4Year={caseData.dG4Date ? new Date(caseData.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    includeFooter={false}
                />
            </Grid>
        </Grid>
    )
}

export default CaseSummaryTab
