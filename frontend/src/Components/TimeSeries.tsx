import { Typography } from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import DataTable, { CellValue } from "../Components/DataTable/DataTable"
import {
    buildGridData, getColumnAbsoluteYears, replaceOldData,
} from "../Components/DataTable/helpers"
import Import from "../Components/Import/Import"
import TimeSeriesEnum from "../models/assets/TimeSeriesEnum"
import { Case } from "../models/Case"
import { ImportButton, Wrapper, WrapperColumn } from "../Views/Asset/StyledAssetComponents"

interface ITimeSeries {
    id?: string
    startYear?: number | undefined
    values?: any [] | null
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined
}

interface IAsset {
    id?: string | undefined
    name?: string | undefined
    projectId?: string | undefined
    costProfile?: ITimeSeries | undefined
    drillingSchedule?: ITimeSeries | undefined
    co2Emissions?: ITimeSeries | undefined
    netSalesGas?: ITimeSeries | undefined
    substructureCessationCostProfileDto?: ITimeSeries | undefined
    surfCessationCostProfileDto?: ITimeSeries | undefined
    topsideCessationCostProfileDto?: ITimeSeries | undefined
    transportCessationCostProfileDto?: ITimeSeries | undefined
    fuelFlaringAndLosses?: ITimeSeries | undefined
    productionProfileGas?: ITimeSeries | undefined
    productionProfileOil?: ITimeSeries | undefined
    productionProfileWater?: ITimeSeries | undefined
    productionProfileWaterInjection?: ITimeSeries | undefined
    dryweight?: number | undefined
    maturity?: Components.Schemas.Maturity | undefined
}

interface Props {
    caseItem: Case | undefined,
    setAsset: React.Dispatch<React.SetStateAction<any | undefined>>
    setHasChanges: React.Dispatch<React.SetStateAction<boolean>>
    asset: IAsset | undefined,
    timeSeriesType: TimeSeriesEnum,
    assetName: string
    timeSeriesTitle: string
}

const TimeSeries = ({
    caseItem,
    setAsset,
    setHasChanges,
    asset,
    timeSeriesType,
    assetName,
    timeSeriesTitle,
}: Props) => {
    const [columns, setColumns] = useState<string[]>([""])
    const [gridData, setGridData] = useState<CellValue[][]>([[]])
    const [dialogOpen, setDialogOpen] = useState(false)

    useEffect(() => {
        if (asset !== undefined) {
            const newColumnTitles = getColumnAbsoluteYears(caseItem, asset[timeSeriesType])
            setColumns(newColumnTitles)
            const newGridData = buildGridData(asset[timeSeriesType])
            setGridData(newGridData)
        }
    }, [asset])

    const onCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData = replaceOldData(gridData, changes)
        setGridData(newGridData)
        setColumns(getColumnAbsoluteYears(caseItem, asset![timeSeriesType]))
        setHasChanges(true)
    }

    const onImport = (input: string, year: number) => {
        const newAsset: IAsset = { ...asset }
        const newTimeSeries: ITimeSeries = { ...newAsset[timeSeriesType] }
        newAsset[timeSeriesType] = newTimeSeries
        newTimeSeries.startYear = year
        newTimeSeries.values = input.replace(/(\r\n|\n|\r)/gm, "").split("\t").map((i) => parseFloat(i))
        newTimeSeries.epaVersion = ""
        setAsset(newAsset)
        const newColumnTitles = getColumnAbsoluteYears(caseItem, newTimeSeries)
        setColumns(newColumnTitles)
        const newGridData = buildGridData(newTimeSeries)
        setGridData(newGridData)
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
