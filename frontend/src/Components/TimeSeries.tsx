import { Button, Typography } from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import styled from "styled-components"
import DataTable, { CellValue } from "../Components/DataTable/DataTable"
import {
    buildGridData, getColumnAbsoluteYears, replaceOldData,
} from "../Components/DataTable/helpers"
import Import from "../Components/Import/Import"
import CostProfile from "../models/assets/CostProfile"
import { Substructure } from "../models/assets/substructure/Substructure"
import { Case } from "../models/Case"

const Wrapper = styled.div`
    display: flex;
    flex-direction: row;
`

const WrapperColumn = styled.div`
    display: flex;
    flex-direction: column;
`

const ImportButton = styled(Button)`
    margin-left: 2rem;
    &:disabled {
        margin-left: 2rem;
    }
`

interface ts {
    id?: string
    startYear?: number | undefined
    values?: any [] | null
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined
}

interface asset {
    id?: string | undefined
    name?: string | undefined
    projectId?: string | undefined
    costProfile?: ts | undefined
    drillingSchedule?: ts | undefined
    co2Emissions?: ts | undefined
    netSalesGas?: ts | undefined
    fuelFlaringAndLosses?: ts | undefined
    productionProfileGas?: ts | undefined
    productionProfileOil?: ts | undefined
    productionProfileWater?: ts | undefined
    productionProfileWaterInjection?: ts | undefined
    dryweight?: number | undefined
    maturity?: Components.Schemas.Maturity | undefined
}

interface Props {
    timeSeries: ts | undefined,
    caseItem: Case | undefined,
    setAsset: React.Dispatch<React.SetStateAction<any | undefined>>
    setHasChanges: React.Dispatch<React.SetStateAction<boolean>>
    asset: asset | undefined,
    costProfile: CostProfile,
    assetName: string
    timeSeriesTitle: string
}

const TimeSeries = ({
    timeSeries,
    caseItem,
    setAsset,
    setHasChanges,
    asset,
    costProfile,
    assetName,
    timeSeriesTitle,
}: Props) => {
    const [columns, setColumns] = useState<string[]>([""])
    const [gridData, setGridData] = useState<CellValue[][]>([[]])
    const [dialogOpen, setDialogOpen] = useState(false)

    useEffect(() => {
        if (asset !== undefined) {
            const newColumnTitles = getColumnAbsoluteYears(caseItem, asset[costProfile])
            setColumns(newColumnTitles)
            const newGridData = buildGridData(asset[costProfile])
            setGridData(newGridData)
        }
    }, [asset])

    const onCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData = replaceOldData(gridData, changes)
        setGridData(newGridData)
        setColumns(getColumnAbsoluteYears(caseItem, timeSeries))
        setHasChanges(true)
    }

    const onImport = (input: string, year: number) => {
        const newAsset = { ...asset }
        const newTimeSeries = { ...timeSeries }
        newAsset[costProfile] = newTimeSeries
        newTimeSeries!.startYear = year
        // eslint-disable-next-line max-len
        newTimeSeries!.values = input.replace(/(\r\n|\n|\r)/gm, "").split("\t").map((i) => parseFloat(i))
        newTimeSeries.epaVersion = ""
        setAsset(newAsset)
        const newColumnTitles = getColumnAbsoluteYears(caseItem, newTimeSeries)
        setColumns(newColumnTitles)
        const newGridData = buildGridData(newTimeSeries)
        setGridData(newGridData)
        setDialogOpen(!dialogOpen)
        setHasChanges(true)
    }

    const deleteCostProfile = () => {
        const assetCopy = new Substructure(asset)
        assetCopy.costProfile = undefined
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
                    disabled={asset !== undefined && asset[costProfile] === undefined}
                    color="danger"
                    onClick={deleteCostProfile}
                >
                    Delete
                </ImportButton>
            </Wrapper>
            <WrapperColumn>
                <DataTable columns={columns} gridData={gridData} onCellsChanged={onCellsChanged} dG4Year="" />
            </WrapperColumn>
            {!dialogOpen ? null
                : <Import onClose={() => { setDialogOpen(!dialogOpen) }} onImport={onImport} />}
        </>
    )
}

export default TimeSeries
