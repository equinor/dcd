import { AgChartsReact } from "ag-charts-react"

interface Props {
    data: any
    chartTitle: string
    barColors: string[]
    barProfiles: string[]
    barNames: string[]
    unit?: string
    lineChart?: object
    width?: string
    height: number
    enableLegend?: boolean
}

export const AgChartsCompareCases = ({
    data, chartTitle, barColors, barProfiles, barNames, unit, lineChart, width, height, enableLegend,
}: Props) => {
    const figmaTheme = {
        palette: {
            fills: barColors,
            strokes: ["black"],
        },
        overrides: {
            cartesian: {
                title: {
                    fontSize: 24,
                },
            },
            column: { axes: { category: { label: { rotation: -20 } } } },
        },
    }

    function insertIf(condition: any, ...elements: any) {
        return condition ? elements : []
    }

    const defaultOptions = {
        data,
        title: { text: chartTitle ?? "" },
        subtitle: { text: unit ?? "" },
        padding: {
            top: 40,
            right: 40,
            bottom: 40,
            left: 40,
        },
        theme: figmaTheme,
        series: [
            {
                type: "column",
                xKey: "cases",
                yKeys: barProfiles,
                yNames: barNames,
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
            },
            ...insertIf(lineChart !== undefined, lineChart),
        ],
        legend: { enabled: enableLegend, position: "bottom", spacing: 40 },
    }

    return (
        <div style={{ height, width }}>
            <AgChartsReact
                options={defaultOptions}
            />
        </div>
    )
}
