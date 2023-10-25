import { AgGridReact } from "@ag-grid-community/react"
import {
    useMemo, useRef, useState,
} from "react"
import { lock } from "@equinor/eds-icons"
import { Icon } from "@equinor/eds-core-react"
import { ColDef } from "@ag-grid-community/core"
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

    // Show data as percentages
    const co2Data = [
        {
            profile: "Oil profile",
            expectedProfile: `${Math.round(Number(topside?.cO2ShareOilProfile) * 100 * 1) / 1}%`,
            maxProfile: `${Math.round(Number(topside?.cO2OnMaxOilProfile) * 100 * 1) / 1}%`,
        },
        {
            profile: "Gas profile",
            expectedProfile: `${Math.round(Number(topside?.cO2ShareGasProfile) * 100 * 1) / 1}%`,
            maxProfile: `${Math.round(Number(topside?.cO2OnMaxGasProfile) * 100 * 1) / 1}%`,
        },
        {
            profile: "Water injection profile",
            expectedProfile: `${Math.round(Number(topside?.cO2ShareWaterInjectionProfile) * 100 * 1) / 1}%`,
            maxProfile: `${Math.round(Number(topside?.cO2OnMaxWaterInjectionProfile) * 100 * 1) / 1}%`,
        },
    ]

    const [rowData] = useState(co2Data)

    const [columnDefs] = useState<ColDef[]>([
        {
            field: "profile",
            headerName: "CO2 distribution",
            width: 110,
            flex: 1,
        },
        {
            field: "expectedProfile",
            headerName: "Expected profile",
            width: 200,
        },
        {
            field: "maxProfile",
            headerName: "Max profile",
            width: 200,
        },
        {
            headerName: "",
            width: 60,
            field: "set",
            aggFunc: "",
            cellStyle: { fontWeight: "normal" },
            cellRenderer: lockIcon,
        },
    ])

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        editable: false,
    }), [])

    return (
        <div
            style={{
                display: "flex", flexDirection: "column", width: "50%",
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
