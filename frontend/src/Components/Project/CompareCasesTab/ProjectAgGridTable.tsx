import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { useRef, useMemo, useState } from "react"
import { Tooltip, Icon } from "@equinor/eds-core-react"
import { bookmark_filled } from "@equinor/eds-icons"
import styled from "styled-components"
import { tokens } from "@equinor/eds-tokens"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery } from "@tanstack/react-query"
import CustomHeaderForSecondaryHeader from "../../../CustomHeaderForSecondaryHeader"
import { cellStyleRightAlign } from "../../../Utils/common"
import { projectQueryFn } from "../../../Services/QueryFunctions"

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
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId

    const { data: apiData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    const nameWithReferenceCase = (p: any) => (
        <span>
            {apiData && apiData.referenceCaseId === p.node.data.id
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
                        cellStyle: cellStyleRightAlign,
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
                        cellStyle: cellStyleRightAlign,
                    },
                ],
            },
            {
                headerName: "Production profiles",
                children: [
                    {
                        field: "oilProduction",
                        headerName: `Oil production (${apiData?.physicalUnit === 0 ? "MSm3" : "mill bbl"})`,
                        width: 175,
                        editable: false,
                        headerComponent: CustomHeaderForSecondaryHeader,
                        headerComponentParams: {
                            columnHeader: "Oil production",
                            unit: apiData?.physicalUnit === 0 ? "MSm3" : "mill bbl",
                        },
                        cellStyle: cellStyleRightAlign,
                    },
                    {
                        field: "gasProduction",
                        headerName: `Rich gas production (${apiData?.physicalUnit === 0 ? "GSm3" : "Bscf"})`,
                        width: 175,
                        editable: false,
                        headerComponent: CustomHeaderForSecondaryHeader,
                        headerComponentParams: {
                            columnHeader: "Rich gas production",
                            unit: apiData?.physicalUnit === 0 ? "GSm3" : "Bscf",
                        },
                        cellStyle: cellStyleRightAlign,
                    },
                    {
                        field: "totalExportedVolumes",
                        headerName: `Total exported volumes (${apiData?.physicalUnit === 0 ? "mill Sm3" : "mill boe"})`,
                        width: 175,
                        editable: false,
                        headerComponent: CustomHeaderForSecondaryHeader,
                        headerComponentParams: {
                            columnHeader: "Total exported volumes",
                            unit: apiData?.physicalUnit === 0 ? "mill Sm3" : "mill boe",
                        },
                        cellStyle: cellStyleRightAlign,
                    },
                ],
            },
            {
                headerName: "Cost profiles",
                children: [
                    {
                        field: "studyCostsPlusOpex",
                        headerName: `Study costs + OPEX (${apiData?.currency === 1 ? "mill NOK" : "mill USD"})`,
                        width: 175,
                        editable: false,
                        headerComponent: CustomHeaderForSecondaryHeader,
                        headerComponentParams: {
                            columnHeader: "Study costs + OPEX",
                            unit: apiData?.currency === 1 ? "mill NOK" : "mill USD",
                        },
                        cellStyle: cellStyleRightAlign,
                    },
                    {
                        field: "cessationCosts",
                        headerName: `Cessation costs (${apiData?.currency === 1 ? "mill NOK" : "mill USD"})`,
                        width: 175,
                        editable: false,
                        headerComponent: CustomHeaderForSecondaryHeader,
                        headerComponentParams: {
                            columnHeader: "Cessation costs",
                            unit: apiData?.currency === 1 ? "mill NOK" : "mill USD",
                        },
                        cellStyle: cellStyleRightAlign,
                    },
                ],
            },
            {
                headerName: "Investment profiles",
                children: [
                    {
                        field: "offshorePlusOnshoreFacilityCosts",
                        headerName: `Offshore + Onshore facility costs (${apiData?.currency === 1 ? "mill NOK" : "mill USD"})`,
                        width: 225,
                        editable: false,
                        headerComponent: CustomHeaderForSecondaryHeader,
                        headerComponentParams: {
                            columnHeader: "Offshore + Onshore facility costs",
                            unit: apiData?.currency === 1 ? "mill NOK" : "mill USD",
                        },
                        cellStyle: cellStyleRightAlign,
                    },
                    {
                        field: "developmentCosts",
                        headerName: `Development well costs (${apiData?.currency === 1 ? "mill NOK" : "mill USD"})`,
                        width: 175,
                        editable: false,
                        headerComponent: CustomHeaderForSecondaryHeader,
                        headerComponentParams: {
                            columnHeader: "Development well costs",
                            unit: apiData?.currency === 1 ? "mill NOK" : "mill USD",
                        },
                        cellStyle: cellStyleRightAlign,
                    },
                    {
                        field: "explorationWellCosts",
                        headerName: `Exploration well costs (${apiData?.currency === 1 ? "mill NOK" : "mill USD"})`,
                        width: 175,
                        editable: false,
                        headerComponent: CustomHeaderForSecondaryHeader,
                        headerComponentParams: {
                            columnHeader: "Exploration well costs",
                            unit: apiData?.currency === 1 ? "mill NOK" : "mill USD",
                        },
                        cellStyle: cellStyleRightAlign,
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
                        cellStyle: cellStyleRightAlign,
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
                        cellStyle: cellStyleRightAlign,
                    },
                ],
            },
        ]
        return columnPinned.concat([...nonPinnedColumns])
    }

    const [columnDefs] = useState(columns())
    const onGridReady = (params: any) => {
        gridRef.current = params.api
        params.api.showLoadingOverlay()
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
