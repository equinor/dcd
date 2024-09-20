/* eslint-disable no-unsafe-optional-chaining */

import { useEffect, useState } from "react"
import { useProjectContext } from "../Context/ProjectContext"
import { GetProjectService } from "../Services/ProjectService"

interface TableCompareCase {
    id: string,
    cases: string,
    description: string,
    npv: number,
    breakEven: number,
    oilProduction: number,
    gasProduction: number,
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
    const { project } = useProjectContext()

    const [rowData, setRowData] = useState<TableCompareCase[]>()
    const [compareCasesTotals, setCompareCasesTotals] = useState<Components.Schemas.CompareCasesDto[]>()
    const [npvChartData, setNpvChartData] = useState<object>()
    const [breakEvenChartData, setBreakEvenChartData] = useState<object>()
    const [productionProfilesChartData, setProductionProfilesChartData] = useState<object>()
    const [investmentProfilesChartData, setInvestmentProfilesChartData] = useState<object>()
    const [totalCo2EmissionsChartData, setTotalCo2EmissionsChartData] = useState<object>()
    const [co2IntensityChartData, setCo2IntensityChartData] = useState<object>()

        const generateAllCharts = () => {
        if (!compareCasesTotals || !project) return

        const npvObject = project.cases.map(caseItem => ({
            cases: caseItem.name,
            npv: caseItem.npv,
        }))

        const breakEvenObject = project.cases.map(caseItem => ({
            cases: caseItem.name,
            breakEven: caseItem.breakEven,
        }))

        const productionProfilesObject = project.cases.map(caseItem => {
            const compareCase = compareCasesTotals.find(c => c.caseId === caseItem.id)
            return {
                cases: caseItem.name,
                oilProduction: compareCase?.totalOilProduction,
                gasProduction: compareCase?.totalGasProduction,
                totalExportedVolumes: compareCase?.totalExportedVolumes,
            }
        })

        const investmentProfilesObject = project.cases.map(caseItem => {
            const compareCase = compareCasesTotals.find(c => c.caseId === caseItem.id)
            return {
                cases: caseItem.name,
                offshorePlusOnshoreFacilityCosts: compareCase?.offshorePlusOnshoreFacilityCosts,
                developmentCosts: compareCase?.developmentWellCosts,
                explorationWellCosts: compareCase?.explorationWellCosts,
            }
        })

        const totalCo2EmissionsObject = project.cases.map(caseItem => {
            const compareCase = compareCasesTotals.find(c => c.caseId === caseItem.id)
            return {
                cases: caseItem.name,
                totalCO2Emissions: compareCase?.totalCo2Emissions,
            }
        })

        const co2IntensityObject = project.cases.map(caseItem => {
            const compareCase = compareCasesTotals.find(c => c.caseId === caseItem.id)
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
        if (project) {
            const tableCompareCases: TableCompareCase[] = []
            if (compareCasesTotals) {
                project.cases.forEach((c) => {
                    const matchingCase = compareCasesTotals.find((checkMatchingCase: any) => checkMatchingCase.caseId === c.id)
                    if (matchingCase) {
                        const tableCase: TableCompareCase = {
                            id: c.id!,
                            cases: c.name ?? "",
                            description: c.description ?? "",
                            npv: Math.round(c.npv ?? 0 * 1) / 1,
                            breakEven: Math.round(c.breakEven ?? 0 * 1) / 1,
                            oilProduction: Math.round(matchingCase.totalOilProduction * 10) / 10,
                            gasProduction: Math.round(matchingCase.totalGasProduction * 10) / 10,
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
        if (project) {
            (async () => {
                try {
                    const compareCasesService = await (await GetProjectService()).compareCases(project.id)
                    const casesOrderedByGuid = compareCasesService.sort((a, b) => a.caseId!.localeCompare(b.caseId!))
                    setCompareCasesTotals(casesOrderedByGuid)
                } catch (error) {
                    console.error("[ProjectView] Error while generating compareCasesTotals", error)
                }
            })()
        }
    }, [])

    useEffect(() => {
        if (project) {
            casesToRowData()
            generateAllCharts()
        }
    }, [project?.cases, compareCasesTotals])

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
