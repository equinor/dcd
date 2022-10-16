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
    timeSeries: (ITimeSeries | undefined)[]
    profileName: string[]
    profileEnum: number
    profileType: string
    readOnlyTimeSeries: (ITimeSeries | undefined)[]
    readOnlyName: string[]
}

const TimeSeriesWells = ({
    dG4Year,
    setTimeSeries,
    setHasChanges,
    timeSeries,
    firstYear,
    lastYear,
    profileName,
    profileEnum,
    profileType,
    readOnlyTimeSeries,
    readOnlyName,
}: Props) => {
    const [columns, setColumns] = useState<string[]>([""])
    const [gridData, setGridData] = useState<CellValue[][]>([[]])
    const [tableFirstYear, setTableFirstYear] = useState<number>(Number.MAX_SAFE_INTEGER)
    const [tableLastYear, setTableLastYear] = useState<number>(Number.MIN_SAFE_INTEGER)

    const combinedTimeseries: any = []
    const combinedEmptyTimeseries: any = []

    const isValidYear = (year: number | undefined) => year?.toString().length === 4

    const buildAlignedGrid = (updatedTimeSeries: ITimeSeries) => {
        if (timeSeries !== undefined) {
            if (timeSeries[0] !== undefined && updatedTimeSeries !== undefined) {
                for (let i = 0; i < timeSeries.length; i += 1) {
                    if (timeSeries[i] !== undefined) {
                        if (updatedTimeSeries !== undefined && timeSeries[i] !== undefined) {
                            const columnTitles: string[] = []
                            if (firstYear !== undefined && lastYear !== undefined) {
                                for (let j = tableFirstYear; j <= tableLastYear; j += 1) {
                                    columnTitles.push(j.toString())
                                }
                            }
                            setColumns(columnTitles)

                            const newGridData = buildGridData(timeSeries[i])

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
    }, [timeSeries, lastYear, firstYear])

    const createEmptyGrid = (j: any) => {
        if (gridData !== undefined && isValidYear(firstYear) && isValidYear(lastYear)) {
            setTableFirstYear(firstYear!)
            setTableLastYear(lastYear! - 1)
        }
        const newTimeSeries: ITimeSeries = { ...timeSeries[j] }
        const colYears = []
        for (let c = tableFirstYear; c <= (tableLastYear ?? Number.MIN_SAFE_INTEGER); c += 1) {
            colYears.push(c.toString())
        }
        setColumns(colYears)

        newTimeSeries.name = profileName[j]
        newTimeSeries.startYear = Number(colYears[0]) - dG4Year
        newTimeSeries.values = []

        setTimeSeries(newTimeSeries)
        if (newTimeSeries !== undefined) {
            const newGridData = buildGridData(timeSeries[j])

            const alignedAssetGridData = new Array(newGridData)
            combinedTimeseries.push(alignedAssetGridData)
        }
        setGridData(combinedEmptyTimeseries)
        setHasChanges(true)
    }

    const NewTableLastYearSmallerThanProfileLastYear = (j: any) => tableLastYear
        < (Number(dG4Year) + Number(timeSeries[j]?.startYear) + Number(timeSeries[j]?.values!.length))
    const NewTableFirstYearGreaterThanProfileFirstYear = (j: any) => tableFirstYear
        > (Number(timeSeries[j]?.startYear!) + Number(dG4Year))

    const createNewGridWithData = (j: number) => {
        if (tableFirstYear && tableLastYear && timeSeries !== undefined) {
            const newTimeSeries: ITimeSeries = { ...timeSeries[j] }
            const colYears = []
            for (let c = tableFirstYear; c <= tableLastYear; c += 1) {
                colYears.push(c.toString())
            }
            setColumns(colYears)
            newTimeSeries.name = profileName[j]
            newTimeSeries.startYear = timeSeries[j]?.startYear

            if (NewTableLastYearSmallerThanProfileLastYear(j)) {
                const yearDifference = (Number(dG4Year) + Number(timeSeries[j]?.startYear)
                + Number(timeSeries[j]?.values!.length) - 1) - tableLastYear
                newTimeSeries.values = timeSeries[j]?.values?.slice(0, -yearDifference) ?? []
            }

            if (NewTableFirstYearGreaterThanProfileFirstYear(j)) {
                newTimeSeries.startYear = tableFirstYear - dG4Year
                const yearDifference = tableFirstYear - (Number(timeSeries[j]?.startYear) + dG4Year)
                newTimeSeries.values = timeSeries[j]?.values?.slice(yearDifference) ?? []
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

            if (timeSeries[0] === undefined) {
                for (let i = 0; i < timeSeries?.length!; i += 1) {
                    createEmptyGrid(i)
                }
            }
            if (timeSeries[0] !== undefined) {
                for (let i = 0; i < timeSeries?.length!; i += 1) {
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
            <WrapperTablePeriod>
                <NumberInputTable
                    value={isValidYear(tableFirstYear) ? tableFirstYear : 2020}
                    setValue={setTableFirstYear}
                    integer
                    label="Start year"
                />
                <Typography variant="h2">-</Typography>
                <NumberInputTable
                    value={isValidYear(tableLastYear) ? tableLastYear : 2030}
                    setValue={setTableLastYear}
                    integer
                    label="End year"
                />
                <ImportButton
                    onClick={addTimeSeries}
                    disabled={disableApplyButton()}
                >
                    Apply
                </ImportButton>
            </WrapperTablePeriod>
            <Typography variant="h2">{profileType}</Typography>

            <WrapperColumn>
                <DataTableWell
                    columns={columns}
                    dG4Year={dG4Year.toString()}
                    profileEnum={profileEnum}
                    profileType={profileType}
                    readOnlyTimeSeries={readOnlyTimeSeries}
                    readOnlyName={readOnlyName}
                />
            </WrapperColumn>
        </>
    )
}

export default TimeSeriesWells
