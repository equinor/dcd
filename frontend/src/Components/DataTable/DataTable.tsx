import React from 'react'
import ReactDataSheet from 'react-datasheet'
import 'react-datasheet/lib/react-datasheet.css'
import './style.css'

export interface CellValue {
    value: number | string
    readOnly?: boolean
}

class Table extends ReactDataSheet<CellValue> {}

interface Props {
    columns: string[]
    gridData: any
    onCellsChanged: any
}

const DataTable = ({ columns, gridData, onCellsChanged }: Props) => {
    return (
        <Table
            onCellsChanged={onCellsChanged}
            data={gridData}
            valueRenderer={cell => (cell.readOnly ? cell.value : new Intl.NumberFormat('no-NO').format(cell.value as number))}
            handleCopy={({ data, start, end }) => {
                let string = ''
                for (let i = start.i; i <= end.i; i++) {
                    for (let j = start.j; j <= end.j; j++) {
                        string = string + data[i][j].value + ' '
                    }
                    string = string + `\n`
                }
                navigator.clipboard.writeText(string)
            }}
            sheetRenderer={props => (
                <>
                    <table className={props.className}>
                        <thead>
                            <tr>
                                <th className="table-header"></th>
                                {columns.map((column, index) => (
                                    <th className="table-header" key={index}>
                                        <span>{column}</span>
                                    </th>
                                ))}
                            </tr>
                        </thead>
                        <tbody>{props.children}</tbody>
                    </table>
                </>
            )}
        />
    )
}

export default DataTable
