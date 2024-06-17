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
    const [compareCasesTotals, setCompareCasesTotals] = useState<any>()
    const [npvChartData, setNpvChartData] = useState<object>()
    const [breakEvenChartData, setBreakEvenChartData] = useState<object>()
    const [productionProfilesChartData, setProductionProfilesChartData] = useState<object>()
    const [investmentProfilesChartData, setInvestmentProfilesChartData] = useState<object>()
    const [totalCo2EmissionsChartData, setTotalCo2EmissionsChartData] = useState<object>()
    const [co2IntensityChartData, setCo2IntensityChartData] = useState<object>()

    const generateAllCharts = () => {
        const npvObject: object[] = []
        const breakEvenObject: object[] = []
        const productionProfilesObject: object[] = []
        const investmentProfilesObject: object[] = []
        const totalCo2EmissionsObject: object[] = []
        const co2IntensityObject: object[] = []
        if (compareCasesTotals !== undefined && project) {
            for (let i = 0; i < project.cases.length; i += 1) {
                npvObject.push({
                    cases: project.cases[i].name,
                    npv: project.cases[i].npv,
                })
                breakEvenObject.push({
                    cases: project.cases[i].name,
                    breakEven: project.cases[i].breakEven,
                })
                productionProfilesObject.push({
                    cases: project.cases[i].name,
                    oilProduction: compareCasesTotals[i]?.totalOilProduction,
                    gasProduction: compareCasesTotals[i]?.totalGasProduction,
                    totalExportedVolumes: compareCasesTotals[i]?.totalExportedVolumes,
                })
                investmentProfilesObject.push({
                    cases: project.cases[i].name,
                    offshorePlusOnshoreFacilityCosts: compareCasesTotals[i]?.offshorePlusOnshoreFacilityCosts,
                    developmentCosts: compareCasesTotals[i]?.developmentWellCosts,
                    explorationWellCosts: compareCasesTotals[i]?.explorationWellCosts,
                })
                totalCo2EmissionsObject.push({
                    cases: project.cases[i].name,
                    totalCO2Emissions: compareCasesTotals[i]?.totalCo2Emissions,
                })
                co2IntensityObject.push({
                    cases: project.cases[i].name,
                    cO2Intensity: compareCasesTotals[i]?.co2Intensity,
                })
            }
        }
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
                            npv: Math.round(c.npv ?? 0 * 1) / 1 ?? 0,
                            breakEven: Math.round(c.breakEven ?? 0 * 1) / 1 ?? 0,
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
