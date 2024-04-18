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
import { SetTableYearsFromProfiles } from "../Components/CaseTabTableHelper"
import { useModalContext } from "../../../Context/ModalContext"

const CaseSummaryTab = (): React.ReactElement | null => {
    const {

        topside,
        setTopsideCost,
        surf,
        setSurfCost,
        substructure,
        setSubstructureCost,
        transport,
        setTransportCost,

        // CAPEX
        totalDrillingCost,
        setTotalDrillingCost,
        cessationOffshoreFacilitiesCost,
        setCessationOffshoreFacilitiesCost,
        cessationOnshoreFacilitiesCostProfile,
        setCessationOnshoreFacilitiesCostProfile,

        // OPEX
        historicCostCostProfile,
        wellInterventionCostProfile,
        offshoreFacilitiesOperationsCostProfile,
        onshoreRelatedOPEXCostProfile,
        setOnshoreRelatedOPEXCostProfile,
        additionalOPEXCostProfile,

        // Exploration
        totalExplorationCost,
        setTotalExplorationCost,
        explorationWellCostProfile,
        // gAndGAdminCost,// missing implementation
        seismicAcquisitionAndProcessing,
        explorationSidetrackCost,
        explorationAppraisalWellCost,
        countryOfficeCost,

        // Study cost
        totalFeasibilityAndConceptStudies,
        totalFeasibilityAndConceptStudiesOverride,
        totalFEEDStudies,
        totalFEEDStudiesOverride,
        totalOtherStudies,
    } = useCaseContext()

    const {
        wellProject,
    } = useModalContext()

    const { project } = useProjectContext()
    const {
        projectCase, setProjectCaseEdited, activeTabCase,
    } = useCaseContext()

    // OPEX
    const [, setTotalStudyCost] = useState<ITimeSeries>()
    const [, setOpexCost] = useState<Components.Schemas.OpexCostProfileDto>()
    const [, setCessationCost] = useState<Components.Schemas.SurfCessationCostProfileDto>()

    const [tableOilProducer, setTableOilProducer] = useState<ITimeSeries>()
    const [tableGasProducer, setTableGasProducer] = useState<ITimeSeries>()
    const [tableWaterInjector, setTableWaterInjector] = useState<ITimeSeries>()
    const [tableGasInjector, setTableGasInjector] = useState<ITimeSeries>()

    const [wellInterventionCostProfileOverride, setWellInterventionCostProfileOverride] = useState<Components.Schemas.WellInterventionCostProfileOverrideDto>()
    const [offshoreFacilitiesOperationsCostProfileOverride, setOffshoreFacilitiesOperationsCostProfileOverride] = useState<Components.Schemas.OffshoreFacilitiesOperationsCostProfileOverrideDto>()

    // CAPEX
    const [offshoreFacilitiesCost, setOffshoreFacilitiesCost] = useState<ITimeSeries>()

    const [, setStartYear] = useState<number>(2020)
    const [, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

    const [offshoreOpexPlussWellIntervention, setOffshoreOpexPlussWellIntervention] = useState<ITimeSeries | undefined>()
    const [wellProjectOilProducerCost, setWellProjectOilProducerCost] = useState<Components.Schemas.OilProducerCostProfileDto>()

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
            profile: offshoreOpexPlussWellIntervention,
            group: "CAPEX",
        },
        {
            profileName: "Cessation - offshore facilities",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationOffshoreFacilitiesCost,
            group: "CAPEX",
        },
        {
            profileName: "Cessation - onshore facilities",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationOnshoreFacilitiesCostProfile,
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
            profile: onshoreRelatedOPEXCostProfile,
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
                if (projectCase && project && topside && surf && substructure && transport && wellProject) {
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

                        setOffshoreOpexPlussWellIntervention(
                            MergeTimeseriesList(
                                [
                                    (projectCase.wellInterventionCostProfileOverride?.override === true
                                        ? projectCase.wellInterventionCostProfileOverride
                                        : projectCase.wellInterventionCostProfile),
                                    (projectCase.offshoreFacilitiesOperationsCostProfileOverride?.override === true
                                        ? projectCase.offshoreFacilitiesOperationsCostProfileOverride
                                        : projectCase.offshoreFacilitiesOperationsCostProfile)],
                            ),
                        )

                         setCessationOffshoreFacilitiesCost(projectCase.cessationOffshoreFacilitiesCostOverride?.override
                             ? projectCase.cessationOffshoreFacilitiesCostOverride : projectCase.cessationOffshoreFacilitiesCost)

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
                            // gAndGAdminCost // Missing implementation, uncomment when gAndGAdminCost is fixed
                        ]))

                        // Drilling cost
                        setTableOilProducer(wellProject?.oilProducerCostProfileOverride.override === true
                            ? wellProject.oilProducerCostProfileOverride
                            : wellProject?.oilProducerCostProfile)

                        setTableGasProducer(wellProject?.gasProducerCostProfileOverride?.override === true
                            ? wellProject.gasProducerCostProfileOverride
                            : wellProject?.gasProducerCostProfile)

                        setTableWaterInjector(wellProject?.waterInjectorCostProfileOverride?.override === true
                            ? wellProject.waterInjectorCostProfileOverride
                            : wellProject?.waterInjectorCostProfile)

                        setTableGasInjector(wellProject?.gasInjectorCostProfileOverride?.override === true
                            ? wellProject.gasInjectorCostProfileOverride
                            : wellProject?.gasInjectorCostProfile)

                        setTotalDrillingCost(MergeTimeseriesList([
                            tableOilProducer,
                            tableGasProducer,
                            tableWaterInjector,
                            tableGasInjector,
                        ]))

                        const addDevelopmentCostsToFirstYearOfDrilling = (totalDrillingCost: ITimeSeries, upgradingCost: number, mobDemobCost: number, setTotalDrillingCost: { (value: SetStateAction<ITimeSeries | undefined>): void; (arg0: any): void }) => {
                            if (totalDrillingCost) {
                                // Clone totalDrillingCost to avoid direct state mutation
                                const updatedTotalDrillingCost = { ...totalDrillingCost }

                                // Ensure values is an array before attempting to spread it.
                                // This also initializes values to an empty array if it's null or undefined.
                                const firstYearValues = Array.isArray(updatedTotalDrillingCost.values) ? [...updatedTotalDrillingCost.values] : []

                                // Proceed with the assumption that firstYearValues is an array (which it now is).
                                if (firstYearValues.length > 0) {
                                    // Only add rigMobDemobCost and rigUpgradingCost if they are greater than 0
                                    const costToAdd = Math.max(0, upgradingCost) + Math.max(0, mobDemobCost)
                                    firstYearValues[0] = (firstYearValues[0] || 0) + costToAdd
                                } else {
                                    // If there were no values, initialize with the costs provided.
                                    firstYearValues.push(Math.max(0, upgradingCost) + Math.max(0, mobDemobCost))
                                }

                                // Update the cloned object with the new or modified values array.
                                updatedTotalDrillingCost.values = firstYearValues

                                // Update the state with the modified drilling cost object.
                                setTotalDrillingCost(updatedTotalDrillingCost)
                            } else {
                                // Handle the case where totalDrillingCost is not initialized properly.
                                console.log("Total drilling cost is undefined.")
                            }
                        }


                        // Call the function with the costs to add

                        useEffect(() => {
                            if (totalDrillingCost) {
                                const rigUpgradingCost = project.developmentOperationalWellCosts.rigUpgrading
                                const rigMobDemobCost = project.developmentOperationalWellCosts.rigMobDemob
                                console.log("rigupgrading", rigUpgradingCost)
                                console.log("mobdemob", rigMobDemobCost)
                                // Call the function with the updated totalDrillingCost
                                addDevelopmentCostsToFirstYearOfDrilling(totalDrillingCost, rigUpgradingCost, rigMobDemobCost, setTotalDrillingCost);
                            }
                        }, [totalDrillingCost])
                        SetTableYearsFromProfiles([
                            totalExplorationCost, totalOtherStudies, totalFeasibilityAndConceptStudies, totalFEEDStudies, historicCostCostProfile,
                            offshoreOpexPlussWellIntervention, additionalOPEXCostProfile, onshoreRelatedOPEXCostProfile,

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
