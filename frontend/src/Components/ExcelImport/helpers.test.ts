import { tsvToJson } from './helpers'

describe('tsvToJson', () => {
    const tsvText = '2022\t2023\n' + '123\t222324\r\n' + '456\t252627\n' + '789\t282930'
    const poorlyFormattedTsvText = '2022\t2023\n' + '123\t222324\t222324\r\n' + '456\t252627\t122324\n' + '789\t282930'
    const tsvTextWithCommas = '2022\t2023\n' + '123\t222,324\r\n' + '456\t25,2627\n' + '789\t28.2930'
    const tsvWithCapitalLetters = 'Hello\tTest\n' + '123\t222324\r\n' + '456\t252627\n' + '789\t282930'

    const returnedJson = [
        { 2022: '123', 2023: '222324' },
        { 2022: '456', 2023: '252627' },
        { 2022: '789', 2023: '282930' },
    ]

    const returnedJsonWhenPoorlyFormatted = [{ 2022: '789', 2023: '282930' }]

    const returnedJsonWhenCommas = [
        { 2022: '123', 2023: '222.324' },
        { 2022: '456', 2023: '25.2627' },
        { 2022: '789', 2023: '28.2930' },
    ]

    const returnedJsonWhenCapitalLetters = [
        { hello: '123', test: '222324' },
        { hello: '456', test: '252627' },
        { hello: '789', test: '282930' },
    ]

    test('The data is split into rows by new lines and carriage return feeds and into columns by tabs and commas', () => {
        expect(tsvToJson(tsvText)).toEqual(returnedJson)
    })

    test('If the column headers (the values before the first newline) are fewer than the columns of data in some lines, an array of only the lines with correct amount of columns is returned', () => {
        expect(tsvToJson(poorlyFormattedTsvText)).toEqual(returnedJsonWhenPoorlyFormatted)
    })

    test('Commas in the form of "," is replaced with "."', () => {
        expect(tsvToJson(tsvTextWithCommas)).toEqual(returnedJsonWhenCommas)
    })

    test('Headers with capital letters is replaced with lower case letters', () => {
        expect(tsvToJson(tsvWithCapitalLetters)).toEqual(returnedJsonWhenCapitalLetters)
    })
})
