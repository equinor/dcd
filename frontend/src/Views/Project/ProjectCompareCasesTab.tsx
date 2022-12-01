/* eslint-disable no-unsafe-optional-chaining */
import styled from "styled-components"
import {
    useEffect, useMemo, useRef, useState,
} from "react"
import { AgGridReact } from "ag-grid-react"
import { customUnitHeaderTemplate } from "../../AgGridUnitInHeader"
import { Project } from "../../models/Project"
import { GetCompareCasesService } from "../../Services/CompareCasesService"

interface Props {
    project: Project
}

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

const Wrapper = styled.div`
width: 40%;
float: left;
padding: 20px;
`

function ProjectCompareCasesTab({
    project,
}: Props) {
    const gridRef = useRef(null)

    const onGridReady = (params: any) => {
        gridRef.current = params.api
    }

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        editable: false,
    }), [])

    const [rowData, setRowData] = useState<TableCompareCase[]>()
    const [compareCasesTotals, setCompareCasesTotals] = useState<any>()

    useEffect(() => {
        (async () => {
            try {
                const compareCasesService = await (await GetCompareCasesService()).calculate(project.id)
                setCompareCasesTotals(compareCasesService)
                console.log(compareCasesService)
            } catch (error) {
                console.error("[ProjectView] Error while generating compareCasesTotals", error)
            }
        })()
    }, [])

    const casesToRowData = () => {
        if (project) {
            const tableCompareCases: TableCompareCase[] = []
            if (compareCasesTotals) {
                project.cases.forEach((c, i) => {
                    const tableCase: TableCompareCase = {
                        id: c.id!,
                        cases: c.name ?? "",
                        description: c.description ?? "",
                        npv: Math.round(c.npv * 1) / 1 ?? 0,
                        breakEven: Math.round(c.breakEven * 1) / 1 ?? 0,
                        oilProduction: Math.round(compareCasesTotals[i]?.totalOilProduction * 10) / 10,
                        gasProduction: Math.round(compareCasesTotals[i]?.totalGasProduction * 10) / 10,
                        totalExportedVolumes: Math.round(compareCasesTotals[i]?.totalExportedVolumes * 10) / 10,
                        studyCostsPlusOpex: Math.round(compareCasesTotals[i]?.totalStudyCostsPlusOpex * 1) / 1,
                        cessationCosts: Math.round(compareCasesTotals[i]?.totalCessationCosts * 1) / 1,
                        // eslint-disable-next-line max-len
                        offshorePlusOnshoreFacilityCosts: Math.round(compareCasesTotals[i]?.offshorePlusOnshoreFacilityCosts * 1) / 1,
                        developmentCosts: Math.round(compareCasesTotals[i]?.developmentWellCosts * 1) / 1,
                        explorationWellCosts: Math.round(compareCasesTotals[i]?.explorationWellCosts * 1) / 1,
                        totalCO2Emissions: Math.round(compareCasesTotals[i]?.totalCo2Emissions * 10) / 10,
                        cO2Intensity: Math.round(compareCasesTotals[i]?.co2Intensity * 10) / 10,
                    }
                    tableCompareCases.push(tableCase)
                })
            }
            setRowData(tableCompareCases)
        }
    }

    useEffect(() => {
        casesToRowData()
    }, [project.cases, compareCasesTotals])

    const columns = () => {
        const columnPinned: any[] = [
            {
                field: "cases", width: 250, pinned: "left", chartDataType: "category",
            },
        ]
        const nonPinnedColumns: any[] = [
            {
                headerName: "Economic KPIs (pre-tax)",
                children: [
                    {
                        field: "npv",
                        headerName: "",
                        width: 175,
                        editable: false,
                        headerComponentParams: {
                            template: customUnitHeaderTemplate("NPV", "mill USD"),
                        },
                    },
                    {
                        field: "breakEven",
                        headerName: "",
                        width: 175,
                        headerComponentParams: {
                            template:
                                customUnitHeaderTemplate("Break even", "USD/bbl"),
                        },
                    },
                ],
            },
            {
                headerName: "Production profiles",
                children: [
                    {
                        field: "oilProduction",
                        headerName: "",
                        width: 175,
                        headerComponentParams: {
                            template: customUnitHeaderTemplate(
                                "Oil production",
                                `${project?.physUnit === 0 ? "MSm3" : "mill bbl"}`,
                            ),
                        },
                    },
                    {
                        field: "gasProduction",
                        headerName: "",
                        width: 175,
                        headerComponentParams: {
                            template: customUnitHeaderTemplate(
                                "Gas production",
                                `${project?.physUnit === 0 ? "GSm3" : "Bscf"}`,
                            ),
                        },
                    },
                    {
                        field: "totalExportedVolumes",
                        headerName: "",
                        width: 175,
                        headerComponentParams: {
                            template: customUnitHeaderTemplate(
                                "Total exported volumes",
                                `${project?.physUnit === 0 ? "mill Sm3" : "mill boe"}`,
                            ),
                        },
                    },
                ],
            },
            {
                headerName: "Cost profiles",
                children: [
                    {
                        field: "studyCostsPlusOpex",
                        headerName: "",
                        width: 175,
                        headerComponentParams: {
                            template: customUnitHeaderTemplate(
                                "Study costs + OPEX",
                                `${project?.currency === 1 ? "mill NOK" : "mill USD"}`,
                            ),
                        },
                    },
                    {
                        field: "cessationCosts",
                        headerName: "",
                        width: 175,
                        headerComponentParams: {
                            template: customUnitHeaderTemplate(
                                "Cessation costs",
                                `${project?.currency === 1 ? "mill NOK" : "mill USD"}`,
                            ),
                        },
                    },
                ],
            },
            {
                headerName: "Investment profiles",
                children: [
                    {
                        field: "offshorePlusOnshoreFacilityCosts",
                        headerName: "",
                        width: 225,
                        headerComponentParams: {
                            template:
                            customUnitHeaderTemplate(
                                "Offshore + Onshore facility costs",
                                `${project?.currency === 1 ? "mill NOK" : "mill USD"}`,
                            ),
                        },
                    },
                    {
                        field: "developmentCosts",
                        headerName: "",
                        width: 175,
                        headerComponentParams: {
                            template: customUnitHeaderTemplate(
                                "Development well costs",
                                `${project?.currency === 1 ? "mill NOK" : "mill USD"}`,
                            ),
                        },
                    },
                    {
                        field: "explorationWellCosts",
                        headerName: "",
                        width: 175,
                        headerComponentParams: {
                            template: customUnitHeaderTemplate(
                                "Exploration well costs",
                                `${project?.currency === 1 ? "mill NOK" : "mill USD"}`,
                            ),
                        },
                    },
                ],
            },
            {
                headerName: "CO2 emissions",
                children: [
                    {
                        field: "totalCO2Emissions",
                        headerName: "",
                        width: 175,
                        headerComponentParams: {
                            template: customUnitHeaderTemplate("Total CO2 emissions", "mill tonnes"),
                        },
                    },
                    {
                        field: "cO2Intensity",
                        headerName: "",
                        width: 175,
                        headerComponentParams: {
                            template: customUnitHeaderTemplate("CO2 intensity", "kg CO2/boe"),
                        },
                    },
                ],
            },
        ]
        return columnPinned.concat([...nonPinnedColumns])
    }

    const [columnDefs] = useState(columns())

    const chartThemes = useMemo<string[]>(() => ["ag-vivid"], [])

    const onFirstDataRendered = (params: any) => {
        const createNPVRangeChartParams = {
            cellRange: {
                rowStartIndex: 0,
                rowEndIndex: 79,
                columns: [
                    "cases",
                    "npv",
                ],
            },
            chartThemeOverrides: {
                common: {
                    title: {
                        enabled: true,
                        text: "NPV",
                    },
                    subtitle: {
                        enabled: true,
                        text: "mill USD",
                        fontSize: 14,
                    },
                    legend: { enabled: false },
                },
                column: { axes: { category: { label: { rotation: 0 } } } },
            },
            unlinkChart: true,
            chartType: "groupedColumn",
            chartContainer: document.querySelector("#myNPVChart"),
            aggFunc: "sum",
        }
        params.api.createRangeChart(createNPVRangeChartParams)
        const createBreakEvenRangeChartParams = {
            cellRange: {
                rowStartIndex: 0,
                rowEndIndex: 79,
                columns: [
                    "cases",
                    "breakEven",
                ],
            },
            chartThemeOverrides: {
                common: {
                    title: {
                        enabled: true,
                        text: "Break even",
                    },
                    subtitle: {
                        enabled: true,
                        text: "USD/bbl",
                        fontSize: 14,
                    },
                    legend: { enabled: false },
                },
                column: { axes: { category: { label: { rotation: 0 } } } },
            },
            unlinkChart: true,
            chartType: "groupedColumn",
            chartContainer: document.querySelector("#myBreakEvenChart"),
            aggFunc: "sum",
        }
        params.api.createRangeChart(createBreakEvenRangeChartParams)
    }

    return (
        <>
            <Wrapper>
                <div id="myNPVChart" className="ag-theme-alpine my-chart" />
            </Wrapper>
            <Wrapper>
                <div id="myBreakEvenChart" className="ag-theme-alpine my-chart" />
            </Wrapper>
            <div
                style={{
                    display: "flex", flexDirection: "column", width: "100%",
                }}
                className="ag-theme-alpine"
            >
                <AgGridReact
                    ref={gridRef}
                    rowData={rowData}
                    columnDefs={columnDefs}
                    defaultColDef={defaultColDef}
                    animateRows
                    domLayout="autoHeight"
                    onGridReady={onGridReady}
                    rowSelection="multiple"
                    enableRangeSelection
                    enableCharts
                    popupParent={document.body}
                    chartThemes={chartThemes}
                    onFirstDataRendered={onFirstDataRendered}
                />
            </div>
        </>
    )
}

export default ProjectCompareCasesTab
