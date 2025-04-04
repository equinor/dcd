import { AgGridReact } from "@ag-grid-community/react"
import { Tooltip, Icon } from "@equinor/eds-core-react"
import { bookmark_filled } from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { useRef, useMemo, useState } from "react"
import styled from "styled-components"

import SecondaryTableHeader from "@/Components/Tables/Components/SecondaryTableHeader"
import { useDataFetch } from "@/Hooks/useDataFetch"
import { cellStyleRightAlign, getCustomContextMenuItems } from "@/Utils/TableUtils"

const MenuIcon = styled(Icon)`
    color: ${tokens.colors.text.static_icons__secondary.rgba};
    margin-right: 8;
    margin-bottom: -4px;
`

const Wrapper = styled.div`
    display: flex;
    flex-direction: column;
    width: 100%;
`

interface props {
    rowData: any
}

const ProjectAgGridTable: React.FC<props> = ({ rowData }) => {
    const revisionAndProjectData = useDataFetch()
    const gridRef = useRef(null)
    const styles = useStyles()

    const nameWithReferenceCase = (p: any) => (
        <span>
            {revisionAndProjectData && revisionAndProjectData.commonProjectAndRevisionData.referenceCaseId === p.node.data.id
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
                        headerComponent: SecondaryTableHeader,
                        headerComponentParams: {
                            columnHeader: "Calculated NPV",
                            unit: "MUSD",
                        },
                        cellStyle: cellStyleRightAlign,
                    },
                    {
                        field: "npvOverride",
                        headerName: "Manually set NPV (mill USD)",
                        width: 175,
                        editable: false,
                        headerComponent: SecondaryTableHeader,
                        headerComponentParams: {
                            columnHeader: "Manually set NPV",
                            unit: "MUSD",
                        },
                        cellStyle: cellStyleRightAlign,
                    },
                    {
                        field: "breakEven",
                        headerName: "Calculated break even (USD/bbl)",
                        width: 190,
                        editable: false,
                        headerComponent: SecondaryTableHeader,
                        headerComponentParams: {
                            columnHeader: "Calculated break even",
                            unit: "USD/bbl",
                        },
                        cellStyle: cellStyleRightAlign,
                    },
                    {
                        field: "breakEvenOverride",
                        headerName: "Manually set break even (USD/bbl)",
                        width: 190,
                        editable: false,
                        headerComponent: SecondaryTableHeader,
                        headerComponentParams: {
                            columnHeader: "Manually set break even",
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
                        headerName: `Oil production (${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === 0 ? "MSm3" : "mill bbl"})`,
                        width: 175,
                        editable: false,
                        headerComponent: SecondaryTableHeader,
                        headerComponentParams: {
                            columnHeader: "Oil production",
                            unit: revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === 0 ? "MSm3" : "mill bbl",
                        },
                        cellStyle: cellStyleRightAlign,
                    },
                    {
                        field: "additionalOilProduction",
                        headerName: `Additional Oil production (${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === 0 ? "MSm3" : "mill bbl"})`,
                        width: 175,
                        editable: false,
                        headerComponent: SecondaryTableHeader,
                        headerComponentParams: {
                            columnHeader: "Additional Oil production",
                            unit: revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === 0 ? "MSm3" : "mill bbl",
                        },
                        cellStyle: cellStyleRightAlign,
                    },
                    {
                        field: "gasProduction",
                        headerName: `Rich gas production (${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === 0 ? "GSm3" : "Bscf"})`,
                        width: 175,
                        editable: false,
                        headerComponent: SecondaryTableHeader,
                        headerComponentParams: {
                            columnHeader: "Rich gas production",
                            unit: revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === 0 ? "GSm3" : "Bscf",
                        },
                        cellStyle: cellStyleRightAlign,
                    },
                    {
                        field: "additionalGasProduction",
                        headerName: `Additional rich gas production (${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === 0 ? "GSm3" : "Bscf"})`,
                        width: 175,
                        editable: false,
                        headerComponent: SecondaryTableHeader,
                        headerComponentParams: {
                            columnHeader: "Additional rich gas production",
                            unit: revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === 0 ? "GSm3" : "Bscf",
                        },
                        cellStyle: cellStyleRightAlign,
                    },
                    {
                        field: "totalExportedVolumes",
                        headerName: `Production & sales volumes (${revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === 0 ? "mill Sm3" : "mill boe"})`,
                        width: 175,
                        editable: false,
                        headerComponent: SecondaryTableHeader,
                        headerComponentParams: {
                            columnHeader: "Production & sales volumes",
                            unit: revisionAndProjectData?.commonProjectAndRevisionData.physicalUnit === 0 ? "mill Sm3" : "mill boe",
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
                        headerName: `Study costs + OPEX (${revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "mill NOK" : "mill USD"})`,
                        width: 175,
                        editable: false,
                        headerComponent: SecondaryTableHeader,
                        headerComponentParams: {
                            columnHeader: "Study costs + OPEX",
                            unit: revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "mill NOK" : "mill USD",
                        },
                        cellStyle: cellStyleRightAlign,
                    },
                    {
                        field: "cessationCosts",
                        headerName: `Cessation costs (${revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "mill NOK" : "mill USD"})`,
                        width: 175,
                        editable: false,
                        headerComponent: SecondaryTableHeader,
                        headerComponentParams: {
                            columnHeader: "Cessation costs",
                            unit: revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "mill NOK" : "mill USD",
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
                        headerName: `Offshore + Onshore facility costs (${revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "mill NOK" : "mill USD"})`,
                        width: 225,
                        editable: false,
                        headerComponent: SecondaryTableHeader,
                        headerComponentParams: {
                            columnHeader: "Offshore + Onshore facility costs",
                            unit: revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "mill NOK" : "mill USD",
                        },
                        cellStyle: cellStyleRightAlign,
                    },
                    {
                        field: "developmentCosts",
                        headerName: `Development well costs (${revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "mill NOK" : "mill USD"})`,
                        width: 175,
                        editable: false,
                        headerComponent: SecondaryTableHeader,
                        headerComponentParams: {
                            columnHeader: "Development well costs",
                            unit: revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "mill NOK" : "mill USD",
                        },
                        cellStyle: cellStyleRightAlign,
                    },
                    {
                        field: "explorationWellCosts",
                        headerName: `Exploration well costs (${revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "mill NOK" : "mill USD"})`,
                        width: 175,
                        editable: false,
                        headerComponent: SecondaryTableHeader,
                        headerComponentParams: {
                            columnHeader: "Exploration well costs",
                            unit: revisionAndProjectData?.commonProjectAndRevisionData.currency === 1 ? "mill NOK" : "mill USD",
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
                        headerComponent: SecondaryTableHeader,
                        headerComponentParams: {
                            columnHeader: "Total CO2 emissions",
                            unit: "mill tonnes",
                        },
                        cellStyle: cellStyleRightAlign,
                    },
                    {
                        field: "co2Intensity",
                        headerName: "CO2 intensity (kg CO2/boe)",
                        width: 175,
                        editable: false,
                        headerComponent: SecondaryTableHeader,
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
            <Wrapper>
                <AgGridReact
                    ref={gridRef}
                    rowData={rowData}
                    columnDefs={columnDefs}
                    defaultColDef={defaultColDef}
                    getContextMenuItems={getCustomContextMenuItems}
                    animateRows
                    domLayout="autoHeight"
                    onGridReady={onGridReady}
                    rowSelection={{
                        mode: "multiRow",
                        checkboxes: false,
                        headerCheckbox: false,
                        enableClickSelection: true,
                    }}
                    cellSelection
                    enableCharts
                />
            </Wrapper>
        </div>
    )
}

export default ProjectAgGridTable
