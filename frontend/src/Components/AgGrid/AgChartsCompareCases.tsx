import { AgChartsReact } from "ag-charts-react"
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
            ...separateProfileObjects(barProfiles, barNames, "cases"),
            ...insertIf(lineChart !== undefined, false, lineChart),
        ],
        legend: { enabled: enableLegend, position: "bottom", spacing: 40 },
    }

    return (
        <div>
            <AgChartsReact
                options={defaultOptions}
            />
        </div>
    )
}
