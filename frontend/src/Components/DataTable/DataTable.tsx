import React from "react"
import ReactDataSheet from "react-datasheet"
import "react-datasheet/lib/react-datasheet.css"
import "./style.css"

export interface CellValue {
    value: number | string
    readOnly?: boolean
}

class Table extends ReactDataSheet<CellValue> {}

interface Props {
    columns: string[]
    gridData: any
    onCellsChanged: any
    dG4Year: string
}

/* eslint-disable react/no-unstable-nested-components */
function DataTable({
    columns, gridData, onCellsChanged, dG4Year,
}: Props) {
    return (
        <Table
            onCellsChanged={onCellsChanged}
            data={gridData}
            valueRenderer={(cell) => (
                cell.readOnly
                    ? cell.value
                    : new Intl.NumberFormat("no-NO").format(cell.value as number)
            )}
            handleCopy={({ data, start, end }) => {
                let string = ""
                for (let { i } = start; i <= end.i; i += 1) {
                    for (let { j } = start; j <= end.j; j += 1) {
                        string = `${string + data[i][j].value} `
                    }
                    string += "\n"
                }
                navigator.clipboard.writeText(string)
            }}
            sheetRenderer={(props) => (
                <table className={props.className}>
                    <thead>
                        <tr>
                            {/* eslint-disable-next-line */}
                            {columns.map((column, index) => (
                                // eslint-disable-next-line max-len
                                <th className="table-header" style={{ backgroundColor: column !== dG4Year ? "white" : "#90ee90" }} key={`table-header-${index + 0}`}>
                                    <span>{column}</span>
                                </th>
                            ))}
                        </tr>
                    </thead>
                    <tbody>{props.children}</tbody>
                </table>
            )}
        />
    )
}

export default DataTable
