import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { useRef, useMemo, useState } from "react"
import { Tooltip, Icon } from "@equinor/eds-core-react"
import { bookmark_filled } from "@equinor/eds-icons"
import styled from "styled-components"
import { tokens } from "@equinor/eds-tokens"
import { useProjectContext } from "../../../Context/ProjectContext"
import CustomHeaderForSecondaryHeader from "../../../CustomHeaderForSecondaryHeader"

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

    const { project } = useProjectContext()

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
                        headerName: "NPV (mill USD)",
                        width: 175,
                        editable: false,
                        headerComponent: CustomHeaderForSecondaryHeader,
                        headerComponentParams: {
                            columnHeader: "NPV",
                            unit: "mill USD",
                        },
                    },
                    {
                        field: "breakEven",
                        headerName: "Break even (USD/bbl)",
                        width: 175,
                        editable: false,
                        headerComponent: CustomHeaderForSecondaryHeader,
                        headerComponentParams: {
                            columnHeader: "Break even",
                            unit: "USD/bbl",
                        },
                    },
                ],
            },
            {
                headerName: "Production profiles",
                children: [
                    {
                        field: "oilProduction",
                        headerName: `Oil production (${project?.physicalUnit === 0 ? "MSm3" : "mill bbl"})`,
                        width: 175,
                        editable: false,
                        headerComponent: CustomHeaderForSecondaryHeader,
                        headerComponentParams: {
                            columnHeader: "Oil production",
                            unit: project?.physicalUnit === 0 ? "MSm3" : "mill bbl",
                        },
                    },
                    {
                        field: "gasProduction",
                        headerName: `Gas production (${project?.physicalUnit === 0 ? "GSm3" : "Bscf"})`,
                        width: 175,
                        editable: false,
                        headerComponent: CustomHeaderForSecondaryHeader,
                        headerComponentParams: {
                            columnHeader: "Gas production",
                            unit: project?.physicalUnit === 0 ? "GSm3" : "Bscf",
                        },
                    },
                    {
                        field: "totalExportedVolumes",
                        headerName: `Total exported volumes (${project?.physicalUnit === 0 ? "mill Sm3" : "mill boe"})`,
                        width: 175,
                        editable: false,
                        headerComponent: CustomHeaderForSecondaryHeader,
                        headerComponentParams: {
                            columnHeader: "Total exported volumes",
                            unit: project?.physicalUnit === 0 ? "mill Sm3" : "mill boe",
                        },
                    },
                ],
            },
            {
                headerName: "Cost profiles",
                children: [
                    {
                        field: "studyCostsPlusOpex",
                        headerName: `Study costs + OPEX (${project?.currency === 1 ? "mill NOK" : "mill USD"})`,
                        width: 175,
                        editable: false,
                        headerComponent: CustomHeaderForSecondaryHeader,
                        headerComponentParams: {
                            columnHeader: "Study costs + OPEX",
                            unit: project?.currency === 1 ? "mill NOK" : "mill USD",
                        },
                    },
                    {
                        field: "cessationCosts",
                        headerName: `Cessation costs (${project?.currency === 1 ? "mill NOK" : "mill USD"})`,
                        width: 175,
                        editable: false,
                        headerComponent: CustomHeaderForSecondaryHeader,
                        headerComponentParams: {
                            columnHeader: "Cessation costs",
                            unit: project?.currency === 1 ? "mill NOK" : "mill USD",
                        },
                    },
                ],
            },
            {
                headerName: "Investment profiles",
                children: [
                    {
                        field: "offshorePlusOnshoreFacilityCosts",
                        headerName: `Offshore + Onshore facility costs (${project?.currency === 1 ? "mill NOK" : "mill USD"})`,
                        width: 225,
                        editable: false,
                        headerComponent: CustomHeaderForSecondaryHeader,
                        headerComponentParams: {
                            columnHeader: "Offshore + Onshore facility costs",
                            unit: project?.currency === 1 ? "mill NOK" : "mill USD",
                        },
                    },
                    {
                        field: "developmentCosts",
                        headerName: `Development well costs (${project?.currency === 1 ? "mill NOK" : "mill USD"})`,
                        width: 175,
                        editable: false,
                        headerComponent: CustomHeaderForSecondaryHeader,
                        headerComponentParams: {
                            columnHeader: "Development well costs",
                            unit: project?.currency === 1 ? "mill NOK" : "mill USD",
                        },
                    },
                    {
                        field: "explorationWellCosts",
                        headerName: `Exploration well costs (${project?.currency === 1 ? "mill NOK" : "mill USD"})`,
                        width: 175,
                        editable: false,
                        headerComponent: CustomHeaderForSecondaryHeader,
                        headerComponentParams: {
                            columnHeader: "Exploration well costs",
                            unit: project?.currency === 1 ? "mill NOK" : "mill USD",
                        },
                    },
                ],
            },
            {
                headerName: "CO2 emissions",
                children: [
                    {
                        field: "totalCO2Emissions",
                        headerName: "Total CO2 emissions (mill tonnes)",
                        width: 175,
                        editable: false,
                        headerComponent: CustomHeaderForSecondaryHeader,
                        headerComponentParams: {
                            columnHeader: "Total CO2 emissions",
                            unit: "mill tonnes",
                        },
                    },
                    {
                        field: "cO2Intensity",
                        headerName: "CO2 intensity (kg CO2/boe)",
                        width: 175,
                        editable: false,
                        headerComponent: CustomHeaderForSecondaryHeader,
                        headerComponentParams: {
                            columnHeader: "CO2 intensity",
                            unit: "kg CO2/boe",
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
        suppressHeaderMenuButton: true,
    }), [])

    return (
        <div className={styles.root}>
            <div
                style={{
                    display: "flex", flexDirection: "column", width: "100%",
                }}
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
