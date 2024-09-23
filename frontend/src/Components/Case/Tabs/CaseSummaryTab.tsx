import {
    Dispatch, SetStateAction, useState, useEffect,
} from "react"
import Grid from "@mui/material/Grid"
import { useQuery } from "@tanstack/react-query"
import { useParams } from "react-router"
import SwitchableNumberInput from "../../Input/SwitchableNumberInput"
import { ITimeSeries } from "../../../Models/ITimeSeries"
import { ITimeSeriesCost } from "../../../Models/ITimeSeriesCost"
import { useProjectContext } from "../../../Context/ProjectContext"
import { useCaseContext } from "../../../Context/CaseContext"
import CaseTabTableWithGrouping from "../Components/CaseTabTableWithGrouping"
import { ITimeSeriesCostOverride } from "../../../Models/ITimeSeriesCostOverride"
import { mergeTimeseries, mergeTimeseriesList } from "../../../Utils/common"
import { SetSummaryTableYearsFromProfiles } from "../Components/CaseTabTableHelper"
import CaseSummarySkeleton from "../../LoadingSkeletons/CaseSummarySkeleton"
import { caseQueryFn } from "../../../Services/QueryFunctions"

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

const CaseSummaryTab = ({ addEdit }: { addEdit: any }) => {
    const { activeTabCase } = useCaseContext()
    const { project } = useProjectContext()
    const { caseId } = useParams()

    const projectId = project?.id

    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])
    const [allTimeSeriesData, setAllTimeSeriesData] = useState<ITimeSeriesData[][]>([])
    const [yearRangeSetFromProfiles, setYearRangeSetFromProfiles] = useState<boolean>(false)

    const [cashflowProfile, setCashflowProfile] = useState<ITimeSeries | undefined>(undefined)

    const { data: apiData } = useQuery<Components.Schemas.CaseWithAssetsDto | undefined>(
        ["apiData", { projectId, caseId }],
        () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        {
            enabled: !!projectId && !!caseId,
            initialData: () => queryClient.getQueryData(["apiData", { projectId, caseId }]),
        },
    )

    const calculateDiscountedVolume = (volumeArray: number[], discountRate: number):
        number => volumeArray.reduce((accumulatedVolume, volume, index) => accumulatedVolume + (volume / (1 + (discountRate / 100)) ** (index + 1)), 0)

    const calculateBreakEvenOilPrice = () => {
        if (!apiData) { return }

        const discountRate = project?.discountRate || 8
        const defaultOilPrice = project?.oilPriceUSD || 75
        const gasPriceNOK = project?.gasPriceNOK
        const exchangeRateUSDToNOK = project?.exchangeRateUSDToNOK ?? 10
        const exchangeRateNOKToUSD = 1 / exchangeRateUSDToNOK
        const oilVolume = mergeTimeseries(apiData.productionProfileOil, apiData.additionalProductionProfileOil)
        console.log("oilVolume", oilVolume)

        const gasVolume = mergeTimeseries(apiData.productionProfileGas, apiData.additionalProductionProfileGas)
        console.log("gasVolume", gasVolume)

        const discountedGasVolume = calculateDiscountedVolume(gasVolume?.values || [], discountRate)
        const discountedOilVolume = calculateDiscountedVolume(oilVolume.values || [], discountRate)
        const discountedTotalCost = calculateDiscountedVolume(apiData?.calculatedTotalCostCostProfile?.values || [], discountRate)

        const GOR = discountedGasVolume / discountedOilVolume
        let PA = 0
        if (gasPriceNOK) {
            PA = gasPriceNOK / (exchangeRateNOKToUSD * 6.29 * defaultOilPrice)
        }

        const breakEvenOilPrice = discountedTotalCost / ((GOR * PA) + 1) / discountedOilVolume
        const caseData = apiData?.case

        if (caseData) {
            caseData.breakEven = breakEvenOilPrice
        }
    }

    const calculateNPV = (cashflowValues: number[], discountRate: number):
        number => cashflowValues.reduce((accumulatedNPV, cashflow, index) => accumulatedNPV + (cashflow / (1 + (0.01 * discountRate)) ** (index + 1)), 0)

    const calculateCashflowProfile = (): ITimeSeries => {
        if (!apiData) {
            return {
                id: "cashflow",
                startYear: 0,
                values: [],
            }
        }

        const dg4Year = new Date(apiData.case.dG4Date).getFullYear()
        const totalCostProfile = apiData.calculatedTotalCostCostProfile
        const totalIncomeProfile = apiData.calculatedTotalIncomeCostProfile
        console.log("totalIncomeProfile.values", totalIncomeProfile?.values)

        console.log("totalCostProfile.values", totalCostProfile?.values)

        if (!totalCostProfile?.values || !totalIncomeProfile?.values) {
            return {
                id: "cashflow",
                startYear: dg4Year,
                values: [],
            }
        }

        const negatedCostProfile = {
            ...totalCostProfile,
            values: totalCostProfile.values.map((value) => -value),
        }
        console.log("negatedCostProfile.values", negatedCostProfile.values)

        const mergedProfile = mergeTimeseries(negatedCostProfile, totalIncomeProfile)
        console.log("mergedProfile.values", mergedProfile.values)

        return {
            id: "cashflow",
            startYear: mergedProfile.startYear,
            values: mergedProfile.values,
        }
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
        if (project) {
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

            const rigUpgradingCost = project.developmentOperationalWellCosts?.rigUpgrading
            const rigMobDemobCost = project.developmentOperationalWellCosts?.rigMobDemob
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
        if (activeTabCase === 7 && apiData && !yearRangeSetFromProfiles) {
            const caseData = apiData.case as Components.Schemas.CaseDto

            const newCashflowProfile = calculateCashflowProfile()
            console.log("newCashflowProfile.values", newCashflowProfile.values)
            setCashflowProfile(newCashflowProfile)
            calculateBreakEvenOilPrice()
            console.log("caseData.breakEven", caseData.breakEven)
            SetSummaryTableYearsFromProfiles([
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
            ], caseData.dG4Date ? new Date(caseData.dG4Date).getFullYear() : 2030, setTableYears)
            setYearRangeSetFromProfiles(true)
        }
    }, [activeTabCase, apiData, project])

    useEffect(() => {
        if (apiData || project) {
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
        }
    }, [apiData, project])
    const caseData = apiData?.case

    if (!caseData) { return <CaseSummarySkeleton /> }

    if (activeTabCase !== 7) { return null }
    console.log("cashflowProfile.values", cashflowProfile?.values)
    console.log("project.discountRate", project?.discountRate)

    const npvValue = cashflowProfile && cashflowProfile.values && project?.discountRate
        ? calculateNPV(cashflowProfile.values, project.discountRate)
        : 0
    console.log("npvValue", npvValue)
    return (
        <Grid container spacing={2}>
            <Grid item xs={12} md={6}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="case"
                    resourcePropertyKey="npv"
                    label="NPV before tax"
                    value={npvValue}
                    previousResourceObject={caseData}
                    integer={false}
                    allowNegative
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={6}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="case"
                    resourcePropertyKey="breakEven"
                    previousResourceObject={caseData}
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
