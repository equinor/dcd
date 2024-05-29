import {
    Dispatch,
    SetStateAction,
    useMemo,
    useState,
    useEffect,
    useCallback,
} from "react"
import { AgGridReact } from "@ag-grid-community/react"
import useStyles from "@equinor/fusion-react-ag-grid-styles"
import { ColDef, GridReadyEvent } from "@ag-grid-community/core"
import {
    isInteger,
    tableCellisEditable,
    numberValueParser,
    getCaseRowStyle,
    validateInput,
} from "../../../Utils/common"
import { OverrideTimeSeriesPrompt } from "../../Modal/OverrideTimeSeriesPrompt"
import { EMPTY_GUID } from "../../../Utils/constants"
import { useAppContext } from "../../../Context/AppContext"
import ErrorCellRenderer from "./ErrorCellRenderer"
import ClickableLockIcon from "./ClickableLockIcon"
import profileAndUnitInSameCell from "./ProfileAndUnitInSameCell"
import hideProfilesWithoutValues from "./HideProfilesWithoutValues"

interface Props {
    timeSeriesData: any[]
    dg4Year: number
    tableYears: [number, number]
    tableName: string
    alignedGridsRef?: any[]
    gridRef?: any
    includeFooter: boolean
    totalRowName?: string
    profilesToHideWithoutValues?: string[]
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
    profilesToHideWithoutValues,
}: Props) => {
    const { editMode } = useAppContext()
    const styles = useStyles()

    const [overrideModalOpen, setOverrideModalOpen] = useState<boolean>(false)
    const [overrideModalProfileName, setOverrideModalProfileName] = useState<string>("")
    const [overrideModalProfileSet, setOverrideModalProfileSet] = useState<Dispatch<SetStateAction<any | undefined>>>()
    const [overrideProfile, setOverrideProfile] = useState<any>()

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

        if (profilesToHideWithoutValues !== undefined) {
            return hideProfilesWithoutValues(
                editMode,
                profilesToHideWithoutValues,
                tableRows,
            )
        }

        return tableRows
    }

    const gridRowData = gridRef.current?.api?.setGridOption("rowData", profilesToRowData())

    const lockIconRenderer = (params: any) => (
        <ClickableLockIcon
            clickedElement={params}
            setOverrideModalOpen={setOverrideModalOpen}
            setOverrideModalProfileName={setOverrideModalProfileName}
            setOverrideModalProfileSet={setOverrideModalProfileSet}
            setOverrideProfile={setOverrideProfile}
        />
    )

    const generateTableYearColDefs = () => {
        const columnPinned: any[] = [
            {
                field: "profileName",
                headerName: tableName,
                cellRenderer: (params: any) => (
                    profileAndUnitInSameCell(params, profilesToRowData())
                ),
                width: 250,
                editable: false,
                pinned: "left",
                aggFunc: () => totalRowName ?? "Total",
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
                cellRenderer: lockIconRenderer,
            },
        ]

        const yearDefs: any[] = []
        for (let index = tableYears[0]; index <= tableYears[1]; index += 1) {
            yearDefs.push({
                field: index.toString(),
                flex: 1,
                editable: (params: any) => tableCellisEditable(params, editMode),
                minWidth: 100,
                aggFunc: "sum",
                cellRenderer: ErrorCellRenderer,
                cellRendererParams: (params: any) => ({
                    value: params.value,
                    errorMsg: validateInput(params, editMode),
                }),
                cellStyle: {
                    padding: "0px",
                    textAlign: "right",
                },
                cellClass: (params: any) => (editMode && tableCellisEditable(params, editMode) ? "editableCell" : undefined),
                valueParser: numberValueParser,
            })
        }
        return columnPinned.concat([...yearDefs])
    }

    const [columnDefs, setColumnDefs] = useState<ColDef[]>(generateTableYearColDefs())

    const handleCellValueChange = (p: any) => {
        console.log(p.newValue)
        console.log(p.oldValue)
        const cellName = p.colDef
        const columnName = p.colDef.headerName
        console.log(cellName)
        console.log(columnName)
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
        suppressMenuButton: true,
    }), [])

    useEffect(() => {
        const newColDefs = generateTableYearColDefs()
        setColumnDefs(newColDefs)
    }, [timeSeriesData, tableYears])

    const onGridReady = useCallback((params: GridReadyEvent) => {
        const generateRowData = profilesToRowData()
        params.api.setGridOption("rowData", generateRowData)
    }, [])

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
                        rowData={gridRowData}
                        columnDefs={columnDefs}
                        defaultColDef={defaultColDef}
                        animateRows
                        domLayout="autoHeight"
                        enableCellChangeFlash={editMode}
                        rowSelection="multiple"
                        enableRangeSelection
                        suppressCopySingleCellRanges
                        suppressMovableColumns
                        enableCharts
                        alignedGrids={gridRefArrayToAlignedGrid()}
                        groupIncludeTotalFooter={includeFooter}
                        getRowStyle={getCaseRowStyle}
                        suppressLastEmptyLineOnPaste
                        stopEditingWhenCellsLoseFocus
                        onGridReady={onGridReady}
                    />
                </div>
            </div>
        </>
    )
}

export default CaseTabTable
