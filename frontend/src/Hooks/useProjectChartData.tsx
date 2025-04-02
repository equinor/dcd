import { useQuery } from "@tanstack/react-query"
import { useEffect, useMemo, useState } from "react"

import { useDataFetch } from "@/Hooks"
import { compareCasesQueryFn } from "@/Services/QueryFunctions"
import { useProjectContext } from "@/Store/ProjectContext"
import { roundToDecimals } from "@/Utils/FormatingUtils"

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
    co2Intensity: number,
}

export const useProjectChartData = () => {
    const { projectId } = useProjectContext()
    const revisionAndProjectData = useDataFetch()

    const [compareCasesTotals, setCompareCasesTotals] = useState<Components.Schemas.CompareCasesDto[]>()
    const [productionProfilesChartData, setProductionProfilesChartData] = useState<object>()
    const [investmentProfilesChartData, setInvestmentProfilesChartData] = useState<object>()
    const [totalCo2EmissionsChartData, setTotalCo2EmissionsChartData] = useState<object>()
    const [co2IntensityChartData, setCo2IntensityChartData] = useState<object>()
    const [breakEvenChartData, setBreakEvenChartData] = useState<object>()
    const [rowData, setRowData] = useState<TableCompareCase[]>()
    const [npvChartData, setNpvChartData] = useState<object>()

    const { data: compareCasesData } = useQuery({
        queryKey: ["compareCasesApiData", projectId],
        queryFn: () => compareCasesQueryFn(projectId),
        enabled: !!projectId,
    })

    const cases = useMemo(
        () => revisionAndProjectData?.commonProjectAndRevisionData.cases.filter((c) => !c.archived) || [],
        [revisionAndProjectData],
    )

    const generateAllCharts = () => {
        if (!compareCasesTotals || !revisionAndProjectData) { return }

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
            const compareCase = compareCasesTotals.find((c) => c.caseId === caseItem.caseId)

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
            const compareCase = compareCasesTotals.find((c) => c.caseId === caseItem.caseId)

            return {
                cases: caseItem.name,
                offshorePlusOnshoreFacilityCosts: compareCase?.offshorePlusOnshoreFacilityCosts,
                developmentCosts: compareCase?.developmentWellCosts,
                explorationWellCosts: compareCase?.explorationWellCosts,
            }
        })

        const totalCo2EmissionsObject = cases.map((caseItem) => {
            const compareCase = compareCasesTotals.find((c) => c.caseId === caseItem.caseId)
            const co2EmissionsValue = compareCase?.totalCo2Emissions ?? 0

            return {
                cases: caseItem.name,
                totalCO2Emissions: co2EmissionsValue,
            }
        })

        const co2IntensityObject = cases.map((caseItem) => {
            const compareCase = compareCasesTotals.find((c) => c.caseId === caseItem.caseId)
            const co2IntensityValue = compareCase?.co2Intensity ?? 0

            return {
                cases: caseItem.name,
                co2Intensity: co2IntensityValue,
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
        if (revisionAndProjectData) {
            const tableCompareCases: TableCompareCase[] = []

            if (compareCasesTotals) {
                cases.forEach((c) => {
                    const matchingCase = compareCasesTotals.find((checkMatchingCase: any) => checkMatchingCase.caseId === c.caseId)

                    if (matchingCase) {
                        const tableCase: TableCompareCase = {
                            id: c.caseId,
                            cases: c.name ?? "",
                            description: c.description ?? "",
                            npv: roundToDecimals(c.npv ?? 0, 0),
                            npvOverride: roundToDecimals(c.npvOverride ?? 0, 0),
                            breakEven: roundToDecimals(c.breakEven ?? 0, 0),
                            breakEvenOverride: roundToDecimals(c.breakEvenOverride ?? 0, 0),
                            oilProduction: roundToDecimals(matchingCase.totalOilProduction, 1),
                            additionalOilProduction: roundToDecimals(matchingCase.additionalOilProduction, 1),
                            gasProduction: roundToDecimals(matchingCase.totalGasProduction, 1),
                            additionalGasProduction: roundToDecimals(matchingCase.additionalGasProduction, 1),
                            totalExportedVolumes: roundToDecimals(matchingCase.totalExportedVolumes, 1),
                            studyCostsPlusOpex: roundToDecimals(matchingCase.totalStudyCostsPlusOpex, 0),
                            cessationCosts: roundToDecimals(matchingCase.totalCessationCosts, 0),
                            offshorePlusOnshoreFacilityCosts: roundToDecimals(matchingCase.offshorePlusOnshoreFacilityCosts, 0),
                            developmentCosts: roundToDecimals(matchingCase.developmentWellCosts, 0),
                            explorationWellCosts: roundToDecimals(matchingCase.explorationWellCosts, 0),
                            totalCO2Emissions: roundToDecimals(matchingCase.totalCo2Emissions, 1),
                            co2Intensity: roundToDecimals(matchingCase.co2Intensity, 1),
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
            const casesOrderedByGuid = compareCasesData.sort((a, b) => a.caseId.localeCompare(b.caseId))

            setCompareCasesTotals(casesOrderedByGuid)
        }
    }, [compareCasesData])

    useEffect(() => {
        if (revisionAndProjectData) {
            casesToRowData()
            generateAllCharts()
        }
    }, [revisionAndProjectData, compareCasesTotals])

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
