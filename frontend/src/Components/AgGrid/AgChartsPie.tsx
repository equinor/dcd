import { AgChartsReact } from "ag-charts-react"

interface Props {
    data: any
    chartTitle: string
    barColors: string[]
    unit?: string
    width?: string
    height: number
    enableLegend?: boolean
    totalCo2Emission: string | undefined
}

export const AgChartsPie = ({
    data, chartTitle, barColors, unit, width, height, enableLegend, totalCo2Emission,
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

    const defaultOptions = {
        data,
        title: { text: chartTitle },
        subtitle: { text: unit },
        padding: {
            top: 40,
            right: 40,
            bottom: 40,
            left: 40,
        },
        theme: figmaTheme,
        series: [
            {
                type: "pie",
                calloutLabelKey: "profile",
                angleKey: "value",
                calloutLabel: { enabled: false },
                innerRadiusOffset: -25,
                strokes: ["white"],
                innerLabels: [
                    {
                        text: totalCo2Emission ?? "0",
                        fontSize: 42,
                    },
                    {
                        text: "million tonnes",
                        fontSize: 14,
                        margin: 4,
                        color: "#B4B4B4",
                    },
                ],
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
