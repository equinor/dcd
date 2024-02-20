import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { useRef, useMemo, useState } from "react"
import { Tooltip, Icon } from "@equinor/eds-core-react"
import { bookmark_filled } from "@equinor/eds-icons"
import styled from "styled-components"
import { tokens } from "@equinor/eds-tokens"
import { useAppContext } from "../../../Context/AppContext"
import { customUnitHeaderTemplate } from "../../../AgGridUnitInHeader"

const MenuIcon = styled(Icon)`
    color: ${tokens.colors.text.static_icons__secondary.rgba};
    margin-right: 0.5rem;
    margin-bottom: -0.2rem;
`

interface props {
    rowData: any
}

const ProjectAgGridTable: React.FC<props> = ({ rowData }) => {
    const styles = useStyles()
    const gridRef = useRef(null)

    const { project } = useAppContext()

    const nameWithReferenceCase = (p: any) => (
        <span>
            {project && project.referenceCaseId === p.node.data.id
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
    const onGridReady = (params: any) => {
        gridRef.current = params.api
    }

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        editable: false,
        suppressMenu: true,
    }), [])

    return (
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
    )
}

export default ProjectAgGridTable
