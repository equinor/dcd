import {
    ChangeEventHandler, Dispatch, SetStateAction, useEffect, useState,
} from "react"
import styled from "styled-components"

import { Typography } from "@equinor/eds-core-react"
import CaseNumberInput from "./CaseNumberInput"
import CaseTabTable from "./CaseTabTable"
import { ITimeSeries } from "../../Models/ITimeSeries"
import { GetGenerateProfileService } from "../../Services/CaseGeneratedProfileService"
import { ITimeSeriesCost } from "../../Models/ITimeSeriesCost"
import InputContainer from "../Input/InputContainer"
import { MergeTimeseries } from "../../Utils/common"


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
    project: Components.Schemas.ProjectDto,
    caseItem: Components.Schemas.CaseDto,
    setCase: Dispatch<SetStateAction<Components.Schemas.CaseDto | undefined>>,
    topside: Components.Schemas.TopsideDto,
    surf: Components.Schemas.SurfDto,
    substructure: Components.Schemas.SubstructureDto,
    transport: Components.Schemas.TransportDto,
    activeTab: number
}

const CaseSummaryTab = ({
    project,
    caseItem, setCase,
    topside,
    surf,
    substructure,
    transport,
    activeTab,
}: Props) => {
    // OPEX
    const [totalStudyCost, setTotalStudyCost] = useState<ITimeSeries>()
    const [opexCost, setOpexCost] = useState<Components.Schemas.OpexCostProfileDto>()
    const [cessationCost, setCessationCost] = useState<Components.Schemas.SurfCessationCostProfileDto>()

    // CAPEX
    const [historicCost, setHistoricCost] = useState<Components.Schemas.HistoricCostCostProfileDto>()

    const [topsideCost, setTopsideCost] = useState<Components.Schemas.TopsideCostProfileDto>()
    const [surfCost, setSurfCost] = useState<Components.Schemas.SurfCostProfileDto>()
    const [substructureCost, setSubstructureCost] = useState<Components.Schemas.SubstructureCostProfileDto>()
    const [transportCost, setTransportCost] = useState<Components.Schemas.TransportCostProfileDto>()

    const [, setStartYear] = useState<number>(2020)
    const [, setEndYear] = useState<number>(2030)
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
        if (firstYear < Number.MAX_SAFE_INTEGER && lastYear > Number.MIN_SAFE_INTEGER && caseItem.dG4Date) {
            setStartYear(firstYear + new Date(caseItem.dG4Date).getFullYear())
            setEndYear(lastYear + new Date(caseItem.dG4Date).getFullYear())
            setTableYears([firstYear + new Date(caseItem.dG4Date).getFullYear(), lastYear + new Date(caseItem.dG4Date).getFullYear()])
        }
    }

    useEffect(() => {
        (async () => {
            try {
                if (activeTab === 7 && caseItem.id) {
                    const studyWrapper = (await GetGenerateProfileService()).generateStudyCost(project.id, caseItem.id)
                    const opexWrapper = (await GetGenerateProfileService()).generateOpexCost(project.id, caseItem.id)
                    const cessationWrapper = (await GetGenerateProfileService()).generateCessationCost(project.id, caseItem.id)

                    const opex = (await opexWrapper).opexCostProfileDto
                    const cessation = (await cessationWrapper).cessationCostDto

                    let feasibility = (await studyWrapper).totalFeasibilityAndConceptStudiesDto
                    let feed = (await studyWrapper).totalFEEDStudiesDto

                    let totalOtherStudies = (await studyWrapper).totalOtherStudiesDto
                    if (caseItem.totalFeasibilityAndConceptStudiesOverride?.override === true) {
                        feasibility = caseItem.totalFeasibilityAndConceptStudiesOverride
                    }
                    if (caseItem.totalFEEDStudiesOverride?.override === true) {
                        feed = caseItem.totalFEEDStudiesOverride
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
    }, [activeTab])

    const handleCaseNPVChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        newCase.npv = e.currentTarget.value.length > 0 ? Number(e.currentTarget.value) : 0
        setCase(newCase)
    }

    const handleCaseBreakEvenChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        newCase.breakEven = e.currentTarget.value.length > 0 ? Math.max(Number(e.currentTarget.value), 0) : 0
        setCase(newCase)
    }

    interface ITimeSeriesData {
        profileName: string
        unit: string,
        set?: Dispatch<SetStateAction<ITimeSeriesCost | undefined>>,
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

    if (activeTab !== 7) { return null }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Summary</PageTitle>
            </TopWrapper>

            <InputContainer mobileColumns={1} desktopColumns={2} breakPoint={850}>
                <CaseNumberInput
                    onChange={handleCaseNPVChange}
                    defaultValue={caseItem.npv}
                    integer={false}
                    label="NPV before tax"
                    allowNegative
                />
                <CaseNumberInput
                    onChange={handleCaseBreakEvenChange}
                    defaultValue={caseItem.breakEven}
                    integer={false}
                    label="B/E before tax"
                />
            </InputContainer>

            <TableWrapper>
                <CaseTabTable
                    timeSeriesData={opexTimeSeriesData}
                    dg4Year={caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="OPEX"
                    includeFooter={false}
                />
            </TableWrapper>
            <TableWrapper>
                <CaseTabTable
                    timeSeriesData={capexTimeSeriesData}
                    dg4Year={caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="CAPEX"
                    includeFooter
                />
            </TableWrapper>
        </>
    )
}

export default CaseSummaryTab
