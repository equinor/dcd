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
import {
    DeleteButton, ImportButton, Wrapper, WrapperColumn, WrapperTablePeriod,
} from "../Views/Asset/StyledAssetComponents"
import NumberInput from "./NumberInput"

interface Props {
    dG4Year: number | undefined
    setTimeSeries: Dispatch<SetStateAction<ITimeSeries | undefined>>,
    setHasChanges: Dispatch<SetStateAction<boolean>>,
    timeSeriesTitle: string,
    firstYear: number | undefined,
    lastYear: number | undefined,
    setFirstYear: Dispatch<SetStateAction<number | undefined>>,
    setLastYear: Dispatch<SetStateAction<number | undefined>>,
    timeSeries: ITimeSeries[] | undefined
    timeSeriesArray: ITimeSeries[] | undefined
    profileName: string[]
    profileEnum: number
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
    timeSeriesArray,
    profileName,
    profileEnum,
}: Props) => {
    const [columns, setColumns] = useState<string[]>([""])
    const [gridData, setGridData] = useState<CellValue[][]>([[]])
    const [dialogOpen, setDialogOpen] = useState(false)
    const [beginningYear, setBeginningYear] = useState<number | undefined>()
    const [endingYear, setEndingYear] = useState<number | undefined>()

    const combinedTimeseries:any = []
    const combinedEmptyTimeseries:any = []

    const buildAlignedGrid = (updatedTimeSeries: ITimeSeries) => {
        if (timeSeries![0] !== undefined && updatedTimeSeries !== undefined) {
            for (let i = 0; i < timeSeries![i]?.values?.length!; i += 1) {
                if (timeSeries![i] !== undefined) {
                    if (updatedTimeSeries !== undefined && timeSeries![i] !== undefined) {
                        const columnTitles: string[] = []
                        if (firstYear !== undefined && lastYear !== undefined) {
                            for (let j = firstYear; j < lastYear; j += 1) {
                                columnTitles.push(j.toString())
                            }
                        }
                        setColumns(columnTitles)

                        const zeroesAtStart: Number[] = Array.from({
                            length: Number(timeSeries![i].startYear!)
                            + Number(dG4Year) - Number(firstYear),
                        }, (() => 0))

                        const zeroesAtEnd: Number[] = Array.from({
                            length: Number(lastYear)
                            - (Number(timeSeries![i].startYear!)
                            + Number(dG4Year)
                            + Number(timeSeries![i].values!.length!)),
                        }, (() => 0))

                        const assetZeroesStartGrid = buildZeroGridData(zeroesAtStart)
                        const assetZeroesEndGrid = buildZeroGridData(zeroesAtEnd)
                        const newGridData = buildGridData(timeSeries![i])

                        const alignedAssetGridData = new Array(
                            assetZeroesStartGrid[0].concat(newGridData[0], assetZeroesEndGrid[0]),
                        )
                        combinedTimeseries.push(alignedAssetGridData)
                    }
                }
            }
        }
        setGridData(combinedTimeseries)
    }

    useEffect(() => {
        buildAlignedGrid(combinedTimeseries!)

        if (gridData !== undefined && firstYear?.toString().length === 4 && lastYear?.toString().length === 4
            && beginningYear === undefined && endingYear === undefined) {
            setBeginningYear(firstYear)
            setEndingYear(lastYear - 1)
        }
    }, [timeSeries, lastYear, firstYear])

    const onCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData: CellValue[][] = replaceOldData(gridData, changes)
        setGridData(newGridData)
        setColumns(getColumnAbsoluteYears(dG4Year, timeSeries))
        setHasChanges(true)
    }

    const onImport = (input: string, year: number) => {
        const newTimeSeries: ITimeSeries = { ...timeSeries![0] }
        newTimeSeries.startYear = year
        newTimeSeries.values = input.replace(/(\r\n|\n|\r)/gm, "").split("\t").map((i) => parseFloat(i))
        setTimeSeries(newTimeSeries)
        if ((Number(year)
        + Number(dG4Year!)) < (firstYear ?? Number.MAX_SAFE_INTEGER)) {
            setFirstYear((Number(year) + Number(dG4Year!)))
        }
        if ((Number(year)
        + Number(dG4Year!)
        + Number(newTimeSeries!.values!.length)) > (lastYear ?? Number.MIN_SAFE_INTEGER)) {
            setLastYear(Number(year)
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

    const createEmptyGrid = (j: any) => {
        if (gridData !== undefined && firstYear?.toString().length === 4 && lastYear?.toString().length === 4) {
            setBeginningYear(firstYear)
            setEndingYear(lastYear - 1)
        }
        const newTimeSeries: ITimeSeries = { ...timeSeries![j] }
        const colYears = []
        for (let c = beginningYear; c! <= endingYear!; c! += 1) {
            colYears.push(c!.toString())
        }
        setColumns(colYears)

        newTimeSeries.name = profileName[j]
        newTimeSeries.startYear = beginningYear! - dG4Year!
        newTimeSeries.values = new Array(colYears.length).fill(0)

        setTimeSeries(newTimeSeries)

        if (newTimeSeries !== undefined) {
            const zeroesAtStart: Number[] = Array.from({
                length: Number(newTimeSeries.startYear!)
                + Number(dG4Year) - Number(beginningYear),
            }, (() => 0))

            const zeroesAtEnd: Number[] = Array.from({
                length: Number(lastYear)
                - (Number(newTimeSeries.startYear!)
                + Number(dG4Year)
                + Number(newTimeSeries.values!.length!)),
            }, (() => 0))

            const assetZeroesStartGrid = buildZeroGridData(zeroesAtStart)
            const assetZeroesEndGrid = buildZeroGridData(zeroesAtEnd)
            const newGridData = buildGridData(newTimeSeries)

            const alignedAssetGridData = new Array(
                assetZeroesStartGrid[0].concat(newGridData[0], assetZeroesEndGrid[0]),
            )

            combinedEmptyTimeseries.push(alignedAssetGridData)
        }
        setGridData(combinedEmptyTimeseries)
        setHasChanges(true)
    }

    const createNewGridWithData = (j: any) => {
        const newTimeSeries: ITimeSeries = { ...timeSeries![j] }
        const colYears = []
        for (let c = beginningYear; c! <= endingYear!; c! += 1) {
            colYears.push(c!.toString())
        }
        setColumns(colYears)
        newTimeSeries.name = profileName[j]
        newTimeSeries.startYear = timeSeries![j].startYear

        if (beginningYear! < (Number(timeSeries![j].startYear!) + Number(dG4Year!))) {
            newTimeSeries.startYear = beginningYear! - dG4Year!
            // eslint-disable-next-line max-len
            newTimeSeries.values = new Array(colYears.length - newTimeSeries.values!.length!).fill(0).concat(newTimeSeries.values)
        }

        if ((endingYear!) > (Number(colYears[0]) + Number(newTimeSeries.values!.length))) {
            // this breaks table when adding value after having increased ending year...
            // eslint-disable-next-line max-len
            newTimeSeries.values = (newTimeSeries.values)?.concat(new Array(colYears.length - timeSeries![j].values!.length!).fill(0))
        }

        if (endingYear! < (Number(colYears[0]) + Number(timeSeries![j].values!.length))) {
            // colyears[0] goes up when increasing startyear
            // this breaks table when adding value after having increased ending year...
            const yearDifference = (Number(colYears[0]) + timeSeries![j].values!.length - 1) - endingYear!
            newTimeSeries.values = timeSeries![j].values?.slice(0, -yearDifference)
        }

        if (beginningYear! > (Number(timeSeries![j].startYear!) + Number(dG4Year!))) {
            newTimeSeries.startYear = beginningYear! - dG4Year!
            const yearDifference = beginningYear! - (timeSeries![j].startYear! + dG4Year!)
            newTimeSeries.values = timeSeries![j].values?.slice(yearDifference)
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

    const addTimeSeries = () => {
        const colYears = []
        if (beginningYear?.toString().length === 4 && endingYear?.toString().length === 4) {
            for (let j = beginningYear; j! < endingYear!; j! += 1) {
                colYears.push(j!.toString())
            }
            setColumns(colYears)

            if (timeSeries![0] === undefined) {
                for (let i = 0; i < timeSeries?.length!; i += 1) {
                    createEmptyGrid(i)
                }
            }
            if (timeSeries![0] !== undefined) {
                for (let i = 0; i < timeSeries?.length!; i += 1) {
                    createNewGridWithData(i)
                }
            }
        }
    }

    return (
        <>
            <Wrapper>
                <Typography variant="h4">{timeSeriesTitle}</Typography>
            </Wrapper>
            <Wrapper>
                <ImportButton
                    onClick={() => { setDialogOpen(true) }}
                >
                    {timeSeries !== undefined ? "Edit" : "Import"}
                </ImportButton>
                <DeleteButton
                    disabled={timeSeries === undefined}
                    color="danger"
                    onClick={deleteTimeseries}
                >
                    Delete
                </DeleteButton>
            </Wrapper>
            <WrapperTablePeriod>
                <NumberInput // must fix new custom wrapper that breaks other numberinputs
                    value={beginningYear?.toString().length === 4 ? beginningYear : 2020}
                    setValue={setBeginningYear}
                    integer
                    label="Start year"
                />
                <Typography variant="h2">-</Typography>
                <NumberInput
                    value={endingYear?.toString().length === 4 ? endingYear : 2030}
                    setValue={setEndingYear}
                    integer
                    label="End year"
                />
                <ImportButton
                    onClick={addTimeSeries}
                >
                    Apply
                </ImportButton>
            </WrapperTablePeriod>

            <WrapperColumn>
                <DataTable
                    columns={columns}
                    gridData={gridData}
                    onCellsChanged={onCellsChanged}
                    dG4Year={dG4Year?.toString()!}
                    timeSeriesArray={timeSeriesArray}
                    profileName={profileName}
                    profileEnum={profileEnum}
                    setHasChanges={setHasChanges}
                    setTimeSeries={setTimeSeries}
                    timeSeries={timeSeries}
                />
            </WrapperColumn>
            {!dialogOpen ? null
                : <Import onClose={() => { setDialogOpen(!dialogOpen) }} onImport={onImport} />}
        </>
    )
}

export default TimeSeries
