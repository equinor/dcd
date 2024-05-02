import {
    Dispatch,
    SetStateAction,
    useMemo,
    useState,
    useEffect,
} from "react"

import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { lock, lock_open } from "@equinor/eds-icons"
import { Icon } from "@equinor/eds-core-react"
import { ColDef } from "@ag-grid-community/core"
import { isInteger } from "../../../Utils/common"
import { OverrideTimeSeriesPrompt } from "../../OverrideTimeSeriesPrompt"
import { EMPTY_GUID } from "../../../Utils/constants"
import { useAppContext } from "../../../Context/AppContext"
import ErrorCellRenderer from "./ErrorCellRenderer"

interface Props {
    timeSeriesData: any[]
    dg4Year: number
    tableYears: [number, number]
    tableName: string
    alignedGridsRef?: any[]
    gridRef?: any
    includeFooter: boolean
    totalRowName?: string
}

const CaseTabTable = ({
    timeSeriesData,
    dg4Year,
    tableYears,
    tableName,
    alignedGridsRef,
    gridRef,
    includeFooter,
    totalRowName,
}: Props) => {
    const styles = useStyles()
    const [overrideModalOpen, setOverrideModalOpen] = useState<boolean>(false)
    const [overrideModalProfileName, setOverrideModalProfileName] = useState<string>("")
    const [overrideModalProfileSet, setOverrideModalProfileSet] = useState<Dispatch<SetStateAction<any | undefined>>>()
    const [overrideProfile, setOverrideProfile] = useState<any>()
    const [rowData, setRowData] = useState<any[]>([{ name: "as" }])
    const [gridApi, setGridApi] = useState(null)

    const { editMode } = useAppContext()

    const profilesToRowData = () => {
        const tableRows: any[] = []
        timeSeriesData.forEach((ts) => {
            const isOverridden = ts.overrideProfile?.override === true
            const rowObject: any = {}
            const { profileName, unit } = ts
            rowObject.profileName = profileName
            rowObject.unit = unit
            rowObject.total = ts.total ?? 0
            rowObject.set = isOverridden ? ts.overrideProfileSet : ts.set
            rowObject.profile = isOverridden ? ts.overrideProfile : ts.profile
            rowObject.override = ts.overrideProfile?.override === true

            rowObject.overrideProfileSet = ts.overrideProfileSet
            rowObject.overrideProfile = ts.overrideProfile ?? {
                id: EMPTY_GUID, startYear: 0, values: [], override: false,
            }

            if (rowObject.profile && rowObject.profile.values?.length > 0) {
                let j = 0
                if (tableName === "Production profiles" || tableName === "CO2 emissions") {
                    for (let i = rowObject.profile.startYear;
                        i < rowObject.profile.startYear + rowObject.profile.values.length;
                        i += 1) {
                        rowObject[(dg4Year + i).toString()] = rowObject.profile.values.map(
                            (v: number) => Math.round((v + Number.EPSILON) * 10000) / 10000,
                        )[j]
                        j += 1
                        rowObject.total = Math.round(rowObject.profile.values.map(
                            (v: number) => (v + Number.EPSILON),
                        ).reduce((x: number, y: number) => x + y) * 10000) / 10000
                        if (ts.total !== undefined) {
                            rowObject.total = Math.round(ts.total * 1000) / 1000
                        }
                    }
                } else {
                    for (let i = rowObject.profile.startYear;
                        i < rowObject.profile.startYear + rowObject.profile.values.length;
                        i += 1) {
                        rowObject[(dg4Year + i).toString()] = rowObject.profile.values.map(
                            (v: number) => Math.round((v + Number.EPSILON) * 10) / 10,
                        )[j]
                        j += 1
                        rowObject.total = Math.round(rowObject.profile.values.map(
                            (v: number) => (v + Number.EPSILON),
                        ).reduce((x: number, y: number) => x + y) * 10) / 10
                    }
                }
            }

            tableRows.push(rowObject)
        })
        return tableRows
    }

    const lockIcon = (params: any) => {
        const handleLockIconClick = () => {
            if (params?.data?.override !== undefined) {
                setOverrideModalOpen(true)
                setOverrideModalProfileName(params.data.profileName)
                setOverrideModalProfileSet(() => params.data.overrideProfileSet)
                setOverrideProfile(params.data.overrideProfile)

                params.api.redrawRows()
                params.api.refreshCells()
            }
        }
        if (params.data?.overrideProfileSet !== undefined) {
            return (params.data.overrideProfile?.override) ? (
                <Icon
                    data={lock_open}
                    opacity={0.5}
                    color="#007079"
                    onClick={handleLockIconClick}
                />
            )
                : (
                    <Icon
                        data={lock}
                        color="#007079"
                        onClick={handleLockIconClick}
                    />
                )
        }
        if (!params?.data?.set) {
            return <Icon data={lock} color="#007079" />
        }
        return null
    }

    const getRowStyle = (params: any) => {
        if (params.node.footer) {
            return { fontWeight: "bold" }
        }
        return undefined
    }

    const numberValueParser = (params: { newValue: any }) => {
        const { newValue } = params
        if (typeof newValue === "string") {
            const processedValue = newValue.replace(/\s/g, "").replace(/,/g, ".")
            const numberValue = Number(processedValue)
            if (!Number.isNaN(numberValue)) {
                return numberValue
            }
        }
        return newValue
    }

    const generateTableYearColDefs = () => {
        const columnPinned: any[] = [
            {
                field: "profileName",
                headerName: tableName,
                width: 250,
                editable: false,
                pinned: "left",
                aggFunc: () => totalRowName ?? "Total",
            },
            {
                field: "unit",
                width: 100,
                editable: false,
                pinned: "left",
                aggFunc: (params: any) => {
                    if (params?.values?.length > 0) {
                        return params.values[0]
                    }
                    return ""
                },
            },
            {
                field: "total",
                flex: 2,
                editable: false,
                pinned: "right",
                width: 100,
                aggFunc: "sum",
                cellStyle: { fontWeight: "bold" },
            },
            {
                headerName: "",
                width: 60,
                field: "set",
                pinned: "right",
                aggFunc: "",
                editable: false,
                cellRenderer: lockIcon,
            },
        ]
        const isEditable = (params: any) => {
            if (editMode && params.data?.overrideProfileSet === undefined && params.data?.set !== undefined) {
                return true
            }
            if (editMode && params.data?.overrideProfile !== undefined && params.data?.overrideProfile.override) {
                return true
            }
            return false
        }

        const validationRules: { [key: string]: { min: number, max: number } } = {
            // production profiles
            "Oil production": { min: 0, max: 1000000 },
            "Gas production": { min: 0, max: 1000000 },
            "Water production": { min: 0, max: 1000000 },
            "Water injection": { min: 0, max: 1000000 },
            "Fuel, flaring and losses": { min: 0, max: 1000000 },
            "Net sales gas": { min: 0, max: 1000000 },
            "Imported electricity": { min: 0, max: 1000000 },

            // CO2 emissions
            "Annual CO2 emissions": { min: 0, max: 1000000 },
            "Year-by-year CO2 intensity": { min: 0, max: 1000000 },
        }

        const validateInput = (params: any) => {
            const { value, data } = params
            if (isEditable(params) && editMode && value) {
                const rule = validationRules[data.profileName]
                if (rule && (value < rule.min || value > rule.max)) {
                    return `Value must be between ${rule.min} and ${rule.max}.`
                }
            }
            return null
        }

        const yearDefs: any[] = []
        for (let index = tableYears[0]; index <= tableYears[1]; index += 1) {
            yearDefs.push({
                field: index.toString(),
                flex: 1,
                editable: (params: any) => isEditable(params),
                minWidth: 100,
                aggFunc: "sum",
                cellRenderer: ErrorCellRenderer,
                cellRendererParams: (params: any) => ({
                    value: params.value,
                    errorMsg: validateInput(params),
                }),
                cellStyle: { padding: "0px" },
                cellClass: (params: any) => (editMode && isEditable(params) ? "editableCell" : undefined),
                valueParser: numberValueParser,
            })
        }
        return columnPinned.concat([...yearDefs])
    }

    const [columnDefs, setColumnDefs] = useState<ColDef[]>(generateTableYearColDefs())

    const onGridReady = (params: any) => {
        setGridApi(params.api)
    }

    const updateRowData = (newData: any) => {
        if (gridApi) {
            (gridApi as any).setRowData(newData)
        } else {
            setRowData(newData)
        }
    }

    const handleCellValueChange = (p: any) => {
        const properties = Object.keys(p.data)
        const tableTimeSeriesValues: any[] = []
        properties.forEach((prop) => {
            if (isInteger(prop)
                && p.data[prop] !== ""
                && p.data[prop] !== null
                && !Number.isNaN(Number(p.data[prop].toString().replace(/,/g, ".")))) {
                tableTimeSeriesValues.push({
                    year: parseInt(prop, 10),
                    value: Number(p.data[prop].toString().replace(/,/g, ".")),
                })
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

    const defaultColDef = useMemo(() => ({
        sortable: true,
        filter: true,
        resizable: true,
        editable: true,
        onCellValueChanged: handleCellValueChange,
        suppressMenu: true,
    }), [])

    useEffect(() => {
        updateRowData(profilesToRowData())
        const newColDefs = generateTableYearColDefs()
        setColumnDefs(newColDefs)
    }, [timeSeriesData, tableYears])

    return (
        <>
            <OverrideTimeSeriesPrompt
                isOpen={overrideModalOpen}
                setIsOpen={setOverrideModalOpen}
                profileName={overrideModalProfileName}
                setProfile={overrideModalProfileSet}
                profile={overrideProfile}
            />
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
                        enableCellChangeFlash
                        rowSelection="multiple"
                        enableRangeSelection
                        suppressCopySingleCellRanges
                        suppressMovableColumns
                        enableCharts
                        alignedGrids={gridRefArrayToAlignedGrid()}
                        groupIncludeTotalFooter={includeFooter}
                        getRowStyle={getRowStyle}
                        suppressLastEmptyLineOnPaste
                        singleClickEdit={editMode}
                        onGridReady={onGridReady}
                    />
                </div>
            </div>
        </>
    )
}

export default CaseTabTable
