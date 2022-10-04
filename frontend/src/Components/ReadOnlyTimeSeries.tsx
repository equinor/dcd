import { Typography } from "@equinor/eds-core-react"
import {
    useEffect, useState,
} from "react"
import { CellValue } from "./DataTable/DataTable"
import {
    buildGridData,
} from "./DataTable/helpers"
import { ITimeSeries } from "../models/ITimeSeries"
import {
    WrapperColumn,
} from "../Views/Asset/StyledAssetComponents"
import DataTableReadOnly from "./DataTable/DataTableReadOnly"

interface Props {
    dG4Year: number
    firstYear: number | undefined,
    lastYear: number | undefined,
    profileEnum: number
    profileType: string
    readOnlyTimeSeries: (ITimeSeries | undefined)[]
    readOnlyName: string[]
}

const ReadOnlyTimeSeries = ({
    dG4Year,
    firstYear,
    lastYear,
    profileEnum,
    profileType,
    readOnlyTimeSeries,
    readOnlyName,
}: Props) => {
    const [columns, setColumns] = useState<string[]>([""])
    const [, setGridData] = useState<CellValue[][]>([[]])
    const [tableFirstYear, setTableFirstYear] = useState<number>(Number.MAX_SAFE_INTEGER)
    const [tableLastYear, setTableLastYear] = useState<number>(Number.MIN_SAFE_INTEGER)

    const combinedEmptyTimeseries: any = []

    const isValidYear = (year: number | undefined) => year?.toString().length === 4

    const createNewGridWithReadOnlyData = (j: number) => {
        if (tableFirstYear && tableLastYear && readOnlyTimeSeries !== undefined) {
            const newGridData = buildGridData(readOnlyTimeSeries[j])
            const alignedAssetGridData = new Array(newGridData[0])
            combinedEmptyTimeseries.push(alignedAssetGridData)
            setGridData(combinedEmptyTimeseries)
        }
    }

    const setYearsIfTableHasData = (i: number) => {
        if (readOnlyTimeSeries[i]?.values?.length !== 0) {
            if (tableFirstYear && tableLastYear && isValidYear(tableFirstYear) && isValidYear(tableLastYear)) {
                const colYears = []
                for (let c = tableFirstYear; c <= tableLastYear; c += 1) {
                    colYears.push(c.toString())
                }
                setColumns(colYears)
            }
            if (isValidYear(firstYear) && isValidYear(lastYear)
            && tableFirstYear === Number.MAX_SAFE_INTEGER && tableLastYear === Number.MIN_SAFE_INTEGER
            && firstYear && lastYear) {
                setTableFirstYear(firstYear)
                setTableLastYear(lastYear - 1)
            }
        }
    }

    useEffect(() => {
        for (let i = 0; i < readOnlyTimeSeries?.length; i += 1) {
            if (readOnlyTimeSeries[i] !== undefined) {
                createNewGridWithReadOnlyData(i)
            }
            setYearsIfTableHasData(i)
        }
    }, [readOnlyTimeSeries, lastYear, firstYear])

    return (
        <>
            <Typography variant="h2">{profileType}</Typography>

            <WrapperColumn>
                <DataTableReadOnly
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

export default ReadOnlyTimeSeries
