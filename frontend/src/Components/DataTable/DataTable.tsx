/* eslint-disable camelcase */
import {
    Dispatch,
    SetStateAction,
    useCallback, useEffect, useMemo, useRef,
} from "react"
import "react-datasheet/lib/react-datasheet.css"
import "./style.css"
import { AgGridReact } from "ag-grid-react"
import { useAgGridStyles } from "@equinor/fusion-react-ag-grid-addons"
import { CellValueChangedEvent, ColDef } from "ag-grid-community"
import { Icon } from "@equinor/eds-core-react"
import { lock, lock_open } from "@equinor/eds-icons"
import { ITimeSeries } from "../../models/ITimeSeries"
import { buildGridData } from "./helpers"
import "ag-grid-enterprise"

export interface CellValue {
    value: number | string
    readOnly?: boolean
}
interface Props {
    columns: string[]
    gridData: any[]
    dG4Year: string
    profileName: string[]
    profileEnum: number
    setHasChanges: Dispatch<SetStateAction<boolean>>,
    setTimeSeries: Dispatch<SetStateAction<ITimeSeries | undefined>>,
    timeSeries: (ITimeSeries | undefined)[]
    profileType: string
    readOnlyTimeSeries: (ITimeSeries | undefined)[]
    readOnlyName: string[]
    yearsBeforeStartYear: boolean
    yearsAfterEndYear: boolean
    tableFirstYear: number
    tableLastYear: number
}

function DataTable({
    columns,
    gridData,
    dG4Year,
    profileName,
    profileEnum,
    setHasChanges,
    setTimeSeries,
    timeSeries,
    profileType,
    readOnlyTimeSeries,
    readOnlyName,
    yearsBeforeStartYear,
    yearsAfterEndYear,
    tableFirstYear,
    tableLastYear,
}: Props) {
    const topGrid = useRef<AgGridReact>(null)
    const bottomGrid = useRef<AgGridReact>(null)

    const combinedTimeseries: any = []

    useAgGridStyles()

    enum CurrencyEnum {
        "MNOK" = 1,
        "MUSD" = 2
    }

    enum GSM3Enum {
        "GSm³/yr" = 0,
        "Bscf/yr" = 1
    }

    enum MSM3Enum {
        "MSm³/yr" = 0,
        "mill bbls/yr" = 1
    }

    const setUnit = (j: number) => {
        if (["CO2 emissions", "Production profile NGL"].includes(profileName[j])) {
            return "MTPA"
        }
        if (["Net sales gas", "Fuel flaring and losses", "Production profile gas"].includes(profileName[j])) {
            return GSM3Enum[profileEnum]
        }
        if (["Production profile oil", "Production profile water",
            "Production profile water injection"].includes(profileName[j])) {
            return MSM3Enum[profileEnum]
        }
        return CurrencyEnum[profileEnum]
    }

    const lockIcon = (params: any) => {
        if (params.data.ReadOnly === true) {
            return <Icon data={lock} color="#007079" />
        }
        if (params.data.Profile === "Total cost") {
            return ""
        }
        return <Icon data={lock_open} color="#A8CED1" />
    }

    const generateReadOnlyTimeSeriesYears = (index: number, dg4: string) => {
        const years = []
        if (dg4) {
            const profileStartYear: number = Number(readOnlyTimeSeries[index]?.startYear) + Number(dG4Year)
            console.log(`Profile start year${profileStartYear}`)
            const maxYear: number = (Number(readOnlyTimeSeries[index]?.values?.length) + profileStartYear) ?? profileStartYear
            console.log(`Max year${maxYear}`)

            for (let i = profileStartYear; i < maxYear; i += 1) {
                years.push(i.toString())
            }
        }
        return years
    }

    const generateTimeSeriesYears = (index: number, dg4: string) => {
        const years = []
        // if (yearsAfterEndYear) {
        //     console.log(tableLastYear)
        // }
        if (dg4) {
            const profileStartYear: number = Number(timeSeries[index]?.startYear) + Number(dG4Year)
            const maxYear: number = Number(timeSeries[index]?.values?.length) + profileStartYear

            const yearsAfterEndYear2 = maxYear < (tableLastYear + 1)
            const yearsBeforeStartYear2 = profileStartYear > tableFirstYear
            console.log(profileStartYear)
            console.log(tableFirstYear)
            console.log(maxYear)
            console.log(tableLastYear)
            // if (yearsAfterEndYear2 && !yearsBeforeStartYear2) {
            //     for (let i = profileStartYear; i < (tableLastYear + 1); i += 1) {
            //         years.push(i.toString())
            //     }
            // }
            // if (yearsBeforeStartYear2 && !yearsAfterEndYear2) {
            //     for (let i = tableFirstYear; i < maxYear; i += 1) {
            //         years.push(i.toString())
            //     }
            // }
            // if (!yearsBeforeStartYear2 && !yearsAfterEndYear2) {
            //     for (let i = profileStartYear; i < maxYear; i += 1) {
            //         years.push(i.toString())
            //     }
            // }
            for (let i = profileStartYear; i < maxYear; i += 1) {
                years.push(i.toString())
            }
        }
        console.log(years)
        return years
    }

    const setEmptyTableWithoutReadOnly = (
        objKey: string[],
        combinedObjArr: object[],
        objVal: string[],
        value: object[],
    ) => {
        if (timeSeries[0] === undefined && readOnlyTimeSeries.length === 0) {
            for (let j = 0; j < profileName.length; j += 1) {
                const rowPinned = { Profile: profileName[j], Unit: setUnit(j), ReadOnly: false }
                const rowObj = objKey
                    .reduce((obj: object, element: string, index: number) => ({ ...obj, [element]: objVal[index] }), {})
                combinedObjArr.push(rowObj)

                const totalValueObj = { Total: 0 }
                value.push({ ...combinedObjArr[j], ...totalValueObj, ...rowPinned })
            }
        }
    }

    const gridDataIsReadOnlyData = () => {
        if (readOnlyTimeSeries[0] !== undefined && gridData[0] !== undefined) {
            const readOnlyData = gridData[0][0]?.map((v: any) => v.value)
            const compareArrays = readOnlyData.filter((e: number) => readOnlyTimeSeries[0]?.values?.includes(e))
            console.log(compareArrays)
            if (compareArrays.length === readOnlyData.length) {
                return true
            }
        }
        return false
    }

    const setEmptyTableWithReadOnly = (
        objKey: string[],
        combinedObjArr: object[],
        objVal: string[],
        value: object[],
    ) => {
        if (gridDataIsReadOnlyData() === false && readOnlyTimeSeries.length !== 0 && timeSeries[0] === undefined) {
            for (let j = 0; j < profileName.length; j += 1) {
                const rowPinned = { Profile: profileName[j], Unit: setUnit(j), ReadOnly: false }
                const rowObj = objKey
                    .reduce((obj: object, element: string, index: number) => ({ ...obj, [element]: objVal[index] }), {})
                combinedObjArr.push(rowObj)

                const totalValueObj = { Total: 0 }
                value.push({ ...combinedObjArr[j], ...totalValueObj, ...rowPinned })
            }
        }
    }

    // const setNonReadOnlyDataToTable = (
    //     objKey: string[],
    //     combinedObjArr: object[],
    //     objVal: string[],
    //     value: object[],
    //     objValSum: number[],
    // ) => {
    //     if (gridData.length >= 1 && columns.length !== 0) {
    //         for (let j = 0; j < gridData.length; j += 1) {
    //             const rowPinned = { Profile: profileName[j], Unit: setUnit(j), ReadOnly: false }
    //             const totalValue: number[] = []
    //             if (gridData[j] !== undefined) {
    //                 for (let i = 0; i < columns.length; i += 1) {
    //                     if (gridData[j][0]) {
    //                         objKey.push(`${columns[i]}`)
    //                         objVal.push(`${gridData[j][0].map((v: any) => v.value)[i]}`)
    //                     }
    //                 }
    //                 objValSum.push(gridData[j][0]?.map((v: any) => v.value).reduce((x: number, y: number) => x + y))
    //                 totalValue.push(objValSum[j])
    //             }
    //             const objValToNumbers = objVal.map((x: string) => parseFloat(x))
    //             const rowObj = objKey
    //                 .reduce((obj: object, element: string, index: number) => (
    //                     { ...obj, [element]: objValToNumbers[index] }), {})
    //             combinedObjArr.push(rowObj)

    //             const totalValueObj = { Total: Number(totalValue) }
    //             value.push({ ...combinedObjArr[j], ...totalValueObj, ...rowPinned })
    //         }
    //     }
    // }

    const setReadOnlyDataToTable = (
        readOnlyCombinedObjArr: object[],
        value: object[],
        readOnlyObjValSum: number[],
    ) => {
        if (readOnlyName.length >= 1 && readOnlyTimeSeries !== undefined && dG4Year) {
            for (let i = 0; i < readOnlyName.length; i += 1) {
                const totalValue: number[] = []
                const readOnly = {
                    Profile: readOnlyName[i], Unit: setUnit(i), Total: totalValue, ReadOnly: true,
                }
                if (readOnlyTimeSeries[i] !== undefined && dG4Year && readOnlyTimeSeries[i]?.values?.length !== 0) {
                    readOnlyObjValSum.push((readOnlyTimeSeries[i]?.values?.map(
                        (v) => Math.round((v + Number.EPSILON) * 10) / 10,
                    ) ?? [])
                        .reduce((x: number, y: number) => x + y))
                    totalValue.push(readOnlyObjValSum[i])
                }

                const objValToNumbers: number[] = readOnlyTimeSeries[i]?.values?.map(
                    (v) => Math.round((v + Number.EPSILON) * 10) / 10,
                ) ?? []
                const rowObj = generateReadOnlyTimeSeriesYears(i, dG4Year)
                    .reduce((obj: object, element: string, index: number) => (
                        { ...obj, [element]: objValToNumbers[index] }), {})
                readOnlyCombinedObjArr.push(rowObj)
                value.push({ ...readOnlyCombinedObjArr[i], ...readOnly })
            }
        }
    }

    const setProfilesWithData = (value: object[]) => {
        const combinedObjArr: object[] = []
        const objValSum: number[] = []

        if (timeSeries[0] !== undefined) {
            for (let i = 0; i < profileName.length; i += 1) {
                const totalValue: number[] = []
                const readOnly = { Profile: profileName[i], Unit: setUnit(i) }
                console.log(timeSeries)
                if (timeSeries[i] !== undefined && dG4Year && timeSeries[i]?.values?.length !== 0) {
                    objValSum.push((timeSeries[i]?.values?.map(
                        (v) => Math.round((v + Number.EPSILON) * 10) / 10,
                    ) ?? [])
                        .reduce((x: number, y: number) => x + y))
                    totalValue.push(objValSum[i])
                }
                if (timeSeries[i] !== undefined && dG4Year && timeSeries[i]?.values?.length === 0) {
                    objValSum.push(0)
                    totalValue.push(objValSum[i])
                }

                // if (yearsAfterEndYear) {
                //     console.log(tableLastYear)
                // }

                const objValToNumbers: number[] = timeSeries[i]?.values!
                const rowObj = generateTimeSeriesYears(i, dG4Year)
                    .reduce((obj: object, element: string, index: number) => (
                        { ...obj, [element]: objValToNumbers[index] }), {})
                console.log(rowObj)
                combinedObjArr.push(rowObj)
                const totalValueObj = { Total: Number(totalValue) }
                value.push({ ...combinedObjArr[i], ...readOnly, ...totalValueObj })
            }
        }
    }

    const rowDataToColumns = () => {
        const objKey: string[] = []
        const objVal: string[] = []
        const objValSum: number[] = []
        const combinedObjArr: object[] = []
        const readOnlyCombinedObjArr: object[] = []
        const readOnlyObjValSum: number[] = []

        const value: object[] = []

        console.log(timeSeries)
        console.log(readOnlyTimeSeries)

        setReadOnlyDataToTable(readOnlyCombinedObjArr, value, readOnlyObjValSum)
        setEmptyTableWithReadOnly(objKey, combinedObjArr, objVal, value)
        setEmptyTableWithoutReadOnly(objKey, combinedObjArr, objVal, value)
        setProfilesWithData(value)
        // setNonReadOnlyDataToTable(objKey, combinedObjArr, objVal, value, objValSum)
        return value
    }

    const addColumnsBasedOnReadOnlyData = () => {
        const newColumns: string[] = []
        if (columns[0] === "" && readOnlyTimeSeries[0] !== undefined) {
            const columnStartYear = Number(dG4Year) + Number(readOnlyTimeSeries[0].startYear)
            const columnLength = Number(readOnlyTimeSeries[0]?.values?.length!)
            for (let i = columnStartYear; i < (columnStartYear + columnLength); i += 1) {
                newColumns.push(i.toString())
            }
        }
        return newColumns
    }

    const columnsArrayToColDef = () => {
        if (columns.length !== 0) {
            const col = columns
            console.log(col)
            const columnToColDef = []
            const columnPinned = [
                {
                    field: "Profile",
                    pinned: "left",
                    width: "autoWidth",
                    aggFunc: "",
                    editable: false,
                },
                {
                    field: "Unit", pinned: "left", width: "120", aggFunc: "", editable: false,
                },
                {
                    field: "Total",
                    width: 100,
                    pinned: "right",
                    aggFunc: "sum",
                    cellStyle: { fontWeight: "bold" },
                    editable: false,
                },
                {
                    headerName: "",
                    width: 60,
                    field: "ReadOnly",
                    pinned: "right",
                    aggFunc: "",
                    cellStyle: { fontWeight: "normal" },
                    editable: false,
                    hide: readOnlyTimeSeries.length === 0,
                    cellRenderer: lockIcon,
                }]
            // const yearColumns: string[] = []
            // if (tableFirstYear && tableLastYear) {
            //     for (let i = tableFirstYear; i < (tableLastYear + 1); i += 1) {
            //         yearColumns.push(i.toString())
            //     }
            //     for (let i = 0; i < yearColumns.length; i += 1) {
            //         columnToColDef.push({ field: yearColumns[i], aggFunc: "sum" })
            //     }
            // }
            addColumnsBasedOnReadOnlyData()
            if (col[0] !== "") {
                for (let i = 0; i < col.length; i += 1) {
                    columnToColDef.push({ field: col[i], aggFunc: "sum" })
                }
            }
            if (col[0] === "" || col.length === 0) {
                for (let i = 0; i < addColumnsBasedOnReadOnlyData().length; i += 1) {
                    columnToColDef.push({ field: addColumnsBasedOnReadOnlyData()[i], aggFunc: "sum" })
                }
            }
            console.log(col)
            console.log(addColumnsBasedOnReadOnlyData())
            // for (let i = 0; i < col.length; i += 1) {
            //     columnToColDef.push({ field: col[i], aggFunc: "sum" })
            // }
            const columnWithProfile = columnToColDef.concat([...columnPinned])
            console.log(columnWithProfile)
            return columnWithProfile
        }
        return undefined
    }

    const defaultColDef = useMemo<ColDef>(() => ({
        resizable: true,
        sortable: true,
        editable: (params) => (params.data.ReadOnly !== true) === true,
        flex: 1,
    }), [])

    const footerColDef = useMemo<ColDef>(() => ({
        resizable: true,
        sortable: true,
        editable: false,
        flex: 1,
    }), [])

    useEffect(() => {
    }, [timeSeries, profileName, gridData, dG4Year, yearsAfterEndYear, yearsBeforeStartYear])

    const onCellValueChanged = useCallback((event: CellValueChangedEvent) => {
        const rowEventData = event.data
        const index = event.node.rowIndex ?? 0
        const convertObj = {
            convertObj:
                (delete rowEventData.Unit, delete rowEventData.Profile,
                delete rowEventData.Total, delete rowEventData.ReadOnly),
            rowEventData,
        }

        const newValueYear = Number(event.column.getColId())

        const yearBeforeNewValueYear = Number(Object.keys(rowEventData)[Object.keys(rowEventData).length - 2])
        const valueYearDifference = newValueYear - yearBeforeNewValueYear

        const yearAfterNewValueYear = Number(Object.keys(rowEventData)[1])
        const afterValueYearDifference = yearAfterNewValueYear - newValueYear

        const keyYears = Object.keys(rowEventData)
        const eventValues: number[] = Object.values(rowEventData)
        const beforeValues: number[] = eventValues.slice(0, (Number(eventValues.length) - 1))
        const afterValues: number[] = eventValues.slice(1)

        const { newValue } = event
        const combinedValueArray: any[] = []
        const combinedYearArray: any[] = []

        const valueBeforeLowestCurrentYear: boolean = newValueYear < yearAfterNewValueYear - 1
        const valueAfterLastCurrentYear: boolean = newValueYear > yearBeforeNewValueYear + 1

        if (valueAfterLastCurrentYear && !valueBeforeLowestCurrentYear) {
            console.log(valueYearDifference)
            const beforeYears = keyYears.slice(0, (Number(keyYears.length) - 1))
            console.log(beforeYears)
            if (yearBeforeNewValueYear < newValueYear) {
                const addBetweenYears: any[] = []
                for (let c = (yearBeforeNewValueYear + 1); c < newValueYear; c += 1) {
                    addBetweenYears.push(c.toString())
                }
                combinedYearArray.push(beforeYears.concat(addBetweenYears, keyYears.slice(-1)))
                console.log(combinedYearArray)
            }
            console.log(Number(beforeYears[beforeYears.length - 1]))
            const zeroesBetween: number[] = Array.from({
                length: Number(newValueYear) - Number(beforeYears[beforeYears.length - 1]) - 1,
            }, (() => 0))
            console.log(zeroesBetween)
            combinedValueArray.push(beforeValues.concat(zeroesBetween, Number(newValue)))
            console.log(combinedValueArray)
            const changeKeysToValue = combinedYearArray
                .reduce((prev: object, curr: number, ind: number) => (
                    { ...prev, [(ind)]: combinedValueArray[curr] }), {})
            if (readOnlyTimeSeries.length !== 0) {
                const newTimeSeries: ITimeSeries = { ...timeSeries[Number(index - readOnlyTimeSeries.length)] }
                newTimeSeries.startYear = (Number(Object.keys(rowEventData)[0]) - Number(dG4Year))
                newTimeSeries.name = profileName[Number(index - readOnlyTimeSeries.length)]
                // eslint-disable-next-line prefer-destructuring
                newTimeSeries.values = combinedValueArray[0]
                setTimeSeries(newTimeSeries)
                const newGridData = buildGridData(newTimeSeries)
                combinedTimeseries.push(newGridData)
                setHasChanges(true)
            }
            if (readOnlyTimeSeries.length === 0) {
                const newTimeSeries: ITimeSeries = { ...timeSeries[index] }
                newTimeSeries.startYear = (Number(Object.keys(rowEventData)[0]) - Number(dG4Year))
                newTimeSeries.name = profileName[index]
                // eslint-disable-next-line prefer-destructuring
                newTimeSeries.values = combinedValueArray[0]
                setTimeSeries(newTimeSeries)
                const newGridData = buildGridData(newTimeSeries)
                combinedTimeseries.push(newGridData)
                setHasChanges(true)
            }
        }

        if (valueBeforeLowestCurrentYear && !valueAfterLastCurrentYear) {
            const afterYears = keyYears.slice(1)
            const firstYear: string[] = [newValueYear.toString()]
            const firstValue: number[] = [Number(newValue)]
            if (yearAfterNewValueYear > newValueYear) {
                const addBetweenYears: any[] = []
                for (let c = (newValueYear + 1); c < yearAfterNewValueYear; c += 1) {
                    addBetweenYears.push(c.toString())
                }
                console.log(addBetweenYears)
                combinedYearArray.push(firstYear.concat(addBetweenYears, afterYears))
                console.log(combinedYearArray)
            }
            const zeroesBetween: number[] = Array.from({
                length: Number(afterYears[0]) - Number(newValueYear) - 1,
            }, (() => 0))
            combinedValueArray.push(firstValue.concat(zeroesBetween, afterValues))

            if (readOnlyTimeSeries.length !== 0) {
                const newTimeSeries: ITimeSeries = { ...timeSeries[Number(index - readOnlyTimeSeries.length)] }
                newTimeSeries.startYear = (Number(Object.keys(rowEventData)[0]) - Number(dG4Year))
                newTimeSeries.name = profileName[Number(index - readOnlyTimeSeries.length)]
                // eslint-disable-next-line prefer-destructuring
                newTimeSeries.values = combinedValueArray[0]
                setTimeSeries(newTimeSeries)
                const newGridData = buildGridData(newTimeSeries)
                combinedTimeseries.push(newGridData)
                setHasChanges(true)
            }
            if (readOnlyTimeSeries.length === 0) {
                const newTimeSeries: ITimeSeries = { ...timeSeries[index] }
                newTimeSeries.startYear = (Number(Object.keys(rowEventData)[0]) - Number(dG4Year))
                newTimeSeries.name = profileName[index]
                // eslint-disable-next-line prefer-destructuring
                newTimeSeries.values = combinedValueArray[0]
                setTimeSeries(newTimeSeries)
                const newGridData = buildGridData(newTimeSeries)
                combinedTimeseries.push(newGridData)
                setHasChanges(true)
            }
        }

        console.log(afterValueYearDifference)
        console.log(valueYearDifference)
        if (!valueBeforeLowestCurrentYear && !valueAfterLastCurrentYear) {
            const changeKeysToValue = Object.keys(rowEventData)
                .reduce((prev: object, curr: string, ind: number) => (
                    { ...prev, [(ind)]: Number(rowEventData[curr]) }), {})

            console.log(changeKeysToValue)
            // console.log(rowEventData)
            // console.log(Number(event.column.getColId()))
            // console.log(Number(Object.keys(rowEventData)[Object.keys(rowEventData).length - 2]))
            // console.log(Number(Object.keys(rowEventData)[Object.keys(rowEventData).length]))
            // console.log(Number(event.column.getColId()) - Number(Object.keys(rowEventData)[Object.keys(rowEventData).length - 2]))

            if (readOnlyTimeSeries.length !== 0) {
                const newTimeSeries: ITimeSeries = { ...timeSeries[Number(index - readOnlyTimeSeries.length)] }
                newTimeSeries.startYear = (Number(Object.keys(rowEventData)[0]) - Number(dG4Year))
                newTimeSeries.name = profileName[Number(index - readOnlyTimeSeries.length)]
                newTimeSeries.values = Object.values(changeKeysToValue)
                setTimeSeries(newTimeSeries)
                const newGridData = buildGridData(newTimeSeries)
                combinedTimeseries.push(newGridData)
                setHasChanges(true)
            }
            if (readOnlyTimeSeries.length === 0) {
                const newTimeSeries: ITimeSeries = { ...timeSeries[index] }
                newTimeSeries.startYear = (Number(Object.keys(rowEventData)[0]) - Number(dG4Year))
                newTimeSeries.name = profileName[index]
                newTimeSeries.values = Object.values(changeKeysToValue)
                console.log(newTimeSeries.values)
                setTimeSeries(newTimeSeries)
                const newGridData = buildGridData(newTimeSeries)
                combinedTimeseries.push(newGridData)
                setHasChanges(true)
            }
        }
    }, [dG4Year])

    const columnTotalsData = () => {
        const footerGridData = {
            Profile: "Total cost",
            Unit: setUnit(0),
        }
        const totalValueArray: number[] = []
        const valueArray: number[][] = []
        const readOnlyValueArray: number[][] = []
        const readOnlyTotalValueArray: number[] = []
        if (readOnlyTimeSeries.length >= 1 && columns.length > 1) {
            for (let i = 0; i < columns.length; i += 1) {
                if (readOnlyTimeSeries[i] !== undefined) {
                    const zeroesAtStart: number[] = Array.from({
                        length: Number(readOnlyTimeSeries[i]?.startYear)
                            + Number(dG4Year) - Number(columns[0]),
                    }, (() => 0))

                    const zeroesAtEnd: number[] = Array.from({
                        length: Number(columns.slice(-1)[0]) + 1
                            - (Number(readOnlyTimeSeries[i]?.startYear)
                                + Number(dG4Year)
                                + Number(readOnlyTimeSeries[i]?.values?.length)),
                    }, (() => 0))

                    const alignedAssetGridData: number[] = zeroesAtStart
                        .concat(readOnlyTimeSeries[i]?.values!, zeroesAtEnd)
                    readOnlyValueArray.push(alignedAssetGridData.map((v) => Math.round((v + Number.EPSILON) * 10) / 10))
                }
            }
            for (let k = 0; k < columns.length; k += 1) {
                readOnlyTotalValueArray.push(readOnlyValueArray.reduce((prev, curr) => prev + curr[k], 0))
            }
        }
        if (timeSeries.length >= 1 && columns.length !== 0) {
            for (let i = 0; i < columns.length; i += 1) {
                if (timeSeries[i] !== undefined && timeSeries[i]?.values?.length === columns.length) {
                    valueArray.push(timeSeries[i]?.values ?? [])
                }
                if (timeSeries[i] !== undefined && timeSeries[i]?.values?.length !== columns.length) {
                    const zeroesAtStart: number[] = Array.from({
                        length: Number(timeSeries[i]?.startYear)
                            + Number(dG4Year) - Number(columns[0]),
                    }, (() => 0))

                    const zeroesAtEnd: number[] = Array.from({
                        length: Number(columns.slice(-1)[0]) + 1
                            - (Number(timeSeries[i]?.startYear)
                                + Number(dG4Year)
                                + Number(timeSeries[i]?.values?.length)),
                    }, (() => 0))

                    const alignedAssetGridData: number[] = zeroesAtStart.concat(timeSeries[i]?.values!, zeroesAtEnd)
                    valueArray.push(alignedAssetGridData)
                }
            }
            for (let k = 0; k < columns.length; k += 1) {
                totalValueArray.push(valueArray.reduce((prev, curr) => prev + curr[k], 0))
            }
        }
        const yearTotals = () => {
            if (readOnlyTimeSeries.length >= 1) {
                const mergedTimeSeries = totalValueArray.map((a: number, i: number) => a + readOnlyTotalValueArray[i])
                return mergedTimeSeries
            }
            return totalValueArray
        }
        const value = columns
            .reduce((obj: object, element: string, index: number) => (
                { ...obj, [element]: yearTotals()[index] }), {})
        const totalTotalCostArray = []
        if (readOnlyTimeSeries.length >= 1 && columns.length !== 0) {
            for (let j = 0; j < readOnlyTimeSeries.length; j += 1) {
                if (readOnlyTimeSeries[j] !== undefined && readOnlyTimeSeries[j]?.values?.length !== 0) {
                    totalTotalCostArray.push((readOnlyTimeSeries[j]?.values ?? [])
                        .reduce((x: number, y: number) => x + y))
                }
            }
        }
        if (timeSeries.length >= 1 && columns.length !== 0) {
            for (let j = 0; j < timeSeries.length; j += 1) {
                if (timeSeries[j] !== undefined && gridData[j] !== undefined && timeSeries[j]?.values?.length !== 0) {
                    totalTotalCostArray.push(gridData[j][0]?.map((v: any) => v.value).reduce((x: any, y: any) => x + y))
                }
            }
        }
        const sum: number = totalTotalCostArray.map((v) => Math.round(
            (v + Number.EPSILON) * 10,
        ) / 10).reduce((prev, curr) => prev + curr, 0)
        const totalTotalObj = { Total: Number(sum) }
        const combinedFooterRow = [{
            ...value, ...footerGridData, ...totalTotalObj,
        }]
        return combinedFooterRow
    }

    return (
        <div
            style={{
                display: "flex", flexDirection: "column", height: 150, marginBottom: 150,
            }}
            className="ag-theme-alpine"
        >
            <div style={{ flex: "1 1 auto" }}>
                <AgGridReact
                    ref={topGrid}
                    alignedGrids={bottomGrid.current ? [bottomGrid.current] : undefined}
                    rowData={rowDataToColumns()}
                    columnDefs={columnsArrayToColDef()}
                    defaultColDef={defaultColDef}
                    animateRows
                    domLayout="autoHeight"
                    enableCellChangeFlash
                    onCellValueChanged={onCellValueChanged}
                    rowSelection="multiple"
                    enableRangeSelection
                    suppressCopySingleCellRanges
                    suppressMovableColumns
                    enableCharts
                />
            </div>
            {profileType === "Cost"
                && (
                    <div style={{ flex: "none", height: "60px" }}>
                        <AgGridReact
                            ref={bottomGrid}
                            alignedGrids={topGrid.current ? [topGrid.current] : undefined}
                            rowData={columnTotalsData()}
                            defaultColDef={footerColDef}
                            columnDefs={columnsArrayToColDef()}
                            headerHeight={0}
                            rowStyle={{ fontWeight: "bold" }}
                        />
                    </div>
                )}
        </div>
    )
}

export default DataTable
