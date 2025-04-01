import { AgChartOptions, AgCharts } from "ag-charts-community"
import { useEffect, useRef } from "react"

interface Props {
    data: any[]
    chartTitle: string
    barColors: string[]
    unit?: string
    enableLegend?: boolean
}

export const PieChart = ({
    data,
    chartTitle,
    barColors,
    unit = "",
    enableLegend = true,
}: Props) => {
    const chartRef = useRef<HTMLDivElement>(null)

    const processedData = data.map((item) => ({
        category: item.profile,
        value: typeof item.value === "number" ? item.value : Number(item.value || 0),
    }))

    const totalValue = processedData.reduce((sum, item) => sum + item.value, 0)
    const chartData = totalValue > 0 ? processedData : [
        { category: "Drilling", value: 33 },
        { category: "Flaring", value: 33 },
        { category: "Fuel", value: 34 },
    ]

    useEffect(() => {
        if (!chartRef.current) { return }

        chartRef.current.innerHTML = ""

        const options: AgChartOptions = {
            container: chartRef.current,
            data: chartData,
            title: {
                text: chartTitle,
            },
            subtitle: {
                text: unit,
            },
            series: [{
                type: "pie",
                angleKey: "value",
                calloutLabelKey: "category",
                sectorLabelKey: "value",
                fills: barColors,
            }],
            legend: {
                enabled: enableLegend,
            },
        }

        AgCharts.create(options)
    }, [chartData, chartTitle, barColors, unit, enableLegend])

    return (
        <div ref={chartRef} style={{ width: "100%", height: "400px" }} />
    )
}
