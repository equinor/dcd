import { useState, useMemo } from "react"
import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { ColDef } from "@ag-grid-community/core"
import Grid from "@mui/material/Grid2"

import SwitchableNumberInput from "@/Components/Input/SwitchableNumberInput"
import { cellStyleRightAlign } from "@/Utils/common"

interface Props {
    addEdit: any
    caseData: any // Replace with proper type when we have the data structure
}

const DrillingCampaign = ({ addEdit, caseData }: Props) => {
    const styles = useStyles()
    const [rigUpgradingCost, setRigUpgradingCost] = useState<number>(4)
    const [rigMobDemobCost, setRigMobDemobCost] = useState<number>(4)

    // This is placeholder data - replace with actual data structure
    const [rowData] = useState([
        {
            name: "Rig upgrading",
            2020: 0,
            2021: 0,
            2022: 0,
            2023: 1,
            2024: 0,
            2025: 0,
            2026: 0,
            2027: 0,
            2028: 0,
            2029: 0,
            total: 1,
        },
        {
            name: "Rig mob/demob",
            2020: 0,
            2021: 0,
            2022: 0,
            2023: 0.5,
            2024: 0,
            2025: 0,
            2026: 0,
            2027: 0.5,
            2028: 0,
            2029: 0,
            total: 1,
        },
        {
            name: "Exploration well -discovery",
            2020: 0,
            2021: 0,
            2022: 0,
            2023: 0,
            2024: 0,
            2025: 0,
            2026: 0,
            2027: 0,
            2028: 0,
            2029: 0,
            total: 1,
        },
        {
            name: "Exploration well - dry",
            2020: 0,
            2021: 0,
            2022: 0,
            2023: 0,
            2024: 0,
            2025: 0,
            2026: 0,
            2027: 0,
            2028: 0,
            2029: 0,
            total: 1,
        },
        {
            name: "Sidetrack",
            2020: 0,
            2021: 0,
            2022: 0,
            2023: 0.5,
            2024: 0,
            2025: 0.5,
            2026: 0,
            2027: 0,
            2028: 0,
            2029: 0,
            total: 1,
        },
        {
            name: "Appraisal well",
            2020: 0,
            2021: 0,
            2022: 0,
            2023: 2,
            2024: 0,
            2025: 0,
            2026: 1,
            2027: 0,
            2028: 0,
            2029: 0,
            total: 1,
        },
        {
            name: "Appraisal well with testing",
            2020: 0,
            2021: 0,
            2022: 0,
            2023: 0,
            2024: 0,
            2025: 0,
            2026: 0,
            2027: 0,
            2028: 0,
            2029: 0,
            total: 1,
        },
    ])

    const columnDefs = useMemo<ColDef[]>(() => [
        {
            field: "name",
            headerName: "Exploration campaign",
            width: 250,
            editable: false,
            pinned: "left",
        },
        {
            field: "2020",
            width: 100,
            cellStyle: cellStyleRightAlign,
        },
        {
            field: "2021",
            width: 100,
            cellStyle: cellStyleRightAlign,
        },
        {
            field: "2022",
            width: 100,
            cellStyle: cellStyleRightAlign,
        },
        {
            field: "2023",
            width: 100,
            cellStyle: cellStyleRightAlign,
        },
        {
            field: "2024",
            width: 100,
            cellStyle: cellStyleRightAlign,
        },
        {
            field: "2025",
            width: 100,
            cellStyle: cellStyleRightAlign,
        },
        {
            field: "2026",
            width: 100,
            cellStyle: cellStyleRightAlign,
        },
        {
            field: "2027",
            width: 100,
            cellStyle: cellStyleRightAlign,
        },
        {
            field: "2028",
            width: 100,
            cellStyle: cellStyleRightAlign,
        },
        {
            field: "2029",
            width: 100,
            cellStyle: cellStyleRightAlign,
        },
        {
            field: "total",
            headerName: "Total",
            width: 100,
            editable: false,
            pinned: "right",
            cellStyle: { fontWeight: "bold", textAlign: "right" },
        },
    ], [])

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        suppressHeaderMenuButton: true,
    }), [])

    return (
        <Grid container spacing={2}>
            <Grid size={{ xs: 12, md: 6 }}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="case"
                    resourcePropertyKey="producerCount"
                    label="Rig upgrading cost - Exploration"
                    previousResourceObject={caseData}
                    value={rigUpgradingCost}
                    unit="MUSD"
                    integer
                />
            </Grid>
            <Grid size={{ xs: 12, md: 6 }}>
                <SwitchableNumberInput
                    addEdit={addEdit}
                    resourceName="case"
                    resourcePropertyKey="producerCount"
                    label="Rig mob/demob cost - Exploration"
                    previousResourceObject={caseData}
                    value={rigMobDemobCost}
                    unit="MUSD"
                    integer
                />
            </Grid>
            <Grid size={12}>
                <div className={styles.root}>
                    <div style={{
                        display: "flex",
                        flexDirection: "column",
                        width: "100%",
                        height: "400px",
                    }}
                    >
                        <AgGridReact
                            rowData={rowData}
                            columnDefs={columnDefs}
                            defaultColDef={defaultColDef}
                            animateRows
                            domLayout="autoHeight"
                            suppressMovableColumns
                            enableCharts
                            stopEditingWhenCellsLoseFocus
                            suppressLastEmptyLineOnPaste
                        />
                    </div>
                </div>
            </Grid>
        </Grid>
    )
}

export default DrillingCampaign
