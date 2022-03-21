import { Case } from "../../models/Case"
import { CellValue } from "./DataTable"

export const generateEmptyGridWithRowTitles = (rowTitles: string[]) => {
    const grid: CellValue[][] = []
    rowTitles.forEach((columnName) => {
        grid.push([{ readOnly: true, value: columnName }])
    })
    return grid
}

export const replaceOldData = (
    oldGridData: CellValue[][],
    changes: { cell: { value: number }; col: number; row: number; value: string }[],
) => {
    if (oldGridData.length === 0 || oldGridData[0].length === 0) {
        return []
    }
    const gridData = oldGridData.map((row) => [...row])
    changes.forEach(({ row, col, value }) => {
        if (value) {
            // Remove white spaces in string due to number formatting, and convert to number
            const newValue = parseFloat(value.replace(/\s+/g, ""))
            gridData[row][col] = { ...gridData[row][col], value: newValue }
        }
    })
    return gridData
}

export const generateNewGrid = (data: { [key: string]: string }[], rowTitles: string[]) => {
    const newGridData: CellValue[][] = generateEmptyGridWithRowTitles(rowTitles)
    const newColumns: string[] = []

    data.forEach((row, index) => {
        // Overflowed data is ignored
        if (index < newGridData.length) {
            Object.entries(row).forEach(([key, value]) => {
                let columnIndex = newColumns.indexOf(key)
                if (columnIndex < 0) {
                    columnIndex = newColumns.length
                    newColumns.push(key)
                }
                // Remove white spaces in string due to number formatting, and convert to number
                const newValue = parseFloat(value.replace(/\s+/g, ""))
                newGridData[index][columnIndex] = { value: 1 }
            })
        }
    })
    return { newGridData, newColumns }
}

export const getColumnTitles = (newCaseItem?: Case, costProfile?: any) => {
    const startYears: string[] = []
    const relativeYear: number = parseInt(costProfile?.startYear, 10)
    const startYear: number = (newCaseItem?.DG4Date?.getFullYear() ?? 0) + (relativeYear ?? 0)
    costProfile?.values?.forEach((_: any, i: number) => {
        startYears.push((startYear + i).toString())
    })
    return startYears
}

export const buildGridData = (costProfile?: any) => {
    const grid: ({value: number, readOnly?: boolean})[][] = [
        [
        ],
    ]
    costProfile?.values?.forEach((element: number) => {
        grid[0].push({ value: element })
    })
    return grid
}

export const importData = (input: string, year: number) => {

}