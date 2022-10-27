import {
    Dispatch,
    SetStateAction,
    useMemo,
    useState,
    useRef,
    useEffect,
} from "react"

import { AgGridReact } from "ag-grid-react"
import { useAgGridStyles } from "@equinor/fusion-react-ag-grid-addons"
import { Project } from "../../models/Project"
import { Case } from "../../models/case/Case"
import "ag-grid-enterprise"
import { isInteger } from "../../Utils/common"

interface Props {
    project: Project,
    setProject: Dispatch<SetStateAction<Project | undefined>>,
    caseItem: Case,
    setCase: Dispatch<SetStateAction<Case | undefined>>,
    timeSeriesData: any[]
    dg4Year: number
    tableYears: [number, number]
    tableName: string
    alignedGridsRef?: any[]
    gridRef?: any
}

function CaseTabTable({
    project, setProject,
    caseItem, setCase,
    timeSeriesData, dg4Year,
    tableYears, tableName,
    alignedGridsRef, gridRef,
}: Props) {
    useAgGridStyles()
    const [rowData, setRowData] = useState<any[]>([{ name: "as" }])

    const profilesToRowData = () => {
        const tableRows: any[] = []
        timeSeriesData.forEach((ts) => {
            const rowObject: any = {}
            const { profileName, unit } = ts
            rowObject.profileName = profileName
            rowObject.unit = unit
            rowObject.total = 0
            rowObject.set = ts.set
            rowObject.profile = ts.profile
            if (ts.profile && ts.profile.values.length > 0) {
                let j = 0
                for (let i = ts.profile.startYear; i < ts.profile.startYear + ts.profile.values.length; i += 1) {
                    rowObject[(dg4Year + i).toString()] = ts.profile.values.map(
                        (v:number) => Math.round((v + Number.EPSILON) * 10) / 10,
                    )[j]
                    j += 1
                    rowObject.total = ts.profile.values.reduce((x: number, y: number) => x + y)
                }
            }

            tableRows.push(rowObject)
        })
        return tableRows
    }

    const generateTableYearColDefs = () => {
        const profileNameDef = {
            field: "profileName", headerName: tableName, width: 250, editable: false,
        }
        const unitDef = { field: "unit", width: 100, editable: false }
        const yearDefs = []
        for (let index = tableYears[0]; index <= tableYears[1]; index += 1) {
            yearDefs.push({
                field: index.toString(),
                flex: 1,
                editable: (params: any) => params.data.set !== undefined,
                minWidth: 100,
            })
        }
        const totalDef = { field: "total", flex: 2, editable: false }
        return [profileNameDef, unitDef, ...yearDefs, totalDef]
    }

    const [columnDefs, setColumnDefs] = useState(generateTableYearColDefs())

    useEffect(() => {
        setRowData(profilesToRowData())
        const newColDefs = generateTableYearColDefs()
        setColumnDefs(newColDefs)
    }, [timeSeriesData, tableYears])

    const handleCellValueChange = (p: any) => {
        const properties = Object.keys(p.data)
        const tableTimeSeriesValues: any[] = []
        properties.forEach((prop) => {
            if (isInteger(prop)
                && p.data[prop] !== ""
                && p.data[prop] !== null
                && !Number.isNaN(Number(p.data[prop]))) {
                tableTimeSeriesValues.push({ year: parseInt(prop, 10), value: Number(p.data[prop]) })
            }
        })
        tableTimeSeriesValues.sort((a, b) => a.year - b.year)
        if (tableTimeSeriesValues.length > 0) {
            const tableTimeSeriesFirstYear = tableTimeSeriesValues[0].year
            const tableTimeSerieslastYear = tableTimeSeriesValues.at(-1).year
            const timeSeriesStartYear = tableTimeSeriesFirstYear - dg4Year
            const values: number[] = []
            for (let i = tableTimeSeriesFirstYear; i <= tableTimeSerieslastYear; i += 1) {
                const tableTimeSeriesValue = tableTimeSeriesValues.find((v) => v.year === i)
                if (tableTimeSeriesValue) {
                    values.push(tableTimeSeriesValue.value)
                } else {
                    values.push(0)
                }
            }
            const newProfile = { ...p.data.profile }
            newProfile.startYear = timeSeriesStartYear
            newProfile.values = values
            p.data.set(newProfile)
        }
    }

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        editable: true,
        onCellValueChanged: handleCellValueChange,
    }), [])

    const gridRefArrayToAlignedGrid = () => {
        if (alignedGridsRef && alignedGridsRef.length > 0) {
            const refArray: any[] = []
            alignedGridsRef.forEach((agr: any) => {
                if (agr && agr.current) {
                    refArray.push(agr.current)
                }
            })
            if (refArray.length > 0) {
                return refArray
            }
        }
        return undefined
    }

    return (
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
                enableCellChangeFlash
                rowSelection="multiple"
                enableRangeSelection
                suppressCopySingleCellRanges
                suppressMovableColumns
                enableCharts
                alignedGrids={gridRefArrayToAlignedGrid()}
            />
        </div>
    )
}

export default CaseTabTable
