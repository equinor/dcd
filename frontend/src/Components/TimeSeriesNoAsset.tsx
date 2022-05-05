import { Typography } from "@equinor/eds-core-react"
import {
    Dispatch, SetStateAction, useEffect, useState,
} from "react"
import DataTable, { CellValue } from "./DataTable/DataTable"
import {
    buildGridData, buildZeroGridData, getColumnAbsoluteYears, replaceOldData,
} from "./DataTable/helpers"
import Import from "./Import/Import"
import { Case } from "../models/Case"
import { ITimeSeries } from "../models/ITimeSeries"
import { ImportButton, Wrapper, WrapperColumn } from "../Views/Asset/StyledAssetComponents"

interface Props {
    dG4Year: number | undefined
    setTimeSeries: Dispatch<SetStateAction<ITimeSeries | undefined>>,
    setHasChanges: Dispatch<SetStateAction<boolean>>,
    timeSeriesTitle: string,
    earliestYear: number | undefined,
    latestYear: number | undefined,
    setEarliestYear: Dispatch<SetStateAction<number>>,
    setLatestYear: Dispatch<SetStateAction<number>>,
    timeSeries: ITimeSeries | undefined
}

const TimeSeriesNoAsset = ({
    dG4Year,
    setTimeSeries,
    setHasChanges,
    timeSeries,
    timeSeriesTitle,
    earliestYear,
    latestYear,
    setEarliestYear,
    setLatestYear,
}: Props) => {
    const [columns, setColumns] = useState<string[]>([""])
    const [gridData, setGridData] = useState<CellValue[][]>([[]])
    const [dialogOpen, setDialogOpen] = useState(false)

    const buildAlignedGrid = (updatedTimeSeries: ITimeSeries) => {
        if (updatedTimeSeries !== undefined && timeSeries !== undefined) {
            let tempEarliest = earliestYear
            let tempLatest = latestYear
            console.log("Earliest", earliestYear)

            if ((Number(updatedTimeSeries.startYear)
            + Number(dG4Year!)) < (earliestYear ?? Number.MAX_SAFE_INTEGER)) {
                tempEarliest = (Number(updatedTimeSeries.startYear) + Number(dG4Year!))
                console.log("Entered if")
                setEarliestYear((Number(updatedTimeSeries.startYear) + Number(dG4Year!)))
            }
            if ((Number(updatedTimeSeries.startYear)
            + Number(dG4Year!)
            + Number(updatedTimeSeries!.values!.length)) > (latestYear ?? Number.MIN_SAFE_INTEGER)) {
                tempLatest = Number(updatedTimeSeries.startYear)
                + Number(dG4Year!) + Number(updatedTimeSeries.values!.length)

                setLatestYear(Number(updatedTimeSeries.startYear)
                + Number(dG4Year!) + Number(updatedTimeSeries.values!.length))
            }

            const columnTitles: string[] = []
            if (tempEarliest !== undefined && tempLatest !== undefined) {
                for (let i = tempEarliest; i < tempLatest; i += 1) {
                    columnTitles.push(i.toString())
                }
            }

            const zeroesAtStart = Array.from({
                length: Number(timeSeries!.startYear!)
                + Number(dG4Year) - Number(tempEarliest),
            }, (() => 0))
            const zeroesAtEnd = Array.from({
                length: Number(tempLatest)
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

            setColumns(columnTitles)
            setGridData(alignedAssetGridData)
        } else {
            setColumns([])
            setGridData([[]])
        }
    }

    useEffect(() => {
        buildAlignedGrid(timeSeries!)
    }, [timeSeries])

    const onCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData = replaceOldData(gridData, changes)
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
        + Number(dG4Year!)) < (earliestYear ?? Number.MAX_SAFE_INTEGER)) {
            setEarliestYear((Number(year) + Number(dG4Year!)))
        }
        if ((Number(year)
        + Number(dG4Year!)
        + Number(newTimeSeries!.values!.length)) > (latestYear ?? Number.MIN_SAFE_INTEGER)) {
            setLatestYear(Number(year)
            + Number(dG4Year!) + Number(newTimeSeries.values.length))
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

export default TimeSeriesNoAsset
