import { AgChartsReact } from "ag-charts-react"

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
    data, chartTitle, barColors, barProfiles, barNames, unit, lineChart, width, height,
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

    function insertIf(condition: any, ...elements: any) {
        return condition ? elements : []
    }

    const defaultOptions = {
        data,
        title: { text: chartTitle },
        subtitle: { text: unit },
        padding: {
            top: 40,
            right: 40,
            bottom: 40,
            left: 40,
        },
        theme: figmaTheme,
        series: [
            {
                type: "column",
                xKey: "year",
                yKeys: barProfiles,
                yNames: barNames,
                grouped: true,
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
            },
            ...insertIf(lineChart !== undefined, lineChart),
        ],
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
