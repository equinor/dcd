import {
    ChangeEventHandler, Dispatch, SetStateAction, useEffect, useState,
} from "react"
import Grid from "@mui/material/Grid"
import CaseNumberInput from "../../Input/CaseNumberInput"
import CaseTabTable from "../Components/CaseTabTable"
import { ITimeSeries } from "../../../Models/ITimeSeries"
import { GetGenerateProfileService } from "../../../Services/CaseGeneratedProfileService"
import { MergeTimeseries } from "../../../Utils/common"
import { ITimeSeriesCost } from "../../../Models/ITimeSeriesCost"
import InputSwitcher from "../../Input/InputSwitcher"
import { useProjectContext } from "../../../Context/ProjectContext"
import { useCaseContext } from "../../../Context/CaseContext"

interface ITimeSeriesData {
    profileName: string
    unit: string,
    set?: Dispatch<SetStateAction<ITimeSeriesCost | undefined>>,
    profile: ITimeSeries | undefined
}

interface Props {
    topside: Components.Schemas.TopsideDto,
    surf: Components.Schemas.SurfDto,
    substructure: Components.Schemas.SubstructureDto,
    transport: Components.Schemas.TransportDto,
}

const CaseSummaryTab = ({
    topside,
    surf,
    substructure,
    transport,
}: Props) => {
    const { project } = useProjectContext()
    const {
        projectCase, projectCaseEdited, setProjectCaseEdited, activeTabCase,
    } = useCaseContext()
    // OPEX
    const [totalStudyCost, setTotalStudyCost] = useState<ITimeSeries>()
    const [opexCost, setOpexCost] = useState<Components.Schemas.OpexCostProfileDto>()
    const [cessationCost, setCessationCost] = useState<Components.Schemas.SurfCessationCostProfileDto>()

    // CAPEX
    const [historicCost] = useState<Components.Schemas.HistoricCostCostProfileDto>()
    const [topsideCost, setTopsideCost] = useState<Components.Schemas.TopsideCostProfileDto>()
    const [surfCost, setSurfCost] = useState<Components.Schemas.SurfCostProfileDto>()
    const [substructureCost, setSubstructureCost] = useState<Components.Schemas.SubstructureCostProfileDto>()
    const [transportCost, setTransportCost] = useState<Components.Schemas.TransportCostProfileDto>()

    const [, setStartYear] = useState<number>(2020)
    const [, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

    const opexTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Study cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: totalStudyCost,
        },
        {
            profileName: "Offshore facliities operations + well intervention",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: opexCost,
        },
        {
            profileName: "Cessation wells + Cessation offshore facilities",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: cessationCost,
        },
    ]

    const capexTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Historic cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: historicCost,
            set: setTopsideCost,
        },
        {
            profileName: "Topside cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: topsideCost,
            set: setTopsideCost,
        },
        {
            profileName: "SURF cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: surfCost,
            set: setSurfCost,
        },
        {
            profileName: "Substructure cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: substructureCost,
            set: setSubstructureCost,
        },
        {
            profileName: "Transport cost",
            unit: `${project?.currency === 1 ? "MNOK" : "MUSD"}`,
            profile: transportCost,
            set: setTransportCost,
        },
    ]

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
        if (firstYear < Number.MAX_SAFE_INTEGER && lastYear > Number.MIN_SAFE_INTEGER && projectCase?.dG4Date) {
            setStartYear(firstYear + new Date(projectCase?.dG4Date).getFullYear())
            setEndYear(lastYear + new Date(projectCase?.dG4Date).getFullYear())
            setTableYears([firstYear + new Date(projectCase?.dG4Date).getFullYear(), lastYear + new Date(projectCase?.dG4Date).getFullYear()])
        }
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
                if (project && activeTabCase === 7 && projectCase?.id) {
                    const studyWrapper = (await GetGenerateProfileService()).generateStudyCost(project.id, projectCase?.id)
                    const opexWrapper = (await GetGenerateProfileService()).generateOpexCost(project.id, projectCase?.id)
                    const cessationWrapper = (await GetGenerateProfileService()).generateCessationCost(project.id, projectCase?.id)

                    const opex = (await opexWrapper).opexCostProfileDto
                    const cessation = (await cessationWrapper).cessationCostDto

                    let feasibility = (await studyWrapper).totalFeasibilityAndConceptStudiesDto
                    let feed = (await studyWrapper).totalFEEDStudiesDto

                    const totalOtherStudies = (await studyWrapper).totalOtherStudiesDto
                    if (projectCase?.totalFeasibilityAndConceptStudiesOverride?.override === true) {
                        feasibility = projectCase?.totalFeasibilityAndConceptStudiesOverride
                    }
                    if (projectCase?.totalFEEDStudiesOverride?.override === true) {
                        feed = projectCase?.totalFEEDStudiesOverride
                    }

                    const totalStudy = MergeTimeseries(feasibility, feed, totalOtherStudies)
                    setTotalStudyCost(totalStudy)

                    setOpexCost(opex)
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
                <CaseTabTable
                    timeSeriesData={opexTimeSeriesData}
                    dg4Year={projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="OPEX"
                    includeFooter={false}
                />
            </Grid>
            <Grid item xs={12}>
                <CaseTabTable
                    timeSeriesData={capexTimeSeriesData}
                    dg4Year={projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="CAPEX"
                    includeFooter
                />
            </Grid>
        </Grid>
    )
}

export default CaseSummaryTab
