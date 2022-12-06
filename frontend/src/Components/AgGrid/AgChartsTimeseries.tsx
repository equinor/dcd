import React, { Component, useEffect, useState } from "react"
import { render } from "react-dom"
import * as agCharts from "ag-charts-community"
import { AgChartsReact } from "ag-charts-react"

interface Props {
    data: any
    chartTitle: string
    barColors: string[]
    barProfiles: string[]
    barNames: string[]
    unit: string
    lineChart?: object
}

export const AgChartsTimeseries = ({
    data, chartTitle, barColors, barProfiles, barNames, unit, lineChart,
}: Props) => {
    const figmaTheme = {
        baseTheme: "ag-default",
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
            },
            ...insertIf(lineChart !== undefined, lineChart),
        ],
        legend: { position: "bottom", spacing: 40 },
    }

    return (
        <div style={{ height: 400 }}>
            <AgChartsReact
                options={defaultOptions}
            />
        </div>
    )
}
