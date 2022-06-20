/* eslint-disable no-plusplus */
/* eslint-disable import/no-extraneous-dependencies */
/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable no-empty-pattern */
/* eslint-disable @typescript-eslint/no-use-before-define */
/* eslint-disable react/no-unused-prop-types */
import { Chart as ReactChart } from "react-chartjs-2"
import {
    Chart as ChartJS, ChartData, ChartDataset, ChartOptions, registerables,
} from "chart.js"
import { min, maxBy, range } from "lodash"

ChartJS.register(...registerables)

interface Props {
    x: number[]
}

const yearLabels = (x:number[], y:number[][]) :string[] => {
    const start = new Date().getFullYear()
    const start2 = min(x.filter((n) => n !== 0)) as number
    const totalstart = start + start2
    const add = maxBy(y, (xx) => xx.length)
    const end = totalstart + add!.length

    const yearArr = range(start, end)
    return yearArr.map((yy) => yy.toString())
}
const yrArr = yearLabels([8, 11, 15, 20], [[1, 2, 3, 4], [10, 20, 30, 40, 50]])
console.log(yrArr)

const chartData: ChartData = {
    datasets: [
        {
            data: [1, 2, 3],
        },
        {
            data: [2, 3, 4],
        },
    ],
}

const ManniDataTable = ({
    x,
}: Props) => {
    chartData.labels = yearLabels(x, [][2])

    return (
        <ReactChart
            type="line"
            options={chartoptions("Compare cases")}
            data={chartData}
        />
    )
}

export default ManniDataTable

export const chartoptions = (title?: string): ChartOptions => ({
    maintainAspectRatio: true,
    responsive: true,
    plugins: {
        // @ts-ignore

        tooltip: {
            backgroundColor: "yellow",
            titleColor: "yellow",
            bodyColor: "yellow",
        },
        title: {
            text: title,
            display: true,
            align: "start",
            color: "black",
            font: {
                size: 16,
                family: "Equinor",
                weight: "bolder",
            },
        },
        legend: {
            display: true,
            labels: {
                usePointStyle: true,
            },
        },
    },
    scales: {
        y: {
            beginAtZero: true,
            position: "left",
            type: "linear",
            title: {
                display: true,
                text: "# of SWCRs",
            },
        },

        xAxis: {
            offset: true,
        },
    },
})
