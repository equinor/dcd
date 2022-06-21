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

const COLORS = ["#087CB8", "#9b59b6", "#e74c3c", "#e74c3c", "#e74c3c", "#e74c3c", "#e74c3c"]

interface Props {
    x: number[]
    y: number[][]
    caseTitles: string[]
}

const yearLabels = (x:number[], y:number[][]) :string[] => {
    const start = new Date().getFullYear()
    const start2 = min(x.filter((n) => n !== null || n !== 0)) as number
    const totalstart = start + start2
    const add = maxBy(y, (xx) => xx.length)
    const end = totalstart + add!.length

    const yearArr = range(totalstart, end)
    return yearArr.map((yy) => yy.toString())
}

console.log("hehe")

const chartData: ChartData = {
    datasets: [

    ],
}

const myFirstDataset = {
    label: "My Second dataset",
    fillColor: "rgba(187,205,151,0.5)",
    strokeColor: "rgba(187,205,151,0.8)",
    highlightFill: "rgba(187,205,151,0.75)",
    highlightStroke: "rgba(187,205,151,1)",
    data: [48, 40, 19, 86, 27, 90, 28],
}

const mySecondDataset = {
    label: "My Second dataset",
    fillColor: "rgba(187,205,151,0.5)",
    strokeColor: "rgba(187,205,151,0.8)",
    highlightFill: "rgba(187,205,151,0.75)",
    highlightStroke: "rgba(187,205,151,1)",
    data: [148, 240, 319, 486, 527, 690, 728],
}

const generateChartDatas = (values: number[][], caseTitles: string[]) => {
    const allObjects: any[] = []

    for (let index = 0; index < values.length; index++) {
        const element = {
            label: caseTitles[index],
            fillColor: COLORS[index],
            strokeColor: COLORS[index],
            highlightFill: COLORS[index],
            highlightStroke: COLORS[index],
            backgroundColor: COLORS[index],
            borderColor: COLORS[index],
            color: COLORS[index],
            data: values[index],
        }
        allObjects.push(element)
    }
    return allObjects
}

const ManniDataTable = ({
    x, y, caseTitles,
}: Props) => {
    chartData.labels = yearLabels(x!, y)
    const asd = generateChartDatas(y, caseTitles)
    chartData.datasets = asd
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
                text: "CapEx",
            },
        },
        xAxis: {
            offset: true,
            title: {
                display: true,
                text: "Year",
            },
        },
    },
})
