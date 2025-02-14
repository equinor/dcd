import { AgCharts } from "ag-charts-react"
import { size } from "lodash"

interface Props {
    data: any
    chartTitle: string
    barColors: string[]
    unit?: string
    enableLegend?: boolean
}

export const AgChartsPie = ({
    data, chartTitle, barColors, unit, enableLegend,
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

    const defaultOptions: object = {
        data,
        title: { text: chartTitle ?? "" },
        subtitle: { text: unit ?? "" },
        padding: {
            top: 10,
            right: 10,
            bottom: 10,
            left: 10,
        },
        theme: figmaTheme,
        series: [
            {
                type: "pie",
                angleKey: "value",
                calloutLabelKey: "value",
                sectorLabelKey: "profile",
                strokes: ["white"],
                sectorLabel: {
                    enabled: false,
                },
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
        legend: {
            legendItemKey: "profile",
            enabled: enableLegend,
            position: "bottom",
            spacing: 40,
            label: {
                fontSize: 14,
            },
        },
    }

    return (
        <div>
            <AgCharts
                options={defaultOptions}
            />
        </div>
    )
}
