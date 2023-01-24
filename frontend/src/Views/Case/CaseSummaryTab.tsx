import {
    ChangeEventHandler, Dispatch, SetStateAction, useEffect, useState,
} from "react"
import styled from "styled-components"

import { Button, Progress, Typography } from "@equinor/eds-core-react"
import { Project } from "../../models/Project"
import { Case } from "../../models/case/Case"
import CaseNumberInput from "../../Components/Case/CaseNumberInput"
import { DrainageStrategy } from "../../models/assets/drainagestrategy/DrainageStrategy"
import CaseTabTable from "./CaseTabTable"
import { GetCaseService } from "../../Services/CaseService"
import { ITimeSeries } from "../../models/ITimeSeries"
import { StudyCostProfile } from "../../models/case/StudyCostProfile"
import { OpexCostProfile } from "../../models/case/OpexCostProfile"
import { CessationCostProfile } from "../../models/case/CessationCostProfile"
import { Exploration } from "../../models/assets/exploration/Exploration"
import { Surf } from "../../models/assets/surf/Surf"
import { GetSurfService } from "../../Services/SurfService"
import { WellProject } from "../../models/assets/wellproject/WellProject"
import { Substructure } from "../../models/assets/substructure/Substructure"
import { Topside } from "../../models/assets/topside/Topside"
import { Transport } from "../../models/assets/transport/Transport"
import { TopsideCostProfile } from "../../models/assets/topside/TopsideCostProfile"
import { SurfCostProfile } from "../../models/assets/surf/SurfCostProfile"
import { SubstructureCostProfile } from "../../models/assets/substructure/SubstructureCostProfile"
import { TransportCostProfile } from "../../models/assets/transport/TransportCostProfile"
import { GetGenerateProfileService } from "../../Services/GenerateProfileService"
import { MergeTimeseries } from "../../Utils/common"
import { TotalFEEDStudies } from "../../models/case/TotalFEEDStudies"
import { TotalFeasibilityAndConceptStudies } from "../../models/case/TotalFeasibilityAndConceptStudies"

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

interface Props {
    project: Project,
    setProject: Dispatch<SetStateAction<Project | undefined>>,
    caseItem: Case,
    setCase: Dispatch<SetStateAction<Case | undefined>>,
    topside: Topside,
    setTopside: Dispatch<SetStateAction<Topside | undefined>>,
    surf: Surf,
    setSurf: Dispatch<SetStateAction<Surf | undefined>>,
    substructure: Substructure,
    setSubstrucutre: Dispatch<SetStateAction<Substructure | undefined>>,
    transport: Transport,
    setTransport: Dispatch<SetStateAction<Transport | undefined>>,
    exploration: Exploration,
    setExploration: Dispatch<SetStateAction<Exploration | undefined>>,
    wellProject: WellProject,
    setWellProject: Dispatch<SetStateAction<WellProject | undefined>>,
    drainageStrategy: DrainageStrategy
    activeTab: number
}

function CaseSummaryTab({
    project, setProject,
    caseItem, setCase,
    exploration, setExploration,
    wellProject, setWellProject,
    topside, setTopside,
    surf, setSurf,
    substructure, setSubstrucutre,
    transport, setTransport,
    drainageStrategy,
    activeTab,
}: Props) {
    // OPEX
    const [totalStudyCost, setTotalStudyCost] = useState<ITimeSeries>()
    const [opexCost, setOpexCost] = useState<OpexCostProfile>()
    const [cessationCost, setCessationCost] = useState<CessationCostProfile>()

    // CAPEX
    const [topsideCost, setTopsideCost] = useState<TopsideCostProfile>()
    const [surfCost, setSurfCost] = useState<SurfCostProfile>()
    const [substructureCost, setSubstructureCost] = useState<SubstructureCostProfile>()
    const [transportCost, setTransportCost] = useState<TransportCostProfile>()

    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

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
        if (firstYear < Number.MAX_SAFE_INTEGER && lastYear > Number.MIN_SAFE_INTEGER) {
            setStartYear(firstYear + caseItem.DG4Date.getFullYear())
            setEndYear(lastYear + caseItem.DG4Date.getFullYear())
            setTableYears([firstYear + caseItem.DG4Date.getFullYear(), lastYear + caseItem.DG4Date.getFullYear()])
        }
    }

    useEffect(() => {
        (async () => {
            try {
                if (activeTab === 7) {
                    const studyWrapper = (await GetGenerateProfileService()).generateStudyCost(caseItem.id)
                    const opexWrapper = (await GetGenerateProfileService()).generateOpexCost(caseItem.id)
                    const cessationWrapper = (await GetGenerateProfileService())
                        .generateCessationCost(caseItem.id)

                    const opex = OpexCostProfile.fromJSON((await opexWrapper).opexCostProfileDto)
                    const cessation = CessationCostProfile.fromJSON((await cessationWrapper).cessationCostDto)

                    let feasibility = TotalFeasibilityAndConceptStudies
                        .fromJSON((await studyWrapper).totalFeasibilityAndConceptStudiesDto)
                    let feed = TotalFEEDStudies.fromJSON((await studyWrapper).totalFEEDStudiesDto)
                    if (caseItem.totalFeasibilityAndConceptStudiesOverride?.override === true) {
                        feasibility = caseItem.totalFeasibilityAndConceptStudiesOverride
                    }
                    if (caseItem.totalFEEDStudiesOverride?.override === true) {
                        feed = caseItem.totalFEEDStudiesOverride
                    }

                    const totalStudy = MergeTimeseries(feasibility, feed)
                    setTotalStudyCost(totalStudy)

                    setOpexCost(opex)
                    setCessationCost(cessation)

                    // CAPEX
                    const topsideCostProfile = topside.costProfile
                    setTopsideCost(topsideCostProfile)
                    const surfCostProfile = surf.costProfile
                    setSurfCost(surfCostProfile)
                    const substructureCostProfile = substructure.costProfile
                    setSubstructureCost(substructureCostProfile)
                    const transportCostProfile = transport.costProfile
                    setTransportCost(transportCostProfile)

                    setTableYearsFromProfiles([
                        topsideCostProfile, surfCostProfile, substructureCostProfile, transportCostProfile,
                    ])
                }
            } catch (error) {
                console.error("[CaseView] Error while generating cost profile", error)
            }
        })()
    }, [activeTab])

    const handleCaseNPVChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = Case.Copy(caseItem)
        newCase.npv = e.currentTarget.value.length > 0
            ? Number(e.currentTarget.value) : undefined
        setCase(newCase)
    }

    const handleCaseBreakEvenChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = Case.Copy(caseItem)
        newCase.breakEven = e.currentTarget.value.length > 0
            ? Math.max(Number(e.currentTarget.value), 0) : undefined
        setCase(newCase)
    }

    interface ITimeSeriesData {
        profileName: string
        unit: string,
        set?: Dispatch<SetStateAction<ITimeSeries | undefined>>,
        profile: ITimeSeries | undefined
    }

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

    if (activeTab !== 7) { return null }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Summary</PageTitle>
            </TopWrapper>
            <ColumnWrapper>
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleCaseNPVChange}
                            defaultValue={caseItem.npv}
                            integer={false}
                            label="NPV before tax"
                            allowNegative
                        />
                    </NumberInputField>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleCaseBreakEvenChange}
                            defaultValue={caseItem.breakEven}
                            integer={false}
                            label="B/E before tax"
                        />
                    </NumberInputField>
                </RowWrapper>
            </ColumnWrapper>
            <TableWrapper>
                <CaseTabTable
                    caseItem={caseItem}
                    project={project}
                    setCase={setCase}
                    setProject={setProject}
                    timeSeriesData={opexTimeSeriesData}
                    dg4Year={caseItem.DG4Date.getFullYear()}
                    tableYears={tableYears}
                    tableName="OPEX"
                    includeFooter={false}
                />
            </TableWrapper>
            <TableWrapper>
                <CaseTabTable
                    caseItem={caseItem}
                    project={project}
                    setCase={setCase}
                    setProject={setProject}
                    timeSeriesData={capexTimeSeriesData}
                    dg4Year={caseItem.DG4Date.getFullYear()}
                    tableYears={tableYears}
                    tableName="CAPEX"
                    includeFooter
                />
            </TableWrapper>
        </>
    )
}

export default CaseSummaryTab
