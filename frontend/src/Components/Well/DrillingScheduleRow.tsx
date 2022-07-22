import { Typography } from "@equinor/eds-core-react"
import {
    Dispatch, SetStateAction, useEffect, useState,
} from "react"
import DataTable, { CellValue } from "../DataTable/DataTable"
import {
    buildGridData, buildZeroGridData, getColumnAbsoluteYears, replaceOldData,
} from "../DataTable/helpers"
import Import from "../Import/Import"
import { ITimeSeries } from "../../models/ITimeSeries"
import {
    DeleteButton, ImportButton, Wrapper, WrapperColumn,
} from "../../Views/Asset/StyledAssetComponents"
import { WellProjectWell } from "../../models/WellProjectWell"
import { Project } from "../../models/Project"
import { GetWellProjectWellService } from "../../Services/WellProjectWellService"
import { ExplorationWell } from "../../models/ExplorationWell"
import { GetExplorationWellService } from "../../Services/ExplorationWellService"

interface Props {
    dG4Year: number | undefined
    timeSeriesTitle: string,
    firstYear: number | undefined,
    lastYear: number | undefined,
    setFirstYear: Dispatch<SetStateAction<number | undefined>>,
    setLastYear: Dispatch<SetStateAction<number | undefined>>,
    wellProjectWell?: WellProjectWell | undefined
    explorationWell?: ExplorationWell | undefined
    setProject: Dispatch<SetStateAction<Project | undefined>>
}

const DrillingScheduleRow = ({
    dG4Year,
    timeSeriesTitle,
    firstYear,
    lastYear,
    setFirstYear,
    setLastYear,
    wellProjectWell,
    explorationWell,
    setProject,
}: Props) => {
    const [columns, setColumns] = useState<string[]>([""])
    const [gridData, setGridData] = useState<CellValue[][]>([[]])
    const [dialogOpen, setDialogOpen] = useState(false)
    // eslint-disable-next-line max-len
    const [drillingSchedule, setDrillingSchedule] = useState<ITimeSeries | undefined>(wellProjectWell?.drillingSchedule ?? explorationWell?.drillingSchedule)

    const buildAlignedGrid = (updatedTimeSeries: ITimeSeries) => {
        if (updatedTimeSeries !== undefined && drillingSchedule !== undefined) {
            const columnTitles: string[] = []
            if (firstYear !== undefined && lastYear !== undefined) {
                for (let i = firstYear; i < lastYear; i += 1) {
                    columnTitles.push(i.toString())
                }
            }
            setColumns(columnTitles)

            const zeroesAtStart: Number[] = Array.from({
                length: Number(drillingSchedule!.startYear!)
                + Number(dG4Year) - Number(firstYear),
            }, (() => 0))

            const zeroesAtEnd: Number[] = Array.from({
                length: Number(lastYear)
                - (Number(drillingSchedule!.startYear!)
                + Number(dG4Year)
                + Number(drillingSchedule!.values!.length!)),
            }, (() => 0))

            const assetZeroesStartGrid = buildZeroGridData(zeroesAtStart)
            const assetZeroesEndGrid = buildZeroGridData(zeroesAtEnd)
            const newGridData = buildGridData(drillingSchedule)

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
        buildAlignedGrid(drillingSchedule!)
    }, [drillingSchedule, lastYear, firstYear])

    const onCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData: CellValue[][] = replaceOldData(gridData, changes)
        setGridData(newGridData)
        setColumns(getColumnAbsoluteYears(dG4Year, drillingSchedule))
    }

    const onImport = async (input: string, year: number) => {
        const newTimeSeries: ITimeSeries = { ...drillingSchedule }
        newTimeSeries.startYear = year
        newTimeSeries.values = input.replace(/(\r\n|\n|\r)/gm, "").split("\t").map((i) => parseFloat(i))
        setDrillingSchedule(newTimeSeries)
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
        if (wellProjectWell) {
            const newAssetWell: WellProjectWell = { ...wellProjectWell }
            newAssetWell.drillingSchedule = newTimeSeries
            const newProject = await (await GetWellProjectWellService()).updateWellProjectWell(newAssetWell)
            setProject(newProject)
        } else if (explorationWell) {
            const newAssetWell: ExplorationWell = { ...explorationWell }
            newAssetWell.drillingSchedule = newTimeSeries
            const newProject = await (await GetExplorationWellService()).updateExplorationWell(newAssetWell)
            setProject(newProject)
        }
    }

    const deleteTimeseries = async () => {
        if (wellProjectWell) {
            const newAssetWell: WellProjectWell = { ...wellProjectWell }
            newAssetWell.drillingSchedule = undefined
            const newProject = await (await GetWellProjectWellService()).updateWellProjectWell(newAssetWell)
            setProject(newProject)
        } else if (explorationWell) {
            const newAssetWell: ExplorationWell = { ...explorationWell }
            newAssetWell.drillingSchedule = undefined
            const newProject = await (await GetExplorationWellService()).updateExplorationWell(newAssetWell)
            setProject(newProject)
        }

        setColumns([])
        setGridData([[]])
        setDrillingSchedule(undefined)
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
                    {drillingSchedule !== undefined ? "Edit" : "Import"}
                </ImportButton>
                <DeleteButton
                    disabled={drillingSchedule === undefined}
                    color="danger"
                    onClick={deleteTimeseries}
                >
                    Delete
                </DeleteButton>
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

export default DrillingScheduleRow
