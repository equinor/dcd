import { AgGridReact } from "ag-grid-react"
import {
    useEffect, useMemo, useRef, useState,
} from "react"
import { Project } from "../../models/Project"

interface Props {
    project: Project,
}

function CaseCO2Distribution({
    project,
}: Props) {
    const gridRef = useRef(null)

    const onGridReady = (params: any) => {
        gridRef.current = params.api
    }

    const [rowData, setRowData] = useState()

    useEffect(() => {
    }, [])

    const [columnDefs] = useState([
        {
            field: "profile", headerName: "CO2 distribution", width: 110, flex: 1,
        },
        {
            field: "expectedProfile", headerName: "Expected profile", width: 200,
        },
        {
            field: "maxProfile", headerName: "Max profile", width: 200,
        },
    ])

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        editable: true,
    }), [])

    return (
        <div
            style={{
                display: "flex", flexDirection: "column", width: "50%",
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
            />
        </div>
    )
}

export default CaseCO2Distribution
