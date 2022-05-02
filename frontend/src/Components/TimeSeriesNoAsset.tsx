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
    caseItem: Case | undefined,
    setTimeSeries: Dispatch<SetStateAction<ITimeSeries | undefined>>,
    setHasChanges: Dispatch<SetStateAction<boolean>>,
    assetName: string,
    timeSeriesTitle: string,
    earliestYear: number | undefined,
    latestYear: number | undefined,
    setEarliestYear: Dispatch<SetStateAction<number | undefined>>,
    setLatestYear: Dispatch<SetStateAction<number | undefined>>,
    timeSeries: ITimeSeries | undefined
}

const TimeSeriesNoAsset = ({
    caseItem,
    setTimeSeries,
    setHasChanges,
    timeSeries,
    assetName,
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
            const columnTitles: string[] = []
            if (earliestYear !== undefined && latestYear !== undefined) {
                for (let i = earliestYear; i < latestYear; i += 1) {
                    columnTitles.push(i.toString())
                }
            }

            const zeroesAtStart = Array.from({
                length: Number(timeSeries!.startYear!)
                + Number(caseItem!.DG4Date!.getFullYear()) - Number(earliestYear),
            }, (() => 0))
            const zeroesAtEnd = Array.from({
                length: Number(latestYear)
                - (Number(timeSeries!.startYear!)
                + Number(caseItem!.DG4Date!.getFullYear())
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
        setColumns(getColumnAbsoluteYears(caseItem, timeSeries))
        setHasChanges(true)
    }

    const onImport = (input: string, year: number) => {
        const newTimeSeries: ITimeSeries = { ...timeSeries }
        newTimeSeries.startYear = year
        newTimeSeries.values = input.replace(/(\r\n|\n|\r)/gm, "").split("\t").map((i) => parseFloat(i))
        setTimeSeries(newTimeSeries)
        if ((Number(year)
        + Number(caseItem!.DG4Date!.getFullYear()!)) < (earliestYear ?? Number.MAX_SAFE_INTEGER)) {
            setEarliestYear((Number(year) + Number(caseItem!.DG4Date!.getFullYear()!)))
        }
        if ((Number(year)
        + Number(caseItem!.DG4Date!.getFullYear()!)
        + Number(newTimeSeries!.values!.length)) > (latestYear ?? Number.MIN_SAFE_INTEGER)) {
            setLatestYear(Number(year)
            + Number(caseItem!.DG4Date!.getFullYear()!) + Number(newTimeSeries.values.length))
        }
        buildAlignedGrid(newTimeSeries)
        setDialogOpen(!dialogOpen)
        if (assetName !== "") {
            setHasChanges(true)
        }
    }

    const deleteCostProfile = () => {
        if (assetName !== "") {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
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
                    onClick={deleteCostProfile}
                >
                    Delete
                </ImportButton>
            </Wrapper>
            <WrapperColumn>
                <DataTable
                    columns={columns}
                    gridData={gridData}
                    onCellsChanged={onCellsChanged}
                    dG4Year={caseItem?.DG4Date?.getFullYear().toString()!}
                />
            </WrapperColumn>
            {!dialogOpen ? null
                : <Import onClose={() => { setDialogOpen(!dialogOpen) }} onImport={onImport} />}
        </>
    )
}

export default TimeSeriesNoAsset
