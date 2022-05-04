import { Typography } from "@equinor/eds-core-react"
import {
    Dispatch, SetStateAction, useEffect, useState,
} from "react"
import DataTable, { CellValue } from "../Components/DataTable/DataTable"
import {
    buildGridData, buildZeroGridData, getColumnAbsoluteYears, replaceOldData,
} from "../Components/DataTable/helpers"
import Import from "../Components/Import/Import"
import TimeSeriesEnum from "../models/assets/TimeSeriesEnum"
import { Case } from "../models/Case"
import { ITimeSeries } from "../models/ITimeSeries"
import { ImportButton, Wrapper, WrapperColumn } from "../Views/Asset/StyledAssetComponents"

interface Props {
    caseItem: Case | undefined,
    setAsset: Dispatch<SetStateAction<any | undefined>>,
    setHasChanges: Dispatch<SetStateAction<boolean>>,
    asset: IAsset | undefined,
    timeSeriesType: TimeSeriesEnum,
    assetName: string,
    timeSeriesTitle: string,
    earliestYear: number | undefined,
    latestYear: number | undefined,
    setEarliestYear: Dispatch<SetStateAction<number | undefined>>,
    setLatestYear: Dispatch<SetStateAction<number | undefined>>,
}

interface IAsset {
    id?: string | undefined
    name?: string | undefined
    projectId?: string | undefined
    costProfile?: ITimeSeries | undefined
    drillingSchedule?: ITimeSeries | undefined
    gAndGAdminCost?: ITimeSeries | undefined
    co2Emissions?: ITimeSeries | undefined
    netSalesGas?: ITimeSeries | undefined
    fuelFlaringAndLosses?: ITimeSeries | undefined
    productionProfileGas?: ITimeSeries | undefined
    productionProfileOil?: ITimeSeries | undefined
    productionProfileWater?: ITimeSeries | undefined
    productionProfileWaterInjection?: ITimeSeries | undefined
    substructureCessationCostProfileDto?: ITimeSeries | undefined
    surfCessationCostProfileDto?: ITimeSeries | undefined
    topsideCessationCostProfileDto?: ITimeSeries | undefined
    transportCessationCostProfileDto?: ITimeSeries | undefined
    dryweight?: number | undefined
    maturity?: Components.Schemas.Maturity | undefined
}

const TimeSeries = ({
    caseItem,
    setAsset,
    setHasChanges,
    asset,
    timeSeriesType,
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

    const buildAlignedGrid = (updatedAsset: IAsset) => {
        if (updatedAsset !== undefined && updatedAsset[timeSeriesType] !== undefined) {
            const columnTitles: string[] = []
            if (earliestYear !== undefined && latestYear !== undefined) {
                for (let i = earliestYear; i < latestYear; i += 1) {
                    columnTitles.push(i.toString())
                }
            }

            const zeroesAtStart = Array.from({
                length: Number(updatedAsset[timeSeriesType]!.startYear!)
                + Number(caseItem!.DG4Date!.getFullYear()) - Number(earliestYear),
            }, (() => 0))
            const zeroesAtEnd = Array.from({
                length: Number(latestYear)
                - (Number(updatedAsset[timeSeriesType]!.startYear!)
                + Number(caseItem!.DG4Date!.getFullYear())
                + Number(updatedAsset[timeSeriesType]!.values!.length!)),
            }, (() => 0))

            const assetZeroesStartGrid = buildZeroGridData(zeroesAtStart)
            const assetZeroesEndGrid = buildZeroGridData(zeroesAtEnd)
            const newGridData = buildGridData(updatedAsset[timeSeriesType])

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
        buildAlignedGrid(asset!)
    }, [asset])

    const onCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData = replaceOldData(gridData, changes)
        setGridData(newGridData)
        setColumns(getColumnAbsoluteYears(caseItem?.DG4Date?.getFullYear(), asset![timeSeriesType]))
        setHasChanges(true)
    }

    const onImport = (input: string, year: number) => {
        const newAsset: IAsset = { ...asset }
        const newTimeSeries: ITimeSeries = { ...newAsset[timeSeriesType] }
        newAsset[timeSeriesType] = newTimeSeries
        newTimeSeries.startYear = year
        newTimeSeries.values = input.replace(/(\r\n|\n|\r)/gm, "").split("\t").map((i) => parseFloat(i))
        // newTimeSeries.epaVersion = ""
        setAsset(newAsset)
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
        buildAlignedGrid(newAsset)
        setDialogOpen(!dialogOpen)
        if (assetName !== "") {
            setHasChanges(true)
        }
    }

    const deleteCostProfile = () => {
        const assetCopy: IAsset = { ...asset }
        assetCopy[timeSeriesType] = undefined
        if (assetName !== "") {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
        setColumns([])
        setGridData([[]])
        setAsset(assetCopy)
    }

    return (
        <>
            <Wrapper>
                <Typography variant="h4">{timeSeriesTitle}</Typography>
                <ImportButton onClick={() => { setDialogOpen(true) }}>Import</ImportButton>
                <ImportButton
                    disabled={asset !== undefined && asset[timeSeriesType] === undefined}
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

export default TimeSeries
