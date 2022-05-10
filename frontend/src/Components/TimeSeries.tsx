import { Typography } from "@equinor/eds-core-react"
import {
    Dispatch, SetStateAction, useEffect, useState,
} from "react"
import DataTable, { CellValue } from "./DataTable/DataTable"
import {
    buildGridData, buildZeroGridData, getColumnAbsoluteYears, replaceOldData,
} from "./DataTable/helpers"
import Import from "./Import/Import"
import { ITimeSeries } from "../models/ITimeSeries"
import { ImportButton, Wrapper, WrapperColumn } from "../Views/Asset/StyledAssetComponents"

interface Props {
    dG4Year: number | undefined
    setTimeSeries: Dispatch<SetStateAction<ITimeSeries | undefined>>,
    setHasChanges: Dispatch<SetStateAction<boolean>>,
    timeSeriesTitle: string,
    firstYear: number | undefined,
    lastYear: number | undefined,
    setFirstYear: Dispatch<SetStateAction<number | undefined>>,
    setLastYear: Dispatch<SetStateAction<number | undefined>>,
    timeSeries: ITimeSeries | undefined
}

const TimeSeries = ({
    dG4Year,
    setTimeSeries,
    setHasChanges,
    timeSeries,
    timeSeriesTitle,
    firstYear,
    lastYear,
    setFirstYear,
    setLastYear,
}: Props) => {
    const [columns, setColumns] = useState<string[]>([""])
    const [gridData, setGridData] = useState<CellValue[][]>([[]])
    const [dialogOpen, setDialogOpen] = useState(false)

    const buildAlignedGrid = (updatedTimeSeries: ITimeSeries) => {
        if (updatedTimeSeries !== undefined && timeSeries !== undefined) {
            const columnTitles: string[] = []
            if (firstYear !== undefined && lastYear !== undefined) {
                for (let i = firstYear; i < lastYear; i += 1) {
                    columnTitles.push(i.toString())
                }
            }
            setColumns(columnTitles)

            const zeroesAtStart: Number[] = Array.from({
                length: Number(timeSeries!.startYear!)
                + Number(dG4Year) - Number(firstYear),
            }, (() => 0))

            const zeroesAtEnd: Number[] = Array.from({
                length: Number(lastYear)
                - (Number(timeSeries!.startYear!)
                + Number(dG4Year)
                + Number(timeSeries!.values!.length!)),
            }, (() => 0))

            const assetZeroesStartGrid = buildZeroGridData(zeroesAtStart)
            const assetZeroesEndGrid = buildZeroGridData(zeroesAtEnd)
            const newGridData = buildGridData(timeSeries)

            const alignedAssetGridData = new Array(
                assetZeroesStartGrid[0].concat(newGridData[0], assetZeroesEndGrid[0]),
            )
            setGridData(alignedAssetGridData)
        } else {
            setColumns([])
            setGridData([[]])
        }
    }

    useEffect(() => {
        buildAlignedGrid(timeSeries!)
    }, [timeSeries, lastYear, firstYear])

    const onCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData: CellValue[][] = replaceOldData(gridData, changes)
        setGridData(newGridData)
        setColumns(getColumnAbsoluteYears(dG4Year, timeSeries))
        setHasChanges(true)
    }

    const onImport = (input: string, year: number) => {
        const newTimeSeries: ITimeSeries = { ...timeSeries }
        newTimeSeries.startYear = year
        newTimeSeries.values = input.replace(/(\r\n|\n|\r)/gm, "").split("\t").map((i) => parseFloat(i))
        setTimeSeries(newTimeSeries)
        if ((Number(year)
<<<<<<< HEAD
        + Number(caseItem?.DG4Date?.getFullYear())) < (earliestYear ?? Number.MAX_SAFE_INTEGER)) {
            setEarliestYear((Number(year) + Number(caseItem?.DG4Date?.getFullYear())))
        }
        if ((Number(year)
        + Number(caseItem?.DG4Date?.getFullYear())
        + Number(newTimeSeries.values.length)) > (latestYear ?? Number.MIN_SAFE_INTEGER)) {
            setLatestYear(Number(year)
            + Number(caseItem?.DG4Date?.getFullYear()) + Number(newTimeSeries.values.length))
=======
        + Number(dG4Year!)) < (firstYear ?? Number.MAX_SAFE_INTEGER)) {
            setFirstYear((Number(year) + Number(dG4Year!)))
        }
        if ((Number(year)
        + Number(dG4Year!)
        + Number(newTimeSeries!.values!.length)) > (lastYear ?? Number.MIN_SAFE_INTEGER)) {
            setLastYear(Number(year)
            + Number(dG4Year!) + Number(newTimeSeries.values.length))
>>>>>>> main
        }
        buildAlignedGrid(newTimeSeries)
        setDialogOpen(!dialogOpen)
        setHasChanges(true)
    }

    const deleteTimeseries = () => {
        setHasChanges(true)
        setColumns([])
        setGridData([[]])
        setTimeSeries(undefined)
    }

    return (
        <>
            <Wrapper>
                <Typography variant="h4">{timeSeriesTitle}</Typography>
                <ImportButton onClick={() => { setDialogOpen(true) }}>Import</ImportButton>
                <ImportButton
                    disabled={timeSeries === undefined}
                    color="danger"
                    onClick={deleteTimeseries}
                >
                    Delete
                </ImportButton>
            </Wrapper>
            <WrapperColumn>
                <DataTable
                    columns={columns}
                    gridData={gridData}
                    onCellsChanged={onCellsChanged}
                    dG4Year={dG4Year?.toString()!}
                />
            </WrapperColumn>
            {!dialogOpen ? null
                : <Import onClose={() => { setDialogOpen(!dialogOpen) }} onImport={onImport} />}
        </>
    )
}

export default TimeSeries
