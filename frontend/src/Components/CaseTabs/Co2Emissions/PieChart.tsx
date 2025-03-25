import { AgCharts } from "ag-charts-react"

import { formatChartNumber } from "@/Utils/FormatingUtils"

interface Props {
    data: any
    chartTitle: string
    barColors: string[]
    unit?: string
    enableLegend?: boolean
}

export const PieChart = ({
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
                calloutLabel: {
                    formatter: (params: any) => {
                        if (params.value === null || params.value === undefined) {
                            return "0"
                        }

                        const value = typeof params.value === "number" ? params.value : parseFloat(params.value)

                        if (Number.isNaN(value)) {
                            return "0"
                        }

                        // For very small decimal values (typical for CO2 intensity)
                        if (Math.abs(value) < 1) {
                            return value.toFixed(4)
                        }

                        // For small whole numbers (1-9)
                        if (Math.abs(value) < 10 && Number.isInteger(value)) {
                            return value.toString()
                        }

                        // Use the standard formatter for all other values
                        return formatChartNumber(value)
                    },
                },
                // Do not use formatter here as it causes issues with pie segments
                // when values are small decimals
            },
        ],
        tooltip: {
            renderer: (params: any) => {
                if (!params.datum || params.datum.value === null || params.datum.value === undefined) {
                    return { content: "0" }
                }

                const value = typeof params.datum.value === "number"
                    ? params.datum.value
                    : parseFloat(params.datum.value)

                if (Number.isNaN(value)) {
                    return { content: "0" }
                }

                let formattedValue

                // For very small decimal values
                if (Math.abs(value) < 1) {
                    formattedValue = value.toFixed(4)
                } else if (Math.abs(value) < 10 && Number.isInteger(value)) {
                    // For small whole numbers
                    formattedValue = value.toString()
                } else {
                    // For other values
                    formattedValue = formatChartNumber(value)
                }

                return {
                    content: `${params.datum.profile || params.title}: ${formattedValue}`,
                }
            },
        },
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
