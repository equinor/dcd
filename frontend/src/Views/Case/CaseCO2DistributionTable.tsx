import { AgGridReact } from "ag-grid-react"
import {
    useEffect, useMemo, useRef, useState,
} from "react"
import { lock } from "@equinor/eds-icons"
import { Icon } from "@equinor/eds-core-react"
import { Topside } from "../../models/assets/topside/Topside"

interface Props {
    topside: Topside,
}

function CaseCO2DistributionTable({
    topside,
}: Props) {
    const gridRef = useRef(null)

    const onGridReady = (params: any) => {
        gridRef.current = params.api
    }

    const lockIcon = (params: any) => {
        if (!params.data.set) {
            return <Icon data={lock} color="#007079" />
        }
        return null
    }

    const co2Data = [
        {
            profile: "Oil profile",
            expectedProfile: topside?.cO2ShareOilProfile,
            maxProfile: topside?.cO2OnMaxOilProfile,
        },
        {
            profile: "Gas profile",
            expectedProfile: topside?.cO2ShareGasProfile,
            maxProfile: topside?.cO2OnMaxGasProfile,
        },
        {
            profile: "Water injection profile",
            expectedProfile: topside?.cO2ShareWaterInjectionProfile,
            maxProfile: topside?.cO2OnMaxWaterInjectionProfile,
        },
    ]

    const [rowData, setRowData] = useState(co2Data)

    useEffect(() => {
    }, [])

    const [columnDefs] = useState([
        {
            field: "profile",
            headerName: "CO2 distribution",
            width: 110,
            flex: 1,
            editable: false,
        },
        {
            field: "expectedProfile",
            headerName: "Expected profile",
            width: 200,
            editable: false,
        },
        {
            field: "maxProfile",
            headerName: "Max profile",
            width: 200,
            editable: false,
        },
        {
            headerName: "",
            width: 60,
            field: "set",
            aggFunc: "",
            cellStyle: { fontWeight: "normal" },
            editable: false,
            cellRenderer: lockIcon,
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
                enableCellChangeFlash
                rowSelection="multiple"
                enableRangeSelection
                suppressCopySingleCellRanges
                suppressMovableColumns
                enableCharts
                onGridReady={onGridReady}
            />
        </div>
    )
}

export default CaseCO2DistributionTable
