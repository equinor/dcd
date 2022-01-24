import { CellValue } from './DataTable'

export const generateEmptyGridWithRowTitles = (rowTitles: string[]) => {
    const grid: CellValue[][] = []
    rowTitles.forEach(columnName => {
        grid.push([{ readOnly: true, value: columnName }])
    })
    return grid
}

export const replaceOldData = (
    oldGridData: CellValue[][],
    changes: { cell: { value: number }; col: number; row: number; value: string }[]
) => {
    if (oldGridData.length === 0 || oldGridData[0].length === 0) {
        return []
    }
    const gridData = oldGridData.map(row => [...row])
    changes.forEach(({ row, col, value }) => {
        if (value) {
            const newValue = parseFloat(value.replace(/\s+/g, '')) // Remove white spaces in string due to number formatting, and convert to number
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
                const newValue = parseFloat(value.replace(/\s+/g, '')) // Remove white spaces in string due to number formatting, and convert to number
                newGridData[index][columnIndex + 1] = { value: newValue }
            })
        }
    })
    return { newGridData, newColumns }
}
