/* eslint-disable camelcase */
/* eslint-disable no-unsafe-optional-chaining */
import styled from "styled-components"
import {
    useEffect, useMemo, useRef, useState,
} from "react"
import { AgGridReact } from "@ag-grid-community/react"
import {
    Icon,
    Tabs, Tooltip,
} from "@equinor/eds-core-react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { bookmark_filled } from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import { customUnitHeaderTemplate } from "../../AgGridUnitInHeader"
import { Project } from "../../models/Project"
import { GetCompareCasesService } from "../../Services/CompareCasesService"
import { AgChartsCompareCases } from "../../Components/AgGrid/AgChartsCompareCases"

const MenuIcon = styled(Icon)`
    color: ${tokens.colors.text.static_icons__secondary.rgba};
    margin-right: 0.5rem;
    margin-bottom: -0.2rem;
`

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

const WrapperTabs = styled.div`
width: 100%;
display: flex;
float: left;
flex-direction: column;
padding: 20px;
`
const WrapperRow = styled.div`
    display: flex;
    flex-direction: row;
    align-content: center;
    margin-bottom: 1rem;
    margin-top: 1rem;
    `
const { Panel } = Tabs
const { List, Tab, Panels } = Tabs

const StyledTabPanel = styled(Panel)`
    padding-top: 0px;
    border-top: 1px solid LightGray;
`

function ProjectCompareCasesTab({
    project,
}: Props) {
    const gridRef = useRef(null)
    const styles = useStyles()

    const onGridReady = (params: any) => {
        gridRef.current = params.api
    }

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        editable: false,
        suppressMenu: true
    }), [])

    const [rowData, setRowData] = useState<TableCompareCase[]>()
    const [compareCasesTotals, setCompareCasesTotals] = useState<any>()

    const [npvChartData, setNpvChartData] = useState<object>()
    const [breakEvenChartData, setBreakEvenChartData] = useState<object>()
    const [productionProfilesChartData, setProductionProfilesChartData] = useState<object>()
    const [investmentProfilesChartData, setInvestmentProfilesChartData] = useState<object>()
    const [totalCo2EmissionsChartData, setTotalCo2EmissionsChartData] = useState<object>()
    const [co2IntensityChartData, setCo2IntensityChartData] = useState<object>()

    useEffect(() => {
        (async () => {
            try {
                const compareCasesService = await (await GetCompareCasesService()).calculate(project.id)
                const casesOrderedByGuid = compareCasesService.sort((a, b) => a.caseId!.localeCompare(b.caseId!))
                setCompareCasesTotals(casesOrderedByGuid)
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
                        npv: Math.round(c.npv ?? 0 * 1) / 1 ?? 0,
                        breakEven: Math.round(c.breakEven ?? 0 * 1) / 1 ?? 0,
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

    const generateAllCharts = () => {
        const npvObject: object[] = []
        const breakEvenObject: object[] = []
        const productionProfilesObject: object[] = []
        const investmentProfilesObject: object[] = []
        const totalCo2EmissionsObject: object[] = []
        const co2IntensityObject: object[] = []
        if (compareCasesTotals !== undefined) {
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

    useEffect(() => {
        casesToRowData()
        generateAllCharts()
    }, [project.cases, compareCasesTotals])

    const nameWithReferenceCase = (p: any) => (
        <span>
            {project.referenceCaseId === p.node.data.id
                        && (
                            <Tooltip title="Reference case">
                                <MenuIcon data={bookmark_filled} size={16} />
                            </Tooltip>
                        )}
            <span>{p.value}</span>
        </span>
    )

    const columns = () => {
        const columnPinned: any[] = [
            {
                field: "cases",
                width: 250,
                pinned: "left",
                chartDataType: "category",
                cellRenderer: nameWithReferenceCase,
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
    const [activeTab, setActiveTab] = useState(0)

    return (
        <>
            <WrapperTabs>
                <Tabs activeTab={activeTab} onChange={setActiveTab}>
                    <List>
                        <Tab>KPIs</Tab>
                        <Tab>Production profiles</Tab>
                        <Tab>Investment profiles</Tab>
                        <Tab>CO2 emissions</Tab>
                    </List>
                    <Panels>
                        <StyledTabPanel>
                            <WrapperRow>
                                <AgChartsCompareCases
                                    data={npvChartData}
                                    chartTitle="NPV"
                                    barColors={["#005F57"]}
                                    barProfiles={["npv"]}
                                    barNames={["NPV"]}
                                    unit="mill USD"
                                    width="50%"
                                    height={400}
                                    enableLegend={false}
                                />
                                <AgChartsCompareCases
                                    data={breakEvenChartData}
                                    chartTitle="Break even"
                                    barColors={["#00977B"]}
                                    barProfiles={["breakEven"]}
                                    barNames={["Break even"]}
                                    unit="USD/bbl"
                                    width="50%"
                                    height={400}
                                    enableLegend={false}
                                />
                            </WrapperRow>
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <AgChartsCompareCases
                                data={productionProfilesChartData}
                                chartTitle="Production profiles"
                                barColors={["#243746", "#EB0037", "#8C1159"]}
                                barProfiles={["oilProduction", "gasProduction", "totalExportedVolumes"]}
                                barNames={[
                                    "Oil production (MSm3)",
                                    "Gas production (GSm3)",
                                    "Total exported volumes (MSm3)",
                                ]}
                                height={432}
                            />
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <AgChartsCompareCases
                                data={investmentProfilesChartData}
                                chartTitle="Investment profiles"
                                barColors={["#005F57", "#00977B", "#40D38F"]}
                                barProfiles={["offshorePlusOnshoreFacilityCosts",
                                    "developmentCosts", "explorationWellCosts"]}
                                barNames={[
                                    "Offshore + Onshore facility costs",
                                    "Development well costs",
                                    "Exploration well costs",
                                ]}
                                unit={`${project?.currency === 1 ? "mill NOK" : "mill USD"}`}
                                height={432}
                            />
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <WrapperRow>
                                <AgChartsCompareCases
                                    data={totalCo2EmissionsChartData}
                                    chartTitle="Total CO2 emissions"
                                    barColors={["#E24973"]}
                                    barProfiles={["totalCO2Emissions"]}
                                    barNames={["Total CO2 emissions"]}
                                    unit="mill tonnes"
                                    width="50%"
                                    height={400}
                                    enableLegend={false}
                                />
                                <AgChartsCompareCases
                                    data={co2IntensityChartData}
                                    chartTitle="CO2 intensity"
                                    barColors={["#FF92A8"]}
                                    barProfiles={["cO2Intensity"]}
                                    barNames={["CO2 intensity"]}
                                    unit="kg CO2/boe"
                                    width="50%"
                                    height={400}
                                    enableLegend={false}
                                />
                            </WrapperRow>
                        </StyledTabPanel>
                    </Panels>
                </Tabs>
            </WrapperTabs>
            <div className={styles.root}>
                <div
                    style={{
                        display: "flex", flexDirection: "column", width: "100%",
                    }}
                    className="ag-theme-alpine-fusion"
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
                    />
                </div>
            </div>
        </>
    )
}

export default ProjectCompareCasesTab
