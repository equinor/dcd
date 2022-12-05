import React, { Component, useEffect, useState } from "react"
import { render } from "react-dom"
import * as agCharts from "ag-charts-community"
import { AgChartsReact } from "ag-charts-react"

interface Props {
    data: any
    barColors?: string[]
    barNames?: string[]
    unit?: string
}

export const AgChartsTimeseries = ({
    data, barColors, barNames, unit,
}: Props) => {
    const [options, setOptions] = useState<any>()
    // const chartThemes = useMemo<string[]>(() => ["ag-vivid"], [])

    const figmaTheme = {
        baseTheme: "ag-default",
        palette: {
            fills: [
                "#243746",
                "#EB0037",
                "#A8CED1",
            ],
            strokes: ["black"],
        },
        overrides: {
            cartesian: {
                title: {
                    fontSize: 24,
                },
                // series: {
                //     column: {
                //         label: {
                //             enabled: true,
                //             color: "black",
                //         },
                //     },
                // },
            },
        },
    }

    const dummyData = [
        {
            year: "2020",
            oilProduction: 450,
            gasProduction: 560,
            waterProduction: 600,
        },
        {
            year: "2021",
            oilProduction: 550,
            gasProduction: 660,
            waterProduction: 800,
        },
        {
            year: "2022",
            oilProduction: 850,
            gasProduction: 560,
            waterProduction: 200,
        },
    ]

    const defaultOptions = {
        data,
        title: { text: "Production profiles" },
        subtitle: { text: "MSm3         GSm3         MSm3" }, // unit
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
                yKeys: ["oilProduction", "gasProduction", "waterProduction"],
                yNames: ["Oil production", "Gas production", "Water production"],
                grouped: true,
            },
            // {
            //     xKey: "year", yKey: "Gas production", stacked: false,
            // },
            // {
            //     xKey: "year", yKey: "Water production", stacked: false,
            // },
        ],
        legend: { position: "bottom", spacing: 40 },
    }

    useEffect(() => {
        setOptions(defaultOptions)
    }, [])

    return (
        <div style={{ height: 400 }}>
            <AgChartsReact
                options={defaultOptions}
            />
        </div>
    )
}
