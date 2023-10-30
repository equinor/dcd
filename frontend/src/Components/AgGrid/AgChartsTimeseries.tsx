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
    height?: string
    axesData?: any
}

export const setValueToCorrespondingYear = (profile: any, i: number, startYear: number, dg4Year: number) => {
    if (profile !== undefined && startYear !== undefined) {
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
            ...separateProfileObjects(barProfiles, barNames, "year"),
            ...insertIf(lineChart !== undefined, false, axesData, lineChart),
        ],
        ...insertIf(axesData !== undefined, true, axesData, lineChart),
        legend: { position: "bottom", spacing: 40 },
    }

    return (
        <div style={{ height: height ?? 400, width }}>
            <AgChartsReact
                options={defaultOptions}
            />
        </div>
    )
}
