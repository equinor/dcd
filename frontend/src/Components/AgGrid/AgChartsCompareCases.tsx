import { AgCharts } from "ag-charts-react"
import { insertIf, separateProfileObjects } from "./AgChartHelperFunctions"

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

export const AgChartsCompareCases = ({
    data, chartTitle, barColors, barProfiles, barNames, unit, lineChart, enableLegend,
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
                            rotation: -30,
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
            },
            {
                type: "number",
                position: "left",
                nice: true,
            },
        ],
        theme: figmaTheme,
        series: [
            ...separateProfileObjects(barProfiles, barNames, "cases"),
            ...insertIf(lineChart !== undefined, false, lineChart),
        ],
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
