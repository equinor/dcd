import {
    Dispatch, SetStateAction, useState, useEffect,
} from "react"
import Grid from "@mui/material/Grid"
import { useQueryClient, useQuery } from "react-query"
import { useParams } from "react-router"
import SwitchableNumberInput from "../../Input/SwitchableNumberInput"
import { ITimeSeries } from "../../../Models/ITimeSeries"
import { ITimeSeriesCost } from "../../../Models/ITimeSeriesCost"
import { useProjectContext } from "../../../Context/ProjectContext"
import { useCaseContext } from "../../../Context/CaseContext"
import CaseTabTableWithGrouping from "../Components/CaseTabTableWithGrouping"
import { ITimeSeriesCostOverride } from "../../../Models/ITimeSeriesCostOverride"
import { mergeTimeseriesList } from "../../../Utils/common"

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

const CaseSummaryTab = (): React.ReactElement | null => {
    const { activeTabCase } = useCaseContext()
    const { project } = useProjectContext()
    const { caseId } = useParams()

    const queryClient = useQueryClient()
    const projectId = project?.id || null

    const [tableYears] = useState<[number, number]>([2020, 2030])
    const [allTimeSeriesData, setAllTimeSeriesData] = useState<ITimeSeriesData[][]>([])

    const { data: apiData } = useQuery<Components.Schemas.CaseWithAssetsDto | undefined>(
        ["apiData", { projectId, caseId }],
        () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        {
            enabled: !!projectId && !!caseId,
            initialData: () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        },
    )

    useEffect(() => {
        if (!apiData || !project) {
            console.error("Missing data in CaseSummaryTab")
            return
        }
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

            const rigUpgradingCost = project.developmentOperationalWellCosts.rigUpgrading
            const rigMobDemobCost = project.developmentOperationalWellCosts.rigMobDemob
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

        const newExplorationTimeSeriesData: ITimeSeriesData[] = [
            {
                profileName: "Exploration cost",
                unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: totalExplorationCostData,
                group: "Exploration",
            },
        ]

        const newCapexTimeSeriesData: ITimeSeriesData[] = [
            {
                profileName: "Drilling",
                unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: totalDrillingCostData,
                group: "CAPEX",
            },
            {
                profileName: "Offshore facilities",
                unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: offshoreFacilitiesCostData,
                group: "CAPEX",
            },
            {
                profileName: "Cessation - offshore facilities",
                unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: cessationOffshoreFacilitiesCostOverrideData?.override ? cessationOffshoreFacilitiesCostOverrideData : cessationOffshoreFacilitiesCostData,
                group: "CAPEX",
            },
            {
                profileName: "Cessation - onshore facilities",
                unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: cessationOnshoreFacilitiesCostProfileData,
                group: "CAPEX",
            },
        ]

        const newStudycostTimeSeriesData: ITimeSeriesData[] = [
            {
                profileName: "Feasibility & Conceptual studies",
                unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: totalFeasibilityAndConceptStudiesOverrideData?.override ? totalFeasibilityAndConceptStudiesOverrideData : totalFeasibilityAndConceptStudiesData,
                group: "Study cost",
            },
            {
                profileName: "FEED studies (DG2-DG3)",
                unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: totalFEEDStudiesOverrideData?.override ? totalFEEDStudiesOverrideData : totalFEEDStudiesData,
                group: "Study cost",
            },
            {
                profileName: "Other studies",
                unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: totalOtherStudiesCostProfileData,
                group: "Study cost",
            },
        ]

        const newOpexTimeSeriesData: ITimeSeriesData[] = [
            {
                profileName: "Historic cost",
                unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: historicCostCostProfileData,
                group: "OPEX",
            },
            {
                profileName: "Offshore related OPEX, incl. well intervention",
                unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: offshoreOpexPlussWellInterventionData,
                group: "OPEX",
            },
            {
                profileName: "Onshore related OPEX",
                unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
                profile: onshoreRelatedOPEXCostProfileData,
                group: "OPEX",
            },
            {
                profileName: "Additional OPEX",
                unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
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
    }, [apiData, project])

    const caseData = apiData?.case

    if (!caseData) { return <p>Loading...</p> }

    if (activeTabCase !== 7) { return null }

    return (
        <Grid container spacing={3}>
            <Grid item xs={12} md={6}>
                <SwitchableNumberInput
                    resourceName="case"
                    resourcePropertyKey="npv"
                    label="NPV before tax"
                    value={caseData.npv}
                    integer={false}
                    allowNegative
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={6}>
                <SwitchableNumberInput
                    resourceName="case"
                    resourcePropertyKey="breakEven"
                    label="B/E before tax"
                    value={caseData.breakEven}
                    integer={false}
                    min={0}
                    max={1000000}
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
