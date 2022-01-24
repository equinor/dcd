export const tsvToJson = (tsvText: string) => {
    //Split all the text into separate lines on new lines and carriage return feeds
    const allLines = tsvText.split(/\r\n|\n/)
    let headerIndex = 0

    //Split per line on tabs and commas
    const headers = allLines[headerIndex].split(/\t/)
    const lines = []

    for (let i = headerIndex + 1; i < allLines.length; i++) {
        const data = allLines[i].split(/\t/)

        if (data.length === headers.length) {
            const row = {} as { [key: string]: string }
            for (let j = 0; j < headers.length; j++) {
                row[headers[j].toLowerCase()] = data[j].replace(',', '.')
            }
            lines.push(row)
        }
    }
    return lines
}
