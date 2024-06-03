import {
    Dispatch, SetStateAction, useEffect, useRef, useState,
} from "react"
import Grid from "@mui/material/Grid"
import SwitchableNumberInput from "../../Input/SwitchableNumberInput"
import { ITimeSeries } from "../../../Models/ITimeSeries"
import { ITimeSeriesCost } from "../../../Models/ITimeSeriesCost"
import { useProjectContext } from "../../../Context/ProjectContext"
import { useCaseContext } from "../../../Context/CaseContext"
import CaseTabTableWithGrouping from "../Components/CaseTabTableWithGrouping"
import { ITimeSeriesCostOverride } from "../../../Models/ITimeSeriesCostOverride"
import { SetTableYearsFromProfiles } from "../Components/CaseTabTableHelper"
import { useModalContext } from "../../../Context/ModalContext"
import { setNonNegativeNumberState } from "../../../Utils/common"

const CaseSummaryTab = (): React.ReactElement | null => {
    const {
        projectCase,
        setProjectCaseEdited,
        activeTabCase,
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
        cessationOffshoreFacilitiesCost,
        setCessationOffshoreFacilitiesCost,
        cessationOffshoreFacilitiesCostOverride,
        setCessationOffshoreFacilitiesCostOverride,
        cessationOnshoreFacilitiesCostProfile,
        setCessationOnshoreFacilitiesCostProfile,

        // OPEX
        historicCostCostProfile,
        setHistoricCostCostProfile,
        onshoreRelatedOPEXCostProfile,
        setOnshoreRelatedOPEXCostProfile,
        additionalOPEXCostProfile,
        setAdditionalOPEXCostProfile,

        // Exploration
        totalExplorationCost,
        explorationWellCostProfile,
        // gAndGAdminCost,// missing implementation
        seismicAcquisitionAndProcessing,

        // Study cost
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

        offshoreFacilitiesCost,

        offshoreOpexPlussWellIntervention,
    } = useCaseContext()

    const {
        wellProject,
        exploration,
    } = useModalContext()

    const { project } = useProjectContext()

    // TODO: this is wrong we are using setters but never using the values
    const [, setStartYear] = useState<number>(2020)
    const [, setEndYear] = useState<number>(2030)

    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

    const summaryGridRef = useRef<any>(null)

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
            profileName: "Cessation - offshore facilities",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationOffshoreFacilitiesCostOverride?.override ? cessationOffshoreFacilitiesCostOverride : cessationOffshoreFacilitiesCost,
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

    const allTimeSeriesData = [
        explorationTimeSeriesData,
        capexTimeSeriesData,
        studycostTimeSeriesData,
        opexTimeSeriesData,
    ]

    console.log(exploration)
    console.log(projectCase)

    useEffect(() => {
        if (summaryGridRef.current
            && summaryGridRef.current.api
            && summaryGridRef.current.api.refreshCells
        ) {
                console.log(summaryGridRef)
            summaryGridRef.current.api.refreshCells()
        }
    }, [totalDrillingCost, cessationOffshoreFacilitiesCost, cessationOnshoreFacilitiesCostProfile,
        historicCostCostProfile, onshoreRelatedOPEXCostProfile, additionalOPEXCostProfile,
        explorationWellCostProfile, totalExplorationCost, seismicAcquisitionAndProcessing,
        totalFeasibilityAndConceptStudies, totalFEEDStudies, totalOtherStudies,
        offshoreFacilitiesCost, offshoreOpexPlussWellIntervention,
    ])

    // certain profiles doesn't update as they are not set in this useEffect
    // exploration cost and drilling is wrong when changing case
    useEffect(() => {
        (async () => {
            try {
                if (projectCase && project && topside && surf && substructure && transport && wellProject && exploration) {
                    if (activeTabCase === 7 && projectCase?.id) {
                        console.log(totalExplorationCost)
                        setTotalFeasibilityAndConceptStudies(projectCase.totalFeasibilityAndConceptStudies)
                        setTotalFeasibilityAndConceptStudiesOverride(projectCase.totalFeasibilityAndConceptStudiesOverride)
                        setTotalFEEDStudies(projectCase.totalFEEDStudies)
                        setTotalFEEDStudiesOverride(projectCase.totalFEEDStudiesOverride)
                        setTotalOtherStudies(projectCase.totalOtherStudies)

                        setHistoricCostCostProfile(projectCase.historicCostCostProfile)
                        setOnshoreRelatedOPEXCostProfile(projectCase.onshoreRelatedOPEXCostProfile)
                        setAdditionalOPEXCostProfile(projectCase.additionalOPEXCostProfile)

                        setCessationOffshoreFacilitiesCost(projectCase.cessationOffshoreFacilitiesCost)
                        setCessationOffshoreFacilitiesCostOverride(projectCase.cessationOffshoreFacilitiesCostOverride)

                        setCessationOnshoreFacilitiesCostProfile(projectCase.cessationOnshoreFacilitiesCostProfile)

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

                        // Drilling cost
                        const oilProducerCostProfile = wellProject?.oilProducerCostProfileOverride?.override
                            ? wellProject.oilProducerCostProfileOverride
                            : wellProject?.oilProducerCostProfile

                        const gasProducerCostProfile = wellProject?.gasProducerCostProfileOverride?.override
                            ? wellProject.gasProducerCostProfileOverride
                            : wellProject?.gasProducerCostProfile

                        const waterInjectorCostProfile = wellProject?.waterInjectorCostProfileOverride?.override
                            ? wellProject.waterInjectorCostProfileOverride
                            : wellProject?.waterInjectorCostProfile

                        const gasInjectorCostProfile = wellProject?.gasInjectorCostProfileOverride?.override
                            ? wellProject.gasInjectorCostProfileOverride
                            : wellProject?.gasInjectorCostProfile

                        SetTableYearsFromProfiles([
                            projectCase, wellProject, totalExplorationCost, totalOtherStudies, totalFeasibilityAndConceptStudies, totalFEEDStudiesOverride, historicCostCostProfile,
                            additionalOPEXCostProfile, onshoreRelatedOPEXCostProfile, offshoreOpexPlussWellIntervention, projectCase?.totalFeasibilityAndConceptStudies,
                            projectCase?.totalFEEDStudies,
                            projectCase?.wellInterventionCostProfile,
                            projectCase?.offshoreFacilitiesOperationsCostProfile,
                            projectCase?.cessationWellsCost,
                            projectCase?.cessationOffshoreFacilitiesCost,
                            projectCase?.cessationOnshoreFacilitiesCostProfile,
                            projectCase?.totalFeasibilityAndConceptStudiesOverride,
                            projectCase?.wellInterventionCostProfileOverride,
                            projectCase?.offshoreFacilitiesOperationsCostProfileOverride,
                            projectCase?.cessationWellsCostOverride,
                            projectCase?.cessationOffshoreFacilitiesCostOverride,
                            surfCostProfile,
                            topsideCostProfile,
                            substructureCostProfile,
                            transportCostProfile,
                            oilProducerCostProfile,
                            gasProducerCostProfile,
                            waterInjectorCostProfile,
                            gasInjectorCostProfile,
                            explorationWellCostProfile,
                            seismicAcquisitionAndProcessing,
                            totalDrillingCost,
                        ], projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030, setStartYear, setEndYear, setTableYears)
                    }
                }
            } catch (error) {
                console.error("[CaseView] Error while generating cost profile", error)
            }
        })()
    }, [activeTabCase, projectCase])

    if (activeTabCase !== 7) { return null }

    return (
        <Grid container spacing={3}>
            <Grid item xs={12} md={6}>
                <SwitchableNumberInput
                    objectKey={projectCase?.npv}
                    label="NPV before tax"
                    onSubmit={(value: number) => setNonNegativeNumberState(value, "npv", projectCase, setProjectCaseEdited)}
                    value={projectCase?.npv}
                    integer={false}
                    allowNegative
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12} md={6}>
                <SwitchableNumberInput
                    objectKey={projectCase?.breakEven}
                    label="B/E before tax"
                    onSubmit={(value: number) => setNonNegativeNumberState(value, "breakEven", projectCase, setProjectCaseEdited)}
                    value={projectCase?.breakEven}
                    integer={false}
                    min={0}
                    max={1000000}
                />
            </Grid>
            <Grid item xs={12}>
                <CaseTabTableWithGrouping
                    allTimeSeriesData={allTimeSeriesData}
                    dg4Year={projectCase?.dG4Date ? new Date(projectCase.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    includeFooter={false}
                    gridRef={summaryGridRef}
                />
            </Grid>
        </Grid>
    )
}

export default CaseSummaryTab
