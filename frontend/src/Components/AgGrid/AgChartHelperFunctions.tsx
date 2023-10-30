export function separateProfileObjects(barProfiles: string[], barNames: string[], xKey: string) {
    const barProfileObjects: object[] = []
    for (let i = 0; i < barProfiles.length; i += 1) {
        barProfileObjects.push({
            type: "column",
            xKey,
            yKey: barProfiles[i],
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
        })
    }
    return barProfileObjects
}

export function insertIf(condition: any, addAxes: boolean, axesData: any, ...elements: any) {
    if (addAxes) {
        const axesObject = { axes: axesData }
        return condition ? axesObject : []
    }
    return condition ? elements : []
}
