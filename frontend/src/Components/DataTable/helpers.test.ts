import { generateEmptyGridWithRowTitles, generateNewGrid, replaceOldData } from "./helpers"

describe("generateEmptyGridWithRowTitles", () => {
    test("length of returned grid is the same as length of rowTitles", () => {
        expect(generateEmptyGridWithRowTitles(["title1", "title2", "title3"]).length).toBe(3)
    })

    test("return object contains only one item in inner array", () => {
        expect(generateEmptyGridWithRowTitles(["title1", "title2", "title3"])[0].length).toBe(1)
    })

    test("return object has column name as value in inner array object", () => {
        expect(generateEmptyGridWithRowTitles(["title1", "title2", "title3"])[0][0].value).toBe("title1")
    })

    test("return object has one array for each rowTitle, that each contain one object with the rowTitle as value", () => {
        expect(generateEmptyGridWithRowTitles(["title1", "title2", "title3"])).toEqual([
            [{ readOnly: true, value: "title1" }],
            [{ readOnly: true, value: "title2" }],
            [{ readOnly: true, value: "title3" }],
        ])
    })

    test("if the array of rowTitles is empty, it returns an empty array", () => {
        expect(generateEmptyGridWithRowTitles([])).toEqual([])
    })
})

describe("replaceOldData", () => {
    const oldGridData = [
        [{ readOnly: true, value: "title1" }, { value: 123 }, { value: 736 }],
        [{ readOnly: true, value: "title2" }, { value: 456 }, { value: 928 }],
        [{ readOnly: true, value: "title3" }, { value: 789 }, { value: 292 }],
    ]

    const emptyGrid = [[{ readOnly: true, value: "title1" }], [{ readOnly: true, value: "title2" }], [{ readOnly: true, value: "title3" }]]

    const changes = [
        {
            cell: { value: 123 }, col: 1, row: 0, value: "111",
        },
        {
            cell: { value: 928 }, col: 2, row: 1, value: "222",
        },
        {
            cell: { value: 736 }, col: 2, row: 0, value: "123 456",
        },
        {
            cell: { value: 789 }, col: 1, row: 2, value: "123.456",
        },
    ]

    test("for each element in the change array it sets the specified new value in the place denoted by the col and row attributes", () => {
        expect(oldGridData[0][1]).toEqual({ value: 123 })
        expect(replaceOldData(oldGridData, changes)[0][1]).toEqual({ value: 111 })
        expect(oldGridData[1][2]).toEqual({ value: 928 })
        expect(replaceOldData(oldGridData, changes)[1][2]).toEqual({ value: 222 })
    })

    test("if the changes array is empty, it makes no changes", () => {
        expect(replaceOldData(oldGridData, [])[0][1]).toEqual({ value: 123 })
        expect(replaceOldData(oldGridData, [])[1][2]).toEqual({ value: 928 })
    })

    test("if the old grid data is \"empty\", it still inserts the new data", () => {
        expect(replaceOldData(emptyGrid, changes)[0][1]).toEqual({ value: 111 })
        expect(replaceOldData(emptyGrid, changes)[1][2]).toEqual({ value: 222 })
    })

    test("if the old grid data is just an empty array, it returns an empty array", () => {
        expect(replaceOldData([], changes)).toEqual([])
    })

    test("if the changes is a formatted string (with spaces) it handles it by removing the spaces", () => {
        expect(replaceOldData(emptyGrid, changes)[0][2]).toEqual({ value: 123456 })
    })

    test("if the changes has a decimal, the string is properly parsed to number", () => {
        expect(replaceOldData(emptyGrid, changes)[2][1]).toEqual({ value: 123.456 })
    })
})

describe("generateNewGrid", () => {
    const rowTitles = ["title1", "title2", "title3"]

    const newGridValues = [
        { 2022: "123", 2023: "131.415" },
        { 2022: "456", 2023: "161 718" },
        { 2022: "789", 2023: "192021" },
        { 2022: "101112", 2023: "222324" },
    ]

    const gridDataWithOnlyRowTitles = [
        [{ readOnly: true, value: "title1" }],
        [{ readOnly: true, value: "title2" }],
        [{ readOnly: true, value: "title3" }],
    ]

    test("it converts string input to number in the resulting array", () => {
        expect(typeof generateNewGrid(newGridValues, rowTitles).newGridData[0][1].value).toEqual("number")
    })

    test("it handles decimals", () => {
        expect(generateNewGrid(newGridValues, rowTitles).newGridData[0][2].value).toEqual(131.415)
    })

    test("it handles formatted strings", () => {
        expect(generateNewGrid(newGridValues, rowTitles).newGridData[1][2].value).toEqual(161718)
    })

    test("it doesnt change the row titles", () => {
        expect(generateNewGrid(newGridValues, rowTitles).newGridData[0][0].value).toEqual("title1")
    })

    test("the existing data is replaced with the new data", () => {
        expect(generateNewGrid(newGridValues, rowTitles).newGridData[0][1].value).toEqual(123)
        expect(generateNewGrid(newGridValues, rowTitles).newGridData[2][2].value).toEqual(192021)
    })

    test("Overflowed data is ignored (i.e. there cant be more rows than row titles)", () => {
        expect(newGridValues.length).toBeGreaterThan(rowTitles.length)
        expect(generateNewGrid(newGridValues, rowTitles).newGridData.length).toEqual(rowTitles.length)
    })

    test("If the new data is empty, an array of only the row titles is returned", () => {
        expect(generateNewGrid([], rowTitles).newGridData).toEqual(gridDataWithOnlyRowTitles)
        expect(generateNewGrid([], rowTitles).newColumns).toEqual([])
    })

    test("If the row titles are empty, an empty array is returned", () => {
        expect(generateNewGrid(newGridValues, []).newGridData).toEqual([])
    })
})
