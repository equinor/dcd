import { useEffect, useMemo, useState } from "react"
import { useQuery } from "@tanstack/react-query"
import { useParams } from "react-router"
import { projectQueryFn, compareCasesQueryFn, revisionQueryFn } from "@/Services/QueryFunctions"
import { useProjectContext } from "@/Context/ProjectContext"

interface TableCompareCase {
    id: string,
    cases: string,
    description: string,
    npv: number,
    npvOverride?: number,
    breakEven: number,
    breakEvenOverride?: number,
    oilProduction: number,
    additionalOilProduction: number,
    gasProduction: number,
    additionalGasProduction: number,
    totalExportedVolumes: number,
    studyCostsPlusOpex: number,
    cessationCosts: number,
    offshorePlusOnshoreFacilityCosts: number,
    developmentCosts: number,
    explorationWellCosts: number,
    totalCO2Emissions: number,
    cO2Intensity: number,
}

export const useProjectChartData = () => {
    const { projectId, isRevision } = useProjectContext()
    const { revisionId } = useParams()
    const [rowData, setRowData] = useState<TableCompareCase[]>()
    const [compareCasesTotals, setCompareCasesTotals] = useState<Components.Schemas.CompareCasesDto[]>()
    const [npvChartData, setNpvChartData] = useState<object>()
    const [breakEvenChartData, setBreakEvenChartData] = useState<object>()
    const [productionProfilesChartData, setProductionProfilesChartData] = useState<object>()
    const [investmentProfilesChartData, setInvestmentProfilesChartData] = useState<object>()
    const [totalCo2EmissionsChartData, setTotalCo2EmissionsChartData] = useState<object>()
    const [co2IntensityChartData, setCo2IntensityChartData] = useState<object>()

    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", projectId],
        queryFn: () => projectQueryFn(projectId),
        enabled: !!projectId,
    })

    const { data: apiRevisionData } = useQuery({
        queryKey: ["revisionApiData", revisionId],
        queryFn: () => revisionQueryFn(projectId, revisionId),
        enabled: !!projectId && !!revisionId && isRevision,
    })

    const { data: compareCasesData } = useQuery({
        queryKey: ["compareCasesApiData", projectId],
        queryFn: () => compareCasesQueryFn(projectId),
        enabled: !!projectId,
    })

    const cases = useMemo(() => {
        if(isRevision) {
            return apiRevisionData?.commonProjectAndRevisionData.cases.filter((c) => !c.archived) || []
        } else return apiData?.commonProjectAndRevisionData.cases.filter((c) => !c.archived) || []
    }, [apiData, isRevision, apiRevisionData])

    const generateAllCharts = () => {
        if (!compareCasesTotals || !apiData) { return }

        const npvObject = cases.map((caseItem) => ({
            cases: caseItem.name,
            npv: caseItem.npv,
            npvOverride: caseItem.npvOverride ?? null,
        }))

        const breakEvenObject = cases.map((caseItem) => ({
            cases: caseItem.name,
            breakEven: caseItem.breakEven,
            breakEvenOverride: caseItem.breakEvenOverride ?? null,
        }))

        const productionProfilesObject = cases.map((caseItem) => {
            const compareCase = compareCasesTotals.find((c) => c.caseId === caseItem.id)
            return {
                cases: caseItem.name,
                oilProduction: compareCase?.totalOilProduction,
                additionalOilProduction: compareCase?.additionalOilProduction,
                gasProduction: compareCase?.totalGasProduction,
                additionalGasProduction: compareCase?.additionalGasProduction,
                totalExportedVolumes: (compareCase?.totalExportedVolumes ?? 0), // + (compareCase?.additionalOilProduction ?? 0) + (compareCase?.additionalGasProduction ?? 0),
            }
        })

        const investmentProfilesObject = cases.map((caseItem) => {
            const compareCase = compareCasesTotals.find((c) => c.caseId === caseItem.id)
            return {
                cases: caseItem.name,
                offshorePlusOnshoreFacilityCosts: compareCase?.offshorePlusOnshoreFacilityCosts,
                developmentCosts: compareCase?.developmentWellCosts,
                explorationWellCosts: compareCase?.explorationWellCosts,
            }
        })

        const totalCo2EmissionsObject = cases.map((caseItem) => {
            const compareCase = compareCasesTotals.find((c) => c.caseId === caseItem.id)
            return {
                cases: caseItem.name,
                totalCO2Emissions: compareCase?.totalCo2Emissions,
            }
        })

        const co2IntensityObject = cases.map((caseItem) => {
            const compareCase = compareCasesTotals.find((c) => c.caseId === caseItem.id)
            return {
                cases: caseItem.name,
                cO2Intensity: compareCase?.co2Intensity,
            }
        })

        setNpvChartData(npvObject)
        setBreakEvenChartData(breakEvenObject)
        setProductionProfilesChartData(productionProfilesObject)
        setInvestmentProfilesChartData(investmentProfilesObject)
        setTotalCo2EmissionsChartData(totalCo2EmissionsObject)
        setCo2IntensityChartData(co2IntensityObject)
    }

    // Convert cases to rowData
    const casesToRowData = () => {
        if (apiData) {
            const tableCompareCases: TableCompareCase[] = []
            if (compareCasesTotals) {
                cases.forEach((c) => {
                    const matchingCase = compareCasesTotals.find((checkMatchingCase: any) => checkMatchingCase.caseId === c.id)
                    if (matchingCase) {
                        const tableCase: TableCompareCase = {
                            id: c.id!,
                            cases: c.name ?? "",
                            description: c.description ?? "",
                            npv: Math.round(c.npv ?? 0 * 1) / 1,
                            npvOverride: Math.round(c.npvOverride ?? 0 * 1) / 1,
                            breakEven: Math.round(c.breakEven ?? 0 * 1) / 1,
                            breakEvenOverride: Math.round(c.breakEvenOverride ?? 0 * 1) / 1,
                            oilProduction: Math.round(matchingCase.totalOilProduction * 10) / 10,
                            additionalOilProduction: Math.round(matchingCase.additionalOilProduction * 10) / 10,
                            gasProduction: Math.round(matchingCase.totalGasProduction * 10) / 10,
                            additionalGasProduction: Math.round(matchingCase.additionalGasProduction * 10) / 10,
                            totalExportedVolumes: Math.round(matchingCase.totalExportedVolumes * 10) / 10,
                            studyCostsPlusOpex: Math.round(matchingCase.totalStudyCostsPlusOpex * 1) / 1,
                            cessationCosts: Math.round(matchingCase.totalCessationCosts * 1) / 1,
                            offshorePlusOnshoreFacilityCosts: Math.round(matchingCase.offshorePlusOnshoreFacilityCosts * 1) / 1,
                            developmentCosts: Math.round(matchingCase.developmentWellCosts * 1) / 1,
                            explorationWellCosts: Math.round(matchingCase.explorationWellCosts * 1) / 1,
                            totalCO2Emissions: Math.round(matchingCase.totalCo2Emissions * 10) / 10,
                            cO2Intensity: Math.round(matchingCase.co2Intensity * 10) / 10,
                        }
                        tableCompareCases.push(tableCase)
                    }
                })
            }
            setRowData(tableCompareCases)
        }
    }

    // Fetch compareCasesTotals and set it to state
    useEffect(() => {
        if (compareCasesData) {
            const casesOrderedByGuid = compareCasesData.sort((a, b) => a.caseId!.localeCompare(b.caseId!))
            setCompareCasesTotals(casesOrderedByGuid)
        }
    }, [compareCasesData])

    useEffect(() => {
        if (apiData || apiRevisionData) {
            casesToRowData()
            generateAllCharts()
        }
    }, [apiData, apiRevisionData, compareCasesTotals])

    return {
        rowData,
        npvChartData,
        breakEvenChartData,
        productionProfilesChartData,
        investmentProfilesChartData,
        totalCo2EmissionsChartData,
        co2IntensityChartData,
    }
}
