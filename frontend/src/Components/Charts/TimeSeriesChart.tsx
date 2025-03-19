import { AgCharts } from "ag-charts-react"
import { insertIf, separateProfileObjects } from "../../Utils/AgGridUtils"

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

export const setValueToCorrespondingYear = (profile: any, year: number, dg4Year: number) => {
    if (profile && profile.values) {
        const profileStartYear: number = Number(profile.startYear) + dg4Year
        const valueYearIndex = year - profileStartYear
        return profile.values[valueYearIndex] ?? 0
    }
    return 0
}

export const TimeSeriesChart = ({
    data, chartTitle, barColors, barProfiles, barNames, unit, lineChart, axesData,
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
            <AgCharts
                options={defaultOptions}
            />
        </div>
    )
}
