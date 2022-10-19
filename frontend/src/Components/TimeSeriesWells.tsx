import { Typography } from "@equinor/eds-core-react"
import {
    Dispatch, SetStateAction, useEffect, useState,
} from "react"
import DataTable, { CellValue } from "./DataTable/DataTable"
import {
    buildGridData,
} from "./DataTable/helpers"
import { ITimeSeries } from "../models/ITimeSeries"
import {
    ImportButton, WrapperColumn, WrapperTablePeriod,
} from "../Views/Asset/StyledAssetComponents"
import NumberInputTable from "./NumberInputTable"
import DataTableWell from "./DataTable/DataTableWell"

interface Props {
    dG4Year: number
    setTimeSeries: Dispatch<SetStateAction<ITimeSeries | undefined>>,
    setHasChanges: Dispatch<SetStateAction<boolean>>,
    firstYear: number | undefined,
    lastYear: number | undefined,
    wellsTimeSeries: (ITimeSeries | undefined)[]
}

const TimeSeriesWells = ({
    dG4Year,
    setTimeSeries,
    setHasChanges,
    wellsTimeSeries,
    firstYear,
    lastYear,
}: Props) => {
    const [columns, setColumns] = useState<string[]>([""])
    const [gridData, setGridData] = useState<CellValue[][]>([[]])
    const [tableFirstYear, setTableFirstYear] = useState<number>(Number.MAX_SAFE_INTEGER)
    const [tableLastYear, setTableLastYear] = useState<number>(Number.MIN_SAFE_INTEGER)

    const combinedTimeseries: any = []
    const combinedEmptyTimeseries: any = []

    const isValidYear = (year: number | undefined) => year?.toString().length === 4

    const buildAlignedGrid = (updatedTimeSeries: ITimeSeries) => {
        if (wellsTimeSeries !== undefined) {
            if (wellsTimeSeries[0] !== undefined && updatedTimeSeries !== undefined) {
                for (let i = 0; i < wellsTimeSeries.length; i += 1) {
                    if (wellsTimeSeries[i] !== undefined) {
                        if (updatedTimeSeries !== undefined && wellsTimeSeries[i] !== undefined) {
                            const columnTitles: string[] = []
                            if (firstYear !== undefined && lastYear !== undefined) {
                                for (let j = tableFirstYear; j <= tableLastYear; j += 1) {
                                    columnTitles.push(j.toString())
                                }
                            }
                            setColumns(columnTitles)

                            const newGridData = buildGridData(wellsTimeSeries[i])

                            const alignedAssetGridData = new Array(newGridData[0])
                            combinedTimeseries.push(alignedAssetGridData)
                        }
                    }
                }
            }
            setGridData(combinedTimeseries)
        }
    }

    useEffect(() => {
        buildAlignedGrid(combinedTimeseries!)

        if (gridData !== undefined && isValidYear(firstYear) && isValidYear(lastYear) && firstYear && lastYear
            && tableFirstYear === Number.MAX_SAFE_INTEGER && tableLastYear === Number.MIN_SAFE_INTEGER
            && (firstYear !== lastYear)) {
            setTableFirstYear(firstYear)
            setTableLastYear(lastYear - 1)
        }
    }, [wellsTimeSeries, lastYear, firstYear])

    const createEmptyGrid = (j: any) => {
        if (gridData !== undefined && isValidYear(firstYear) && isValidYear(lastYear)) {
            setTableFirstYear(firstYear!)
            setTableLastYear(lastYear! - 1)
        }
        const newTimeSeries: ITimeSeries = { ...wellsTimeSeries[j] }
        const colYears = []
        for (let c = tableFirstYear; c <= (tableLastYear ?? Number.MIN_SAFE_INTEGER); c += 1) {
            colYears.push(c.toString())
        }
        setColumns(colYears)

        newTimeSeries.name = ""
        newTimeSeries.startYear = Number(colYears[0]) - dG4Year
        newTimeSeries.values = []

        setTimeSeries(newTimeSeries)
        if (newTimeSeries !== undefined) {
            const newGridData = buildGridData(wellsTimeSeries[j])

            const alignedAssetGridData = new Array(newGridData)
            combinedTimeseries.push(alignedAssetGridData)
        }
        setGridData(combinedEmptyTimeseries)
        setHasChanges(true)
    }

    const NewTableLastYearSmallerThanProfileLastYear = (j: any) => tableLastYear
        < (Number(dG4Year) + Number(wellsTimeSeries[j]?.startYear) + Number(wellsTimeSeries[j]?.values!.length))
    const NewTableFirstYearGreaterThanProfileFirstYear = (j: any) => tableFirstYear
        > (Number(wellsTimeSeries[j]?.startYear!) + Number(dG4Year))

    const createNewGridWithData = (j: number) => {
        if (tableFirstYear && tableLastYear && wellsTimeSeries !== undefined) {
            const newTimeSeries: ITimeSeries = { ...wellsTimeSeries[j] }
            const colYears = []
            for (let c = tableFirstYear; c <= tableLastYear; c += 1) {
                colYears.push(c.toString())
            }
            setColumns(colYears)
            newTimeSeries.name = ""
            newTimeSeries.startYear = wellsTimeSeries[j]?.startYear

            if (NewTableLastYearSmallerThanProfileLastYear(j)) {
                const yearDifference = (Number(dG4Year) + Number(wellsTimeSeries[j]?.startYear)
                + Number(wellsTimeSeries[j]?.values!.length) - 1) - tableLastYear
                newTimeSeries.values = wellsTimeSeries[j]?.values?.slice(0, -yearDifference) ?? []
            }

            if (NewTableFirstYearGreaterThanProfileFirstYear(j)) {
                newTimeSeries.startYear = tableFirstYear - dG4Year
                const yearDifference = tableFirstYear - (Number(wellsTimeSeries[j]?.startYear) + dG4Year)
                newTimeSeries.values = wellsTimeSeries[j]?.values?.slice(yearDifference) ?? []
            }
            setTimeSeries(newTimeSeries)
            if (newTimeSeries !== undefined) {
                const newGridData = buildGridData(newTimeSeries)
                const alignedAssetGridData = new Array(newGridData[0])
                combinedEmptyTimeseries.push(alignedAssetGridData)
            }
            setGridData(combinedEmptyTimeseries)
            setHasChanges(true)
        }
    }

    const addTimeSeries = () => {
        const colYears = []
        if (isValidYear(tableFirstYear) && isValidYear(tableLastYear)) {
            for (let j = tableFirstYear; j < tableLastYear; j += 1) {
                colYears.push(j.toString())
            }
            setColumns(colYears)

            if (wellsTimeSeries[0] === undefined) {
                for (let i = 0; i < wellsTimeSeries?.length!; i += 1) {
                    createEmptyGrid(i)
                }
            }
            if (wellsTimeSeries[0] !== undefined) {
                for (let i = 0; i < wellsTimeSeries?.length!; i += 1) {
                    createNewGridWithData(i)
                }
            }
        }
    }

    const disableApplyButton = () => {
        if (firstYear === tableFirstYear && lastYear === (tableLastYear + 1)) {
            return true
        }
        return false
    }

    return (
        <>
            <WrapperColumn>
                <DataTableWell
                    columns={columns}
                    dG4Year={dG4Year.toString()}
                    wellsTimeSeries={[]}
                />
            </WrapperColumn>
        </>
    )
}

export default TimeSeriesWells
