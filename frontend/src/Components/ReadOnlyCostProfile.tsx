import { Typography } from "@equinor/eds-core-react"
import {
    useEffect, useState,
} from "react"
import DataTable, { CellValue } from "./DataTable/DataTable"
import {
    buildGridData,
} from "./DataTable/helpers"
import { ITimeSeries } from "../models/ITimeSeries"
import {
    Wrapper, WrapperColumn,
} from "../Views/Asset/StyledAssetComponents"

interface Props {
    dG4Year: number | undefined
    timeSeries: ITimeSeries | undefined
    title: string
}

const ReadOnlyCostProfile = ({
    dG4Year,
    timeSeries,
    title,
}: Props) => {
    const [columns, setColumns] = useState<string[]>([""])
    const [gridData, setGridData] = useState<CellValue[][]>([[]])

    useEffect(() => {
        if (timeSeries) {
            const columnTitles: string[] = []
            const { startYear } = timeSeries
            if (startYear && dG4Year) {
                timeSeries.values?.forEach((_, i) => {
                    columnTitles.push((dG4Year + startYear + i).toString())
                })
            }

            setColumns(columnTitles)

            const newGridData = buildGridData(timeSeries)

            setGridData(newGridData)
        } else {
            setColumns([])
            setGridData([[]])
        }
    }, [timeSeries])

    return (
        <>
            <Wrapper>
                <Typography variant="h4">{title}</Typography>
            </Wrapper>

            <WrapperColumn>
                {/* <DataTable
                    columns={columns}
                    gridData={gridData}
                    onCellsChanged={() => { }}
                    dG4Year={dG4Year?.toString()!}
                /> */}
            </WrapperColumn>
        </>
    )
}

export default ReadOnlyCostProfile
