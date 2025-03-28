import { AgCharts } from "ag-charts-react"

import { insertIf, separateProfileObjects } from "@/Utils/TableUtils"

interface Props {
    data: object
    chartTitle: string
    barColors: string[]
    barProfiles: string[]
    barNames: string[]
    unit?: string
    lineChart?: object | object[]
    axesData?: any
    crossLines?: any[]
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
    data, chartTitle, barColors, barProfiles, barNames, unit, lineChart, axesData, crossLines,
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

    // Handle both single lineChart object and array of lineChart objects
    let lineChartSeries: object[] = []

    if (lineChart) {
        if (Array.isArray(lineChart)) {
            lineChartSeries = lineChart
        } else {
            lineChartSeries = [lineChart]
        }
    }

    const defaultOptions: object = {
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
            ...lineChartSeries,
        ],
        ...insertIf(axesData !== undefined, true, axesData, lineChart),
        ...(crossLines ? { crossLines } : {}),
        legend: { position: "bottom", spacing: 40 },
    }

    return (
        <div>
            <AgCharts
                options={defaultOptions}
            />
        </div>
    )
}
