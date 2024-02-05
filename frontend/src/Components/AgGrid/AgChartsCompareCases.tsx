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

    const defaultOptions: any = {
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
            ...separateProfileObjects(barProfiles, barNames, "cases"),
            ...insertIf(lineChart !== undefined, false, lineChart),
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
