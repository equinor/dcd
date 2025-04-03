import { AgCharts } from "ag-charts-react"

import { formatNumberForView, roundToDecimals } from "@/Utils/FormatingUtils"
import { insertIf, separateProfileObjects } from "@/Utils/TableUtils"

interface Props {
    data: any
    chartTitle: string
    barColors: string[]
    barProfiles: string[]
    barNames: string[]
    unit?: string
    lineChart?: object
    enableLegend?: boolean
}

export const CompareCasesChart = ({
    data,
    chartTitle,
    barColors,
    barProfiles,
    barNames,
    unit,
    lineChart,
    enableLegend,
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
            bar: {
                axes: {
                    category: {
                        label: {
                            rotation: -20,
                            formatter(params: any) {
                                if (params.value.length > 18) {
                                    return `${params.value.substr(0, 16)}...`
                                }

                                return params.value
                            },
                        },
                    },
                },
            },
        },
    }

    const defaultOptions: object = {
        data,
        title: { text: chartTitle ?? "" },
        subtitle: { text: unit ?? "" },
        padding: {
            top: 0,
            right: 10,
            bottom: 0,
            left: 10,
        },
        axes: [
            {
                type: "category",
                position: "bottom",
                nice: true,
                gridLine: {
                    style: [
                        {
                            stroke: "rgba(0, 0, 0, 0.2)",
                            lineDash: [3, 2],
                        },
                        {
                            stroke: "rgba(0, 0, 0, 0.2)",
                            lineDash: [3, 2],
                        },
                    ],
                },
            },
            {
                type: "number",
                position: "left",
                nice: true,
                label: {
                    formatter: (params: any) => formatNumberForView(roundToDecimals(params.value, 4)),
                },
                gridLine: {
                    style: [
                        {
                            stroke: "rgba(0, 0, 0, 0.1)",
                            lineDash: [3, 2],
                        },
                        {
                            stroke: "rgba(0, 0, 0, 0.1)",
                            lineDash: [3, 2],
                        },
                    ],
                },
            },
        ],
        theme: figmaTheme,
        series: [
            ...separateProfileObjects(barProfiles, barNames, "cases"),
            ...insertIf(lineChart !== undefined, false, lineChart),
        ],
        tooltip: {
            renderer: (params: any) => ({
                content: `${params.title}: ${formatNumberForView(roundToDecimals(params.yValue, 4))}`,
            }),
        },
        legend: { enabled: enableLegend, position: "bottom", spacing: 40 },
    }

    return (
        <div>
            <AgCharts
                options={defaultOptions}
            />
        </div>
    )
}
