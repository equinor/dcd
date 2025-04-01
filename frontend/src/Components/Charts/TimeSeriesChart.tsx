import { AgCharts } from "ag-charts-react"

import { formatNumberForView } from "@/Utils/FormatingUtils"
import { separateProfileObjects } from "@/Utils/TableUtils"

interface Props {
    data: object
    chartTitle: string
    barColors: string[]
    barProfiles: string[]
    barNames: string[]
    unit?: string
    lineChart?: object
    axesData?: any
}

export const setValueToCorrespondingYear = (profile: any, year: number, dg4Year: number): number => {
    if (profile && profile.values) {
        const profileStartYear: number = Number(profile.startYear) + dg4Year
        const valueYearIndex = year - profileStartYear

        return profile.values[valueYearIndex] ?? 0
    }

    return 0
}

export const TimeSeriesChart = ({
    data, chartTitle, barColors, barProfiles, barNames, unit, lineChart, axesData,
}: Props): React.ReactNode => {
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
        },
    }

    const tooltipRenderer = (params: any) => ({
        content: `${params.title}: ${formatNumberForView(params.yValue)}`,
    })

    const defaultAxes = [
        {
            type: "category",
            position: "bottom",
            nice: true,
        },
        {
            type: "number",
            position: "left",
            nice: true,
            label: {
                formatter: (params: any) => formatNumberForView(params.value),
            },
        },
    ]

    const defaultOptions = {
        data,
        title: { text: chartTitle ?? "" },
        subtitle: { text: unit ?? "" },
        height: 400,
        padding: {
            top: 10,
            right: 10,
            bottom: 10,
            left: 10,
        },
        theme: figmaTheme,
        series: [
            ...separateProfileObjects(barProfiles, barNames, "year"),
            ...(lineChart ? [lineChart] : []),
        ],
        tooltip: {
            renderer: tooltipRenderer,
        },
        axes: axesData || defaultAxes,
        legend: { position: "bottom", spacing: 40 },
    } as any

    return (
        <div>
            <AgCharts
                options={defaultOptions}
            />
        </div>
    )
}
