export const separateProfileObjects = (barProfiles: string[], barNames: string[], xKey: string) => {
export const separateProfileObjects = (barProfiles: string[], barNames: string[], xKey: string) => {
    const barProfileObjects = barProfiles.map((bp, i) => ({
        type: "column",
        xKey,
        yKey: bp,
        yName: barNames[i],
        grouped: true,
        highlightStyle: {
            item: {
                fill: undefined,
                stroke: undefined,
                strokeWidth: 1,
            },
            series: {
                enabled: true,
                dimOpacity: 0.2,
                strokeWidth: 2,
            },
        },
    }))
    return barProfileObjects
}

export const insertIf = (condition: boolean, addAxes: boolean, axesData: any, ...elements: any) => {
    if (addAxes) {
        return condition ? { axes: axesData } : []
    }
    return condition ? elements : []
}
