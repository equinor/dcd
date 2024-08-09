import { AgChartsReact } from "ag-charts-react"
import { insertIf, separateProfileObjects } from "./AgChartHelperFunctions"

interface Props {
    data: object
    chartTitle: string
    barColors: string[]
    barProfiles: string[]
    barNames: string[]
    unit?: string
    lineChart?: object
    width?: string
    height?: string
    axesData?: any
}

export const setValueToCorrespondingYear = (profile: any, i: number, startYear: number, dg4Year: number) => {
    if (profile && startYear !== undefined && profile.values) {
        const profileStartYear: number = Number(profile.startYear) + dg4Year
        const profileYears: number[] = Array.from(
            { length: profile.values.length },
            ((v, x: number) => x + profileStartYear),
        )
        const valueYearIndex = profileYears.findIndex((x) => x === i)
        return profile.values[valueYearIndex] ?? 0
    }
    return 0
}

export const AgChartsTimeseries = ({
    data, chartTitle, barColors, barProfiles, barNames, unit, lineChart, width, height, axesData,
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
        },
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
            ...insertIf(lineChart !== undefined, false, axesData, lineChart),
        ],
        ...insertIf(axesData !== undefined, true, axesData, lineChart),
        legend: { position: "bottom", spacing: 40 },
    }

    return (
        <div>
            <AgChartsReact
                options={defaultOptions}
            />
        </div>
    )
}
